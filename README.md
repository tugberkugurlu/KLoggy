KLoggy - Yet Another Blog Engine
======

[![Build status](https://ci.appveyor.com/api/projects/status/vicnydhrabh3vpru?svg=true)](https://ci.appveyor.com/project/tugberkugurlu/kloggy)

New version of [Bloggy](https://github.com/tugberkugurlu/BLoggy) which is written on KRuntime. BLoggy is a small blog engine I use for [my blog](http://www.tugberkugurlu.com) and it's working for my needs. However, the next generation, Cloud Optimized&#8482; ASP.NET platform is approaching and this project is sort of a test area for me.

## Requirements
To run this project you need to have following tools installed on your machine:

 * .NET Framework 4.5 or above.
 * node.js and npm
 * Latest KRE from dev branch.
 * bower CLI
 * gulp CLI

The application currenly doesn't run on any other platforms except for Windows.

## Running the Project
The project is currently have no functionality. However, you ca get the the site up and running to see a few things. First, run the following commands to get everything ready (under `src/Kloggy.Web/` dir):
    
    ## restore ASP.NET vNext packages
    kpm restore
    
    // restore bower packages
    bower install
    
    // restore npm packages
    npm install
    
    // run the gulp tasks
    gulp
    
After that, everything will be ready to be hit. You should now be able to run `k web` command to get the stuff up and running.

##Development Workflow
`master` branch only holds the latest stable version of the product. Navigate to `dev` branch in order to see latest work.

##Pull Requests &amp; Branching
Every feature must be developed under a so-called feature branch and that branch must be brached off from `dev` branch.

*Pull Requests* should be targeted to `dev` branch, not `master`! Before sending the PR, make sure you have the latest `dev` branch rebased into you feature branch.

#License and Copyright
This project licensed under MIT license.