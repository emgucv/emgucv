using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;

namespace Emgu.CV
{
    /// <summary>
    /// An object recognizer using PCA (Principle Components Analysis)
    /// </summary>
    [Serializable]    
    public class EigenObjectRecognizer 
    {
        private Image<Gray, Single>[] _eigenImages;
        private Image<Gray, Single> _avgImage;
        private Matrix<float>[] _eigenValues;
        private string[] _labels;
        private double _simularityThreshold;

        /// <summary>
        /// Get the Eigen vectors that form the eigen space
        /// </summary>
        /// <remarks>The set method is primary used for deserialization, do not attemps to set it unless you know what you are doing</remarks>
        public Image<Gray, Single>[] EigenImages
        {
            get { return _eigenImages; }
            set { _eigenImages = value; }
        }

        /// <summary>
        /// Get or set the labels for the corresponding training image
        /// </summary>
        public String[] Labels
        {
            get { return _labels; }
            set { _labels = value; }
        }

        /// <summary>
        /// Get or set the simularity threshold.
        /// The smaller the number, the more likely an examined image will be treated as unrecognized object. 
        /// Set it to a huge number (e.g. 5000) and the recognizer will always treated the examined image as one of the known object. 
        /// </summary>
        public double SimularityThreshold
        {
            get { return _simularityThreshold; }
            set { _simularityThreshold = value; }
        }

        /// <summary>
        /// Get the average Image. 
        /// </summary>
        /// <remarks>The set method is primary used for deserialization, do not attemps to set it unless you know what you are doing</remarks>
        public Image<Gray, Single> AverageImage
        {
            get { return _avgImage; }
            set { _avgImage = value; }
        }

        /// <summary>
        /// Get the eigen values of each of the training image
        /// </summary>
        /// <remarks>The set method is primary used for deserialization, do not attemps to set it unless you know what you are doing</remarks>
        public Matrix<float>[] EigenValues
        {
            get { return _eigenValues; }
            set { _eigenValues = value; }
        }

        private EigenObjectRecognizer()
        {
        }

        /// <summary>
        /// Create an object recognizer using the specific tranning data and parameters
        /// </summary>
        /// <param name="images">the images used for training, each of them should be the same size. It's recommended the images are histogram normalized</param>
        /// <param name="labels">the labels corresponding to the images</param>
        /// <param name="simularityThreshold">
        /// The simmilarity threshold, [0, ~1000].
        /// The smaller the number, the more likely an examined image will be treated as unrecognized object. 
        /// Set it to a huge number (e.g. 5000) and the recognizer will always treated the examined image as one of the known object. 
        /// </param>
        /// <param name="termCrit">The criteria for recognizer training</param>
        public EigenObjectRecognizer(Image<Gray, Byte>[] images, String[] labels, double simularityThreshold, ref MCvTermCriteria termCrit)
        {
            Debug.Assert(images.Length == labels.Length, "The number of images should equals the number of labels");
            Debug.Assert(simularityThreshold >= 0.0, "Simularity threshold should always >= 0.0");
            
            CalcEigenObjects(images, ref termCrit, out _eigenImages, out _avgImage);

            /*
            _avgImage.SerializationCompressionRatio = 9;

            foreach (Image<Gray, Single> img in _eigenImages)
                //Set the compression ration to best compression. The serialized object can therefore save spaces
                img.SerializationCompressionRatio = 9;
            */

            _eigenValues = Array.ConvertAll<Image<Gray, Byte>, Matrix<float>>(images,
                delegate(Image<Gray, Byte> img)
                {
                    return new Matrix<float>(EigenDecomposite(img, _eigenImages, _avgImage));
                });

            _labels = labels;

            _simularityThreshold = simularityThreshold;
        }

        #region static methods
        /// <summary>
        /// Caculate the eigen images for the specific traning image
        /// </summary>
        /// <param name="trainingImages">The images used for training </param>
        /// <param name="termCrit">The criteria for tranning</param>
        /// <param name="eigenImages">The resulting eigen images</param>
        /// <param name="avg">the resulting average image</param>
        public static void CalcEigenObjects(Image<Gray, Byte>[] trainingImages, ref MCvTermCriteria termCrit, out Image<Gray, Single>[] eigenImages, out Image<Gray, Single> avg)
        {
            int width = trainingImages[0].Width;
            int height = trainingImages[0].Height;

            IntPtr[] inObjs = Array.ConvertAll<Image<Gray, Byte>, IntPtr>(trainingImages, delegate(Image<Gray, Byte> img) { return img.Ptr; });

            int maxEigenObjs = termCrit.max_iter == 0 ? trainingImages.Length : termCrit.max_iter;

            #region initialize eigen images
            Image<Gray, Single>[] eigenImgsInit = new Image<Gray, float>[maxEigenObjs];
            for (int i = 0; i < eigenImgsInit.Length; i++)
                eigenImgsInit[i] = new Image<Gray, float>(width, height);
            IntPtr[] eigObjs = Array.ConvertAll<Image<Gray, Single>, IntPtr>(eigenImgsInit, delegate(Image<Gray, Single> img) { return img.Ptr; });
            #endregion

            float[] eigValsInit = new float[maxEigenObjs];

            avg = new Image<Gray, Single>(width, height);

            CvInvoke.cvCalcEigenObjects(
                inObjs,
                ref termCrit,
                eigObjs,
                eigValsInit,
                avg.Ptr);

            if (maxEigenObjs == termCrit.max_iter)
            {
                eigenImages = eigenImgsInit;
            }
            else
            {
                eigenImages = new Image<Gray, float>[termCrit.max_iter];
                Array.Copy(eigenImgsInit, eigenImages, termCrit.max_iter);
            }
        }

        /// <summary>
        /// Decompose the image as eigen values, using the specific eigen vectors
        /// </summary>
        /// <param name="src">the image to be decomposed</param>
        /// <param name="eigenImages">the eigen images</param>
        /// <param name="avg">the average images</param>
        /// <returns>eigen values of the decomposed image</returns>
        public static float[] EigenDecomposite(Image<Gray, Byte> src, Image<Gray, Single>[] eigenImages, Image<Gray, Single> avg)
        {
            return CvInvoke.cvEigenDecomposite(
                src.Ptr,
                Array.ConvertAll<Image<Gray, Single>, IntPtr>(eigenImages, delegate(Image<Gray, Single> img) { return img.Ptr; }),
                avg.Ptr);
        }
        #endregion

        /// <summary>
        /// Given the eigen value, reconstruct the projected image
        /// </summary>
        /// <param name="eigenValue">the eigen values</param>
        /// <returns>the projected image</returns>
        public Image<Gray, Byte> EigenProjection(float[] eigenValue)
        {
            Image<Gray, Byte> res = new Image<Gray, byte>(_avgImage.Width, _avgImage.Height);
            CvInvoke.cvEigenProjection(
                Array.ConvertAll<Image<Gray, Single>, IntPtr>(_eigenImages, delegate(Image<Gray, Single> img) { return img.Ptr; }),
                eigenValue,
                _avgImage.Ptr,
                res.Ptr);
            return res;
        }

        /// <summary>
        /// Given an image to be examined, find in the database the most similar image and return the index
        /// </summary>
        /// <param name="image">The image to be searched from the database</param>
        /// <returns>
        /// -1, if such none of the images in the database is similar to the given image;
        /// n, the index of the image in database that is most similar to the gicen image
        /// </returns>
        public int FindIndex(Image<Gray, Byte> image)
        {
            Matrix<float> eigenValue = new Matrix<float>(EigenDecomposite(image, _eigenImages, _avgImage));

            float[] dist = Array.ConvertAll<Matrix<float>, float>(_eigenValues,
                delegate(Matrix<float> eigenValueI)
                {
                    return (float)CvInvoke.cvNorm(eigenValue.Ptr, eigenValueI.Ptr, Emgu.CV.CvEnum.NORM_TYPE.CV_L2, IntPtr.Zero);
                });

            #region find the index that has minimum distance
            int index = 0;
            float minDist = dist[0];
            for (int i = 1; i < dist.Length; i++)
            {
                if (dist[i] < minDist)
                {
                    index = i;
                    minDist = dist[i];
                }
            }
            #endregion
            //If the minimum distance is grater than the threhold
            if (minDist >= _simularityThreshold) 
                return -1; //label this image as unrecognized object

            return index;//return the index of the recognized object
        }

        /// <summary>
        /// Try to recognize the image an return a label
        /// </summary>
        /// <param name="image">the image to be recognized</param>
        /// <returns>
        /// Empty String, if not recognized;
        /// Label of the corresponding image, if recognized
        /// </returns>
        public String Recognize(Image<Gray, Byte> image)
        {
            int index = FindIndex(image);
            return index == -1 ? String.Empty : _labels[index];
        }

    }
}
