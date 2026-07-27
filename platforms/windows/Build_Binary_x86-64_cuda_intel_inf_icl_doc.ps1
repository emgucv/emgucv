<#
.SYNOPSIS
    PowerShell equivalent of Build_Binary_x86-64_cuda_intel_inf_icl_doc.bat.
#>
& "$PSScriptRoot\Build_Binary.ps1" -Arch x86_64 -Cuda -Toolchain IntelOpenVino -Documentation
