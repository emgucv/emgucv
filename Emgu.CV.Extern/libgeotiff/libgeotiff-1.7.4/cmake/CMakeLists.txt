#############################################################################
# Config file generation and installation
#############################################################################

# geotiff-config.cmake for the install tree.  It's installed in
# ${INSTALL_CMAKE_DIR} and @PROJECT_ROOT_DIR@ is the relative
# path to the root from there.  (Note that the whole install tree can
# be relocated.)
if (NOT WIN32)
  set (INSTALL_CMAKE_DIR "share/cmake/${PROJECT_NAME}")
  set (PROJECT_ROOT_DIR "../../..")
else ()
  set (INSTALL_CMAKE_DIR "cmake")
  set (PROJECT_ROOT_DIR "..")
endif ()

configure_file (project-config.cmake.in project-config.cmake @ONLY)
configure_file (project-config-version.cmake.in
  project-config-version.cmake @ONLY)
install (FILES
  "${CMAKE_CURRENT_BINARY_DIR}/project-config.cmake"
  DESTINATION "${INSTALL_CMAKE_DIR}"
  RENAME "${PROJECT_NAME_LOWER}-config.cmake")
install (FILES
  "${CMAKE_CURRENT_BINARY_DIR}/project-config-version.cmake"
  DESTINATION "${INSTALL_CMAKE_DIR}"
  RENAME "${PROJECT_NAME_LOWER}-config-version.cmake")
# Make information about the cmake targets (the library and the tools)
# available.
install (EXPORT depends
  FILE ${PROJECT_NAME_LOWER}-depends.cmake
  DESTINATION "${INSTALL_CMAKE_DIR}")


#  ------------------------------------------------------------------------------
  # CPACK controls
  #------------------------------------------------------------------------------


  SET(CPACK_PACKAGE_VERSION_MAJOR ${PROJECT_VERSION_MAJOR})
  SET(CPACK_PACKAGE_VERSION_MINOR ${PROJECT_VERSION_MINOR})
  SET(CPACK_PACKAGE_VERSION_PATCH ${PROJECT_VERSION_PATCH})
  SET(CPACK_PACKAGE_NAME "GeoTIFF")

  SET(CPACK_SOURCE_GENERATOR "TBZ2;TGZ;ZIP")
  SET(CPACK_PACKAGE_VENDOR "GeoTIFF Development Team")
  SET(CPACK_RESOURCE_FILE_LICENSE    "${PROJECT_SOURCE_DIR}/LICENSE")

  set(CPACK_SOURCE_PACKAGE_FILE_NAME
      "lib${PROJECT_NAME_LOWER}-${GeoTIFF_VERSION}")

  set(CPACK_SOURCE_IGNORE_FILES
    "/\\\\.gitattributes"
    "/\\\\.DS_Store"
    "/\\\\.git/"
    "\\\\.swp$"
    "~$"
    "\\\\.\\\\#"
    "/\\\\#"
    "CMakeScripts/"
    "COPYING-CMAKE-SCRIPTS"
    "_CPack_Packages"
    "cmake_install.cmake"
    ".gitignore"
    ".ninja"
    ".deps"
    "HOWTORELEASE.txt"
    "README"
    "build/"
    "CMakeFiles"
    "CTestTestfile.cmake"
    ".gz"
    ".bz2"
  )

  include(CPack)
  add_custom_target(dist COMMAND ${CMAKE_MAKE_PROGRAM} package_source)
