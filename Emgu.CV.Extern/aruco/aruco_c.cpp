//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "aruco_c.h"

cv::aruco::Dictionary* cveArucoGetPredefinedDictionary(int name, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::getPredefinedDictionary(static_cast<cv::aruco::PREDEFINED_DICTIONARY_NAME>(name));
	*sharedPtr = new cv::Ptr<cv::aruco::Dictionary>(ptr);
	return ptr.get();
#else
	throw_no_aruco();
#endif
}

cv::aruco::Dictionary* cveArucoDictionaryCreate1(int nMarkers, int markerSize, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::Dictionary::create(nMarkers, markerSize);
	*sharedPtr = new cv::Ptr<cv::aruco::Dictionary>(ptr);
	return ptr.get();
#else
	throw_no_aruco();
#endif
}
cv::aruco::Dictionary* cveArucoDictionaryCreate2(int nMarkers, int markerSize, cv::Ptr<cv::aruco::Dictionary>* baseDictionary, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::Dictionary::create(nMarkers, markerSize, baseDictionary? *baseDictionary : cv::makePtr<cv::aruco::Dictionary>());
	*sharedPtr = new cv::Ptr<cv::aruco::Dictionary>(ptr);
	return ptr.get();
#else
	throw_no_aruco();
#endif
}

void cveArucoDictionaryRelease(cv::aruco::Dictionary** dict, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
#ifdef HAVE_OPENCV_ARUCO
	delete *sharedPtr;
	*dict = 0;
	*sharedPtr = 0;
#else
	throw_no_aruco();
#endif
}

void cveArucoDrawMarker(cv::aruco::Dictionary* dictionary, int id, int sidePixels, cv::_OutputArray* img, int borderBits)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Dictionary> dictPtr(dictionary, [](cv::aruco::Dictionary*) {});
    cv::aruco::drawMarker(dictPtr, id, sidePixels, *img, borderBits);
#else
	throw_no_aruco();
#endif
}

void cveArucoDrawAxis(cv::_InputOutputArray* image, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* rvec, cv::_InputArray* tvec, float length)
{
#ifdef HAVE_OPENCV_ARUCO
   cv::aruco::drawAxis(*image, *cameraMatrix, *distCoeffs, *rvec, *tvec, length);
#else
	throw_no_aruco();
#endif
}

void cveArucoDetectMarkers(
   cv::_InputArray* image, cv::aruco::Dictionary* dictionary, cv::_OutputArray* corners,
   cv::_OutputArray* ids, cv::aruco::DetectorParameters* parameters,
   cv::_OutputArray* rejectedImgPoints)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::DetectorParameters> arucoParam = cv::aruco::DetectorParameters::create();
	if (parameters)
		memcpy(arucoParam.get(), parameters, sizeof(cv::aruco::DetectorParameters));
	cv::Ptr<cv::aruco::Dictionary> dictPtr(dictionary, [](cv::aruco::Dictionary*) {});
    cv::aruco::detectMarkers(*image, dictPtr, *corners, *ids, arucoParam, rejectedImgPoints ? *rejectedImgPoints : (cv::OutputArrayOfArrays) cv::noArray());
#else
	throw_no_aruco();
#endif
}

void cveArucoEstimatePoseSingleMarkers(cv::_InputArray* corners, float markerLength,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs)
{
#ifdef HAVE_OPENCV_ARUCO
   cv::aruco::estimatePoseSingleMarkers(*corners, markerLength, *cameraMatrix, *distCoeffs, *rvecs, *tvecs);
#else
	throw_no_aruco();
#endif
}

cv::aruco::GridBoard* cveArucoGridBoardCreate(
   int markersX, int markersY, float markerLength, float markerSeparation,
   cv::aruco::Dictionary* dictionary, int firstMarker, cv::aruco::Board** boardPtr, cv::Ptr<cv::aruco::GridBoard>** sharedPtr)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Dictionary> dictPtr(dictionary, [](cv::aruco::Dictionary*) {});
	cv::Ptr<cv::aruco::GridBoard> ptr = cv::aruco::GridBoard::create(markersX, markersY, markerLength, markerSeparation, dictPtr, firstMarker);
	*boardPtr = dynamic_cast<cv::aruco::Board*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::aruco::GridBoard>(ptr);
	return ptr.get();
#else
	throw_no_aruco();
#endif
}

void cveArucoGridBoardDraw(cv::aruco::GridBoard* gridBoard, CvSize* outSize, cv::_OutputArray* img, int marginSize, int borderBits)
{
#ifdef HAVE_OPENCV_ARUCO
   gridBoard->draw(*outSize, *img, marginSize, borderBits);
#else
	throw_no_aruco();
#endif
}

void cveArucoGridBoardRelease(cv::aruco::GridBoard** gridBoard, cv::Ptr<cv::aruco::GridBoard>** sharedPtr)
{
#ifdef HAVE_OPENCV_ARUCO
   delete *sharedPtr;
   *gridBoard = 0;
   *sharedPtr = 0;
#else
	throw_no_aruco();
#endif
}

cv::aruco::CharucoBoard* cveCharucoBoardCreate(
    int squaresX, int squaresY, float squareLength, float markerLength,
	cv::aruco::Dictionary* dictionary, cv::aruco::Board** boardPtr, cv::Ptr<cv::aruco::CharucoBoard>** sharedPtr)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Dictionary> dictPtr(dictionary, [](cv::aruco::Dictionary*) {});
	cv::Ptr<cv::aruco::CharucoBoard> ptr = cv::aruco::CharucoBoard::create(squaresX, squaresY, squareLength, markerLength, dictPtr);
	*boardPtr = dynamic_cast<cv::aruco::Board*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::aruco::CharucoBoard>(ptr);
	return ptr.get();
#else
	throw_no_aruco();
#endif
}

void cveCharucoBoardDraw(cv::aruco::CharucoBoard* charucoBoard, CvSize* outSize, cv::_OutputArray* img, int marginSize, int borderBits)
{
#ifdef HAVE_OPENCV_ARUCO
   charucoBoard->draw(*outSize, *img, marginSize, borderBits);
#else
	throw_no_aruco();
#endif
}

void cveCharucoBoardRelease(cv::aruco::CharucoBoard** charucoBoard, cv::Ptr<cv::aruco::CharucoBoard>** sharedPtr)
{
#ifdef HAVE_OPENCV_ARUCO
   delete *sharedPtr;
   *charucoBoard = 0;
   *sharedPtr = 0;
#else
	throw_no_aruco();
#endif
}

void cveArucoRefineDetectedMarkers(
   cv::_InputArray* image, cv::aruco::Board* board, cv::_InputOutputArray* detectedCorners,
   cv::_InputOutputArray* detectedIds, cv::_InputOutputArray* rejectedCorners,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   float minRepDistance, float errorCorrectionRate, bool checkAllOrders,
   cv::_OutputArray* recoveredIdxs, cv::aruco::DetectorParameters* parameters)
{
#ifdef HAVE_OPENCV_ARUCO
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
#else
	throw_no_aruco();
#endif
}

void cveArucoDrawDetectedMarkers(
   cv::_InputOutputArray* image, cv::_InputArray* corners,
   cv::_InputArray* ids, CvScalar* borderColor)
{
#ifdef HAVE_OPENCV_ARUCO
   cv::aruco::drawDetectedMarkers(*image, *corners, ids ? *ids : static_cast<cv::InputArray>(cv::noArray()), *borderColor);
#else
	throw_no_aruco();
#endif
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
#ifdef HAVE_OPENCV_ARUCO
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
#else
	throw_no_aruco();
#endif
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
#ifdef HAVE_OPENCV_ARUCO
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
#else
	throw_no_aruco();
#endif
}

void cveArucoDetectorParametersGetDefault(cv::aruco::DetectorParameters* parameters)
{
#ifdef HAVE_OPENCV_ARUCO
   cv::aruco::DetectorParameters p;
   memcpy(parameters, &p, sizeof(cv::aruco::DetectorParameters));
#else
	throw_no_aruco();
#endif
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
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::CharucoBoard> boardPtr ( board, [](cv::aruco::CharucoBoard*) {} );

	return cv::aruco::interpolateCornersCharuco(
		*markerCorners, *markerIds, *image,
		boardPtr,
		*charucoCorners, *charucoIds,
		cameraMatrix ? *cameraMatrix : (cv::InputArray) cv::noArray(),
		distCoeffs ? *distCoeffs : (cv::InputArray) cv::noArray(),
		minMarkers);
#else
	throw_no_aruco();
#endif
}

void cveArucoDrawDetectedCornersCharuco(
	cv::_InputOutputArray* image,
	cv::_InputArray* charucoCorners,
	cv::_InputArray* charucoIds,
	CvScalar* cornerColor)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::aruco::drawDetectedCornersCharuco(
		*image, 
		*charucoCorners, 
		charucoIds ? *charucoIds : (cv::InputArray) cv::noArray(), 
		*cornerColor);
#else
	throw_no_aruco();
#endif
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
#ifdef HAVE_OPENCV_ARUCO
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
#else
	throw_no_aruco();
#endif
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
#ifdef HAVE_OPENCV_ARUCO
	cv::aruco::detectCharucoDiamond(
		*image, *markerCorners, *markerIds,
		squareMarkerLengthRate,
		*diamondCorners, *diamondIds,
		cameraMatrix ? *cameraMatrix : (cv::InputArray) cv::noArray(),
		distCoeffs ? *distCoeffs : (cv::InputArray) cv::noArray());
#else
	throw_no_aruco();
#endif
}

void cveArucoDrawDetectedDiamonds(
	cv::_InputOutputArray* image,
	cv::_InputArray* diamondCorners,
	cv::_InputArray* diamondIds,
	CvScalar* borderColor)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::aruco::drawDetectedDiamonds(*image, *diamondCorners, *diamondIds, *borderColor);
#else
	throw_no_aruco();
#endif
}

void cveArucoDrawCharucoDiamond(
	cv::aruco::Dictionary* dictionary,
	int* ids, int squareLength,
	int markerLength,
	cv::_OutputArray* img,
	int marginSize,
	int borderBits)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Dictionary> dictPtr ( dictionary, [] (cv::aruco::Dictionary*) {} );
	cv::Vec4i idsVec(ids[0], ids[1], ids[2], ids[3]);
	cv::aruco::drawCharucoDiamond(dictPtr, idsVec, squareLength, markerLength, *img, marginSize, borderBits);
#else
	throw_no_aruco();
#endif
}

void cveArucoDrawPlanarBoard(
	cv::aruco::Board* board,
	CvSize* outSize,
	cv::_OutputArray* img,
	int marginSize,
	int borderBits)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board* b) {});
	cv::aruco::drawPlanarBoard(boardPtr, *outSize, *img, marginSize, borderBits);
#else
	throw_no_aruco();
#endif
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
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board* b) {});
	return cv::aruco::estimatePoseBoard(*corners, *ids, boardPtr, *cameraMatrix, *distCoeffs, *rvec, *tvec, useExtrinsicGuess);
#else
	throw_no_aruco();
#endif
}

void cveArucoGetBoardObjectAndImagePoints(
	cv::aruco::Board* board,
	cv::_InputArray* detectedCorners,
	cv::_InputArray* detectedIds,
	cv::_OutputArray* objPoints,
	cv::_OutputArray* imgPoints)
{
#ifdef HAVE_OPENCV_ARUCO
	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board* b) {});
	cv::aruco::getBoardObjectAndImagePoints(boardPtr, *detectedCorners, *detectedIds, *objPoints, *imgPoints);
#else
	throw_no_aruco();
#endif
}