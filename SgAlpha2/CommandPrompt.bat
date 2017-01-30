@CD /D "%~dp0"
@title SgAlpha2 Command Prompt
@SET PATH=C:\Program Files (x86)\MSBuild\14.0\Bin\;%PATH%
@doskey b=msbuild $* SgAlpha2.proj
type readme.txt
%comspec%
