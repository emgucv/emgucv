#include "calib_c.h"

//2D tracker
bool cveGetHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, std::vector< std::vector< cv::DMatch > >* matches, cv::Mat* mask, double randsacThreshold, cv::Mat* homography)
{
#ifdef HAVE_OPENCV_CALIB
	//cv::Mat_<int> indMat = (cv::Mat_<int>) cv::cvarrToMat(indices);

	cv::Mat_<uchar> maskMat = mask ? static_cast<cv::Mat_<uchar>>(*mask) : cv::Mat_<uchar>(matches->size(), 1, 255);
	int nonZero = mask ? cv::countNonZero(maskMat) : matches->size();
	if (nonZero < 4) return false;

	std::vector<cv::Point2f> srcPtVec;
	std::vector<cv::Point2f> dstPtVec;

	for (int i = 0; i < maskMat.rows; i++)
	{
		if (maskMat.at<uchar>(i))
		{
			int modelIdx = matches->at(i).at(0).trainIdx; //indMat(i, 0); 
			srcPtVec.push_back((*model)[modelIdx].pt);
			dstPtVec.push_back((*observed)[i].pt);
		}
	}

	//cv::Mat_<uchar> ransacMask(srcPtVec.size(), 1);
	std::vector<uchar> ransacMask;
	cv::Mat result = cv::findHomography(cv::Mat(srcPtVec), cv::Mat(dstPtVec), cv::RANSAC, randsacThreshold, ransacMask);
	if (result.empty())
	{
		return false;
	}
	cv::swap(result, *homography);

	int idx = 0;
	for (int i = 0; i < maskMat.rows; i++)
	{
		uchar* val = maskMat.ptr<uchar>(i);
		if (*val)
			*val = ransacMask[idx++];
	}
	return true;
#else
	throw_no_calib();
#endif
}

bool cveFindCirclesGrid(cv::_InputArray* image, cv::Size* patternSize, cv::_OutputArray* centers, int flags, cv::Feature2D* blobDetector)
{
#ifdef HAVE_OPENCV_CALIB
	cv::Ptr<cv::Feature2D> ptr(blobDetector, [](cv::Feature2D*){});
	return cv::findCirclesGrid(*image, *patternSize, *centers, flags, ptr);
#else
	throw_no_calib();
#endif
}

bool cveFindChessboardCornersSB(cv::_InputArray* image, cv::Size* patternSize, cv::_OutputArray* corners, int flags)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::findChessboardCornersSB(*image, *patternSize, *corners, flags);
#else
	throw_no_calib();
#endif
}

void cveEstimateChessboardSharpness(
	cv::_InputArray* image,
	cv::Size* patternSize,
	cv::_InputArray* corners,
	float riseDistance,
	bool vertical,
	cv::_OutputArray* sharpness,
	cv::Scalar* result)
{
#ifdef HAVE_OPENCV_CALIB
	cv::Scalar r = cv::estimateChessboardSharpness(*image, *patternSize, *corners, riseDistance, vertical, sharpness ? *sharpness : static_cast<cv::OutputArray>(cv::noArray())) ;
	result->val[0] = r.val[0];
	result->val[1] = r.val[1];
	result->val[2] = r.val[2];
	result->val[3] = r.val[3];
#else
	throw_no_calib();
#endif
}

void cveDrawChessboardCorners(cv::_InputOutputArray* image, cv::Size* patternSize, cv::_InputArray* corners, bool patternWasFound)
{
#ifdef HAVE_OPENCV_CALIB
	cv::drawChessboardCorners(*image, *patternSize, *corners, patternWasFound);
#else
	throw_no_calib();
#endif
}

void cveFilterSpeckles(cv::_InputOutputArray* img, double newVal, int maxSpeckleSize, double maxDiff, cv::_InputOutputArray* buf)
{
#ifdef HAVE_OPENCV_CALIB
	cv::filterSpeckles(*img, newVal, maxSpeckleSize, maxDiff, buf ? *buf : static_cast<cv::_InputOutputArray>(cv::noArray()));
#else
	throw_no_calib();
#endif
}

bool cveFindChessboardCorners(cv::_InputArray* image, cv::Size* patternSize, cv::_OutputArray* corners, int flags)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::findChessboardCorners(*image, *patternSize, *corners, flags);
#else
	throw_no_calib();
#endif
}

bool cveFind4QuadCornerSubpix(cv::_InputArray* image, cv::_InputOutputArray* corners, cv::Size* regionSize)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::find4QuadCornerSubpix(*image, *corners, *regionSize);
#else
	throw_no_calib();
#endif
}


double cveCalibrateCamera(
	cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::Size* imageSize,
	cv::_InputOutputArray* cameraMatrix, cv::_InputOutputArray* distCoeffs,
	cv::_OutputArray* rvecs, cv::_OutputArray* tvecs, int flags, cv::TermCriteria* criteria)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::calibrateCamera(*objectPoints, *imagePoints, *imageSize, *cameraMatrix, *distCoeffs, *rvecs, *tvecs, flags, *criteria);
#else
	throw_no_calib();
#endif
}

void cveReprojectImageTo3D(cv::_InputArray* disparity, cv::_OutputArray* threeDImage, cv::_InputArray* q, bool handleMissingValues, int ddepth)
{
#ifdef HAVE_OPENCV_CALIB
	cv::reprojectImageTo3D(*disparity, *threeDImage, *q, handleMissingValues, ddepth);
#else
	throw_no_calib();
#endif
}


void cveCalibrationMatrixValues(
	cv::_InputArray* cameraMatrix, cv::Size* imageSize, double apertureWidth, double apertureHeight,
	double* fovx, double* fovy, double* focalLength, cv::Point2d* principalPoint, double* aspectRatio)
{
#ifdef HAVE_OPENCV_CALIB
	double _fovx, _fovy, _focalLength, _aspectRatio;
	cv::Point2d _principalPoint;

	cv::calibrationMatrixValues(*cameraMatrix, *imageSize, apertureWidth, apertureHeight, _fovx, _fovy, _focalLength, _principalPoint, _aspectRatio);
	*fovx = _fovx;
	*fovy = _fovy;
	*focalLength = _focalLength;
	*aspectRatio = _aspectRatio;
	principalPoint->x = _principalPoint.x;
	principalPoint->y = _principalPoint.y;
#else
	throw_no_calib();
#endif
}

double cveStereoCalibrate1(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints1,
	cv::_InputArray* imagePoints2,
	cv::_InputOutputArray* cameraMatrix1,
	cv::_InputOutputArray* distCoeffs1,
	cv::_InputOutputArray* cameraMatrix2,
	cv::_InputOutputArray* distCoeffs2,
	cv::Size* imageSize,
	cv::_InputOutputArray* r,
	cv::_InputOutputArray* t,
	cv::_OutputArray* e,
	cv::_OutputArray* f,
	cv::_OutputArray* rvecs,
	cv::_OutputArray* tvecs,
	cv::_OutputArray* perViewErrors,
	int flags,
	cv::TermCriteria* criteria)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::stereoCalibrate(
		*objectPoints,
		*imagePoints1,
		*imagePoints2,
		*cameraMatrix1,
		*distCoeffs1,
		*cameraMatrix2,
		*distCoeffs2,
		*imageSize,
		*r,
		*t,
		*e,
		*f,
		*rvecs,
		*tvecs,
		*perViewErrors,
		flags,
		*criteria);
#else
	throw_no_calib();
#endif
}


double cveStereoCalibrate2(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints1,
	cv::_InputArray* imagePoints2,
	cv::_InputOutputArray* cameraMatrix1,
	cv::_InputOutputArray* distCoeffs1,
	cv::_InputOutputArray* cameraMatrix2,
	cv::_InputOutputArray* distCoeffs2,
	cv::Size* imageSize,
	cv::_OutputArray* r,
	cv::_OutputArray* t,
	cv::_OutputArray* e,
	cv::_OutputArray* f,
	int flags,
	cv::TermCriteria* criteria)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::stereoCalibrate(*objectPoints, *imagePoints1, *imagePoints2, *cameraMatrix1, *distCoeffs1, *cameraMatrix2, *distCoeffs2, *imageSize, *r, *t, *e, *f,
		flags, *criteria);
#else
	throw_no_calib();
#endif
}

void cveInitCameraMatrix2D(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::Size* imageSize,
	double aspectRatio,
	cv::Mat* cameraMatrix)
{
#ifdef HAVE_OPENCV_CALIB
	cv::Mat m = cv::initCameraMatrix2D(*objectPoints, *imagePoints, *imageSize, aspectRatio);
	cv::swap(m, *cameraMatrix);
#else
	throw_no_calib();
#endif
}

/* Fisheye calibration */
void cveFisheyeProjectPoints(cv::_InputArray* objectPoints, cv::_OutputArray* imagePoints, cv::_InputArray* rvec, cv::_InputArray* tvec,
	cv::_InputArray* K, cv::_InputArray* D, double alpha, cv::_OutputArray* jacobian)
{
#ifdef HAVE_OPENCV_CALIB
	cv::fisheye::projectPoints(
		*objectPoints, 
		*imagePoints, 
		*rvec, 
		*tvec, 
		*K, 
		*D, 
		alpha, 
		jacobian ? *jacobian : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_calib();
#endif
}

void cveFisheyeDistortPoints(cv::_InputArray* undistored, cv::_OutputArray* distorted, cv::_InputArray* K, cv::_InputArray* D, double alpha)
{
#ifdef HAVE_OPENCV_CALIB
	cv::fisheye::distortPoints(*undistored, *distorted, *K, *D, alpha);
#else
	throw_no_calib();
#endif
}

void cveFisheyeUndistortPoints(
	cv::_InputArray* distorted,
	cv::_OutputArray* undistorted, 
	cv::_InputArray* K, 
	cv::_InputArray* D, 
	cv::_InputArray* R, 
	cv::_InputArray* P,
	cv::TermCriteria* criteria
	)
{
#ifdef HAVE_OPENCV_CALIB
	cv::fisheye::undistortPoints(
		*distorted, 
		*undistorted, 
		*K, 
		*D, 
		R ? *R : static_cast<cv::InputArray>(cv::noArray()), 
		P ? *P : static_cast<cv::InputArray>(cv::noArray()),
		*criteria);
#else
	throw_no_calib();
#endif
}

void cveFisheyeInitUndistortRectifyMap(
	cv::_InputArray* K, 
	cv::_InputArray* D, 
	cv::_InputArray* R, 
	cv::_InputArray* P, 
	cv::Size* size, 
	int m1Type, 
	cv::_OutputArray* map1, 
	cv::_OutputArray* map2)
{
#ifdef HAVE_OPENCV_CALIB
	cv::fisheye::initUndistortRectifyMap(
		*K, 
		*D, 
		*R, 
		*P, 
		*size, 
		m1Type, 
		*map1, 
		map2 ? *map2 : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_calib();
#endif
}

void cveFisheyeUndistortImage(cv::_InputArray* distorted, cv::_OutputArray* undistored, cv::_InputArray* K, cv::_InputArray* D, cv::_InputArray* Knew, cv::Size* newSize)
{
#ifdef HAVE_OPENCV_CALIB
	cv::fisheye::undistortImage(
		*distorted, 
		*undistored, 
		*K, 
		*D, 
		Knew ? *Knew : static_cast<cv::InputArray>(cv::noArray()), 
		*newSize);
#else
	throw_no_calib();
#endif
}

void cveFisheyeEstimateNewCameraMatrixForUndistortRectify(cv::_InputArray* K, cv::_InputArray* D, cv::Size* imageSize, cv::_InputArray* R, cv::_OutputArray* P, double balance, cv::Size* newSize, double fovScale)
{
#ifdef HAVE_OPENCV_CALIB
	cv::fisheye::estimateNewCameraMatrixForUndistortRectify(*K, *D, *imageSize, *R, *P, balance, *newSize, fovScale);
#else
	throw_no_calib();
#endif
}

void cveFisheyeStereoRectify(cv::_InputArray* K1, cv::_InputArray*D1, cv::_InputArray* K2, cv::_InputArray* D2, cv::Size* imageSize,
	cv::_InputArray* R, cv::_InputArray* tvec, cv::_OutputArray* R1, cv::_OutputArray* R2, cv::_OutputArray* P1, cv::_OutputArray* P2, cv::_OutputArray* Q, int flags,
	cv::Size* newImageSize, double balance, double fovScale)
{
#ifdef HAVE_OPENCV_CALIB
	cv::fisheye::stereoRectify(*K1, *D1, *K2, *D2, *imageSize, *R, *tvec, *R1, *R2, *P1, *P2, *Q, flags, *newImageSize, balance, fovScale);
#else
	throw_no_calib();
#endif
}

double cveFisheyeCalibrate(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::Size* imageSize,
	cv::_InputOutputArray* K, cv::_InputOutputArray* D, cv::_OutputArray* rvecs, cv::_OutputArray* tvecs, int flags,
	cv::TermCriteria* criteria)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::fisheye::calibrate(*objectPoints, *imagePoints, *imageSize, *K, *D, *rvecs, *tvecs, flags, *criteria);
#else
	throw_no_calib();
#endif
}

double cveFisheyeStereoCalibrate(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints1,
	cv::_InputArray* imagePoints2, cv::_InputOutputArray* K1, cv::_InputOutputArray* D1, cv::_InputOutputArray* K2, cv::_InputOutputArray* D2,
	cv::Size* imageSize, cv::_OutputArray* R, cv::_OutputArray* T, int flags, cv::TermCriteria* criteria)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::fisheye::stereoCalibrate(*objectPoints, *imagePoints1, *imagePoints2, *K1, *D1, *K2, *D2, *imageSize, *R, *T, flags, *criteria);
#else
	throw_no_calib();
#endif
}

bool cveFisheyeSolvePnP(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* rvec,
	cv::_OutputArray* tvec,
	bool useExtrinsicGuess,
	int flags,
	cv::TermCriteria* criteria)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::fisheye::solvePnP(
		*objectPoints, 
		*imagePoints, 
		*cameraMatrix, 
		*distCoeffs, 
		*rvec, 
		*tvec, 
		useExtrinsicGuess, 
		flags,
		*criteria);
#else
	throw_no_calib();
#endif
}

bool cveFisheyeSolvePnPRansac(
	cv::_InputArray* opoints,
	cv::_InputArray* ipoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* rvec,
	cv::_OutputArray* tvec,
	bool useExtrinsicGuess,
	int iterationsCount,
	float reprojectionError,
	double confidence,
	cv::_OutputArray* inliers,
	int flags,
	cv::TermCriteria* criteria)
{
#ifdef HAVE_OPENCV_CALIB
	return cv::fisheye::solvePnPRansac(
		*opoints,
		*ipoints,
		*cameraMatrix,
		*distCoeffs,
		*rvec,
		*tvec,
		useExtrinsicGuess,
		iterationsCount,
		reprojectionError,
		confidence,
		*inliers,
		flags,
		*criteria
		);
#else
	throw_no_calib();
#endif	
}

void cveCalibrateHandEye(
	cv::_InputArray* R_gripper2base, cv::_InputArray* t_gripper2base,
	cv::_InputArray* R_target2cam, cv::_InputArray* t_target2cam,
	cv::_OutputArray* R_cam2gripper, cv::_OutputArray* t_cam2gripper,
	int method)
{
#ifdef HAVE_OPENCV_CALIB
	cv::calibrateHandEye(
		*R_gripper2base, *t_gripper2base,
		*R_target2cam, *t_target2cam,
		*R_cam2gripper, *t_cam2gripper,
		static_cast<cv::HandEyeCalibrationMethod>(method));
#else
	throw_no_calib();
#endif
}

