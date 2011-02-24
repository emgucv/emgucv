#include "opencv2/gpu/gpu.hpp"

CVAPI(cv::gpu::SURF_GPU*) gpuSURFDetectorCreate(            
            //! The interest operator threshold
            float threshold,
            //! The number of octaves to process
            int nOctaves,
            //! The number of intervals in each octave
            int nIntervals,
            //! The scale associated with the first interval of the first octave
            float initialScale,
            //! mask parameter l_1
            float l1,
            //! mask parameter l_2 
            float l2,
            //! mask parameter l_3
            float l3,
            //! mask parameter l_4
            float l4,
            //! The amount to scale the edge rejection mask
            float edgeScale,
            //! The initial sampling step in pixels.
            int initialStep,
            //! True, if generate 128-len descriptors, false - 64-len descriptors
            bool extended,
            //! max features = featuresRatio * img.size().srea()
            float featuresRatio)
{
   cv::gpu::SURF_GPU* result = new cv::gpu::SURF_GPU();
   result->threshold = threshold;
   result->nOctaves = nOctaves;
   result->nIntervals = nIntervals;
   result->initialScale = initialScale;
   result->l1 = l1;
   result->l2 = l2;
   result->l3 = l3;
   result->l4 = l4;
   result->edgeScale = edgeScale;
   result->initialStep = initialStep;
   result->extended = extended;
   result->featuresRatio = featuresRatio;
   return result;
}

CVAPI(void) gpuSURFDetectorRelease(cv::gpu::SURF_GPU** detector)
{
   delete *detector;
}

CVAPI(void) gpuSURFDetectorDetectKeyPoints(cv::gpu::SURF_GPU* detector, const cv::gpu::GpuMat* img, const cv::gpu::GpuMat* mask, cv::gpu::GpuMat* keypoints)
{
   (*detector)(*img, mask ? *mask : cv::gpu::GpuMat() , *keypoints);
}

CVAPI(void) gpuDownloadKeypoints(const cv::gpu::GpuMat* keypointsGPU, vector<cv::KeyPoint>* keypoints)
{
   cv::gpu::SURF_GPU::downloadKeypoints(*keypointsGPU, *keypoints);
}

CVAPI(void) gpuUploadKeypoints(const vector<cv::KeyPoint>* keypoints, cv::gpu::GpuMat* keypointsGPU)
{
   cv::gpu::SURF_GPU::uploadKeypoints(*keypoints, *keypointsGPU);
}

CVAPI(void) gpuSURFDetectorCompute(
   cv::gpu::SURF_GPU* detector, 
   const cv::gpu::GpuMat* img, 
   const cv::gpu::GpuMat* mask, 
   cv::gpu::GpuMat* keypoints, 
   cv::gpu::GpuMat* descriptors, 
   bool useProvidedKeypoints, 
   bool calcOrientation)
{
   (*detector)(
      *img, 
      mask? *mask : cv::gpu::GpuMat(), 
      *keypoints,
      *descriptors,
      useProvidedKeypoints,
      calcOrientation);
}

CVAPI(int) gpuSURFDetectorGetDescriptorSize(cv::gpu::SURF_GPU* detector)
{
   return detector->descriptorSize();
}