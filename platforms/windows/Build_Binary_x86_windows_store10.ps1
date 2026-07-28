<#
.SYNOPSIS
    Build x86 for the Windows Store 10 (UWP) toolchain.
#>
& "$PSScriptRoot\Build_Binary.ps1" -Arch x86 -Toolchain WindowsStore10
