REM go to the folder of the current script
pushd %~p0
cd ..
cd ..
cd ..
docker run -v %cd%:c:\src -w c:\src\platforms\windows\Docker emgu/vs2019_buildtools_cuda_openvino:latest Docker_Build_Helper.bat %*
popd
