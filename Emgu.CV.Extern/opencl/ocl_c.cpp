//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ocl_c.h"

int oclGetDevice(std::vector<cv::ocl::Info>* oclInfoVec, int deviceType)
{
   if (oclInfoVec)
      return cv::ocl::getDevice(*oclInfoVec, deviceType);
   else
   {
      std::vector<cv::ocl::Info> oclInfo;
      return cv::ocl::getDevice(oclInfo, deviceType);
   }
}

cv::ocl::oclMat* oclMatCreateDefault()
{
   return new cv::ocl::oclMat();
}

cv::ocl::oclMat* oclMatCreate(int rows, int cols, int type)
{
   return new cv::ocl::oclMat(rows, cols, type);
}

/*
cv::ocl::oclMat* oclMatCreateContinuous(int rows, int cols, int type)
{
   cv::ocl::oclMat* result = new cv::ocl::oclMat();
   cv::ocl::createContinuous(rows, cols, type, *result);
   return result;
}*/

cv::ocl::oclMat* oclMatCreateFromArr(CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   return new cv::ocl::oclMat(mat);
}

int oclMatGetType(cv::ocl::oclMat* oclMat)
{
   return oclMat->type();
}

emgu::size oclMatGetSize(cv::ocl::oclMat* oclMat)
{
   emgu::size s;
   s.width = oclMat->cols;
   s.height = oclMat->rows;
   return s;
}

emgu::size oclMatGetWholeSize(cv::ocl::oclMat* oclMat)
{
   emgu::size s;
   s.width = oclMat->wholecols;
   s.height = oclMat->wholerows;
   return s;
}

bool oclMatIsEmpty(cv::ocl::oclMat* oclMat)
{
   return oclMat->empty();
}

bool oclMatIsContinuous(cv::ocl::oclMat* oclMat)
{
   return oclMat->isContinuous();
}

int oclMatGetChannels(cv::ocl::oclMat* oclMat)
{
   return oclMat->channels();
}

void oclMatRelease(cv::ocl::oclMat** mat)
{
   delete *mat;
   *mat = 0;
}

//Pefroms blocking upload data to oclMat.
void oclMatUpload(cv::ocl::oclMat* oclMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   oclMat->upload(mat);
}

//Downloads data from device to host memory. Blocking calls.
void oclMatDownload(cv::ocl::oclMat* oclMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   int oldRows = mat.rows;
   int oldCols = mat.cols;
   int oldType = mat.type();
   
   oclMat->download(mat);
   CV_Assert(oldRows == mat.rows);
   CV_Assert(oldCols == mat.cols);

   //char message[2000];
   //sprintf(message, "oldType = %d; newType = %d", oldType, mat.type());
   //CV_Error(0, message);

   CV_Assert(oldType == mat.type());
}

int oclCountNonZero(cv::ocl::oclMat* oclMat)
{
   return cv::ocl::countNonZero(*oclMat);
}

void oclMatAdd(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask)
{
   if (mask)
   {
      cv::ocl::add(*a, *b, *c, *mask);
   }
   else
   {
      cv::ocl::add(*a, *b, *c);
   }
}

void oclMatAddS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask)
{
   cv::ocl::add(*a, scale, *c, mask? *mask : cv::ocl::oclMat());
}

void oclMatSubtract(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask)
{
   if (mask)
   {
      cv::ocl::subtract(*a, *b, *c, *mask);
   } else
   {
      cv::ocl::subtract(*a, *b, *c);
   }
}

void oclMatSubtractS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask)
{
   cv::ocl::subtract(*a, scale, *c, mask ? *mask : cv::ocl::oclMat());
}

void oclMatMultiply(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale)
{
   cv::ocl::multiply(*a, b ? *b : cv::ocl::oclMat(), *c, scale);
}

void oclMatMultiplyS(const cv::ocl::oclMat* a, const double s, cv::ocl::oclMat* c)
{
   cv::ocl::multiply(s, *a, *c);
}

void oclMatDivide(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale)
{
   cv::ocl::divide(*a, b ? *b : cv::ocl::oclMat(), *c, scale);
}
/*
void oclMatDivideSR(const cv::ocl::oclMat* a, const CvScalar s, cv::ocl::oclMat* c)
{
   cv::ocl::divide(*a, s, *c, 1, c->depth());
}*/

void oclMatDivideSL(const double s, const cv::ocl::oclMat* b, cv::ocl::oclMat* c)
{
   cv::ocl::divide(s, *b, *c);
}

void oclMatAddWeighted(const cv::ocl::oclMat* src1, double alpha, const cv::ocl::oclMat* src2, double beta, double gamma, cv::ocl::oclMat* dst)
{
   cv::ocl::addWeighted(*src1, alpha, *src2, beta, gamma, *dst);
}

void oclMatAbsdiff(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c)
{
   cv::ocl::absdiff(*a, *b, *c);
}

void oclMatAbsdiffS(const cv::ocl::oclMat* a, const CvScalar s, cv::ocl::oclMat* c)
{
   cv::ocl::absdiff(*a, s, *c);
}

void oclMatFlip(const cv::ocl::oclMat* a, cv::ocl::oclMat* b, int flipCode)
{
   cv::ocl::flip(*a, *b, flipCode);
}

void oclMatBitwiseNot(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst)
{
   cv::ocl::bitwise_not(*src, *dst);
}

void oclMatBitwiseAnd(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_and(*src1, *src2, *dst, mask ? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseAndS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_and(*src1, sc, *dst,  mask ? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseOr(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_or(*src1, *src2, *dst, mask? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseOrS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_or(*src1, sc, *dst, mask? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseXor(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_xor(*src1, *src2, *dst, mask? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseXorS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_xor(*src1, sc, *dst, mask? *mask : cv::ocl::oclMat());
}

void oclMatErode( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue)
{
   //char message[2000];
   //sprintf(message, "anchor = (%d, %d); iteration = %d; borderType = %d; borderValue = (%d, %d, %d, %d)", anchor.x, anchor.y, iterations, borderType, borderValue.val[0], borderValue.val[1], borderValue.val[2], borderValue.val[3]);
   //CV_Error(0, message);
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::ocl::erode(*src, *dst, kernelMat, anchor, iterations, borderType, borderValue);
}

void oclMatDilate( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel,  CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue)
{
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::ocl::dilate(*src, *dst, kernelMat, anchor, iterations, borderType, borderValue);
}

void oclMatMorphologyEx( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int op, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue)
{
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::ocl::morphologyEx( *src, *dst, op, kernelMat, anchor, iterations, borderType, borderValue);
}

void oclMatCompare(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, int cmpop)
{
   cv::ocl::compare(*a, *b, *c, cmpop);
}

void oclMatCvtColor(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int code)
{
   cv::ocl::cvtColor(*src, *dst, code);
}

void oclMatCopy(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   src->copyTo(*dst, mask? * mask : cv::ocl::oclMat());
}

void oclMatSetTo(cv::ocl::oclMat* mat, const CvScalar s, const cv::ocl::oclMat* mask)
{
   (*mat).setTo(s, mask ? *mask : cv::ocl::oclMat());
}
void oclMatResize(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double fx, double fy, int interpolation)
{
   cv::ocl::resize(*src, *dst, dst->size(), fx, fy, interpolation);
}

//only support single channel oclMat
void oclMatMinMaxLoc(const cv::ocl::oclMat* src, 
   double* minVal, double* maxVal, 
   CvPoint* minLoc, CvPoint* maxLoc, 
   const cv::ocl::oclMat* mask)
{
   cv::Point minimunLoc, maximunLoc;
   cv::ocl::oclMat maskMat = mask ? *mask : cv::ocl::oclMat();
   cv::ocl::minMaxLoc(*src, minVal, maxVal, &minimunLoc, &maximunLoc, maskMat);
   maxLoc->x = maximunLoc.x; maxLoc->y = maximunLoc.y;
   minLoc->x = minimunLoc.x; minLoc->y = minimunLoc.y;
}

void oclMatMatchTemplate(const cv::ocl::oclMat* image, const cv::ocl::oclMat* templ, cv::ocl::oclMat* result, int method, cv::ocl::MatchTemplateBuf* buffer)
{
   if (buffer)
      cv::ocl::matchTemplate(*image, *templ, *result, method, *buffer);
   else
   {
      cv::ocl::matchTemplate(*image, *templ, *result, method);      
   }
}

void oclMatPyrDown(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst)
{
   cv::ocl::pyrDown(*src, *dst);
}

void oclMatPyrUp(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst)
{
   cv::ocl::pyrUp(*src, *dst);
}

void oclMatSplit(const cv::ocl::oclMat* src, cv::ocl::oclMat** dst)
{
   int channels = src->channels();
   std::vector<cv::ocl::oclMat> dstArr(channels);
   //cv::ocl::oclMat* dstArr = new cv::ocl::oclMat[channels];
   for (int i = 0; i < channels; i++)
      dstArr[i] = *(dst[i]);
   cv::ocl::split(*src, dstArr);
   //delete[] dstArr;
}

void oclMatMerge(const cv::ocl::oclMat** src, cv::ocl::oclMat* dst)
{
   int channels = dst->channels();
   cv::ocl::oclMat* srcArr = new cv::ocl::oclMat[channels];
   for (int i = 0; i < channels; ++i)
      srcArr[i] = *(src[i]);
   cv::ocl::merge(srcArr, dst->channels(), *dst);
   delete[] srcArr;
}

void oclMatConvertTo(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double alpha, double beta)
{
   src->convertTo(*dst, dst->type(), alpha, beta);
}

void oclMatFilter2D(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int borderType)
{
   cv::Mat kMat = cv::cvarrToMat(kernel);
   cv::ocl::filter2D(*src, *dst, src->depth(), kMat, anchor, borderType);
}

void oclMatReshape(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int cn, int rows)
{
   cv::ocl::oclMat tmp = src->reshape(cn, rows);
   dst->swap(tmp);
}

void oclMatSobel(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int dx, int dy, int ksize, double scale, int borderType)
{
   cv::ocl::Sobel(*src, *dst, dst->depth(), dx, dy, ksize, scale, borderType); 
}

void oclMatGaussianBlur(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, CvSize ksize, double sigma1, double sigma2, int borderType)
{
   cv::ocl::GaussianBlur(*src, *dst, ksize, sigma1, sigma2, borderType);
}

void oclMatLaplacian(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int ksize, double scale)
{
   cv::ocl::Laplacian(*src, *dst, src->depth(), ksize, scale);
}

void oclMatGemm(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, double alpha, 
                const cv::ocl::oclMat* src3, double beta, cv::ocl::oclMat* dst, int flags)
{
   cv::ocl::oclMat src3Mat = src3 ? *src3 : cv::ocl::oclMat();
   cv::ocl::gemm(*src1, *src2, alpha, src3Mat, beta, *dst, flags);
}

void oclMatCanny(const cv::ocl::oclMat* image, cv::ocl::oclMat* edges, double lowThreshold, double highThreshold, int apertureSize, bool L2gradient)
{
   cv::ocl::Canny(*image, *edges, lowThreshold, highThreshold, apertureSize, L2gradient);
}

//----------------------------------------------------------------------------
//
//  OclHOGDescriptor
//
//----------------------------------------------------------------------------

void oclHOGDescriptorGetPeopleDetector64x128(std::vector<float>* vector)
{
   std::vector<float> v = cv::ocl::HOGDescriptor::getPeopleDetector64x128();
   v.swap(*vector);
}

void oclHOGDescriptorGetPeopleDetector48x96(std::vector<float>* vector)
{
   std::vector<float> v = cv::ocl::HOGDescriptor::getPeopleDetector48x96();
   v.swap(*vector);
}

cv::ocl::HOGDescriptor* oclHOGDescriptorCreateDefault() { return new cv::ocl::HOGDescriptor; }

cv::ocl::HOGDescriptor* oclHOGDescriptorCreate(
   cv::Size* _winSize, 
   cv::Size* _blockSize, 
   cv::Size* _blockStride,
   cv::Size* _cellSize, 
   int _nbins, 
   double _winSigma,
   double _L2HysThreshold, 
   bool _gammaCorrection, 
   int _nlevels)
{
   return new cv::ocl::HOGDescriptor(*_winSize, *_blockSize, *_blockStride, *_cellSize, _nbins, _winSigma, _L2HysThreshold, _gammaCorrection, _nlevels);
}

void oclHOGSetSVMDetector(cv::ocl::HOGDescriptor* descriptor, std::vector<float>* vector) 
{ 
   descriptor->setSVMDetector(*vector); 
}

void oclHOGDescriptorRelease(cv::ocl::HOGDescriptor** descriptor) 
{ 
   delete *descriptor;
   *descriptor = 0;
}

void oclHOGDescriptorDetectMultiScale(
   cv::ocl::HOGDescriptor* descriptor, 
   cv::ocl::oclMat* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding, 
   double scale,
   int groupThreshold)
{
   cvClearSeq(foundLocations);

   std::vector<cv::Rect> rects;
   
   //char message[2000];
   //sprintf(message, "winstride = (%d, %d); padding = (%d, %d)", winStride.width, winStride.height, padding.width, padding.height);
   //CV_Error(0, message);
   
   descriptor->detectMultiScale(*img, rects, hitThreshold, winStride, padding, scale, groupThreshold);
   if (!rects.empty())
      cvSeqPushMulti(foundLocations, &rects[0], static_cast<int>(rects.size()));
}


//----------------------------------------------------------------------------
//
//  oclMatchTemplateBuf
//
//----------------------------------------------------------------------------
cv::ocl::MatchTemplateBuf* oclMatchTemplateBufCreate()
{
   return new cv::ocl::MatchTemplateBuf();
}
void oclMatchTemplateBufRelease(cv::ocl::MatchTemplateBuf** buffer)
{
   delete *buffer;
   *buffer = 0;
}

//----------------------------------------------------------------------------
//
//  oclCascadeClassifier
//
//----------------------------------------------------------------------------
cv::ocl::OclCascadeClassifierBuf* oclCascadeClassifierCreate(const char* filename)
{
   cv::ocl::OclCascadeClassifierBuf* classifier =  new cv::ocl::OclCascadeClassifierBuf();
   classifier->load(filename);
   return classifier;
}

void oclCascadeClassifierRelease(cv::ocl::OclCascadeClassifierBuf** classifier)
{
   delete *classifier;
   *classifier = 0;
}

//----------------------------------------------------------------------------
//
//  OclPyrLKOpticalFlow
//
//----------------------------------------------------------------------------
cv::ocl::PyrLKOpticalFlow* oclPyrLKOpticalFlowCreate(cv::Size winSize, int maxLevel, int iters, bool useInitialFlow)
{
   cv::ocl::PyrLKOpticalFlow* flow = new cv::ocl::PyrLKOpticalFlow();
   
   flow->winSize = winSize;
   flow->maxLevel = maxLevel;
   flow->iters = iters;
   flow->useInitialFlow = useInitialFlow;
   return flow;
}

void oclPyrLKOpticalFlowSparse(
   cv::ocl::PyrLKOpticalFlow* flow, 
   const cv::ocl::oclMat* prevImg, 
   const cv::ocl::oclMat* nextImg, 
   const cv::ocl::oclMat* prevPts, 
   cv::ocl::oclMat* nextPts,
   cv::ocl::oclMat* status, 
   cv::ocl::oclMat* err)
{
   flow->sparse(*prevImg, *nextImg, *prevPts, *nextPts, *status, err);
}

void oclPyrLKOpticalFlowDense(
   cv::ocl::PyrLKOpticalFlow* flow, 
   const cv::ocl::oclMat* prevImg, 
   const cv::ocl::oclMat* nextImg,
   cv::ocl::oclMat* u, 
   cv::ocl::oclMat* v, 
   cv::ocl::oclMat* err)
{
   flow->dense(*prevImg, *nextImg, *u, *v, err);
}

void oclPyrLKOpticalFlowRelease(cv::ocl::PyrLKOpticalFlow** flow)
{
   delete *flow;
   *flow = 0;
}

//----------------------------------------------------------------------------
//
//  Ocl Stereo
//
//----------------------------------------------------------------------------
cv::ocl::StereoBM_OCL* oclStereoBMCreate(int preset, int ndisparities, int winSize)
{
   return new cv::ocl::StereoBM_OCL(preset, ndisparities, winSize);
}

void oclStereoBMFindStereoCorrespondence(cv::ocl::StereoBM_OCL* stereo, const cv::ocl::oclMat* left, const cv::ocl::oclMat* right, cv::ocl::oclMat* disparity)
{
   (*stereo)(*left, *right, *disparity);
}

void oclStereoBMRelease(cv::ocl::StereoBM_OCL** stereoBM)
{
   delete *stereoBM;
   *stereoBM = 0;
}

cv::ocl::StereoConstantSpaceBP* oclStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane)
{
   return new cv::ocl::StereoConstantSpaceBP(ndisp, iters, levels, nr_plane, CV_32F);
}

void oclStereoConstantSpaceBPFindStereoCorrespondence(cv::ocl::StereoConstantSpaceBP* stereo, const cv::ocl::oclMat* left, const cv::ocl::oclMat* right, cv::ocl::oclMat* disparity)
{
   (*stereo)(*left, *right, *disparity);
}

void oclStereoConstantSpaceBPRelease(cv::ocl::StereoConstantSpaceBP** stereo)
{
   delete *stereo;
   *stereo = 0;
}

//----------------------------------------------------------------------------
//
//  OclBruteForceMatcher
//
//----------------------------------------------------------------------------
cv::ocl::BFMatcher_OCL* oclBruteForceMatcherCreate(int distType) 
{
   return new cv::ocl::BFMatcher_OCL(distType);
}

void oclBruteForceMatcherRelease(cv::ocl::BFMatcher_OCL** matcher) 
{
   delete *matcher;
   *matcher = 0;
}

void oclBruteForceMatcherAdd(cv::ocl::BFMatcher_OCL* matcher, const cv::ocl::oclMat* trainDescs)
{
   std::vector< cv::ocl::oclMat > mats;
   mats.push_back( *trainDescs );
   matcher->add(mats);
}

void oclBruteForceMatcherKnnMatchSingle(
                                  cv::ocl::BFMatcher_OCL* matcher,
                                  const cv::ocl::oclMat* queryDescs, const cv::ocl::oclMat* trainDescs,
                                  cv::ocl::oclMat* trainIdx, cv::ocl::oclMat* distance, 
                                  int k, const cv::ocl::oclMat* mask)
{
   cv::ocl::oclMat emptyMat;
   mask = mask ? mask : &emptyMat;
   /*
   if (k == 2)
   {  //special case for k == 2;
      cv::ocl::oclMat idxMat = trainIdx->reshape(2, 1);
      cv::ocl::oclMat distMat = distance->reshape(2, 1);
      matcher->knnMatchSingle(*queryDescs, *trainDescs, 
         idxMat, distMat, 
         emptyMat, k, *mask);
      CV_Assert(idxMat.channels() == 2);
      CV_Assert(distMat.channels() == 2);
      CV_Assert(idxMat.data == trainIdx->data);
      CV_Assert(distMat.data == distance->data);
   } else*/
      matcher->knnMatchSingle(*queryDescs, *trainDescs, *trainIdx, *distance, emptyMat, k, *mask);
}


//----------------------------------------------------------------------------
//
//  Vector of VectorOfOclInfo
//
//----------------------------------------------------------------------------
std::vector<cv::ocl::Info>* VectorOfOclInfoCreate()
{
   return new std::vector<cv::ocl::Info>();
}

std::vector<cv::ocl::Info>* VectorOfOclInfoCreateSize(int size)
{
   return new std::vector<cv::ocl::Info>();
}

int VectorOfOclInfoGetSize(std::vector<cv::ocl::Info>* v)
{
   return v->size();
}

void VectorOfOclInfoClear(std::vector<cv::ocl::Info>* v)
{
   v->clear();
}

void VectorOfOclInfoRelease(std::vector<cv::ocl::Info>* v)
{
   delete v;
}

cv::ocl::Info* VectorOfOclInfoGetStartAddress(std::vector<cv::ocl::Info>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}

cv::ocl::Info* VectorOfOclInfoGetItem(std::vector<cv::ocl::Info>* v, int index)
{
   return &(*v)[index];
}

//----------------------------------------------------------------------------
//
//  OclInfo
//
//----------------------------------------------------------------------------
const char* oclInfoGetPlatformName(cv::ocl::Info* oclInfo)
{
   return oclInfo->PlatformName.c_str();
}

int oclInfoGetDeviceCount(cv::ocl::Info* oclInfo)
{
   return oclInfo->DeviceName.size();
}
const char* oclInfoGetDeviceName(cv::ocl::Info* oclInfo, int index)
{
   return oclInfo->DeviceName[index].c_str();
}