//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "structured_light_c.h"


bool cveStructuredLightPatternGenerate(cv::structured_light::StructuredLightPattern* structuredLight, cv::_OutputArray* patternImages)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    return structuredLight->generate(*patternImages);
#else
    throw_no_structured_light();
#endif
}

bool cveStructuredLightPatternDecode(
    cv::structured_light::StructuredLightPattern* structuredLight,
    std::vector< std::vector< cv::Mat > >* patternImages,
    cv::_OutputArray* disparityMap,
    cv::_InputArray* blackImages,
    cv::_InputArray* whiteImages,
    int flags)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    return structuredLight->decode(
        *patternImages, 
        *disparityMap, 
        blackImages ? *blackImages : static_cast<cv::InputArray>(cv::noArray()),
        whiteImages ? *whiteImages : static_cast<cv::InputArray>(cv::noArray()), 
        flags);
#else
    throw_no_structured_light();
#endif
}

cv::structured_light::GrayCodePattern* cveGrayCodePatternCreate(
    int width,
    int height,
    cv::Ptr<cv::structured_light::GrayCodePattern>** sharedPtr,
    cv::structured_light::StructuredLightPattern** structuredLightPattern,
    cv::Algorithm** algorithm)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    cv::structured_light::GrayCodePattern::Params p;
    p.width = width;
    p.height = height;
    cv::Ptr<cv::structured_light::GrayCodePattern> pattern = cv::structured_light::GrayCodePattern::create(p);
    *sharedPtr = new cv::Ptr<cv::structured_light::GrayCodePattern>(pattern);
    cv::structured_light::GrayCodePattern* patternPtr = (*sharedPtr)->get();
    *structuredLightPattern = dynamic_cast<cv::structured_light::StructuredLightPattern*>(patternPtr);
    *algorithm = dynamic_cast<cv::Algorithm*>(patternPtr);
    return patternPtr;
#else
    throw_no_structured_light();
#endif
}
void cveGrayCodePatternRelease(cv::Ptr<cv::structured_light::GrayCodePattern>** sharedPtr)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    delete* sharedPtr;
    *sharedPtr = 0;
#else
    throw_no_structured_light();
#endif
}
void cveGrayCodePatternGetImagesForShadowMasks(cv::structured_light::GrayCodePattern* grayCodePattern, cv::_InputOutputArray* blackImage, cv::_InputOutputArray* whiteImage)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    grayCodePattern->getImagesForShadowMasks(*blackImage, *whiteImage);
#else
    throw_no_structured_light();
#endif	
}

bool cveGrayCodePatternGetProjPixel(cv::structured_light::GrayCodePattern* grayCodePattern, cv::_InputArray* patternImages, int x, int y, CvPoint* projPix)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    cv::Point p;
    bool result = grayCodePattern->getProjPixel(*patternImages, x, y, p);
    projPix->x = p.x;
    projPix->y = p.y;
    return result;
#else
    throw_no_structured_light();
#endif		
}

cv::structured_light::SinusoidalPattern* cveSinusoidalPatternCreate(
    int width,
    int height,
    int nbrOfPeriods,
    float shiftValue,
    int methodId,
    int nbrOfPixelsBetweenMarkers,
    bool horizontal,
    bool setMarkers,
    std::vector< cv::Point2f >* markersLocation,
    cv::Ptr<cv::structured_light::SinusoidalPattern>** sharedPtr,
    cv::structured_light::StructuredLightPattern** structuredLightPattern,
    cv::Algorithm** algorithm)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    cv::Ptr<cv::structured_light::SinusoidalPattern::Params> p =
        cv::makePtr<cv::structured_light::SinusoidalPattern::Params>();
    p->width = width;
    p->height = height;
    p->nbrOfPeriods = nbrOfPeriods;
    p->shiftValue = shiftValue;
    p->methodId = methodId;
    p->nbrOfPixelsBetweenMarkers = nbrOfPixelsBetweenMarkers;
    p->horizontal = horizontal;
    p->setMarkers = setMarkers;
    if (markersLocation)
        p->markersLocation = *markersLocation;
    cv::Ptr<cv::structured_light::SinusoidalPattern> pattern = cv::structured_light::SinusoidalPattern::create(p);
    *sharedPtr = new cv::Ptr<cv::structured_light::SinusoidalPattern>(pattern);
    cv::structured_light::SinusoidalPattern* patternPtr = (*sharedPtr)->get();
    *structuredLightPattern = dynamic_cast<cv::structured_light::StructuredLightPattern*>(patternPtr);
    *algorithm = dynamic_cast<cv::Algorithm*>(patternPtr);
    return patternPtr;
#else
    throw_no_structured_light();
#endif
}
void cveSinusoidalPatternRelease(cv::Ptr<cv::structured_light::SinusoidalPattern>** sharedPtr)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    delete* sharedPtr;
    *sharedPtr = 0;
#else
    throw_no_structured_light();
#endif
}
void cveSinusoidalPatternComputePhaseMap(
    cv::structured_light::SinusoidalPattern* pattern,
    cv::_InputArray* patternImages,
    cv::_OutputArray* wrappedPhaseMap,
    cv::_OutputArray* shadowMask,
    cv::_InputArray* fundamental)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    pattern->computePhaseMap(
        *patternImages,
        *wrappedPhaseMap,
        shadowMask ? *shadowMask : static_cast<cv::OutputArray>(cv::noArray()),
        fundamental ? *fundamental : static_cast<cv::InputArray>(cv::noArray()));
#else
    throw_no_structured_light();
#endif
}

void cveSinusoidalPatternUnwrapPhaseMap(
    cv::structured_light::SinusoidalPattern* pattern,
    cv::_InputArray* wrappedPhaseMap,
    cv::_OutputArray* unwrappedPhaseMap,
    CvSize* camSize,
    cv::_InputArray* shadowMask)
{
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
    pattern->unwrapPhaseMap(
        *wrappedPhaseMap,
        *unwrappedPhaseMap,
        *camSize,
        shadowMask ? *shadowMask : static_cast<cv::InputArray>(cv::noArray()));
#else
    throw_no_structured_light();
#endif
}


