<#
.SYNOPSIS
    PowerShell equivalent of Build_Binary_arm64_windows_store10.bat.
#>
& "$PSScriptRoot\Build_Binary_x86.ps1" -Arch arm64 -Toolchain WindowsStore10
