# - Try to find the platform type
#
# defines
#
# IS_UBUNTU - If the system is Ubuntu
# IS_RASPBIAN - If the system is raspbian
#
# copyright (c) 2009 - 2012 Canming Huang support@emgu.com

SET (IS_UBUNTU FALSE)
#SET (IS_RASPBIAN FALSE)

IF (UNIX AND NOT APPLE)
  find_program(LSB_RELEASE_EXEC lsb_release)
  IF(LSB_RELEASE_EXEC)
    execute_process(COMMAND ${LSB_RELEASE_EXEC} -is
      OUTPUT_VARIABLE LSB_RELEASE_ID_SHORT
      OUTPUT_STRIP_TRAILING_WHITESPACE
      )
    execute_process(COMMAND ${LSB_RELEASE_EXEC} -rs
      OUTPUT_VARIABLE LSB_RELEASE_RELEASE_SHORT
      OUTPUT_STRIP_TRAILING_WHITESPACE
      )
    IF (LSB_RELEASE_ID_SHORT STREQUAL "Ubuntu")
      SET(IS_UBUNTU TRUE)
    ELSEIF (LSB_RELEASE_ID_SHORT STREQUAL "Raspbian")
      SET(IS_RASPBIAN TRUE)
    ENDIF()
  ELSE()
    find_program(UNAME_EXEC uname)
    IF (UNAME_EXEC)
#      MESSAGE(status "------------------------- Found uname: ${UNAME_EXEC}")
      execute_process(COMMAND ${UNAME_EXEC} -a
        OUTPUT_VARIABLE UNAME_ALL
        OUTPUT_STRIP_TRAILING_WHITESPACE
	)
#      MESSAGE(status "------------------------- UNAME_ALL: ${UNAME_ALL}")
      IF ("${UNAME_ALL}" MATCHES ".*[Uu]buntu.*")
	 SET(IS_UBUNTU TRUE)
      ENDIF()
    ENDIF()
  ENDIF()
ENDIF()

MARK_AS_ADVANCED(IS_UBUNTU)
MARK_AS_ADVANCED(IS_RASPBIAN)
 
