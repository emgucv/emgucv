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
IF(MSVC AND CMAKE_CL_64)
  STRING(FIND "${CMAKE_GENERATOR}" "ARM" IS_ARM)
  IF(IS_ARM GREATER -1)
  ELSE()
      SET(TARGET_ARCH_64 TRUE)
  ENDIF()
ELSE()
  IF((CMAKE_SIZEOF_VOID_P EQUAL 8) AND NOT(APPLE AND CMAKE_OSX_ARCHITECTURES MATCHES "i386"))
    SET(TARGET_ARCH_64 TRUE)
  ENDIF() 
ENDIF()
ENDMACRO(CHECK_TARGET_ARCH)