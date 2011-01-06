#include "opencv2/gpu/gpu.hpp"

CVAPI(int) gpuGetCudaEnabledDeviceCount()
{
   return cv::gpu::getCudaEnabledDeviceCount();
}

CVAPI(int) gpuGetDevice()
{
   return cv::gpu::getDevice();
}

CVAPI(void) gpuGetDeviceName(int device, char* name, int maxSizeInBytes)
{
   std::string dName = cv::gpu::getDeviceName(device);
   strcpy_s(name, maxSizeInBytes, dName.c_str());
}

CVAPI(void) gpuGetComputeCapability(int device, int* major, int* minor)
{
   int maj, min;
   cv::gpu::getComputeCapability(device, maj, min);
   *major = maj;
   *minor = min;
}

CVAPI(int) gpuGetNumberOfSMs(int device)
{
   return cv::gpu::getNumberOfSMs(device);
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
   {
      src->copyTo(*dst);
   } else
   {
      src->copyTo(*dst, *mask);
   }
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