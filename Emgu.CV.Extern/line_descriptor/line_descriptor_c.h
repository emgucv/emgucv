//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_LINE_DESCRIPTOR_C_H
#define EMGU_LINE_DESCRIPTOR_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/line_descriptor.hpp"

CVAPI(cv::line_descriptor::BinaryDescriptor*) cveLineDescriptorBinaryDescriptorCreate();
CVAPI(void) cveLineDescriptorBinaryDescriptoyRelease(cv::line_descriptor::BinaryDescriptor** descriptor);
#endif