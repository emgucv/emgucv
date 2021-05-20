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
# copyright (c) 2009 - 2014 Canming Huang support@emgu.com
#
# Redistribution and use is allowed according to the terms of the GPL license.


# ----- support macros -----
MACRO(GET_CS_LIBRARY_TARGET_DIR)
  IF (NOT LIBRARY_OUTPUT_PATH)
    SET(CS_LIBRARY_TARGET_DIR ${CMAKE_CURRENT_BINARY_DIR})
  ELSE ()
    SET(CS_LIBRARY_TARGET_DIR ${LIBRARY_OUTPUT_PATH})
  ENDIF ()
ENDMACRO(GET_CS_LIBRARY_TARGET_DIR)

MACRO(GET_CS_EXECUTABLE_TARGET_DIR)
  IF (NOT EXECUTABLE_OUTPUT_PATH)
    SET(CS_EXECUTABLE_TARGET_DIR ${CMAKE_CURRENT_BINARY_DIR})
  ELSE ()
    SET(CS_EXECUTABLE_TARGET_DIR ${EXECUTABLE_OUTPUT_PATH})
  ENDIF ()
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
  IF (WIN32)
    SET(CS_EXECUTABLE_EXTENSION "exe")
  ELSE(WIN32)
    SET(CS_EXECUTABLE_EXTENSION "monoexe")
  ENDIF(WIN32)
ENDMACRO(GET_CS_EXECUTABLE_EXTENSION)

MACRO(ADD_CS_FILE_TO_DEPLOY file)
  GET_CS_LIBRARY_TARGET_DIR()
  
  IF ("${ARGV1}" STREQUAL "")
    SET(CS_FILE_TO_DEPLOY_DEST_PATH ${CS_LIBRARY_TARGET_DIR})
  ELSE()
    SET(CS_FILE_TO_DEPLOY_DEST_PATH "${CS_LIBRARY_TARGET_DIR}/${ARGV1}")
  ENDIF()
  LIST(APPEND
    CS_PREBUILD_COMMAND  
    COMMAND ${CMAKE_COMMAND} -E copy_if_different ${ARGV0} ${CS_FILE_TO_DEPLOY_DEST_PATH})
ENDMACRO(ADD_CS_FILE_TO_DEPLOY)

MACRO(ADD_CS_DIRECTORY_TO_DEPLOY dir)
  GET_CS_LIBRARY_TARGET_DIR()
  
  IF ("${ARGV1}" STREQUAL "")
    SET(CS_DIR_TO_DEPLOY_DEST_PATH ${CS_LIBRARY_TARGET_DIR})
  ELSE()
    SET(CS_DIR_TO_DEPLOY_DEST_PATH "${CS_LIBRARY_TARGET_DIR}/${ARGV1}")
  ENDIF()
  LIST(APPEND
    CS_PREBUILD_COMMAND  
    COMMAND ${CMAKE_COMMAND} -E copy_directory ${ARGV0} ${CS_DIR_TO_DEPLOY_DEST_PATH})
ENDMACRO(ADD_CS_DIRECTORY_TO_DEPLOY)

# ----- end support macros -----
MACRO(ADD_CS_MODULE target source)

ENDMACRO(ADD_CS_MODULE)

MACRO(ADD_CS_REFERENCES references)
  FOREACH(ref ${references})
    LIST(APPEND CS_FLAGS -r:\"${ref}\")
  ENDFOREACH(ref)
ENDMACRO(ADD_CS_REFERENCES)

MACRO(ADD_CS_FRAMEWORK_REFERENCES ver refs)
  #MESSAGE(STATUS "ADD_CS_FRAMEWORK_REFERENCES ver: ${ver}; refs: ${refs}")	
  SET(CSC_MSCORLIB_FOLDER "")
  IF("${ver}" STREQUAL "3.5")
    GET_FILENAME_COMPONENT(CSC_MSCORLIB_FOLDER ${CSC_MSCORLIB_35} DIRECTORY)
    SET(CSC_MSCORLIB_FOLDER "${CSC_MSCORLIB_FOLDER}/")
  ENDIF() 
  
  FOREACH(ref ${refs})
    #MESSAGE(STATUS "Adding ${ref} from ${refs}")
    LIST(APPEND CS_FLAGS -r:\"${CSC_MSCORLIB_FOLDER}${ref}\")
  ENDFOREACH()
ENDMACRO()

MACRO(SET_CS_TARGET_FRAMEWORK)
  SET(EXTRA_MACRO_ARGS ${ARGN})
  #MESSAGE(STATUS "SET_CS_TARGET_FRAMEWORK ${EXTRA_MACRO_ARGS}")
  #did we get the version string?
  list(LENGTH EXTRA_MACRO_ARGS NUM_EXTRA_ARGS)
  #MESSAGE(STATUS "Extra parameters: ${NUM_EXTRA_ARGS}")
  IF (${NUM_EXTRA_ARGS} GREATER 0)
    #MESSAGE(STATUS "GREATER than 0")
    LIST(GET EXTRA_MACRO_ARGS 0 version)
    #MESSAGE(STATUS "VERSION: ${version}")
  ELSE()
    SET(version "")
  ENDIF()
  
  IF("${version}" STREQUAL "3.5")
    LIST(APPEND CS_COMMANDLINE_FLAGS -noconfig )
    LIST(APPEND CS_FLAGS -nostdlib)
    LIST(APPEND FRAMEWORK_REFERENCES mscorlib.dll System.dll ${FRAMEWORK_REFERENCES})
  ENDIF()
  
  IF(NOT NETFX_CORE)
    LIST(APPEND FRAMEWORK_REFERENCES System.Core.dll System.Xml.dll System.Drawing.dll System.Data.dll System.ServiceModel.dll System.Xml.Linq.dll)
  ENDIF()
  #MESSAGE(STATUS "FRAMEWORK reference: ver: ${version}; ref: ${FRAMEWORK_REFERENCES}")
  ADD_CS_FRAMEWORK_REFERENCES("${version}" "${FRAMEWORK_REFERENCES}")
ENDMACRO(SET_CS_TARGET_FRAMEWORK)

SET(DEFAULT_CS_CONFIG "Release" CACHE STRING "Default C# build configuration")

MACRO(BUILD_CSPROJ target csproj_file extra_flags)
  IF(APPLE AND ("${EMGUCV_ARCH}" STREQUAL "x64"))
    SET(MAC_FRESH_SHELL_PREFIX env -i zsh)
  ENDIF()
  
  ADD_CUSTOM_TARGET (${target} ${ARGV3} SOURCES ${csproj_file} )
  
  #  IF (WIN32 AND MSVC AND NOT ("${CMAKE_VS_DEVENV_COMMAND}" STREQUAL ""))
  #    ADD_CUSTOM_COMMAND (
  #      TARGET ${target}
  #      COMMAND ${CMAKE_VS_DEVENV_COMMAND} /Build ${DEFAULT_CS_CONFIG} ${extra_flags} ${csproj_file}
  #      COMMENT "Building ${target} with ${CMAKE_VS_DEVENV_COMMAND}")
  IF(MSBUILD_EXECUTABLE)
    #MESSAGE(STATUS "Adding custom command: ${MSBUILD_EXECUTABLE} /t:Build /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${csproj_file}")
    ADD_CUSTOM_COMMAND (
      TARGET ${target}
      COMMAND ${MAC_FRESH_SHELL_PREFIX} ${MSBUILD_EXECUTABLE} /t:Build /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${csproj_file}
      COMMENT "Building ${target} with ${MSBUILD_EXECUTABLE}")
  ELSEIF (DOTNET_EXECUTABLE)
    ADD_CUSTOM_COMMAND (
      TARGET ${target}
      COMMAND ${MAC_FRESH_SHELL_PREFIX} "${DOTNET_EXECUTABLE}" build -c ${DEFAULT_CS_CONFIG} "${csproj_file}"
      COMMENT "Building ${target} with ${DOTNET_EXECUTABLE}")
  ELSE()
    MESSAGE(FATAL_ERROR "Neither Visual Studio, msbuild nor dotnot is found!")
  ENDIF()
ENDMACRO()

MACRO(BUILD_CSPROJ_IN_SOLUTION target solution_file project_name extra_flags)
  IF(APPLE AND ("${EMGUCV_ARCH}" STREQUAL "x64"))
    SET(MAC_FRESH_SHELL_PREFIX env -i zsh)
  ENDIF()
  ADD_CUSTOM_TARGET (${target} ${ARGV4})
  #  IF (WIN32 AND MSVC AND NOT ("${CMAKE_VS_DEVENV_COMMAND}" STREQUAL ""))
  #    ADD_CUSTOM_COMMAND (
  #      TARGET ${target}
  #      COMMAND ${CMAKE_VS_DEVENV_COMMAND} /Build ${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file} /project ${project_name}
  #      COMMENT "Building ${target} with ${CMAKE_VS_DEVENV_COMMAND}")
  #  ELSEIF(VSTOOL_EXECUTABLE)      
  #    IF ("${project_name}" STREQUAL "")
  #    ADD_CUSTOM_COMMAND (
  #      TARGET ${target}
  #      COMMAND ${MAC_FRESH_SHELL_PREFIX} ${MSBUILD_EXECUTABLE} -t:restore ${solution_file}
  #      COMMAND "${VSTOOL_EXECUTABLE}" build -t:Build -c:"${DEFAULT_CS_CONFIG}" ${extra_flags}${solution_file}
  #      COMMENT "Building ${target}")
  #    ELSE()
  #    ADD_CUSTOM_COMMAND (
  #      TARGET ${target}
  #      COMMAND ${MAC_FRESH_SHELL_PREFIX} ${MSBUILD_EXECUTABLE} -t:restore ${solution_file}
  #      COMMAND "${VSTOOL_EXECUTABLE}" build -t:Build -c:"${DEFAULT_CS_CONFIG}" ${extra_flags}${solution_file} -p:${project_name}
  #      COMMENT "Building ${target}")
  #    ENDIF()
  IF ("${project_name}" STREQUAL "")
	SET(MSBUILD_TARGET_OPTION "")
  ELSE()
	#MESSAGE(STATUS ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> msbuild_target_name: ${msbuild_target_name}")
    STRING(REGEX REPLACE "\\." "_" msbuild_target_name ${project_name})
	SET(MSBUILD_TARGET_OPTION /target:${msbuild_target_name})
  ENDIF()
  IF (DOTNET_EXECUTABLE)
    ADD_CUSTOM_COMMAND (
	TARGET ${target}
	COMMAND ${MAC_FRESH_SHELL_PREFIX} ${DOTNET_EXECUTABLE} msbuild -t:restore /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file}
	COMMAND ${MAC_FRESH_SHELL_PREFIX} ${DOTNET_EXECUTABLE} msbuild /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file} ${MSBUILD_TARGET_OPTION}
	COMMENT "Building ${target} with command: ${MAC_FRESH_SHELL_PREFIX} ${DOTNET_EXECUTABLE} msbuild /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file} ${MSBUILD_TARGET_OPTION}")
  ELSEIF(MSBUILD_EXECUTABLE)
    ADD_CUSTOM_COMMAND (
	TARGET ${target}
	COMMAND ${MAC_FRESH_SHELL_PREFIX} ${MSBUILD_EXECUTABLE} -t:restore /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file}
	COMMAND ${MAC_FRESH_SHELL_PREFIX} ${MSBUILD_EXECUTABLE} /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file} ${MSBUILD_TARGET_OPTION}
	COMMENT "Building ${target} with command: ${MSBUILD_EXECUTABLE} /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file} ${MSBUILD_TARGET_OPTION}")
  ELSE()
    MESSAGE(FATAL_ERROR "dotnet command is not found!")
  ENDIF()
ENDMACRO()

MACRO(MSBUILD_CSPROJ_IN_SOLUTION target solution_file project_name extra_flags)
  IF((APPLE AND ("${EMGUCV_ARCH}" STREQUAL "x64")) OR ("${CMAKE_SYSTEM_NAME}" STREQUAL "iOS"))
    # Mac OS targeting intel cpu, or iOS
    SET(MAC_FRESH_SHELL_PREFIX env -i zsh)
  ENDIF()
  ADD_CUSTOM_TARGET (${target} ${ARGV4})
  IF ("${project_name}" STREQUAL "")
	SET(MSBUILD_TARGET_OPTION "")
  ELSE()
	#MESSAGE(STATUS ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> msbuild_target_name: ${msbuild_target_name}")
    STRING(REGEX REPLACE "\\." "_" msbuild_target_name ${project_name})
	SET(MSBUILD_TARGET_OPTION /target:${msbuild_target_name})
  ENDIF()
  IF(MSBUILD_EXECUTABLE)
    ADD_CUSTOM_COMMAND (
	TARGET ${target}
	COMMAND ${MAC_FRESH_SHELL_PREFIX} ${MSBUILD_EXECUTABLE} -t:restore /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file}
	COMMAND ${MAC_FRESH_SHELL_PREFIX} ${MSBUILD_EXECUTABLE} /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file} ${MSBUILD_TARGET_OPTION}
	COMMENT "Building ${target} with command: ${MSBUILD_EXECUTABLE} /p:Configuration=${DEFAULT_CS_CONFIG} ${extra_flags} ${solution_file} ${MSBUILD_TARGET_OPTION}")
  ELSE()
    MESSAGE(FATAL_ERROR "msbuild command is not found!")
  ENDIF()
ENDMACRO()


MACRO(BUILD_DOTNET_PROJ target csproj_file extra_flags)
  ADD_CUSTOM_TARGET (${target} ${ARGV3})
  #IF(APPLE)
  #  SET(MAC_FRESH_SHELL_PREFIX env -i zsh)
  #ENDIF()
  
  IF (DOTNET_EXECUTABLE)
    ADD_CUSTOM_COMMAND (
      TARGET ${target}
      COMMAND ${MAC_FRESH_SHELL_PREFIX} "${DOTNET_EXECUTABLE}" build -c ${DEFAULT_CS_CONFIG} ${extra_flags} "${csproj_file}"
      COMMENT "Building ${target} with command: ${MAC_FRESH_SHELL_PREFIX} \"${DOTNET_EXECUTABLE}\" build -c ${DEFAULT_CS_CONFIG} ${extra_flags} \"${csproj_file}\"")
  ELSE()
    MESSAGE(FATAL_ERROR "DOTNET_EXECUTABLE not found!")
  ENDIF()
ENDMACRO()

MACRO(BUILD_NUGET_PACKAGE target csproj_file nuspec_file output_dir working_dir)
  IF(APPLE AND ("${EMGUCV_ARCH}" STREQUAL "x64"))
    SET(MAC_FRESH_SHELL_PREFIX env -i zsh)
  ENDIF()
  IF (DOTNET_EXECUTABLE)
    ADD_CUSTOM_TARGET(
      ${target} ALL
	  COMMAND ${MAC_FRESH_SHELL_PREFIX} ${DOTNET_EXECUTABLE} msbuild -t:restore /p:Configuration=${DEFAULT_CS_CONFIG} ${csproj_file}
      COMMAND ${DOTNET_EXECUTABLE} pack "${csproj_file}" -p:NuspecFile="${nuspec_file}" --no-build -o "${output_dir}"
      WORKING_DIRECTORY "${working_dir}"
	  COMMENT "Building ${target} with command: ${DOTNET_EXECUTABLE} pack \"${csproj_file}\" -p:NuspecFile=\"${nuspec_file}\" --no-build -o \"${output_dir}\""
    )
  ELSE()
    MESSAGE(FATAL_ERROR "DOTNET_EXECUTABLE not found!")
  ENDIF()
ENDMACRO()



MACRO(COMPILE_CS target target_type source)
  IF(${target_type} STREQUAL "library")
    GET_CS_LIBRARY_TARGET_DIR()
    SET(target_name "${CS_LIBRARY_TARGET_DIR}/${target}.dll")
    SET(COMPILE_CS_TARGET_DIR "${CS_LIBRARY_TARGET_DIR}")
  ELSE()
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
  ENDIF()
  
  MAKE_PROPER_FILE_LIST("${source}")
  FILE(RELATIVE_PATH relative_path ${CMAKE_BINARY_DIR} ${target_name})
  
  ADD_CUSTOM_TARGET (
    ${target} ${ARGV3}
    SOURCES ${source})
  
  #Make sure the destination folder exist
  LIST(APPEND
    CS_PREBUILD_COMMAND  
    COMMAND ${CMAKE_COMMAND} -E make_directory "${COMPILE_CS_TARGET_DIR}"
    )
  
  #enable optimization
  LIST(APPEND CS_FLAGS -optimize+)
  
  #set the output target
  SET(TMP "-out:\"${target_name}\" -target:${target_type}")
  #set the compiler flags
  FOREACH(TMP_NAME ${CS_FLAGS})
    SET(TMP "${TMP} ${TMP_NAME}")
  ENDFOREACH()
  
  
  #set the source files
  FOREACH(TMP_NAME ${proper_file_list})
    SET(TMP "${TMP} \"${TMP_NAME}\"")
  ENDFOREACH()
  FILE(WRITE ${CMAKE_CURRENT_BINARY_DIR}/${target}_SourceList.rsp  ${TMP})
  
  ADD_CUSTOM_COMMAND (
    TARGET ${target}
    ${CS_PREBUILD_COMMAND}	   
    COMMAND ${CSC_EXECUTABLE} ${CS_COMMANDLINE_FLAGS} @${target}_SourceList.rsp
    ${CS_POSTBUILD_COMMAND}
    DEPENDS ${source}
    COMMENT "Building ${relative_path}")
  
  SET(relative_path "")
  SET(proper_file_list "")
  SET(CS_FLAGS "")
  SET(CS_COMMANDLINE_FLAGS "")
  SET(CS_PREBUILD_COMMAND "")
  SET(CS_POSTBUILD_COMMAND "")
ENDMACRO(COMPILE_CS)

MACRO(ADD_CS_PACKAGE_REFERENCES references)
  FOREACH(ref ${references})
    LIST(APPEND CS_FLAGS -pkg:${ref})
  ENDFOREACH(ref)
ENDMACRO(ADD_CS_PACKAGE_REFERENCES references)

MACRO(ADD_CS_RESOURCES resx resources)
  LIST(APPEND
    CS_PREBUILD_COMMAND 
    COMMAND ${RESGEN_EXECUTABLE} \"${resx}\" \"${resources}\"
    )
  LIST(APPEND CS_FLAGS -resource:\"${resources}\")
  LIST(APPEND
    CS_POSTBUILD_COMMAND
    COMMAND ${CMAKE_COMMAND} -E remove \"${resources}\"
    )
ENDMACRO(ADD_CS_RESOURCES)

MACRO(SIGN_ASSEMBLY key) 
  IF(EMGU_SIGN_ASSEMBLY)
    LIST(APPEND CS_FLAGS -keyfile:\"${key}\")
  ENDIF()
ENDMACRO(SIGN_ASSEMBLY)

MACRO(GENERATE_DOCUMENT file)
  LIST(APPEND CS_FLAGS -doc:\"${file}.xml\")
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
