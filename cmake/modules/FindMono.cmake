# - Try to find the mono, mcs, gmcs and gacutil
#
# defines
#
# MONO_FOUND - system has mono, mcs, gmcs and gacutil
# MONO_PATH - where to find 'mono'
# MCS_PATH - where to find 'mcs'
# GMCS_PATH - where to find 'gmcs'
# GACUTIL_PATH - where to find 'gacutil'
#
# copyright (c) 2007 Arno Rehn arno@arnorehn.de
#
# Redistribution and use is allowed according to the terms of the GPL license.

FIND_PROGRAM (MONO_EXECUTABLE mono)
FIND_PROGRAM (MCS_EXECUTABLE mcs)
FIND_PROGRAM (GMCS_EXECUTABLE gmcs)
FIND_PROGRAM (GACUTIL_EXECUTABLE gacutil)

SET (MONO_FOUND FALSE)

IF (MONO_EXECUTABLE AND MCS_EXECUTABLE AND GMCS_EXECUTABLE AND GACUTIL_EXECUTABLE)
	SET (MONO_FOUND TRUE)
ENDIF (MONO_EXECUTABLE AND MCS_EXECUTABLE AND GMCS_EXECUTABLE AND GACUTIL_EXECUTABLE)

IF (MONO_FOUND)
	IF (NOT Mono_FIND_QUIETLY)
		MESSAGE(STATUS "Found mono: ${MONO_EXECUTABLE}")
		MESSAGE(STATUS "Found mcs: ${MCS_EXECUTABLE}")
		MESSAGE(STATUS "Found gmcs: ${GMCS_EXECUTABLE}")
		MESSAGE(STATUS "Found gacutil: ${GACUTIL_EXECUTABLE}")
	ENDIF (NOT Mono_FIND_QUIETLY)
ELSE (MONO_FOUND)
	IF (Mono_FIND_REQUIRED)
		MESSAGE(FATAL_ERROR "Could not find one or more of the following programs: mono, mcs, gmcs, gacutil")
	ENDIF (Mono_FIND_REQUIRED)
ENDIF (MONO_FOUND)

MARK_AS_ADVANCED(MONO_EXECUTABLE MCS_EXECUTABLE GMCS_EXECUTABLE GACUTIL_EXECUTABLE)
