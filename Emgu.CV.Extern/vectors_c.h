//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VECTORS_C_H
#define EMGU_VECTORS_C_H

#include <vector>
#include "opencv2/core/core_c.h"
#include "opencv2/features2d/features2d.hpp"
#include "opencv2/objdetect/objdetect.hpp"
#include "opencv2/objdetect/objdetect_c.h"

#ifdef HAVE_OPENCV_TEXT
#include "opencv2/text/erfilter.hpp"
#endif

#include "opencv2/line_descriptor.hpp"
#include "opencv2/core/ocl.hpp"

namespace cv {
	namespace traits {
		template<>
		struct Depth < cv::String > { enum { value = Depth<uchar>::value }; };
		template<>
		struct Type< cv::String > { enum { value = CV_MAKETYPE(Depth<uchar>::value, sizeof(cv::String)) }; };

		template<>
		struct Depth < cv::DMatch > { enum { value = Depth<uchar>::value }; };
		template<>
		struct Type< cv::DMatch > { enum { value = CV_MAKETYPE(Depth<uchar>::value, sizeof(cv::DMatch)) }; };

#ifdef HAVE_OPENCV_TEXT
		template<>
		struct Depth < cv::text::ERStat > { enum { value = Depth<uchar>::value }; };
		template<>
		struct Type< cv::text::ERStat > { enum { value = CV_MAKETYPE(Depth<uchar>::value, sizeof(cv::text::ERStat)) }; };
#endif

		template<>
		struct Depth < cv::line_descriptor::KeyLine > { enum { value = Depth<uchar>::value }; };
		template<>
		struct Type< cv::line_descriptor::KeyLine > { enum { value = CV_MAKETYPE(Depth<uchar>::value, sizeof(cv::line_descriptor::KeyLine)) }; };

		template<>
		struct Depth < cv::KeyPoint > { enum { value = Depth<uchar>::value }; };
		template<>
		struct Type< cv::KeyPoint > { enum { value = CV_MAKETYPE(Depth<uchar>::value, sizeof(cv::KeyPoint)) }; };

		template<>
		struct Depth < cv::ocl::PlatformInfo > { enum { value = Depth<uchar>::value }; };
		template<>
		struct Type< cv::ocl::PlatformInfo > { enum { value = CV_MAKETYPE(Depth<uchar>::value, sizeof(cv::ocl::PlatformInfo)) }; };
	}
}


template <class dataType> 
void VectorPushMulti(std::vector<dataType>* v, dataType* values, int count)
{
   if (count > 0)
   {
      size_t oldSize = v->size();
      v->resize(oldSize + count);
      memcpy(&(*v)[oldSize], values, count * sizeof(dataType));
   }
}

template <class dataType> 
void VectorCopyData(std::vector<dataType>* v, dataType* data)
{
   if (!v->empty())
      memcpy(data, &(*v)[0], v->size() * sizeof(dataType));
}

//----------------------------------------------------------------------------
//
//  Vector of DMatch
//
//----------------------------------------------------------------------------

CVAPI(void) VectorOfDMatchPushMatrix(std::vector<cv::DMatch>* matches, const CvMat* trainIdx, const CvMat* distance = 0, const CvMat* mask = 0);

CVAPI(void) VectorOfDMatchToMat(std::vector< std::vector<cv::DMatch> >* matches, CvMat* trainIdx, CvMat* distance);

//----------------------------------------------------------------------------
//
//  Vector of KeyPoint
//
//----------------------------------------------------------------------------
CVAPI(void) VectorOfKeyPointFilterByImageBorder( std::vector<cv::KeyPoint>* keypoints, CvSize imageSize, int borderSize );

CVAPI(void) VectorOfKeyPointFilterByKeypointSize( std::vector<cv::KeyPoint>* keypoints, float minSize, float maxSize);

CVAPI(void) VectorOfKeyPointFilterByPixelsMask( std::vector<cv::KeyPoint>* keypoints, CvMat* mask );

/*
//----------------------------------------------------------------------------
//
//  Vector of DataMatrixCode
//
//----------------------------------------------------------------------------
CVAPI(std::vector<CvDataMatrixCode>*) VectorOfDataMatrixCodeCreate();

CVAPI(std::vector<CvDataMatrixCode>*) VectorOfDataMatrixCodeCreateSize(int size);

CVAPI(int) VectorOfDataMatrixCodeGetSize(std::vector<CvDataMatrixCode>* v);

CVAPI(void) VectorOfDataMatrixCodePushMulti(std::vector<CvDataMatrixCode>* v, CvDataMatrixCode* values, int count);

CVAPI(void) VectorOfDataMatrixCodeClear(std::vector<CvDataMatrixCode>* v);

CVAPI(void) VectorOfDataMatrixCodeRelease(std::vector<CvDataMatrixCode>** v);

CVAPI(CvDataMatrixCode*) VectorOfDataMatrixCodeGetStartAddress(std::vector<CvDataMatrixCode>* v);

CVAPI(CvDataMatrixCode*) VectorOfDataMatrixCodeGetItem(std::vector<CvDataMatrixCode>* v, int index);

CVAPI(void) VectorOfDataMatrixCodeFind(std::vector<CvDataMatrixCode>* v, IplImage* image);

CVAPI(void) VectorOfDataMatrixCodeDraw(std::vector<CvDataMatrixCode>* v, IplImage* image);
*/

#endif