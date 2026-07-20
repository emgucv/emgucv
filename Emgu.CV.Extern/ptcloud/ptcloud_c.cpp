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

cv::Odometry* cveOdometryCreate(int odometryType)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return new cv::Odometry(static_cast<cv::OdometryType>(odometryType));
#else
	throw_no_ptcloud();
#endif
}

void cveOdometryRelease(cv::Odometry** ptr)
{
#ifdef HAVE_OPENCV_PTCLOUD
	delete *ptr;
	*ptr = nullptr;
#else
	throw_no_ptcloud();
#endif
}

bool cveOdometryCompute1(cv::Odometry* odometry, cv::_InputArray* srcFrame, cv::_InputArray* dstFrame, cv::_OutputArray* rt)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return odometry->compute(*srcFrame, *dstFrame, *rt);
#else
	throw_no_ptcloud();
#endif
}

bool cveOdometryCompute2(cv::Odometry* odometry, cv::_InputArray* srcDepthFrame, cv::_InputArray* srcRGBFrame, cv::_InputArray* dstDepthFrame, cv::_InputArray* dstRGBFrame, cv::_OutputArray* rt)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return odometry->compute(*srcDepthFrame, *srcRGBFrame, *dstDepthFrame, *dstRGBFrame, *rt);
#else
	throw_no_ptcloud();
#endif
}

cv::RgbdNormals* cveRgbdNormalsCreate(int rows, int cols, int depth, cv::_InputArray* K, int window_size, int method, cv::Algorithm** algorithm, cv::Ptr<cv::RgbdNormals>** sharedPtr)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::Ptr<cv::RgbdNormals> ptr = cv::RgbdNormals::create(rows, cols, depth, *K, window_size, 50.f, static_cast<cv::RgbdNormals::RgbdNormalsMethod>(method));
	*sharedPtr = new cv::Ptr<cv::RgbdNormals>(ptr);
	*algorithm = nullptr;
	return ptr.get();
#else
	throw_no_ptcloud();
#endif
}

void cveRgbdNormalsRelease(cv::Ptr<cv::RgbdNormals>** sharedPtr)
{
#ifdef HAVE_OPENCV_PTCLOUD
	delete *sharedPtr;
	*sharedPtr = nullptr;
#else
	throw_no_ptcloud();
#endif
}

void cveRgbdNormalsApply(cv::RgbdNormals* rgbdNormals, cv::_InputArray* points, cv::_OutputArray* normals)
{
#ifdef HAVE_OPENCV_PTCLOUD
	rgbdNormals->apply(*points, *normals);
#else
	throw_no_ptcloud();
#endif
}

cv::Octree* cveOctreeCreate(std::vector<cv::Point3f>* pointCloud, int maxDepth, cv::Ptr<cv::Octree>** sharedPtr)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::Ptr<cv::Octree> ptr = cv::Octree::createWithDepth(maxDepth, *pointCloud);
	*sharedPtr = new cv::Ptr<cv::Octree>(ptr);
	return ptr.get();
#else
	throw_no_ptcloud();
#endif
}

void cveOctreeRelease(cv::Ptr<cv::Octree>** sharedPtr)
{
#ifdef HAVE_OPENCV_PTCLOUD
	delete *sharedPtr;
	*sharedPtr = nullptr;
#else
	throw_no_ptcloud();
#endif
}

void cveDepthTo3d(cv::_InputArray* depth, cv::_InputArray* K, cv::_OutputArray* points3d, cv::_InputArray* mask)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::depthTo3d(*depth, *K, *points3d, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
#else
	throw_no_ptcloud();
#endif
}

void cveDepthTo3dSparse(cv::_InputArray* depth, cv::_InputArray* inK, cv::_InputArray* inPoints, cv::_OutputArray* points3d)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::depthTo3dSparse(*depth, *inK, *inPoints, *points3d);
#else
	throw_no_ptcloud();
#endif
}

void cveRescaleDepth(cv::_InputArray* in, int type, cv::_OutputArray* out, double depthFactor)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::rescaleDepth(*in, type, *out, depthFactor);
#else
	throw_no_ptcloud();
#endif
}

void cveRegisterDepth(
	cv::_InputArray* unregisteredCameraMatrix,
	cv::_InputArray* registeredCameraMatrix,
	cv::_InputArray* registeredDistCoeffs,
	cv::_InputArray* Rt,
	cv::_InputArray* unregisteredDepth,
	CvSize* outputImagePlaneSize,
	cv::_OutputArray* registeredDepth,
	bool depthDilation)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::registerDepth(
		*unregisteredCameraMatrix,
		*registeredCameraMatrix,
		*registeredDistCoeffs,
		*Rt,
		*unregisteredDepth,
		cv::Size(outputImagePlaneSize->width, outputImagePlaneSize->height),
		*registeredDepth,
		depthDilation);
#else
	throw_no_ptcloud();
#endif
}

void cveWarpFrame(
	cv::_InputArray* depth,
	cv::_InputArray* image,
	cv::_InputArray* mask,
	cv::_InputArray* Rt,
	cv::_InputArray* cameraMatrix,
	cv::_OutputArray* warpedDepth,
	cv::_OutputArray* warpedImage,
	cv::_OutputArray* warpedMask)
{
#ifdef HAVE_OPENCV_PTCLOUD
	cv::warpFrame(
		*depth,
		image ? *image : static_cast<cv::InputArray>(cv::noArray()),
		mask ? *mask : static_cast<cv::InputArray>(cv::noArray()),
		*Rt,
		*cameraMatrix,
		warpedDepth ? *warpedDepth : static_cast<cv::OutputArray>(cv::noArray()),
		warpedImage ? *warpedImage : static_cast<cv::OutputArray>(cv::noArray()),
		warpedMask ? *warpedMask : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_ptcloud();
#endif
}
