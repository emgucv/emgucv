//#define TESSDLL_IMPORTS
#include "opencv2/core/core.hpp"
#include "stdio.h"
#include "baseapi.h"

CVAPI(tesseract::TessBaseAPI*) TessBaseAPICreate(const char* dataPath, const char* language, int* initResult) 
{ 
   tesseract::TessBaseAPI* ocr = new tesseract::TessBaseAPI(); 
   *initResult = ocr->Init(dataPath, language);
   return ocr;
}

CVAPI(void) TessBaseAPIRelease(tesseract::TessBaseAPI** ocr)
{
   delete *ocr;
}

CVAPI(void) TessBaseAPISetImage(tesseract::TessBaseAPI* ocr, IplImage* image)
{
   ocr->SetImage( (const unsigned char*)image->imageData, image->width, image->height, 1, image->widthStep);
}

CVAPI(void) TessBaseAPIGetUTF8Text(tesseract::TessBaseAPI* ocr, char* text, int maxSizeInBytes)
{
   char* result = ocr->GetUTF8Text();
   strncpy(text, result, maxSizeInBytes);
   delete[] result;
}