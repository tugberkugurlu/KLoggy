var gulp = require('gulp'),
    minifycss = require('gulp-minify-css'),
    uglify = require('gulp-uglify'),
    concat = require('gulp-concat'),
    less = require('gulp-less'),
    del = require('del');

gulp.task('clean', function(cb) {
    del(['assets/css', 'assets/js', 'assets/less', 'assets/img', 'assets/fonts'], cb)
});

gulp.task('default', ['clean'], function() {
    gulp.start('scripts', 'fonts', 'styles', 'less');
});

gulp.task('fonts', function() {
    
   return gulp.src(['bower_components/bootstrap/dist/fonts/*', 'bower_components/fontawesome/fonts/*'])
              .pipe(gulp.dest('assets/fonts'));
});

gulp.task('scripts', function() {
    
   return gulp.src([
        'bower_components/jquery/dist/*',
        'bower_components/bootstrap/dist/js/*',
        'bower_components/angular/*.js', 
        'bower_components/angular/*.map',
        'bower_components/underscore/*.js',
        'bower_components/underscore/*.map',
       
        'client/js/*'
       ]
   ).pipe(gulp.dest('assets/js'));
});

gulp.task('styles', function() {
    
   return gulp.src('bower_components/fontawesome/css/font-awesome.css')
              .pipe(gulp.dest('assets/css'));
});


gulp.task('less', function() {
    
    gulp.src('client/less/bootstrap.less')
        .pipe(less())
        .pipe(gulp.dest('assets/css'));
    
    gulp.src('client/less/main.less')
        .pipe(less())
        .pipe(gulp.dest('assets/css'));
});