//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ptcloud_c.h"

void cveLoadPointCloud(cv::String* filename, cv::_OutputArray* vertices, cv::_OutputArray* normals, cv::_OutputArray* rgb)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::loadPointCloud(
		*filename,
		*vertices,
		normals ? *normals : static_cast<cv::OutputArray>(cv::noArray()),
		rgb ? *rgb : static_cast<cv::OutputArray>(cv::noArray())
	);
#else
	throw_no_ptcloud();
#endif
}
void cveSavePointCloud(cv::String* filename, cv::_InputArray* vertices, cv::_InputArray* normals, cv::_InputArray* rgb)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::savePointCloud(
		*filename,
		*vertices,
		normals ? *normals : static_cast<cv::InputArray>(cv::noArray()),
		rgb ? *rgb : static_cast<cv::InputArray>(cv::noArray())
	);
#else
	throw_no_ptcloud();
#endif
}
void cveLoadMesh(
	cv::String* filename,
	cv::_OutputArray* vertices,
	cv::_OutputArray* indices,
	cv::_OutputArray* normals,
	cv::_OutputArray* colors,
	cv::_OutputArray* texCoords)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::loadMesh(
		*filename,
		*vertices,
		*indices,
		normals ? *normals : static_cast<cv::OutputArray>(cv::noArray()),
		colors ? *colors : static_cast<cv::OutputArray>(cv::noArray()),
		texCoords ? *texCoords : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_ptcloud();
#endif
}
void cveSaveMesh(
	cv::String* filename,
	cv::_InputArray* vertices,
	cv::_InputArray* indices,
	cv::_InputArray* normals,
	cv::_InputArray* colors,
	cv::_InputArray* texCoords)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::saveMesh(
		*filename,
		*vertices,
		*indices,
		normals ? *normals : static_cast<cv::InputArray>(cv::noArray()),
		colors ? *colors : static_cast<cv::InputArray>(cv::noArray()),
		texCoords ? *texCoords : static_cast<cv::InputArray>(cv::noArray())
	);
#else
	throw_no_ptcloud();
#endif
}
