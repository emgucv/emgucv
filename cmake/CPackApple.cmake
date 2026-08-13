# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  Body of the APPLE branch of the top-level CPack ANDROID/WIN32/APPLE/LINUX
# selection: source/example file installs and the macOS bundle Info.plist.
# Included from inside 'ELSEIF (APPLE)'.
#  Included from the root CMakeLists.txt; CMAKE_CURRENT_SOURCE_DIR still
#  refers to the repository root here (include() does not change it).
# ----------------------------------------------------------------------------

  SET(CPACK_GENERATOR ZIP)
  SET(CPACK_BUNDLE_NAME ${CPACK_PACKAGE_NAME})
  
  SET(CPACK_BUNDLE_ICON "${CMAKE_CURRENT_SOURCE_DIR}/platforms/macos/icons.icns")
  IF (IOS)
    SET(CPACK_ARCHIVE_COMPONENT_INSTALL ON) #enable components install for zip
    IF(CPACK_GENERATOR MATCHES "ZIP")
      SET(CPACK_COMPONENTS_ALL_IN_ONE_PACKAGE 1)
    ENDIF()
    set(CPACK_COMPONENTS_ALL emgucv_binary emgucv_source emgucv_example_source)
  ENDIF()

  # ----------------------------------------------------------------------------
  #  The source files
  # ----------------------------------------------------------------------------
  
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/lib
    DESTINATION .
    COMPONENT emgucv_source
    FILES_MATCHING 
    PATTERN "*.dll"
    PATTERN "*.txt"
    PATTERN "*.xml"
    PATTERN ".git" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMake*" EXCLUDE
    PATTERN "Release" EXCLUDE
    PATTERN "${PROJECT_NAME}.dir" EXCLUDE
    )

  
  # ----------------------------------------------------------------------------
  #  The example files
  # ----------------------------------------------------------------------------
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Example
    DESTINATION .
    COMPONENT emgucv_example_source
    FILES_MATCHING 
    PATTERN "*.cs"
    PATTERN "*.csproj"
    PATTERN "*.resx"
    PATTERN "*.h"
    PATTERN "*.cpp"
    PATTERN "*.resX"
    PATTERN "*.ico"
    PATTERN "*.rc"
    PATTERN "CPlusPlus/*.vcproj"
    PATTERN "CPlusPlus/*.vcxproj"
    PATTERN "*.vb"
    PATTERN "*.vbproj"
    PATTERN "*.aspx" 
    PATTERN "*.dll"
    PATTERN "*.txt"
    PATTERN "*.xml"
    PATTERN "*.xaml"
    PATTERN "*.jpg"
    PATTERN "*.png"  
    PATTERN "*.settings"
    PATTERN "*.config"
    PATTERN "tessdata/*"
    PATTERN ".git" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMake*" EXCLUDE
    PATTERN "Release" EXCLUDE
    PATTERN "Debug" EXCLUDE
    PATTERN "*.dir" EXCLUDE
    PATTERN "Android" EXCLUDE
    PATTERN "iOS" EXCLUDE
    )
  
  INSTALL(
    FILES
    "${OPENCV_EXTRA_SUBFOLDER}/testdata/cv/cascadeandhog/cascades/haarcascade_frontalface_default.xml"
    "${OPENCV_EXTRA_SUBFOLDER}/testdata/cv/cascadeandhog/cascades/haarcascade_eye.xml"
    DESTINATION opencv_extra/test_data/cv/cascadeandhog/cascades
    COMPONENT emgucv_example_source
    )
  
  INSTALL(
    FILES
    "${OPENCV_CONTRIB_SUBFOLDER}/modules/text/samples/trained_classifierNM1.xml"
    "${OPENCV_CONTRIB_SUBFOLDER}/modules/text/samples/trained_classifierNM2.xml"
    "${OPENCV_CONTRIB_SUBFOLDER}/modules/text/samples/trained_classifier_erGrouping.xml"
    DESTINATION opencv_contrib/modules/text/samples/
    COMPONENT emgucv_example_source
    )
  
  
  # ----------------------------------------------------------------------------
  #  Generate required common assembly file for Emgu CV project
  # ----------------------------------------------------------------------------
  SET(CPACK_BUNDLE_PLIST ${CMAKE_CURRENT_SOURCE_DIR}/cmake/macos/Info.plist)
  FILE(WRITE ${CPACK_BUNDLE_PLIST}
    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>
<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">
<plist version=\"1.0\">
<dict>
   <key>CFBundleIdentifier</key>
   <string>com.emgu.cv</string>
   <key>CFBundleName</key>
   <string>${CPACK_BUNDLE_NAME}</string>
   <key>CFBundleVersion</key>
   <string>1</string>
   <key>LSMinimumSystemVersion</key>
   <string>10.6</string>
</dict>
</plist>")
