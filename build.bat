@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.0.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

%nuget% restore AesCompress.sln

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild AesCompress.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false

dir AesCompress\bin\Release

%nuget% pack "AesCompress.nuspec" -NoPackageAnalysis -verbosity detailed -Version %version% -p Configuration="%config%"
