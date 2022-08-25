//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "directx.h"

void cveDirectxConvertToD3D11Texture2D(cv::_InputArray* src, ID3D11Texture2D* pD3D11Texture2D)
{
	cv::directx::convertToD3D11Texture2D(*src, pD3D11Texture2D);
}

void cveDirectxConvertFromD3D11Texture2D(ID3D11Texture2D* pD3D11Texture2D, cv::_OutputArray* dst)
{
	cv::directx::convertFromD3D11Texture2D(pD3D11Texture2D, *dst);
}

void cveDirectxConvertToD3D10Texture2D(cv::_InputArray* src, ID3D10Texture2D* pD3D10Texture2D)
{
	cv::directx::convertToD3D10Texture2D(*src, pD3D10Texture2D);
}

void cveDirectxConvertFromD3D10Texture2D(ID3D10Texture2D* pD3D10Texture2D, cv::_OutputArray* dst)
{
	cv::directx::convertFromD3D10Texture2D(pD3D10Texture2D, *dst);
}

void cveDirectxConvertToDirect3DSurface9(cv::_InputArray* src, IDirect3DSurface9* pDirect3DSurface9, void* surfaceSharedHandle)
{
	cv::directx::convertToDirect3DSurface9(*src, pDirect3DSurface9, surfaceSharedHandle);
}

void cveDirectxConvertFromDirect3DSurface9(IDirect3DSurface9* pDirect3DSurface9, cv::_OutputArray* dst, void* surfaceSharedHandle)
{
	cv::directx::convertFromDirect3DSurface9(pDirect3DSurface9, *dst, surfaceSharedHandle);
}