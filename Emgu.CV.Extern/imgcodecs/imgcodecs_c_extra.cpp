//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "imgcodecs_c_extra.h"

bool cveHaveImageReader(cv::String* filename)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return cv::haveImageReader(*filename);
#else
	throw_no_imgcodecs();
#endif
}
bool cveHaveImageWriter(cv::String* filename)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return cv::haveImageWriter(*filename);
#else
	throw_no_imgcodecs();
#endif
}

bool cveImwrite(cv::String* filename, cv::_InputArray* img, std::vector<int>* params)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return cv::imwrite(*filename, *img, params ? *params : std::vector<int>());
#else
	throw_no_imgcodecs();
#endif
}

bool cveImwritemulti(cv::String* filename, cv::_InputArray* img, std::vector<int>* params)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return cv::imwritemulti(*filename, *img, params ? *params : std::vector<int>());
#else
	throw_no_imgcodecs();
#endif
}

bool cveImwriteWithMetadata(
	cv::String* filename,
	cv::_InputArray* img,
	std::vector<int>* metadataTypes,
	cv::_InputArray* metadata,
	std::vector<int>* params)
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

void cveImread(cv::String* fileName, int flags, cv::Mat* result)
{
#ifdef HAVE_OPENCV_IMGCODECS
	cv::Mat m = cv::imread(*fileName, flags);
	cv::swap(*result, m);
#else
	throw_no_imgcodecs();
#endif
}

bool cveImreadmulti(const cv::String* filename, std::vector<cv::Mat>* mats, int flags)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return cv::imreadmulti(*filename, *mats, flags);
#else
	throw_no_imgcodecs();
#endif
}

void cveImreadWithMetadata(
	const cv::String* filename,
	std::vector<int>* metadataTypes,
	cv::_OutputArray* metadata,
	int flags,
	cv::Mat* result)
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

void cveImdecode(cv::_InputArray* buf, int flags, cv::Mat* dst)
{
#ifdef HAVE_OPENCV_IMGCODECS
	cv::imdecode(*buf, flags, dst);
#else
	throw_no_imgcodecs();
#endif
}

bool cveImdecodemulti(cv::_InputArray* buf, int flags, std::vector<cv::Mat>* mats, cv::Range* range)
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

void cveImdecodeWithMetadata(
	cv::_InputArray* buf,
	std::vector<int>* metadataTypes,
	cv::_OutputArray* metadata,
	int flags,
	cv::Mat* dst)
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

bool cveImencode(cv::String* ext, cv::_InputArray* img, std::vector< unsigned char >* buf, std::vector< int >* params)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return cv::imencode(*ext, *img, *buf, params ? *params : std::vector<int>());
#else
	throw_no_imgcodecs();
#endif
}

bool cveImencodemulti(cv::String* ext, cv::_InputArray* imgs, std::vector<uchar>* buf, std::vector<int>* params)
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

bool cveImencodeWithMetadata(
	cv::String* ext,
	cv::_InputArray* img,
	std::vector< int >* metadataTypes,
	cv::_InputArray* metadata,
	std::vector< uchar >* buf,
	std::vector< int >* params)
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

cv::Animation* cveAnimationCreate(int loopCount, cv::Scalar* bgColor)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return new cv::Animation(loopCount, *bgColor);
#else
	throw_no_imgcodecs();
#endif
}
void cveAnimationRelease(cv::Animation** animation)
{
#ifdef HAVE_OPENCV_IMGCODECS
	delete* animation;
	*animation = 0;
#else
	throw_no_imgcodecs();
#endif
}
std::vector<int>* cveAnimationGetDurations(cv::Animation* animation)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return &(animation->durations);
#else
	throw_no_imgcodecs();
#endif
}
std::vector<cv::Mat>* cveAnimationGetFrames(cv::Animation* animation)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return &(animation->frames);
#else
	throw_no_imgcodecs();
#endif
}

bool cveImreadAnimation(cv::String* filename, cv::Animation* animation, int start, int count)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return cv::imreadanimation(*filename, *animation, start, count);
#else
	throw_no_imgcodecs();
#endif
}

bool cveImdecodeAnimation(cv::_InputArray* buf, cv::Animation* animation, int start, int count)
{
#ifdef HAVE_OPENCV_IMGCODECS
	return cv::imdecodeanimation(*buf, *animation, start, count);
#else
	throw_no_imgcodecs();
#endif	
}

bool cveImwriteAnimation(cv::String* filename, cv::Animation* animation, std::vector<int>* params)
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

bool cveImencodeAnimation(
	cv::String* ext,
	cv::Animation* animation,
	std::vector<uchar>* buf,
	std::vector<int>* params)
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