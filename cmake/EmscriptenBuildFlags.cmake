# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  LTO/archiver flags for the Emscripten (WebAssembly) toolchain.
#  Included from the root CMakeLists.txt; CMAKE_CURRENT_SOURCE_DIR still
#  refers to the repository root here (include() does not change it).
# ----------------------------------------------------------------------------

IF("${CMAKE_SYSTEM_NAME}" STREQUAL "Emscripten")
  SET(CMAKE_C_FLAGS_RELEASE "-flto ${CMAKE_C_FLAGS_RELEASE}")
  SET(CMAKE_CXX_FLAGS_RELEASE "-flto ${CMAKE_CXX_FLAGS_RELEASE}")
  SET(CMAKE_C_FLAGS_DEBUG "-flto ${CMAKE_C_FLAGS_DEBUG}")
  SET(CMAKE_CXX_FLAGS_DEBUG "-flto ${CMAKE_CXX_FLAGS_DEBUG}")
  SET(CMAKE_EXE_LINKER_FLAGS "-flto ${CMAKE_EXE_LINKER_FLAGS}")
  SET(CMAKE_SHARED_LINKER_FLAGS "-flto ${CMAKE_SHARED_LINKER_FLAGS}")
  #SET(CMAKE_STATIC_LINKER_FLAGS "-flto ${CMAKE_STATIC_LINKER_FLAGS}")

  IF(EMGU_CV_EMSCRIPTEN_LLVM_AR_PATH)
    # Use llvm-ar to create LLVM IR bitcode archives.  This defers WASM
    # instruction-selection (including SjLj/EH lowering) to the final
    # non-relocatable link step, avoiding LLVM 19 wasm-ld crashes that occur
    # when -wasm-enable-sjlj + -exception-model=wasm are passed to
    # wasm-ld --relocatable (emcc -flto -r).
    set(CMAKE_AR "${EMGU_CV_EMSCRIPTEN_LLVM_AR_PATH}")
    set(CMAKE_RANLIB "true")
    set(CMAKE_STATIC_LIBRARY_SUFFIX ".bc")
    set(CMAKE_C_CREATE_STATIC_LIBRARY "<CMAKE_AR> rcs <TARGET> <OBJECTS>")
    set(CMAKE_CXX_CREATE_STATIC_LIBRARY "<CMAKE_AR> rcs <TARGET> <OBJECTS>")
  ELSE()
    set(CMAKE_AR "emcc")
    set(CMAKE_STATIC_LIBRARY_SUFFIX ".bc")
    set(CMAKE_C_CREATE_STATIC_LIBRARY "<CMAKE_AR> -flto -r -o <TARGET> <LINK_FLAGS> <OBJECTS>")
    set(CMAKE_CXX_CREATE_STATIC_LIBRARY "<CMAKE_AR> -flto -r -o <TARGET> <LINK_FLAGS> <OBJECTS>")
  ENDIF()

ENDIF()
