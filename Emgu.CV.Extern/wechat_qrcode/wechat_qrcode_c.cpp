//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "wechat_qrcode_c.h"
cv::wechat_qrcode::WeChatQRCode* cveWeChatQRCodeCreate(
    cv::String* detectorPrototxtPath,
    cv::String* detectorCaffeModelPath,
    cv::String* superResolutionPrototxtPath,
    cv::String* superResolutionCaffeModelPath)
{
    return new cv::wechat_qrcode::WeChatQRCode(
        *detectorPrototxtPath,
        *detectorCaffeModelPath,
        *superResolutionPrototxtPath,
        *superResolutionCaffeModelPath);
}

void cveWeChatQRCodeRelease(cv::wechat_qrcode::WeChatQRCode** detector)
{
    delete* detector;
    *detector = 0;
}

void cveWeChatQRCodeDetectAndDecode(
    cv::wechat_qrcode::WeChatQRCode* detector,
    cv::_InputArray* img,
    cv::_OutputArray* points,
    std::vector<std::string>* results)
{
    std::vector<std::string> r = detector->detectAndDecode(*img, *points);
    results->clear();
	for (std::vector<std::string>::iterator it = r.begin(); it != r.end(); it++)
	{
        results->push_back(*it);
	}
}