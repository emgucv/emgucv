# ---------------------------------------------------------------------------
# Merge all LLVM IR bitcode archives into a single cvextern.a
# ---------------------------------------------------------------------------
# Invoked as a POST_BUILD step for the cvextern target when building for
# Emscripten.  Variables passed in via -D from the add_custom_command:
#   LLVM_AR        - path to llvm-ar
#   BUILD_DIR      - CMAKE_BINARY_DIR (e.g. platforms/emscripten/build_dotnet)
#   SOURCE_DIR     - CMAKE_SOURCE_DIR (repo root)
#   OBJ_DIR        - directory containing cvextern object files
#                    (CMAKE_CURRENT_BINARY_DIR/CMakeFiles/cvextern.dir)
#
# Because we used llvm-ar (EMGU_CV_EMSCRIPTEN_LLVM_AR_PATH) as CMAKE_AR,
# all *.bc files are LLVM IR archives (llvm-ar format), not WASM relocatable
# objects.  WASM SjLj/EH lowering is deferred to the final non-relocatable
# Blazor link step (EmccExtraLDFlags: -flto -sSUPPORT_LONGJMP=wasm) where
# the LLVM 19 wasm-ld crash in --relocatable mode does not occur.
#
# Merge strategy: use 'llvm-ar qL' (quick-append + library-contents) to add
# every member from each source archive into the output archive WITHOUT
# replacing existing members.  This correctly handles:
#  - Cross-archive name conflicts (alloc.cpp.o in multiple modules)
#  - Intra-archive duplicate names (parallel.cpp.o x2 in libopencv_core.bc,
#    28 dups in liblibjpeg-turbo.bc from dispatch variants)
# 'q' never replaces; both copies land in the archive at different offsets.
# The final symbol table maps each exported symbol to the first member that
# defines it, so all needed definitions are found by the LTO linker.

set(OUTPUT_DIR "${SOURCE_DIR}/libs/webgl")
set(OUTPUT_FILE "${OUTPUT_DIR}/cvextern.a")

file(MAKE_DIRECTORY "${OUTPUT_DIR}")
file(REMOVE "${OUTPUT_FILE}")

# Step 1: Quick-append all members from each OpenCV and 3rdparty .bc archive.
file(GLOB BC_ARCHIVES
    "${BUILD_DIR}/bin/webgl/*.bc"
    "${BUILD_DIR}/opencv/3rdparty/lib/*.bc"
)
foreach(bc_archive ${BC_ARCHIVES})
    execute_process(
        COMMAND "${LLVM_AR}" qL "${OUTPUT_FILE}" "${bc_archive}"
    )
endforeach()

# Step 2: Append cvextern objects in batches to avoid ARG_MAX limits.
file(GLOB_RECURSE CVEXTERN_OBJECTS "${OBJ_DIR}/*.o")
set(BATCH "")
set(COUNT 0)
foreach(obj ${CVEXTERN_OBJECTS})
    list(APPEND BATCH "${obj}")
    math(EXPR COUNT "${COUNT} + 1")
    if(COUNT EQUAL 100)
        execute_process(COMMAND "${LLVM_AR}" q "${OUTPUT_FILE}" ${BATCH})
        set(BATCH "")
        set(COUNT 0)
    endif()
endforeach()
if(BATCH)
    execute_process(COMMAND "${LLVM_AR}" q "${OUTPUT_FILE}" ${BATCH})
endif()

# Step 3: Build the symbol table so the LTO linker can resolve symbols.
execute_process(COMMAND "${LLVM_AR}" s "${OUTPUT_FILE}")

message(STATUS "Done. Output: ${OUTPUT_FILE}")
