//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "core_c_extra.h"
#include "opencv2/core/directx.hpp"

CVAPI(void) cveDirectxConvertToD3D11Texture2D(cv::_InputArray* src, ID3D11Texture2D* pD3D11Texture2D);

CVAPI(void) cveDirectxConvertFromD3D11Texture2D(ID3D11Texture2D* pD3D11Texture2D, cv::_OutputArray* dst);

CVAPI(void) cveDirectxConvertToD3D10Texture2D(cv::_InputArray* src, ID3D10Texture2D* pD3D10Texture2D);

CVAPI(void) cveDirectxConvertFromD3D10Texture2D(ID3D10Texture2D* pD3D10Texture2D, cv::_OutputArray* dst);

CVAPI(void) cveDirectxConvertToDirect3DSurface9(cv::_InputArray* src, IDirect3DSurface9* pDirect3DSurface9, void* surfaceSharedHandle);

CVAPI(void) cveDirectxConvertFromDirect3DSurface9(IDirect3DSurface9* pDirect3DSurface9, cv::_OutputArray* dst, void* surfaceSharedHandle);