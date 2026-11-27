# Combines the per-architecture .msix packages built by the Windows MAUI demo app's
# Example.Maui.DemoApp.Windows.Msix target (one dotnet publish per entry in
# MAUI_WINDOWS_PLATFORMS, see CMakeLists.txt) into a single .msixbundle via
# makeappx.exe. `makeappx bundle` requires every input package to sit directly in
# one folder (non-recursive) with nothing else .msix alongside it, and all packages
# must share the same Identity Name/Version but differ in architecture -- both of
# which are already guaranteed here since every per-arch build comes from the same
# Package.appxmanifest.in / AppxPackageName for this one release VERSION.
#
# Expects:
#   -DSRC_DIR=<MauiDemoApp project dir>
#   -DPLATFORMS=<semicolon-separated list, e.g. "x64;arm64;x86">
#   -DVERSION=<EmguCVPackageVersion, e.g. "5.1.0.6716">
#   -DMAKEAPPX_EXECUTABLE=<path to makeappx.exe>
#   -DOUTPUT_FILE=<full path of the .msixbundle to produce>

SET(_staging_dir "${SRC_DIR}/AppPackages/_bundle_staging")
FILE(REMOVE_RECURSE "${_staging_dir}")
FILE(MAKE_DIRECTORY "${_staging_dir}")

FOREACH(_plat ${PLATFORMS})
  # The AppPackages subfolder name embeds a build-mode suffix (e.g. "_Test" for the
  # sideload/unsigned test packaging mode this project uses, UapAppxPackageBuildMode
  # is never overridden to StoreUpload) that file(GLOB) tolerates without needing to
  # hardcode it, the same reasoning as CopyMsixToPackage.cmake's own glob.
  FILE(GLOB _src_msix
    "${SRC_DIR}/AppPackages/Emgu.CV.Maui.Demo.windows.${_plat}.${VERSION}*/Emgu.CV.Maui.Demo.windows.${_plat}.${VERSION}.msix")
  LIST(LENGTH _src_msix _src_msix_count)
  IF(NOT _src_msix_count EQUAL 1)
    MESSAGE(FATAL_ERROR "BundleMsixPackages: expected exactly one ${_plat} package for version ${VERSION} under ${SRC_DIR}/AppPackages, found ${_src_msix_count}: ${_src_msix}")
  ENDIF()
  FILE(COPY ${_src_msix} DESTINATION "${_staging_dir}")
ENDFOREACH()

IF(EXISTS "${OUTPUT_FILE}")
  FILE(REMOVE "${OUTPUT_FILE}")
ENDIF()

EXECUTE_PROCESS(
  COMMAND "${MAKEAPPX_EXECUTABLE}" bundle /d "${_staging_dir}" /p "${OUTPUT_FILE}" /o
  RESULT_VARIABLE _makeappx_rc
  OUTPUT_VARIABLE _makeappx_out
  ERROR_VARIABLE _makeappx_err
  )
MESSAGE(STATUS "${_makeappx_out}")
IF(NOT _makeappx_rc EQUAL 0)
  MESSAGE(FATAL_ERROR "makeappx bundle failed (exit ${_makeappx_rc}): ${_makeappx_err}")
ENDIF()

MESSAGE(STATUS "Created MSIX bundle: ${OUTPUT_FILE}")
