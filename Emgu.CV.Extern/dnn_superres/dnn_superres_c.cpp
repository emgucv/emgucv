//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_superres_c.h"

cv::dnn_superres::DnnSuperResImpl* cveDnnSuperResImplCreate()
{
	return new cv::dnn_superres::DnnSuperResImpl();
}
void cveDnnSuperResImplSetModel(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* algo, int scale)
{
	dnnSuperRes->setModel(*algo, scale);
}
void cveDnnSuperResImplReadModel1(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* path)
{
	dnnSuperRes->readModel(*path);
}
void cveDnnSuperResImplReadModel2(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* weights, cv::String* definition)
{
	dnnSuperRes->readModel(*weights, *definition);
}
void cveDnnSuperResImplUpsample(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, cv::_InputArray* img, cv::_OutputArray* result)
{
	dnnSuperRes->upsample(*img, *result);
}
int cveDnnSuperResImplGetScale(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes)
{
	return dnnSuperRes->getScale();
}
void cveDnnSuperResImplGetAlgorithm(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, cv::String* algorithm)
{
	std::string s = dnnSuperRes->getAlgorithm();
	*algorithm = s;
}
void cveDnnSuperResImplRelease(cv::dnn_superres::DnnSuperResImpl** dnnSuperRes)
{
	delete* dnnSuperRes;
	*dnnSuperRes = 0;
}