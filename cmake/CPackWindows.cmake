# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  Body of the WIN32 branch of the top-level CPack ANDROID/WIN32/APPLE/LINUX
# selection: DebuggerVisualizer subdirectory, FFmpeg/version_string.inc
# installs, NSIS menu links, component wiring, and the installer/7z signing
# scripts. Included from inside 'ELSEIF (WIN32)'.
#  Included from the root CMakeLists.txt; CMAKE_CURRENT_SOURCE_DIR still
#  refers to the repository root here (include() does not change it).
# ----------------------------------------------------------------------------

  SET(CPACK_GENERATOR ZIP;7Z)
  
  SET(CPACK_ARCHIVE_COMPONENT_INSTALL ON) #enable components install for zip
  IF(CPACK_GENERATOR MATCHES "ZIP")
    SET(CPACK_COMPONENTS_ALL_IN_ONE_PACKAGE 1)
  ENDIF()
  SET(CPACK_NSIS_MODIFY_PATH OFF)
  SET(CPACK_NSIS_INSTALL_ROOT "C:\\\\Emgu")
  
  IF(NOT NETFX_CORE)
	ADD_SUBDIRECTORY(DebuggerVisualizer)
  ENDIF()

  if(WITH_FFMPEG)
    INSTALL(
  	FILES
  	"${UNMANAGED_LIBRARY_OUTPUT_PATH}/${OPENCV_FFMPEG_OUTPUT_NAME}.dll"
  	DESTINATION "libs/${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
  	COMPONENT libs
  	)
  endif()
  
  # ----------------------------------------------------------------------------
  #  Build the documents of Emgu CV
  # ----------------------------------------------------------------------------
  SET(EMGU_CV_DOCUMENTATION_BUILD OFF CACHE BOOL "Build Emgu CV Documentation")
  IF(EMGU_CV_DOCUMENTATION_BUILD)
    ADD_SUBDIRECTORY(miscellaneous)
  ENDIF()

  # ----------------------------------------------------------------------------
  #  Set if we should sign the managed assembly
  # ----------------------------------------------------------------------------  
  IF (WIN32)
	SET(EMGU_SIGN_ASSEMBLY OFF CACHE BOOL "If enabled, we will sign the managed assembly")
  ELSE()
	SET(EMGU_SIGN_ASSEMBLY ON CACHE BOOL "If enabled, we will sign the managed assembly")
  ENDIF()
  
  # ----------------------------------------------------------------------------
  #  Build the extra components of Emgu CV
  # ----------------------------------------------------------------------------
  SET(EMGU_CV_EXTRA_BUILD OFF CACHE BOOL "Build Emgu CV Extra")
  IF(EMGU_CV_EXTRA_BUILD)
    ADD_SUBDIRECTORY(Emgu.RPC)
  ENDIF()
	
  # ----------------------------------------------------------------------------
  #  Include the version_str.inc files in the package
  # ----------------------------------------------------------------------------
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/libs
    DESTINATION .
    COMPONENT emgucv_binary
    FILES_MATCHING 
    PATTERN "*version_string.inc"
    PATTERN ".git" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMake*" EXCLUDE
    )
  
  #WINDOWS STORE EXAMPLE
  IF(HAVE_WINSTORE_10_X86 OR HAVE_WINSTORE_10_X64 OR HAVE_WINSTORE_10_ARM)  
    INSTALL(
      DIRECTORY
      ${CMAKE_CURRENT_SOURCE_DIR}/Solution
      DESTINATION .
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Example.Windows.UWP.sln"
	  PATTERN "Windows.UWP/packages/repositories.config"
      PATTERN ".git" EXCLUDE
      PATTERN "bin" EXCLUDE
      PATTERN "Android" EXCLUDE
      PATTERN "iOS" EXCLUDE
      PATTERN "Windows.Phone" EXCLUDE
      PATTERN "Windows.Desktop" EXCLUDE
      PATTERN "CrossPlatform" EXCLUDE
	  PATTERN "Mac" EXCLUDE
      )
  ENDIF()
  
  #WIN32 and not NETFX_CORE solution files
  IF (HAVE_WINDESKTOP)
    INSTALL(
      DIRECTORY
      ${CMAKE_CURRENT_SOURCE_DIR}/Solution
      DESTINATION .
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.sln"
      PATTERN "Emgu.CV.DebuggerVisualizers.sln"
      PATTERN ".git" EXCLUDE
      PATTERN "bin" EXCLUDE
      PATTERN "Android" EXCLUDE
      PATTERN "iOS" EXCLUDE
      PATTERN "CrossPlatform" EXCLUDE
	  PATTERN "Mac" EXCLUDE
	  PATTERN "Windows.UWP" EXCLUDE
      )
    INSTALL(
      DIRECTORY
      ${CMAKE_CURRENT_SOURCE_DIR}/Solution
      DESTINATION .
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Example.sln"
      PATTERN ".git" EXCLUDE
      PATTERN "bin" EXCLUDE
      PATTERN "Android" EXCLUDE
      PATTERN "iOS" EXCLUDE
      PATTERN "Windows.UWP" EXCLUDE
      PATTERN "CrossPlatform" EXCLUDE
	  PATTERN "Mac" EXCLUDE
      )
    
  ENDIF()
  
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
  #  Build the package
  # ----------------------------------------------------------------------------
  
  set(CPACK_COMPONENTS_ALL 
    libs #opencv components 
    emgucv_binary 
    emgucv_source
    emgucv_example_source
    )
  
  set(CPACK_PACKAGE_EXECUTABLES "" "") #http://public.kitware.com/Bug/view.php?id=7828
  
  SET(CPACK_NSIS_CONTACT "support@emgu.com")
  
  # Define MUI_TEMP that will be used for uninstalling menulinks
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n !insertmacro MUI_STARTMENU_GETFOLDER Application $MUI_TEMP")
  
  # ----------------------------------------------------------------------------
  #  Add menu link for documentations
  # ----------------------------------------------------------------------------
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateDirectory \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Documentation\\\" ")
  
  #SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Documentation\\\\Open CV Documentation.lnk\\\"  \\\"$INSTDIR\\\\doc\\\\opencv2refman.pdf\\\" ")	
  #SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Documentation\\\\Open CV Documentation.lnk\\\" ")
  
  IF(EMGU_CV_DOCUMENTATION_BUILD)
    LIST(APPEND CPACK_COMPONENTS_ALL emgucv_document)
    set(CPACK_COMPONENT_EMGUCV_DOCUMENT_DISPLAY_NAME "Emgu CV Documentation")
    set(CPACK_COMPONENT_EMGUCV_DOCUMENT_DEPENDS emgucv_binary)
    SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Documentation\\\\Emgu CV Documentation.lnk\\\"  \\\"$INSTDIR\\\\Emgu.CV.Documentation.chm\\\" ")	
    SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Documentation\\\\Emgu CV Documentation.lnk\\\" ")
  ENDIF()
  
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\nRMDir  \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Documentation\\\" ")
  
  # ----------------------------------------------------------------------------
  #  Add menu link for web sites
  # ----------------------------------------------------------------------------
  LIST(APPEND CPACK_NSIS_MENU_LINKS "https://www.emgu.com" "Emgu CV wiki")
  LIST(APPEND CPACK_NSIS_MENU_LINKS "${GITHUB_REPO_URL}/discussions" "Discussions")
  LIST(APPEND CPACK_NSIS_MENU_LINKS "${GITHUB_REPO_URL}/issues" "Issues Tracking")
  
  # ----------------------------------------------------------------------------
  #  Add menu link for Visual Studio solutions 
  # ----------------------------------------------------------------------------
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateDirectory \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Visual Studio Solution\\\" ")
  
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Visual Studio Solution\\\\Visual Studio 2017 - 2022 Examples.lnk\\\"  \\\"$INSTDIR\\\\Solution\\\\Windows.Desktop\\\\Emgu.CV.Example.sln\\\" ")	
  
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Visual Studio Solution\\\\Visual Studio 2017 - 2022 Examples.lnk\\\" ")
  
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\nRMDir  \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Visual Studio Solution\\\" ")
  
  # ----------------------------------------------------------------------------
  #  Add menu link for Licenses 
  # ----------------------------------------------------------------------------
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateDirectory \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\" ")
  
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\Emgu CV License.lnk\\\"  \\\"$INSTDIR\\\\${LICENSE_FILE_NAME}\\\" ")	
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\Open CV License.lnk\\\"  \\\"$INSTDIR\\\\lib\\\\opencv.license.txt\\\" ")
  IF(EMGU_CV_WITH_TESSERACT)
    SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\Tesseract OCR License.lnk\\\"  \\\"$INSTDIR\\\\lib\\\\tesseract-ocr.license.txt\\\" ")	
  ENDIF()
  
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\Emgu CV License.lnk\\\" ")
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\Open CV License.lnk\\\" ")
  IF(EMGU_CV_WITH_TESSERACT)
    SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\Tesseract OCR License.lnk\\\" ")
  ENDIF()
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\nRMDir  \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\" ")
  
  set(CPACK_COMPONENT_MAIN_DISPLAY_NAME "OpenCV Native Binary")
  set(CPACK_COMPONENT_MAIN_REQUIRED ON)
  set(CPACK_COMPONENT_EMGUCV_BINARY_DISPLAY_NAME "Emgu CV (Binary)")
  set(CPACK_COMPONENT_EMGUCV_BINARY_REQUIRED ON)
  set(CPACK_COMPONENT_EMGUCV_BINARY_DEPENDS libs)
  set(CPACK_COMPONENT_EMGUCV_SOURCE_DISPLAY_NAME "Emgu CV (Source)")
  set(CPACK_COMPONENT_EMGUCV_SOURCE_DEPENDS libs)
  set(CPACK_COMPONENT_EMGUCV_EXAMPLE_SOURCE_DISPLAY_NAME "Emgu CV Examples (Source)")
  set(CPACK_COMPONENT_EMGUCV_EXAMPLE_SOURCE_DEPENDS emgucv_source)
  
  
  IF (${CMAKE_VERSION} VERSION_GREATER "3.19.0" AND WIN32)
    STRING(REGEX REPLACE "/" "\\\\\\\\" WIN_CMAKE_COMMAND "${CMAKE_COMMAND}" )
	
    IF(EMGU_SIGN_FOUND)
	  #Sign the windows installer (.exe)
	  SET(NSIS_PACKAGE_FILE ${CMAKE_BINARY_DIR}/lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}.exe)
      #MESSAGE(STATUS "CPACK_PACKAGE_FILES: ${NSIS_PACKAGE_FILE}")  
	  STRING(REGEX REPLACE "/" "\\\\\\\\" WIN_EMGU_SIGN_EXECUTABLE "${EMGU_SIGN_EXECUTABLE}")
	  STRING(REGEX REPLACE "/" "\\\\\\\\" WIN_PACKAGE_FILE "${CMAKE_BINARY_DIR}/lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}.exe")
	  STRING(REGEX REPLACE "/" "\\\\\\\\" WIN_PACKAGE_FOLDER "${CMAKE_BINARY_DIR}/signed")
	  STRING(REGEX REPLACE "/" "\\\\\\\\" WIN_SIGNTOOL_EXECUTABLE "${SIGNTOOL_EXECUTABLE}")
	  #FILE(WRITE "${CMAKE_BINARY_DIR}/sign_package.txt" "add_custom_command(OUTPUT ${WIN_PACKAGE_FOLDER}/lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}.exe\n")
	  FILE(WRITE "${CMAKE_BINARY_DIR}/sign_package.cmake" "IF(EXISTS \"${CMAKE_BINARY_DIR}/lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}.exe\")\n")
	  FILE(APPEND "${CMAKE_BINARY_DIR}/sign_package.cmake" "  FILE(MAKE_DIRECTORY \"${CMAKE_BINARY_DIR}/signed\")\n")
	  #FILE(APPEND "${CMAKE_BINARY_DIR}/sign_package.cmake" "  EXECUTE_PROCESS(COMMAND \"${WIN_CMAKE_COMMAND}\" -E make_directory \"${CMAKE_BINARY_DIR}/signed\")\n")
	  FILE(APPEND "${CMAKE_BINARY_DIR}/sign_package.cmake" "  EXECUTE_PROCESS(COMMAND \"${WIN_EMGU_SIGN_EXECUTABLE}\" \"${WIN_PACKAGE_FILE}\" \"${WIN_PACKAGE_FOLDER}\" \"${WIN_SIGNTOOL_EXECUTABLE}\")\n")
	  FILE(APPEND "${CMAKE_BINARY_DIR}/sign_package.cmake" "ENDIF()\n")
      LIST(APPEND CPACK_POST_BUILD_SCRIPTS "${CMAKE_BINARY_DIR}/sign_package.cmake")
	  #LIST(APPEND CPACK_POST_BUILD_SCRIPTS "${WIN_EMGU_SIGN_EXECUTABLE} ${CMAKE_BINARY_DIR}/lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}.exe ${CMAKE_BINARY_DIR}/signed")
	ENDIF()
	
	IF (WITH_CUDA)
		FIND_PROGRAM (SEVEN_ZIP_EXECUTABLE
			NAMES 7z 
			PATHS
			$ENV{programfiles}/7-Zip
			CMAKE_FIND_ROOT_PATH_BOTH
			)
			
		IF (SEVEN_ZIP_EXECUTABLE)
		  MESSAGE(STATUS "Found 7z.exe: ${SEVEN_ZIP_EXECUTABLE}")
		  STRING(REGEX REPLACE "/" "\\\\\\\\" WIN_PACKAGE_ZIP_FILE "${CMAKE_BINARY_DIR}/lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}.zip")
		  STRING(REGEX REPLACE "/" "\\\\\\\\" WIN_SEVEN_ZIP_EXECUTABLE "${SEVEN_ZIP_EXECUTABLE}")
		  FILE(WRITE "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake" "EXECUTE_PROCESS(COMMAND echo \\\\${CPACK_PACKAGE_FILES})\n")
		  FILE(APPEND "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake" "IF (EXISTS \"${CMAKE_BINARY_DIR}/lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}.zip\")\n")
		  FILE(APPEND "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake" "  EXECUTE_PROCESS(COMMAND \"${WIN_CMAKE_COMMAND}\" -E rm -rf \"${CMAKE_BINARY_DIR}/lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}\")\n")
		  FILE(APPEND "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake" "  EXECUTE_PROCESS(COMMAND \"${WIN_CMAKE_COMMAND}\" -E rm -f \"${WIN_PACKAGE_ZIP_FILE}.selfextract.exe\")\n")
		  FILE(APPEND "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake" "  EXECUTE_PROCESS(COMMAND \"${WIN_CMAKE_COMMAND}\" -E make_directory \"${CMAKE_BINARY_DIR}/lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}\")\n")
		  FILE(APPEND "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake" "  EXECUTE_PROCESS(COMMAND \"${WIN_SEVEN_ZIP_EXECUTABLE}\" x \"${WIN_PACKAGE_ZIP_FILE}\" -olib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION} -y)\n")
		  FILE(APPEND "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake" "  EXECUTE_PROCESS(COMMAND \"${WIN_SEVEN_ZIP_EXECUTABLE}\" a -sfx7z.sfx \"${WIN_PACKAGE_ZIP_FILE}.selfextract.exe\" lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION} -mx=9 -m0=LZMA2 -mmt=off -md=512m )\n")
		  
		  IF (EMGU_SIGN_FOUND)
			FILE(APPEND "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake" "  EXECUTE_PROCESS(COMMAND \"${WIN_EMGU_SIGN_EXECUTABLE}\" \"${WIN_PACKAGE_ZIP_FILE}.selfextract.exe\" \"${WIN_PACKAGE_FOLDER}\" \"${WIN_SIGNTOOL_EXECUTABLE}\")\n")
		  ENDIF()
		  
		  FILE(APPEND "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake" "ENDIF()\n")
		  LIST(APPEND CPACK_POST_BUILD_SCRIPTS "${CMAKE_BINARY_DIR}/convert_to_7zip.cmake")
		ENDIF()
	ENDIF()

  ENDIF()
