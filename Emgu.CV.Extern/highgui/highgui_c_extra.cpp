//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
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