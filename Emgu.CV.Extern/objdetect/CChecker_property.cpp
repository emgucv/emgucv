 
   #include "objdetect/CChecker_property.h"
cv::mcc::ColorChart cveCCheckerGetTarget(cv::mcc::CChecker* obj) { return obj->getTarget(); }
void cveCCheckerSetTarget(cv::mcc::CChecker* obj, cv::mcc::ColorChart value) { obj->setTarget( value ); }     
     
float cveCCheckerGetCost(cv::mcc::CChecker* obj) { return obj->getCost(); }
void cveCCheckerSetCost(cv::mcc::CChecker* obj, float value) { obj->setCost( value ); }     
      
  