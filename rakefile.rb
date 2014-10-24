require 'os'
require 'json'
require 'rake'
require 'rake/clean'

SOLUTIONROOT = File.dirname(__FILE__)
SRCROOT = File.join(SOLUTIONROOT, "src")
TESTROOT = File.join(SOLUTIONROOT, "test")
ARTIFACTSROOT = File.join(SOLUTIONROOT, "artifacts")
ARTIFACTSBUILDROOT = File.join(ARTIFACTSROOT, "build")
ARTIFACTSTESTROOT = File.join(ARTIFACTSROOT, "test")
ARTIFACTSAPPSROOT = File.join(ARTIFACTSROOT, "apps")
PROJECTFILENAME = "project.json"
CONFIGURATION = 'Release'

APPPROJECTFILES = Dir[File.join(SRCROOT, "**", PROJECTFILENAME)]
TESTROJECTFILES = Dir[File.join(TESTROOT, "**", PROJECTFILENAME)]

CLEAN.include(ARTIFACTSROOT)
CLEAN.include(File.join(TESTROOT, "**", "**", "bin"))
CLEAN.include(File.join(SRCROOT, "**", "**", "bin"))
CLEAN.include(File.join(SRCROOT, "**", "**", "node_modules"))
CLEAN.include(File.join(SRCROOT, "**", "**", "bower_components"))

task:default => [:clean, :check, :npm, :bower, :gulp, :pack]

task:check do
    %w(npm bower kpm gulp).each do |cmd|
        begin sh "#{cmd} --version >> #{OS.dev_null}" rescue raise "#{cmd} doesn't exists globally" end
    end
end

task:npm do
    APPPROJECTFILES.each do |file|
        _projectDir = File.dirname(file)
        _packageFilePath = File.join(_projectDir, "package.json")
        if File.exists?(_packageFilePath)
            sh "npm install --prefix \"#{_projectDir}\""
        end
    end
end

task:bower do
    APPPROJECTFILES.each do |file|
        _projectDir = File.dirname(file)
        _bowerFilePath = File.join(_projectDir, "bower.json")
        if File.exists?(_bowerFilePath)
            # TODO: rely on .bowerrc file for now. config.cwd is so weird. ref: https://github.com/bower/bower/issues/1561
            # sh "bower install --config.cwd \"#{_bowerFilePath}\""
            sh "bower install"
        end
    end
end

task:gulp do
    APPPROJECTFILES.each do |file|
        _projectDir = File.dirname(file)
        _gulpFilePath = File.join(_projectDir, "gulpfile.js")
        if File.exists?(_gulpFilePath)
            sh "gulp --cwd \"#{_projectDir}\""
        end
    end
end

task:pack => [:build] do
    _packableProjectFiles = APPPROJECTFILES.select { |file|
        _fileBody = File.read(file)
        _project = JSON.parse _fileBody
        _project.has_key?("commands")
    }
    
    _packableProjectFiles.each do |file|
        # TODO: Should we get the project name from the project.json?
        # TODO: have a way of getting the CONFIGURATION as an argument
        
        _projectDir = File.dirname(file)
        _projectName = File.basename(_projectDir)
        _outputDir = File.join(ARTIFACTSAPPSROOT, _projectName)
        sh "kpm pack \"#{_projectDir}\" --configuration #{CONFIGURATION} --out \"#{_outputDir}\""
    end
end

task:build => [:restore] do
    APPPROJECTFILES.each do |file|
        sh "kpm build #{file} --configuration #{CONFIGURATION} --out \"#{ARTIFACTSBUILDROOT}\""
    end
    
    TESTROJECTFILES.each do |file|
        sh "kpm build #{file} --configuration #{CONFIGURATION} --out \"#{ARTIFACTSTESTROOT}\""
    end
end

task:restore do
    _projectFiles = APPPROJECTFILES + TESTROJECTFILES
    _projectFiles.each do |file|
        sh "kpm restore #{File.dirname(file)}"
    end
end