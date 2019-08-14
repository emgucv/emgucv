pushd %~p0
call wsl dos2unix sync_unity_assets
call wsl ./sync_unity_assets
popd