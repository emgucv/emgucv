//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "aruco_c.h"

cv::aruco::Dictionary* cveArucoGetPredefinedDictionary(int name, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::getPredefinedDictionary(static_cast<cv::aruco::PREDEFINED_DICTIONARY_NAME>(name));
	*sharedPtr = new cv::Ptr<cv::aruco::Dictionary>(ptr);
	return ptr.get();
}

cv::aruco::Dictionary* cveArucoDictionaryCreate1(int nMarkers, int markerSize, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::Dictionary::create(nMarkers, markerSize);
	*sharedPtr = new cv::Ptr<cv::aruco::Dictionary>(ptr);
	return ptr.get();
}
cv::aruco::Dictionary* cveArucoDictionaryCreate2(int nMarkers, int markerSize, cv::Ptr<cv::aruco::Dictionary>* baseDictionary, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::Dictionary::create(nMarkers, markerSize, baseDictionary? *baseDictionary : cv::makePtr<cv::aruco::Dictionary>());
	*sharedPtr = new cv::Ptr<cv::aruco::Dictionary>(ptr);
	return ptr.get();
}
void cveArucoDictionaryRelease(cv::aruco::Dictionary** dict, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
	delete *sharedPtr;
	*dict = 0;
	*sharedPtr = 0;
}

void cveArucoDrawMarker(cv::aruco::Dictionary* dictionary, int id, int sidePixels, cv::_OutputArray* img, int borderBits)
{
	cv::Ptr<cv::aruco::Dictionary> dictPtr(dictionary, [](cv::aruco::Dictionary*) {});
    cv::aruco::drawMarker(dictPtr, id, sidePixels, *img, borderBits);
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
	cv::Ptr<cv::aruco::DetectorParameters> arucoParam = cv::aruco::DetectorParameters::create();
	if (parameters)
		memcpy(arucoParam.get(), parameters, sizeof(cv::aruco::DetectorParameters));
	cv::Ptr<cv::aruco::Dictionary> dictPtr(dictionary, [](cv::aruco::Dictionary*) {});
    cv::aruco::detectMarkers(*image, dictPtr, *corners, *ids, arucoParam, rejectedImgPoints ? *rejectedImgPoints : (cv::OutputArrayOfArrays) cv::noArray());
}

void cveArucoEstimatePoseSingleMarkers(cv::_InputArray* corners, float markerLength,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs)
{
   cv::aruco::estimatePoseSingleMarkers(*corners, markerLength, *cameraMatrix, *distCoeffs, *rvecs, *tvecs);
}

cv::aruco::GridBoard* cveArucoGridBoardCreate(
   int markersX, int markersY, float markerLength, float markerSeparation,
   cv::aruco::Dictionary* dictionary, int firstMarker, cv::aruco::Board** boardPtr, cv::Ptr<cv::aruco::GridBoard>** sharedPtr)
{
	cv::Ptr<cv::aruco::Dictionary> dictPtr(dictionary, [](cv::aruco::Dictionary*) {});
	cv::Ptr<cv::aruco::GridBoard> ptr = cv::aruco::GridBoard::create(markersX, markersY, markerLength, markerSeparation, dictPtr, firstMarker);
	*boardPtr = dynamic_cast<cv::aruco::Board*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::aruco::GridBoard>(ptr);
	return ptr.get();
}

void cveArucoGridBoardDraw(cv::aruco::GridBoard* gridBoard, CvSize* outSize, cv::_OutputArray* img, int marginSize, int borderBits)
{
   gridBoard->draw(*outSize, *img, marginSize, borderBits);
}

void cveArucoGridBoardRelease(cv::aruco::GridBoard** gridBoard, cv::Ptr<cv::aruco::GridBoard>** sharedPtr)
{
   delete *sharedPtr;
   *gridBoard = 0;
   *sharedPtr = 0;
}

cv::aruco::CharucoBoard* cveCharucoBoardCreate(
    int squaresX, int squaresY, float squareLength, float markerLength,
	cv::aruco::Dictionary* dictionary, cv::aruco::Board** boardPtr, cv::Ptr<cv::aruco::CharucoBoard>** sharedPtr)
{
	cv::Ptr<cv::aruco::Dictionary> dictPtr(dictionary, [](cv::aruco::Dictionary*) {});
	cv::Ptr<cv::aruco::CharucoBoard> ptr = cv::aruco::CharucoBoard::create(squaresX, squaresY, squareLength, markerLength, dictPtr);
	*boardPtr = dynamic_cast<cv::aruco::Board*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::aruco::CharucoBoard>(ptr);
	return ptr.get();
}
void cveCharucoBoardDraw(cv::aruco::CharucoBoard* charucoBoard, CvSize* outSize, cv::_OutputArray* img, int marginSize, int borderBits)
{
   charucoBoard->draw(*outSize, *img, marginSize, borderBits);
}
void cveCharucoBoardRelease(cv::aruco::CharucoBoard** charucoBoard, cv::Ptr<cv::aruco::CharucoBoard>** sharedPtr)
{
   delete *sharedPtr;
   *charucoBoard = 0;
   *sharedPtr = 0;
}



void cveArucoRefineDetectedMarkers(
   cv::_InputArray* image, cv::aruco::Board* board, cv::_InputOutputArray* detectedCorners,
   cv::_InputOutputArray* detectedIds, cv::_InputOutputArray* rejectedCorners,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   float minRepDistance, float errorCorrectionRate, bool checkAllOrders,
   cv::_OutputArray* recoveredIdxs, cv::aruco::DetectorParameters* parameters)
{
	cv::Ptr<cv::aruco::DetectorParameters> detectorParametersPtr = cv::aruco::DetectorParameters::create();	
	if (parameters)
		memcpy(detectorParametersPtr.get(), parameters, sizeof(cv::aruco::DetectorParameters));

	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board*) {});

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
	cv::_OutputArray* rvecs, cv::_OutputArray* tvecs,
	cv::_OutputArray* stdDeviationsIntrinsics,
	cv::_OutputArray* stdDeviationsExtrinsics,
	cv::_OutputArray* perViewErrors,
	int flags, CvTermCriteria* criteria)
{
	cv::Ptr<cv::aruco::Board> boardPtr ( board, [](cv::aruco::Board*) {});

	return cv::aruco::calibrateCameraAruco(
		*corners, *ids, *counter, boardPtr, *imageSize,
		*cameraMatrix, *distCoeffs, 
		rvecs ? *rvecs : (cv::OutputArrayOfArrays) cv::noArray(),
		tvecs ? *tvecs : (cv::OutputArrayOfArrays) cv::noArray(),
		stdDeviationsIntrinsics ? *stdDeviationsIntrinsics : (cv::OutputArrayOfArrays) cv::noArray(),
		stdDeviationsExtrinsics ? *stdDeviationsExtrinsics : (cv::OutputArrayOfArrays) cv::noArray(),
		perViewErrors ? *perViewErrors : (cv::OutputArrayOfArrays) cv::noArray(),
		flags, *criteria);
}

double cveArucoCalibrateCameraCharuco(
	cv::_InputArray* charucoCorners, 
	cv::_InputArray* charucoIds, 
	cv::aruco::CharucoBoard* board,
	CvSize* imageSize, 
	cv::_InputOutputArray* cameraMatrix, 
	cv::_InputOutputArray* distCoeffs,
	cv::_OutputArray* rvecs, 
	cv::_OutputArray* tvecs,
	cv::_OutputArray* stdDeviationsIntrinsics,
	cv::_OutputArray* stdDeviationsExtrinsics,
	cv::_OutputArray* perViewErrors,
	int flags, 
	CvTermCriteria* criteria)
{
	cv::Ptr<cv::aruco::CharucoBoard> boardPtr(board, [](cv::aruco::CharucoBoard*) {});

	return cv::aruco::calibrateCameraCharuco(
		*charucoCorners, 
		*charucoIds,  
		boardPtr, 
		*imageSize,
		*cameraMatrix, *distCoeffs,
		rvecs ? *rvecs : (cv::OutputArrayOfArrays) cv::noArray(),
		tvecs ? *tvecs : (cv::OutputArrayOfArrays) cv::noArray(),
		stdDeviationsIntrinsics ? *stdDeviationsIntrinsics : (cv::OutputArrayOfArrays) cv::noArray(),
		stdDeviationsExtrinsics ? *stdDeviationsExtrinsics : (cv::OutputArrayOfArrays) cv::noArray(),
		perViewErrors ? *perViewErrors : (cv::OutputArrayOfArrays) cv::noArray(),
		flags, 
		*criteria);
}

void cveArucoDetectorParametersGetDefault(cv::aruco::DetectorParameters* parameters)
{
   cv::aruco::DetectorParameters p;
   memcpy(parameters, &p, sizeof(cv::aruco::DetectorParameters));
}

int cveArucoInterpolateCornersCharuco(
	cv::_InputArray* markerCorners,
	cv::_InputArray* markerIds,
	cv::_InputArray* image,
	cv::aruco::CharucoBoard* board,
	cv::_OutputArray* charucoCorners,
	cv::_OutputArray* charucoIds,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	int minMarkers)
{
	cv::Ptr<cv::aruco::CharucoBoard> boardPtr ( board, [](cv::aruco::CharucoBoard*) {} );

	return cv::aruco::interpolateCornersCharuco(
		*markerCorners, *markerIds, *image,
		boardPtr,
		*charucoCorners, *charucoIds,
		cameraMatrix ? *cameraMatrix : (cv::InputArray) cv::noArray(),
		distCoeffs ? *distCoeffs : (cv::InputArray) cv::noArray(),
		minMarkers);
}

void cveArucoDrawDetectedCornersCharuco(
	cv::_InputOutputArray* image,
	cv::_InputArray* charucoCorners,
	cv::_InputArray* charucoIds,
	CvScalar* cornerColor)
{
	cv::aruco::drawDetectedCornersCharuco(
		*image, 
		*charucoCorners, 
		charucoIds ? *charucoIds : (cv::InputArray) cv::noArray(), 
		*cornerColor);
}

bool cveArucoEstimatePoseCharucoBoard(
	cv::_InputArray* charucoCorners,
	cv::_InputArray* charucoIds,
	cv::aruco::CharucoBoard* board,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	bool useExtrinsicGuess)
{
	cv::Ptr<cv::aruco::CharucoBoard> boardPtr ( board , [] (cv::aruco::CharucoBoard*) {} );
	return cv::aruco::estimatePoseCharucoBoard(
		*charucoCorners,
		*charucoIds,
		boardPtr,
		*cameraMatrix,
		*distCoeffs,
		*rvec,
		*tvec,
		useExtrinsicGuess);
}

void cveArucoDetectCharucoDiamond(
	cv::_InputArray* image,
	cv::_InputArray* markerCorners,
	cv::_InputArray* markerIds,
	float squareMarkerLengthRate,
	cv::_OutputArray* diamondCorners,
	cv::_OutputArray* diamondIds,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs)
{
	cv::aruco::detectCharucoDiamond(
		*image, *markerCorners, *markerIds,
		squareMarkerLengthRate,
		*diamondCorners, *diamondIds,
		cameraMatrix ? *cameraMatrix : (cv::InputArray) cv::noArray(),
		distCoeffs ? *distCoeffs : (cv::InputArray) cv::noArray());
}

void cveArucoDrawDetectedDiamonds(
	cv::_InputOutputArray* image,
	cv::_InputArray* diamondCorners,
	cv::_InputArray* diamondIds,
	CvScalar* borderColor)
{
	cv::aruco::drawDetectedDiamonds(*image, *diamondCorners, *diamondIds, *borderColor);
}

void cveArucoDrawCharucoDiamond(
	cv::aruco::Dictionary* dictionary,
	int* ids, int squareLength,
	int markerLength,
	cv::_OutputArray* img,
	int marginSize,
	int borderBits)
{
	cv::Ptr<cv::aruco::Dictionary> dictPtr ( dictionary, [] (cv::aruco::Dictionary*) {} );
	cv::Vec4i idsVec(ids[0], ids[1], ids[2], ids[3]);
	cv::aruco::drawCharucoDiamond(dictPtr, idsVec, squareLength, markerLength, *img, marginSize, borderBits);
}

void cveArucoDrawPlanarBoard(
	cv::aruco::Board* board,
	CvSize* outSize,
	cv::_OutputArray* img,
	int marginSize,
	int borderBits)
{
	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board* b) {});
	cv::aruco::drawPlanarBoard(boardPtr, *outSize, *img, marginSize, borderBits);
}


int cveArucoEstimatePoseBoard(
	cv::_InputArray* corners,
	cv::_InputArray* ids,
	cv::aruco::Board* board,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	bool useExtrinsicGuess)
{
	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board* b) {});
	return cv::aruco::estimatePoseBoard(*corners, *ids, boardPtr, *cameraMatrix, *distCoeffs, *rvec, *tvec, useExtrinsicGuess);
}

void cveArucoGetBoardObjectAndImagePoints(
	cv::aruco::Board* board,
	cv::_InputArray* detectedCorners,
	cv::_InputArray* detectedIds,
	cv::_OutputArray* objPoints,
	cv::_OutputArray* imgPoints)
{
	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board* b) {});
	cv::aruco::getBoardObjectAndImagePoints(boardPtr, *detectedCorners, *detectedIds, *objPoints, *imgPoints);
}