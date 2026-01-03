//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "vectors_c.h"

/*
void VectorOfDMatchPushMatrix(std::vector<cv::DMatch>* matches, const cv::Mat* trainIdx, const cv::Mat* distances, const cv::Mat* mask)
{
	CV_Assert(trainIdx->step == trainIdx->cols * sizeof(int));
	CV_Assert(!distances || (distances->step == distances->cols * sizeof(float)));
	CV_Assert(!mask || (mask->step == mask->cols * sizeof(unsigned char)));

	const int* trainIdx_ptr = trainIdx->ptr<int>();
	const float* distance_ptr = distances ? distances->ptr<float>() : 0;
	const unsigned char* mask_ptr = mask ? mask->data : 0;

	for (int queryIdx = 0; queryIdx < trainIdx->rows; ++queryIdx, ++trainIdx_ptr)
	{
		if (*trainIdx_ptr != -1 && (mask_ptr == 0 || *mask_ptr))
		{
			cv::DMatch m(queryIdx, *trainIdx_ptr, 0, distance_ptr ? *distance_ptr : -1);
			matches->push_back(m);
		}

		if (mask_ptr)
			++mask_ptr;

		if (distance_ptr)
			++distance_ptr;
	}
}

void VectorOfDMatchToMat(std::vector< std::vector<cv::DMatch> >* matches, cv::Mat* trainIdx, cv::Mat* distance)
{
	CV_Assert(trainIdx->rows == (int)matches->size() && trainIdx->step == trainIdx->cols * sizeof(int));
	CV_Assert(distance->rows == (int)matches->size() && distance->step == distance->cols * sizeof(float));

	int k = trainIdx->cols;
	float* distance_ptr = distance->ptr<float>();
	int* trainIdx_ptr = trainIdx->ptr<int>();
	for (std::vector< std::vector<cv::DMatch> >::iterator v = matches->begin(); v != matches->end(); ++v)
	{
		int idx = 0;
		if (!v->empty())
		{
			for (std::vector< cv::DMatch >::iterator m = v->begin(); m != v->end() && idx < k; ++m, ++idx)
			{
				*distance_ptr++ = m->distance;
				*trainIdx_ptr++ = m->trainIdx;
			}
		}
		for (; idx < k; ++idx)
		{
			*trainIdx_ptr++ = -1;
			*distance_ptr++ = -1;
		}
	}
}
*/

//----------------------------------------------------------------------------
//
//  Vector of KeyPoint
//
//----------------------------------------------------------------------------

void VectorOfKeyPointFilterByImageBorder(std::vector<cv::KeyPoint>* keypoints, cv::Size imageSize, int borderSize)
{
#ifdef HAVE_OPENCV_FEATURES
	cv::KeyPointsFilter::runByImageBorder(*keypoints, imageSize, borderSize);
#else
	vectors_throw_no_features();
#endif
}

void VectorOfKeyPointFilterByKeypointSize(std::vector<cv::KeyPoint>* keypoints, float minSize, float maxSize)
{
#ifdef HAVE_OPENCV_FEATURES
	cv::KeyPointsFilter::runByKeypointSize(*keypoints, minSize, maxSize);
#else
	vectors_throw_no_features();
#endif
}

void VectorOfKeyPointFilterByPixelsMask(std::vector<cv::KeyPoint>* keypoints, cv::Mat* mask)
{
#ifdef HAVE_OPENCV_FEATURES
	//cv::Mat m = cv::cvarrToMat(mask);
	cv::KeyPointsFilter::runByPixelsMask(*keypoints, *mask);
#else
	vectors_throw_no_features();
#endif
}
