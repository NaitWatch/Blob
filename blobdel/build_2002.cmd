call "C:\Program Files\Microsoft Visual Studio .NET\Common7\Tools\vsvars32.bat"
del "bin\Debug\blobdel.exe"
del "bin\Release\blobdel.exe"
devenv "blobdel.sln" /rebuild Debug /useenv
devenv "blobdel.sln" /rebuild Release /useenv
"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\mt.exe" -manifest "%~dp0app.asInvoker.manifest" -outputresource:"%~dp0bin\Debug\blobdel.exe";#1
"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\mt.exe" -manifest "%~dp0app.asInvoker.manifest" -outputresource:"%~dp0bin\Release\blobdel.exe";#1
pause
exit