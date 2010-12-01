#include "opencv2/gpu/gpu.hpp"

CVAPI(int) gpuGetCudaEnabledDeviceCount()
{
   return cv::gpu::getCudaEnabledDeviceCount();
}