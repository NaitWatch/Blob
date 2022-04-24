call "C:\Program Files\Microsoft Visual Studio .NET\Common7\Tools\vsvars32.bat"
SET SOLUTION=blobstub.sln
SET OUTPUTFINA=blobstub
SET OUTPUTTYPE=.exe
SET OUTPUT=%OUTPUTFINA%%OUTPUTTYPE%
SET OUTPUTI=%OUTPUTFINA%i%OUTPUTTYPE%
SET OUTPUTA=%OUTPUTFINA%a%OUTPUTTYPE%

devenv "%SOLUTION%" /rebuild Debug /useenv
devenv "%SOLUTION%" /rebuild Release /useenv
"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\mt.exe" -manifest "%~dp0app.asInvoker.manifest" -outputresource:"%~dp0bin\Debug\%OUTPUT%";#1
"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\mt.exe" -manifest "%~dp0app.asInvoker.manifest" -outputresource:"%~dp0bin\Release\%OUTPUT%";#1
del "bin\Debug\%OUTPUTI%"
del "bin\Release\%OUTPUTI%"
ren "bin\Debug\%OUTPUT%" "%OUTPUTI%"
ren "bin\Release\%OUTPUT%" "%OUTPUTI%"

devenv "%SOLUTION%" /rebuild Debug /useenv
devenv "%SOLUTION%" /rebuild Release /useenv
"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\mt.exe" -manifest "%~dp0app.requireAdministrator.manifest" -outputresource:"%~dp0bin\Debug\%OUTPUT%";#1
"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\mt.exe" -manifest "%~dp0app.requireAdministrator.manifest" -outputresource:"%~dp0bin\Release\%OUTPUT%";#1
del "bin\Debug\%OUTPUTA%"
del "bin\Release\%OUTPUTA%"
ren "bin\Debug\%OUTPUT%" "%OUTPUTA%"
ren "bin\Release\%OUTPUT%" "%OUTPUTA%"

pause
exit