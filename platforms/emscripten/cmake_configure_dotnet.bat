pushd %~p0
call wsl dos2unix cmake_configure_dotnet
call wsl ./cmake_configure_dotnet
popd