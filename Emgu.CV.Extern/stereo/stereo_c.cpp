#include "stereo_c.h"

cv::stereo::QuasiDenseStereo* cveQuasiDenseStereoCreate(
	CvSize* monoImgSize,
	cv::String* paramFilepath,
	cv::Ptr<cv::stereo::QuasiDenseStereo>** sharedPtr)
{
#ifdef HAVE_OPENCV_STEREO
	cv::Ptr<cv::stereo::QuasiDenseStereo> qds = cv::stereo::QuasiDenseStereo::create(*monoImgSize, *paramFilepath);
	*sharedPtr = new cv::Ptr<cv::stereo::QuasiDenseStereo>(qds);
	return (*sharedPtr)->get();
#else
	throw_no_stereo();
#endif
}

void cveQuasiDenseStereoRelease(cv::Ptr<cv::stereo::QuasiDenseStereo>** sharedPtr)
{
#ifdef HAVE_OPENCV_STEREO
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_stereo();
#endif
}

void cveQuasiDenseStereoProcess(cv::stereo::QuasiDenseStereo* stereo, cv::Mat* imgLeft, cv::Mat* imgRight)
{
#ifdef HAVE_OPENCV_STEREO
	stereo->process(*imgLeft, *imgRight);
#else
	throw_no_stereo();
#endif
}

void cveQuasiDenseStereoGetDisparity(cv::stereo::QuasiDenseStereo* stereo, uint8_t disparityLvls, cv::Mat* disparity)
{
#ifdef HAVE_OPENCV_STEREO
	cv::Mat d = stereo->getDisparity(disparityLvls);
	cv::swap(d, *disparity);
#else
	throw_no_stereo();
#endif
}