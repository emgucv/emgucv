# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (HAVE_MAUI_ANDROID OR HAVE_MAUI_IOS OR HAVE_MAUI_MAC OR HAVE_WINDESKTOP)

    SET(MAUI_BUILD_FRAMEWORKS)
    SET(MAUI_BUILD_RUNTIME_IDENTIFIERS)
    SET(MAUI_BUILD_EXTRA_FLAGS)
    IF(HAVE_MAUI_ANDROID)
      #Build android
      LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-android")      
    ENDIF()
    IF (HAVE_MAUI_IOS AND APPLE AND IPHONESIMULATOR)
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
    IF (HAVE_MAUI_MAC AND APPLE AND MAC_CATALYST)
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
      # the dotnet argument list. MSBuild/dotnet decodes "%3B" back to ";" at runtime,
      # so the property value is received correctly as a semicolon-separated list.
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
      )
	  
  IF (NOT "${MAUI_BUILD_FRAMEWORKS_STR}" STREQUAL "")
    PROJECT(Example.Maui.DemoApp)
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj" "${MAUI_BUILD_FRAMEWORKS_STR} ${MAUI_BUILD_RUNTIME_IDENTIFIERS_STR} ${MAUI_BUILD_EXTRA_FLAGS}" ALL)
  
    TARGET_SOURCES(${PROJECT_NAME} PRIVATE
	"${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in"
	"${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props"
	"${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj"
    )

    source_group("Template" FILES 
	"${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in")
	
    source_group("Source" FILES 
	"${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props"
	"${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj"
    )


	set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")

    IF (TARGET Emgu.CV.runtime.maui.ios)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.ios)
    ENDIF()
  
    IF (TARGET Emgu.CV.runtime.maui.macos)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.macos)
    ENDIF()
  
    IF (TARGET Emgu.CV.runtime.maui.android)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui.android)
    ENDIF()
  
    IF (TARGET Emgu.CV.runtime.windows.nuget)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.windows.nuget)
    ENDIF()

    IF (TARGET Emgu.CV.runtime.maui)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui)
    ENDIF()

    IF (TARGET cvextern)
      ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
    ENDIF()
  ENDIF()
ENDIF()
