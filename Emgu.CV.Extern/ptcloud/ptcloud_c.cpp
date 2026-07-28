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

cv::OdometryFrame* cveOdometryFrameCreate(
	cv::_InputArray* depth,
	cv::_InputArray* image,
	cv::_InputArray* mask,
	cv::_InputArray* normals)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return new cv::OdometryFrame(
		depth ? *depth : static_cast<cv::InputArray>(cv::noArray()),
		image ? *image : static_cast<cv::InputArray>(cv::noArray()),
		mask ? *mask : static_cast<cv::InputArray>(cv::noArray()),
		normals ? *normals : static_cast<cv::InputArray>(cv::noArray()));
#else
	throw_no_ptcloud();
#endif
}
void cveOdometryFrameRelease(cv::OdometryFrame** ptr)
{
#ifdef HAVE_OPENCV_PTCLOUD
	delete *ptr;
	*ptr = nullptr;
#else
	throw_no_ptcloud();
#endif
}
void cveOdometryFrameGetImage(cv::OdometryFrame* frame, cv::_OutputArray* image)
{
#ifdef HAVE_OPENCV_PTCLOUD
	frame->getImage(*image);
#else
	throw_no_ptcloud();
#endif
}
void cveOdometryFrameGetGrayImage(cv::OdometryFrame* frame, cv::_OutputArray* image)
{
#ifdef HAVE_OPENCV_PTCLOUD
	frame->getGrayImage(*image);
#else
	throw_no_ptcloud();
#endif
}
void cveOdometryFrameGetDepth(cv::OdometryFrame* frame, cv::_OutputArray* depth)
{
#ifdef HAVE_OPENCV_PTCLOUD
	frame->getDepth(*depth);
#else
	throw_no_ptcloud();
#endif
}
void cveOdometryFrameGetProcessedDepth(cv::OdometryFrame* frame, cv::_OutputArray* depth)
{
#ifdef HAVE_OPENCV_PTCLOUD
	frame->getProcessedDepth(*depth);
#else
	throw_no_ptcloud();
#endif
}
void cveOdometryFrameGetMask(cv::OdometryFrame* frame, cv::_OutputArray* mask)
{
#ifdef HAVE_OPENCV_PTCLOUD
	frame->getMask(*mask);
#else
	throw_no_ptcloud();
#endif
}
void cveOdometryFrameGetNormals(cv::OdometryFrame* frame, cv::_OutputArray* normals)
{
#ifdef HAVE_OPENCV_PTCLOUD
	frame->getNormals(*normals);
#else
	throw_no_ptcloud();
#endif
}
int cveOdometryFrameGetPyramidLevels(cv::OdometryFrame* frame)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return frame->getPyramidLevels();
#else
	throw_no_ptcloud();
#endif
}
void cveOdometryFrameGetPyramidAt(cv::OdometryFrame* frame, cv::_OutputArray* img, int pyrType, size_t level)
{
#ifdef HAVE_OPENCV_PTCLOUD
	frame->getPyramidAt(*img, static_cast<cv::OdometryFramePyramidType>(pyrType), level);
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
	cv::Size* outputImagePlaneSize,
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

//----------------------------------------------------------------------------
// VolumeSettings
//----------------------------------------------------------------------------
cv::VolumeSettings* cveVolumeSettingsCreate(int volumeType)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return new cv::VolumeSettings(static_cast<cv::VolumeType>(volumeType));
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeSettingsRelease(cv::VolumeSettings** settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	delete *settings;
	*settings = nullptr;
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetIntegrateWidth(cv::VolumeSettings* settings, int val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setIntegrateWidth(val);
#else
	throw_no_ptcloud();
#endif
}
int cveVolumeSettingsGetIntegrateWidth(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getIntegrateWidth();
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeSettingsSetIntegrateHeight(cv::VolumeSettings* settings, int val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setIntegrateHeight(val);
#else
	throw_no_ptcloud();
#endif
}
int cveVolumeSettingsGetIntegrateHeight(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getIntegrateHeight();
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetRaycastWidth(cv::VolumeSettings* settings, int val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setRaycastWidth(val);
#else
	throw_no_ptcloud();
#endif
}
int cveVolumeSettingsGetRaycastWidth(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getRaycastWidth();
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeSettingsSetRaycastHeight(cv::VolumeSettings* settings, int val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setRaycastHeight(val);
#else
	throw_no_ptcloud();
#endif
}
int cveVolumeSettingsGetRaycastHeight(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getRaycastHeight();
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetDepthFactor(cv::VolumeSettings* settings, float val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setDepthFactor(val);
#else
	throw_no_ptcloud();
#endif
}
float cveVolumeSettingsGetDepthFactor(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getDepthFactor();
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetVoxelSize(cv::VolumeSettings* settings, float val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setVoxelSize(val);
#else
	throw_no_ptcloud();
#endif
}
float cveVolumeSettingsGetVoxelSize(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getVoxelSize();
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetTsdfTruncateDistance(cv::VolumeSettings* settings, float val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setTsdfTruncateDistance(val);
#else
	throw_no_ptcloud();
#endif
}
float cveVolumeSettingsGetTsdfTruncateDistance(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getTsdfTruncateDistance();
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetMaxDepth(cv::VolumeSettings* settings, float val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setMaxDepth(val);
#else
	throw_no_ptcloud();
#endif
}
float cveVolumeSettingsGetMaxDepth(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getMaxDepth();
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetMaxWeight(cv::VolumeSettings* settings, int val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setMaxWeight(val);
#else
	throw_no_ptcloud();
#endif
}
int cveVolumeSettingsGetMaxWeight(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getMaxWeight();
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetRaycastStepFactor(cv::VolumeSettings* settings, float val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setRaycastStepFactor(val);
#else
	throw_no_ptcloud();
#endif
}
float cveVolumeSettingsGetRaycastStepFactor(cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return settings->getRaycastStepFactor();
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetVolumePose(cv::VolumeSettings* settings, cv::_InputArray* val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setVolumePose(*val);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeSettingsGetVolumePose(cv::VolumeSettings* settings, cv::_OutputArray* val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->getVolumePose(*val);
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetVolumeResolution(cv::VolumeSettings* settings, cv::_InputArray* val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setVolumeResolution(*val);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeSettingsGetVolumeResolution(cv::VolumeSettings* settings, cv::_OutputArray* val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->getVolumeResolution(*val);
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsGetVolumeStrides(cv::VolumeSettings* settings, cv::_OutputArray* val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->getVolumeStrides(*val);
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetCameraIntegrateIntrinsics(cv::VolumeSettings* settings, cv::_InputArray* val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setCameraIntegrateIntrinsics(*val);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeSettingsGetCameraIntegrateIntrinsics(cv::VolumeSettings* settings, cv::_OutputArray* val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->getCameraIntegrateIntrinsics(*val);
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSettingsSetCameraRaycastIntrinsics(cv::VolumeSettings* settings, cv::_InputArray* val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->setCameraRaycastIntrinsics(*val);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeSettingsGetCameraRaycastIntrinsics(cv::VolumeSettings* settings, cv::_OutputArray* val)
{
#ifdef HAVE_OPENCV_PTCLOUD
	settings->getCameraRaycastIntrinsics(*val);
#else
	throw_no_ptcloud();
#endif
}

//----------------------------------------------------------------------------
// Volume
//----------------------------------------------------------------------------
cv::Volume* cveVolumeCreate(int volumeType, cv::VolumeSettings* settings)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return new cv::Volume(static_cast<cv::VolumeType>(volumeType), *settings);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeRelease(cv::Volume** volume)
{
#ifdef HAVE_OPENCV_PTCLOUD
	delete *volume;
	*volume = nullptr;
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeIntegrate(cv::Volume* volume, cv::_InputArray* depth, cv::_InputArray* pose)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->integrate(*depth, *pose);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeIntegrateColor(cv::Volume* volume, cv::_InputArray* depth, cv::_InputArray* image, cv::_InputArray* pose)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->integrate(*depth, *image, *pose);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeIntegrateFrame(cv::Volume* volume, cv::OdometryFrame* frame, cv::_InputArray* pose)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->integrate(*frame, *pose);
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeRaycast(
	cv::Volume* volume,
	cv::_InputArray* cameraPose,
	cv::_OutputArray* points,
	cv::_OutputArray* normals)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->raycast(*cameraPose, *points, *normals);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeRaycastColor(
	cv::Volume* volume,
	cv::_InputArray* cameraPose,
	cv::_OutputArray* points,
	cv::_OutputArray* normals,
	cv::_OutputArray* colors)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->raycast(*cameraPose, *points, *normals, *colors);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeRaycastEx(
	cv::Volume* volume,
	cv::_InputArray* cameraPose,
	int height,
	int width,
	cv::_InputArray* k,
	cv::_OutputArray* points,
	cv::_OutputArray* normals)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->raycast(*cameraPose, height, width, *k, *points, *normals);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeRaycastExColor(
	cv::Volume* volume,
	cv::_InputArray* cameraPose,
	int height,
	int width,
	cv::_InputArray* k,
	cv::_OutputArray* points,
	cv::_OutputArray* normals,
	cv::_OutputArray* colors)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->raycast(*cameraPose, height, width, *k, *points, *normals, *colors);
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeFetchNormals(cv::Volume* volume, cv::_InputArray* points, cv::_OutputArray* normals)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->fetchNormals(*points, *normals);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeFetchPointsNormals(cv::Volume* volume, cv::_OutputArray* points, cv::_OutputArray* normals)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->fetchPointsNormals(*points, *normals);
#else
	throw_no_ptcloud();
#endif
}
void cveVolumeFetchPointsNormalsColors(
	cv::Volume* volume,
	cv::_OutputArray* points,
	cv::_OutputArray* normals,
	cv::_OutputArray* colors)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->fetchPointsNormalsColors(*points, *normals, *colors);
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeReset(cv::Volume* volume)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->reset();
#else
	throw_no_ptcloud();
#endif
}

int cveVolumeGetVisibleBlocks(cv::Volume* volume)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return volume->getVisibleBlocks();
#else
	throw_no_ptcloud();
#endif
}
size_t cveVolumeGetTotalVolumeUnits(cv::Volume* volume)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return volume->getTotalVolumeUnits();
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeGetBoundingBox(cv::Volume* volume, cv::_OutputArray* bb, int precision)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->getBoundingBox(*bb, precision);
#else
	throw_no_ptcloud();
#endif
}

void cveVolumeSetEnableGrowth(cv::Volume* volume, bool v)
{
#ifdef HAVE_OPENCV_PTCLOUD
	volume->setEnableGrowth(v);
#else
	throw_no_ptcloud();
#endif
}
bool cveVolumeGetEnableGrowth(cv::Volume* volume)
{
#ifdef HAVE_OPENCV_PTCLOUD
	return volume->getEnableGrowth();
#else
	throw_no_ptcloud();
#endif
}
