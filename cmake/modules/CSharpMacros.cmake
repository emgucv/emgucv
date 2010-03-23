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
# copyright (c) 2009, 2010 Canming Huang emgucv@gmail.com
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

MACRO(GET_CS_EXECUTABLE_EXTENSION)
  IF (MSVC)
    SET(CS_EXECUTABLE_EXTENSION "exe")
  ELSE(MSVC)
    SET(CS_EXECUTABLE_EXTENSION "monoexe")
  ENDIF(MSVC)
ENDMACRO(GET_CS_EXECUTABLE_EXTENSION)


MACRO(ADD_CS_FILE_TO_DEPLOY file)
  GET_CS_LIBRARY_TARGET_DIR()

  IF ("${ARGV1}" STREQUAL "")
    SET(CS_FILE_TO_DEPLOY_DEST_PATH ${CS_LIBRARY_TARGET_DIR})
  ELSE()
    SET(CS_FILE_TO_DEPLOY_DEST_PATH "${CS_LIBRARY_TARGET_DIR}/${ARGV1}")
  ENDIF()
  SET(CS_PREBUILD_COMMAND 
    ${CS_PREBUILD_COMMAND} 
    COMMAND ${CMAKE_COMMAND} copy -E ${ARGV0} ${CS_FILE_TO_DEPLOY_DEST_PATH})
ENDMACRO(ADD_CS_FILE_TO_DEPLOY)

# ----- end support macros -----
MACRO(ADD_CS_MODULE target source)

ENDMACRO(ADD_CS_MODULE)

MACRO(COMPILE_CS target target_type source)
IF(${target_type} STREQUAL "library")
  GET_CS_LIBRARY_TARGET_DIR()
  SET(target_name "${CS_LIBRARY_TARGET_DIR}/${target}.dll")
  SET(COMPILE_CS_TARGET_DIR "${CS_LIBRARY_TARGET_DIR}")
ELSE(${target_type} STREQUAL "library")
  IF(${target_type} STREQUAL "winexe" OR ${target_type} STREQUAL "exe")
    GET_CS_EXECUTABLE_TARGET_DIR()
    GET_CS_EXECUTABLE_EXTENSION()
	SET(COMPILE_CS_TARGET_DIR "${CS_EXECUTABLE_TARGET_DIR}")
    # FIXME:
    # Seems like cmake doesn't like the ".exe" ending for custom commands.
    # If we call it ${target}.exe, 'make' will later complain about a missing rule.
    # mono doesn't care about endings, so temporarily add ".monoexe".
    SET(target_name "${CS_EXECUTABLE_TARGET_DIR}/${target}.${CS_EXECUTABLE_EXTENSION}")
  ELSE(${target_type} STREQUAL "winexe" OR ${target_type} STREQUAL "exe")
    #module
    GET_CS_LIBRARY_TARGET_DIR()
    SET(target_name "${CS_LIBRARY_TARGET_DIR}/${target}.netmodule")
	SET(COMPILE_CS_TARGET_DIR "${CS_LIBRARY_TARGET_DIR}")
  ENDIF(${target_type} STREQUAL "winexe" OR ${target_type} STREQUAL "exe")
ENDIF(${target_type} STREQUAL "library")

    MAKE_PROPER_FILE_LIST("${source}")
    FILE(RELATIVE_PATH relative_path ${CMAKE_BINARY_DIR} ${target_name})

    ADD_CUSTOM_TARGET (
      ${target} ${ARGV3}
      SOURCES ${source})
  
    #Make sure the destination folder exist
    SET(CS_PREBUILD_COMMAND 
      ${CS_PREBUILD_COMMAND} 
      COMMAND ${CMAKE_COMMAND} -E make_directory "${COMPILE_CS_TARGET_DIR}"
      )

	#enable optimization
	SET(CS_FLAGS "${CS_FLAGS} -optimize+")
	
	SET(TMP "-out:\"${target_name}\" -target:${target_type}")
	FOREACH(TMP_NAME ${CS_FLAGS})
	  SET(TMP "${TMP} ${TMP_NAME}")
	ENDFOREACH()
	FOREACH(TMP_NAME ${proper_file_list})
	  SET(TMP "${TMP} \"${TMP_NAME}\"")
	ENDFOREACH()
	
	FILE(WRITE ${CMAKE_CURRENT_SOURCE_DIR}/cscSourceList.rsp  "${TMP}")
	  
    ADD_CUSTOM_COMMAND (
      TARGET ${target}
      ${CS_PREBUILD_COMMAND}	   
      COMMAND ${CSC_EXECUTABLE} @cscSourceList.rsp
      DEPENDS ${source}
      COMMENT "Building ${relative_path}")

    SET(relative_path "")
    SET(proper_file_list "")
    SET(CS_FLAGS "")
    SET(CS_PREBUILD_COMMAND "")
ENDMACRO(COMPILE_CS)

MACRO(ADD_CS_REFERENCES references)
  FOREACH(ref ${references})
    SET(CS_FLAGS ${CS_FLAGS} -r:\"${ref}\")
  ENDFOREACH(ref)
ENDMACRO(ADD_CS_REFERENCES references)

MACRO(ADD_CS_PACKAGE_REFERENCES references)
  FOREACH(ref ${references})
    SET(CS_FLAGS ${CS_FLAGS} -pkg:${ref})
  ENDFOREACH(ref)
ENDMACRO(ADD_CS_PACKAGE_REFERENCES references)

MACRO(ADD_CS_RESOURCES resx resources)
  SET(CS_PREBUILD_COMMAND 
    ${CS_PREBUILD_COMMAND} 
    COMMAND ${RESGEN_EXECUTABLE} ${resx} ${resources}
    )
  SET(CS_FLAGS ${CS_FLAGS} -resource:\"${resources}\")
ENDMACRO(ADD_CS_RESOURCES)

MACRO(SIGN_ASSEMBLY key) 
  SET(CS_FLAGS ${CS_FLAGS} -keyfile:\"${key}\")
ENDMACRO(SIGN_ASSEMBLY)

MACRO(GENERATE_DOCUMENT file)
  SET(CS_FLAGS ${CS_FLAGS} -doc:\"${file}.xml\")
ENDMACRO(GENERATE_DOCUMENT)

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


