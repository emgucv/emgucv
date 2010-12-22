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
   delete *mat;
}

CVAPI(cv::gpu::GpuMat*) gpuMatCreateFromArr(CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   return new cv::gpu::GpuMat(mat);
}