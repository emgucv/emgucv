#include "3d_c.h"

void cveRodrigues(cv::_InputArray* src, cv::_OutputArray* dst, cv::_OutputArray* jacobian)
{
#ifdef HAVE_OPENCV_3D
	cv::Rodrigues(*src, *dst, jacobian ? *jacobian : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_3d();
#endif
}

void cveFindHomography(cv::_InputArray* srcPoints, cv::_InputArray* dstPoints, cv::_OutputArray* dst, int method, double ransacReprojThreshold, cv::_OutputArray* mask)
{
#ifdef HAVE_OPENCV_3D
	cv::Mat tmp = cv::findHomography(
		*srcPoints,
		*dstPoints,
		method,
		ransacReprojThreshold,
		mask ? *mask : static_cast<cv::OutputArray>(cv::noArray()));
	tmp.copyTo(*dst);
#else
	throw_no_3d();
#endif
}

void cveRQDecomp3x3(
	cv::_InputArray* src,
	CvPoint3D64f* out,
	cv::_OutputArray* mtxR,
	cv::_OutputArray* mtxQ,
	cv::_OutputArray* Qx,
	cv::_OutputArray* Qy,
	cv::_OutputArray* Qz)
{
#ifdef HAVE_OPENCV_3D
	cv::Vec3d result = cv::RQDecomp3x3(
		*src,
		*mtxR,
		*mtxQ,
		Qx ? *Qx : static_cast<cv::OutputArray>(cv::noArray()),
		Qy ? *Qy : static_cast<cv::OutputArray>(cv::noArray()),
		Qz ? *Qz : static_cast<cv::OutputArray>(cv::noArray())
	);
	out->x = result[0];
	out->y = result[1];
	out->z = result[2];
#else
	throw_no_3d();
#endif
}

void cveDecomposeProjectionMatrix(
	cv::_InputArray* projMatrix,
	cv::_OutputArray* cameraMatrix,
	cv::_OutputArray* rotMatrix,
	cv::_OutputArray* transVect,
	cv::_OutputArray* rotMatrixX,
	cv::_OutputArray* rotMatrixY,
	cv::_OutputArray* rotMatrixZ,
	cv::_OutputArray* eulerAngles)
{
#ifdef HAVE_OPENCV_3D
	cv::decomposeProjectionMatrix(
		*projMatrix,
		*cameraMatrix,
		*rotMatrix,
		*transVect,
		rotMatrixX ? *rotMatrixX : static_cast<cv::_OutputArray>(cv::noArray()),
		rotMatrixY ? *rotMatrixY : static_cast<cv::_OutputArray>(cv::noArray()),
		rotMatrixZ ? *rotMatrixZ : static_cast<cv::_OutputArray>(cv::noArray()),
		eulerAngles ? *eulerAngles : static_cast<cv::_OutputArray>(cv::noArray())
	);
#else
	throw_no_3d();
#endif
}

void cveProjectPoints(
	cv::_InputArray* objPoints, cv::_InputArray* rvec, cv::_InputArray* tvec, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
	cv::_OutputArray* imagePoints, cv::_OutputArray* jacobian, double aspectRatio)
{
#ifdef HAVE_OPENCV_3D
	cv::projectPoints(
		*objPoints,
		*rvec, *tvec,
		*cameraMatrix,
		distCoeffs ? *distCoeffs : static_cast<cv::InputArray>(cv::noArray()),
		*imagePoints,
		jacobian ? *jacobian : static_cast<cv::OutputArray>(cv::noArray()), aspectRatio);
#else
	throw_no_3d();
#endif
}

bool cveSolvePnP(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_OutputArray* rvec, cv::_OutputArray* tvec, bool useExtrinsicGuess, int flags)
{
#ifdef HAVE_OPENCV_3D
	return cv::solvePnP(*objectPoints, *imagePoints, *cameraMatrix, *distCoeffs, *rvec, *tvec, useExtrinsicGuess, flags);
#else
	throw_no_3d();
#endif
}

bool cveSolvePnPRansac(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* rvec,
	cv::_OutputArray* tvec,
	bool useExtrinsicGuess,
	int iterationsCount,
	float reprojectionError,
	double confident,
	cv::_OutputArray* inliers,
	int flags)
{
#ifdef HAVE_OPENCV_3D
	return cv::solvePnPRansac(
		*objectPoints,
		*imagePoints,
		*cameraMatrix,
		distCoeffs ? *distCoeffs : static_cast<cv::InputArray>(cv::noArray()),
		*rvec,
		*tvec,
		useExtrinsicGuess,
		iterationsCount,
		reprojectionError,
		confident,
		inliers ? *inliers : static_cast<cv::OutputArray>(cv::noArray()),
		flags);
#else
	throw_no_3d();
#endif
}

int cveSolveP3P(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* rvecs,
	cv::_OutputArray* tvecs,
	int flags)
{
#ifdef HAVE_OPENCV_3D
	return cv::solveP3P(*objectPoints, *imagePoints, *cameraMatrix, *distCoeffs, *rvecs, *tvecs, flags);
#else
	throw_no_3d();
#endif
}

void cveSolvePnPRefineLM(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	CvTermCriteria* criteria)
{
#ifdef HAVE_OPENCV_3D
	cv::solvePnPRefineLM(
		*objectPoints,
		*imagePoints,
		*cameraMatrix,
		distCoeffs ? *distCoeffs : static_cast<cv::InputArray>(cv::noArray()),
		*rvec,
		*tvec,
		*criteria);
#else
	throw_no_3d();
#endif
}

void cveSolvePnPRefineVVS(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	CvTermCriteria* criteria,
	double VVSlambda)
{
#ifdef HAVE_OPENCV_3D
	cv::solvePnPRefineVVS(
		*objectPoints,
		*imagePoints,
		*cameraMatrix,
		distCoeffs ? *distCoeffs : static_cast<cv::InputArray>(cv::noArray()),
		*rvec,
		*tvec,
		*criteria,
		VVSlambda);
#else
	throw_no_3d();
#endif
}

int cveSolvePnPGeneric(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* rvecs,
	cv::_OutputArray* tvecs,
	bool useExtrinsicGuess,
	int flags,
	cv::_InputArray* rvec,
	cv::_InputArray* tvec,
	cv::_OutputArray* reprojectionError)
{
#ifdef HAVE_OPENCV_3D
	return cv::solvePnPGeneric(
		*objectPoints,
		*imagePoints,
		*cameraMatrix,
		*distCoeffs,
		*rvecs,
		*tvecs,
		useExtrinsicGuess,
		static_cast<cv::SolvePnPMethod>(flags),
		rvec ? *rvec : static_cast<cv::InputArray>(cv::noArray()),
		tvec ? *tvec : static_cast<cv::InputArray>(cv::noArray()),
		reprojectionError ? *reprojectionError : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_3d();
#endif
}

void cveFindFundamentalMat(cv::_InputArray* points1, cv::_InputArray* points2, cv::_OutputArray* dst, int method, double param1, double param2, cv::_OutputArray* mask)
{
#ifdef HAVE_OPENCV_3D
	cv::Mat tmp = cv::findFundamentalMat(
		*points1,
		*points2,
		method,
		param1,
		param2,
		mask ? *mask : static_cast<cv::OutputArray>(cv::noArray()));
	tmp.copyTo(*dst);
#else
	throw_no_3d();
#endif
}

void cveFindEssentialMat(
	cv::_InputArray* points1,
	cv::_InputArray* points2,
	cv::_InputArray* cameraMatrix,
	int method,
	double prob,
	double threshold,
	int maxIter,
	cv::_OutputArray* mask,
	cv::Mat* essentialMat)
{
#ifdef HAVE_OPENCV_3D
	cv::Mat res = cv::findEssentialMat(
		*points1,
		*points2,
		*cameraMatrix,
		method,
		prob,
		threshold,
		maxIter,
		mask ? *mask : static_cast<cv::OutputArray>(cv::noArray()));
	cv::swap(res, *essentialMat);
#else
	throw_no_3d();
#endif
}

cv::Odometry* cveOdometryCreate(
	cv::OdometryType odometryType
)
{
#ifdef HAVE_OPENCV_3D
	cv::Odometry* odometry = new cv::Odometry(odometryType);
	return odometry;
#else
	throw_no_3d();
#endif
}
void cveOdometryRelease(cv::Odometry** sharedPtr)
{
#ifdef HAVE_OPENCV_3D
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_3d();
#endif
}

bool cveOdometryCompute1(
	cv::Odometry* odometry,
	cv::_InputArray* srcFrame,
	cv::_InputArray* dstFrame,
	cv::_OutputArray* rt)
{
#ifdef HAVE_OPENCV_3D
	return odometry->compute(
		*srcFrame,
		*dstFrame,
		*rt
	);
#else
	throw_no_3d();
#endif

}

bool cveOdometryCompute2(
	cv::Odometry* odometry,
	cv::_InputArray* srcDepthFrame,
	cv::_InputArray* srcRGBFrame,
	cv::_InputArray* dstDepthFrame,
	cv::_InputArray* dstRGBFrame,
	cv::_OutputArray* rt)
{
#ifdef HAVE_OPENCV_3D
	return odometry->compute(
		*srcDepthFrame,
		*srcRGBFrame,
		*dstDepthFrame,
		*dstRGBFrame,
		*rt
	);
#else
	throw_no_3d();
#endif
}



cv::RgbdNormals* cveRgbdNormalsCreate(
	int rows,
	int cols,
	int depth,
	cv::_InputArray* K,
	int window_size,
	int method,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::RgbdNormals>** sharedPtr)
{
#ifdef HAVE_OPENCV_3D
	cv::Ptr<cv::RgbdNormals> odometry = cv::RgbdNormals::create(
		rows,
		cols,
		depth,
		*K,
		window_size,
		(cv::RgbdNormals::RgbdNormalsMethod) method);
	*sharedPtr = new cv::Ptr<cv::RgbdNormals>(odometry);
	*algorithm = dynamic_cast<cv::Algorithm*>((*sharedPtr)->get());
	return (*sharedPtr)->get();
#else
	throw_no_3d();
#endif
}
void cveRgbdNormalsRelease(cv::Ptr<cv::RgbdNormals>** sharedPtr)
{
#ifdef HAVE_OPENCV_3D
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_3d();
#endif	
}
void cveRgbdNormalsApply(
	cv::RgbdNormals* rgbdNormals,
	cv::_InputArray* points,
	cv::_OutputArray* normals)
{
#ifdef HAVE_OPENCV_3D
	rgbdNormals->apply(*points, *normals);
#else
	throw_no_3d();
#endif	
}

void cveDecomposeEssentialMat(cv::_InputArray* e, cv::_OutputArray* r1, cv::_OutputArray* r2, cv::_OutputArray* t)
{
#ifdef HAVE_OPENCV_3D
	cv::decomposeEssentialMat(*e, *r1, *r2, *t);
#else
	throw_no_3d();
#endif	
}

int cveDecomposeHomographyMat(
	cv::_InputArray* h,
	cv::_InputArray* k,
	cv::_OutputArray* rotations,
	cv::_OutputArray* translations,
	cv::_OutputArray* normals)
{
#ifdef HAVE_OPENCV_3D
	return cv::decomposeHomographyMat(
		*h,
		*k,
		*rotations,
		*translations,
		*normals);
#else
	throw_no_3d();
#endif	
}

void cveComputeCorrespondEpilines(cv::_InputArray* points, int whichImage, cv::_InputArray* f, cv::_OutputArray* lines)
{
#ifdef HAVE_OPENCV_3D
	cv::computeCorrespondEpilines(*points, whichImage, *f, *lines);
#else
	throw_no_3d();
#endif
}

void cveConvertPointsToHomogeneous(cv::_InputArray* src, cv::_OutputArray* dst)
{
#ifdef HAVE_OPENCV_3D
	cv::convertPointsToHomogeneous(*src, *dst);
#else
	throw_no_3d();
#endif
}

void cveConvertPointsFromHomogeneous(cv::_InputArray* src, cv::_OutputArray* dst)
{
#ifdef HAVE_OPENCV_3D
	cv::convertPointsFromHomogeneous(*src, *dst);
#else
	throw_no_3d();
#endif
}


void cveTriangulatePoints(cv::_InputArray* projMat1, cv::_InputArray* projMat2, cv::_InputArray* projPoints1, cv::_InputArray* projPoints2, cv::_OutputArray* points4D)
{
#ifdef HAVE_OPENCV_3D
	cv::triangulatePoints(*projMat1, *projMat2, *projPoints1, *projPoints2, *points4D);
#else
	throw_no_3d();
#endif
}

void cveCorrectMatches(cv::_InputArray* f, cv::_InputArray* points1, cv::_InputArray* points2, cv::_OutputArray* newPoints1, cv::_OutputArray* newPoints2)
{
#ifdef HAVE_OPENCV_3D
	cv::correctMatches(*f, *points1, *points2, *newPoints1, *newPoints2);
#else
	throw_no_3d();
#endif
}

int cveEstimateAffine3D(
	cv::_InputArray* src, cv::_InputArray* dst,
	cv::_OutputArray* out, cv::_OutputArray* inliers,
	double ransacThreshold, double confidence)
{
#ifdef HAVE_OPENCV_3D
	return cv::estimateAffine3D(*src, *dst, *out, *inliers, ransacThreshold, confidence);
#else
	throw_no_3d();
#endif
}

void cveEstimateAffine2D(
	cv::_InputArray* from, cv::_InputArray* to,
	cv::_OutputArray* inliners,
	int method, double ransacReprojThreshold,
	int maxIters, double confidence,
	int refineIters,
	cv::Mat* affine)
{
#ifdef HAVE_OPENCV_3D
	cv::Mat m = cv::estimateAffine2D(
		*from, *to,
		inliners ? *inliners : static_cast<cv::OutputArray>(cv::noArray()),
		method, ransacReprojThreshold, maxIters, confidence, refineIters);
	cv::swap(m, *affine);
#else
	throw_no_3d();
#endif
}

void cveEstimateAffinePartial2D(
	cv::_InputArray* from, cv::_InputArray* to,
	cv::_OutputArray* inliners,
	int method, double ransacReprojThreshold,
	int maxIters, double confidence,
	int refineIters,
	cv::Mat* affine)
{
#ifdef HAVE_OPENCV_3D
	cv::Mat m = cv::estimateAffinePartial2D(
		*from, *to,
		inliners ? *inliners : static_cast<cv::OutputArray>(cv::noArray()),
		method, ransacReprojThreshold, maxIters, confidence, refineIters);
	cv::swap(m, *affine);
#else
	throw_no_3d();
#endif
}

void cveInitUndistortRectifyMap(cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* r, cv::_InputArray* newCameraMatrix, CvSize* size, int m1type, cv::_OutputArray* map1, cv::_OutputArray* map2)
{
#ifdef HAVE_OPENCV_3D
	cv::initUndistortRectifyMap(
		*cameraMatrix, *distCoeffs,
		r ? *r : static_cast<cv::_InputArray>(cv::noArray()),
		*newCameraMatrix,
		*size,
		m1type,
		*map1,
		map2 ? *map2 : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_3d();
#endif
}

void cveUndistort(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* cameraMatrix, cv::_InputArray* distorCoeffs, cv::_InputArray* newCameraMatrix)
{
#ifdef HAVE_OPENCV_3D
	cv::undistort(
		*src,
		*dst,
		*cameraMatrix,
		*distorCoeffs,
		newCameraMatrix ? *newCameraMatrix : static_cast<cv::InputArray>(cv::noArray()));
#else
	throw_no_3d();
#endif
}

void cveUndistortPoints(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* r, cv::_InputArray* p)
{
#ifdef HAVE_OPENCV_3D
	cv::undistortPoints(
		*src,
		*dst,
		*cameraMatrix,
		*distCoeffs,
		r ? *r : static_cast<cv::InputArray>(cv::noArray()),
		p ? *p : static_cast<cv::InputArray>(cv::noArray()));
#else
	throw_no_3d();
#endif
}

void cveGetDefaultNewCameraMatrix(cv::_InputArray* cameraMatrix, CvSize* imgsize, bool centerPrincipalPoint, cv::Mat* cm)
{
#ifdef HAVE_OPENCV_3D
	cv::Mat res = cv::getDefaultNewCameraMatrix(*cameraMatrix, *imgsize, centerPrincipalPoint);
	cv::swap(*cm, res);
#else
	throw_no_3d();
#endif
}

void cveGetOptimalNewCameraMatrix(
	cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
	CvSize* imageSize, double alpha, CvSize* newImgSize,
	CvRect* validPixROI,
	bool centerPrincipalPoint,
	cv::Mat* newCameraMatrix)
{
#ifdef HAVE_OPENCV_CALIB
	cv::Rect r;
	cv::Mat m = cv::getOptimalNewCameraMatrix(
		*cameraMatrix,
		distCoeffs ? *distCoeffs : static_cast<cv::InputArray>(cv::noArray()),
		*imageSize,
		alpha,
		*newImgSize,
		&r,
		centerPrincipalPoint);
	if (validPixROI)
	{
		validPixROI->x = r.x;
		validPixROI->y = r.y;
		validPixROI->width = r.width;
		validPixROI->height = r.height;
	}
	cv::swap(m, *newCameraMatrix);
#else
	throw_no_calib();
#endif
}