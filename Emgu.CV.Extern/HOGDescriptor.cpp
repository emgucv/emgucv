#include "cvaux.h"

CVAPI(cv::Vector<float>*) cvHOGDescriptorPeopleDetectorCreate() {   return &(cv::HOGDescriptor::getDefaultPeopleDetector()); }
CVAPI(cv::HOGDescriptor*) cvHOGDescriptorDescriptorCreate() { cv::HOGDescriptor hog; return  &hog; }
CVAPI(void) cvHOGDescriptorDescriptorRelease(cv::HOGDescriptor* descriptor) { descriptor->~HOGDescriptor(); }

