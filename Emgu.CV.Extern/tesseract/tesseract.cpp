//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------


#include "tesseract_c.h"

const char* cveTesseractGetVersion()
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}

EmguTesseract* cveTessBaseAPICreate()
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		EmguTesseract* ocr = new EmguTesseract();
		return ocr;
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int cveTessBaseAPIInit(EmguTesseract* ocr, cv::String* dataPath, cv::String* language, int mode)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return ocr->Init(dataPath->c_str(), language->c_str(), (tesseract::OcrEngineMode) mode);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(-1)
}

int cveTessBaseAPIInitRaw(EmguTesseract* ocr, char* dataRaw, int size, cv::String* language, int mode)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return ocr->Init(dataRaw, size, language->c_str(), (tesseract::OcrEngineMode)mode, nullptr, 0, nullptr, nullptr, false, nullptr);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(-1)
}

void cveTessBaseAPIRelease(EmguTesseract** ocr)
{
#ifdef HAVE_EMGUCV_TESSERACT
	delete *ocr;
	*ocr = 0;
#else
	throw_no_tesseract();
#endif
}

int cveTessBaseAPIRecognize(EmguTesseract* ocr)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return ocr->Recognize(NULL);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(-1)
}

void cveTessBaseAPISetImage(EmguTesseract* ocr, cv::_InputArray* mat)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		cv::Mat m = mat->getMat();
		ocr->SetImage(static_cast<const unsigned char*>(m.data), m.size().width, m.size().height, (int) m.elemSize(), (int) m.step);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveTessBaseAPISetImagePix(EmguTesseract* ocr, Pix* pix)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		ocr->SetImage(pix);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveTessBaseAPIGetUTF8Text(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveTessBaseAPIGetHOCRText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveTessBaseAPIGetTSVText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveTessBaseAPIGetBoxText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveTessBaseAPIGetUNLVText(EmguTesseract* ocr, std::vector<unsigned char>* vectorOfByte)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveTessBaseAPIGetOsdText(EmguTesseract* ocr, int pageNumber, std::vector<unsigned char>* vectorOfByte)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveTessBaseAPIExtractResult(EmguTesseract* ocr, std::vector<char>* charSeq, std::vector<TesseractResult>* resultSeq)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		if (ocr == NULL)
			return;

		tesseract::ResultIterator* results = ocr->GetIterator();
		if (!results)
			return;

		results->Begin();

		int x0, y0, x1, y1;

		do {
			if (results->Empty(tesseract::RIL_PARA)) {
				continue;
			}
			do
			{
				TesseractResult tr;
				tr.confident = results->Confidence(tesseract::RIL_WORD);
				results->BoundingBox(tesseract::RIL_WORD, &x0, &y0, &x1, &y1);
				tr.region.x = x0;
				//tr.region.y = height - y1;
				tr.region.y = y0;
				tr.region.width = x1 - x0;
				tr.region.height = y1 - y0;
				char* t = results->GetUTF8Text(tesseract::RIL_WORD);
				tr.length = strlen(t);
				for (int i = 0; i < tr.length; i++)
					charSeq->push_back(*(t + i));
				delete[] t;
				resultSeq->push_back(tr);
			} while (results->Next(tesseract::RIL_WORD));
		} while (results->Next(tesseract::RIL_PARA));


		delete results;
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveTessBaseAPIProcessPage(
	EmguTesseract* ocr,
	Pix* pix,
	int pageIndex,
	cv::String* filename,
	cv::String* retryConfig,
	int timeoutMillisec,
	tesseract::TessResultRenderer* renderer)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return ocr->ProcessPage(pix, pageIndex, filename->c_str(), retryConfig->empty() ? 0 : retryConfig->c_str(), timeoutMillisec, renderer);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveTessBaseAPISetVariable(EmguTesseract* ocr, const char* varName, const char* value)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return ocr->SetVariable(varName, value);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveTessBaseAPISetPageSegMode(EmguTesseract* ocr, tesseract::PageSegMode mode)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		ocr->SetPageSegMode(mode);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

tesseract::PageSegMode cveTessBaseAPIGetPageSegMode(EmguTesseract* ocr)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return ocr->GetPageSegMode();
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(static_cast<tesseract::PageSegMode>(0))
}

tesseract::PageIterator* cveTessBaseAPIAnalyseLayout(EmguTesseract* ocr, bool mergeSimilarWords)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return ocr->AnalyseLayout(mergeSimilarWords);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveTessPageIteratorGetOrientation(tesseract::PageIterator* iterator, tesseract::Orientation* orientation, tesseract::WritingDirection* writingDirection, tesseract::TextlineOrder* order, float* deskewAngle)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		iterator->Orientation(orientation, writingDirection, order, deskewAngle);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveTessPageIteratorGetBaseLine(
	tesseract::PageIterator* iterator,
	tesseract::PageIteratorLevel level,
	int* x1, int* y1, int* x2, int* y2)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return iterator->Baseline(level, x1, y1, x2, y2);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveTessPageIteratorRelease(tesseract::PageIterator** iterator)
{
#ifdef HAVE_EMGUCV_TESSERACT
	delete *iterator;
	*iterator = 0;
#else
	throw_no_tesseract();
#endif
}

int cveTessBaseAPIIsValidWord(EmguTesseract* ocr, char* word)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return ocr->IsValidWord(word);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int cveTessBaseAPIGetOem(EmguTesseract* ocr)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return ocr->oem();
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(-1)
}

tesseract::TessPDFRenderer* cveTessPDFRendererCreate(cv::String* outputbase, cv::String* datadir, bool textonly, tesseract::TessResultRenderer** resultRenderer)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		tesseract::TessPDFRenderer* renderer = new tesseract::TessPDFRenderer(outputbase->c_str(), datadir->c_str(), textonly);
		*resultRenderer = static_cast<tesseract::TessResultRenderer*>(renderer);
		return renderer;
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveTessPDFRendererRelease(tesseract::TessPDFRenderer** renderer)
{
#ifdef HAVE_EMGUCV_TESSERACT
	delete *renderer;
	*renderer = 0;
#else
	throw_no_tesseract();
#endif
}

bool cveTessResultRendererBeginDocument(tesseract::TessResultRenderer* resultRenderer, cv::String* title)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return resultRenderer->BeginDocument(title->c_str());
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveTessResultRendererAddImage(tesseract::TessResultRenderer* resultRenderer, EmguTesseract* api)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return resultRenderer->AddImage(api);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveTessResultRendererEndDocument(tesseract::TessResultRenderer* resultRenderer)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return resultRenderer->EndDocument();
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveTessResultRendererHappy(tesseract::TessResultRenderer* resultRenderer)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return resultRenderer->happy();
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

int cveTessResultRendererImageNum(tesseract::TessResultRenderer* resultRenderer)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return resultRenderer->imagenum();
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}


Pix* cveLeptCreatePixFromMat(cv::Mat* m)
{
	try
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
			CV_Error(cv::Error::StsError, "Cannot convert RAW image to Pix\n");
		}
		pixSetYRes(pix, 300);
		return pix;
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveLeptPixDestroy(Pix** pix)
{
#ifdef HAVE_EMGUCV_TESSERACT
	pixDestroy(pix);
	*pix = 0;
#else
	throw_no_tesseract();
#endif
}

char* cveStdSetlocale(int category, char* locale)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		return std::setlocale(category, locale);
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveTessBaseAPIGetDatapath(EmguTesseract* ocr, cv::String* datapath)
{
	try
	{
	#ifdef HAVE_EMGUCV_TESSERACT
		if (ocr->tesseract())
			*datapath = ocr->GetDatapath();
	#else
		throw_no_tesseract();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
