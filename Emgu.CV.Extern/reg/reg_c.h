//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_REG_C_H
#define EMGU_REG_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_REG
#include "opencv2/reg/map.hpp"
#include "opencv2/reg/mapshift.hpp"
#include "opencv2/reg/mapprojec.hpp"
#include "opencv2/reg/mapaffine.hpp"
#include "opencv2/reg/mapper.hpp"
#include "opencv2/reg/mappergradaffine.hpp"
#include "opencv2/reg/mappergradeuclid.hpp"
#include "opencv2/reg/mappergradproj.hpp"
#include "opencv2/reg/mappergradshift.hpp"
#include "opencv2/reg/mappergradsimilar.hpp"
#include "opencv2/reg/mapperpyramid.hpp"
#else
static inline CV_NORETURN void throw_no_reg() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without reg support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv
{
	namespace reg
	{
		class Map {};
		class Mapper {};
		class MapShift {};
		class MapProjec {};
		class MapAffine {};
		class MapperGradAffine {};
		class MapperGradEuclid {};
		class MapperGradProj {};
		class MapperGradShift {};
		class MapperGradSimilar {};
		class MapperPyramid {};
	}
}

#endif

CVAPI(void) cveMapWarp(
	cv::reg::Map* map,
	cv::_InputArray* img1, 
	cv::_OutputArray* img2);
CVAPI(void) cveMapInverseWarp(
	cv::reg::Map* map,
	cv::_InputArray* img1,
	cv::_OutputArray* img2);
CVAPI(void) cveMapScale(
	cv::reg::Map* map, 
	double factor);
CVAPI(void) cveMapRelease(cv::Ptr< cv::reg::Map >** mapSharedPtr);

CVAPI(cv::reg::MapShift*) cveMapShiftCreate(CvPoint2D64f* shift, cv::reg::Map** map);
CVAPI(void) cveMapShiftRelease(cv::reg::MapShift** mapShift);

CVAPI(cv::reg::MapProjec*) cveMapProjecCreate(cv::_InputArray* projTr, cv::reg::Map** map);
CVAPI(void) cveMapProjecRelease(cv::reg::MapProjec** mapProjec);

CVAPI(cv::reg::MapAffine*) cveMapAffineCreate(
	cv::_InputArray* linTr, 
	cv::_InputArray* shift, 
	cv::reg::Map** map);
CVAPI(void) cveMapAffineRelease(cv::reg::MapAffine** mapAffine);


CVAPI(cv::reg::Map*) cveMapperCalculate(
	cv::reg::Mapper* mapper,
	cv::_InputArray* img1,
	cv::_InputArray* img2,
	cv::reg::Map* init,
	cv::Ptr<cv::reg::Map>** sharedPtr);

CVAPI(cv::reg::MapperGradAffine*) cveMapperGradAffineCreate(cv::reg::Mapper** mapper);
CVAPI(void) cveMapperGradAffineRelease(cv::reg::MapperGradAffine** mapper);

CVAPI(cv::reg::MapperGradEuclid*) cveMapperGradEuclidCreate(cv::reg::Mapper** mapper);
CVAPI(void) cveMapperGradEuclidRelease(cv::reg::MapperGradEuclid** mapper);

CVAPI(cv::reg::MapperGradProj*) cveMapperGradProjCreate(cv::reg::Mapper** mapper);
CVAPI(void) cveMapperGradProjRelease(cv::reg::MapperGradProj** mapper);

CVAPI(cv::reg::MapperGradShift*) cveMapperGradShiftCreate(cv::reg::Mapper** mapper);
CVAPI(void) cveMapperGradShiftRelease(cv::reg::MapperGradShift** mapper);

CVAPI(cv::reg::MapperGradSimilar*) cveMapperGradSimilarCreate(cv::reg::Mapper** mapper);
CVAPI(void) cveMapperGradSimilarRelease(cv::reg::MapperGradSimilar** mapper);

CVAPI(cv::reg::MapperPyramid*) cveMapperPyramidCreate(cv::reg::Mapper* baseMapper, cv::reg::Mapper** mapper);
CVAPI(void) cveMapperPyramidRelease(cv::reg::MapperPyramid** mapper);

#endif