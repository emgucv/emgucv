#!/bin/bash
# Install Android NDK for macOS (Apple Silicon and Intel).
#
# Run this script once to set up the build environment, then add the printed
# export lines to your ~/.zshrc (or ~/.bash_profile).
#
# Prerequisites:
#   Homebrew — https://brew.sh
#   cmake and ninja (installed below if missing)

set -e

NDK_VERSION="28.0.12916984"
ANDROID_HOME="${ANDROID_HOME:-$HOME/Library/Android/sdk}"

echo "=== Android NDK macOS installer ==="
echo "ANDROID_HOME: $ANDROID_HOME"
echo "NDK version:  $NDK_VERSION"
echo ""

# ---- cmake and ninja ----
if ! command -v brew &>/dev/null; then
    echo "ERROR: Homebrew is not installed."
    echo "  Install it from https://brew.sh, then re-run this script."
    exit 1
fi

if ! command -v cmake &>/dev/null; then
    echo "Installing cmake..."
    brew install cmake
fi

if ! command -v ninja &>/dev/null; then
    echo "Installing ninja..."
    brew install ninja
fi

# ---- Android command-line tools ----
CMDLINE_TOOLS_DIR="$ANDROID_HOME/cmdline-tools"
SDKMANAGER="$CMDLINE_TOOLS_DIR/latest/bin/sdkmanager"

if [ ! -x "$SDKMANAGER" ]; then
    # Install via Homebrew cask if available, otherwise guide the user
    if brew info --cask android-commandlinetools &>/dev/null 2>&1; then
        echo "Installing Android command-line tools via Homebrew..."
        brew install --cask android-commandlinetools

        # Homebrew installs to a read-only prefix; sdkmanager writes packages
        # to ANDROID_SDK_ROOT, which we set to $ANDROID_HOME below.
        BREW_SDKMANAGER="$(find /opt/homebrew /usr/local \
            -name sdkmanager -path "*/cmdline-tools/*/bin/sdkmanager" \
            2>/dev/null | head -1)"
        if [ -n "$BREW_SDKMANAGER" ]; then
            SDKMANAGER="$BREW_SDKMANAGER"
        fi
    else
        echo "ERROR: Cannot locate sdkmanager."
        echo "  Install Android Studio from https://developer.android.com/studio"
        echo "  or download the command-line tools from:"
        echo "  https://developer.android.com/studio#command-tools"
        echo "  Unzip to: $CMDLINE_TOOLS_DIR/latest/"
        exit 1
    fi
fi

mkdir -p "$ANDROID_HOME"

# ---- Accept licenses ----
echo "Accepting Android SDK licenses..."
yes | "$SDKMANAGER" --sdk_root="$ANDROID_HOME" --licenses > /dev/null 2>&1 || true

# ---- Install NDK ----
echo "Installing NDK $NDK_VERSION (this may take a few minutes)..."
"$SDKMANAGER" --sdk_root="$ANDROID_HOME" --install "ndk;$NDK_VERSION"

ANDROID_NDK="$ANDROID_HOME/ndk/$NDK_VERSION"

if [ ! -d "$ANDROID_NDK" ]; then
    echo "ERROR: NDK installation failed; directory not found: $ANDROID_NDK"
    exit 1
fi

echo ""
echo "=== Installation complete ==="
echo ""
echo "Add the following to your ~/.zshrc (or ~/.bash_profile):"
echo ""
echo "  export ANDROID_HOME=\"$ANDROID_HOME\""
echo "  export ANDROID_NDK=\"$ANDROID_NDK\""
echo "  export PATH=\"\$ANDROID_HOME/cmdline-tools/latest/bin:\$PATH\""
echo ""
echo "Then reload your shell:"
echo "  source ~/.zshrc"
echo ""
echo "To build Emgu CV for Android, run:"
echo "  cd platforms/android/scripts"
echo "  ./build.sh arm64-v8a         # single ABI, full build"
echo "  ./build.sh arm64-v8a mini    # single ABI, mini build"
echo "  ./build_all.sh               # all four ABIs, full build"
