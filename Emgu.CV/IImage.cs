using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Emgu.CV
{
    /// <summary>
    /// IImage interface
    /// </summary>
    public interface IImage
    {
        /// <summary>
        /// Convert this image into Bitmap 
        /// </summary>
        /// <returns></returns>
        Bitmap Bitmap
        {
            [ExposableMethod(Exposable = false)]
            get;
        }

        /// <summary>
        /// The width of this image
        /// </summary>
        int Width 
        {
            [ExposableMethod(Exposable = false)]
            get;
        }

        /// <summary>
        /// The height of this image
        /// </summary>
        int Height 
        {
            [ExposableMethod(Exposable = false)]
            get;
        }

        /// <summary>
        /// Inplace compute the complement image
        /// </summary>
        void _Not();

        /// <summary>
        /// Inplace compute the minimum of the image pixel with the specific value
        /// </summary>
        /// <param name="value">the minimun value</param>
        void _Min(double value);

        /// <summary>
        /// Inplace compute the maximum of the image pixel with the specific value
        /// </summary>
        /// <param name="value"></param>
        void _Max(double value);

        /// <summary>
        /// Inplace perform Erode function for <paramref name="iterations"/>
        /// </summary>
        /// <param name="iterations">the number of iterations for erode</param>
        void _Erode(int iterations);

        /// <summary>
        /// Inplace perform Dilate function for <paramref name="iterations"/>
        /// </summary>
        /// <param name="iterations">the number of iterations for dilate</param>
        void _Dilate(int iterations);

        /// <summary>
        /// Inplace fills Array with uniformly distributed random numbers
        /// </summary>
        /// <param name="seed">Seed for the random number generator</param>
        /// <param name="floorValue">the inclusive lower boundary of random numbers range</param>
        /// <param name="ceilingValue">the exclusive upper boundary of random numbers range</param>
        void _RandUniform(UInt64 seed, MCvScalar floorValue, MCvScalar ceilingValue);

        /// <summary>
        /// Inplace fills Array with normally distributed random numbers
        /// </summary>
        /// <param name="seed">Seed for the random number generator</param>
        /// <param name="mean">the mean value of random numbers</param>
        /// <param name="std"> the standard deviation of random numbers</param>
        void _RandNormal(UInt64 seed, MCvScalar mean, MCvScalar std);

        ///<summary>
        ///The function cvPyrUp performs up-sampling step of Gaussian pyramid decomposition. 
        ///First it upsamples <i>this</i> image by injecting even zero rows and columns and then convolves 
        ///result with the specified filter multiplied by 4 for interpolation. 
        ///So the resulting image is four times larger than the source image.
        ///</summary>
        ///<returns> The upsampled image</returns>
        IImage PyrUp();

        ///<summary>
        ///The function PyrDown performs downsampling step of Gaussian pyramid decomposition. 
        ///First it convolves <i>this</i> image with the specified filter and then downsamples the image 
        ///by rejecting even rows and columns.
        ///</summary>
        ///<returns> The downsampled image</returns>
        IImage PyrDown();

        /// <summary>
        /// The function cvLaplace calculates Laplacian of the source image by summing second x- and y- derivatives calculated using Sobel operator:
        /// dst(x,y) = d2src/dx2 + d2src/dy2
        /// Specifying aperture_size=1 gives the fastest variant that is equal to convolving the image with the following kernel:
        ///
        /// |0  1  0|
        /// |1 -4  1|
        /// |0  1  0|
        /// </summary>
        /// <param name="apertureSize">Aperture size </param>
        /// <returns>The Laplacian of the image</returns>
        IImage Laplace(int apertureSize);

        ///<summary>
        /// Scale the image to the specific size 
        /// </summary>
        ///<param name="width">The width of the returned image.</param>
        ///<param name="height">The height of the returned image.</param>
        ///<returns>The resized image</returns>
        IImage Resize(int width, int height);

        ///<summary>
        /// Scale the image to the specific size: width *= scale; height *= scale  
        /// </summary>
        /// <returns>The scaled image</returns>
        IImage Resize(double scale);

        ///<summary> 
        /// Find the edges on this image and marked them in the returned image.
        ///</summary>
        ///<param name="thresh"> The threshhold to find initial segments of strong edges</param>
        ///<param name="threshLinking"> The threshold used for edge Linking</param>
        ///<returns> The edges found by the Canny edge detector</returns>
        IImage Canny(MCvScalar thresh, MCvScalar threshLinking);

        /// <summary>
        /// The function cvSobel calculates the image derivative by convolving the image with the appropriate kernel:
        /// dst(x,y) = dxorder+yodersrc/dxxorder•dyyorder |(x,y)
        /// The Sobel operators combine Gaussian smoothing and differentiation so the result is more or less robust to the noise. Most often, the function is called with (xorder=1, yorder=0, aperture_size=3) or (xorder=0, yorder=1, aperture_size=3) to calculate first x- or y- image derivative.
        /// </summary>
        /// <param name="xorder">Order of the derivative x</param>
        /// <param name="yorder">Order of the derivative y</param>
        /// <param name="apertureSize">Size of the extended Sobel kernel, must be 1, 3, 5 or 7. In all cases except 1, aperture_size ×aperture_size separable kernel will be used to calculate the derivative.</param>
        /// <returns>The result of the sobel edge detector</returns>
        IImage Sobel(int xorder, int yorder, int apertureSize);

        ///<summary> Return a filpped copy of the current image</summary>
        ///<param name="flipType">The type of the flipping</param>
        ///<returns> The flipped copy of <i>this</i> image </returns>
        IImage Flip(CvEnum.FLIP flipType);

        /// <summary>
        /// Rotate the image the specified angle
        /// </summary>
        /// <param name="angle">The angle of rotation in degrees.</param>
        /// <param name="background">The color with wich to fill the background</param>
        /// <param name="crop">If set to true the image is cropped to its original size, possibly losing corners information. If set to false the result image has different size than original and all rotation information is preserved</param>
        /// <returns>The rotated image</returns>
        IImage Rotate(double angle, MCvScalar background, bool crop);

        /// <summary>
        /// performs forward or inverse transform of 1D or 2D floating-point array
        /// </summary>
        /// <param name="type">Transformation flags</param>
        /// <param name="nonzeroRows">Number of nonzero rows to in the source array (in case of forward 2d transform), or a number of rows of interest in the destination array (in case of inverse 2d transform). If the value is negative, zero, or greater than the total number of rows, it is ignored. The parameter can be used to speed up 2d convolution/correlation when computing them via DFT</param>
        /// <returns>The result of DFT</returns>
        IImage DFT(CvEnum.CV_DXT type, int nonzeroRows);

        /// <summary>
        /// performs forward or inverse transform of 2D floating-point image
        /// </summary>
        /// <param name="type">Transformation flags</param>
        /// <returns>The result of DCT</returns>
        IImage DCT(CvEnum.CV_DCT_TYPE type);

        /// <summary>
        /// Convert the current image to grayscale image
        /// </summary>
        /// <returns>The Grayscale image</returns>
        IImage ToGray();

        /// <summary>
        /// Convert the current image to depth of Single
        /// </summary>
        /// <returns>The Single(floating point) image </returns>
        IImage ToSingle();

        /// <summary>
        /// Convert the current image to depth of Byte
        /// </summary>
        /// <returns>The Byte image </returns>
        IImage ToByte();

        /// <summary>
        /// The type of color for this image
        /// </summary>
        Type TypeOfColor
        {
            [ExposableMethod(Exposable = false)]
            get;
        }

        /// <summary>
        /// The type fo depth for this image
        /// </summary>
        Type TypeOfDepth
        {
            [ExposableMethod(Exposable = false)]
            get;
        }

        /// <summary>
        /// Obtain the color from the specific location on the image
        /// </summary>
        /// <param name="position">The location of the pixel</param>
        /// <returns>The color value on the specific <paramref name="position"/></returns>
        [ExposableMethod(Exposable = false)]
        ColorType GetColor(Point2D<int> position);

        /// <summary>
        /// Save the image to the specific <paramref name="fileName"/> 
        /// </summary>
        /// <param name="fileName">The file name of the image</param>
        [ExposableMethod(Exposable = false)]
        void Save(String fileName);
    }
}
