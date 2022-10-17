//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SURFACE_MATCHING_C_H
#define EMGU_SURFACE_MATCHING_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_SURFACE_MATCHING

#include "opencv2/surface_matching.hpp"
#include "opencv2/surface_matching/ppf_helpers.hpp"

#else
static inline CV_NORETURN void throw_no_surface_matching() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without surface matching support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv {
	namespace ppf_match_3d {
		class ICP {};
		class Pose3D {};
		class PPF3DDetector {};
	}
}

#endif

CVAPI(cv::ppf_match_3d::ICP*) cveICPCreate(
	int iterations, 
	float tolerance, 
	float rejectionScale, 
	int numLevels, 
	int sampleType, 
	int numMaxCorr);

CVAPI(int) cveICPRegisterModelToScene(cv::ppf_match_3d::ICP* icp, cv::Mat* srcPC, cv::Mat* dstPC, double* residual, cv::Mat* pose);

CVAPI(int) cveICPRegisterModelToScene2(cv::ppf_match_3d::ICP* icp, cv::Mat* srcPC, cv::Mat* dstPC, std::vector< cv::ppf_match_3d::Pose3D >* poses);

CVAPI(void) cveICPRelease(cv::ppf_match_3d::ICP** icp);


CVAPI(cv::ppf_match_3d::Pose3D*) cvePose3DCreate();
CVAPI(void) cvePose3DUpdatePose(cv::ppf_match_3d::Pose3D* pose3d, cv::Mat* pose);
CVAPI(void) cvePose3DRelease(cv::ppf_match_3d::Pose3D** pose3d);
CVAPI(void) cvePose3DGetT(cv::ppf_match_3d::Pose3D* pose3d, CvPoint3D64f* t);
CVAPI(void) cvePose3DSetT(cv::ppf_match_3d::Pose3D* pose3d, CvPoint3D64f* t);
CVAPI(void) cvePose3DGetQ(cv::ppf_match_3d::Pose3D* pose3d, CvScalar* q);
CVAPI(void) cvePose3DSetQ(cv::ppf_match_3d::Pose3D* pose3d, CvScalar* q);

CVAPI(cv::ppf_match_3d::PPF3DDetector*) cvePPF3DDetectorCreate(double relativeSamplingStep, double relativeDistanceStep, double numAngles);
CVAPI(void) cvePPF3DDetectorTrainModel(cv::ppf_match_3d::PPF3DDetector* detector, cv::Mat* model);
CVAPI(void) cvePPF3DDetectorMatch(cv::ppf_match_3d::PPF3DDetector* detector, cv::Mat* scene, std::vector< cv::ppf_match_3d::Pose3D >* results, double relativeSceneSampleStep, double relativeSceneDistance);
CVAPI(void) cvePPF3DDetectorRelease(cv::ppf_match_3d::PPF3DDetector** detector);

CVAPI(void) cveLoadPLYSimple(cv::String* fileName, int withNormals, cv::_OutputArray* result);
CVAPI(void) cveTransformPCPose(cv::Mat* pc, cv::Mat* pose, cv::_OutputArray* result);
#endif