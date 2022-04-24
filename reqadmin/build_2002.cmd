call "C:\Program Files\Microsoft Visual Studio .NET\Common7\Tools\vsvars32.bat"
del "bin\Debug\reqadmin.exe"
del "bin\Release\reqadmin.exe"
devenv "reqadmin.sln" /rebuild Debug /useenv
devenv "reqadmin.sln" /rebuild Release /useenv
"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\mt.exe" -manifest "%~dp0app.requireAdministrator.manifest" -outputresource:"%~dp0bin\Debug\reqadmin.exe";#1
"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\mt.exe" -manifest "%~dp0app.requireAdministrator.manifest" -outputresource:"%~dp0bin\Release\reqadmin.exe";#1
pause
exit