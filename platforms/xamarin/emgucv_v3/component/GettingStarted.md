## Getting Started with Emgu CV V3.2

### The demo version only supports the following platforms: 
* Android (x86) 
* iOS (x86 simulator , x64 simulator)

If you are using the demo version, please make sure your app is targeting only the above platforms, or there will be compilation error.

### The full version support the following platforms:
* Android (armeabi, armeabi-v7a, arm64-v8a, x86, x64)
* iOS (x86 simulator , x64 simulator, armeabi-v7, armeabi-v7s, arm64)

If you are using the full version, you can target any platform of your choice.

### To run the demo program, 

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
