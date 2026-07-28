<#
.SYNOPSIS
    Build x86_64 for the Windows Store 10 (UWP) toolchain.
#>
& "$PSScriptRoot\Build_Binary.ps1" -Arch x86_64 -Toolchain WindowsStore10
