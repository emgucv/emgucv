//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

#if !defined (HAVE_CUDA) || defined (CUDA_DISABLER)
void cv::gpu::matchTemplate(const GpuMat&, const GpuMat&, GpuMat&, int, MatchTemplateBuf&, Stream&) 
{
   CV_Error(CV_GpuNotSupported, "The library is compiled without GPU support");
}
#endif

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

CvSize gpuMatGetSize(cv::gpu::GpuMat* gpuMat)
{
   return gpuMat->size();
}

bool gpuMatIsEmpty(cv::gpu::GpuMat* gpuMat)
{
   return gpuMat->empty();
}

int gpuMatGetChannels(cv::gpu::GpuMat* gpuMat)
{
   return gpuMat->channels();
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
   cv::gpu::add(*a, scale, *c, mask ? *mask : cv::gpu::GpuMat(), c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatSubtract(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::subtract(*a, *b, *c, mask ? *mask : cv::gpu::GpuMat(), c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatSubtractS(const cv::gpu::GpuMat* a, const CvScalar scale, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::subtract(*a, scale, *c, mask ? *mask : cv::gpu::GpuMat(), c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMultiply(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, double scale, cv::gpu::Stream* stream)
{
   cv::gpu::multiply(*a, *b, *c, scale, c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMultiplyS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream)
{
   cv::gpu::multiply(*a, s, *c, 1, c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatDivide(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, double scale, cv::gpu::Stream* stream)
{
   cv::gpu::divide(*a, *b, *c, scale, c->depth(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatDivideSR(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream)
{
   cv::gpu::divide(*a, s, *c, 1, c->depth(), stream ? *stream : cv::gpu::Stream::Null());
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
   cv::gpu::absdiff(*a, s, *c, stream ? *stream : cv::gpu::Stream::Null());
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
   if (stream)
      stream->enqueueConvert(*src, *dst, dst->type(), alpha, beta);
   else
      src->convertTo(*dst, dst->type(), alpha, beta);
}

void gpuMatCopy(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask)
{
   if (mask)
      src->copyTo(*dst, *mask);
   else
      src->copyTo(*dst);
}

void gpuMatSetTo(cv::gpu::GpuMat* mat, const CvScalar s, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   if (stream)
   {
      if (mask)
         stream->enqueueMemSet(*mat, s, *mask);
      else 
         stream->enqueueMemSet(*mat, s);
   }
   else
      (*mat).setTo(s, mask ? *mask : cv::gpu::GpuMat());
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

void gpuMatMatchTemplate(const cv::gpu::GpuMat* image, const cv::gpu::GpuMat* templ, cv::gpu::GpuMat* result, int method, cv::gpu::MatchTemplateBuf* buffer, cv::gpu::Stream* stream)
{
   if (buffer)
      cv::gpu::matchTemplate(*image, *templ, *result, method, *buffer, stream ? *stream : cv::gpu::Stream::Null());
   else
   {
      if (stream)
         CV_Error(CV_StsError, "Must specify gpu MatchTemplateBuf when using Stream");
      else
      {
         cv::gpu::matchTemplate(*image, *templ, *result, method);
      }
   }
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

void gpuMatLUT(const cv::gpu::GpuMat* src, const CvArr* lut, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::Mat lutMat = cv::cvarrToMat(lut);
   cv::gpu::LUT(*src, lutMat, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatFilter2D(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const CvArr* kernel, CvPoint anchor, int borderType, cv::gpu::Stream* stream)
{
   cv::Mat kMat = cv::cvarrToMat(kernel);
   cv::gpu::filter2D(*src, *dst, src->depth(), kMat, anchor, borderType, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseNot(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_not(*src, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseAnd(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_and(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseAndS(const cv::gpu::GpuMat* src1, const cv::Scalar sc, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_and(*src1, sc, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseOr(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_or(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseOrS(const cv::gpu::GpuMat* src1, const cv::Scalar sc, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_or(*src1, sc, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseXor(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_xor(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat(), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatBitwiseXorS(const cv::gpu::GpuMat* src1, const cv::Scalar sc, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   cv::gpu::bitwise_xor(*src1, sc, *dst, stream ? *stream : cv::gpu::Stream::Null());
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

void gpuMatSobel(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int dx, int dy, cv::gpu::GpuMat* buffer, int ksize, double scale, int rowBorderType, int columnBorderType, cv::gpu::Stream* stream)
{
   cv::gpu::GpuMat bufferMat = buffer ? *buffer : cv::gpu::GpuMat();
   cv::gpu::Sobel(*src, *dst, dst->depth(), dx, dy, bufferMat, ksize, scale, rowBorderType, columnBorderType, stream ? *stream : cv::gpu::Stream::Null()); 
}

void gpuMatGaussianBlur(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, CvSize ksize, cv::gpu::GpuMat* buffer, double sigma1, double sigma2, int rowBorderType, int columnBorderType, cv::gpu::Stream* stream)
{
   cv::gpu::GpuMat bufferMat = buffer ? *buffer : cv::gpu::GpuMat();
   cv::gpu::GaussianBlur(*src, *dst, ksize, bufferMat, sigma1, sigma2, rowBorderType, columnBorderType, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatLaplacian(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int ksize, double scale, int borderType, cv::gpu::Stream* stream)
{
   cv::gpu::Laplacian(*src, *dst, src->depth(), ksize, scale, borderType, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatGemm(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, double alpha, 
                const cv::gpu::GpuMat* src3, double beta, cv::gpu::GpuMat* dst, int flags, cv::gpu::Stream* stream)
{
   cv::gpu::GpuMat src3Mat = src3 ? *src3 : cv::gpu::GpuMat();
   cv::gpu::gemm(*src1, *src2, alpha, src3Mat, beta, *dst, flags, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatErode( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const CvArr* kernel, cv::gpu::GpuMat* buffer, CvPoint anchor, int iterations, cv::gpu::Stream* stream)
{
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::gpu::GpuMat bufferMat = buffer ? *buffer : cv::gpu::GpuMat();
   cv::gpu::erode(*src, *dst, kernelMat, bufferMat, anchor, iterations, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatDilate( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const CvArr* kernel, cv::gpu::GpuMat* buffer, CvPoint anchor, int iterations, cv::gpu::Stream* stream)
{
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::gpu::GpuMat bufferMat = buffer ? *buffer : cv::gpu::GpuMat();
   cv::gpu::dilate(*src, *dst, kernelMat, bufferMat, anchor, iterations, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMorphologyEx( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int op, const CvArr* kernel, cv::gpu::GpuMat* buffer1, cv::gpu::GpuMat* buffer2, CvPoint anchor, int iterations, cv::gpu::Stream* stream)
{
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::gpu::GpuMat buffer1Mat = buffer1 ? *buffer1 : cv::gpu::GpuMat();
   cv::gpu::GpuMat buffer2Mat = buffer2 ? *buffer2 : cv::gpu::GpuMat();
   cv::gpu::morphologyEx(*src, *dst, op, kernelMat, buffer1Mat, buffer2Mat, anchor, iterations, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatWarpAffine( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst,  const CvArr* M, int flags, int borderMode, cv::Scalar borderValue, cv::gpu::Stream* stream)
{
   cv::Mat Mat = cv::cvarrToMat(M);
   cv::gpu::warpAffine(*src, *dst, Mat, dst->size(), flags, borderMode, borderValue, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatWarpPerspective( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst,  const CvArr* M, int flags,  int borderMode, cv::Scalar borderValue, cv::gpu::Stream* stream)
{
   cv::Mat Mat = cv::cvarrToMat(M);
   cv::gpu::warpPerspective(*src, *dst, Mat, dst->size(), flags, borderMode, borderValue, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatRemap(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* xmap, const cv::gpu::GpuMat* ymap, int interpolation, int borderMode, CvScalar borderValue, cv::gpu::Stream* stream)
{
	cv::gpu::remap(*src, *dst, *xmap, *ymap, interpolation, borderMode, borderValue, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMeanShiftFiltering(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int sp, int sr,
                              CvTermCriteria criteria, cv::gpu::Stream* stream)
{
   cv::gpu::meanShiftFiltering(*src, *dst, sp, sr, criteria, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMeanShiftProc(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dstr, cv::gpu::GpuMat* dstsp, int sp, int sr,
                         CvTermCriteria criteria, cv::gpu::Stream* stream)
{
   cv::gpu::meanShiftProc(*src, *dstr, *dstsp, sp, sr, criteria, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatMeanShiftSegmentation(const cv::gpu::GpuMat* src, cv::Mat* dst, int sp, int sr, int minsize,
                                 CvTermCriteria criteria)
{
   cv::gpu::meanShiftSegmentation(*src, *dst, sp, sr, minsize, criteria);
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

void gpuMatCornerHarris(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int blockSize, int ksize, double k, int borderType)
{
   cv::gpu::cornerHarris(*src, *dst, blockSize, ksize, k, borderType);
}

void gpuMatDft(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int flags, cv::gpu::Stream* stream)
{
   cv::gpu::dft(*src, *dst, dst->size(), flags | (dst->channels() == 1 ? cv::DFT_REAL_OUTPUT : 0), stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMatCanny(const cv::gpu::GpuMat* image, cv::gpu::GpuMat* edges, double lowThreshold, double highThreshold, int apertureSize, bool L2gradient)
{
   cv::gpu::Canny(*image, *edges, lowThreshold, highThreshold, apertureSize, L2gradient);
}

//----------------------------------------------------------------------------
//
//  GpuGoodFeaturesToTrackDetector
//
//----------------------------------------------------------------------------
cv::gpu::GoodFeaturesToTrackDetector_GPU* gpuGoodFeaturesToTrackDetectorCreate(int maxCorners, double qualityLevel, double minDistance)
{
   return new cv::gpu::GoodFeaturesToTrackDetector_GPU(maxCorners, qualityLevel, minDistance);
}
void gpuGoodFeaturesToTrackDetectorDetect(cv::gpu::GoodFeaturesToTrackDetector_GPU* detector, const cv::gpu::GpuMat* image, cv::gpu::GpuMat* corners, const cv::gpu::GpuMat* mask)
{
   (*detector)(*image, *corners, mask ? *mask : cv::gpu::GpuMat());
}
void gpuGoodFeaturesToTrackDetectorRelease(cv::gpu::GoodFeaturesToTrackDetector_GPU** detector)
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
//  GpuMatchTemplateBuf
//
//----------------------------------------------------------------------------
cv::gpu::MatchTemplateBuf* gpuMatchTemplateBufCreate()
{
   return new cv::gpu::MatchTemplateBuf();
}
void gpuMatchTemplateBufRelease(cv::gpu::MatchTemplateBuf** buffer)
{
   delete *buffer;
   *buffer = 0;
}