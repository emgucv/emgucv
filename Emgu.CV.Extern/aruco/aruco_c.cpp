//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "aruco_c.h"

cv::aruco::Dictionary* cveArucoGetPredefinedDictionary(int name)
{
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::getPredefinedDictionary(static_cast<cv::aruco::PREDEFINED_DICTIONARY_NAME>(name));
	ptr.addref();
	return ptr.get();
}

cv::aruco::Dictionary* cveArucoDictionaryCreate1(int nMarkers, int markerSize)
{
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::Dictionary::create(nMarkers, markerSize);
	ptr.addref();
	return ptr.get();
}
cv::aruco::Dictionary* cveArucoDictionaryCreate2(int nMarkers, int markerSize, cv::aruco::Dictionary* baseDictionary)
{
	cv::Ptr<cv::aruco::Dictionary> baseDict;
	if (baseDictionary)
	{
		baseDict = baseDictionary;
		baseDict.addref();
	} else
	{
		baseDict = cv::makePtr<cv::aruco::Dictionary>();
	}

	cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::Dictionary::create(nMarkers, markerSize, baseDict);
	ptr.addref();
	return ptr.get();
}
void cveArucoDictionaryRelease(cv::aruco::Dictionary** dict)
{
	delete *dict;
	*dict = 0;
}

void cveArucoDrawMarker(cv::aruco::Dictionary* dictionary, int id, int sidePixels, cv::_OutputArray* img, int borderBits)
{
	cv::Ptr<cv::aruco::Dictionary> arucoDict = dictionary;
	arucoDict.addref();
    cv::aruco::drawMarker(arucoDict, id, sidePixels, *img, borderBits);
}

void cveArucoDrawAxis(cv::_InputOutputArray* image, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* rvec, cv::_InputArray* tvec, float length)
{
   cv::aruco::drawAxis(*image, *cameraMatrix, *distCoeffs, *rvec, *tvec, length);
}

void cveArucoDetectMarkers(
   cv::_InputArray* image, cv::aruco::Dictionary* dictionary, cv::_OutputArray* corners,
   cv::_OutputArray* ids, cv::aruco::DetectorParameters* parameters,
   cv::_OutputArray* rejectedImgPoints)
{
	cv::Ptr<cv::aruco::Dictionary> arucoDict = dictionary;
	arucoDict.addref();
	cv::Ptr<cv::aruco::DetectorParameters> arucoParam = parameters;
	arucoParam.addref();
    cv::aruco::detectMarkers(*image, arucoDict, *corners, *ids, arucoParam, rejectedImgPoints ? *rejectedImgPoints : (cv::OutputArrayOfArrays) cv::noArray());
}

void cveArucoEstimatePoseSingleMarkers(cv::_InputArray* corners, float markerLength,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs)
{
   cv::aruco::estimatePoseSingleMarkers(*corners, markerLength, *cameraMatrix, *distCoeffs, *rvecs, *tvecs);
}

cv::aruco::GridBoard* cveArucoGridBoardCreate(
   int markersX, int markersY, float markerLength, float markerSeparation,
   cv::aruco::Dictionary* dictionary, int firstMarker, cv::aruco::Board** boardPtr)
{
	cv::Ptr<cv::aruco::Dictionary> arucoDict = dictionary;
	arucoDict.addref();
	cv::Ptr<cv::aruco::GridBoard> ptr = cv::aruco::GridBoard::create(markersX, markersY, markerLength, markerSeparation, arucoDict, firstMarker);
	ptr.addref();
	*boardPtr = dynamic_cast<cv::aruco::Board*>(ptr.get());
	return ptr.get();
}

void cveArucoGridBoardDraw(cv::aruco::GridBoard* gridBoard, CvSize* outSize, cv::_OutputArray* img, int marginSize, int borderBits)
{
   gridBoard->draw(*outSize, *img, marginSize, borderBits);
}

void cveArucoGridBoardRelease(cv::aruco::GridBoard** gridBoard)
{
   delete * gridBoard;
   *gridBoard = 0;
}

cv::aruco::CharucoBoard* cveCharucoBoardCreate(
   int squaresX, int squaresY, float squareLength, float markerLength,
   cv::aruco::Dictionary* dictionary, cv::aruco::Board** boardPtr)
{
	cv::Ptr<cv::aruco::Dictionary> dictPtr = dictionary;
	dictPtr.addref();
	cv::Ptr<cv::aruco::CharucoBoard> ptr = cv::aruco::CharucoBoard::create(squaresX, squaresY, squareLength, markerLength, dictPtr);
	
	ptr.addref();
	*boardPtr = dynamic_cast<cv::aruco::Board*>(ptr.get());
	return ptr.get();
}
void cveCharucoBoardDraw(cv::aruco::CharucoBoard* charucoBoard, CvSize* outSize, cv::_OutputArray* img, int marginSize, int borderBits)
{
   charucoBoard->draw(*outSize, *img, marginSize, borderBits);
}
void cveCharucoBoardRelease(cv::aruco::CharucoBoard** charucoBoard)
{
   delete *charucoBoard;
   *charucoBoard = 0;
}



void cveArucoRefineDetectedMarkers(
   cv::_InputArray* image, cv::aruco::Board* board, cv::_InputOutputArray* detectedCorners,
   cv::_InputOutputArray* detectedIds, cv::_InputOutputArray* rejectedCorners,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   float minRepDistance, float errorCorrectionRate, bool checkAllOrders,
   cv::_OutputArray* recoveredIdxs, cv::aruco::DetectorParameters* parameters)
{
	cv::Ptr<cv::aruco::Board> boardPtr = board;
	boardPtr.addref();
	cv::Ptr<cv::aruco::DetectorParameters> detectorParametersPtr = cv::aruco::DetectorParameters::create();
	
	if (parameters)
	{
		detectorParametersPtr = parameters;
		detectorParametersPtr.addref();
	}
   cv::aruco::refineDetectedMarkers(
      *image, boardPtr, *detectedCorners, *detectedIds, *rejectedCorners,
      cameraMatrix ? *cameraMatrix : static_cast<cv::InputArray>(cv::noArray()),
      distCoeffs ? *distCoeffs : static_cast<cv::InputArray>(cv::noArray()),
      minRepDistance, errorCorrectionRate, checkAllOrders,
      recoveredIdxs ? *recoveredIdxs : static_cast<cv::OutputArray>(cv::noArray()),
      detectorParametersPtr);
}

void cveArucoDrawDetectedMarkers(
   cv::_InputOutputArray* image, cv::_InputArray* corners,
   cv::_InputArray* ids, CvScalar* borderColor)
{
   cv::aruco::drawDetectedMarkers(*image, *corners, ids ? *ids : static_cast<cv::InputArray>(cv::noArray()), *borderColor);
}

double cveArucoCalibrateCameraAruco(
   cv::_InputArray* corners, cv::_InputArray* ids, cv::_InputArray* counter, cv::aruco::Board* board,
   CvSize* imageSize, cv::_InputOutputArray* cameraMatrix, cv::_InputOutputArray* distCoeffs,
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs, int flags,
   CvTermCriteria* criteria)
{
	cv::Ptr<cv::aruco::Board> boardPtr = board;
	boardPtr.addref();

   return cv::aruco::calibrateCameraAruco(*corners, *ids, *counter, boardPtr, *imageSize,
      *cameraMatrix, *distCoeffs, rvecs ? *rvecs : (cv::OutputArrayOfArrays) cv::noArray(),
      tvecs ? *tvecs : (cv::OutputArrayOfArrays) cv::noArray(), flags, *criteria);
}

void cveArucoDetectorParametersGetDefault(cv::aruco::DetectorParameters* parameters)
{
   cv::aruco::DetectorParameters p;
   memcpy(parameters, &p, sizeof(cv::aruco::DetectorParameters));
}