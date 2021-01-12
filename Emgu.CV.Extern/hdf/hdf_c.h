//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_HDF_C_H
#define EMGU_HDF_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_HDF
#include "opencv2/hdf.hpp"
#else
static inline CV_NORETURN void throw_no_hdf() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without hdf module support"); }

namespace cv {
	namespace hdf {
		class HDF5 {};
	}
}
#endif

CVAPI(cv::hdf::HDF5*) cveHDF5Create(
	cv::String* fileName,
	cv::Ptr<cv::hdf::HDF5>** sharedPtr);

CVAPI(void) cveHDF5Release(cv::Ptr<cv::hdf::HDF5>** hdfPtr);

CVAPI(void) cveHDF5GrCreate(cv::hdf::HDF5* hdf, cv::String* grlabel);
CVAPI(bool) cveHDF5HlExists(cv::hdf::HDF5* hdf, cv::String* label);
CVAPI(void) cveHDF5DsCreate(cv::hdf::HDF5* hdf, int rows, int cols, int type, cv::String* dslabel, int compresslevel, std::vector<int>* dims_chunks);
CVAPI(void) cveHDF5DsWrite(cv::hdf::HDF5* hdf, cv::_InputArray* Array, cv::String* dslabel);
CVAPI(void) cveHDF5DsRead(cv::hdf::HDF5* hdf, cv::_OutputArray* Array, cv::String* dslabel);

CVAPI(bool) cveHDF5AtExists(cv::hdf::HDF5* hdf, cv::String* atlabel);
CVAPI(void) cveHDF5AtDelete(cv::hdf::HDF5* hdf, cv::String* atlabel);
CVAPI(void) cveHDF5AtWriteInt(cv::hdf::HDF5* hdf, int value, cv::String* atlabel);
CVAPI(void) cveHDF5AtReadInt(cv::hdf::HDF5* hdf, int* value, cv::String* atlabel);
CVAPI(void) cveHDF5AtWriteDouble(cv::hdf::HDF5* hdf, double value, cv::String* atlabel);
CVAPI(void) cveHDF5AtReadDouble(cv::hdf::HDF5* hdf, double* value, cv::String* atlabel);
CVAPI(void) cveHDF5AtWriteString(cv::hdf::HDF5* hdf, cv::String* value, cv::String* atlabel);
CVAPI(void) cveHDF5AtReadString(cv::hdf::HDF5* hdf, cv::String* value, cv::String* atlabel);
CVAPI(void) cveHDF5AtReadArray(cv::hdf::HDF5* hdf, cv::_OutputArray* value, cv::String* atlabel);
CVAPI(void) cveHDF5AtWriteArray(cv::hdf::HDF5* hdf, cv::_InputArray* value, cv::String* atlabel);

CVAPI(void) cveHDF5KpRead(
	cv::hdf::HDF5* hdf, 
	std::vector<cv::KeyPoint>* keypoints, 
	cv::String* kplabel,
	int offset,
	int counts);
CVAPI(void) cveHDF5KpWrite(
	cv::hdf::HDF5* hdf,
	std::vector<cv::KeyPoint>* keypoints,
	cv::String* kplabel,
	int offset,
	int counts);

CVAPI(void) cveHDF5Close(cv::hdf::HDF5* hdf);

#endif