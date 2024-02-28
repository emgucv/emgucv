# - Try to find the platform type
#
# defines
#
# IS_UBUNTU - If the system is Ubuntu
# IS_RASPBIAN - If the system is raspbian
#
# copyright (c) 2009 - 2012 Canming Huang support@emgu.com

SET (IS_UBUNTU FALSE)
SET (IS_DEBIAN FALSE)
SET (IS_RHEL FALSE)

IF (UNIX AND NOT APPLE)
  IF(${CMAKE_VERSION} VERSION_GREATER "3.22")
    cmake_host_system_information(RESULT DISTRO_ID QUERY DISTRIB_ID)
	MARK_AS_ADVANCED(DISTRO_ID)
    MESSAGE(STATUS "SYSTEM DISTRO_ID: ${DISTRO_ID}")
    IF(DISTRO_ID STREQUAL "rhel")
      SET(IS_RHEL TRUE)
    ELSEIF(DISTRO_ID STREQUAL "ol")
	  #Oracle Linux is based on RHEL
	  SET(IS_RHEL TRUE)
    ENDIF()
	MESSAGE(STATUS "IS_RHEL: ${IS_RHEL}")
  ENDIF()
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
      SET(IS_DEBIAN TRUE)
	ELSEIF (LSB_RELEASE_ID_SHORT STREQUAL "Debian")
	  SET(IS_DEBIAN TRUE)
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
MARK_AS_ADVANCED(IS_DEBIAN)
MARK_AS_ADVANCED(IS_RHEL) 
