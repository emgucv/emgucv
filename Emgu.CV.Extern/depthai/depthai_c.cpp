//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "depthai_c.h"


dai::Pipeline* daiPipelineCreate()
{
	try
	{
#ifdef HAVE_DEPTHAI
		return new dai::Pipeline();
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void daiPipelineRelease(dai::Pipeline** pipeline)
{
	try
	{
#ifdef HAVE_DEPTHAI
		delete* pipeline;
		*pipeline = 0;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int daiPipelineGetOpenVINOVersion(dai::Pipeline* pipeline)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return pipeline->getOpenVINOVersion();
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

dai::node::ColorCamera* daiPipelineCreateColorCamera(dai::Pipeline* pipeline, std::shared_ptr<dai::node::ColorCamera>** colorCameraSharedPtr, dai::Node** node)
{
	try
	{
#ifdef HAVE_DEPTHAI
		std::shared_ptr<dai::node::ColorCamera> ptr = pipeline->create<dai::node::ColorCamera>();
		*colorCameraSharedPtr = new std::shared_ptr<dai::node::ColorCamera>(ptr);
		dai::node::ColorCamera* colorCameraPtr = (*colorCameraSharedPtr)->get();
		*node = static_cast<dai::Node*>(colorCameraPtr);
		return colorCameraPtr;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void daiColorCameraRelease(std::shared_ptr<dai::node::ColorCamera>** colorCameraSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		delete* colorCameraSharedPtr;
		*colorCameraSharedPtr = 0;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void* daiColorCameraGetPreview(dai::node::ColorCamera* colorCamera)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return &colorCamera->preview;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
int daiColorCameraGetImageOrientation(dai::node::ColorCamera* obj)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return static_cast<int>(obj->getImageOrientation());
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void daiColorCameraSetImageOrientation(dai::node::ColorCamera* obj, int value)
{
	try
	{
#ifdef HAVE_DEPTHAI
		obj->setImageOrientation(static_cast<dai::CameraImageOrientation>(value));
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


dai::node::MonoCamera* daiPipelineCreateMonoCamera(dai::Pipeline* pipeline, std::shared_ptr<dai::node::MonoCamera>** monoCameraSharedPtr, dai::Node** node)
{
	try
	{
#ifdef HAVE_DEPTHAI
		std::shared_ptr<dai::node::MonoCamera> ptr = pipeline->create<dai::node::MonoCamera>();
		*monoCameraSharedPtr = new std::shared_ptr<dai::node::MonoCamera>(ptr);
		dai::node::MonoCamera* monoCameraPtr = (*monoCameraSharedPtr)->get();
		*node = static_cast<dai::Node*>(monoCameraPtr);
		return monoCameraPtr;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void daiMonoCameraRelease(std::shared_ptr<dai::node::MonoCamera>** monoCameraSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		delete* monoCameraSharedPtr;
		*monoCameraSharedPtr = 0;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void* daiMonoCameraGetOutput(dai::node::MonoCamera* monoCamera)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return &monoCamera->out;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void daiMonoCameraSetBoardSocket(dai::node::MonoCamera* monoCamera, int boardSocket)
{
	try
	{
#ifdef HAVE_DEPTHAI
		monoCamera->setBoardSocket(static_cast<dai::CameraBoardSocket>(boardSocket));
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int daiMonoCameraGetBoardSocket(dai::node::MonoCamera* monoCamera)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return static_cast<int>(monoCamera->getBoardSocket());
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
int daiMonoCameraGetImageOrientation(dai::node::MonoCamera* camera)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return static_cast<int>(camera->getImageOrientation());
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void daiMonoCameraSetImageOrientation(dai::node::MonoCamera* camera, int value)
{
	try
	{
#ifdef HAVE_DEPTHAI
		camera->setImageOrientation(static_cast<dai::CameraImageOrientation>(value));
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void daiMonoCameraSetResolution(dai::node::MonoCamera* camera, int resolution)
{
	try
	{
#ifdef HAVE_DEPTHAI
		camera->setResolution(static_cast<dai::MonoCameraProperties::SensorResolution>(resolution));
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int daiMonoCameraGetResolution(dai::node::MonoCamera* camera)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return static_cast<int>(camera->getResolution());
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}


dai::node::XLinkOut* daiPipelineCreateXLinkOut(dai::Pipeline* pipeline, std::shared_ptr<dai::node::XLinkOut>** xlinkOutSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		std::shared_ptr<dai::node::XLinkOut> ptr = pipeline->create<dai::node::XLinkOut>();
		*xlinkOutSharedPtr = new std::shared_ptr<dai::node::XLinkOut>(ptr);
		return (*xlinkOutSharedPtr)->get();
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void daiXLinkOutRelease(std::shared_ptr<dai::node::XLinkOut>** xlinkOutSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		delete* xlinkOutSharedPtr;
		*xlinkOutSharedPtr = 0;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void* daiXLinkOutGetInput(dai::node::XLinkOut* xlinkOut)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return &(xlinkOut->input);
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}


dai::Device* daiDeviceCreate(dai::Pipeline* pipeline)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return new dai::Device(*pipeline);
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

dai::Device* daiDeviceCreate2(dai::Pipeline* pipeline, bool usb2Mode)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return new dai::Device(*pipeline, usb2Mode);
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void daiDeviceRelease(dai::Device** device)
{
	try
	{
#ifdef HAVE_DEPTHAI
		delete* device;
		*device = 0;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void daiDeviceGetInputQueueNames(dai::Device* device, std::vector< cv::String >* names)
{
	try
	{
#ifdef HAVE_DEPTHAI
		auto iqn = device->getInputQueueNames();
		names->clear();
		for (std::vector< std::string >::iterator it = iqn.begin(); it != iqn.end(); ++it)
		{
			names->push_back(*it);
		}
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void daiDeviceGetOutputQueueNames(dai::Device* device, std::vector< cv::String >* names)
{
	try
	{
#ifdef HAVE_DEPTHAI
		auto iqn = device->getOutputQueueNames();
		names->clear();
		for (std::vector< std::string >::iterator it = iqn.begin(); it != iqn.end(); ++it)
		{
			names->push_back(*it);
		}
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

dai::DataOutputQueue* daiDeviceGetOutputQueue(dai::Device* device, cv::String* name, std::shared_ptr<dai::DataOutputQueue>** outputQueueSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		std::shared_ptr<dai::DataOutputQueue> ptr = device->getOutputQueue(*name);
		*outputQueueSharedPtr = new std::shared_ptr<dai::DataOutputQueue>(ptr);
		return (*outputQueueSharedPtr)->get();
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void daiDataOutputQueueRelease(std::shared_ptr<dai::DataOutputQueue>** outputQueueSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		delete* outputQueueSharedPtr;
		*outputQueueSharedPtr = 0;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

dai::ImgFrame* daiDataOutputQueueGetImgFrame(dai::DataOutputQueue* dataOutputQueue, std::shared_ptr<dai::ImgFrame>** imgFrameSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		std::shared_ptr<dai::ImgFrame> ptr = dataOutputQueue->get<dai::ImgFrame>();
		*imgFrameSharedPtr = new std::shared_ptr<dai::ImgFrame>(ptr);
		return (*imgFrameSharedPtr)->get();
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void daiImgFrameRelease(std::shared_ptr<dai::ImgFrame>** imgFrameSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		delete* imgFrameSharedPtr;
		*imgFrameSharedPtr = 0;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void* daiImgFrameGetData(dai::ImgFrame* imgFrame)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return imgFrame->getData().data();
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

#ifdef HAVE_DEPTHAI
class daiNode : public dai::Node
{
public:
	static void NodeOutputLink(void* outputNode, void* inputNode)
	{
		Output* o = (dai::Node::Output*)(outputNode);
		Input* i = (dai::Node::Input*)(inputNode);
		o->link(*i);
	}

	static void NodeOutputGetName(void* outputNode, cv::String* name)
	{
		Output* o = (dai::Node::Output*)(outputNode);
		*name = o->name;
	}

	static void NodeInputGetName(void* inputNode, cv::String* name)
	{
		Input* i = (dai::Node::Input*)(inputNode);
		*name = i->name;
	}
};
#endif

void daiNodeOutputLink(void* nodeOutput, void* nodeInput)
{
	try
	{
#ifdef HAVE_DEPTHAI
		daiNode::NodeOutputLink(nodeOutput, nodeInput);
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void daiNodeOutputGetName(void* nodeOutput, cv::String* name)
{
	try
	{
#ifdef HAVE_DEPTHAI
		daiNode::NodeOutputGetName(nodeOutput, name);
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void daiNodeInputGetName(void* nodeInput, cv::String* name)
{
	try
	{
#ifdef HAVE_DEPTHAI
		daiNode::NodeInputGetName(nodeInput, name);
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

dai::node::NeuralNetwork* daiPipelineCreateNeuralNetwork(dai::Pipeline* pipeline, std::shared_ptr<dai::node::NeuralNetwork>** neuralNetworkSharedPtr, dai::Node** nodePtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		std::shared_ptr<dai::node::NeuralNetwork> ptr = pipeline->create<dai::node::NeuralNetwork>();
		*neuralNetworkSharedPtr = new std::shared_ptr<dai::node::NeuralNetwork>(ptr);
		dai::node::NeuralNetwork* neuralNetworkPtr = (*neuralNetworkSharedPtr)->get();
		*nodePtr = static_cast<dai::Node*>(neuralNetworkPtr);
		return neuralNetworkPtr;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void daiNeuralNetworkRelease(std::shared_ptr<dai::node::NeuralNetwork>** neuralNetworkSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		delete* neuralNetworkSharedPtr;
		*neuralNetworkSharedPtr = 0;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void daiNeuralNetworkSetBlobPath(dai::node::NeuralNetwork* neuralNetwork, cv::String* path)
{
	try
	{
#ifdef HAVE_DEPTHAI
		neuralNetwork->setBlobPath(*path);
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void* daiNeuralNetworkGetInput(dai::node::NeuralNetwork* neuralNetwork)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return &(neuralNetwork->input);
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

dai::node::StereoDepth* daiPipelineCreateStereoDepth(dai::Pipeline* pipeline, std::shared_ptr<dai::node::StereoDepth>** stereoDepthSharedPtr, dai::Node** nodePtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		std::shared_ptr<dai::node::StereoDepth> ptr = pipeline->create<dai::node::StereoDepth>();
		*stereoDepthSharedPtr = new std::shared_ptr<dai::node::StereoDepth>(ptr);
		dai::node::StereoDepth* stereoDepthPtr = (*stereoDepthSharedPtr)->get();
		*nodePtr = static_cast<dai::Node*>(stereoDepthPtr);
		return stereoDepthPtr;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void daiStereoDepthRelease(std::shared_ptr<dai::node::StereoDepth>** stereoDepthSharedPtr)
{
	try
	{
#ifdef HAVE_DEPTHAI
		delete* stereoDepthSharedPtr;
		*stereoDepthSharedPtr = 0;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void* daiStereoDepthGetLeft(dai::node::StereoDepth* stereoDepth)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return &stereoDepth->left;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void* daiStereoDepthGetRight(dai::node::StereoDepth* stereoDepth)
{
	try
	{
#ifdef HAVE_DEPTHAI
		return &stereoDepth->right;
#else
		throw_no_depthai();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
