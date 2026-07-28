<#
.SYNOPSIS
    Build ARM64 for the Windows Store 10 (UWP) toolchain.
#>
& "$PSScriptRoot\Build_Binary.ps1" -Arch arm64 -Toolchain WindowsStore10
