
SG Alpha 2
==========

Scottish Government Alpha 2.

Building
========

To build, open CommandPrompt.bat, and type 'b'.

Build commands:

b = msbuild SgAlpha2.proj           : build and test locally
b /t:RestorePackages                : Restore NuGet packages
bPackage                            : packages the project for deployment to Azure
b /p:WithCoverage=true              : run local unit-test coverage report
