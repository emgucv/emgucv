#include "3d_c.h"

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