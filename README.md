KLoggy
======

[![Build status](https://ci.appveyor.com/api/projects/status/vicnydhrabh3vpru/branch/master?svg=true)](https://ci.appveyor.com/project/tugberkugurlu/kloggy/branch/master)

New version of Bloggy which is written on KRuntime

## Get it up
First, run the following commands to get everything ready (under `src/Kloggy.Web/` dir):
    
    // restore ASP.NET vNext packages
    kpm restore
    
    // restore bower packages
    bower install
    
    // restore npm packages
    npm install
    
    // run the gulp tasks
    gulp
    
After that, everything will be ready to be hit. You should now be able to run `k web` command to get the stuff up and running.