#!/usr/bin/env bash
set -e

declare -r BUILD_NUMBER=${TRAVIS_BUILD_NUMBER:=1}
declare -r REVISION=$(printf "%04d" $BUILD_NUMBER)
declare -r OUTPUT=../../artifacts

echo "building revision $REVISION"
dotnet restore src/SendOwl
dotnet restore src/SendOwl.Test

dotnet test src/SendOwl.Test

dotnet pack -c Release src/SendOwl -o $OUTPUT --version-suffix=ci-$REVISION
