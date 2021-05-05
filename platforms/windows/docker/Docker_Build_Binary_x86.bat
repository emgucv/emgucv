REM go to the folder of the current script
pushd %~p0
cd ..
cd ..
cd ..
docker run -v %cd%:c:\src -w c:\src\platforms\windows\Docker --rm -m 3G emgu/vs2019_buildtools_cuda_openvino:openvino_2021.2.185 Docker_Build_Helper.bat %*
popd
