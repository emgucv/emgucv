#!/bin/bash
# Build Emgu CV native library for all Android ABIs on macOS.
#
# Usage:
#   cd platforms/android/scripts
#   ./build_all.sh [variant]
#
# variant: (empty)/full, core, or mini  (same as build.sh)

set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

VARIANT="${1:-}"

for ABI in armeabi-v7a arm64-v8a x86 x86_64; do
    echo ""
    echo "=========================================="
    echo " ABI: $ABI  variant: ${VARIANT:-full}"
    echo "=========================================="
    "$SCRIPT_DIR/build.sh" "$ABI" "$VARIANT"
done

echo ""
echo "=== All ABIs built successfully ==="
