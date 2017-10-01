#!/usr/bin/env bash
#exit if any command fails
set -e
dotnet pack src/Sendowl -c Release -o ../../nuget --version-suffix=$revision 