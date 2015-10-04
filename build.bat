@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

REM %nuget% restore AesCompress.sln

REM Build
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild AesCompress.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM Package
mkdir Build
call %nuget% pack "AesCompress\AesCompress.csproj" -symbols -o Build -p Configuration=%config% %version%
