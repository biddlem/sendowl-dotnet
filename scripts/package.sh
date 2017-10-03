#!/usr/bin/env bash
#exit if any command fails
set -e
revision=${TRAVIS_JOB_ID:=1}  
revision=$(printf "%04d" $revision) 
dotnet pack src/Sendowl -c Release -o ../../nuget --version-suffix=$revision dotnet pack src/SendOwl -c Release -o ../../nuget --version-suffix=ci-$revision 