call "C:\Program Files\Microsoft Visual Studio .NET\Common7\Tools\vsvars32.bat"
devenv "blob.sln" /rebuild Debug /useenv
devenv "blob.sln" /rebuild Release /useenv
pause
exit