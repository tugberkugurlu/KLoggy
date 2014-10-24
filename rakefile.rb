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

CLEAN.include(ARTIFACTSROOT)
CLEAN.include(File.join(TESTROOT, "**", "**", "bin"))
CLEAN.include(File.join(SRCROOT, "**", "**", "bin"))
CLEAN.include(File.join(SRCROOT, "**", "**", "node_modules"))
CLEAN.include(File.join(SRCROOT, "**", "**", "bower_components"))

task:default => [:clean] do
end