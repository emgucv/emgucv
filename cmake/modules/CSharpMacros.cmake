# - This is a support module for easy Mono/C# handling with CMake
# It defines the following macros:
#
# ADD_CS_LIBRARY (<target> <source> [ALL])
# ADD_CS_EXECUTABLE (<target> <source> [ALL])
# INSTALL_GAC (<target>)
#
# Note that the order of the arguments is important (including "ALL").
# It is recommended that you quote the arguments, especially <source>, if
# you have more than one source file.
#
# You can optionally set the variable CS_FLAGS to tell the macros whether
# to pass additional flags to the compiler. This is particularly useful to
# set assembly references, unsafe code, etc... These flags are always reset
# after the target was added so you don't have to care about that.
#
# copyright (c) 2007 Arno Rehn arno@arnorehn.de
# copyright (c) 2008 Helio castro helio@kde.org
#
# Redistribution and use is allowed according to the terms of the GPL license.


# ----- support macros -----
MACRO(GET_CS_LIBRARY_TARGET_DIR)
	IF (NOT LIBRARY_OUTPUT_PATH)
		SET(CS_LIBRARY_TARGET_DIR ${CMAKE_CURRENT_BINARY_DIR})
	ELSE (NOT LIBRARY_OUTPUT_PATH)
		SET(CS_LIBRARY_TARGET_DIR ${LIBRARY_OUTPUT_PATH})
	ENDIF (NOT LIBRARY_OUTPUT_PATH)
ENDMACRO(GET_CS_LIBRARY_TARGET_DIR)

MACRO(GET_CS_EXECUTABLE_TARGET_DIR)
	IF (NOT EXECUTABLE_OUTPUT_PATH)
		SET(CS_EXECUTABLE_TARGET_DIR ${CMAKE_CURRENT_BINARY_DIR})
	ELSE (NOT EXECUTABLE_OUTPUT_PATH)
		SET(CS_EXECUTABLE_TARGET_DIR ${EXECUTABLE_OUTPUT_PATH})
	ENDIF (NOT EXECUTABLE_OUTPUT_PATH)
ENDMACRO(GET_CS_EXECUTABLE_TARGET_DIR)

MACRO(MAKE_PROPER_FILE_LIST source)
	FOREACH(file ${source})
		# first assume it's a relative path
		FILE(GLOB globbed ${CMAKE_CURRENT_SOURCE_DIR}/${file})
		IF(globbed)
			FILE(TO_NATIVE_PATH ${CMAKE_CURRENT_SOURCE_DIR}/${file} native)
		ELSE(globbed)
			FILE(TO_NATIVE_PATH ${file} native)
		ENDIF(globbed)
		SET(proper_file_list ${proper_file_list} ${native})
		SET(native "")
	ENDFOREACH(file)
ENDMACRO(MAKE_PROPER_FILE_LIST)
# ----- end support macros -----

MACRO(ADD_CS_LIBRARY target source)
	GET_CS_LIBRARY_TARGET_DIR()
	
	SET(target_DLL "${CS_LIBRARY_TARGET_DIR}/${target}.dll")
	MAKE_PROPER_FILE_LIST("${source}")
	FILE(RELATIVE_PATH relative_path ${CMAKE_BINARY_DIR} ${target_DLL})
	
	ADD_CUSTOM_COMMAND (OUTPUT ${target_DLL}
		COMMAND ${GMCS_EXECUTABLE} ${CS_FLAGS} -out:${target_DLL} -target:library ${proper_file_list}
		DEPENDS ${source}
		COMMENT "Building ${relative_path}")
	ADD_CUSTOM_TARGET (${target} ${ARGV2} DEPENDS ${target_DLL})
	SET(relative_path "")
	SET(proper_file_list "")
	SET(CS_FLAGS "")
ENDMACRO(ADD_CS_LIBRARY)

MACRO(ADD_CS_EXECUTABLE target source)
	GET_CS_EXECUTABLE_TARGET_DIR()
	
	# FIXME:
	# Seems like cmake doesn't like the ".exe" ending for custom commands.
	# If we call it ${target}.exe, 'make' will later complain about a missing rule.
	# mono doesn't care about endings, so temporarily add ".monoexe".
	SET(target_EXE "${CS_EXECUTABLE_TARGET_DIR}/${target}.monoexe")
	MAKE_PROPER_FILE_LIST("${source}")
	FILE(RELATIVE_PATH relative_path ${CMAKE_BINARY_DIR} ${target_EXE})
	
	ADD_CUSTOM_COMMAND (OUTPUT "${target_EXE}"
		COMMAND ${GMCS_EXECUTABLE} ${CS_FLAGS} -out:${target_EXE} ${proper_file_list}
		DEPENDS ${source}
		COMMENT "Building ${relative_path}")
	ADD_CUSTOM_TARGET ("${target}" "${ARGV2}" DEPENDS "${target_EXE}")
	SET(relative_path "")
	SET(proper_file_list "")
	SET(CS_FLAGS "")
ENDMACRO(ADD_CS_EXECUTABLE)

MACRO(INSTALL_GAC target)
	GET_CS_LIBRARY_TARGET_DIR()

	IF(NOT WIN32)
		INCLUDE(FindPkgConfig)
		PKG_SEARCH_MODULE(MONO_CECIL mono-cecil)
		if(MONO_CECIL_FOUND)
			EXECUTE_PROCESS(COMMAND ${PKG_CONFIG_EXECUTABLE} mono-cecil --variable=assemblies_dir OUTPUT_VARIABLE GAC_ASSEMBLY_DIR OUTPUT_STRIP_TRAILING_WHITESPACE)
		endif(MONO_CECIL_FOUND)
		
		PKG_SEARCH_MODULE(CECIL cecil)
		if(CECIL_FOUND)
			EXECUTE_PROCESS(COMMAND ${PKG_CONFIG_EXECUTABLE} cecil --variable=assemblies_dir OUTPUT_VARIABLE GAC_ASSEMBLY_DIR OUTPUT_STRIP_TRAILING_WHITESPACE)
		endif(CECIL_FOUND)

		if(CECIL_FOUND OR MONO_CECIL_FOUND)
			INSTALL(CODE "EXECUTE_PROCESS(COMMAND ${GACUTIL_EXECUTABLE} -i ${CS_LIBRARY_TARGET_DIR}/${target}.dll -package 2.0 -root ${CMAKE_CURRENT_BINARY_DIR})")
			MAKE_DIRECTORY(${CMAKE_CURRENT_BINARY_DIR}/mono/)
			INSTALL(DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/mono/ DESTINATION ${GAC_ASSEMBLY_DIR} )
		endif(CECIL_FOUND OR MONO_CECIL_FOUND)
	ELSE(NOT WIN32)
		INSTALL(CODE "EXECUTE_PROCESS(COMMAND ${GACUTIL_EXECUTABLE} -i ${CS_LIBRARY_TARGET_DIR}/${target}.dll -package 2.0)")
	ENDIF(NOT WIN32)

ENDMACRO(INSTALL_GAC target)
