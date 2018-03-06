//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cvblob_c.h"

unsigned int cvbCvLabel(const IplImage *img, IplImage *imgOut, cvb::CvBlobs* blobs)
{
   return cvb::cvLabel(img, imgOut, *blobs);
}

CvScalar cvbCvBlobMeanColor(const cvb::CvBlob* blob, const IplImage* imgLabel, const IplImage* img)
{
   CvScalar color = cvb::cvBlobMeanColor(blob, imgLabel, img);
   //change RGB to BGR
   double tmp = color.val[0];
   color.val[0] = color.val[2];
   color.val[2] = tmp;
   return color;
}

void cvbCvFilterLabels(IplImage *imgIn, IplImage *imgOut, const cvb::CvBlobs* blobs)
{
   cvb::cvFilterLabels(imgIn, imgOut, *blobs);
}

//Blobs
cvb::CvBlobs* cvbCvBlobsCreate()
{
   return new cvb::CvBlobs();
}

void cvbCvFilterByArea(cvb::CvBlobs* blobs, unsigned int minArea, unsigned int maxArea)
{
   cvb::cvFilterByArea(*blobs, minArea, maxArea);
}

int cvbCvBlobsGetSize(cvb::CvBlobs* blobs)
{
   return blobs->size();
}

void cvbCvBlobsClear(cvb::CvBlobs* blobs)
{
   cvb::cvReleaseBlobs(*blobs);
}

bool cvbCvBlobsAdd(cvb::CvBlobs* blobs, unsigned int label, cvb::CvBlob* blob)
{
   if (!blob) return false;

   cvb::CvBlob* newBlob = new cvb::CvBlob();
   memcpy(newBlob, blob, sizeof(cvb::CvBlob));

   std::pair< cvb::CvBlobs::iterator, bool > result = blobs->insert(cvb::CvLabelBlob(label, newBlob));

   if (!result.second)
   {  //key already exist
      //blob is not added
      //we should delete the newBlob to avoid memory leak
      cvReleaseBlob(newBlob);
      delete newBlob;
   }
   return result.second;
}

cvb::CvBlob* cvbCvBlobsFind(cvb::CvBlobs* blobs, unsigned int label)
{
   cvb::CvBlobs::iterator result = blobs->find(label);
   if (result == blobs->end())
      return 0;
   else
      return result->second;
}

void cvbCvBlobsGetBlobs(cvb::CvBlobs* blobs, unsigned int* labelsArray, cvb::CvBlob** blobsArray)
{
   for (cvb::CvBlobs::iterator it = blobs->begin(); it != blobs->end(); ++it)
   {
      *labelsArray++ = it->first;
      *blobsArray++ = it->second;        
   }
}

void cvbCvBlobsRelease(cvb::CvBlobs** blobs)
{
   cvb::cvReleaseBlobs(**blobs);
   delete *blobs;
   *blobs = 0;
}

void cvbCvRenderBlobs(const IplImage *imgLabel, cvb::CvBlobs* blobs, IplImage *imgSource, IplImage *imgDest, unsigned short mode, double alpha)
{
   cvb::cvRenderBlobs(imgLabel, *blobs, imgSource, imgDest, mode, alpha);
}

bool cvbCvBlobsRemove(cvb::CvBlobs* blobs, unsigned int label)
{
   cvb::CvBlobs::iterator findResult = blobs->find(label);
   if (findResult == blobs->end())
      return false;
   else
   {
      delete findResult->second;
      blobs->erase(findResult);
      return true;
   }
}

//Blob
unsigned int cvbCvBlobGetLabel(cvb::CvBlob* blob)
{
   return blob->label;
}

void cvbCvBlobGetRect(cvb::CvBlob* blob, CvRect* rect)
{
   rect->x = blob->minx;
   rect->y = blob->miny;
   rect->width = blob->maxx - blob->minx + 1;
   rect->height = blob->maxy - blob->miny + 1;
}

void cvbCvBlobGetMoment(cvb::CvBlob* blob, CvBlobMoments* moments)
{
   moments->m00 = blob->m00;
   moments->m10 = blob->m10;
   moments->m01 = blob->m01;
   moments->m11 = blob->m11;
   moments->m20 = blob->m20;
   moments->m02 = blob->m02;

   moments->u02 = blob->u02;
   moments->u20 = blob->u20;
   moments->u11 = blob->u11;

   moments->n11 = blob->n11;
   moments->n20 = blob->n20;
   moments->n02 = blob->n02;

   moments->p1 = blob->p1;
   moments->p2 = blob->p2;
}

void cvbCvBlobGetContour(cvb::CvBlob* blob, std::vector<CvPoint>* contour)
{
   std::vector<CvPoint>* pts = cvb::cvConvertChainCodesToPolygon(&blob->contour);
   contour->resize(pts->size());
   memcpy(&(*contour)[0], &(*pts)[0], pts->size() * sizeof(CvPoint));
   delete pts;
}

//CvTracks
cvb::CvTracks* cvbCvTracksCreate() 
{
   return new cvb::CvTracks();
}

void cvbCvTracksRelease(cvb::CvTracks** tracks)
{
   cvb::cvReleaseTracks(**tracks);
   delete *tracks;
   *tracks = 0;
}

void cvbCvUpdateTracks(cvb::CvBlobs* blob, cvb::CvTracks* tracks, const double thDistance, const unsigned int thInactive, const unsigned int thActive)
{
   cvb::cvUpdateTracks(*blob, *tracks, thDistance, thInactive, thActive);
}

int cvbCvTracksGetSize(cvb::CvTracks* tracks)
{
   return tracks->size();
}

void cvbCvTracksClear(cvb::CvTracks* tracks)
{
   cvb::cvReleaseTracks(*tracks);
}

//return true if this is a new id. False if the id already exist and the value in the map will NOT be modified.
bool cvbCvTracksAdd(cvb::CvTracks* tracks, unsigned int id, cvb::CvTrack* track)
{
   if (!track) return false;

   cvb::CvTrack* newTrack = new cvb::CvTrack();
   memcpy(newTrack, track, sizeof(cvb::CvTrack));
   std::pair< cvb::CvTracks::iterator, bool > result = tracks->insert(cvb::CvIDTrack(id, newTrack));

   if (!result.second)
   {  //key already exist
      //track is not added
      //we should delete the newBlob to avoid memory leak
      delete newTrack;
   }
   return result.second;
}

void cvbCvTracksSetTrack(cvb::CvTracks* tracks, unsigned int id, cvb::CvTrack* track)
{
   cvb::CvTracks::iterator findResult = tracks->find(id);
   if (findResult == tracks->end())
   {  //if the key does not exist, add the track.
      cvbCvTracksAdd(tracks, id, track);
   } else
   {  //if the key already exist, update the track value.
      memcpy(findResult->second, track, sizeof(cvb::CvTrack));
   }
}

cvb::CvTrack* cvbCvTracksFind(cvb::CvTracks* tracks, unsigned int id)
{
   cvb::CvTracks::iterator result = tracks->find(id);
   if (result == tracks->end())
      return 0;
   else
      return result->second;
}

void cvbCvTracksGetTracks(cvb::CvTracks* tracks, unsigned int* idsArray, cvb::CvTrack* tracksArray)
{
   for (cvb::CvTracks::iterator it = tracks->begin(); it != tracks->end(); ++it)
   {
      *idsArray++ = it->first;
      memcpy(tracksArray++, it->second, sizeof(cvb::CvTrack));        
   }
}

bool cvbCvTracksRemove(cvb::CvTracks* tracks, unsigned int id)
{
   cvb::CvTracks::iterator findResult = tracks->find(id);
   if (findResult == tracks->end())
      return false;
   else
   {
      delete findResult->second;
      tracks->erase(findResult);
      return true;
   }
}

//CvTrack
bool cvbCvTrackEquals(cvb::CvTrack* track1, cvb::CvTrack* track2)
{
   return memcmp(track1, track2, sizeof(cvb::CvTrack)) == 0;
}