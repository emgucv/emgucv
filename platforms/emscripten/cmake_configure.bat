pushd %~p0
call wsl dos2unix cmake_configure
call wsl ./cmake_configure
popd