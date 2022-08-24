//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_TESSERACT_C_H
#define EMGU_TESSERACT_C_H


#if (_MSC_VER >= 1200)          
typedef __int64 INT64;
typedef unsigned __int64 UINT64;
#endif

#include "opencv2/core/core.hpp"
#include "opencv2/core/core_c.h"
#include "stdio.h"

#if HAVE_EMGUCV_TESSERACT


#include "tesseract/capi.h"


#include <clocale>

#include "image.h"
#include "thresholder.h"
#include "allheaders.h"

#define EmguTesseract tesseract::TessBaseAPI
/*
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

};*/

#else

class EmguTesseract {};
class Pix {};

namespace tesseract
{
	class TessResultRenderer {};
	class PageIterator {};
	class Orientation {};
	class WritingDirection {};
	class TextlineOrder {};
	class TessPDFRenderer {};
	
	enum PageSegMode {};
	enum PageIteratorLevel {};
}

static inline CV_NORETURN void throw_no_tesseract() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without tesseract ocr support"); }

#endif


struct TesseractResult
{
   int length;
   float confident;
   CvRect region;
};

namespace cv {
	namespace traits {
		template<>
		struct Depth < TesseractResult > { enum { value = Depth<uchar>::value }; };
		template<>
		struct Type< TesseractResult > { enum { value = CV_MAKETYPE(Depth<uchar>::value, sizeof(TesseractResult)) }; };
	}
}

CVAPI(const char*) cveTesseractGetVersion();

CVAPI(EmguTesseract*) cveTessBaseAPICreate();

CVAPI(int) cveTessBaseAPIInit(EmguTesseract* ocr, cv::String* dataPath, cv::String* language, int mode);

CVAPI(int) cveTessBaseAPIInitRaw(EmguTesseract* ocr, char* dataRaw, int size, cv::String* language, int mode);

CVAPI(void) cveTessBaseAPIRelease(EmguTesseract** ocr);

CVAPI(int) cveTessBaseAPIRecognize(EmguTesseract* ocr);

CVAPI(void) cveTessBaseAPISetImage(EmguTesseract* ocr, cv::_InputArray* mat);
CVAPI(void) cveTessBaseAPISetImagePix(EmguTesseract* ocr, Pix* pix);

CVAPI(void) cveTessBaseAPIGetUTF8Text(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte);

CVAPI(void) cveTessBaseAPIGetHOCRText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte);

CVAPI(void) cveTessBaseAPIGetTSVText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte);
CVAPI(void) cveTessBaseAPIGetBoxText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte);
CVAPI(void) cveTessBaseAPIGetUNLVText(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte);
CVAPI(void) cveTessBaseAPIGetOsdText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte);

CVAPI(void) cveTessBaseAPIExtractResult(EmguTesseract* ocr, std::vector<char>* charSeq, std::vector<TesseractResult>* resultSeq);

CVAPI(bool) cveTessBaseAPIProcessPage(
	EmguTesseract* ocr,
	Pix* pix, 
	int pageIndex, 
	cv::String* filename, 
	cv::String* retryConfig,
	int timeoutMillisec,
	tesseract::TessResultRenderer* renderer);

CVAPI(bool) cveTessBaseAPISetVariable(EmguTesseract* ocr, const char* varName, const char* value);

CVAPI(void) cveTessBaseAPISetPageSegMode(EmguTesseract* ocr, tesseract::PageSegMode mode);

CVAPI(tesseract::PageSegMode) cveTessBaseAPIGetPageSegMode(EmguTesseract* ocr);

CVAPI(int) cveTessBaseAPIGetOpenCLDevice(EmguTesseract* ocr, void **device);

CVAPI(tesseract::PageIterator*) cveTessBaseAPIAnalyseLayout(EmguTesseract* ocr, bool mergeSimilarWords);

CVAPI(void) cveTessPageIteratorGetOrientation(tesseract::PageIterator* iterator, tesseract::Orientation* orientation, tesseract::WritingDirection* writingDirection, tesseract::TextlineOrder* order, float* deskewAngle);

CVAPI(void) cveTessPageIteratorRelease(tesseract::PageIterator** iterator);

CVAPI(bool) cveTessPageIteratorGetBaseLine(
   tesseract::PageIterator* iterator,
   tesseract::PageIteratorLevel level,
   int* x1, int* y1, int* x2, int* y2);

CVAPI(int) cveTessBaseAPIIsValidWord(EmguTesseract* ocr, char* word);

CVAPI(int) cveTessBaseAPIGetOem(EmguTesseract* ocr);


CVAPI(tesseract::TessPDFRenderer*) cveTessPDFRendererCreate(cv::String* outputbase, cv::String* datadir, bool textonly, tesseract::TessResultRenderer** resultRenderer);
CVAPI(void) cveTessPDFRendererRelease(tesseract::TessPDFRenderer** renderer);


CVAPI(bool) cveTessResultRendererBeginDocument(tesseract::TessResultRenderer* resultRenderer, cv::String* title);
CVAPI(bool) cveTessResultRendererAddImage(tesseract::TessResultRenderer* resultRenderer, EmguTesseract* api);
CVAPI(bool) cveTessResultRendererEndDocument(tesseract::TessResultRenderer* resultRenderer);

CVAPI(Pix*) cveLeptCreatePixFromMat(cv::Mat* m);
CVAPI(void) cveLeptPixDestroy(Pix** pix);

CVAPI(char*) cveStdSetlocale(int category, char* locale);

CVAPI(void) cveTessBaseAPIGetDatapath(EmguTesseract* ocr, cv::String* datapath);

#endif
