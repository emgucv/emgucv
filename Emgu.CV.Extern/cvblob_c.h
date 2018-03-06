//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#ifndef CVBLOB_C_H
#define CVBLOB_C_H

#include "cvblob.h"

CVAPI(unsigned int) cvbCvLabel(const IplImage *img, IplImage *imgOut, cvb::CvBlobs* blobs);
CVAPI(void) cvbCvRenderBlobs(const IplImage *imgLabel, cvb::CvBlobs* blobs, IplImage *imgSource, IplImage *imgDest, unsigned short mode, double alpha);
CVAPI(CvScalar) cvbCvBlobMeanColor(const cvb::CvBlob* blob, const IplImage* imgLabel, const IplImage* img);
CVAPI(void) cvbCvFilterLabels(IplImage *imgIn, IplImage *imgOut, const cvb::CvBlobs* blobs);

//CvBlobs
CVAPI(cvb::CvBlobs*) cvbCvBlobsCreate();
CVAPI(void) cvbCvFilterByArea(cvb::CvBlobs* blobs, unsigned int minArea, unsigned int maxArea);
CVAPI(void) cvbCvBlobsRelease(cvb::CvBlobs** blobs);
CVAPI(int) cvbCvBlobsGetSize(cvb::CvBlobs* blobs);
CVAPI(void) cvbCvBlobsClear(cvb::CvBlobs* blobs);
//return true if this is a new label. False if the label already exist and the value in the map will NOT be modified.
CVAPI(bool) cvbCvBlobsAdd(cvb::CvBlobs* blobs, unsigned int label, cvb::CvBlob* blob);
CVAPI(cvb::CvBlob*) cvbCvBlobsFind(cvb::CvBlobs* blobs, unsigned int label);
CVAPI(void) cvbCvBlobsGetBlobs(cvb::CvBlobs* blobs, unsigned int* labelsArray, cvb::CvBlob** blobsArray);
CVAPI(bool) cvbCvBlobsRemove(cvb::CvBlobs* blobs, unsigned int label);


typedef struct CvBlobMoments
{
   double m00; // Mement 00.
   double m10; // Moment 10.
   double m01; // Moment 01.
   double m11; // Moment 11.
   double m20; // Moment 20.
   double m02; // Moment 02.

   double u11; // Central moment 11.
   double u20; // Central moment 20.
   double u02; // Central moment 02.

   double n11; // Normalized central moment 11.
   double n20; // Normalized central moment 20.
   double n02; // Normalized central moment 02.

   double p1; // Hu moment 1.
   double p2; // Hu moment 2.
}
CvBlobMoments;

//CvBlob
CVAPI(unsigned int) cvbCvBlobGetLabel(cvb::CvBlob* blob);
CVAPI(void) cvbCvBlobGetRect(cvb::CvBlob* blob, CvRect* rect);
CVAPI(void) cvbCvBlobGetMoment(cvb::CvBlob* blob, CvBlobMoments* moments);
CVAPI(void) cvbCvBlobGetContour(cvb::CvBlob* blob, std::vector<CvPoint>* contour);

//CvTracks
CVAPI(cvb::CvTracks*) cvbCvTracksCreate();
CVAPI(void) cvbCvTracksRelease(cvb::CvTracks** tracks);
CVAPI(void) cvbCvUpdateTracks(cvb::CvBlobs* blob, cvb::CvTracks* tracks, const double thDistance, const unsigned int thInactive, const unsigned int thActive);
CVAPI(int) cvbCvTracksGetSize(cvb::CvTracks* tracks);
CVAPI(void) cvbCvTracksClear(cvb::CvTracks* tracks);
//return true if this is a new id. False if the id already exist and the value in the map will NOT be modified.
CVAPI(bool) cvbCvTracksAdd(cvb::CvTracks* tracks, unsigned int id, cvb::CvTrack* track);
CVAPI(void) cvbCvTracksSetTrack(cvb::CvTracks* tracks, unsigned int id, cvb::CvTrack* track);
CVAPI(cvb::CvTrack*) cvbCvTracksFind(cvb::CvTracks* tracks, unsigned int id);
CVAPI(void) cvbCvTracksGetTracks(cvb::CvTracks* tracks, unsigned int* idsArray, cvb::CvTrack* tracksArray);
CVAPI(bool) cvbCvTracksRemove(cvb::CvTracks* tracks, unsigned int id);

//CvTrack
CVAPI(bool) cvbCvTrackEquals(cvb::CvTrack* track1, cvb::CvTrack* track2);
#endif