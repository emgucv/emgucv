//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "aruco_c.h"

void cveArucoDictionaryGenerateImageMarker(cv::aruco::Dictionary* dict, int id, int sizePixels, cv::_OutputArray* _img, int borderBits)
{
#ifdef HAVE_OPENCV_OBJDETECT
	dict->generateImageMarker(id, sizePixels, *_img, borderBits);
#else
	throw_no_objdetect();
#endif
}

cv::aruco::Dictionary* cveArucoGetPredefinedDictionary(int name, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::aruco::Dictionary dict = cv::aruco::getPredefinedDictionary(name);
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::makePtr<cv::aruco::Dictionary>();
	*ptr = dict;
	*sharedPtr = new cv::Ptr<cv::aruco::Dictionary>(ptr);
	return ptr.get();
#else
	throw_no_objdetect();
#endif
}

cv::aruco::Dictionary* cveArucoDictionaryCreate(cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::makePtr<cv::aruco::Dictionary>();
	*sharedPtr = new cv::Ptr<cv::aruco::Dictionary>(ptr);
	return ptr.get();
#else
	throw_no_objdetect();
#endif
}

cv::aruco::Dictionary* cveArucoExtendDictionary(
	int nMarkers, 
	int markerSize, 
	cv::Ptr<cv::aruco::Dictionary>* baseDictionary,
	int randomSeed,
	cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::aruco::Dictionary dict = cv::aruco::extendDictionary(nMarkers, markerSize, *(baseDictionary->get()), randomSeed);
	cv::Ptr<cv::aruco::Dictionary> ptr = cv::makePtr<cv::aruco::Dictionary>();
	*ptr = dict;
	*sharedPtr = new cv::Ptr<cv::aruco::Dictionary>(ptr);
	return ptr.get();
#else
	throw_no_objdetect();
#endif
}

void cveArucoDictionaryRelease(cv::aruco::Dictionary** dict, cv::Ptr<cv::aruco::Dictionary>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete *sharedPtr;
	*dict = 0;
	*sharedPtr = 0;
#else
	throw_no_objdetect();
#endif
}

/*
void cveArucoDrawMarker(cv::aruco::Dictionary* dictionary, int id, int sidePixels, cv::_OutputArray* img, int borderBits)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::aruco::Dictionary> dictPtr(dictionary, [](cv::aruco::Dictionary*) {});
    cv::aruco::drawMarker(dictPtr, id, sidePixels, *img, borderBits);
#else
	throw_no_objdetect();
#endif
}

void cveArucoDrawAxis(cv::_InputOutputArray* image, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* rvec, cv::_InputArray* tvec, float length)
{
#ifdef HAVE_OPENCV_OBJDETECT
   cv::aruco::drawAxis(*image, *cameraMatrix, *distCoeffs, *rvec, *tvec, length);
#else
	throw_no_objdetect();
#endif
}
*/

cv::aruco::ArucoDetector* cveArucoDetectorCreate(
	cv::aruco::Dictionary* dictionary,
	cv::aruco::DetectorParameters* detectorParams,
	cv::aruco::RefineParameters* refineParams,
	cv::Algorithm** algorithm)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::aruco::ArucoDetector* detector = new cv::aruco::ArucoDetector(
		*dictionary,
		detectorParams ? *detectorParams : cv::aruco::DetectorParameters(),
		refineParams ? *refineParams : cv::aruco::RefineParameters());
	*algorithm = dynamic_cast<cv::Algorithm*>(detector);
	return detector;
#else
	throw_no_objdetect();
#endif
}
void cveArucoDetectorRelease(cv::aruco::ArucoDetector** arucoDetector)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* arucoDetector;
	*arucoDetector = 0;
#else
	throw_no_objdetect();
#endif	
}

void cveArucoDetectorDetectMarkers(
	cv::aruco::ArucoDetector* detector,
	cv::_InputArray* image,
	cv::_OutputArray* corners,
	cv::_OutputArray* ids,
	cv::_OutputArray* rejectedImgPoints)
{
#ifdef HAVE_OPENCV_OBJDETECT
	detector->detectMarkers(
		*image,
		*corners,
		*ids,
		rejectedImgPoints ? *rejectedImgPoints : static_cast<cv::OutputArrayOfArrays>(cv::noArray()));
#else
	throw_no_objdetect();
#endif
}

void cveArucoRefineDetectedMarkers(
	cv::aruco::ArucoDetector* detector,
	cv::_InputArray* image, 
	cv::aruco::Board* board, 
	cv::_InputOutputArray* detectedCorners,
	cv::_InputOutputArray* detectedIds, 
	cv::_InputOutputArray* rejectedCorners,
	cv::_InputArray* cameraMatrix, 
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* recoveredIdxs)
{
#ifdef HAVE_OPENCV_OBJDETECT
	detector->refineDetectedMarkers(
		*image, 
		*board, *detectedCorners, *detectedIds, *rejectedCorners,
		cameraMatrix ? *cameraMatrix : static_cast<cv::InputArray>(cv::noArray()),
		distCoeffs ? *distCoeffs : static_cast<cv::InputArray>(cv::noArray()),
		recoveredIdxs ? *recoveredIdxs : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_objdetect();
#endif
}

/*
void cveArucoEstimatePoseSingleMarkers(cv::_InputArray* corners, float markerLength,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs)
{
#ifdef HAVE_OPENCV_OBJDETECT
    cv::aruco::estimatePoseSingleMarkers(*corners, markerLength, *cameraMatrix, *distCoeffs, *rvecs, *tvecs);
#else
	throw_no_objdetect();
#endif
}
*/

cv::aruco::GridBoard* cveArucoGridBoardCreate(
   int markersX, int markersY, float markerLength, float markerSeparation,
   cv::aruco::Dictionary* dictionary, cv::_InputArray* ids, cv::aruco::Board** boardPtr, cv::Ptr<cv::aruco::GridBoard>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT

	cv::aruco::GridBoard* ptr = new cv::aruco::GridBoard(
		cv::Size(markersX, markersY), 
		markerLength, 
		markerSeparation, 
		*dictionary,
		ids ? *ids : static_cast<cv::InputArray>(cv::noArray()));
	*boardPtr = dynamic_cast<cv::aruco::Board*>(ptr);
	*sharedPtr = new cv::Ptr<cv::aruco::GridBoard>(ptr, [](cv::aruco::GridBoard* b) { delete b; });
	return ptr;
#else
	throw_no_objdetect();
#endif
}


void cveArucoBoardGenerateImage(
	cv::aruco::Board* board, 
	cv::Size* outSize, 
	cv::_OutputArray* img, 
	int marginSize, 
	int borderBits)
{
#ifdef HAVE_OPENCV_OBJDETECT
   board->generateImage(*outSize, *img, marginSize, borderBits);
#else
	throw_no_objdetect();
#endif
}

void cveArucoBoardMatchImagePoints(
	cv::aruco::Board* board,
	cv::_InputArray* detectedCorners,
	cv::_InputArray* detectedIds,
	cv::_OutputArray* objPoints,
	cv::_OutputArray* imgPoints)
{
#ifdef HAVE_OPENCV_OBJDETECT
	board->matchImagePoints(*detectedCorners, *detectedIds, *objPoints, *imgPoints);
#else
	throw_no_objdetect();
#endif
}

void cveArucoGridBoardRelease(cv::Ptr<cv::aruco::GridBoard>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
   delete *sharedPtr;
   *sharedPtr = 0;
#else
	throw_no_objdetect();
#endif
}

cv::aruco::CharucoBoard* cveCharucoBoardCreate(
    int squaresX, 
	int squaresY, 
	float squareLength, 
	float markerLength,
	cv::aruco::Dictionary* dictionary, 
	cv::aruco::Board** boardPtr, 
	cv::Ptr<cv::aruco::CharucoBoard>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::aruco::CharucoBoard* ptr = new cv::aruco::CharucoBoard(cv::Size(squaresX, squaresY), squareLength, markerLength, *dictionary, cv::noArray());
	*boardPtr = dynamic_cast<cv::aruco::Board*>(ptr);
	*sharedPtr = new cv::Ptr<cv::aruco::CharucoBoard>(ptr, [](cv::aruco::CharucoBoard* b) {delete b; });
	return ptr;
#else
	throw_no_objdetect();
#endif
}

/*
void cveCharucoBoardDraw(cv::aruco::CharucoBoard* charucoBoard, CvSize* outSize, cv::_OutputArray* img, int marginSize, int borderBits)
{
#ifdef HAVE_OPENCV_OBJDETECT
   charucoBoard->draw(*outSize, *img, marginSize, borderBits);
#else
	throw_no_objdetect();
#endif
}*/

void cveCharucoBoardRelease(cv::aruco::CharucoBoard** charucoBoard, cv::Ptr<cv::aruco::CharucoBoard>** sharedPtr)
{
#ifdef HAVE_OPENCV_OBJDETECT
   delete *sharedPtr;
   *charucoBoard = 0;
   *sharedPtr = 0;
#else
	throw_no_objdetect();
#endif
}

cv::aruco::CharucoParameters* cveCharucoParametersCreate(
	int minMarkers,
	bool tryRefineMarkers,
	bool checkMarkers)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::aruco::CharucoParameters* p = new cv::aruco::CharucoParameters();
	p->minMarkers = minMarkers;
	p->tryRefineMarkers = tryRefineMarkers;
	p->checkMarkers = checkMarkers;
	return p;
#else
	throw_no_objdetect();
#endif
}
void cveCharucoParametersRelease(cv::aruco::CharucoParameters** charucoParameters)
{
	delete* charucoParameters;
	*charucoParameters = 0;
}

cv::aruco::CharucoDetector* cveCharucoDetectorCreate(
	cv::aruco::CharucoBoard* board,
	cv::aruco::CharucoParameters* charucoParams,
	cv::aruco::DetectorParameters* detectorParams,
	cv::aruco::RefineParameters* refineParams,
	cv::Algorithm** algorithm)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::aruco::CharucoDetector* detector = new cv::aruco::CharucoDetector(
		*board, 
		*charucoParams,
		*detectorParams,
		*refineParams);
	*algorithm = dynamic_cast<cv::Algorithm*>(detector);
	return detector;
#else
	throw_no_objdetect();
#endif
}
void cveCharucoDetectorRelease(cv::aruco::CharucoDetector** detector)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* detector;
	*detector = 0;
#else
	throw_no_objdetect();
#endif
}

void cveCharucoDetectorDetectDiamonds(
	cv::aruco::CharucoDetector* detector,
	cv::_InputArray* image,
	cv::_OutputArray* diamondCorners,
	cv::_OutputArray* diamondIds,
	cv::_InputOutputArray* markerCorners,
	cv::_InputOutputArray* markerIds)
{
#ifdef HAVE_OPENCV_OBJDETECT
	detector->detectDiamonds(
		*image, 
		*diamondCorners,
		*diamondIds,
		*markerCorners, 
		*markerIds);
#else
	throw_no_objdetect();
#endif
}

void cveCharucoDetectorDetectBoard(
	cv::aruco::CharucoDetector* detector,
	cv::_InputArray* image,
	cv::_OutputArray* charucoCorners,
	cv::_OutputArray* charucoIds,
	cv::_InputOutputArray* markerCorners,
	cv::_InputOutputArray* markerIds)
{
#ifdef HAVE_OPENCV_OBJDETECT
	detector->detectBoard(
		*image,
		*charucoCorners,
		*charucoIds,
		markerCorners? *markerCorners : static_cast<cv::InputOutputArrayOfArrays>(cv::noArray()),
		markerIds? *markerIds: static_cast<cv::InputOutputArray>(cv::noArray()));
#else
	throw_no_objdetect();
#endif
}

void cveArucoDrawDetectedMarkers(
   cv::_InputOutputArray* image, cv::_InputArray* corners,
   cv::_InputArray* ids, cv::Scalar* borderColor)
{
#ifdef HAVE_OPENCV_OBJDETECT
   cv::aruco::drawDetectedMarkers(*image, *corners, ids ? *ids : static_cast<cv::InputArray>(cv::noArray()), *borderColor);
#else
	throw_no_objdetect();
#endif
}

/*
double cveArucoCalibrateCameraAruco(
	cv::_InputArray* corners, cv::_InputArray* ids, cv::_InputArray* counter, cv::aruco::Board* board,
	cv::Size* imageSize, cv::_InputOutputArray* cameraMatrix, cv::_InputOutputArray* distCoeffs,
	cv::_OutputArray* rvecs, cv::_OutputArray* tvecs,
	cv::_OutputArray* stdDeviationsIntrinsics,
	cv::_OutputArray* stdDeviationsExtrinsics,
	cv::_OutputArray* perViewErrors,
	int flags, cv::TermCriteria* criteria)
{
#ifdef HAVE_OPENCV_OBJDETECT
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
	throw_no_objdetect();
#endif
}

double cveArucoCalibrateCameraCharuco(
	cv::_InputArray* charucoCorners, 
	cv::_InputArray* charucoIds, 
	cv::aruco::CharucoBoard* board,
	cv::Size* imageSize, 
	cv::_InputOutputArray* cameraMatrix, 
	cv::_InputOutputArray* distCoeffs,
	cv::_OutputArray* rvecs, 
	cv::_OutputArray* tvecs,
	cv::_OutputArray* stdDeviationsIntrinsics,
	cv::_OutputArray* stdDeviationsExtrinsics,
	cv::_OutputArray* perViewErrors,
	int flags, 
	cv::TermCriteria* criteria)
{
#ifdef HAVE_OPENCV_OBJDETECT
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
	throw_no_objdetect();
#endif
}
*/

void cveArucoDetectorParametersGetDefault(cv::aruco::DetectorParameters* parameters)
{
#ifdef HAVE_OPENCV_OBJDETECT
   cv::aruco::DetectorParameters p;
   memcpy(parameters, &p, sizeof(cv::aruco::DetectorParameters));
#else
	throw_no_objdetect();
#endif
}

/*
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
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::aruco::CharucoBoard> boardPtr ( board, [](cv::aruco::CharucoBoard*) {} );

	return cv::aruco::interpolateCornersCharuco(
		*markerCorners, *markerIds, *image,
		boardPtr,
		*charucoCorners, *charucoIds,
		cameraMatrix ? *cameraMatrix : (cv::InputArray) cv::noArray(),
		distCoeffs ? *distCoeffs : (cv::InputArray) cv::noArray(),
		minMarkers);
#else
	throw_no_objdetect();
#endif
}

*/
void cveArucoDrawDetectedCornersCharuco(
	cv::_InputOutputArray* image,
	cv::_InputArray* charucoCorners,
	cv::_InputArray* charucoIds,
	cv::Scalar* cornerColor)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::aruco::drawDetectedCornersCharuco(
		*image, 
		*charucoCorners, 
		charucoIds ? *charucoIds : static_cast<cv::InputArray>(cv::noArray()), 
		*cornerColor);
#else
	throw_no_objdetect();
#endif
}

/*
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
#ifdef HAVE_OPENCV_OBJDETECT
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
	throw_no_objdetect();
#endif
}
*/

void cveArucoDrawDetectedDiamonds(
	cv::_InputOutputArray* image,
	cv::_InputArray* diamondCorners,
	cv::_InputArray* diamondIds,
	cv::Scalar* borderColor)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::aruco::drawDetectedDiamonds(*image, *diamondCorners, *diamondIds, *borderColor);
#else
	throw_no_objdetect();
#endif
}

/*
void cveArucoDrawCharucoDiamond(
	cv::aruco::Dictionary* dictionary,
	int* ids, int squareLength,
	int markerLength,
	cv::_OutputArray* img,
	int marginSize,
	int borderBits)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::aruco::Dictionary> dictPtr ( dictionary, [] (cv::aruco::Dictionary*) {} );
	cv::Vec4i idsVec(ids[0], ids[1], ids[2], ids[3]);
	cv::aruco::drawCharucoDiamond(dictPtr, idsVec, squareLength, markerLength, *img, marginSize, borderBits);
#else
	throw_no_objdetect();
#endif
}


void cveArucoDrawPlanarBoard(
	cv::aruco::Board* board,
	cv::Size* outSize,
	cv::_OutputArray* img,
	int marginSize,
	int borderBits)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board* b) {});
	cv::aruco::drawPlanarBoard(boardPtr, *outSize, *img, marginSize, borderBits);
#else
	throw_no_objdetect();
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
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board* b) {});
	return cv::aruco::estimatePoseBoard(*corners, *ids, boardPtr, *cameraMatrix, *distCoeffs, *rvec, *tvec, useExtrinsicGuess);
#else
	throw_no_objdetect();
#endif
} 

void cveArucoGetBoardObjectAndImagePoints(
	cv::aruco::Board* board,
	cv::_InputArray* detectedCorners,
	cv::_InputArray* detectedIds,
	cv::_OutputArray* objPoints,
	cv::_OutputArray* imgPoints)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Ptr<cv::aruco::Board> boardPtr(board, [](cv::aruco::Board* b) {});
	cv::aruco::getBoardObjectAndImagePoints(boardPtr, *detectedCorners, *detectedIds, *objPoints, *imgPoints);
#else
	throw_no_objdetect();
#endif
}
*/