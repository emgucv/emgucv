# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (HAVE_MAUI_ANDROID OR HAVE_MAUI_IOS OR HAVE_MAUI_MAC OR HAVE_WINDESKTOP) 
  PROJECT(Example.Maui.DemoApp)

    SET(MAUI_WINDOWS_SDK_VERSION "10.0.22621.0")
    CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)

    SET(MAUI_BUILD_FRAMEWORKS)
    IF(HAVE_MAUI_ANDROID)
      #Build android
      LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-android")      
	ENDIF()
    IF (HAVE_MAUI_IOS AND APPLE)
      #Build android
      LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-ios")      
    ENDIF()
    IF (HAVE_MAUI_MAC AND APPLE)
      #Build android
      LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-ios")      
    ENDIF()
	IF(HAVE_WINDESKTOP AND WIN32)
      #Build windows
      LIST(APPEND MAUI_BUILD_FRAMEWORKS "net${DOTNET_VERSION_MAJOR}.0-windows10.0.22621.0")      
	ENDIF()
    IF(MAUI_BUILD_FRAMEWORKS)
      LIST(JOIN MAUI_BUILD_FRAMEWORKS ";" MAUI_BUILD_FRAMEWORKS_STR)
      SET(MAUI_BUILD_FRAMEWORKS_STR "/p:TargetFrameworks=${MAUI_BUILD_FRAMEWORKS_STR}")
    ELSE()
      SET(MAUI_BUILD_FRAMEWORKS_STR "")
    ENDIF()
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/MauiDemoApp.csproj" "${MAUI_BUILD_FRAMEWORKS_STR}" ALL)
  
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
	set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
  
    IF (TARGET Emgu.CV.runtime.maui)
      ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.runtime.maui)
    ENDIF()
ENDIF()