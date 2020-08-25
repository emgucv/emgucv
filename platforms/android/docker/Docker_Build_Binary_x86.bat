REM go to the folder of the current script
pushd %~p0
cd ..
docker run -v %cd%\..\..:c:\bb\cv_android_x86\build -w c:\bb\cv_android_x86\build\ emgu/vs2019_buildtools_cuda:latest platforms\android\scripts\build.cmd x86
