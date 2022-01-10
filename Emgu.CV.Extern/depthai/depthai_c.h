//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DEPTHAI_C_H
#define EMGU_DEPTHAI_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_DEPTHAI
#include "depthai/device.hpp"
#else
#include <list>

static inline CV_NORETURN void throw_no_depthai() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without depthai support"); }

class NNetPacket {};
class HostDataPacket {};
class Device {};
class CNNHostPipeline {};
class FrameMetadata {};

namespace dai
{
	class Detection {};
}
#endif

class NNetAndDataPackets
{
public:
	std::list< std::shared_ptr< NNetPacket > > _nnetPackets;
	std::list< std::shared_ptr< HostDataPacket > > _hostDataPackets;
	
	NNetAndDataPackets(std::tuple< std::list< std::shared_ptr< NNetPacket > >, std::list< std::shared_ptr< HostDataPacket > > > packets)
	{
		_nnetPackets = std::get<0>(packets);
		_hostDataPackets = std::get<1>(packets);
	}
};

CVAPI(Device*) depthaiDeviceCreate(cv::String* usb_device, bool usb2_mode);
CVAPI(void) depthaiDeviceRelease(Device** usb_device);
CVAPI(void) depthaiDeviceGetAvailableStreams(Device* usb_device, std::vector< cv::String >* availableStreams);

CVAPI(CNNHostPipeline*) depthaiDeviceCreatePipeline(Device* usb_device, cv::String* config_json_str, std::shared_ptr<CNNHostPipeline>** hostedPipelinePtr);
CVAPI(void) depthaiCNNHostPipelineRelease(std::shared_ptr<CNNHostPipeline>** hostedPipelinePtr);

CVAPI(NNetAndDataPackets*) depthaiCNNHostPipelineGetAvailableNNetAndDataPackets(CNNHostPipeline* cnnHostPipeline, bool blocking);
CVAPI(int) depthaiNNetAndDataPacketsGetNNetCount(NNetAndDataPackets* nnetAndDataPackets);
CVAPI(void) depthaiNNetAndDataPacketsGetNNetArr(NNetAndDataPackets* nnetAndDataPackets, NNetPacket** packetArr);
CVAPI(int) depthaiNNetAndDataPacketsGetHostDataPacketCount(NNetAndDataPackets* nnetAndDataPackets);
CVAPI(void) depthaiNNetAndDataPacketsGetHostDataPacketArr(NNetAndDataPackets* nnetAndDataPackets, HostDataPacket** packetArr);
CVAPI(void) depthaiNNetAndDataPacketsRelease(NNetAndDataPackets** nnetAndDataPackets);


CVAPI(void) depthaiHostDataPacketGetDimensions(HostDataPacket* packet, std::vector< int >* dimensions);
CVAPI(bool) depthaiHostDataPacketGetMetadata(HostDataPacket* packet, FrameMetadata* metadata);

CVAPI(int) depthaiNNetPacketGetDetectedObjectsCount(NNetPacket* packet);
CVAPI(void) depthaiNNetPacketGetDetectedObjects(NNetPacket* packet, dai::Detection* detections);
CVAPI(bool) depthaiNNetPacketGetMetadata(NNetPacket* packet, FrameMetadata* metadata);

CVAPI(FrameMetadata*) depthaiFrameMetadataCreate();
CVAPI(void) depthaiFrameMetadataRelease(FrameMetadata** metadata);

#endif