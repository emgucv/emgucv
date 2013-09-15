//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

/*
#if !defined (HAVE_CUDA) || defined (CUDA_DISABLER)
void cv::gpu::matchTemplate(const GpuMat&, const GpuMat&, GpuMat&, int, MatchTemplateBuf&, Stream&) 
{
   CV_Error(CV_GpuNotSupported, "The library is compiled without GPU support");
}
#endif
*/

//----------------------------------------------------------------------------
//
//  Gpu Device Info
//
//----------------------------------------------------------------------------

int gpuGetCudaEnabledDeviceCount()
{
   return cv::gpu::getCudaEnabledDeviceCount();
}

void gpuSetDevice(int deviceId)
{
   cv::gpu::setDevice(deviceId);
}

int gpuGetDevice()
{
   return cv::gpu::getDevice();
}

cv::gpu::DeviceInfo* gpuDeviceInfoCreate(int* deviceId)
{
   if (deviceId < 0)
      *deviceId = cv::gpu::getDevice();

   return new cv::gpu::DeviceInfo(*deviceId);
}

void gpuDeviceInfoRelease(cv::gpu::DeviceInfo** di)
{
   delete *di;
   *di = 0;
}

void gpuDeviceInfoDeviceName(cv::gpu::DeviceInfo* device, char* name, int maxSizeInBytes)
{
   std::string dName = device->name();
   strncpy(name, dName.c_str(), maxSizeInBytes);
}

void gpuDeviceInfoComputeCapability(cv::gpu::DeviceInfo* device, int* major, int* minor)
{
   *major = device->majorVersion();
   *minor = device->minorVersion();
}

int gpuDeviceInfoMultiProcessorCount(cv::gpu::DeviceInfo* device)
{
   return device->multiProcessorCount();
}

void gpuDeviceInfoFreeMemInfo(cv::gpu::DeviceInfo* info, size_t* free)
{
   *free = info->freeMemory();
}

void gpuDeviceInfoTotalMemInfo(cv::gpu::DeviceInfo* info, size_t* total)
{
   *total = info->totalMemory();
}

bool gpuDeviceInfoSupports(cv::gpu::DeviceInfo* device, cv::gpu::FeatureSet feature)
{
   return device->supports(feature);
}

bool gpuDeviceInfoIsCompatible(cv::gpu::DeviceInfo* device)
{
   return device->isCompatible();
}

//----------------------------------------------------------------------------
//
//  Gpu Module Info
//
//----------------------------------------------------------------------------

bool targetArchsBuildWith(cv::gpu::FeatureSet featureSet)
{
   return cv::gpu::TargetArchs::builtWith(featureSet);
}

bool targetArchsHas(int major, int minor)
{
   return cv::gpu::TargetArchs::has(major, minor);
}

bool targetArchsHasPtx(int major, int minor)
{
   return cv::gpu::TargetArchs::hasPtx(major, minor);
}

bool targetArchsHasBin(int major, int minor)
{
   return cv::gpu::TargetArchs::hasBin(major, minor);
}

bool targetArchsHasEqualOrLessPtx(int major, int minor)
{
   return cv::gpu::TargetArchs::hasBin(major, minor);
}

bool targetArchsHasEqualOrGreater(int major, int minor)
{
   return cv::gpu::TargetArchs::hasEqualOrGreater(major, minor);
}

bool targetArchsHasEqualOrGreaterPtx(int major, int minor)
{
   return cv::gpu::TargetArchs::hasEqualOrGreaterPtx(major, minor);
}

bool targetArchsHasEqualOrGreaterBin(int major, int minor)
{
   return cv::gpu::TargetArchs::hasEqualOrGreaterBin(major, minor);
}

//----------------------------------------------------------------------------
//
//  GpuMat
//
//----------------------------------------------------------------------------

cv::gpu::GpuMat* gpuMatCreateDefault() { return new cv::gpu::GpuMat() ; }

cv::gpu::GpuMat* gpuMatCreate(int rows, int cols, int type)
{
   return new cv::gpu::GpuMat(rows, cols, type);
}

cv::gpu::GpuMat* gpuMatCreateContinuous(int rows, int cols, int type)
{
   cv::gpu::GpuMat* result = new cv::gpu::GpuMat();
   cv::gpu::createContinuous(rows, cols, type, *result);
   return result;
}

bool gpuMatIsContinuous(cv::gpu::GpuMat* gpuMat)
{
   return gpuMat->isContinuous();
}

cv::gpu::GpuMat* gpuMatGetRegion(cv::gpu::GpuMat* other, CvSlice rowRange, CvSlice colRange)
{
   return new cv::gpu::GpuMat(*other, cv::Range(rowRange), cv::Range(colRange));
}

void gpuMatRelease(cv::gpu::GpuMat** mat)
{
   delete *mat;
   *mat = 0;
}

cv::gpu::GpuMat* gpuMatCreateFromArr(CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   return new cv::gpu::GpuMat(mat);
}

emgu::size gpuMatGetSize(cv::gpu::GpuMat* gpuMat)
{
   cv::Size s = gpuMat->size();
   emgu::size result;
   result.width = s.width;
   result.height = s.height;
   return result;
}

bool gpuMatIsEmpty(cv::gpu::GpuMat* gpuMat)
{
   return gpuMat->empty();
}

int gpuMatGetChannels(cv::gpu::GpuMat* gpuMat)
{
   return gpuMat->channels();
}

int gpuMatGetType(cv::gpu::GpuMat* gpuMat)
{
   return gpuMat->type();
}

void gpuMatUpload(cv::gpu::GpuMat* gpuMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   gpuMat->upload(mat);
}

void gpuMatDownload(cv::gpu::GpuMat* gpuMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   gpuMat->download(mat);
}

void gpuMatLShift(const cv::gpu::GpuMat* a, CvScalar scale, cv::gpu::GpuMat* c, cv::gpu::Stream* stream)
{
	cv::gpu::lshift(*a, scale, *c, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatRShift(const cv::gpu::GpuMat* a, CvScalar scale, cv::gpu::GpuMat* c, cv::gpu::Stream* stream)
{
	cv::gpu::rshift(*a, scale, *c, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatAdd(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::add(*a, *b, *c, mask ? *mask : cv::gpu::GpuMat(), c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatAddS(const cv::gpu::GpuMat* a, const CvScalar scale, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::Scalar s = scale;
   cv::gpu::add(*a, s, *c, mask ? *mask : cv::gpu::GpuMat(), c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatSubtract(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::subtract(*a, *b, *c, mask ? *mask : cv::gpu::GpuMat(), c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatSubtractS(const cv::gpu::GpuMat* a, const CvScalar scale, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::Scalar s = scale;
   cv::gpu::subtract(*a, s, *c, mask ? *mask : cv::gpu::GpuMat(), c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMultiply(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, double scale, cv::gpu::Stream* stream)
{
   cv::gpu::multiply(*a, *b, *c, scale, c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMultiplyS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream)
{
   cv::Scalar scalar = s;
   cv::gpu::multiply(*a, scalar, *c, 1, c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatDivide(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, double scale, cv::gpu::Stream* stream)
{
   cv::gpu::divide(*a, *b, *c, scale, c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatDivideSR(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream)
{
   cv::Scalar scalar = s;
   cv::gpu::divide(*a, scalar, *c, 1, c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatDivideSL(const double s, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, cv::gpu::Stream* stream)
{
   cv::gpu::divide(s, *b, *c, c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatAddWeighted(const cv::gpu::GpuMat* src1, double alpha, const cv::gpu::GpuMat* src2, double beta, double gamma, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::addWeighted(*src1, alpha, *src2, beta, gamma, *dst, dst->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatAbsdiff(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, cv::gpu::Stream* stream)
{
   cv::gpu::absdiff(*a, *b, *c, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatAbs(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::abs(*src, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatSqr(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::sqr(*src, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatSqrt(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::sqrt(*src, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatAbsdiffS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream)
{
   cv::Scalar scalar = s;
   cv::gpu::absdiff(*a, scalar, *c, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatCompare(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, int cmpop, cv::gpu::Stream* stream)
{
   cv::gpu::compare(*a, *b, *c, cmpop, stream ? *stream : cv::gpu::Stream::Null());
}

double gpuMatThreshold(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double thresh, double maxval, int type, cv::gpu::Stream* stream)
{
   return cv::gpu::threshold(*src, *dst, thresh, maxval, type, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatCvtColor(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int code, cv::gpu::Stream* stream)
{
   cv::gpu::cvtColor(*src, *dst, code, dst->channels(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatSwapChannels(cv::gpu::GpuMat* image, const int* dstOrder, cv::gpu::Stream* stream)
{
   cv::gpu::swapChannels(*image, dstOrder, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatConvertTo(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double alpha, double beta, cv::gpu::Stream* stream)
{
   src->convertTo(*dst, dst->type(), alpha, beta, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatCopy(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   if (mask)
      src->copyTo(*dst, *mask, stream ? *stream : cv::gpu::Stream::Null());
   else
      src->copyTo(*dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatSetTo(cv::gpu::GpuMat* mat, const CvScalar s, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
      (*mat).setTo(s, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatResize(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int interpolation, cv::gpu::Stream* stream)
{  
   if ( !stream && !(src->channels() == 1 || src->channels() == 4) )
   {
      //in synchronous version
      //added support for gpuMat with number of channels other than 1 or 4.
      cv::gpu::Stream ts;
      std::vector<cv::gpu::GpuMat> channels(src->channels());
      std::vector<cv::gpu::GpuMat> resizedChannels(src->channels());
      cv::gpu::split(*src, channels, ts);
      for (unsigned int i = 0; i < channels.size(); ++i)
      {
         //CV_Assert(channels[i].size() == src->size());
         cv::gpu::resize(channels[i], resizedChannels[i], dst->size(), 0, 0, interpolation, ts);
         //CV_Assert(resizedChannels[i].size() == dst->size());
      }

      cv::gpu::merge(resizedChannels, *dst, ts);
      ts.waitForCompletion();
   } else
   {  
      cv::gpu::resize(*src, *dst, dst->size(), 0, 0, interpolation, stream ? *stream : cv::gpu::Stream::Null());
   }
}

CVAPI(void) gpuMatReshape(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int cn, int rows)
{
   cv::gpu::GpuMat tmp = src->reshape(cn, rows);
   dst->swap(tmp);
}

void gpuMatFlip(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int flipcode, cv::gpu::Stream* stream)
{
   cv::gpu::flip(*src, *dst, flipcode, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatSplit(const cv::gpu::GpuMat* src, cv::gpu::GpuMat** dst, cv::gpu::Stream* stream)
{
   int channels = src->channels();
   cv::gpu::GpuMat* dstArr = new cv::gpu::GpuMat[channels];
   for (int i = 0; i < channels; i++)
      dstArr[i] = *(dst[i]);
   cv::gpu::split(*src, dstArr, stream? *stream : cv::gpu::Stream::Null());
   delete[] dstArr;
}

void gpuMatExp(const cv::gpu::GpuMat* a, cv::gpu::GpuMat* b, cv::gpu::Stream* stream)
{
   cv::gpu::exp(*a, *b, stream? *stream : cv::gpu::Stream::Null());
}

void gpuMatPow(const cv::gpu::GpuMat* src, double power, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::pow(*src, power, *dst, stream? *stream : cv::gpu::Stream::Null()); 
}

void gpuMatLog(const cv::gpu::GpuMat* a, cv::gpu::GpuMat* b, cv::gpu::Stream* stream)
{
   cv::gpu::log(*a, *b, stream? *stream : cv::gpu::Stream::Null());
}

void gpuMatMagnitude(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* magnitude, cv::gpu::Stream* stream)
{
   cv::gpu::magnitude(*x, *y, *magnitude, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMagnitudeSqr(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* magnitude, cv::gpu::Stream* stream)
{
   cv::gpu::magnitudeSqr(*x, *y, *magnitude, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatPhase(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* angle, bool angleInDegrees, cv::gpu::Stream* stream)
{
   cv::gpu::phase(*x, *y, *angle, angleInDegrees, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatCartToPolar(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* magnitude, cv::gpu::GpuMat* angle, bool angleInDegrees, cv::gpu::Stream* stream)
{
   cv::gpu::cartToPolar(*x, *y, *magnitude, *angle, angleInDegrees, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatPolarToCart(const cv::gpu::GpuMat* magnitude, const cv::gpu::GpuMat* angle, cv::gpu::GpuMat* x, cv::gpu::GpuMat* y, bool angleInDegrees, cv::gpu::Stream* stream)
{
   cv::gpu::polarToCart(*magnitude, *angle, *x, *y, angleInDegrees, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMerge(const cv::gpu::GpuMat** src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   int channels = dst->channels();
   cv::gpu::GpuMat* srcArr = new cv::gpu::GpuMat[channels];
   for (int i = 0; i < channels; ++i)
      srcArr[i] = *(src[i]);
   cv::gpu::merge(srcArr, dst->channels(), *dst, stream ? *stream : cv::gpu::Stream::Null());
   delete[] srcArr;
}

//only support single channel gpuMat
void gpuMatMinMaxLoc(const cv::gpu::GpuMat* src, 
                     double* minVal, double* maxVal, 
                     CvPoint* minLoc, CvPoint* maxLoc, 
                     const cv::gpu::GpuMat* mask)
{
   cv::Point minimunLoc, maximunLoc;
   cv::gpu::GpuMat maskMat = mask ? *mask : cv::gpu::GpuMat();
   cv::gpu::minMaxLoc(*src, minVal, maxVal, &minimunLoc, &maximunLoc, maskMat);
   maxLoc->x = maximunLoc.x; maxLoc->y = maximunLoc.y;
   minLoc->x = minimunLoc.x; minLoc->y = minimunLoc.y;
}

void gpuMatPyrDown(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::pyrDown(*src, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatPyrUp(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::pyrUp(*src, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBlendLinear(
            const cv::gpu::GpuMat* img1, const cv::gpu::GpuMat* img2, 
            const cv::gpu::GpuMat* weights1, const cv::gpu::GpuMat* weights2, 
            cv::gpu::GpuMat* result, cv::gpu::Stream* stream)
{
   cv::gpu::blendLinear(*img1, *img2, *weights1, *weights2, *result, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMeanStdDev(const cv::gpu::GpuMat* mtx, CvScalar* mean, CvScalar* stddev, cv::gpu::GpuMat* buffer)
{
   cv::Scalar meanVal, stdDevVal;
   if (buffer)
      cv::gpu::meanStdDev(*mtx, meanVal, stdDevVal, *buffer);
   else
      cv::gpu::meanStdDev(*mtx, meanVal, stdDevVal);

   memcpy(mean->val, meanVal.val, sizeof(double)*4);
   memcpy(stddev->val, stdDevVal.val, sizeof(double) * 4);
}

double gpuMatNorm(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, int normType)
{
   if (src2)
      return cv::gpu::norm(*src1, *src2, normType);
   else
      return cv::gpu::norm(*src1, normType);
}

int gpuMatCountNonZero(const cv::gpu::GpuMat* src)
{
   return cv::gpu::countNonZero(*src);
}

void gpuMatReduce(const cv::gpu::GpuMat* mtx, cv::gpu::GpuMat* vec, int dim, int reduceOp, cv::gpu::Stream* stream)
{
   cv::gpu::reduce(*mtx, *vec, dim, reduceOp, vec->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

cv::gpu::LookUpTable* gpuLookUpTableCreate( const CvArr* lut )
{
   cv::Mat lutMat = cv::cvarrToMat(lut);
   cv::Ptr<cv::gpu::LookUpTable> ptr = cv::gpu::createLookUpTable(lutMat);
   ptr.addref();
   return ptr.obj;
}
void gpuLookUpTableTransform(cv::gpu::LookUpTable* lut, cv::gpu::GpuMat* image, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   lut->transform(*image, *dst, stream ? *stream : cv::gpu::Stream::Null());
}
void gpuLookUpTableRelease(cv::gpu::LookUpTable** lut)
{
   delete *lut;
   *lut=0;
}

void gpuMatBitwiseNot(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_not(*src, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseAnd(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_and(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseAndS(const cv::gpu::GpuMat* src1, const cv::Scalar sc, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_and(*src1, sc, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseOr(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_or(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseOrS(const cv::gpu::GpuMat* src1, const cv::Scalar sc, cv::gpu::GpuMat* dst,  const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_or(*src1, sc, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseXor(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_xor(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseXorS(const cv::gpu::GpuMat* src1, const cv::Scalar sc, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_xor(*src1, sc, *dst,  mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMin(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::min(*src1, *src2, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMinS(const cv::gpu::GpuMat* src1, double src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::min(*src1, src2, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMax(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::max(*src1, *src2, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMaxS(const cv::gpu::GpuMat* src1, double src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::max(*src1, src2, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

cv::gpu::Filter* gpuCreateSobelFilter(int srcType, int dstType,  int dx, int dy, int ksize, double scale, int rowBorderType, int columnBorderType)
{
   cv::Ptr<cv::gpu::Filter> ptr =  cv::gpu::createSobelFilter(srcType, dstType, dx, dy, ksize, scale, rowBorderType, columnBorderType);
   ptr.addref();
   return ptr.obj;
}

cv::gpu::Filter* gpuCreateGaussianFilter(int srcType, int dstType, emgu::size* ksize, double sigma1, double sigma2, int rowBorderType, int columnBorderType)
{
   cv::Size s(ksize->width, ksize->height);
   cv::Ptr<cv::gpu::Filter> ptr = cv::gpu::createGaussianFilter(srcType, dstType, s, sigma1, sigma2, rowBorderType, columnBorderType); 
   ptr.addref();
   return ptr.obj;
}

cv::gpu::Filter* gpuCreateLaplacianFilter(int srcType, int dstType, int ksize, double scale, int borderMode, CvScalar* borderValue)
{
   cv::Ptr<cv::gpu::Filter> ptr = cv::gpu::createLaplacianFilter(srcType, dstType, ksize, scale, borderMode, *borderValue);
   ptr.addref();
   return ptr.obj;
}

cv::gpu::Filter* gpuCreateLinearFilter(int srcType, int dstType, const CvArr* kernel, CvPoint* anchor, int borderMode, CvScalar* borderValue)
{
   cv::Mat kMat = cv::cvarrToMat(kernel);
   cv::Ptr<cv::gpu::Filter> ptr = cv::gpu::createLinearFilter(srcType, dstType, kMat, *anchor, borderMode, *borderValue);
   ptr.addref();
   return ptr.obj;
}

cv::gpu::Filter* gpuCreateBoxMaxFilter( int srcType, emgu::size* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue)
{
   cv::Size s(ksize->width, ksize->height);
   cv::Ptr<cv::gpu::Filter> ptr = cv::gpu::createBoxMaxFilter(srcType, s, *anchor, borderMode, *borderValue);
   ptr.addref();
   return ptr.obj;
}

cv::gpu::Filter* gpuCreateBoxMinFilter( int srcType, emgu::size* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue)
{
   cv::Size s(ksize->width, ksize->height);
   cv::Ptr<cv::gpu::Filter> ptr = cv::gpu::createBoxMinFilter(srcType, s, *anchor, borderMode, *borderValue);
   ptr.addref();
   return ptr.obj;
}

cv::gpu::Filter* gpuCreateMorphologyFilter( int op, int srcType, const CvArr* kernel, CvPoint* anchor, int iterations)
{
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::Ptr<cv::gpu::Filter> ptr = cv::gpu::createMorphologyFilter(op, srcType, kernelMat, *anchor, iterations);
   ptr.addref();
   return ptr.obj;
}

void gpuMatGemm(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, double alpha, 
                const cv::gpu::GpuMat* src3, double beta, cv::gpu::GpuMat* dst, int flags, cv::gpu::Stream* stream)
{
   cv::gpu::GpuMat src3Mat = src3 ? *src3 : cv::gpu::GpuMat();
   cv::gpu::gemm(*src1, *src2, alpha, src3Mat, beta, *dst, flags, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatWarpAffine( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst,  const CvArr* M, int flags, int borderMode, CvScalar borderValue, cv::gpu::Stream* stream)
{
   cv::Mat Mat = cv::cvarrToMat(M);
   cv::gpu::warpAffine(*src, *dst, Mat, dst->size(), flags, borderMode, borderValue, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatWarpPerspective( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst,  const CvArr* M, int flags,  int borderMode, CvScalar borderValue, cv::gpu::Stream* stream)
{
   cv::Mat Mat = cv::cvarrToMat(M);
   cv::gpu::warpPerspective(*src, *dst, Mat, dst->size(), flags, borderMode, borderValue, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatRemap(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* xmap, const cv::gpu::GpuMat* ymap, int interpolation, int borderMode, CvScalar borderValue, cv::gpu::Stream* stream)
{
	cv::gpu::remap(*src, *dst, *xmap, *ymap, interpolation, borderMode, borderValue, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMeanShiftFiltering(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int sp, int sr,
                              CvTermCriteria* criteria, cv::gpu::Stream* stream)
{
   cv::gpu::meanShiftFiltering(*src, *dst, sp, sr, *criteria, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMeanShiftProc(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dstr, cv::gpu::GpuMat* dstsp, int sp, int sr,
                         CvTermCriteria* criteria, cv::gpu::Stream* stream)
{
   cv::gpu::meanShiftProc(*src, *dstr, *dstsp, sp, sr, *criteria, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMeanShiftSegmentation(const cv::gpu::GpuMat* src, cv::Mat* dst, int sp, int sr, int minsize,
                                 CvTermCriteria* criteria)
{
   cv::gpu::meanShiftSegmentation(*src, *dst, sp, sr, minsize, *criteria);
}

void gpuMatHistEven(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* hist, cv::gpu::GpuMat* buffer, int histSize, int lowerLevel, int upperLevel, cv::gpu::Stream* stream)
{
   cv::gpu::GpuMat bufferMat = buffer ? *buffer : cv::gpu::GpuMat();
   cv::gpu::histEven(*src, *hist, bufferMat, histSize, lowerLevel, upperLevel, stream ? *stream : cv::gpu::Stream::Null());
}

cv::gpu::GpuMat* gpuMatGetSubRect(const cv::gpu::GpuMat* arr, CvRect rect) 
{ 
   return new cv::gpu::GpuMat(*arr, rect);
}

void gpuMatRotate(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double angle, double xShift, double yShift, int interpolation, cv::gpu::Stream* s)
{
	cv::gpu::rotate(*src, *dst, dst->size(), angle, xShift, yShift, interpolation, s ? *s : cv::gpu::Stream::Null());
}

void gpuMatCopyMakeBorder(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int top, int bottom, int left, int right, int gpuBorderType, const CvScalar value, cv::gpu::Stream* stream)
{
   cv::gpu::copyMakeBorder(*src, *dst, top, bottom, left, right, gpuBorderType, value, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatIntegral(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* sum, cv::gpu::GpuMat* sqrSum, cv::gpu::Stream* stream)
{
   if (sum && sqrSum)
      cv::gpu::integralBuffered(*src, *sum, *sqrSum, stream ? *stream : cv::gpu::Stream::Null());
   else if (sum)
   {
      CV_Assert(stream == 0);
      cv::gpu::integral(*src, *sum, cv::gpu::Stream::Null());
   } else if (sqrSum)
   {
      cv::gpu::sqrIntegral(*src, *sqrSum, stream ? *stream : cv::gpu::Stream::Null());
   }
}

cv::gpu::CornernessCriteria* gpuCreateHarrisCorner(int srcType, int blockSize, int ksize, double k, int borderType)
{
   cv::Ptr<cv::gpu::CornernessCriteria> ptr = cv::gpu::createHarrisCorner(srcType, blockSize, ksize, k, borderType);
   ptr.addref();
   return ptr.obj;
}

void gpuCornernessCriteriaCompute(cv::gpu::CornernessCriteria* detector, const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   detector->compute(*src, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuCornernessCriteriaRelease(cv::gpu::CornernessCriteria** detector)
{
   delete *detector;
   *detector = 0;
}

void gpuMatDft(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int flags, cv::gpu::Stream* stream)
{
   cv::gpu::dft(*src, *dst, dst->size(), flags | (dst->channels() == 1 ? cv::DFT_REAL_OUTPUT : 0), stream ? *stream : cv::gpu::Stream::Null());
}

cv::gpu::CannyEdgeDetector* gpuCreateCannyEdgeDetector(double lowThreshold, double highThreshold, int apertureSize, bool L2gradient)
{
   cv::Ptr<cv::gpu::CannyEdgeDetector> ptr = cv::gpu::createCannyEdgeDetector(lowThreshold, highThreshold, apertureSize, L2gradient);
   ptr.addref();
   return ptr.obj;
}
void gpuCannyEdgeDetectorDetect(cv::gpu::CannyEdgeDetector* detector, cv::gpu::GpuMat* src, cv::gpu::GpuMat* edges)
{
   detector->detect(*src, *edges);
}
void gpuCannyEdgeDetectorRelease(cv::gpu::CannyEdgeDetector** detector)
{
   delete *detector;
   *detector = 0;
}

//----------------------------------------------------------------------------
//
//  GpuGoodFeaturesToTrackDetector
//
//----------------------------------------------------------------------------
cv::gpu::CornersDetector* gpuGoodFeaturesToTrackDetectorCreate(int srcType, int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double harrisK )
{
	cv::Ptr<cv::gpu::CornersDetector> detector =  cv::gpu::createGoodFeaturesToTrackDetector (srcType, maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, harrisK );
	detector.addref();
	return detector.obj;
}
void gpuCornersDetectorDetect(cv::gpu::CornersDetector* detector, const cv::gpu::GpuMat* image, cv::gpu::GpuMat* corners, const cv::gpu::GpuMat* mask)
{
   detector->detect (*image, *corners, mask ? *mask : cv::gpu::GpuMat());
}
void gpuCornersDetectorRelease(cv::gpu::CornersDetector** detector)
{
   delete *detector;
   *detector=0;
}

//----------------------------------------------------------------------------
//
//  Utilities
//
//----------------------------------------------------------------------------
void gpuCreateOpticalFlowNeedleMap(const cv::gpu::GpuMat* u, const cv::gpu::GpuMat* v, cv::gpu::GpuMat* vertex, cv::gpu::GpuMat* colors)
{
   cv::gpu::createOpticalFlowNeedleMap(*u, *v, *vertex, *colors);
}

//----------------------------------------------------------------------------
//
//  GpuTemplateMatching
//
//----------------------------------------------------------------------------
cv::gpu::TemplateMatching* gpuTemplateMatchingCreate(int srcType, int method, emgu::size* blockSize)
{
   cv::Size s(blockSize->width, blockSize->height);
	cv::Ptr<cv::gpu::TemplateMatching> ptr = cv::gpu::createTemplateMatching(srcType, method, s);
   ptr.addref();
   return ptr.obj;
}

void gpuTemplateMatchingMatch(cv::gpu::TemplateMatching* tm, const cv::gpu::GpuMat* image, const cv::gpu::GpuMat* templ, cv::gpu::GpuMat* result,  cv::gpu::Stream* stream)
{
   tm->match(*image, *templ, *result, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuTemplateMatchingRelease(cv::gpu::TemplateMatching** tm)
{
   delete *tm;
   *tm = 0;
}

//----------------------------------------------------------------------------
//
//  GpuFilter
//
//----------------------------------------------------------------------------
void gpuFilterApply(cv::gpu::Filter* filter, cv::gpu::GpuMat* image, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   filter->apply(*image, *dst, stream ? *stream : cv::gpu::Stream::Null());
}
void gpuFilterRelease(cv::gpu::Filter** filter)
{
   delete *filter;
   *filter = 0;
}