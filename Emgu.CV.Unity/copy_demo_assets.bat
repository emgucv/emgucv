pushd %~p0
call wsl dos2unix copy_demo_assets
call wsl ./copy_demo_assets
popd