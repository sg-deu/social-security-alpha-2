@CD /D "%~dp0"
@title SGAlpha2 Command Prompt
@SET PATH=C:\Program Files (x86)\MSBuild\14.0\Bin\;%PATH%
@doskey b=msbuild $* SgAlpha2.proj
@doskey bPackage=msbuild /p:Configuration=Release /p:DeployOnBuild=true /p:WebPublishMethod=Package /p:PackageAsSingleFile=true /p:SkipInvalidConfigurations=true /p:PackageLocation="%CD%\azurePackage" SgAlpha2.sln
type readme.txt
%comspec%
