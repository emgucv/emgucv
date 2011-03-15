//#define TESSDLL_IMPORTS
#include "opencv2/core/core.hpp"
#include "opencv2/core/core_c.h"
#include "stdio.h"
#include "baseapi.h"

class EmguTesseract: public tesseract::TessBaseAPI
{
public:
   bool RecognitionDone() const
   {
      return recognition_done_;
   }

   int GetTextLength(int* blob_count)
   {
      return TextLength(blob_count);
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

CVAPI(EmguTesseract*) TessBaseAPICreate(const char* dataPath, const char* language, int* initResult) 
{ 
   EmguTesseract* ocr = new EmguTesseract(); 
   *initResult = ocr->Init(dataPath, language);
   return ocr;
}

CVAPI(void) TessBaseAPIRelease(EmguTesseract** ocr)
{
   delete *ocr;
}

CVAPI(void) TessBaseAPISetImage(EmguTesseract* ocr, IplImage* image)
{
   ocr->SetImage( (const unsigned char*)image->imageData, image->width, image->height, 1, image->widthStep);
}

CVAPI(void) TessBaseAPIGetUTF8Text(EmguTesseract* ocr, char* text, int maxSizeInBytes)
{
   char* result = ocr->GetUTF8Text();
   strncpy(text, result, maxSizeInBytes);
   delete[] result;
}

CVAPI(void) TessBaseAPIRecognize(EmguTesseract* ocr)
{
   if (!ocr->RecognitionDone())
   {
      if (ocr->Recognize(NULL) != 0)
         CV_Error(CV_StsError, "Tesseract engine: Recognize Failed");
   }
}

struct TesseractResult
{
   int length;
   float cost;
   int x0;
   int x1;
   int y0;
   int y1;
};

CVAPI(void) TessBaseAPIExtractResult(EmguTesseract* ocr, CvSeq* charSeq, CvSeq* resultSeq)
{
   if (ocr == NULL ||
      (!ocr->RecognitionDone() && ocr->Recognize(NULL) < 0))
      return;
  int total_length = ocr->GetTextLength(NULL);

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
  for (int i = 0; i < n; i++)
  {  
     totalTextLength += lengths[i];
     TesseractResult tr;
     tr.length = lengths[i];
     tr.cost = costs[i];
     tr.x0 = x0[i];
     tr.x1 = x1[i];
     tr.y0 = y0[i];
     tr.y1 = y1[i];
     cvSeqPush(resultSeq, &tr);
  }
  cvSeqPushMulti(charSeq, text, totalTextLength);
  delete[] text;
  delete[] lengths;
  delete[] x0;
  delete[] y0;
  delete[] x1;
  delete[] y1;
}