#!/bin/bash
# macOS build environment configuration for Android cross-compilation.
# Source this file from build.sh — do not run it directly.
#
# Sets ANDROID_NDK, CMAKE, and MAKE if they are not already defined.
# Prefer environment variables already set by the caller over auto-detection.

# ---- Android SDK / NDK ----
if [ -z "$ANDROID_SDK" ]; then
    for SDK_CANDIDATE in \
        "$HOME/Library/Android/sdk" \
        "$HOME/Android/Sdk" \
        "/usr/local/lib/android/sdk" \
    ; do
        if [ -d "$SDK_CANDIDATE" ]; then
            ANDROID_SDK="$SDK_CANDIDATE"
            break
        fi
    done
fi

if [ -z "$ANDROID_NDK" ] && [ -n "$ANDROID_SDK" ]; then
    # Side-by-side NDKs (<sdk>/ndk/<version>): use the newest installed one.
    # Folders whose name is not a plain version number are ignored.
    NEWEST_NDK="$(ls -1 "$ANDROID_SDK/ndk" 2>/dev/null \
        | grep -E '^[0-9]+(\.[0-9]+)*$' \
        | sort -t. -k1,1n -k2,2n -k3,3n \
        | tail -n 1)"
    if [ -n "$NEWEST_NDK" ] && [ -d "$ANDROID_SDK/ndk/$NEWEST_NDK" ]; then
        ANDROID_NDK="$ANDROID_SDK/ndk/$NEWEST_NDK"
    elif [ -d "$ANDROID_SDK/ndk-bundle" ]; then
        ANDROID_NDK="$ANDROID_SDK/ndk-bundle"
    fi
fi

# ---- make / ninja ----
if [ -z "$MAKE" ]; then
    # Prefer ninja (faster incremental builds)
    if command -v ninja &>/dev/null; then
        MAKE="$(command -v ninja)"
    elif [ -n "$ANDROID_NDK" ]; then
        # NDK bundles a prebuilt GNU make — prefer ARM64 on Apple Silicon, x86_64 on Intel
        for MAKE_CANDIDATE in \
            "$ANDROID_NDK/prebuilt/darwin-arm64/bin/make" \
            "$ANDROID_NDK/prebuilt/darwin-x86_64/bin/make" \
        ; do
            if [ -x "$MAKE_CANDIDATE" ]; then
                MAKE="$MAKE_CANDIDATE"
                break
            fi
        done
    fi
    # Fall back to system make
    if [ -z "$MAKE" ]; then
        MAKE="$(command -v make 2>/dev/null || true)"
    fi
fi

# ---- cmake ----
if [ -z "$CMAKE" ]; then
    for CMAKE_CANDIDATE in \
        "/opt/homebrew/bin/cmake" \
        "/usr/local/bin/cmake" \
        "/Applications/CMake.app/Contents/bin/cmake" \
        "$(command -v cmake 2>/dev/null)" \
    ; do
        if [ -n "$CMAKE_CANDIDATE" ] && [ -x "$CMAKE_CANDIDATE" ]; then
            CMAKE="$CMAKE_CANDIDATE"
            break
        fi
    done
fi

export ANDROID_SDK ANDROID_NDK CMAKE MAKE
