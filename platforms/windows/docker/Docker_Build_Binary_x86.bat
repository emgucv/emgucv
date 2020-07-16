REM go to the folder of the current script
pushd %~p0

docker run -v %cd%\..\..\..:c:\bb\cv_x86\build -w c:\bb\cv_x86\build\platforms\windows\docker vs2019_buildtools .\Build_Binary_x86.bat
