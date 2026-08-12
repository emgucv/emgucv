//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "imgcodecs_c_extra.h"

bool cveHaveImageReader(cv::String* filename)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::haveImageReader(*filename);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
bool cveHaveImageWriter(cv::String* filename)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::haveImageWriter(*filename);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveImwrite(cv::String* filename, cv::_InputArray* img, std::vector<int>* params)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::imwrite(*filename, *img, params ? *params : std::vector<int>());
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveImwritemulti(cv::String* filename, cv::_InputArray* img, std::vector<int>* params)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::imwritemulti(*filename, *img, params ? *params : std::vector<int>());
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveImwriteWithMetadata(
	cv::String* filename,
	cv::_InputArray* img,
	std::vector<int>* metadataTypes,
	cv::_InputArray* metadata,
	std::vector<int>* params)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::imwriteWithMetadata(
			*filename, 
			*img,
			*metadataTypes,
			*metadata,
			params ? *params : std::vector<int>());
	#else
		throw_no_imgcodecs();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveImread(cv::String* fileName, int flags, cv::Mat* result)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		cv::Mat m = cv::imread(*fileName, flags);
		cv::swap(*result, m);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveImreadmulti(const cv::String* filename, std::vector<cv::Mat>* mats, int flags)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::imreadmulti(*filename, *mats, flags);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveImreadWithMetadata(
	const cv::String* filename,
	std::vector<int>* metadataTypes,
	cv::_OutputArray* metadata,
	int flags,
	cv::Mat* result)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		cv::Mat m = cv::imreadWithMetadata(
			*filename,
			*metadataTypes,
			*metadata,
			flags);
		cv::swap(*result, m);
	#else
		throw_no_imgcodecs();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveImdecode(cv::_InputArray* buf, int flags, cv::Mat* dst)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		cv::imdecode(*buf, flags, dst);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveImdecodemulti(cv::_InputArray* buf, int flags, std::vector<cv::Mat>* mats, cv::Range* range)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		if ((range->start == 0) && (range->end == 0))
			return cv::imdecodemulti(*buf, flags, *mats);
		else
			return cv::imdecodemulti(*buf, flags, *mats, *range);
	#else
		throw_no_imgcodecs();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveImdecodeWithMetadata(
	cv::_InputArray* buf,
	std::vector<int>* metadataTypes,
	cv::_OutputArray* metadata,
	int flags,
	cv::Mat* dst)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		cv::Mat result = cv::imdecodeWithMetadata(
			*buf,
			*metadataTypes,
			*metadata,
			flags
		);
		cv::swap(result, *dst);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveImencode(cv::String* ext, cv::_InputArray* img, std::vector< unsigned char >* buf, std::vector< int >* params)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::imencode(*ext, *img, *buf, params ? *params : std::vector<int>());
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveImencodemulti(cv::String* ext, cv::_InputArray* imgs, std::vector<uchar>* buf, std::vector<int>* params)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		if (params)
			return cv::imencodemulti(*ext, *imgs, *buf, *params);
		else
			return cv::imencodemulti(*ext, *imgs, *buf);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveImencodeWithMetadata(
	cv::String* ext,
	cv::_InputArray* img,
	std::vector< int >* metadataTypes,
	cv::_InputArray* metadata,
	std::vector< uchar >* buf,
	std::vector< int >* params)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::imencodeWithMetadata(
			*ext, 
			*img,
			*metadataTypes,
			*metadata,
			*buf, 
			params ? *params : std::vector<int>());
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

cv::Animation* cveAnimationCreate(int loopCount, cv::Scalar* bgColor)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return new cv::Animation(loopCount, *bgColor);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveAnimationRelease(cv::Animation** animation)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		delete* animation;
		*animation = 0;
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
std::vector<int>* cveAnimationGetDurations(cv::Animation* animation)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return &(animation->durations);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
std::vector<cv::Mat>* cveAnimationGetFrames(cv::Animation* animation)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return &(animation->frames);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

bool cveImreadAnimation(cv::String* filename, cv::Animation* animation, int start, int count)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::imreadanimation(*filename, *animation, start, count);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveImdecodeAnimation(cv::_InputArray* buf, cv::Animation* animation, int start, int count)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		return cv::imdecodeanimation(*buf, *animation, start, count);
	#else
		throw_no_imgcodecs();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveImwriteAnimation(cv::String* filename, cv::Animation* animation, std::vector<int>* params)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		if (params)
			return cv::imwriteanimation(*filename, *animation, *params);
		else
			return cv::imwriteanimation(*filename, *animation);
	#else
		throw_no_imgcodecs();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveImencodeAnimation(
	cv::String* ext,
	cv::Animation* animation,
	std::vector<uchar>* buf,
	std::vector<int>* params)
{
	try
	{
	#ifdef HAVE_OPENCV_IMGCODECS
		if (params)
			return cv::imencodeanimation(*ext, *animation, *buf, *params);
		else
			return cv::imencodeanimation(*ext, *animation, *buf);
	#else
		throw_no_imgcodecs();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
