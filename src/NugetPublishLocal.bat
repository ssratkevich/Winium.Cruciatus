REM delete existing nuget packages
del *.nupkg
set NUGET=.\.nuget\nuget.exe
set LocalSource="C:\workspace\nugetLocal"
rem %NUGET% pack .\Winium.Cruciatus\Winium.Cruciatus.csproj -IncludeReferencedProjects -Prop Configuration=Debug
%NUGET% push .\Winium.Cruciatus\bin\Debug\*.nupkg -Source %LocalSource%