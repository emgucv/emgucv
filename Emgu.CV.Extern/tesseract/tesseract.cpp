//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
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

int TessBaseAPIInit(EmguTesseract* ocr, cv::String* dataPath, cv::String* language, int mode)
{
	return ocr->Init(dataPath->c_str(), language->c_str(), (tesseract::OcrEngineMode) mode);
}

void TessBaseAPIRelease(EmguTesseract** ocr)
{
	delete *ocr;
}

int TessBaseAPIRecognize(EmguTesseract* ocr)
{
	return ocr->Recognize(NULL);
}

void TessBaseAPISetImage(EmguTesseract* ocr, cv::_InputArray* mat)
{
	cv::Mat m = mat->getMat();
	ocr->SetImage(static_cast<const unsigned char*>(m.data), m.size().width, m.size().height, m.elemSize(), m.step);
}

void TessBaseAPISetImagePix(EmguTesseract* ocr, Pix* pix)
{
	ocr->SetImage(pix);
}

void TessBaseAPIGetUTF8Text(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte)
{
	char* result = ocr->GetUTF8Text();
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
}

void TessBaseAPIGetHOCRText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
	char* result = ocr->GetHOCRText(pageNumber);
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
}

void TessBaseAPIGetTSVText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
	char* result = ocr->GetTSVText(pageNumber);
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
}
void TessBaseAPIGetBoxText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
	char* result = ocr->GetBoxText(pageNumber);
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
}
void TessBaseAPIGetUNLVText(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte)
{
	char* result = ocr->GetUNLVText();
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
		memcpy(&(*vectorOfByte)[0], result, length);
	delete[] result;
}
void TessBaseAPIGetOsdText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
	char* result = ocr->GetOsdText(pageNumber);
	size_t length = strlen(result);
	vectorOfByte->resize(length);
	if (length > 0)
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
	return ocr->ProcessPage(pix, pageIndex, filename->c_str(), retryConfig->empty() ? 0 : retryConfig->c_str(), timeoutMillisec, renderer);
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

int TessBaseAPIIsValidWord(EmguTesseract* ocr, char* word)
{
	return ocr->IsValidWord(word);
}

int TessBaseAPIGetOem(EmguTesseract* ocr)
{
	return ocr->oem();
}

tesseract::TessPDFRenderer* TessPDFRendererCreate(cv::String* outputbase, cv::String* datadir, bool textonly, tesseract::TessResultRenderer** resultRenderer)
{
	tesseract::TessPDFRenderer* renderer = new tesseract::TessPDFRenderer(outputbase->c_str(), datadir->c_str(), textonly);
	*resultRenderer = static_cast<tesseract::TessResultRenderer*>(renderer);
	return renderer;
}
void TessPDFRendererRelease(tesseract::TessPDFRenderer** renderer)
{
	delete *renderer;
	*renderer = 0;
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

void leptPixDestroy(Pix** pix)
{
	pixDestroy(pix);
	*pix = 0;
}