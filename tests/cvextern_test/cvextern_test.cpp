#include "sse.h"
#include "doubleOps.h"
#include "stdio.h"
#include <iostream>
#include "quaternions.h"
#include "opencv2/opencv_modules.hpp"

#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/imgproc/types_c.h"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/photo/photo.hpp"
#include "opencv2/dnn/dnn.hpp"
//#include "opencv2/gpu/gpu.hpp"

#ifdef _MSC_VER
#include "windows.h"
#include "time.h"
#endif

using namespace std;

#define ERROR_EPS 1.0e-12
#define fequal(a, b) (fabs((a) - (b))< ( (fabs(a) + fabs(b)) / 2 * ERROR_EPS)) 
void Test_2D_cross_product()
{
#if EMGU_SSE2
	__m128d v0 = _mm_set_pd(0.01, 0.02);
	__m128d v1 = _mm_set_pd(0.03, 0.04);

	double val0;
	_mm_store_sd(&val0, _cross_product(v1, v0));
	double val1 = 0.01 * 0.04 - 0.02 * 0.03;
	cout << "Test 2D cross product: " << (fequal(val0, val1) ? "Passed" : "Failed") << std::endl;
#endif
}

void Test_3D_cross_product()
{
	CvPoint3D64f
		x = cvPoint3D64f(1.0, 0.0, 0.0),
		y = cvPoint3D64f(0.0, 1.0, 0.0),
		z = cvPoint3D64f(0.0, 0.0, 1.0),
		temp;

	bool pass = true;
	cvPoint3D64fCrossProduct(&x, &y, &temp);
	pass &= cvPoint3D64Equals(&temp, &z);
	cvPoint3D64fCrossProduct(&y, &z, &temp);
	pass &= cvPoint3D64Equals(&temp, &x);
	cvPoint3D64fCrossProduct(&z, &x, &temp);
	pass &= cvPoint3D64Equals(&temp, &y);

	cout << "Test cvPoint3D64f cross product: " << (pass ? "Passed" : "Failed") << std::endl;
}

void Test_UMat_MinMaxLoc()
{
	cv::UMat m(5, 5, CV_8UC1);
	m.setTo(255);
	double minVal, maxVal;
	cv::Point minLoc, maxLoc;
	cv::minMaxLoc(m, &minVal, &maxVal, &minLoc, &maxLoc);
	cout << "minVal: " << minVal << "; maxVal: " << maxVal << std::endl;
}

void Test_double_MulS()
{
	double val0[] = { 0.1, 0.2, 0.3 };

	double scale = 0.12345;

	double val1[3];

	bool success = true;
	doubleOps::mulS(val0, scale, 3, val1);
	for (int i = 0; i < 3; i++)
	{
		bool equals = fequal(val1[i], (val0[i] * scale));
		if (!equals) cout << val1[i] << " != " << (val0[i] * scale) << std::endl;
		success &= equals;
	}

	memset(val1, 0, 3 * sizeof(double));
	doubleOps::mulS(val0, scale, 2, val1);
	for (int i = 0; i < 2; i++)
	{
		bool equals = fequal(val1[i], (val0[i] * scale));
		if (!equals) cout << val1[i] << " != " << (val0[i] * scale) << std::endl;
		success &= equals;
	}

	cout << "Test mulS: " << (success ? "Passed" : "Failed") << std::endl;
}

void Test_quaternions()
{
	const double eps = 1.0e-10;
	Quaternions q1, q2, q;
	CvPoint3D64f a1, a2;
	a1.x = 0.0; a1.y = 175.0 / 180.0 * CV_PI; a1.z = 0.0;
	a2.x = 0.0; a2.y = 5.0 / 180.0 * CV_PI; a2.z = 0.0;
	q1.setAxisAngle(&a1);
	q2.setAxisAngle(&a2);

	q1.slerp(&q2, 0.5, &q);
	double x = 0, y = 0, z = 0;
	q.getEuler(&x, &y, &z);
	cout << "Test quaternions slerp: " << ((
		(fabs(x) < eps || fabs(x - CV_PI) < eps)
		&& (fabs(y - CV_PI / 2.0) < eps)
		&& (fabs(z) < eps || fabs(x - CV_PI) < eps))
		? "Passed" : "Failed") << std::endl;

	q2 = q1;
	q1.conjugate();
	q1.multiply(&q2, &q);
	cout << "Test quaternions inverse: " <<
		((fabs(q.w - 1.0) < eps && fabs(q.x) < eps && fabs(q.y) < eps && fabs(q.z) < eps)
			? "Passed" : "Failed") << std::endl;
}

/*
void Test_GpuMatCopy()
{
   cv::gpu::GpuMat m1(480, 320, CV_8UC1);
   cv::gpu::GpuMat m2(480, 320, CV_8UC1);
   m1.copyTo(m2);
}*/

void Test_vectorOfPoint_to_mat()
{
	std::vector< cv::Point3f > pts;
	pts.push_back(cv::Point3f(0, 1, 2));
	pts.push_back(cv::Point3f(1, 2, 3));
	std::vector< std::vector< cv::Point3f > > vecOfVec;
	vecOfVec.push_back(pts);
	cv::InputArray iaPts = cv::InputArray(vecOfVec);
	//cv::Mat m = iaPts.getMat();
}

#ifdef _MSC_VER

#ifdef HAVE_OPENCV_VIDEOIO
void Test_MSMF_VideoWriter()
{
	cv::Size frameSize(640, 480);
	cv::VideoWriter writer("tmp.mp4", 1400, cv::VideoWriter::fourcc('H', '2', '6', '4'), 24, frameSize, true);
	bool isOpen = writer.isOpened();
	cv::Mat frame(frameSize, CV_8UC3, cv::Scalar(255, 0, 0));
	writer.write(frame);
	writer.release();
}
#endif

void Test_quaternions_performance()
{
	LARGE_INTEGER begin;
	LARGE_INTEGER end;

	Quaternions q1, q2, q;

	/* initialize random seed: */
	srand(time(NULL));

	q1.w = rand(); q1.x = rand(); q1.y = rand(); q1.z = rand();
	q2.w = rand(); q2.x = rand(); q2.y = rand(); q2.z = rand();
	q1.renorm();
	q2.renorm();

	int count = 100000;
	{
		QueryPerformanceCounter(&begin);
		for (int i = 0; i < count; i++)
		{
			//perform tasks
			q1.multiply(&q2, &q1);
		}
		QueryPerformanceCounter(&end);
		cout << "Quaternions multiplication total CPU Cycle: " << (end.QuadPart - begin.QuadPart) << std::endl;
	}
	{
		QueryPerformanceCounter(&begin);
		for (int i = 0; i < count; i++)
		{
			//perform tasks
			q1.renorm();
		}
		QueryPerformanceCounter(&end);
		cout << "Quaternions renorm total CPU Cycle: " << (end.QuadPart - begin.QuadPart) << std::endl;
	}
}
#endif

void Test_MatchTemplate()
{
	cv::Point offset(39, 123);
	cv::Mat templ(54, 71, CV_8UC3, cv::Scalar_<double>(255, 255, 255));
	cv::Mat img(300, 200, CV_8UC3, cv::Scalar_<double>(0, 0, 0));

	cv::Mat submat = img(cv::Range(offset.y, offset.y + templ.rows), cv::Range(offset.x, offset.x + templ.cols));
	templ.copyTo(submat);

	cv::Mat result;

	cv::matchTemplate(img, templ, result, CV_TM_SQDIFF_NORMED);
	double minVal, maxVal;
	cv::Point minLoc, maxLoc;
	cv::minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc);
	cout << "Template matched expected: " << offset.x << "," << offset.y << "; computed: " << minLoc.x << "," << minLoc.y << /*"; maxLoc: " << maxLoc.x << "," <<maxLoc.y <<*/ std::endl;
}

void Test_SeamlessClone(int size)
{
	//cv::Mat source = cv::imread("C:\\work\\sourceforge\\emgucv\\libs\\x64\\lena.jpg");
	cv::Mat source = cv::imread(".\\lena.jpg");
	if (!source.empty())
	{
		cv::Mat img1;
		cv::resize(source, img1, cv::Size(size, size));
		cv::Mat img2;
		cv::resize(source, img2, cv::Size(size / 2, size / 2));
		cv::Mat mask(img2.size(), CV_8UC1);
		int rows = mask.rows;
		int cols = mask.cols;
		int radius = (int)((std::min)(rows, cols) / 2.0);
		cv::circle(mask, cv::Point(mask.rows / 2, mask.cols / 2), radius, cv::Scalar(255), -1);

		cv::TickMeter meter;
		cv::Mat blend(img1.size(), CV_8UC3);
		meter.start();
		cv::seamlessClone(img2, img1, mask, cv::Point(mask.rows / 2, mask.cols / 2), blend, cv::NORMAL_CLONE);
		meter.stop();
		cout << "Seamless clone time: " << meter.getTimeMilli() << " milliseconds. " << std::endl;
	}
}

void Test_InferenceEngine()
{
	std::vector<std::pair<cv::dnn::Backend, cv::dnn::Target>> backends = cv::dnn::getAvailableBackends();
}

int main()
{
	char tmp;
	//Test_CvPoint2D32f();
	Test_2D_cross_product();
	Test_3D_cross_product();

	Test_double_MulS();
	Test_quaternions();
	//Test_GpuMatCopy();
	Test_MatchTemplate();

	Test_UMat_MinMaxLoc();

	cout << "Size of CvSize (expected " << sizeof(int) * 2 << "): " << sizeof(CvSize) << std::endl;
	cout << "Size of CvPoint2D32f (expected " << sizeof(float) * 2 << "): " << sizeof(CvSize) << std::endl;
	cout << "Size of CvRect (expected " << sizeof(int) * 4 << "): " << sizeof(CvRect) << std::endl;
	cout << "Size of IplImage (expected 144):" << sizeof(IplImage) << std::endl;
	cout << "Size of CvScalar (expected " << sizeof(double) * 4 << "): " << sizeof(CvScalar) << std::endl;

	Test_vectorOfPoint_to_mat();
#ifdef _MSC_VER
	Test_quaternions_performance();

	Test_SeamlessClone(3840);

#ifdef HAVE_OPENCV_VIDEOIO
	Test_MSMF_VideoWriter();
#endif

	Test_InferenceEngine();

	cin >> tmp; //wait for input only if compiling with visual C++ 
#endif


}


