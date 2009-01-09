# ----------------------------------------------------------------------------
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvextern)

file(GLOB extern_srcs "./*.cpp")
source_group("Src" FILES ${extern_srcs})
file(GLOB extern_hdrs "./*.h*")
source_group("Include" FILES ${extern_hdrs})

set(the_target "cvextern")

add_library(${the_target} ${extern_srcs} ${extern_hdrs})

# Additional target properties
set_target_properties(${the_target} PROPERTIES
	DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin/"
	)


add_dependencies(${the_target} cxcore cv ml)

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} cxcore cv cvaux highgui ml)

