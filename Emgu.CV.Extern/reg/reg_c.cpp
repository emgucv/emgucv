//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "reg_c.h"

void cveMapWarp(
	cv::reg::Map* map,
	cv::_InputArray* img1,
	cv::_OutputArray* img2)
{
#ifdef HAVE_OPENCV_REG
	map->warp(*img1, *img2);
#else
	throw_no_reg();
#endif
}

void cveMapInverseWarp(
	cv::reg::Map* map,
	cv::_InputArray* img1,
	cv::_OutputArray* img2)
{
#ifdef HAVE_OPENCV_REG
	map->inverseWarp(*img1, *img2);
#else
	throw_no_reg();
#endif
}

void cveMapScale(
	cv::reg::Map* map,
	double factor)
{
#ifdef HAVE_OPENCV_REG
	map->scale(factor);
#else
	throw_no_reg();
#endif
}

void cveMapRelease(cv::Ptr< cv::reg::Map >** mapSharedPtr)
{
	delete* mapSharedPtr;
	*mapSharedPtr = 0;
}


cv::reg::MapShift* cveMapShiftCreate(CvPoint2D64f* shift, cv::reg::Map** map)
{
#ifdef HAVE_OPENCV_REG
	cv::Vec<double, 2> s(shift->x, shift->y);
	cv::reg::MapShift* ptr = new cv::reg::MapShift(s);
	*map = dynamic_cast<cv::reg::Map*>(ptr);
	return ptr;
#else
	throw_no_reg();
#endif
}

void cveMapShiftRelease(cv::reg::MapShift** mapShift)
{
#ifdef HAVE_OPENCV_REG
	delete* mapShift;
	*mapShift = 0;
#else
	throw_no_reg();
#endif
}

cv::reg::MapProjec* cveMapProjecCreate(cv::_InputArray* projTr, cv::reg::Map** map)
{
#ifdef HAVE_OPENCV_REG
	cv::reg::MapProjec* ptr = new cv::reg::MapProjec(*projTr);
	*map = dynamic_cast<cv::reg::Map*>(ptr);
	return ptr;
#else
	throw_no_reg();
#endif
}

void cveMapProjecRelease(cv::reg::MapProjec** mapProjec)
{
#ifdef HAVE_OPENCV_REG
	delete* mapProjec;
	*mapProjec = 0;
#else
	throw_no_reg();
#endif
}

cv::reg::MapAffine* cveMapAffineCreate(
	cv::_InputArray* linTr,
	cv::_InputArray* shift,
	cv::reg::Map** map)
{
#ifdef HAVE_OPENCV_REG
	cv::reg::MapAffine* ptr = new cv::reg::MapAffine(*linTr, *shift);
	*map = dynamic_cast<cv::reg::Map*>(ptr);
	return ptr;
#else
	throw_no_reg();
#endif	
}
void cveMapAffineRelease(cv::reg::MapAffine** mapAffine)
{
#ifdef HAVE_OPENCV_REG
	delete* mapAffine;
	*mapAffine = 0;
#else
	throw_no_reg();
#endif
}

cv::reg::Map* cveMapperCalculate(
	cv::reg::Mapper* mapper,
	cv::_InputArray* img1,
	cv::_InputArray* img2,
	cv::reg::Map* init,
	cv::Ptr<cv::reg::Map>** sharedPtr)
{
#ifdef HAVE_OPENCV_REG
	cv::Ptr<cv::reg::Map> newMap;
	if (init)
	{
		cv::Ptr<cv::reg::Map> p(init, [](cv::reg::Map* ptr) {});
		newMap = mapper->calculate(*img1, *img2, p);
	} else
	{
		newMap = mapper->calculate(*img1, *img2);
	}
	*sharedPtr = new cv::Ptr<cv::reg::Map>(newMap);
	return (*sharedPtr)->get();
#else
	throw_no_reg();
#endif
}

cv::reg::MapperGradAffine* cveMapperGradAffineCreate(cv::reg::Mapper** mapper)
{
#ifdef HAVE_OPENCV_REG
	cv::reg::MapperGradAffine* mapperGradAffine = new cv::reg::MapperGradAffine();
	*mapper = dynamic_cast<cv::reg::Mapper*>(mapperGradAffine);
	return mapperGradAffine;
#else
	throw_no_reg();
#endif	

}
void cveMapperGradAffineRelease(cv::reg::MapperGradAffine** mapper)
{
#ifdef HAVE_OPENCV_REG
	delete* mapper;
	*mapper = 0;
#else
	throw_no_reg();
#endif	
}


cv::reg::MapperGradEuclid* cveMapperGradEuclidCreate(cv::reg::Mapper** mapper)
{
#ifdef HAVE_OPENCV_REG
	cv::reg::MapperGradEuclid* mapperGradEuclid = new cv::reg::MapperGradEuclid();
	*mapper = dynamic_cast<cv::reg::Mapper*>(mapperGradEuclid);
	return mapperGradEuclid;
#else
	throw_no_reg();
#endif	
}
void cveMapperGradEuclidRelease(cv::reg::MapperGradEuclid** mapper)
{
#ifdef HAVE_OPENCV_REG
	delete* mapper;
	*mapper = 0;
#else
	throw_no_reg();
#endif		
}


cv::reg::MapperGradProj* cveMapperGradProjCreate(cv::reg::Mapper** mapper)
{
#ifdef HAVE_OPENCV_REG
	cv::reg::MapperGradProj* mapperGradProj = new cv::reg::MapperGradProj();
	*mapper = dynamic_cast<cv::reg::Mapper*>(mapperGradProj);
	return mapperGradProj;
#else
	throw_no_reg();
#endif
}

void cveMapperGradProjRelease(cv::reg::MapperGradProj** mapper)
{
#ifdef HAVE_OPENCV_REG
	delete* mapper;
	*mapper = 0;
#else
	throw_no_reg();
#endif	
}

cv::reg::MapperGradShift* cveMapperGradShiftCreate(cv::reg::Mapper** mapper)
{
#ifdef HAVE_OPENCV_REG
	cv::reg::MapperGradShift* mapperGradShift = new cv::reg::MapperGradShift();
	*mapper = dynamic_cast<cv::reg::Mapper*>(mapperGradShift);
	return mapperGradShift;
#else
	throw_no_reg();
#endif
}

void cveMapperGradShiftRelease(cv::reg::MapperGradShift** mapper)
{
#ifdef HAVE_OPENCV_REG
	delete* mapper;
	*mapper = 0;
#else
	throw_no_reg();
#endif	
}

cv::reg::MapperGradSimilar* cveMapperGradSimilarCreate(cv::reg::Mapper** mapper)
{
#ifdef HAVE_OPENCV_REG
	cv::reg::MapperGradSimilar* mapperGradSimilar = new cv::reg::MapperGradSimilar();
	*mapper = dynamic_cast<cv::reg::Mapper*>(mapperGradSimilar);
	return mapperGradSimilar;
#else
	throw_no_reg();
#endif	
}
void cveMapperGradSimilarRelease(cv::reg::MapperGradSimilar** mapper)
{
#ifdef HAVE_OPENCV_REG
	delete* mapper;
	*mapper = 0;
#else
	throw_no_reg();
#endif	
}

cv::reg::MapperPyramid* cveMapperPyramidCreate(cv::reg::Mapper* baseMapper, cv::reg::Mapper** mapper)
{
#ifdef HAVE_OPENCV_REG
	cv::Ptr<cv::reg::Mapper> m(baseMapper, [](cv::reg::Mapper* ptr) {});
	cv::reg::MapperPyramid* mapperPyramid = new cv::reg::MapperPyramid(m);
	*mapper = dynamic_cast<cv::reg::Mapper*>(mapperPyramid);
	return mapperPyramid;
#else
	throw_no_reg();
#endif
}
void cveMapperPyramidRelease(cv::reg::MapperPyramid** mapper)
{
#ifdef HAVE_OPENCV_REG
	delete* mapper;
	*mapper = 0;
#else
	throw_no_reg();
#endif	
}
