//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "tesseract_c.h"

const char* TesseractGetVersion()
{
#ifdef HAVE_EMGUCV_TESSERACT
#ifdef _WIN32 
	return tesseract::TessBaseAPI::Version();
#else
	return 0;
#endif
#else
	throw_no_tesseract();
#endif
}

EmguTesseract* TessBaseAPICreate()
{
#ifdef HAVE_EMGUCV_TESSERACT
	EmguTesseract* ocr = new EmguTesseract();
	return ocr;
#else
	throw_no_tesseract();
#endif
}

int TessBaseAPIInit(EmguTesseract* ocr, cv::String* dataPath, cv::String* language, int mode)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return ocr->Init(dataPath->c_str(), language->c_str(), (tesseract::OcrEngineMode) mode);
#else
	throw_no_tesseract();
#endif
}

void TessBaseAPIRelease(EmguTesseract** ocr)
{
#ifdef HAVE_EMGUCV_TESSERACT
	delete *ocr;
#else
	throw_no_tesseract();
#endif
}

int TessBaseAPIRecognize(EmguTesseract* ocr)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return ocr->Recognize(NULL);
#else
	throw_no_tesseract();
#endif
}

void TessBaseAPISetImage(EmguTesseract* ocr, cv::_InputArray* mat)
{
#ifdef HAVE_EMGUCV_TESSERACT
	cv::Mat m = mat->getMat();
	ocr->SetImage(static_cast<const unsigned char*>(m.data), m.size().width, m.size().height, m.elemSize(), m.step);
#else
	throw_no_tesseract();
#endif
}

void TessBaseAPISetImagePix(EmguTesseract* ocr, Pix* pix)
{
#ifdef HAVE_EMGUCV_TESSERACT
	ocr->SetImage(pix);
#else
	throw_no_tesseract();
#endif
}

void TessBaseAPIGetUTF8Text(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte)
{
#ifdef HAVE_EMGUCV_TESSERACT
	char* result = ocr->GetUTF8Text();
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
#else
	throw_no_tesseract();
#endif
}

void TessBaseAPIGetHOCRText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
#ifdef HAVE_EMGUCV_TESSERACT
	char* result = ocr->GetHOCRText(pageNumber);
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
#else
	throw_no_tesseract();
#endif
}

void TessBaseAPIGetTSVText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
#ifdef HAVE_EMGUCV_TESSERACT
	char* result = ocr->GetTSVText(pageNumber);
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
#else
	throw_no_tesseract();
#endif
}
void TessBaseAPIGetBoxText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
#ifdef HAVE_EMGUCV_TESSERACT
	char* result = ocr->GetBoxText(pageNumber);
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
#else
	throw_no_tesseract();
#endif
}
void TessBaseAPIGetUNLVText(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte)
{
#ifdef HAVE_EMGUCV_TESSERACT
	char* result = ocr->GetUNLVText();
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
#else
	throw_no_tesseract();
#endif
}
void TessBaseAPIGetOsdText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
#ifdef HAVE_EMGUCV_TESSERACT
	char* result = ocr->GetOsdText(pageNumber);
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
#else
	throw_no_tesseract();
#endif
}

void TessBaseAPIExtractResult(EmguTesseract* ocr, std::vector<unsigned char>* charSeq, std::vector<TesseractResult>* resultSeq)
{
#ifdef HAVE_EMGUCV_TESSERACT
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
	}

	charSeq->resize(totalTextLength);
	if (n > 0)
	{
		memcpy(&(*charSeq)[0], text, totalTextLength);
	}

	delete[] text;
	delete[] lengths;
	delete[] x0;
	delete[] y0;
	delete[] x1;
	delete[] y1;
#else
	throw_no_tesseract();
#endif
}

bool TessBaseAPIProcessPage(
	EmguTesseract* ocr,
	Pix* pix,
	int pageIndex,
	cv::String* filename,
	cv::String* retryConfig,
	int timeoutMillisec,
	tesseract::TessResultRenderer* renderer)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return ocr->ProcessPage(pix, pageIndex, filename->c_str(), retryConfig->empty() ? 0 : retryConfig->c_str(), timeoutMillisec, renderer);
#else
	throw_no_tesseract();
#endif
}

bool TessBaseAPISetVariable(EmguTesseract* ocr, const char* varName, const char* value)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return ocr->SetVariable(varName, value);
#else
	throw_no_tesseract();
#endif
}

void TessBaseAPISetPageSegMode(EmguTesseract* ocr, tesseract::PageSegMode mode)
{
#ifdef HAVE_EMGUCV_TESSERACT
	ocr->SetPageSegMode(mode);
#else
	throw_no_tesseract();
#endif
}

tesseract::PageSegMode TessBaseAPIGetPageSegMode(EmguTesseract* ocr)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return ocr->GetPageSegMode();
#else
	throw_no_tesseract();
#endif
}

int TessBaseAPIGetOpenCLDevice(EmguTesseract* ocr, void **device)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return static_cast<int>(ocr->getOpenCLDevice(device));
#else
	throw_no_tesseract();
#endif
}

tesseract::PageIterator* TessBaseAPIAnalyseLayout(EmguTesseract* ocr, bool mergeSimilarWords)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return ocr->AnalyseLayout(mergeSimilarWords);
#else
	throw_no_tesseract();
#endif
}

void TessPageIteratorGetOrientation(tesseract::PageIterator* iterator, tesseract::Orientation* orientation, tesseract::WritingDirection* writingDirection, tesseract::TextlineOrder* order, float* deskewAngle)
{
#ifdef HAVE_EMGUCV_TESSERACT
	iterator->Orientation(orientation, writingDirection, order, deskewAngle);
#else
	throw_no_tesseract();
#endif
}

bool TessPageIteratorGetBaseLine(
	tesseract::PageIterator* iterator,
	tesseract::PageIteratorLevel level,
	int* x1, int* y1, int* x2, int* y2)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return iterator->Baseline(level, x1, y1, x2, y2);
#else
	throw_no_tesseract();
#endif
}

void TessPageIteratorRelease(tesseract::PageIterator** iterator)
{
#ifdef HAVE_EMGUCV_TESSERACT
	delete *iterator;
	*iterator = 0;
#else
	throw_no_tesseract();
#endif
}

int TessBaseAPIIsValidWord(EmguTesseract* ocr, char* word)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return ocr->IsValidWord(word);
#else
	throw_no_tesseract();
#endif
}

int TessBaseAPIGetOem(EmguTesseract* ocr)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return ocr->oem();
#else
	throw_no_tesseract();
#endif
}

tesseract::TessPDFRenderer* TessPDFRendererCreate(cv::String* outputbase, cv::String* datadir, bool textonly, tesseract::TessResultRenderer** resultRenderer)
{
#ifdef HAVE_EMGUCV_TESSERACT
	tesseract::TessPDFRenderer* renderer = new tesseract::TessPDFRenderer(outputbase->c_str(), datadir->c_str(), textonly);
	*resultRenderer = static_cast<tesseract::TessResultRenderer*>(renderer);
	return renderer;
#else
	throw_no_tesseract();
#endif
}
void TessPDFRendererRelease(tesseract::TessPDFRenderer** renderer)
{
#ifdef HAVE_EMGUCV_TESSERACT
	delete *renderer;
	*renderer = 0;
#else
	throw_no_tesseract();
#endif
}

Pix* leptCreatePixFromMat(cv::Mat* m)
{
#ifdef HAVE_EMGUCV_TESSERACT
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
#else
	throw_no_tesseract();
#endif
}

void leptPixDestroy(Pix** pix)
{
#ifdef HAVE_EMGUCV_TESSERACT
	pixDestroy(pix);
	*pix = 0;
#else
	throw_no_tesseract();
#endif
}

char* stdSetlocale(int category, char* locale)
{
#ifdef HAVE_EMGUCV_TESSERACT
	return std::setlocale(category, locale);
#else
	throw_no_tesseract();
#endif
}