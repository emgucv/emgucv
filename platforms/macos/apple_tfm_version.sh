#!/usr/bin/env bash
# Prints the iOS/MacCatalyst platform version to put in the .NET TFM
# (net10.0-ios<VERSION> / net10.0-maccatalyst<VERSION>) for the installed Xcode.
# An empty result means the bare TFM, i.e. the workload's newest bindings.
#
# The bare TFM resolves to the newest bindings in the installed .NET workload,
# which require the matching Xcode (e.g. 27.x bindings need Xcode 27). Older
# Xcode releases must pin an explicit platform version the workload still ships.
# Update the table below when the workload adds or drops platform versions.
#
# Set EMGU_IOS_TFM_VERSION to override the detection.

if [ -n "${EMGU_IOS_TFM_VERSION+x}" ]; then
    echo "$EMGU_IOS_TFM_VERSION"
    exit 0
fi

SDK_VERSION="$(xcrun --sdk iphoneos --show-sdk-version 2>/dev/null)"
if [[ ! "$SDK_VERSION" =~ ^([0-9]+)\.([0-9]+) ]]; then
    echo "apple_tfm_version.sh: could not read the iOS SDK version from xcrun; using the bare TFM" >&2
    echo ""
    exit 0
fi
MAJOR=${BASH_REMATCH[1]}
MINOR=${BASH_REMATCH[2]}

if [ "$MAJOR" -ge 27 ]; then
    echo ""
elif [ "$MAJOR" -eq 26 ] && [ "$MINOR" -ge 5 ]; then
    echo "26.5"
elif [ "$MAJOR" -eq 26 ]; then
    echo "26.0"
else
    echo "apple_tfm_version.sh: iOS SDK $SDK_VERSION is older than any platform version this table knows; using the bare TFM" >&2
    echo ""
fi
