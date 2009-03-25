# - Try to find the gmcs and gacutil
#
# defines
#
# CSharp_FOUND - system has mono, mcs, gmcs and gacutil
# GMCS_PATH - where to find 'gmcs'
# GACUTIL_PATH - where to find 'gacutil'
#
# copyright (c) 2007 Arno Rehn arno@arnorehn.de
#
# Redistribution and use is allowed according to the terms of the GPL license.
#
# Modified by canming to find .NET on Windows
# copyright (c) 2009 Canming Huang emgucv@gmail.com

IF(WIN32)
FIND_PROGRAM (GMCS_EXECUTABLE csc c:/WINDOWS/Microsoft.NET/Framework/v2.0.50727 C:/Windows/Microsoft.NET/Framework/v3.5)
FIND_PROGRAM (AL_EXECUTABLE al c:/WINDOWS/Microsoft.NET/Framework/v2.0.50727 C:/Windows/Microsoft.NET/Framework/v3.5)
ELSE(WIN32)
FIND_PROGRAM (GMCS_EXECUTABLE gmcs)
FIND_PROGRAM (AL_EXECUTATBLE al)
ENDIF(WIN32)

FIND_PROGRAM (GACUTIL_EXECUTABLE gacutil "C:/Program Files/Microsoft SDKs/Windows/v6.0A/bin")

SET (CSharp_FOUND FALSE)

IF (GMCS_EXECUTABLE AND GACUTIL_EXECUTABLE AND AL_EXECUTABLE)
	SET (CSharp_FOUND TRUE)
ENDIF (GMCS_EXECUTABLE AND GACUTIL_EXECUTABLE AND AL_EXECUTABLE)

IF (CSharp_FOUND)
	IF (NOT CSharp_FIND_QUIETLY)
		MESSAGE(STATUS "Found gmcs: ${GMCS_EXECUTABLE}")
		MESSAGE(STATUS "Found gacutil: ${GACUTIL_EXECUTABLE}")
		MESSAGE(STATUS "Found al: ${AL_EXECUTABLE}")
	ENDIF (NOT CSharp_FIND_QUIETLY)
ELSE (CSharp_FOUND)
	IF (CSharp_FIND_REQUIRED)
		MESSAGE(FATAL_ERROR "Could not find one or more of the
following programs: gmcs, gacutil, al")
	ENDIF (CSharp_FIND_REQUIRED)
ENDIF (CSharp_FOUND)

MARK_AS_ADVANCED(GMCS_EXECUTABLE AL_EXECUTABLE GACUTIL_EXECUTABLE)


