//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_WECHAT_QRCODE_C_H
#define EMGU_WECHAT_QRCODE_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_WECHAT_QRCODE
#include "opencv2/wechat_qrcode.hpp"
#else
static inline CV_NORETURN void throw_no_wechat_qrcode() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without wechat qrcode support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv
{
	namespace wechat_qrcode
	{
		class WeChatQRCode {};
	}
}

#endif

CVAPI(cv::wechat_qrcode::WeChatQRCode*) cveWeChatQRCodeCreate(
    cv::String* detectorPrototxtPath,
    cv::String* detectorCaffeModelPath,
    cv::String* superResolutionPrototxtPath,
    cv::String* superResolutionCaffeModelPath);

CVAPI(void) cveWeChatQRCodeRelease(cv::wechat_qrcode::WeChatQRCode** detector);

CVAPI(void) cveWeChatQRCodeDetectAndDecode(
    cv::wechat_qrcode::WeChatQRCode* detector,
    cv::_InputArray* img,
    cv::_OutputArray* points, 
    std::vector<std::string>* results);


#endif