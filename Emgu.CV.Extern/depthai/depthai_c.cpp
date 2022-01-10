//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "depthai_c.h"

Device* depthaiDeviceCreate(cv::String* usb_device, bool usb2_mode)
{
#ifdef HAVE_DEPTHAI
	return new Device(*usb_device, usb2_mode);
#else
	throw_no_depthai();
#endif
}
void depthaiDeviceRelease(Device** usb_device)
{
#ifdef HAVE_DEPTHAI
	delete* usb_device;
	*usb_device = 0;
#else
	throw_no_depthai();
#endif
}

void depthaiDeviceGetAvailableStreams(Device* usb_device, std::vector< cv::String >* availableStreams)
{
#ifdef HAVE_DEPTHAI
	std::vector< std::string > as = usb_device->get_available_streams();
	availableStreams->clear();
	for (std::vector< std::string >::iterator it = as.begin(); it != as.end(); ++it)
	{
		//cv::String s(*it);
		availableStreams->push_back( *it );
	}
#else
	throw_no_depthai();
#endif
}

CNNHostPipeline* depthaiDeviceCreatePipeline(Device* usb_device, cv::String* config_json_str, std::shared_ptr<CNNHostPipeline>** hostedPipelinePtr)
{
#ifdef HAVE_DEPTHAI
 	std::shared_ptr<CNNHostPipeline> ptr = usb_device->create_pipeline(*config_json_str);
	*hostedPipelinePtr = new std::shared_ptr<CNNHostPipeline>(ptr);
	return (*hostedPipelinePtr)->get();
#else
	throw_no_depthai();
#endif
}

void depthaiCNNHostPipelineRelease(std::shared_ptr<CNNHostPipeline>** hostedPipelinePtr)
{
#ifdef HAVE_DEPTHAI
	delete* hostedPipelinePtr;
	hostedPipelinePtr = 0;
#else
	throw_no_depthai();
#endif
}

NNetAndDataPackets* depthaiCNNHostPipelineGetAvailableNNetAndDataPackets(CNNHostPipeline* cnnHostPipeline, bool blocking)
{
#ifdef HAVE_DEPTHAI
	return new NNetAndDataPackets(cnnHostPipeline->getAvailableNNetAndDataPackets(blocking));
#else
	throw_no_depthai();
#endif
}

int depthaiNNetAndDataPacketsGetNNetCount(NNetAndDataPackets* nnetAndDataPackets)
{
#ifdef HAVE_DEPTHAI
	return nnetAndDataPackets->_nnetPackets.size();
#else
	throw_no_depthai();
#endif
}
void depthaiNNetAndDataPacketsGetNNetArr(NNetAndDataPackets* nnetAndDataPackets, NNetPacket** packetArr)
{
#ifdef HAVE_DEPTHAI
	for (auto i = nnetAndDataPackets->_nnetPackets.begin(); i != nnetAndDataPackets->_nnetPackets.end(); ++i)
	{
		*packetArr = (*i).get();
		packetArr++;
	}
#else
	throw_no_depthai();
#endif
}
int depthaiNNetAndDataPacketsGetHostDataPacketCount(NNetAndDataPackets* nnetAndDataPackets)
{
#ifdef HAVE_DEPTHAI
	return nnetAndDataPackets->_hostDataPackets.size();
#else
	throw_no_depthai();
#endif
}
void depthaiNNetAndDataPacketsGetHostDataPacketArr(NNetAndDataPackets* nnetAndDataPackets, HostDataPacket** packetArr)
{
#ifdef HAVE_DEPTHAI
	for (auto i = nnetAndDataPackets->_hostDataPackets.begin(); i != nnetAndDataPackets->_hostDataPackets.end(); ++i)
	{
		*packetArr = (*i).get();
		packetArr++;
	}
#else
	throw_no_depthai();
#endif
}

void depthaiNNetAndDataPacketsRelease(NNetAndDataPackets** nnetAndDataPackets)
{
#ifdef HAVE_DEPTHAI
	delete* nnetAndDataPackets;
	*nnetAndDataPackets = 0;
#else
	throw_no_depthai();
#endif
}

void depthaiHostDataPacketGetDimensions(HostDataPacket* packet, std::vector< int >* dimensions)
{
#ifdef HAVE_DEPTHAI
	dimensions->clear();
	for (auto i = packet->dimensions.begin(); i != packet->dimensions.end(); ++i)
	{
		dimensions->push_back(*i);
	}
#else
	throw_no_depthai();
#endif
}
bool depthaiHostDataPacketGetMetadata(HostDataPacket* packet, FrameMetadata* metadata)
{
#ifdef HAVE_DEPTHAI
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
#else
	throw_no_depthai();
#endif
}


int depthaiNNetPacketGetDetectedObjectsCount(NNetPacket* packet)
{
#ifdef HAVE_DEPTHAI
	auto sharedPtr = packet->getDetectedObjects();
	return sharedPtr->detection_count;
#else
	throw_no_depthai();
#endif
}
void depthaiNNetPacketGetDetectedObjects(NNetPacket* packet, dai::Detection* detections)
{
#ifdef HAVE_DEPTHAI
	auto sharedPtr = packet->getDetectedObjects();
	int count = sharedPtr->detection_count;
	for (int i = 0; i < count; i++)
	{
		detections[i] = sharedPtr->detections[i];
	}
#else
	throw_no_depthai();
#endif
}
bool depthaiNNetPacketGetMetadata(NNetPacket* packet, FrameMetadata* metadata)
{
#ifdef HAVE_DEPTHAI
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
#else
	throw_no_depthai();
#endif
}


FrameMetadata* depthaiFrameMetadataCreate()
{
#ifdef HAVE_DEPTHAI
	return new FrameMetadata();
#else
	throw_no_depthai();
#endif
}
void depthaiFrameMetadataRelease(FrameMetadata** metadata)
{
#ifdef HAVE_DEPTHAI
	delete* metadata;
	*metadata = 0;
#else
	throw_no_depthai();
#endif
}