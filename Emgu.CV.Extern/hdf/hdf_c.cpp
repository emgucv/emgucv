//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "hdf_c.h"

cv::hdf::HDF5* cveHDF5Create(
	cv::String* fileName,
	cv::Ptr<cv::hdf::HDF5>** sharedPtr)
{
#ifdef HAVE_OPENCV_HDF
	cv::Ptr<cv::hdf::HDF5> ptr = cv::hdf::open(*fileName);
	*sharedPtr = new cv::Ptr<cv::hdf::HDF5>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_hdf();
#endif
}

void cveHDF5Release(cv::Ptr<cv::hdf::HDF5>** hdfPtr)
{
#ifdef HAVE_OPENCV_HDF
	delete *hdfPtr;
	*hdfPtr = 0;
#else
	throw_no_hdf();
#endif
}

void cveHDF5GrCreate(cv::hdf::HDF5* hdf, cv::String* grlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->grcreate(*grlabel);
#else
	throw_no_hdf();
#endif
}

bool cveHDF5HlExists(cv::hdf::HDF5* hdf, cv::String* label)
{
#ifdef HAVE_OPENCV_HDF
	return hdf->hlexists(*label);
#else
	throw_no_hdf();
#endif
}
void cveHDF5DsCreate(cv::hdf::HDF5* hdf, int rows, int cols, int type, cv::String* dslabel, int compresslevel, std::vector<int>* dims_chunks)
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
void cveHDF5DsWrite(cv::hdf::HDF5* hdf, cv::_InputArray* array, cv::String* dslabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->dswrite(*array, *dslabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5DsRead(cv::hdf::HDF5* hdf, cv::_OutputArray* array, cv::String* dslabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->dsread(*array, *dslabel);
#else
	throw_no_hdf();
#endif
}

bool cveHDF5AtExists(cv::hdf::HDF5* hdf, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	return hdf->atexists(*atlabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5AtDelete(cv::hdf::HDF5* hdf, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->atdelete(*atlabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5AtWriteInt(cv::hdf::HDF5* hdf, int value, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->atwrite(value, *atlabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5AtReadInt(cv::hdf::HDF5* hdf, int* value, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->atread(value, *atlabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5AtWriteDouble(cv::hdf::HDF5* hdf, double value, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->atwrite(value, *atlabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5AtReadDouble(cv::hdf::HDF5* hdf, double* value, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->atread(value, *atlabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5AtWriteString(cv::hdf::HDF5* hdf, cv::String* value, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->atwrite(*value, *atlabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5AtReadString(cv::hdf::HDF5* hdf, cv::String* value, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->atread(value, *atlabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5AtReadArray(cv::hdf::HDF5* hdf, cv::_OutputArray* value, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->atread(*value, *atlabel);
#else
	throw_no_hdf();
#endif
}
void cveHDF5AtWriteArray(cv::hdf::HDF5* hdf, cv::_InputArray* value, cv::String* atlabel)
{
#ifdef HAVE_OPENCV_HDF
	hdf->atwrite(*value, *atlabel);
#else
	throw_no_hdf();
#endif
}

void cveHDF5KpRead(
	cv::hdf::HDF5* hdf,
	std::vector<cv::KeyPoint>* keypoints,
	cv::String* kplabel,
	int offset,
	int counts)
{
#ifdef HAVE_OPENCV_HDF
	hdf->kpread(*keypoints, *kplabel, offset, counts);
#else
	throw_no_hdf();
#endif
}

void cveHDF5KpWrite(
	cv::hdf::HDF5* hdf,
	std::vector<cv::KeyPoint>* keypoints,
	cv::String* kplabel,
	int offset,
	int counts)
{
#ifdef HAVE_OPENCV_HDF
	hdf->kpwrite(*keypoints, *kplabel, offset, counts);
#else
	throw_no_hdf();
#endif
}

void cveHDF5Close(cv::hdf::HDF5* hdf)
{
#ifdef HAVE_OPENCV_HDF
	hdf->close();
#else
	throw_no_hdf();
#endif
}
