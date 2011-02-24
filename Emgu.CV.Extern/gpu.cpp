#include "opencv2/gpu/gpu.hpp"

CVAPI(int) gpuGetCudaEnabledDeviceCount()
{
   return cv::gpu::getCudaEnabledDeviceCount();
}

CVAPI(int) gpuGetDevice()
{
   return cv::gpu::getDevice();
}

CVAPI(cv::gpu::DeviceInfo*) gpuDeviceInfoCreate(int* deviceId)
{
   if (deviceId < 0)
      *deviceId = cv::gpu::getDevice();

   return new cv::gpu::DeviceInfo(*deviceId);
}

CVAPI(void) gpuDeviceInfoRelease(cv::gpu::DeviceInfo** di)
{
   delete *di;
}

CVAPI(void) gpuDeviceInfoDeviceName(cv::gpu::DeviceInfo* device, char* name, int maxSizeInBytes)
{
   std::string dName = device->name();
   strncpy(name, dName.c_str(), maxSizeInBytes);
}

CVAPI(void) gpuDeviceInfoComputeCapability(cv::gpu::DeviceInfo* device, int* major, int* minor)
{
   *major = device->majorVersion();
   *minor = device->minorVersion();
}

CVAPI(int) gpuDeviceInfoMultiProcessorCount(cv::gpu::DeviceInfo* device)
{
   return device->multiProcessorCount();
}

CVAPI(void) gpuDeviceInfoFreeMemInfo(cv::gpu::DeviceInfo* info, size_t* free)
{
   *free = info->freeMemory();
}

CVAPI(void) gpuDeviceInfoTotalMemInfo(cv::gpu::DeviceInfo* info, size_t* total)
{
   *total = info->totalMemory();
}

CVAPI(bool) gpuDeviceInfoSupports(cv::gpu::DeviceInfo* device, cv::gpu::FeatureSet feature)
{
   return device->supports(feature);
}

CVAPI(cv::gpu::GpuMat*) gpuMatCreateDefault() { return new cv::gpu::GpuMat() ; }

CVAPI(cv::gpu::GpuMat*) gpuMatCreate(int rows, int cols, int type)
{
   return new cv::gpu::GpuMat(rows, cols, type);
}

CVAPI(void) gpuMatRelease(cv::gpu::GpuMat** mat)
{
   delete *mat;
}

CVAPI(cv::gpu::GpuMat*) gpuMatCreateFromArr(CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   return new cv::gpu::GpuMat(mat);
}

CVAPI(CvSize) gpuMatGetSize(cv::gpu::GpuMat* gpuMat, cv::Size* size)
{
   return gpuMat->size();
}

CVAPI(bool) gpuMatIsEmpty(cv::gpu::GpuMat* gpuMat)
{
   return gpuMat->empty();
}

CVAPI(int) gpuMatGetChannels(cv::gpu::GpuMat* gpuMat)
{
   return gpuMat->channels();
}

CVAPI(void) gpuMatUpload(cv::gpu::GpuMat* gpuMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   gpuMat->upload(mat);
}

CVAPI(void) gpuMatDownload(cv::gpu::GpuMat* gpuMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   gpuMat->download(mat);
}

CVAPI(void) gpuMatAdd(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c)
{
   cv::gpu::add(*a, *b, *c);
}

CVAPI(void) gpuMatAddS(const cv::gpu::GpuMat* a, const CvScalar scale, cv::gpu::GpuMat* c)
{
   cv::gpu::add(*a, scale, *c);
}

CVAPI(void) gpuMatSubtract(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c)
{
   cv::gpu::subtract(*a, *b, *c);
}

CVAPI(void) gpuMatSubtractS(const cv::gpu::GpuMat* a, const CvScalar scale, cv::gpu::GpuMat* c)
{
   cv::gpu::subtract(*a, scale, *c);
}

CVAPI(void) gpuMatMultiply(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c)
{
   cv::gpu::multiply(*a, *b, *c);
}

CVAPI(void) gpuMatMultiplyS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c)
{
   cv::gpu::multiply(*a, s, *c);
}

CVAPI(void) gpuMatDivide(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c)
{
   cv::gpu::divide(*a, *b, *c);
}

CVAPI(void) gpuMatDivideS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c)
{
   cv::gpu::divide(*a, s, *c);
}

CVAPI(void) gpuMatAbsdiff(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c)
{
   cv::gpu::absdiff(*a, *b, *c);
}

CVAPI(void) gpuMatAbsdiffS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c)
{
   cv::gpu::absdiff(*a, s, *c);
}

CVAPI(void) gpuMatCompare(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, int cmpop)
{
   cv::gpu::compare(*a, *b, *c, cmpop);
}

CVAPI(double) gpuMatThreshold(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double thresh, double maxval, int type, const cv::gpu::Stream* stream)
{
   if(stream)
      return cv::gpu::threshold(*src, *dst, thresh, maxval, type, *stream);
   else
      return cv::gpu::threshold(*src, *dst, thresh, maxval, type);
}

CVAPI(void) gpuMatCvtColor(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int code, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::cvtColor(*src, *dst, code, dst->channels(), *stream);
   else
      cv::gpu::cvtColor(*src, *dst, code);
}

CVAPI(void) gpuMatConvertTo(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double alpha, double beta, cv::gpu::Stream* stream)
{
   if (stream)
      stream->enqueueConvert(*src, *dst, dst->type(), alpha, beta);
   else
      src->convertTo(*dst, dst->type(), alpha, beta);
}

CVAPI(void) gpuMatCopy(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask)
{
   if (mask)
      src->copyTo(*dst, *mask);
   else
      src->copyTo(*dst);
}

CVAPI(void) gpuMatSetTo(cv::gpu::GpuMat* mat, const CvScalar s, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
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

CVAPI(void) gpuMatResize(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int interpolation)
{
   if (src->channels() == 1 || src->channels() == 4)
   {
      cv::gpu::resize(*src, *dst, dst->size(), 0, 0, interpolation);
   } else
   {  //added support for gpuMat with number of channels other than 1 or 4.
      std::vector<cv::gpu::GpuMat> channels(src->channels());
      std::vector<cv::gpu::GpuMat> resizedChannels(src->channels());
      cv::gpu::split(*src, channels);
      for (unsigned int i = 0; i < channels.size(); ++i)
      {
         cv::gpu::resize(channels[i], resizedChannels[i], dst->size(), 0, 0, interpolation);
      }
      cv::gpu::merge(resizedChannels, *dst);
   }
}

CVAPI(cv::gpu::GpuMat*) gpuMatReshape(const cv::gpu::GpuMat* src, int cn, int rows)
{
   cv::gpu::GpuMat* result = new cv::gpu::GpuMat();
   cv::gpu::GpuMat tmp = src->reshape(cn, rows);
   tmp.swap(*result);
   return result;
}

CVAPI(void) gpuMatFlip(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int flipcode)
{
   if (src->channels() == 1 || src->channels() == 4)
   {
      cv::gpu::flip(*src, *dst, flipcode);
   } else
   {  //added support for gpuMat with number of channels other than 1 or 4.
      std::vector<cv::gpu::GpuMat> channels(src->channels());
      std::vector<cv::gpu::GpuMat> resizedChannels(src->channels());
      cv::gpu::split(*src, channels);
      for (unsigned int i = 0; i < channels.size(); ++i)
      {
         cv::gpu::flip(channels[i], resizedChannels[i], flipcode);
      }
      cv::gpu::merge(resizedChannels, *dst);
   }
}

CVAPI(void) gpuMatSplit(const cv::gpu::GpuMat* src, cv::gpu::GpuMat** dst, const cv::gpu::Stream* stream)
{
   std::vector<cv::gpu::GpuMat> dstMat;
   for(int i = 0; i < src->channels(); i++)
      dstMat.push_back(  *(dst[i]) );
   if (stream)
      cv::gpu::split(*src, dstMat, *stream);
   else
      cv::gpu::split(*src, dstMat);
}

CVAPI(void) gpuMatExp(const cv::gpu::GpuMat* a, cv::gpu::GpuMat* b)
{
	cv::gpu::exp(*a, *b);
}

CVAPI(void) gpuMatLog(const cv::gpu::GpuMat* a, cv::gpu::GpuMat* b)
{
	cv::gpu::log(*a, *b);
}

CVAPI(void) gpuMatMagnitude(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* magnitude, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::magnitude(*x, *y, *magnitude, *stream);
   else 
      cv::gpu::magnitude(*x, *y, *magnitude);
}

CVAPI(void) gpuMatMagnitudeSqr(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* magnitude, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::magnitudeSqr(*x, *y, *magnitude, *stream);
   else 
      cv::gpu::magnitudeSqr(*x, *y, *magnitude);
}

CVAPI(void) gpuMatPhase(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* angle, bool angleInDegrees, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::phase(*x, *y, *angle, angleInDegrees, *stream);
   else
      cv::gpu::phase(*x, *y, *angle, angleInDegrees);
}

CVAPI(void) gpuMatCartToPolar(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* magnitude, cv::gpu::GpuMat* angle, bool angleInDegrees, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::cartToPolar(*x, *y, *magnitude, *angle, angleInDegrees, *stream);
   else
      cv::gpu::cartToPolar(*x, *y, *magnitude, *angle, angleInDegrees);
}

CVAPI(void) gpuMatPolarToCart(const cv::gpu::GpuMat* magnitude, const cv::gpu::GpuMat* angle, cv::gpu::GpuMat* x, cv::gpu::GpuMat* y, bool angleInDegrees, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::polarToCart(*magnitude, *angle, *x, *y, angleInDegrees, *stream);
   else
      cv::gpu::polarToCart(*magnitude, *angle, *x, *y, angleInDegrees);
}

CVAPI(void) gpuMatMerge(const cv::gpu::GpuMat** src, cv::gpu::GpuMat* dst, const cv::gpu::Stream* stream)
{
   std::vector<cv::gpu::GpuMat> srcMat;
   for(int i = 0; i < dst->channels(); i++)
      srcMat.push_back(  *(src[i]) );
   if (stream)
      cv::gpu::merge(srcMat, *dst, *stream);
   else
      cv::gpu::merge(srcMat, *dst);
}

//only support single channel gpuMat
CVAPI(void) gpuMatMinMaxLoc(const cv::gpu::GpuMat* src, 
                            double* minVal, double* maxVal, 
                            CvPoint* minLoc, CvPoint* maxLoc, 
                            const cv::gpu::GpuMat* mask)
{
   cv::Point minimunLoc, maximunLoc;
   cv::gpu::minMaxLoc(*src, minVal, maxVal, &minimunLoc, &maximunLoc, *mask);
   maxLoc->x = maximunLoc.x; maxLoc->y = maximunLoc.y;
   minLoc->x = minimunLoc.x; maxLoc->y = minimunLoc.y;
}

CVAPI(void) gpuMatMeanStdDev(const cv::gpu::GpuMat* mtx, CvScalar* mean, CvScalar* stddev)
{
   cv::Scalar meanVal, stdDevVal;
   cv::gpu::meanStdDev(*mtx, meanVal, stdDevVal);
   memcpy(mean->val, meanVal.val, sizeof(double)*4);
   memcpy(stddev->val, stdDevVal.val, sizeof(double) * 4);
}

CVAPI(double) gpuMatNorm(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, int normType)
{
   if (src2)
      return cv::gpu::norm(*src1, *src2, normType);
   else
      return cv::gpu::norm(*src1, normType);
}

CVAPI(int) gpuMatCountNonZero(const cv::gpu::GpuMat* src)
{
   return cv::gpu::countNonZero(*src);
}

CVAPI(void) gpuMatLUT(const cv::gpu::GpuMat* src, const CvArr* lut, cv::gpu::GpuMat* dst)
{
   cv::Mat lutMat = cv::cvarrToMat(lut);
   cv::gpu::LUT(*src, lutMat, *dst);
}

CVAPI(void) gpuMatFilter2D(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const CvArr* kernel, CvPoint anchor)
{
   cv::Mat kMat = cv::cvarrToMat(kernel);
   cv::gpu::filter2D(*src, *dst, src->depth(), kMat, anchor);
}

CVAPI(void) gpuMatBitwiseNot(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::bitwise_not(*src, *dst, mask ? *mask : cv::gpu::GpuMat(), *stream);
   else
      cv::gpu::bitwise_not(*src, *dst, mask ? *mask : cv::gpu::GpuMat());
}

CVAPI(void) gpuMatBitwiseAnd(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::bitwise_and(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat(), *stream);
   else
      cv::gpu::bitwise_and(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat());
}

CVAPI(void) gpuMatBitwiseOr(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::bitwise_or(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat(), *stream);
   else
      cv::gpu::bitwise_or(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat());
}

CVAPI(void) gpuMatBitwiseXor(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::bitwise_xor(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat(), *stream);
   else
      cv::gpu::bitwise_xor(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat());
}

CVAPI(void) gpuMatMin(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::min(*src1, *src2, *dst, *stream);
   else
      cv::gpu::min(*src1, *src2, *dst);
}

CVAPI(void) gpuMatMinS(const cv::gpu::GpuMat* src1, double src2, cv::gpu::GpuMat* dst, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::min(*src1, src2, *dst, *stream);
   else
      cv::gpu::min(*src1, src2, *dst);
}

CVAPI(void) gpuMatMax(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::max(*src1, *src2, *dst, *stream);
   else
      cv::gpu::max(*src1, *src2, *dst);
}

CVAPI(void) gpuMatMaxS(const cv::gpu::GpuMat* src1, double src2, cv::gpu::GpuMat* dst, const cv::gpu::Stream* stream)
{
   if (stream)
      cv::gpu::max(*src1, src2, *dst, *stream);
   else
      cv::gpu::max(*src1, src2, *dst);
}

CVAPI(void) gpuMatSobel(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int dx, int dy, int ksize, double scale)
{
   cv::gpu::Sobel(*src, *dst, dst->depth(), dx, dy, ksize, scale); 
}

CVAPI(void) gpuMatGaussianBlur(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, CvSize ksize, double sigma1, double sigma2)
{
   cv::gpu::GaussianBlur(*src, *dst, ksize, sigma1, sigma2);
}

CVAPI(void) gpuMatLaplacian(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int ksize, double scale)
{
   cv::gpu::Laplacian(*src, *dst, src->depth(), ksize, scale);
}

CVAPI(void) gpuMatErode( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, CvArr* kernel, CvPoint anchor, int iterations)
{
   cv::Mat kernelMat = cv::cvarrToMat(kernel);
   cv::gpu::erode(*src, *dst, kernelMat, anchor, iterations);
}

CVAPI(void) gpuMatDilate( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, CvArr* kernel, CvPoint anchor, int iterations)
{
   cv::Mat kernelMat = cv::cvarrToMat(kernel);
   cv::gpu::dilate(*src, *dst, kernelMat, anchor, iterations);
}

CVAPI(void) gpuMatWarpAffine( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst,  const CvArr* M, int flags)
{
   cv::Mat Mat = cv::cvarrToMat(M);
   cv::gpu::warpAffine(*src, *dst, Mat, dst->size(), flags);
}

CVAPI(void) gpuMatWarpPerspective( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst,  const CvArr* M, int flags)
{
   cv::Mat Mat = cv::cvarrToMat(M);
   cv::gpu::warpPerspective(*src, *dst, Mat, dst->size(), flags);
}

CVAPI(void) gpuMatRemap(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* xmap, const cv::gpu::GpuMat* ymap)
{
   cv::gpu::remap(*src, *dst, *xmap, *ymap);
}

CVAPI(cv::gpu::GpuMat*) gpuMatHistEven(const cv::gpu::GpuMat* src, int histSize, int lowerLevel, int upperLevel)
{
   cv::gpu::GpuMat* hist = new cv::gpu::GpuMat();
   cv::gpu::histEven(*src, *hist, histSize, lowerLevel, upperLevel);
   return hist;
}

CVAPI(cv::gpu::GpuMat*) gpuMatGetSubRect(const cv::gpu::GpuMat* arr, CvRect rect) 
{ 
   return new cv::gpu::GpuMat(*arr, rect);
}

CVAPI(void) gpuMatIntegral(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* sum, cv::gpu::GpuMat* sqsum)
{
   if (sqsum)
      cv::gpu::integral(*src, *sum, *sqsum);
   else
      cv::gpu::integral(*src, *sum);
}

/*
CVAPI(void) gpuMatCanny(const cv::gpu::GpuMat* image, cv::gpu::GpuMat* edges, double threshold1, double threshold2, int apertureSize)
{
   cv::gpu::Canny(*image, *edges, threshold1, threshold2, apertureSize);
}*/
