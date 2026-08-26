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

  # Compile with the wasm-native exception-handling ABI (matching the .NET
  # WASM runtime's own -fwasm-exceptions build) instead of Emscripten's
  # legacy JS-exception default. Without this, C++ try/catch in cvextern.a
  # (e.g. CVAPI_CATCH_CV_ERRORS) never fires -- not even a catch(...) around
  # the exact throw site -- because the throw/catch personality routine
  # baked into cvextern.a's bitcode at compile time doesn't match the
  # wasm-native-EH ABI the final consuming project links against. This is a
  # frontend (compile-time) ABI commitment, unlike SjLj lowering above,
  # which is a pure backend transform and can stay safely deferred to the
  # final link.
  #
  # This is opt-out (EMGU_CV_EMSCRIPTEN_WASM_EXCEPTIONS) for the Unity WebGL
  # build: Unity's own final link (its GameAssembly.a + bundled runtime
  # modules) uses Emscripten's legacy JS-exception model
  # (-sDISABLE_EXCEPTION_CATCHING=0, no -fwasm-exceptions anywhere in
  # Unity's own emcc invocation), and mixing the two ABIs in one final link
  # crashes Unity's bundled LLVM 17 wasm-ld with a SIGSEGV inside its
  # "Expand indirectbr instructions" pass (seen deep in core OpenCV code,
  # e.g. cv::utils::getConfigurationParameterBool, not anything
  # module-specific). cvextern.a must match whatever exception ABI the
  # final consuming project uses -- for Unity that means the legacy model.
  IF(NOT DEFINED EMGU_CV_EMSCRIPTEN_WASM_EXCEPTIONS)
    SET(EMGU_CV_EMSCRIPTEN_WASM_EXCEPTIONS TRUE)
  ENDIF()
  IF(EMGU_CV_EMSCRIPTEN_WASM_EXCEPTIONS)
    SET(CMAKE_C_FLAGS "-fwasm-exceptions ${CMAKE_C_FLAGS}")
    SET(CMAKE_CXX_FLAGS "-fwasm-exceptions ${CMAKE_CXX_FLAGS}")
  ENDIF()

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
