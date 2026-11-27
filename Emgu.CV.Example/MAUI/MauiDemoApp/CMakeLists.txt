# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_MAUI_ANDROID OR HAVE_MAUI_IOS OR HAVE_MAUI_MAC OR HAVE_WINDESKTOP)

  SET(MAUI_BUILD_FRAMEWORKS)
  SET(MAUI_BUILD_RUNTIME_IDENTIFIERS)
  SET(MAUI_BUILD_EXTRA_FLAGS)
  # Each native build packages only its own platform's demo, like the iOS and
  # Catalyst branches below. HAVE_MAUI_ANDROID alone is true whenever Android
  # binaries exist in libs/android, which made e.g. the Catalyst build add an
  # Android target too -- and one dotnet build can't take two TFMs here (see
  # MAUI_BUILD_FRAMEWORKS_STR below).
  IF(HAVE_MAUI_ANDROID AND ANDROID)
    #Build android
    LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-android")
  ENDIF()
  IF(HAVE_MAUI_IOS AND APPLE AND IPHONESIMULATOR)
    MESSAGE(STATUS ">>>>>> HAVE_MAUI_IOS: ${HAVE_MAUI_IOS}; APPLE: ${APPLE}; CMAKE_HOST_SYSTEM_PROCESSOR: ${CMAKE_HOST_SYSTEM_PROCESSOR}; EMGUCV_ARCH: ${EMGUCV_ARCH}")
    IF(CMAKE_HOST_SYSTEM_PROCESSOR STREQUAL "arm64")
      IF(EMGUCV_ARCH STREQUAL "arm64")
        MESSAGE(STATUS "Build demo for ios arm64 simulator")
        LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-ios${IOS_TFM_VERSION}")
        LIST(APPEND MAUI_BUILD_RUNTIME_IDENTIFIERS "iossimulator-arm64")
      ENDIF()
    ELSEIF(CMAKE_HOST_SYSTEM_PROCESSOR STREQUAL "x86_64")
      IF(EMGUCV_ARCH STREQUAL "x64")
        MESSAGE(STATUS "Build demo for ios x64 simulator")
        LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-ios${IOS_TFM_VERSION}")
        LIST(APPEND MAUI_BUILD_RUNTIME_IDENTIFIERS "iossimulator-x64")
      ENDIF()
    ENDIF()
  ENDIF()
  IF(HAVE_MAUI_MAC AND APPLE AND MAC_CATALYST)
    #Build Catalyst
    IF(CMAKE_HOST_SYSTEM_PROCESSOR STREQUAL "arm64")
      IF(EMGUCV_ARCH STREQUAL "arm64")
        LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-maccatalyst${IOS_TFM_VERSION}")
        LIST(APPEND MAUI_BUILD_RUNTIME_IDENTIFIERS "maccatalyst-arm64")
      ENDIF()
    ELSEIF(CMAKE_HOST_SYSTEM_PROCESSOR STREQUAL "x86_64")
      IF(EMGUCV_ARCH STREQUAL "x64")
        LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-maccatalyst${IOS_TFM_VERSION}")
        LIST(APPEND MAUI_BUILD_RUNTIME_IDENTIFIERS "maccatalyst-x64")
      ENDIF()
    ENDIF()
  ENDIF()
  # The installed .NET iOS / MacCatalyst SDK validates its bundled pack version against
  # an EXACT major.minor Xcode match (e.g. SDK 27.0.x demands Xcode 27.0, rejecting
  # even a newer/older installed Xcode such as 26.6) and fails the build outright:
  # "This version of .NET for MacCatalyst (X) requires Xcode Y. The current version
  # of Xcode is Z." CI runner images and local dev machines drift from whatever
  # exact version the SDK pins independently of this repo, so skip that check via
  # the SDK's own documented opt-out rather than pinning a Xcode/SDK combination
  # here. IOS_TFM_VERSION (cmake/PlatformDetection.cmake) already selects bindings the
  # installed Xcode's SDK can build. See the "Native C++ (MacCatalyst / Xcode)" /
  # "MAUI Demo App (MacCatalyst)" sections of CLAUDE.md.
  IF((HAVE_MAUI_MAC AND MAC_CATALYST) OR (HAVE_MAUI_IOS AND IPHONESIMULATOR))
    LIST(APPEND MAUI_BUILD_EXTRA_FLAGS "-p:ValidateXcodeVersion=false")
  ENDIF()
  #    IF(HAVE_WINDESKTOP AND WIN32)
  #      #Build windows
  #      LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-windows${WINDOWS_TFM_VERSION}")
  #      IF(HAVE_WINDESKTOP_X64)
  #        LIST(APPEND MAUI_BUILD_RUNTIME_IDENTIFIERS "win-x64")
  #	  ENDIF()
  #      IF(HAVE_WINDESKTOP_ARM64)
  #        LIST(APPEND MAUI_BUILD_RUNTIME_IDENTIFIERS "win-arm64")
  #	  ENDIF()
  #      IF(HAVE_WINDESKTOP_X86)
  #        LIST(APPEND MAUI_BUILD_RUNTIME_IDENTIFIERS "win-x86")
  #      ENDIF()
  #      SET(MAUI_BUILD_EXTRA_FLAGS "/p:PublishReadyToRun=False")
  #    ENDIF()
  IF(MAUI_BUILD_FRAMEWORKS)
    # Use "%3B" (URL-encoded semicolon) instead of ";" as the list separator.
    # The generated Xcode post-build shell script embeds these values unquoted, so a
    # literal ";" would be interpreted by the shell as a command separator, truncating
    # the dotnet argument list. But MSBuild treats an escaped "%3B" as a literal
    # character, not a list separator, so two or more entries reach the inner build
    # as one invalid TargetFramework (NETSDK1046). This only works with a single
    # framework, which the per-platform conditions above guarantee.
    LIST(LENGTH MAUI_BUILD_FRAMEWORKS _maui_framework_count)
    IF(_maui_framework_count GREATER 1)
      MESSAGE(FATAL_ERROR "MauiDemoApp: one target framework per build is supported, got: ${MAUI_BUILD_FRAMEWORKS}")
    ENDIF()
    LIST(JOIN MAUI_BUILD_FRAMEWORKS "%3B" MAUI_BUILD_FRAMEWORKS_STR)
    SET(MAUI_BUILD_FRAMEWORKS_STR "/p:TargetFrameworks=${MAUI_BUILD_FRAMEWORKS_STR}")
  ELSE()
    SET(MAUI_BUILD_FRAMEWORKS_STR "")
  ENDIF()
  IF(MAUI_BUILD_RUNTIME_IDENTIFIERS)
    # Same "%3B" encoding rationale as MAUI_BUILD_FRAMEWORKS_STR above.
    LIST(JOIN MAUI_BUILD_RUNTIME_IDENTIFIERS "%3B" MAUI_BUILD_RUNTIME_IDENTIFIERS_STR)
    SET(MAUI_BUILD_RUNTIME_IDENTIFIERS_STR "/p:RuntimeIdentifiers=${MAUI_BUILD_RUNTIME_IDENTIFIERS_STR}")
  ELSE()
    SET(MAUI_BUILD_RUNTIME_IDENTIFIERS_STR "")
  ENDIF()

  SET(MAUI_WINDOWS_SDK_VERSION "10.0.22621.0")
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
  # Package.appxmanifest's Identity Version must be a real (CPACK-derived) version rather
  # than a hardcoded placeholder, so that AppxPackage's output filename -- which embeds
  # this Identity Version, see the MSIX packaging block below -- carries the actual Emgu CV
  # release version (e.g. "5.1.0.6690", matching CPACK_PACKAGE_VERSION / version.rc.in's
  # MAJOR.MINOR.PATCH.EMGUCV_GITVERSION convention) instead of always "1.0.0.0".
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Platforms/Windows/Package.appxmanifest.in ${CMAKE_CURRENT_SOURCE_DIR}/Platforms/Windows/Package.appxmanifest)

  INSTALL(
    DIRECTORY
    "${CMAKE_CURRENT_SOURCE_DIR}/"
    DESTINATION Emgu.CV.Example/MAUI/MauiDemoApp/
    COMPONENT emgucv_example_source
    FILES_MATCHING
    PATTERN "*.cs"
    PATTERN "*.csproj"
    PATTERN "*.config"
    PATTERN "*.shproj"
    PATTERN "*.projitems"
    PATTERN "*.props"
    PATTERN "*.axml"
    PATTERN "*.xaml"
    PATTERN "*.xml"
    PATTERN "*.txt"
    PATTERN "*.png"
    PATTERN "*.jpg"
    PATTERN "*.svg"
    PATTERN "*.dll"
    PATTERN "*.json"
    PATTERN "*.manifest"
    PATTERN "*.appxmanifest"
    PATTERN "*.plist"
    PATTERN "*.ttf"
    PATTERN ".git" EXCLUDE
    PATTERN "Debug" EXCLUDE
    PATTERN "Release" EXCLUDE
    PATTERN "CMake*" EXCLUDE
    )

  IF(NOT "${MAUI_BUILD_FRAMEWORKS_STR}" STREQUAL "")
    PROJECT(Example.Maui.DemoApp)
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj" "${MAUI_BUILD_FRAMEWORKS_STR} ${MAUI_BUILD_RUNTIME_IDENTIFIERS_STR} ${MAUI_BUILD_EXTRA_FLAGS}" ALL)

    TARGET_SOURCES(${PROJECT_NAME} PRIVATE
      "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in"
      "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props"
      "${CMAKE_CURRENT_SOURCE_DIR}/Platforms/Windows/Package.appxmanifest.in"
      "${CMAKE_CURRENT_SOURCE_DIR}/Platforms/Windows/Package.appxmanifest"
      "${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj"
      )

    SOURCE_GROUP("Template" FILES
      "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in"
      "${CMAKE_CURRENT_SOURCE_DIR}/Platforms/Windows/Package.appxmanifest.in")

    SOURCE_GROUP("Source" FILES
      "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props"
      "${CMAKE_CURRENT_SOURCE_DIR}/Platforms/Windows/Package.appxmanifest"
      "${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj"
      )

    SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")

    IF(TARGET Emgu.CV.runtime.maui.ios)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.ios)
    ENDIF()

    IF(TARGET Emgu.CV.runtime.maui.macos)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.macos)
    ENDIF()

    IF(TARGET Emgu.CV.runtime.maui.android)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.android)
    ENDIF()

    IF(TARGET Emgu.CV.runtime.windows.nuget)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.windows.nuget)
    ENDIF()

    IF(TARGET Emgu.CV.runtime.maui)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui)
    ENDIF()

    IF(TARGET cvextern)
      ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
    ENDIF()

    IF(HAVE_MAUI_ANDROID)
      ADD_CUSTOM_COMMAND(
        TARGET ${PROJECT_NAME}
        POST_BUILD
        COMMAND ${CMAKE_COMMAND}
          -DSRC_DIR=${CMAKE_CURRENT_SOURCE_DIR}/bin/${DEFAULT_CS_CONFIG}/net${DOTNET_VERSION_MAJOR}.0-android
          -DDEST_DIR=${EMGU_CV_SOURCE_DIR}/package
          -P ${EMGU_CV_SOURCE_DIR}/cmake/CopyMauiApk.cmake
        COMMENT "Copying MAUI Android demo .apk to ${EMGU_CV_SOURCE_DIR}/package")
    ENDIF()

    # The csproj zips the Release Mac Catalyst app only when it is signed with a
    # Developer ID identity (and notarized, when it can be), so an ad hoc signed
    # app that Gatekeeper would block never lands in the package folder. With
    # RuntimeIdentifiers set, the signed app and the zip are in the TFM folder,
    # not its per-RID subfolder.
    IF(HAVE_MAUI_MAC AND MAC_CATALYST)
      ADD_CUSTOM_COMMAND(
        TARGET ${PROJECT_NAME}
        POST_BUILD
        COMMAND ${CMAKE_COMMAND}
          -DSRC_DIR=${CMAKE_CURRENT_SOURCE_DIR}/bin/${DEFAULT_CS_CONFIG}/net${DOTNET_VERSION_MAJOR}.0-maccatalyst${IOS_TFM_VERSION}
          -DDEST_DIR=${EMGU_CV_SOURCE_DIR}/package
          -DPATTERN=Emgu.CV.Maui.Demo.maccatalyst.*.zip
          -P ${EMGU_CV_SOURCE_DIR}/cmake/CopyMauiApk.cmake
        COMMENT "Copying MAUI Mac Catalyst demo .zip to ${EMGU_CV_SOURCE_DIR}/package")
    ENDIF()
  ENDIF()

  # Windows builds of the MAUI demo app: an unpackaged .exe (WindowsPackageType=None,
  # no cert needed) and, when a signing cert is available, a packaged/signed MSIX.
  # Both invocations build the SAME csproj/Configuration/TargetFramework, and MSBuild's
  # obj/bin intermediate paths are keyed only by Configuration+TargetFramework -- NOT by
  # WindowsPackageType -- so both builds write to the identical
  # obj/Release/net10.0-windows10.0.19041.0/ and bin/Release/.../win-x64/ directories
  # (confirmed by inspecting the actual obj/ layout after building each). Running them
  # concurrently (as independent CMake targets can be, e.g. under a parallel Visual
  # Studio "Build Solution" or `cmake --build . --parallel`) risks two dotnet/MSBuild
  # processes racing on the same intermediate files. The MSIX target below therefore
  # takes an explicit ADD_DEPENDENCIES on the .exe target to force them to run
  # sequentially instead.
  IF(WIN32 AND HAVE_WINDESKTOP)
    SET(MAUI_WINDOWS_TFM "net${DOTNET_VERSION_MAJOR}.0-windows10.0.19041.0")

    # Without an explicit Platform, MSBuild's $(Platform) defaults to "AnyCPU", which the
    # WindowsAppSDK/MAUI Windows packaging targets then resolve to an x86 RuntimeIdentifier
    # regardless of which architecture's cvextern.dll was actually built by Build_Binary.ps1
    # -- there is no implicit link between the two. This bit the "cv x64" builder: its native
    # build correctly produced win-x64 binaries, but the MAUI Windows .exe/.msix below silently
    # packaged win-x86 ones instead, with no build error. Pin Platform explicitly from the same
    # HAVE_WINDESKTOP_X64/_ARM64/_X86 flags (cmake/PlatformDetection.cmake) that already gate
    # this whole block, mirroring the Android/iOS/MacCatalyst branches above which pass their
    # RuntimeIdentifier(s) explicitly rather than relying on an SDK default.
    #
    # Pinning Platform alone is NOT enough once more than one architecture is built in the same
    # configure (see MAUI_WINDOWS_PLATFORMS below): Microsoft.Maui.Sdk.Before.targets sets
    # RuntimeIdentifier unconditionally from the *build machine's own OS architecture*
    # ($(_SingleProjectHostArchitecture), via System.Runtime.InteropServices.RuntimeInformation),
    # never from $(Platform) -- it only skips this when RuntimeIdentifier/RuntimeIdentifiers was
    # already explicitly set. So on an x64 build machine, every Platform value silently gets
    # RuntimeIdentifier=win-x64, and MSBuild's own PlatformTarget/RuntimeIdentifier consistency
    # check (Microsoft.NET.RuntimeIdentifierInference.targets) then fails outright for any
    # Platform other than x64 ("error NETSDK1032: The RuntimeIdentifier platform 'win-x64' and
    # the PlatformTarget 'arm64' must be compatible") -- reproduced locally by building this
    # csproj directly for Platform=arm64/x86 with nothing else changed, even from a clean obj/.
    # Passing RuntimeIdentifier explicitly below (matching Platform) bypasses MAUI's host-arch
    # default entirely, the same fix already used by the Android/iOS/MacCatalyst branches above.
    #
    # CHECK_BINARY_EXIST (cmake/PlatformDetection.cmake) sets these flags whenever
    # libs/runtimes/win-<arch>/native/*.dll already exists on disk, regardless of which single
    # EMGUCV_ARCH the current configure is for -- so if Build_Binary.ps1 was run for more than
    # one -Arch against this same tree without cleaning in between (e.g. a release machine that
    # ran x64, then arm64, then x86), more than one of these can be TRUE simultaneously in one
    # configure. Build every available architecture's own .exe/.msix below, then combine them
    # into one signed .msixbundle when there's more than one -- a single builder's normal
    # single-arch case (only one flag TRUE) is unaffected and keeps producing just one .msix.
    SET(MAUI_WINDOWS_PLATFORMS)
    IF(HAVE_WINDESKTOP_X64)
      LIST(APPEND MAUI_WINDOWS_PLATFORMS "x64")
    ENDIF()
    IF(HAVE_WINDESKTOP_ARM64)
      LIST(APPEND MAUI_WINDOWS_PLATFORMS "arm64")
    ENDIF()
    IF(HAVE_WINDESKTOP_X86)
      LIST(APPEND MAUI_WINDOWS_PLATFORMS "x86")
    ENDIF()
    LIST(LENGTH MAUI_WINDOWS_PLATFORMS MAUI_WINDOWS_PLATFORM_COUNT)
    # Matches Directory.Build.props.in's EmguCVPackageVersion / Package.appxmanifest.in's
    # Identity Version substitution -- the single source of truth for this release's version,
    # and what MauiDemoApp.csproj's <AppxPackageName> bakes into each per-arch .msix's filename.
    SET(MAUI_WINDOWS_PACKAGE_VERSION "${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR}.${CPACK_PACKAGE_VERSION_PATCH}.${EMGUCV_GITVERSION}")

    PROJECT(Example.Maui.DemoApp.Windows.Exe)
    SET(_maui_windows_exe_first TRUE)
    FOREACH(_maui_plat ${MAUI_WINDOWS_PLATFORMS})
      IF(_maui_windows_exe_first)
        ADD_CUSTOM_TARGET(${PROJECT_NAME} ALL
          COMMAND "${DOTNET_EXECUTABLE}" build "${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj" -c ${DEFAULT_CS_CONFIG} -f ${MAUI_WINDOWS_TFM} -p:Platform=${_maui_plat} -p:RuntimeIdentifier=win-${_maui_plat} -p:WindowsPackageType=None
          COMMENT "Building unpackaged .exe for ${PROJECT_NAME} with ${DOTNET_EXECUTABLE} (Platform=${_maui_plat})")
        SET(_maui_windows_exe_first FALSE)
      ELSE()
        ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} POST_BUILD
          COMMAND "${DOTNET_EXECUTABLE}" build "${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj" -c ${DEFAULT_CS_CONFIG} -f ${MAUI_WINDOWS_TFM} -p:Platform=${_maui_plat} -p:RuntimeIdentifier=win-${_maui_plat} -p:WindowsPackageType=None
          COMMENT "Building unpackaged .exe for ${PROJECT_NAME} with ${DOTNET_EXECUTABLE} (Platform=${_maui_plat})")
      ENDIF()
    ENDFOREACH()

    SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")

    IF(TARGET Emgu.CV.runtime.windows.nuget)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.windows.nuget)
    ENDIF()

    IF(TARGET Emgu.CV.runtime.maui)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui)
    ENDIF()

    IF(TARGET cvextern)
      ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
    ENDIF()

    # Signed MSIX packaging. Only added to the build graph when the same EmguSign.bat /
    # code-signing certificate used to Authenticode-sign the other Windows binaries
    # (Emgu.CV.dll, cvextern.dll, etc. -- see FindCodeSign.cmake and
    # Package.appxmanifest's Identity Publisher, which must match that cert's Subject)
    # is present on this machine. An MSIX built without it would either fail outright
    # (AppxPackageSigningEnabled=true + a PackageCertificateThumbprint the local cert
    # store can't resolve) or produce an unsigned/untrusted package not worth shipping,
    # so on a machine without the signing cert we skip the MSIX target entirely rather
    # than let the ALL build fail or silently produce a package nobody can install.
    IF(EMGU_SIGN_FOUND)
      PROJECT(Example.Maui.DemoApp.Windows.Msix)
      # AppxPackage's output filename embeds the Emgu CV release version (currently
      # AppPackages/Emgu.CV.Maui.Demo.windows.x64.5.1.0.6716_Test/Emgu.CV.Maui.Demo.windows.x64.5.1.0.6716.msix,
      # see MauiDemoApp.csproj's <AppxPackageName> override and Directory.Build.props.in's
      # EmguCVPackageVersion, both ultimately from the CPACK/EMGUCV_GITVERSION variables set
      # near the top of the root CMakeLists.txt), so the copy step below globs for it via
      # CopyMsixToPackage.cmake rather than hardcoding that version a second time here.
      # A plain `cmd /c copy` custom command
      # doesn't work for this: cmd.exe's copy only expands wildcards in the filename
      # component of a path, not in a directory component like "AppPackages\*\", so
      # `copy AppPackages\*\*_x64.msix` fails with "The syntax of the command is
      # incorrect." -- confirmed by reproducing it standalone. file(GLOB ...) in a
      # cmake -P script has no such restriction.
      SET(_maui_windows_msix_first TRUE)
      FOREACH(_maui_plat ${MAUI_WINDOWS_PLATFORMS})
        IF(_maui_windows_msix_first)
          ADD_CUSTOM_TARGET(${PROJECT_NAME} ALL
            COMMAND "${DOTNET_EXECUTABLE}" publish "${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj" -c ${DEFAULT_CS_CONFIG} -f ${MAUI_WINDOWS_TFM} -p:Platform=${_maui_plat} -p:RuntimeIdentifier=win-${_maui_plat} -p:WindowsPackageType=MSIX -p:GenerateAppxPackageOnBuild=true
            COMMAND ${CMAKE_COMMAND} "-DSRC_DIR=${CMAKE_CURRENT_SOURCE_DIR}" "-DDEST_DIR=${CMAKE_SOURCE_DIR}/package" -P "${CMAKE_CURRENT_SOURCE_DIR}/cmake/CopyMsixToPackage.cmake"
            COMMENT "Building signed MSIX for ${PROJECT_NAME} with ${DOTNET_EXECUTABLE} (Platform=${_maui_plat}, signing via EMGU_SIGN_EXECUTABLE_NATIVE_PATH=${EMGU_SIGN_EXECUTABLE_NATIVE_PATH}), then copying it to ${CMAKE_SOURCE_DIR}/package")
          SET(_maui_windows_msix_first FALSE)
        ELSE()
          ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} POST_BUILD
            COMMAND "${DOTNET_EXECUTABLE}" publish "${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj" -c ${DEFAULT_CS_CONFIG} -f ${MAUI_WINDOWS_TFM} -p:Platform=${_maui_plat} -p:RuntimeIdentifier=win-${_maui_plat} -p:WindowsPackageType=MSIX -p:GenerateAppxPackageOnBuild=true
            COMMAND ${CMAKE_COMMAND} "-DSRC_DIR=${CMAKE_CURRENT_SOURCE_DIR}" "-DDEST_DIR=${CMAKE_SOURCE_DIR}/package" -P "${CMAKE_CURRENT_SOURCE_DIR}/cmake/CopyMsixToPackage.cmake"
            COMMENT "Building signed MSIX for ${PROJECT_NAME} with ${DOTNET_EXECUTABLE} (Platform=${_maui_plat}, signing via EMGU_SIGN_EXECUTABLE_NATIVE_PATH=${EMGU_SIGN_EXECUTABLE_NATIVE_PATH}), then copying it to ${CMAKE_SOURCE_DIR}/package")
        ENDIF()
      ENDFOREACH()

      # More than one architecture's native binaries were available this configure (see the
      # HAVE_WINDESKTOP_X64/_ARM64/_X86 comment above) -- combine the per-arch .msix packages
      # just built into one signed .msixbundle via makeappx.exe, in addition to (not instead
      # of) the individual per-arch .msix files already copied to package/ above.
      IF(MAUI_WINDOWS_PLATFORM_COUNT GREATER 1)
        IF(NOT MAKEAPPX_FOUND)
          MESSAGE(FATAL_ERROR "MAUI_WINDOWS_PLATFORMS has ${MAUI_WINDOWS_PLATFORM_COUNT} entries (${MAUI_WINDOWS_PLATFORMS}) but MAKEAPPX_EXECUTABLE (cmake/modules/FindCSharp.cmake) was not found -- cannot combine them into a .msixbundle.")
        ENDIF()
        SET(MAUI_WINDOWS_BUNDLE_FILE "${CMAKE_SOURCE_DIR}/package/Emgu.CV.Maui.Demo.windows.${MAUI_WINDOWS_PACKAGE_VERSION}.msixbundle")
        ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} POST_BUILD
          COMMAND ${CMAKE_COMMAND}
            "-DSRC_DIR=${CMAKE_CURRENT_SOURCE_DIR}"
            "-DPLATFORMS=${MAUI_WINDOWS_PLATFORMS}"
            "-DVERSION=${MAUI_WINDOWS_PACKAGE_VERSION}"
            "-DMAKEAPPX_EXECUTABLE=${MAKEAPPX_EXECUTABLE}"
            "-DOUTPUT_FILE=${MAUI_WINDOWS_BUNDLE_FILE}"
            -P "${CMAKE_CURRENT_SOURCE_DIR}/cmake/BundleMsixPackages.cmake"
          COMMENT "Combining ${MAUI_WINDOWS_PLATFORM_COUNT} per-arch MSIX packages (${MAUI_WINDOWS_PLATFORMS}) into ${MAUI_WINDOWS_BUNDLE_FILE}")
        # EMGU_SIGN_BINARY (cmake/modules/FindCodeSign.cmake) is generic over any file, not
        # just DLLs -- reuse the same EmguSign.bat/signtool machinery used to sign cvextern.dll
        # etc. elsewhere, instead of hand-rolling a second signing invocation here. Custom
        # commands on the same target run in the order they were added, so this runs after the
        # bundle above.
        EMGU_SIGN_BINARY(${PROJECT_NAME} "${MAUI_WINDOWS_BUNDLE_FILE}")
      ENDIF()

      SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")

      # Force sequential ordering with the .exe target -- see the comment above this
      # IF(WIN32 AND HAVE_WINDESKTOP) block for why this is required, not optional.
      ADD_DEPENDENCIES(${PROJECT_NAME} Example.Maui.DemoApp.Windows.Exe)

      IF(TARGET Emgu.CV.runtime.windows.nuget)
        ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.windows.nuget)
      ENDIF()

      IF(TARGET Emgu.CV.runtime.maui)
        ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui)
      ENDIF()

      IF(TARGET cvextern)
        ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
      ENDIF()
    ELSE()
      MESSAGE(STATUS "EMGU_SIGN_EXECUTABLE_NATIVE_PATH not found -- skipping signed MSIX packaging for the MAUI Windows demo app.")
    ENDIF()
  ENDIF()
ENDIF()
