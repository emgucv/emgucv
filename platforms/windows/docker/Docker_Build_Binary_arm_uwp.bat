REM go to the folder of the current script
pushd %~p0
call Docker_Build_binary_x86.bat ARM nogpu WindowsStore10 no-openni nodoc package build
popd
