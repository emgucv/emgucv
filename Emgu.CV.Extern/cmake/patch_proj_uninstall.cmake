# Patch PROJ's CMakeLists.txt for embedding in a larger CMake build tree:
# Guard the "uninstall" custom target so it does not clash with OpenCV's.
# Invoked as PATCH_COMMAND with -DPROJ_SRC_DIR=<SOURCE_DIR>.
# The replacement is idempotent: if already applied it matches nothing and
# the file is left unchanged.

file(READ "${PROJ_SRC_DIR}/CMakeLists.txt" _content)

# Remove the block that sets CMAKE_CXX_STANDARD so the top-level project controls it.
string(REPLACE
    "# Set C++ version\n# Make CMAKE_CXX_STANDARD available as cache option overridable by user\nset(CMAKE_CXX_STANDARD 11\n  CACHE STRING \"C++ standard version to use (default is 11)\")\nmessage(STATUS \"Requiring C++\${CMAKE_CXX_STANDARD}\")\nset(CMAKE_CXX_STANDARD_REQUIRED ON)\nset(CMAKE_CXX_EXTENSIONS OFF)\nmessage(STATUS \"Requiring C++\${CMAKE_CXX_STANDARD} - done\")"
    ""
    _content "${_content}")

string(FIND "${_content}" "if(NOT TARGET uninstall)" _already_guarded)
if(_already_guarded EQUAL -1)
    string(REPLACE
        "add_custom_target(uninstall COMMAND"
        "if(NOT TARGET uninstall)\nadd_custom_target(uninstall COMMAND"
        _content "${_content}")
    string(REPLACE
        "proj_uninstall.cmake)"
        "proj_uninstall.cmake)\nendif()"
        _content "${_content}")
endif()

file(WRITE "${PROJ_SRC_DIR}/CMakeLists.txt" "${_content}")
