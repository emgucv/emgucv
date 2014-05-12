//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "highgui_c_extra.h"

void OpenniGetColorPoints(CvCapture* capture, CvSeq* points, IplImage* maskImg)
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
            cvSeqPush(points, &cp);
         }
   } else
   {
      for (int i = 0; i < ptCount; i++, position++, color += 3)
      {
         memcpy(&cp.position, position, sizeof(CvPoint3D32f));
         memcpy(&cp.red, color, 3);
         cvSeqPush(points, &cp);
      }
   }
}

/*
cv::Mat* cvMatCreateFromFile(char* fileName, int flags)
{
   cv::Mat* m = new cv::Mat();
   cv::Mat tmp = cv::imread(fileName, flags);
   cv::swap(*m, tmp);
   return m;
}*/

bool cveImwrite(cv::String* filename, cv::_InputArray* img, const std::vector<int>* params)
{
   return cv::imwrite(*filename, *img, params ? *params : std::vector<int>());
}

void cveImread(cv::String* fileName, int flags, cv::Mat* result)
{
   cv::Mat m = cv::imread(*fileName, flags);
   cv::swap(*result, m);
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

cv::VideoWriter* cveVideoWriterCreate(cv::String* filename, int fourcc, double fps, CvSize* frameSize, bool isColor)
{
   return new cv::VideoWriter(*filename, fourcc, fps, *frameSize, isColor);
}
void cveVideoWriterRelease(cv::VideoWriter** writer)
{
   delete *writer;
   *writer = 0;
}
void cveVideoWriterWrite(cv::VideoWriter* writer, cv::Mat* image)
{
   writer->write(*image);
}
int cveVideoWriterFourcc(char c1, char c2, char c3, char c4)
{
   return cv::VideoWriter::fourcc(c1, c2, c3, c4);
}

void cveImdecode(cv::_InputArray* buf, int flags, cv::Mat* dst)
{
   cv::imdecode(*buf, flags, dst);
}
void cveImencode(cv::String* ext, cv::_InputArray* img, std::vector< unsigned char >* buf, std::vector< int >* params)
{
   cv::imencode(*ext, *img, *buf, params ? *params : std::vector<int>());
}