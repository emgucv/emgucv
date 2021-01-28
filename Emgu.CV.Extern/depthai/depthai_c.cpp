//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "depthai_c.h"

Device* depthaiDeviceCreate(cv::String* usb_device, bool usb2_mode)
{
	return new Device(*usb_device, usb2_mode);
}
void depthaiDeviceRelease(Device** usb_device)
{
	delete* usb_device;
	*usb_device = 0;
}

void depthaiDeviceGetAvailableStreams(Device* usb_device, std::vector< cv::String >* availableStreams)
{
	std::vector< std::string > as = usb_device->get_available_streams();
	availableStreams->clear();
	for (std::vector< std::string >::iterator it = as.begin(); it != as.end(); ++it)
	{
		//cv::String s(*it);
		availableStreams->push_back( *it );
	}	
}

CNNHostPipeline* depthaiDeviceCreatePipeline(Device* usb_device, cv::String* config_json_str, std::shared_ptr<CNNHostPipeline>** hostedPipelinePtr)
{
 	std::shared_ptr<CNNHostPipeline> ptr = usb_device->create_pipeline(*config_json_str);
	*hostedPipelinePtr = new std::shared_ptr<CNNHostPipeline>(ptr);
	return (*hostedPipelinePtr)->get();
}

void depthaiCNNHostPipelineRelease(std::shared_ptr<CNNHostPipeline>** hostedPipelinePtr)
{
	delete* hostedPipelinePtr;
	hostedPipelinePtr = 0;
}

NNetAndDataPackets* depthaiCNNHostPipelineGetAvailableNNetAndDataPackets(CNNHostPipeline* cnnHostPipeline, bool blocking)
{
	return new NNetAndDataPackets(cnnHostPipeline->getAvailableNNetAndDataPackets(blocking));
}

int depthaiNNetAndDataPacketsGetNNetCount(NNetAndDataPackets* nnetAndDataPackets)
{
	return nnetAndDataPackets->_nnetPackets.size();
}
void depthaiNNetAndDataPacketsGetNNetArr(NNetAndDataPackets* nnetAndDataPackets, NNetPacket** packetArr)
{
	for (auto i = nnetAndDataPackets->_nnetPackets.begin(); i != nnetAndDataPackets->_nnetPackets.end(); ++i)
	{
		*packetArr = (*i).get();
		packetArr++;
	}
}
int depthaiNNetAndDataPacketsGetHostDataPacketCount(NNetAndDataPackets* nnetAndDataPackets)
{
	return nnetAndDataPackets->_hostDataPackets.size();
}
void depthaiNNetAndDataPacketsGetHostDataPacketArr(NNetAndDataPackets* nnetAndDataPackets, HostDataPacket** packetArr)
{
	for (auto i = nnetAndDataPackets->_hostDataPackets.begin(); i != nnetAndDataPackets->_hostDataPackets.end(); ++i)
	{
		*packetArr = (*i).get();
		packetArr++;
	}
}

void depthaiNNetAndDataPacketsRelease(NNetAndDataPackets** nnetAndDataPackets)
{
	delete* nnetAndDataPackets;
	*nnetAndDataPackets = 0;
}

void depthaiHostDataPacketGetDimensions(HostDataPacket* packet, std::vector< int >* dimensions)
{
	dimensions->clear();
	for (auto i = packet->dimensions.begin(); i != packet->dimensions.end(); ++i)
	{
		dimensions->push_back(*i);
	}
}
bool depthaiHostDataPacketGetMetadata(HostDataPacket* packet, FrameMetadata* metadata)
{
	auto meta = packet->getMetadata();
	if (meta)
	{
		if (!meta->isValid())
			return false;

		//memcpy(metadata, &(*meta), sizeof(FrameMetadata));
		*metadata = *meta;
		return true;
	}
	else
	{
		return false;
	}
}


int depthaiNNetPacketGetDetectedObjectsCount(NNetPacket* packet)
{
	auto sharedPtr = packet->getDetectedObjects();
	return sharedPtr->detection_count;
}
void depthaiNNetPacketGetDetectedObjects(NNetPacket* packet, dai::Detection* detections)
{
	auto sharedPtr = packet->getDetectedObjects();
	int count = sharedPtr->detection_count;
	for (int i = 0; i < count; i++)
	{
		detections[i] = sharedPtr->detections[i];
	}
}
bool depthaiNNetPacketGetMetadata(NNetPacket* packet, FrameMetadata* metadata)
{
	auto meta = packet->getMetadata();
	if (meta)
	{
		if (!meta->isValid())
			return false;
		
		//memcpy(metadata, &(*meta), sizeof(FrameMetadata));
		*metadata = *meta;
		return true;
	} else
	{
		return false;
	}
}


FrameMetadata* depthaiFrameMetadataCreate()
{
	return new FrameMetadata();
}
void depthaiFrameMetadataRelease(FrameMetadata** metadata)
{
	delete* metadata;
	*metadata = 0;
}