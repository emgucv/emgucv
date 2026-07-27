<#
.SYNOPSIS
    PowerShell equivalent of Build_Binary_arm_windows_store10.bat.
#>
& "$PSScriptRoot\Build_Binary.ps1" -Arch arm -Toolchain WindowsStore10
