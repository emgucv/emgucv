//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "tesseract_c.h"

const char* TesseractGetVersion()
{
#ifdef _WIN32
   return tesseract::TessBaseAPI::Version();
#else
   return 0;
#endif
}

EmguTesseract* TessBaseAPICreate() 
{ 
   EmguTesseract* ocr = new EmguTesseract(); 
   return ocr;
}

int TessBaseAPIInit(EmguTesseract* ocr, const char* dataPath, const char* language, int mode)
{ 
   return ocr->Init(dataPath, language, (tesseract::OcrEngineMode) mode);
}

void TessBaseAPIRelease(EmguTesseract** ocr)
{
   delete *ocr;
}

/*
void TessBaseAPIRecognizeImage(EmguTesseract* ocr, IplImage* image)
{
   ocr->SetImage( (const unsigned char*)image->imageData, image->width, image->height, image->nChannels, image->widthStep);
   if (ocr->Recognize(NULL) != 0)
      CV_Error(CV_StsError, "Tesseract engine: Recognize Failed");
}*/

void TessBaseAPIRecognizeArray(EmguTesseract* ocr, cv::_InputArray* mat)
{
   cv::Mat m = mat->getMat();
   ocr->SetImage((const unsigned char*) m.data, m.size().width, m.size().height, m.channels(), m.step);
   if (ocr->Recognize(NULL) != 0)
      CV_Error(CV_StsError, "Tesseract engine: Recognize Failed");
}

void TessBaseAPIGetUTF8Text(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte)
{
   char* result = ocr->GetUTF8Text();
   size_t length = strlen(result);
   vectorOfByte->resize(length);
   memcpy(&(*vectorOfByte)[0], result, length);
   delete[] result;
}

void TessBaseAPIExtractResult(EmguTesseract* ocr, std::vector<unsigned char>* charSeq, std::vector<TesseractResult>* resultSeq)
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
      resultSeq->push_back(tr);
      //cvSeqPush(resultSeq, &tr);
   }
   if (n > 0)
   {
      charSeq->resize(totalTextLength);
      memcpy(&(*charSeq)[0], text, totalTextLength);
   }
      
   delete[] text;
   delete[] lengths;
   delete[] x0;
   delete[] y0;
   delete[] x1;
   delete[] y1;
}

bool TessBaseAPISetVariable(EmguTesseract* ocr, const char* varName, const char* value)
{
   return ocr->SetVariable(varName, value);
}


void TessBaseAPISetPageSegMode(EmguTesseract* ocr, tesseract::PageSegMode mode)
{
   ocr->SetPageSegMode(mode);
}

tesseract::PageSegMode TessBaseAPIGetPageSegMode(EmguTesseract* ocr)
{
   return ocr->GetPageSegMode();
}

int TessBaseAPIGetOpenCLDevice(EmguTesseract* ocr, void **device)
{
   return static_cast<int>(ocr->getOpenCLDevice(device));
}

tesseract::PageIterator* TessBaseAPIAnalyseLayout(EmguTesseract* ocr, bool mergeSimilarWords)
{
   return ocr->AnalyseLayout(mergeSimilarWords);
}

void TessPageIteratorGetOrientation(tesseract::PageIterator* iterator, tesseract::Orientation* orientation, tesseract::WritingDirection* writingDirection, tesseract::TextlineOrder* order, float* deskewAngle)
{
   iterator->Orientation(orientation, writingDirection, order, deskewAngle);
}

bool TessPageIteratorGetBaseLine(
   tesseract::PageIterator* iterator,
   tesseract::PageIteratorLevel level,
   int* x1, int* y1, int* x2, int* y2)
{
   return iterator->Baseline(level, x1, y1, x2, y2);
}

void TessPageIteratorRelease(tesseract::PageIterator** iterator)
{
   delete *iterator;
   *iterator = 0;
}

Pix* leptCreatePixFromMat(cv::Mat* m)
{
   const unsigned char* imagedata = m->data;
   int width = m->size().width;
   int height = m->size().height;
   int bytes_per_pixel = m->channels();
   int bytes_per_line = m->step;

   //The following code is based on tesseract's ImageThresholder.SetImage function
   int bpp = bytes_per_pixel * 8;
   if (bpp == 0) bpp = 1;
   Pix* pix = pixCreate(width, height, bpp == 24 ? 32 : bpp);
   l_uint32* data = pixGetData(pix);
   int wpl = pixGetWpl(pix);
   switch (bpp) {
   case 1:
      for (int y = 0; y < height; ++y, data += wpl, imagedata += bytes_per_line) {
         for (int x = 0; x < width; ++x) {
            if (imagedata[x / 8] & (0x80 >> (x % 8)))
               CLEAR_DATA_BIT(data, x);
            else
               SET_DATA_BIT(data, x);
         }
      }
      break;

   case 8:
      // Greyscale just copies the bytes in the right order.
      for (int y = 0; y < height; ++y, data += wpl, imagedata += bytes_per_line) {
         for (int x = 0; x < width; ++x)
            SET_DATA_BYTE(data, x, imagedata[x]);
      }
      break;

   case 24:
      // Put the colors in the correct places in the line buffer.
      for (int y = 0; y < height; ++y, imagedata += bytes_per_line) {
         for (int x = 0; x < width; ++x, ++data) {
            SET_DATA_BYTE(data, COLOR_RED, imagedata[3 * x]);
            SET_DATA_BYTE(data, COLOR_GREEN, imagedata[3 * x + 1]);
            SET_DATA_BYTE(data, COLOR_BLUE, imagedata[3 * x + 2]);
         }
      }
      break;

   case 32:
      // Maintain byte order consistency across different endianness.
      for (int y = 0; y < height; ++y, imagedata += bytes_per_line, data += wpl) {
         for (int x = 0; x < width; ++x) {
            data[x] = (imagedata[x * 4] << 24) | (imagedata[x * 4 + 1] << 16) |
               (imagedata[x * 4 + 2] << 8) | imagedata[x * 4 + 3];
         }
      }
      break;

   default:
      CV_Error(CV_StsError, "Cannot convert RAW image to Pix\n");
   }
   pixSetYRes(pix, 300);
   return pix;
}