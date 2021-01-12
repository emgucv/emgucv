//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __IOS__
using System;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using CoreGraphics;
using Emgu.CV.Cuda;
using UIKit;

namespace Emgu.CV
{


   /// <summary>
   /// Provide extension method to convert IInputArray to and from UIImage
   /// </summary>
   public static class UIImageExtension
    {
        /// <summary>
        /// Creating an Image from the UIImage
        /// </summary>
        public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this UIImage uiImage)
           where TColor : struct, IColor
           where TDepth : new()
            //: this( (int) uiImage.Size.Width, (int) uiImage.Size.Height)
        {
            using (CGImage cgImage = uiImage.CGImage)
            {
                return cgImage.ToImage<TColor, TDepth>();
            }
        }

        /// <summary>
        /// Convert this Image object to UIImage
        /// </summary>
        public static UIImage ToUIImage<TColor, TDepth>(this Image<TColor, TDepth> image)
           where TColor : struct, IColor
           where TDepth : new()
        {
            using (CGImage cgImage = image.ToCGImage())
            {
                return UIImage.FromImage(cgImage);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.UMat"/> class from UIImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="uiImage">The UIImage.</param>
        public static UMat ToUMat(this UIImage uiImage, ImreadModes mode = ImreadModes.AnyColor)
        {
            //UMat umat = new UMat ();
            using (CGImage cgImage = uiImage.CGImage)
            {
                //ConvertCGImageToArray (cgImage, this, mode);
                return cgImage.ToUMat(mode);
            }
        }

        /// <summary>
        /// Converts to UIImage.
        /// </summary>
        /// <returns>The UIImage.</returns>
        public static UIImage ToUIImage(this UMat umat)
        {
            using (CGImage tmp = umat.ToCGImage())
            {
                return UIImage.FromImage(tmp);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from UIImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="uiImage">The UIImage.</param>
        public static Mat ToMat(this UIImage uiImage, ImreadModes mode = ImreadModes.AnyColor)
        {
            using (CGImage cgImage = uiImage.CGImage)
            {
                return cgImage.ToMat(mode);
            }
        }

        /// <summary>
        /// Converts to UIImage.
        /// </summary>
        /// <returns>The UIImage.</returns>
        public static UIImage ToUIImage(this Mat mat)
        {
            using (CGImage tmp = mat.ToCGImage())
            {
                return UIImage.FromImage(tmp);
            }
        }

        /// <summary>
        /// Converts to UIImage.
        /// </summary>
        /// <returns>The UIImage.</returns>
        public static UIImage ToUIImage(this GpuMat gpuMat)
        {
            using (Mat tmp = new Mat())
            {
                gpuMat.Download(tmp);
                return tmp.ToUIImage();
            }
        }
    }

    /// <summary>
    /// Class that can be used to read a file to Mat using UIImage
    /// </summary>
    public class UIImageFileReaderMat : Emgu.CV.IFileReaderMat
    {
        /// <summary>
        /// Read the file into Mat using UIImage
        /// </summary>
        /// <param name="fileName">The image file to be read</param>
        /// <param name="mat">The Mat to read the file into</param>
        /// <param name="loadType">Image load type</param>
        /// <returns>True if successfully read the file into the Mat</returns>
        public bool ReadFile(String fileName, Mat mat, CvEnum.ImreadModes loadType)
        {
            try
            {
                //try again to load with UIImage
                using (UIImage tmp = UIImage.FromFile(fileName))
                {
                    tmp.CGImage.ToArray(mat, loadType);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }

    /*
    public class UIImageFileReaderImage : Emgu.CV.IFileReaderImage
    {
        public bool ReadFile<TColor, TDepth>(String fileName, Image<TColor, TDepth> image)
            where TColor : struct, IColor
            where TDepth : new()
        {
            try
            {
                //try again to load with UIImage
                using (UIImage tmp = UIImage.FromFile(fileName))
                {
                    tmp.CGImage.ToImage<TColor, TDepth>(image);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }*/
}

#endif