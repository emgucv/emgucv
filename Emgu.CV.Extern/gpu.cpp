#include "opencv2/gpu/gpu.hpp"

CVAPI(int) gpuGetCudaEnabledDeviceCount()
{
   return cv::gpu::getCudaEnabledDeviceCount();
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