<#
.SYNOPSIS
    PowerShell equivalent of Build_Binary_x86-64_intel_icl_doc.bat.
#>
& "$PSScriptRoot\Build_Binary_x86.ps1" -Arch x86_64 -Toolchain Intel -Documentation
