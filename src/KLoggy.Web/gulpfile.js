var gulp = require('gulp'),
    minifycss = require('gulp-minify-css'),
    uglify = require('gulp-uglify'),
    concat = require('gulp-concat'),
    less = require('gulp-less'),
    del = require('del'),
    rename = require('gulp-rename'),
    gitshasuffix = require('gulp-gitshasuffix'),
    gulpif = require('gulp-if'),
    order = require('gulp-order'),
    gutil = require('gulp-util'),
    ini = require('ini');

gulp.task('default', ['clean'], function() {
    gulp.start('fonts', 'scripts', 'styles', 'git-info');
});

gulp.task('clean', function(cb) {
    del(['assets/css', 'assets/js', 'assets/less', 'assets/img', 'assets/fonts'], cb)
});

gulp.task('fonts', function() {
    
    var fileList = [
        'bower_components/bootstrap/dist/fonts/*', 
        'bower_components/fontawesome/fonts/*'
    ];
    
    return gulp.src(fileList)
        .pipe(gulp.dest('assets/fonts'));
});

gulp.task('scripts', function() {
    
    var fileList = [
        'bower_components/jquery/dist/jquery.js',
        'bower_components/angular/angular.js',
        'bower_components/underscore/underscore.js',
        'bower_components/bootstrap/dist/js/bootstrap.js',

        'client/js/*'
    ];
    
    return gulp.src(fileList)
        .pipe(gulp.dest('assets/js'))
        .pipe(concat('script.js'))
        .pipe(gulp.dest('assets/js'))
        .pipe(rename({suffix: '.min'}))
        .pipe(gitshasuffix({ length: 40, separator: "-" }))
        .pipe(uglify())
        .pipe(gulp.dest('assets/js'));
});

gulp.task('styles', function() {
    
    var fileList = [
        'client/less/bootstrap.less',
        'client/less/main.less',
        'bower_components/fontawesome/css/font-awesome.css'
    ];
    
    return gulp.src(fileList)
        .pipe(gulpif(/[.]less$/, less()))
        .pipe(gulp.dest('assets/css'))
        .pipe(order(['**/bootstrap.css', '**/font-awesome.css', '**/main.css']))
        .pipe(concat('style.css'))
        .pipe(gulp.dest('assets/css'))
        .pipe(rename({suffix: '.min'}))
        .pipe(gitshasuffix({ length: 40, separator: "-" }))
        .pipe(minifycss())
        .pipe(gulp.dest('assets/css'));
});

gulp.task('git-info', function () {
    
    return getGitSha('./', function(error, output) {
        if(error) {
            console.error(output);
        } else {
            return writeToFile("git.ini", output)
                    .pipe(gulp.dest('./'));
        }
    });
});

// https://www.npmjs.org/package/gitsha
function getGitSha(path, callback) {
    var exec = require('child_process').exec;
    return exec('git rev-parse HEAD', {
      cwd: path
    }, function(error, stdout, stderr) {
      if (error) {
        return callback(error, stderr.trim());
      }
      return callback(null, stdout.trim());
    });
}

// https://github.com/isaacs/ini
// http://stackoverflow.com/questions/23230569/how-do-you-create-a-file-from-a-string-in-gulp
function writeToFile(filename, string) {
    var src = require('stream').Readable({ objectMode: true });
    var config = {
        sha: string
    };
    
    src._read = function () {
        this.push(new gutil.File({ cwd: "", base: "", path: filename, contents: new Buffer(ini.stringify(config, { section: 'git' })) }));
        this.push(null);
    }

    return src;
}