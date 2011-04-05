//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "opencv2/core/core.hpp"
#include "opencv2/core/core_c.h"
#include "stdio.h"
#include "baseapi.h"

class EmguTesseract: public tesseract::TessBaseAPI
{
public:
   int GetTextLength(int* blob_count)
   {
      return TextLength(blob_count);
   }

   int GetImageHeight()
   {
      int left, top, width, height, imageWidth, imageHeight;
      thresholder_->GetImageSizes(&left, &top, &width, &height,
                             &imageWidth, &imageHeight);
      return imageHeight;
   }

   int TesseractExtractResult(char** text,
      int** lengths,
      float** costs,
      int** x0,
      int** y0,
      int** x1,
      int** y1)
   {
      return tesseract::TessBaseAPI::TesseractExtractResult(text, lengths, costs, x0, y0, x1, y1, page_res_);
   }

};

CVAPI(const char*) TesseractGetVersion()
{
#ifdef _WIN32
   return tesseract::TessBaseAPI::Version();
#else
   return 0;
#endif
}

CVAPI(EmguTesseract*) TessBaseAPICreate() 
{ 
   EmguTesseract* ocr = new EmguTesseract(); 
   return ocr;
}

CVAPI(int) TessBaseAPIInit(EmguTesseract* ocr, const char* dataPath, const char* language, int mode)
{ 
#ifdef _WIN32
   return ocr->Init(dataPath, language, (tesseract::OcrEngineMode) mode);
#else
   return ocr->Init(dataPath, language);
#endif
}

CVAPI(void) TessBaseAPIRelease(EmguTesseract** ocr)
{
   delete *ocr;
}

CVAPI(void) TessBaseAPIRecognizeImage(EmguTesseract* ocr, IplImage* image)
{
   ocr->SetImage( (const unsigned char*)image->imageData, image->width, image->height, image->nChannels, image->widthStep);
   if (ocr->Recognize(NULL) != 0)
      CV_Error(CV_StsError, "Tesseract engine: Recognize Failed");
}

CVAPI(void) TessBaseAPIGetUTF8Text(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte)
{
   char* result = ocr->GetUTF8Text();
   size_t length = strlen(result);
   vectorOfByte->resize(length);
   memcpy(&(*vectorOfByte)[0], result, length);
   delete[] result;
}

struct TesseractResult
{
   int length;
   float cost;
   CvRect region;
};

CVAPI(void) TessBaseAPIExtractResult(EmguTesseract* ocr, CvSeq* charSeq, CvSeq* resultSeq)
{
   if (ocr == NULL)
      return;

   char* text;
   int* lengths;
   float* costs;
   int* x0, *y0, *x1, *y1;
   int n = ocr->TesseractExtractResult(&text,
      &lengths,
      &costs,
      &x0,
      &y0,
      &x1,
      &y1);
   int totalTextLength = 0;
   
   int height = ocr->GetImageHeight();

   for (int i = 0; i < n; i++)
   {  
      totalTextLength += lengths[i];
      TesseractResult tr;
      tr.length = lengths[i];
      tr.cost = costs[i];

      tr.region.x = x0[i];
      tr.region.y = height - y1[i];
      tr.region.width = x1[i] - x0[i];
      tr.region.height = y1[i] - y0[i];
      cvSeqPush(resultSeq, &tr);
   }
   if (n > 0)
      cvSeqPushMulti(charSeq, text, totalTextLength);
   delete[] text;
   delete[] lengths;
   delete[] x0;
   delete[] y0;
   delete[] x1;
   delete[] y1;
}

CVAPI(bool) TessBaseAPISetVariable(EmguTesseract* ocr, const char* varName, const char* value)
{
   return ocr->SetVariable(varName, value);
}
