<#
.SYNOPSIS
    PowerShell equivalent of Build_Binary_x86_windows_store10.bat.
#>
& "$PSScriptRoot\Build_Binary_x86.ps1" -Arch x86 -Toolchain WindowsStore10
