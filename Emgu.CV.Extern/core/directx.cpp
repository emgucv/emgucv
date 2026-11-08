//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "directx.h"

cv::ocl::Context* cveDirectxOclInitializeContextFromD3D11Device(ID3D11Device* pD3D11Device)
{
	try
	{
		return &cv::directx::ocl::initializeContextFromD3D11Device(pD3D11Device);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveDirectxConvertToD3D11Texture2D(cv::_InputArray* src, ID3D11Texture2D* pD3D11Texture2D)
{
	try
	{
		cv::directx::convertToD3D11Texture2D(*src, pD3D11Texture2D);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDirectxConvertFromD3D11Texture2D(ID3D11Texture2D* pD3D11Texture2D, cv::_OutputArray* dst)
{
	try
	{
		cv::directx::convertFromD3D11Texture2D(pD3D11Texture2D, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDirectxConvertToD3D10Texture2D(cv::_InputArray* src, ID3D10Texture2D* pD3D10Texture2D)
{
	try
	{
		cv::directx::convertToD3D10Texture2D(*src, pD3D10Texture2D);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDirectxConvertFromD3D10Texture2D(ID3D10Texture2D* pD3D10Texture2D, cv::_OutputArray* dst)
{
	try
	{
		cv::directx::convertFromD3D10Texture2D(pD3D10Texture2D, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDirectxConvertToDirect3DSurface9(cv::_InputArray* src, IDirect3DSurface9* pDirect3DSurface9, void* surfaceSharedHandle)
{
	try
	{
		cv::directx::convertToDirect3DSurface9(*src, pDirect3DSurface9, surfaceSharedHandle);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDirectxConvertFromDirect3DSurface9(IDirect3DSurface9* pDirect3DSurface9, cv::_OutputArray* dst, void* surfaceSharedHandle)
{
	try
	{
		cv::directx::convertFromDirect3DSurface9(pDirect3DSurface9, *dst, surfaceSharedHandle);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}