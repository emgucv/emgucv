#include "stereo_c.h"

//StereoSGBM
cv::StereoSGBM* cveStereoSGBMCreate(
	int minDisparity, int numDisparities, int blockSize,
	int P1, int P2, int disp12MaxDiff,
	int preFilterCap, int uniquenessRatio,
	int speckleWindowSize, int speckleRange,
	int mode, cv::StereoMatcher** stereoMatcher,
	cv::Ptr<cv::StereoSGBM>** sharedPtr)
{
	cv::Ptr<cv::StereoSGBM> ptr = cv::StereoSGBM::create(minDisparity, numDisparities, blockSize, P1, P2, disp12MaxDiff, preFilterCap, uniquenessRatio, speckleWindowSize, speckleRange, mode);
	*sharedPtr = new cv::Ptr<cv::StereoSGBM>(ptr);
	cv::StereoSGBM* result = ptr.get();
	*stereoMatcher = dynamic_cast<cv::StereoMatcher*>(result);
	return result;
}
void cveStereoSGBMRelease(cv::Ptr<cv::StereoSGBM>** sharedPtr)
{
	delete* sharedPtr;
	*sharedPtr = 0;
}

//StereoBM
cv::StereoMatcher* cveStereoBMCreate(int numberOfDisparities, int blockSize, cv::Ptr<cv::StereoMatcher>** sharedPtr)
{
	cv::Ptr<cv::StereoMatcher> ptr = cv::StereoBM::create(numberOfDisparities, blockSize);
	*sharedPtr = new cv::Ptr<cv::StereoMatcher>(ptr);
	return ptr.get();
}

//StereoMatcher
void cveStereoMatcherCompute(cv::StereoMatcher* disparitySolver, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity)
{
	disparitySolver->compute(*left, *right, *disparity);
}
void cveStereoMatcherRelease(cv::Ptr<cv::StereoMatcher>** sharedPtr)
{
	delete* sharedPtr;
	*sharedPtr = 0;
}

bool cveStereoRectifyUncalibrated(cv::_InputArray* points1, cv::_InputArray* points2, cv::_InputArray* f, CvSize* imgSize, cv::_OutputArray* h1, cv::_OutputArray* h2, double threshold)
{
	return cv::stereoRectifyUncalibrated(*points1, *points2, *f, *imgSize, *h1, *h2, threshold);
}

void cveStereoRectify(
	cv::_InputArray* cameraMatrix1, cv::_InputArray* distCoeffs1,
	cv::_InputArray* cameraMatrix2, cv::_InputArray* distCoeffs2,
	CvSize* imageSize, cv::_InputArray* r, cv::_InputArray* t,
	cv::_OutputArray* r1, cv::_OutputArray* r2,
	cv::_OutputArray* p1, cv::_OutputArray* p2,
	cv::_OutputArray* q, int flags,
	double alpha, CvSize* newImageSize,
	CvRect* validPixROI1, CvRect* validPixROI2)
{
	cv::Rect rect1, rect2;
	cv::stereoRectify(*cameraMatrix1, *distCoeffs1, *cameraMatrix2, *distCoeffs2, *imageSize, *r, *t, *r1, *r2,
		*p1, *p2, *q, flags, alpha, *newImageSize, &rect1, &rect2);
	*validPixROI1 = cvRect(rect1);
	*validPixROI2 = cvRect(rect2);
}