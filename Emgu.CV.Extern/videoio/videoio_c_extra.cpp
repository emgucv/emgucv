//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "videoio_c_extra.h"

void OpenniGetColorPoints(CvCapture* capture, std::vector<ColorPoint>* points, IplImage* maskImg)
{
   IplImage* pcm = cvRetrieveFrame(capture, CV_CAP_OPENNI_POINT_CLOUD_MAP); //XYZ in meters (CV_32FC3)
   IplImage* bgr = cvRetrieveFrame(capture, CV_CAP_OPENNI_BGR_IMAGE); //CV_8UC3
   
   int ptCount = pcm->width * pcm->height;
   CvPoint3D32f* position = (CvPoint3D32f*) pcm->imageData;
   unsigned char* color = (unsigned char*) bgr->imageData;

   //int colorIdx = 0;
   ColorPoint cp;
   if (maskImg)
   {
      unsigned char* mask = (unsigned char*)maskImg->imageData;
      for (int i = 0; i < ptCount; i++, mask++, position++, color += 3)
         if (*mask)
         {
            memcpy(&cp.position, position, sizeof(CvPoint3D32f));
            memcpy(&cp.red, color, 3);
            points->push_back(cp);
            
         }
   } else
   {
      for (int i = 0; i < ptCount; i++, position++, color += 3)
      {
         memcpy(&cp.position, position, sizeof(CvPoint3D32f));
         memcpy(&cp.red, color, 3);
         points->push_back(cp); 
         
      }
   }
}

cv::VideoCapture* cveVideoCaptureCreateFromDevice(int device)
{
   return new cv::VideoCapture(device);
}

cv::VideoCapture* cveVideoCaptureCreateFromFile(cv::String* fileName)
{
   return new cv::VideoCapture(*fileName);
}

void cveVideoCaptureRelease(cv::VideoCapture** capture)
{
   delete *capture;
   *capture = 0;
}
bool cveVideoCaptureSet(cv::VideoCapture* capture, int propId, double value)
{
   return capture->set(propId, value);
}
double cveVideoCaptureGet(cv::VideoCapture* capture, int propId)
{
   return capture->get(propId);
}
bool cveVideoCaptureGrab(cv::VideoCapture* capture)
{
   return capture->grab();
}
bool cveVideoCaptureRetrieve(cv::VideoCapture* capture, cv::_OutputArray* image, int flag)
{
   return capture->retrieve(*image, flag);
}
bool cveVideoCaptureRead(cv::VideoCapture* capture, cv::_OutputArray* image)
{
   return capture->read(*image);
}

void cveVideoCaptureReadToMat(cv::VideoCapture* capture, cv::Mat* mat)
{
   cv::Mat m;
   (*capture) >> m;
   cv::swap(m, *mat);
}

#if WINAPI_FAMILY
void cveWinrtSetFrameContainer(::Windows::UI::Xaml::Controls::Image^ image)
{
   cv::winrt_setFrameContainer(image);
}
void cveWinrtStartMessageLoop(CvWinrtMessageLoopCallback callback)
{
   cv::winrt_startMessageLoop(callback);
}
void cveWinrtImshow()
{
   cv::winrt_imshow();
}
void cveWinrtOnVisibilityChanged(bool visible)
{
   cv::winrt_onVisibilityChanged(visible);
}
#endif

cv::VideoWriter* cveVideoWriterCreate(cv::String* filename, int fourcc, double fps, CvSize* frameSize, bool isColor)
{
   return new cv::VideoWriter(*filename, fourcc, fps, *frameSize, isColor);
}
void cveVideoWriterRelease(cv::VideoWriter** writer)
{
   delete *writer;
   *writer = 0;
}
bool cveVideoWriterIsOpened(cv::VideoWriter* writer)
{
   return writer->isOpened();
}
bool cveVideoWriterSet(cv::VideoWriter* writer, int propId, double value)
{
   return writer->set(propId, value);
}
double cveVideoWriterGet(cv::VideoWriter* writer, int propId)
{
   return writer->get(propId);
}



void cveVideoWriterWrite(cv::VideoWriter* writer, cv::Mat* image)
{
   writer->write(*image);
}
int cveVideoWriterFourcc(char c1, char c2, char c3, char c4)
{
   return cv::VideoWriter::fourcc(c1, c2, c3, c4);
}

