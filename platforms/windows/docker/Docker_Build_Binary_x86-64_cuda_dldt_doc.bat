REM go to the folder of the current script
pushd %~p0
call Docker_Build_binary_x86.bat 64 gpu inf no-openni doc package build
popd