# Copies the signed MSIX installer (produced by AppxPackage under
# AppPackages/<Name>_<Version>_Test/*_x64.msix) to the repo's package/ staging
# folder. This needs to be a cmake -P script rather than a plain `copy`
# COMMAND because the AppPackages subfolder name embeds the manifest Version,
# and cmd.exe's `copy` only expands wildcards in the filename component of a
# path, not in a directory component -- file(GLOB ...) has no such limit.
#
# Expects -DSRC_DIR=<MauiDemoApp project dir> -DDEST_DIR=<repo>/package

FILE(GLOB MSIX_FILE "${SRC_DIR}/AppPackages/*/*_x64.msix")
IF(NOT MSIX_FILE)
  MESSAGE(FATAL_ERROR "No *_x64.msix found under ${SRC_DIR}/AppPackages -- did the MSIX publish step run?")
ENDIF()

FILE(MAKE_DIRECTORY "${DEST_DIR}")
FILE(COPY ${MSIX_FILE} DESTINATION "${DEST_DIR}")
MESSAGE(STATUS "Copied ${MSIX_FILE} to ${DEST_DIR}")
