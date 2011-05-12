# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(tesseract_neural_networks)
file(GLOB tesseract_neural_networks_srcs "*.cpp")

file(GLOB tesseract_neural_networks_hdrs "*.h")
source_group("Src" FILES ${tesseract_neural_networks_srcs})
source_group("Include" FILES ${tesseract_neural_networks_hdrs})
add_definitions(-DUSE_STD_NAMESPACE -W0)
add_library(${PROJECT_NAME} STATIC ${tesseract_neural_networks_srcs} ${tesseract_neural_networks_hdrs})
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "tesseract")

