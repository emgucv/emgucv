## Getting Started with Emgu CV V3.0
### Convert Native Image Object to and from Emgu CV Mat
#### Android
```csharp
using Emgu.CV;
//...

Bitmap nativeBitmap = ... //this is the native android bitmap
Mat m = new Mat(nativeBitmap);
//... process the image  
Bitmap result = m.ToBitmap();
```
#### iOS
```csharp
using Emgu.CV;
//...

CGImage cgImage = ... //this is the native android bitmap
Mat m = new Mat(cvImage);
//... process the image  
CGImage result = m.ToCGImage();
```
### Hello, World
```csharp
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
//...

Mat img = new Mat(200, 400, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
img.SetTo(new MCvScalar(255, 0, 0)); // set it to Blue color

//Draw "Hello, world." on the image using the specific font
CvInvoke.PutText(
   img, 
   "Hello, world", 
   new System.Drawing.Point(10, 80), 
   FontFace.HersheyComplex, 
   1.0, 
   new MCvScalar(0, 255, 0));      
```
### Other advance image processing code
You can find more tutorials available from our  [website](http://www.emgu.com/wiki/index.php/Tutorial#Examples/).
