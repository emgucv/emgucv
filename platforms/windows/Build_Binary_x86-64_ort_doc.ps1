<#
.SYNOPSIS
    Build x86_64 with ONNX Runtime (CPU execution provider) and documentation.
#>
& "$PSScriptRoot\Build_Binary.ps1" -Arch x86_64 -OnnxRuntime -Documentation
