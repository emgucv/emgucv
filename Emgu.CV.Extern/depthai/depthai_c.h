//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DEPTHAI_C_H
#define EMGU_DEPTHAI_C_H




#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_DEPTHAI
#include "depthai/pipeline/Pipeline.hpp"
#include "depthai/device/Device.hpp"
#include "depthai/pipeline/node/ColorCamera.hpp"
#include "depthai/pipeline/node/MonoCamera.hpp"
#include "depthai/pipeline/node/XLinkOut.hpp"
#include "depthai/pipeline/node/NeuralNetwork.hpp"
#include "depthai/pipeline/node/StereoDepth.hpp"
#include "depthai/pipeline/Node.hpp"
#include "depthai/pipeline/datatype/ImgFrame.hpp"

#else
#include <list>

static inline CV_NORETURN void throw_no_depthai() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without depthai support"); }

namespace dai
{
	class Pipeline {};
	class Node {};
	class DataOutputQueue {};
	class ImgFrame {};
	class Device {};

	namespace node
	{
		class ColorCamera {};
		class MonoCamera {};
		class XLinkOut {};
		class NeuralNetwork {};
		class StereoDepth {};
	}
}
#endif




CVAPI(dai::Pipeline*) daiPipelineCreate();
CVAPI(void) daiPipelineRelease(dai::Pipeline** pipeline);
CVAPI(int) daiPipelineGetOpenVINOVersion(dai::Pipeline* pipeline);

CVAPI(dai::node::ColorCamera*) daiPipelineCreateColorCamera(dai::Pipeline* pipeline, std::shared_ptr<dai::node::ColorCamera>** colorCameraSharedPtr, dai::Node** node);
CVAPI(void) daiColorCameraRelease(std::shared_ptr<dai::node::ColorCamera>** colorCameraSharedPtr);
CVAPI(void*) daiColorCameraGetPreview(dai::node::ColorCamera* colorCamera);
CVAPI(int) daiColorCameraGetImageOrientation(dai::node::ColorCamera* obj);
CVAPI(void) daiColorCameraSetImageOrientation(dai::node::ColorCamera* obj, int value);


CVAPI(dai::node::MonoCamera*) daiPipelineCreateMonoCamera(dai::Pipeline* pipeline, std::shared_ptr<dai::node::MonoCamera>** monoCameraSharedPtr, dai::Node** node);
CVAPI(void) daiMonoCameraRelease(std::shared_ptr<dai::node::MonoCamera>** monoCameraSharedPtr);
CVAPI(void*) daiMonoCameraGetOutput(dai::node::MonoCamera* monoCamera);
CVAPI(void) daiMonoCameraSetBoardSocket(dai::node::MonoCamera* monoCamera, int boardSocket);
CVAPI(int) daiMonoCameraGetBoardSocket(dai::node::MonoCamera* monoCamera);
CVAPI(int) daiMonoCameraGetImageOrientation(dai::node::MonoCamera* camera);
CVAPI(void) daiMonoCameraSetImageOrientation(dai::node::MonoCamera* camera, int value);
CVAPI(void) daiMonoCameraSetResolution(dai::node::MonoCamera* camera, int resolution);
CVAPI(int) daiMonoCameraGetResolution(dai::node::MonoCamera* camera);


CVAPI(dai::node::XLinkOut*) daiPipelineCreateXLinkOut(dai::Pipeline* pipeline, std::shared_ptr<dai::node::XLinkOut>** xlinkOutSharedPtr);
CVAPI(void) daiXLinkOutRelease(std::shared_ptr<dai::node::XLinkOut>** xlinkOutSharedPtr);
CVAPI(void*) daiXLinkOutGetInput(dai::node::XLinkOut* xlinkOut);

CVAPI(dai::Device*) daiDeviceCreate(dai::Pipeline* pipeline);
CVAPI(dai::Device*) daiDeviceCreate2(dai::Pipeline* pipeline, bool usb2Mode);
CVAPI(void) daiDeviceRelease(dai::Device** device);
CVAPI(void) daiDeviceGetInputQueueNames(dai::Device* device, std::vector< cv::String >* names);
CVAPI(void) daiDeviceGetOutputQueueNames(dai::Device* device, std::vector< cv::String >* names);

CVAPI(dai::DataOutputQueue*) daiDeviceGetOutputQueue(dai::Device* device, cv::String* name, std::shared_ptr<dai::DataOutputQueue>** outputQueueSharedPtr);
CVAPI(void) daiDataOutputQueueRelease(std::shared_ptr<dai::DataOutputQueue>** outputQueueSharedPtr);

CVAPI(dai::ImgFrame*) daiDataOutputQueueGetImgFrame(dai::DataOutputQueue* dataOutputQueue, std::shared_ptr<dai::ImgFrame>** imgFrameSharedPtr);
CVAPI(void) daiImgFrameRelease(std::shared_ptr<dai::ImgFrame>** imgFrameSharedPtr);
CVAPI(void*) daiImgFrameGetData(dai::ImgFrame* imgFrame);

CVAPI(void) daiNodeOutputLink(void* nodeOutput, void* nodeInput);
CVAPI(void) daiNodeOutputGetName(void* nodeOutput, cv::String* name);
CVAPI(void) daiNodeInputGetName(void* nodeInput, cv::String* name);


CVAPI(dai::node::NeuralNetwork*) daiPipelineCreateNeuralNetwork(dai::Pipeline* pipeline, std::shared_ptr<dai::node::NeuralNetwork>** neuralNetworkSharedPtr, dai::Node** nodePtr);
CVAPI(void) daiNeuralNetworkRelease(std::shared_ptr<dai::node::NeuralNetwork>** neuralNetworkSharedPtr);
CVAPI(void) daiNeuralNetworkSetBlobPath(dai::node::NeuralNetwork* neuralNetwork, cv::String* path);
CVAPI(void*) daiNeuralNetworkGetInput(dai::node::NeuralNetwork* neuralNetwork);


CVAPI(dai::node::StereoDepth*) daiPipelineCreateStereoDepth(dai::Pipeline* pipeline, std::shared_ptr<dai::node::StereoDepth>** stereoDepthSharedPtr, dai::Node** nodePtr);
CVAPI(void) daiStereoDepthRelease(std::shared_ptr<dai::node::StereoDepth>** stereoDepthSharedPtr);
CVAPI(void*) daiStereoDepthGetLeft(dai::node::StereoDepth* stereoDepth);
CVAPI(void*) daiStereoDepthGetRight(dai::node::StereoDepth* stereoDepth);


#endif