//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "hdf_c.h"

cv::hdf::HDF5* cveHDF5Create(
	cv::String* fileName,
	cv::Ptr<cv::hdf::HDF5>** sharedPtr)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		cv::Ptr<cv::hdf::HDF5> ptr = cv::hdf::open(*fileName);
		*sharedPtr = new cv::Ptr<cv::hdf::HDF5>(ptr);
		return (*sharedPtr)->get();
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveHDF5Release(cv::Ptr<cv::hdf::HDF5>** hdfPtr)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		delete *hdfPtr;
		*hdfPtr = 0;
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveHDF5GrCreate(cv::hdf::HDF5* hdf, cv::String* grlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->grcreate(*grlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveHDF5HlExists(cv::hdf::HDF5* hdf, cv::String* label)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		return hdf->hlexists(*label);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveHDF5DsCreate(cv::hdf::HDF5* hdf, int rows, int cols, int type, cv::String* dslabel, int compresslevel, std::vector<int>* dims_chunks)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		if (dims_chunks)
		{
			hdf->dscreate(rows, cols, type, *dslabel, compresslevel, *dims_chunks);
		}
		else
		{
			hdf->dscreate(rows, cols, type, *dslabel, compresslevel);
		}
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5DsWrite(cv::hdf::HDF5* hdf, cv::_InputArray* array, cv::String* dslabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->dswrite(*array, *dslabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5DsRead(cv::hdf::HDF5* hdf, cv::_OutputArray* array, cv::String* dslabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->dsread(*array, *dslabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveHDF5AtExists(cv::hdf::HDF5* hdf, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		return hdf->atexists(*atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveHDF5AtDelete(cv::hdf::HDF5* hdf, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->atdelete(*atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5AtWriteInt(cv::hdf::HDF5* hdf, int value, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->atwrite(value, *atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5AtReadInt(cv::hdf::HDF5* hdf, int* value, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->atread(value, *atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5AtWriteDouble(cv::hdf::HDF5* hdf, double value, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->atwrite(value, *atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5AtReadDouble(cv::hdf::HDF5* hdf, double* value, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->atread(value, *atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5AtWriteString(cv::hdf::HDF5* hdf, cv::String* value, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->atwrite(*value, *atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5AtReadString(cv::hdf::HDF5* hdf, cv::String* value, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->atread(value, *atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5AtReadArray(cv::hdf::HDF5* hdf, cv::_OutputArray* value, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->atread(*value, *atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHDF5AtWriteArray(cv::hdf::HDF5* hdf, cv::_InputArray* value, cv::String* atlabel)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->atwrite(*value, *atlabel);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveHDF5KpRead(
	cv::hdf::HDF5* hdf,
	std::vector<cv::KeyPoint>* keypoints,
	cv::String* kplabel,
	int offset,
	int counts)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->kpread(*keypoints, *kplabel, offset, counts);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveHDF5KpWrite(
	cv::hdf::HDF5* hdf,
	std::vector<cv::KeyPoint>* keypoints,
	cv::String* kplabel,
	int offset,
	int counts)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->kpwrite(*keypoints, *kplabel, offset, counts);
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveHDF5Close(cv::hdf::HDF5* hdf)
{
	try
	{
#ifdef HAVE_OPENCV_HDF
		hdf->close();
#else
		throw_no_hdf();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
