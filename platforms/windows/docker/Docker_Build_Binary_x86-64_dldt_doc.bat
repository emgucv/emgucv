REM go to the folder of the current script
pushd %~p0
cd ..
docker run -v %cd%\..\..:c:\bb\cv_x86-64\build -w c:\bb\cv_x86-64\build\platforms\windows emgu/vs2019_buildtools_cuda_openvino:latest .\Build_Binary_x86.bat 64 no-gpu inf no-openni doc package build
