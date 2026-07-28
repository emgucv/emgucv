//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PTCLOUD_C_H
#define EMGU_PTCLOUD_C_H

#include "opencv2/core.hpp"
#include "cvapi_compat.h"

#ifdef HAVE_OPENCV_PTCLOUD
#include "opencv2/ptcloud.hpp"
#include "opencv2/ptcloud/volume.hpp"
#include "opencv2/ptcloud/volume_settings.hpp"
#else
static inline CV_NORETURN void throw_no_ptcloud() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without ptcloud support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv {
	class Odometry {};
	class RgbdNormals {};
	class Octree {};
	enum class VolumeType
	{
		TSDF = 0,
		HashTSDF = 1,
		ColorTSDF = 2
	};
	class VolumeSettings {};
	class Volume {};
}
#endif

CVAPI(cv::Odometry*) cveOdometryCreate(int odometryType);
CVAPI(void) cveOdometryRelease(cv::Odometry** ptr);
CVAPI(bool) cveOdometryCompute1(cv::Odometry* odometry, cv::_InputArray* srcFrame, cv::_InputArray* dstFrame, cv::_OutputArray* rt);
CVAPI(bool) cveOdometryCompute2(cv::Odometry* odometry, cv::_InputArray* srcDepthFrame, cv::_InputArray* srcRGBFrame, cv::_InputArray* dstDepthFrame, cv::_InputArray* dstRGBFrame, cv::_OutputArray* rt);

CVAPI(cv::RgbdNormals*) cveRgbdNormalsCreate(int rows, int cols, int depth, cv::_InputArray* K, int window_size, int method, cv::Algorithm** algorithm, cv::Ptr<cv::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsRelease(cv::Ptr<cv::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsApply(cv::RgbdNormals* rgbdNormals, cv::_InputArray* points, cv::_OutputArray* normals);

CVAPI(cv::Octree*) cveOctreeCreate(std::vector<cv::Point3f>* pointCloud, int maxDepth, cv::Ptr<cv::Octree>** sharedPtr);
CVAPI(void) cveOctreeRelease(cv::Ptr<cv::Octree>** sharedPtr);

CVAPI(void) cveLoadPointCloud(
	cv::String* filename,
	cv::_OutputArray* vertices,
	cv::_OutputArray* normals,
	cv::_OutputArray* rgb);
CVAPI(void) cveSavePointCloud(
	cv::String* filename,
	cv::_InputArray* vertices,
	cv::_InputArray* normals,
	cv::_InputArray* rgb);
CVAPI(void) cveLoadMesh(
	cv::String* filename,
	cv::_OutputArray* vertices,
	cv::_OutputArray* indices,
	cv::_OutputArray* normals,
	cv::_OutputArray* colors,
	cv::_OutputArray* texCoords);
CVAPI(void) cveSaveMesh(
	cv::String* filename,
	cv::_InputArray* vertices,
	cv::_InputArray* indices,
	cv::_InputArray* normals,
	cv::_InputArray* colors,
	cv::_InputArray* texCoords);

CVAPI(void) cveDepthTo3d(cv::_InputArray* depth, cv::_InputArray* K, cv::_OutputArray* points3d, cv::_InputArray* mask);
CVAPI(void) cveDepthTo3dSparse(cv::_InputArray* depth, cv::_InputArray* inK, cv::_InputArray* inPoints, cv::_OutputArray* points3d);
CVAPI(void) cveRescaleDepth(cv::_InputArray* in, int type, cv::_OutputArray* out, double depthFactor);
CVAPI(void) cveRegisterDepth(
	cv::_InputArray* unregisteredCameraMatrix,
	cv::_InputArray* registeredCameraMatrix,
	cv::_InputArray* registeredDistCoeffs,
	cv::_InputArray* Rt,
	cv::_InputArray* unregisteredDepth,
	cv::Size* outputImagePlaneSize,
	cv::_OutputArray* registeredDepth,
	bool depthDilation);
CVAPI(void) cveWarpFrame(
	cv::_InputArray* depth,
	cv::_InputArray* image,
	cv::_InputArray* mask,
	cv::_InputArray* Rt,
	cv::_InputArray* cameraMatrix,
	cv::_OutputArray* warpedDepth,
	cv::_OutputArray* warpedImage,
	cv::_OutputArray* warpedMask);

//----------------------------------------------------------------------------
// VolumeSettings
//----------------------------------------------------------------------------
CVAPI(cv::VolumeSettings*) cveVolumeSettingsCreate(int volumeType);
CVAPI(void) cveVolumeSettingsRelease(cv::VolumeSettings** settings);

CVAPI(void) cveVolumeSettingsSetIntegrateWidth(cv::VolumeSettings* settings, int val);
CVAPI(int) cveVolumeSettingsGetIntegrateWidth(cv::VolumeSettings* settings);
CVAPI(void) cveVolumeSettingsSetIntegrateHeight(cv::VolumeSettings* settings, int val);
CVAPI(int) cveVolumeSettingsGetIntegrateHeight(cv::VolumeSettings* settings);

CVAPI(void) cveVolumeSettingsSetRaycastWidth(cv::VolumeSettings* settings, int val);
CVAPI(int) cveVolumeSettingsGetRaycastWidth(cv::VolumeSettings* settings);
CVAPI(void) cveVolumeSettingsSetRaycastHeight(cv::VolumeSettings* settings, int val);
CVAPI(int) cveVolumeSettingsGetRaycastHeight(cv::VolumeSettings* settings);

CVAPI(void) cveVolumeSettingsSetDepthFactor(cv::VolumeSettings* settings, float val);
CVAPI(float) cveVolumeSettingsGetDepthFactor(cv::VolumeSettings* settings);

CVAPI(void) cveVolumeSettingsSetVoxelSize(cv::VolumeSettings* settings, float val);
CVAPI(float) cveVolumeSettingsGetVoxelSize(cv::VolumeSettings* settings);

CVAPI(void) cveVolumeSettingsSetTsdfTruncateDistance(cv::VolumeSettings* settings, float val);
CVAPI(float) cveVolumeSettingsGetTsdfTruncateDistance(cv::VolumeSettings* settings);

CVAPI(void) cveVolumeSettingsSetMaxDepth(cv::VolumeSettings* settings, float val);
CVAPI(float) cveVolumeSettingsGetMaxDepth(cv::VolumeSettings* settings);

CVAPI(void) cveVolumeSettingsSetMaxWeight(cv::VolumeSettings* settings, int val);
CVAPI(int) cveVolumeSettingsGetMaxWeight(cv::VolumeSettings* settings);

CVAPI(void) cveVolumeSettingsSetRaycastStepFactor(cv::VolumeSettings* settings, float val);
CVAPI(float) cveVolumeSettingsGetRaycastStepFactor(cv::VolumeSettings* settings);

CVAPI(void) cveVolumeSettingsSetVolumePose(cv::VolumeSettings* settings, cv::_InputArray* val);
CVAPI(void) cveVolumeSettingsGetVolumePose(cv::VolumeSettings* settings, cv::_OutputArray* val);

CVAPI(void) cveVolumeSettingsSetVolumeResolution(cv::VolumeSettings* settings, cv::_InputArray* val);
CVAPI(void) cveVolumeSettingsGetVolumeResolution(cv::VolumeSettings* settings, cv::_OutputArray* val);

CVAPI(void) cveVolumeSettingsGetVolumeStrides(cv::VolumeSettings* settings, cv::_OutputArray* val);

CVAPI(void) cveVolumeSettingsSetCameraIntegrateIntrinsics(cv::VolumeSettings* settings, cv::_InputArray* val);
CVAPI(void) cveVolumeSettingsGetCameraIntegrateIntrinsics(cv::VolumeSettings* settings, cv::_OutputArray* val);

CVAPI(void) cveVolumeSettingsSetCameraRaycastIntrinsics(cv::VolumeSettings* settings, cv::_InputArray* val);
CVAPI(void) cveVolumeSettingsGetCameraRaycastIntrinsics(cv::VolumeSettings* settings, cv::_OutputArray* val);

//----------------------------------------------------------------------------
// Volume
//----------------------------------------------------------------------------
CVAPI(cv::Volume*) cveVolumeCreate(int volumeType, cv::VolumeSettings* settings);
CVAPI(void) cveVolumeRelease(cv::Volume** volume);

CVAPI(void) cveVolumeIntegrate(cv::Volume* volume, cv::_InputArray* depth, cv::_InputArray* pose);
CVAPI(void) cveVolumeIntegrateColor(cv::Volume* volume, cv::_InputArray* depth, cv::_InputArray* image, cv::_InputArray* pose);

CVAPI(void) cveVolumeRaycast(
	cv::Volume* volume,
	cv::_InputArray* cameraPose,
	cv::_OutputArray* points,
	cv::_OutputArray* normals);
CVAPI(void) cveVolumeRaycastColor(
	cv::Volume* volume,
	cv::_InputArray* cameraPose,
	cv::_OutputArray* points,
	cv::_OutputArray* normals,
	cv::_OutputArray* colors);
CVAPI(void) cveVolumeRaycastEx(
	cv::Volume* volume,
	cv::_InputArray* cameraPose,
	int height,
	int width,
	cv::_InputArray* k,
	cv::_OutputArray* points,
	cv::_OutputArray* normals);
CVAPI(void) cveVolumeRaycastExColor(
	cv::Volume* volume,
	cv::_InputArray* cameraPose,
	int height,
	int width,
	cv::_InputArray* k,
	cv::_OutputArray* points,
	cv::_OutputArray* normals,
	cv::_OutputArray* colors);

CVAPI(void) cveVolumeFetchNormals(cv::Volume* volume, cv::_InputArray* points, cv::_OutputArray* normals);
CVAPI(void) cveVolumeFetchPointsNormals(cv::Volume* volume, cv::_OutputArray* points, cv::_OutputArray* normals);
CVAPI(void) cveVolumeFetchPointsNormalsColors(
	cv::Volume* volume,
	cv::_OutputArray* points,
	cv::_OutputArray* normals,
	cv::_OutputArray* colors);

CVAPI(void) cveVolumeReset(cv::Volume* volume);

CVAPI(int) cveVolumeGetVisibleBlocks(cv::Volume* volume);
CVAPI(size_t) cveVolumeGetTotalVolumeUnits(cv::Volume* volume);

CVAPI(void) cveVolumeGetBoundingBox(cv::Volume* volume, cv::_OutputArray* bb, int precision);

CVAPI(void) cveVolumeSetEnableGrowth(cv::Volume* volume, bool v);
CVAPI(bool) cveVolumeGetEnableGrowth(cv::Volume* volume);

#endif
