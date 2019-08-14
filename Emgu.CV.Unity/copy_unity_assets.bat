pushd %~p0
call wsl dos2unix copy_unity_assets
call wsl ./copy_unity_assets
popd