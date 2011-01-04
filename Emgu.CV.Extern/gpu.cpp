#include "opencv2/gpu/gpu.hpp"
#include <string.h>


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

CVAPI(cv::gpu::GpuMat*) gpuMatCreate(int rows, int cols, int type)
{
   return new cv::gpu::GpuMat(rows, cols, type);
}

CVAPI(void) gpuMatRelease(cv::gpu::GpuMat** mat)
{
   //TODO: Double check and make sure there is no memory leak here.
   (*mat)->release();
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

CVAPI(void) gpuMatSubtract(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c)
{
   cv::gpu::subtract(*a, *b, *c);
}

CVAPI(void) gpuMatSobel(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int dx, int dy, int ksize, double scale)
{
   cv::gpu::Sobel(*src, *dst, dst->depth(), dx, dy, ksize, scale); 
}

CVAPI(void) gpuMatCvtColor(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int code)
{
   cv::gpu::cvtColor(*src, *dst, code);
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

CVAPI(void) gpuMatSplit(const cv::gpu::GpuMat* src, cv::gpu::GpuMat** dst)
{
   std::vector<cv::gpu::GpuMat> dstMat;
   for(int i = 0; i < src->channels(); i++)
      dstMat.push_back(  *(dst[i]) );
   cv::gpu::split(*src, dstMat);
}

CVAPI(void) gpuMatMerge(const cv::gpu::GpuMat** src, cv::gpu::GpuMat* dst)
{
   std::vector<cv::gpu::GpuMat> srcMat;
   for(int i = 0; i < dst->channels(); i++)
      srcMat.push_back(  *(src[i]) );
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

CVAPI(void) gpuMatBitwiseNot(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask)
{
   cv::gpu::bitwise_not(*src, *dst, mask ? *mask : cv::gpu::GpuMat());
}

CVAPI(void) gpuMatBitwiseAnd(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask)
{
   cv::gpu::bitwise_and(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat());
}

CVAPI(void) gpuMatBitwiseOr(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask)
{
   cv::gpu::bitwise_or(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat());
}

CVAPI(void) gpuMatBitwiseXor(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask)
{
   cv::gpu::bitwise_xor(*src1, *src2, *dst, mask ? *mask : cv::gpu::GpuMat());
}

CVAPI(void) gpuMatLaplacian(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int ksize, double scale)
{
   cv::gpu::Laplacian(*src, *dst, src->depth(), ksize, scale);
}