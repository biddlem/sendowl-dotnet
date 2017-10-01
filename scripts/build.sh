#!/usr/bin/env bash
#exit if any command fails
set -e
dotnet restore && dotnet test src/Sendowl.Test 