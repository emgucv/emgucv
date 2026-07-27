<#
.SYNOPSIS
    PowerShell equivalent of Build_Binary_x86-64_windows_store10.bat.
#>
& "$PSScriptRoot\Build_Binary_x86.ps1" -Arch x86_64 -Toolchain WindowsStore10
