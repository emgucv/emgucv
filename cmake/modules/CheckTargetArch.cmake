# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
#
# - This is a support module for checking if the compiled target is 32/64bit
# It define the following macro:
#
# CHECK_TARGET_ARCH ()
#
# if the target is 64bit, the flag TARGET_ARCH_64 will be set
# --------------------------------------------------------

include(CheckSymbolExists)

MACRO(CHECK_TARGET_ARCH)

MESSAGE(STATUS "CMAKE_SYSTEM_NAME: ${CMAKE_SYSTEM_NAME}")
IF(MSVC AND CMAKE_CL_64)
  SET(TARGET_ARCH_64 TRUE)
ELSE()
  IF((CMAKE_SIZEOF_VOID_P EQUAL 8) AND NOT(APPLE AND CMAKE_OSX_ARCHITECTURES MATCHES "i386"))
    SET(TARGET_ARCH_64 TRUE)
  ENDIF() 
ENDIF()

STRING(FIND "${CMAKE_GENERATOR}" "ARM" IS_ARM)
IF(IS_ARM GREATER -1)
  SET(TARGET_ARM TRUE)
ENDIF()

#Check if ARM specific compiler predefined marco exist
SET(IS_ARM)
check_symbol_exists("__arm__" "" IS_ARM)
IF(IS_ARM)
  SET(TARGET_ARM TRUE)
ENDIF()

#Check if ARM64 specific compiler predefined marco exist
SET(IS_ARM64)
check_symbol_exists("__aarch64__" "" IS_ARM64)
IF(IS_ARM64)
  SET(TARGET_ARM TRUE)
  SET(TARGET_ARCH_64 TRUE)
ENDIF()

IF(MSVC)
  SET(IS_ARM)
  STRING(FIND "${MSVC_C_ARCHITECTURE_ID}" "ARM" IS_ARM)
  IF(IS_ARM GREATER -1)
    SET(TARGET_ARM TRUE)
  ENDIF()
ENDIF()

IF(IS_UBUNTU)
  IF(IS_ARM64)
    SET(TARGET_ARCH_NAME "ubuntu_arm64")
  ELSEIF(IS_ARM)
    SET(TARGET_ARCH_NAME "ubuntu_arm")
  ELSEIF(TARGET_ARCH_64)
    SET(TARGET_ARCH_NAME "ubuntu_x64")
  ELSE()
    SET(TARGET_ARCH_NAME "ubuntu_x86")
  ENDIF()
ELSEIF(IS_DEBIAN)
  IF(IS_ARM64)
    SET(TARGET_ARCH_NAME "debian_arm64")
  ELSEIF(IS_ARM)
    SET(TARGET_ARCH_NAME "debian_arm")
  ELSEIF(TARGET_ARCH_64)
    SET(TARGET_ARCH_NAME "debian_x64")
  ELSE()
    SET(TARGET_ARCH_NAME "debian_x86")
  ENDIF()
ELSEIF(TARGET_ARM)
  IF(TARGET_ARCH_64)
	SET(TARGET_ARCH_NAME "arm64")
  ELSE()
    SET(TARGET_ARCH_NAME "arm")
  ENDIF()
ELSEIF(WIN32)
  IF(TARGET_ARCH_64)
    SET(TARGET_ARCH_NAME "win64")
  ELSE()
    SET(TARGET_ARCH_NAME "win32")
  ENDIF()
ELSE()
  SET(TARGET_ARCH_NAME ${CMAKE_SYSTEM_NAME})
ENDIF()

#MESSAGE(STATUS "Building for ${TARGET_ARCH_NAME}")
ENDMACRO(CHECK_TARGET_ARCH)
