//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_superres_c.h"

cv::dnn_superres::DnnSuperResImpl* cveDnnSuperResImplCreate()
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		return new cv::dnn_superres::DnnSuperResImpl();
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::dnn_superres::DnnSuperResImpl* cveDnnSuperResImplCreate2(cv::String* algo, int scale)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		return new cv::dnn_superres::DnnSuperResImpl(*algo, scale);
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveDnnSuperResImplSetModel(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* algo, int scale)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		dnnSuperRes->setModel(*algo, scale);
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveDnnSuperResImplReadModel1(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* path)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		dnnSuperRes->readModel(*path);
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveDnnSuperResImplReadModel2(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* weights, cv::String* definition)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		dnnSuperRes->readModel(*weights, *definition);
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveDnnSuperResImplUpsample(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, cv::_InputArray* img, cv::_OutputArray* result)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		dnnSuperRes->upsample(*img, *result);
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveDnnSuperResImplGetScale(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		return dnnSuperRes->getScale();
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDnnSuperResImplGetAlgorithm(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, cv::String* algorithm)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		std::string s = dnnSuperRes->getAlgorithm();
		*algorithm = s;
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveDnnSuperResImplRelease(cv::dnn_superres::DnnSuperResImpl** dnnSuperRes)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		delete* dnnSuperRes;
		*dnnSuperRes = 0;
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void cveDnnSuperResImplSetPreferableBackend(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, int backendId)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		dnnSuperRes->setPreferableBackend(backendId);
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveDnnSuperResImplSetPreferableTarget(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, int targetId)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_SUPERRES
		dnnSuperRes->setPreferableTarget(targetId);
#else
		throw_no_dnn_superres();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
