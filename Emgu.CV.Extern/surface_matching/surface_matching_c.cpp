//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "surface_matching_c.h"

cv::ppf_match_3d::ICP* cveICPCreate(
	int iterations,
	float tolerance,
	float rejectionScale,
	int numLevels,
	int sampleType,
	int numMaxCorr)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	return new cv::ppf_match_3d::ICP(iterations, tolerance, rejectionScale, numLevels, sampleType, numMaxCorr);
#else
	throw_no_surface_matching();
#endif
}

int cveICPRegisterModelToScene(cv::ppf_match_3d::ICP* icp, cv::Mat* srcPC, cv::Mat* dstPC, double* residual, cv::Mat* pose)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	double r;
	cv::Matx44d p;
	int result = icp->registerModelToScene(*srcPC, *dstPC, r, p);
	*residual = r;
	cv::Mat matP(p);
	matP.copyTo(*pose);
	return result;
#else
	throw_no_surface_matching();
#endif
}


int cveICPRegisterModelToScene2(cv::ppf_match_3d::ICP* icp, cv::Mat* srcPC, cv::Mat* dstPC, std::vector< cv::ppf_match_3d::Pose3D >* poses)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	std::vector< cv::ppf_match_3d::Pose3DPtr > posePtrs;
	for (int i = 0; i < poses->size(); ++i)
	{
		cv::ppf_match_3d::Pose3D* ptr = &(*poses)[i];
		cv::Ptr<cv::ppf_match_3d::Pose3D> cvPtr(ptr);
		posePtrs.push_back(cvPtr);
	}
	return icp->registerModelToScene(*srcPC, *dstPC, posePtrs);
#else
	throw_no_surface_matching();
#endif	
}

void cveICPRelease(cv::ppf_match_3d::ICP** icp)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	delete* icp;
	*icp = 0;
#else
	throw_no_surface_matching();
#endif
}

cv::ppf_match_3d::Pose3D* cvePose3DCreate()
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	return new cv::ppf_match_3d::Pose3D();
#else
	throw_no_surface_matching();
#endif	
}
void cvePose3DUpdatePose(cv::ppf_match_3d::Pose3D* pose3d, cv::Mat* pose)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	cv::Matx44d p(*pose);
	pose3d->updatePose(p);
#else
	throw_no_surface_matching();
#endif		
}
void cvePose3DRelease(cv::ppf_match_3d::Pose3D** pose3d)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	delete* pose3d;
	*pose3d = 0;
#else
	throw_no_surface_matching();
#endif
}
void cvePose3DGetT(cv::ppf_match_3d::Pose3D* pose3d, CvPoint3D64f* t)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	cv::Vec3d translation = pose3d->t;
	t->x = translation[0];
	t->y = translation[1];
	t->z = translation[2];
#else
	throw_no_surface_matching();
#endif
}
void cvePose3DSetT(cv::ppf_match_3d::Pose3D* pose3d, CvPoint3D64f* t)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	cv::Vec3d translation;
	translation[0] = t->x;
	translation[1] = t->y;
	translation[2] = t->z;
	pose3d->t = translation;
#else
	throw_no_surface_matching();
#endif
}

void cvePose3DGetQ(cv::ppf_match_3d::Pose3D* pose3d, CvScalar* q)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	cv::Vec4d quat = pose3d->q;
	q->val[0] = quat[0];
	q->val[1] = quat[1];
	q->val[2] = quat[2];
	q->val[3] = quat[3];
#else
	throw_no_surface_matching();
#endif
}
void cvePose3DSetQ(cv::ppf_match_3d::Pose3D* pose3d, CvScalar* q)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	cv::Vec4d quat;
	quat[0] = q->val[0];
	quat[1] = q->val[1];
	quat[2] = q->val[2];
	quat[2] = q->val[2];
	pose3d->q = quat;
#else
	throw_no_surface_matching();
#endif
}

cv::ppf_match_3d::PPF3DDetector* cvePPF3DDetectorCreate(double relativeSamplingStep, double relativeDistanceStep, double numAngles)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	return new cv::ppf_match_3d::PPF3DDetector(relativeSamplingStep, relativeDistanceStep, numAngles);
#else
	throw_no_surface_matching();
#endif
}
void cvePPF3DDetectorTrainModel(cv::ppf_match_3d::PPF3DDetector* detector, cv::Mat* model)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	detector->trainModel(*model);
#else
	throw_no_surface_matching();
#endif
}

void cvePPF3DDetectorMatch(cv::ppf_match_3d::PPF3DDetector* detector, cv::Mat* scene, std::vector< cv::ppf_match_3d::Pose3D >* results, double relativeSceneSampleStep, double relativeSceneDistance)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	std::vector< cv::ppf_match_3d::Pose3DPtr > rawResult;
	detector->match(*scene, rawResult, relativeSceneSampleStep, relativeSceneDistance);

	results->clear();
	for (std::vector< cv::ppf_match_3d::Pose3DPtr >::iterator iter = rawResult.begin(); iter != rawResult.end(); ++iter)
	{
		cv::ppf_match_3d::Pose3DPtr ptr = *iter;
		results->push_back(*ptr);
	}
#else
	throw_no_surface_matching();
#endif
}

void cvePPF3DDetectorRelease(cv::ppf_match_3d::PPF3DDetector** detector)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	delete* detector;
	*detector = 0;
#else
	throw_no_surface_matching();
#endif
}

void cveLoadPLYSimple(cv::String* fileName, int withNormals, cv::_OutputArray* result)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	cv::Mat m = cv::ppf_match_3d::loadPLYSimple(fileName->c_str(), withNormals);
	m.copyTo(*result);
#else
	throw_no_surface_matching();
#endif
}

void cveTransformPCPose(cv::Mat* pc, cv::Mat* pose, cv::_OutputArray* result)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	cv::Mat m = cv::ppf_match_3d::transformPCPose(*pc, *pose);
	m.copyTo(*result);
#else
	throw_no_surface_matching();
#endif
}