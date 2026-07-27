<#
.SYNOPSIS
    PowerShell equivalent of Build_Binary_x86_cuda_intel_icl.bat.
#>
& "$PSScriptRoot\Build_Binary_x86.ps1" -Arch x86 -Cuda -Toolchain Intel
