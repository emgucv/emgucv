#include "stdafx.h"
#include "ImageProcessor.h"

using namespace CPlusPlus;

array<Image<Bgr^, Byte>^>^ ImageProcessor::ProcessImage()
{
    //---- plain old OpenCV code ----
	IplImage* img1 = cvCreateImage(cvSize(300, 200), IPL_DEPTH_8U, 3);
	cvSet(img1, cvScalar(255, 255, 255));
	CvFont font;
	cvInitFont(&font, CV_FONT_HERSHEY_SIMPLEX, 1.0, 1.0);
	cvPutText(img1, "Hello, World", cvPoint(50, 100), &font, cvScalar(0.0, 0.0, 0.0));
	//---- end of plain old OpenCV code

	//create the managed Emgu::CV::Image array
	array<Image<Bgr^, Byte>^>^ imageArray = gcnew array<Image<Bgr^, Byte>^>(2);
	
	//---- Copying image from IplImage to Emgu::CV::Image class
	//this image will be a copy of the image generated from plain old OpenCV code
	imageArray[0] = gcnew Image<Bgr^, Byte>(img1->width, img1->height);
	//copy the image from IplImage to Emgu::CV::Image array
	cvCopy(img1, imageArray[0]->Ptr.ToPointer());
	//---- End of image copying

	//---- clean up the IplImages ----
	cvReleaseImage(&img1);
	
	//---- Image Processing in EmguCV using .Net Syntax
	//another image to be displayed
	imageArray[1] = gcnew Image<Bgr^, Byte>(imageArray[0]->Width, imageArray[0]->Height);
	//fill the image with random colors of mean 50 and standard deviation of 10;
	imageArray[1]->_RandNormal(MCvScalar(50.0, 50.0, 50.0), MCvScalar(10.0, 10.0, 10.0));
	imageArray[1] = imageArray[0] - imageArray[1];
	//---- End of Image Processing in Emgu CV.

	return imageArray;
}