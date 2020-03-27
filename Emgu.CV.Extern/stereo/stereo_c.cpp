#include "stereo_c.h"

cv::stereo::QuasiDenseStereo* cveQuasiDenseStereoCreate(
	CvSize* monoImgSize,
	cv::String* paramFilepath,
	cv::Ptr<cv::stereo::QuasiDenseStereo>** sharedPtr)
{
	cv::Ptr<cv::stereo::QuasiDenseStereo> qds = cv::stereo::QuasiDenseStereo::create(*monoImgSize, *paramFilepath);
	*sharedPtr = new cv::Ptr<cv::stereo::QuasiDenseStereo>(qds);
	return (*sharedPtr)->get();
}

void cveQuasiDenseStereoRelease(cv::Ptr<cv::stereo::QuasiDenseStereo>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

void cveQuasiDenseStereoProcess(cv::stereo::QuasiDenseStereo* stereo, cv::Mat* imgLeft, cv::Mat* imgRight)
{
	stereo->process(*imgLeft, *imgRight);
}

void cveQuasiDenseStereoGetDisparity(cv::stereo::QuasiDenseStereo* stereo, uint8_t disparityLvls, cv::Mat* disparity)
{
	cv::Mat d = stereo->getDisparity(disparityLvls);
	cv::swap(d, *disparity);
}