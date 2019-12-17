# --------------------------------------------------------
#  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
#
# - This is a support module for checking if the compiled target is 32/64bit
# It define the following macro:
#
# CHECK_TARGET_ARCH ()
#
# if the target is 64bit, the flag TARGET_ARCH_64 will be set
# --------------------------------------------------------

MACRO(CHECK_TARGET_ARCH)

STRING(FIND "${CMAKE_GENERATOR}" "ARM" IS_ARM)
IF(IS_ARM GREATER -1)
  SET(TARGET_ARM TRUE)
ENDIF()
STRING(FIND "${CMAKE_GENERATOR_PLATFORM}" "ARM" IS_ARM)
IF(IS_ARM GREATER -1)
  SET(TARGET_ARM TRUE)
ENDIF()
  
IF(MSVC AND CMAKE_CL_64)
  SET(TARGET_ARCH_64 TRUE)
ELSEIF((CMAKE_SIZEOF_VOID_P EQUAL 8) AND NOT(APPLE AND CMAKE_OSX_ARCHITECTURES MATCHES "i386"))    
    SET(TARGET_ARCH_64 TRUE)
ENDIF()

IF (TARGET_ARM)
  MESSAGE(STATUS "Building for ARM arch")
ELSE()
  MESSAGE(STATUS "Building for Intel arch")
ENDIF()

IF (TARGET_ARCH_64)
  MESSAGE(STATUS "Building for 64-bit arch")
ELSE()
  MESSAGE(STATUS "Building for 32-bit arch")
ENDIF()

ENDMACRO(CHECK_TARGET_ARCH)