//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "wechat_qrcode_c.h"
cv::wechat_qrcode::WeChatQRCode* cveWeChatQRCodeCreate(
    cv::String* detectorPrototxtPath,
    cv::String* detectorCaffeModelPath,
    cv::String* superResolutionPrototxtPath,
    cv::String* superResolutionCaffeModelPath)
{
#ifdef HAVE_OPENCV_WECHAT_QRCODE
    return new cv::wechat_qrcode::WeChatQRCode(
        *detectorPrototxtPath,
        *detectorCaffeModelPath,
        *superResolutionPrototxtPath,
        *superResolutionCaffeModelPath);
#else
    throw_no_wechat_qrcode();
#endif
}

void cveWeChatQRCodeRelease(cv::wechat_qrcode::WeChatQRCode** detector)
{
#ifdef HAVE_OPENCV_WECHAT_QRCODE
    delete* detector;
    *detector = 0;
#else
    throw_no_wechat_qrcode();
#endif
}

void cveWeChatQRCodeDetectAndDecode(
    cv::wechat_qrcode::WeChatQRCode* detector,
    cv::_InputArray* img,
    cv::_OutputArray* points,
    std::vector<std::string>* results)
{
#ifdef HAVE_OPENCV_WECHAT_QRCODE
    std::vector<std::string> r = detector->detectAndDecode(*img, points ? *points : static_cast<cv::OutputArrayOfArrays>(cv::noArray()));
    results->clear();
	for (std::vector<std::string>::iterator it = r.begin(); it != r.end(); it++)
	{
        results->push_back(*it);
	}
#else
    throw_no_wechat_qrcode();
#endif
}