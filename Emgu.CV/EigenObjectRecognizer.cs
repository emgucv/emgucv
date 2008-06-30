using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    public class EigenObjectRecognizer
    {
        private Image<Gray, Single>[] _eigenImages;
        private Image<Gray, Single> _avgImage;
        private Matrix<float>[] _eigenValues;

        public EigenObjectRecognizer(Image<Gray, Byte>[] images, ref MCvTermCriteria termCrit)
        {
            CalcEigenObjects(images, ref termCrit, out _eigenImages, out _avgImage);

            _eigenValues = Array.ConvertAll<Image<Gray, Byte>, Matrix<float>>(images,
                delegate(Image<Gray, Byte> img)
                {
                    return new Matrix<float>(EigenDecomposite(img, _eigenImages, _avgImage));
                });
        }

        /*
        public EigenObjectRecognizer(int totalImages, GetImageCallback getImageCallback, ref MCvTermCriteria termCrit)
        {
            CalcEigenObjects(totalImages, getImageCallback, ref termCrit, out _eigenImages, out _avgImage);

            _eigenValues = new Matrix<float>[totalImages];

            for (int i = 0; i < totalImages; i++)
            {
                _eigenValues[i] = new Matrix<float>(EigenDecomposite(getImageCallback(i), _eigenImages, _avgImage));    
            }
        }

        public delegate Image<Gray, Byte> GetImageCallback(int index);
        */

        #region static methods
        /*
        public static void CalcEigenObjects(int totalImages, GetImageCallback callback, ref MCvTermCriteria termCrit, out Image<Gray, Single>[] eigenImages, out Image<Gray, Single> avg)
        {
            int maxEigenObjs = termCrit.max_iter == 0 ? totalImages : termCrit.max_iter;

            Image<Gray, Byte> img1 = callback(0);

            if (img1 == null)
                throw new NotSupportedException("GetImageCallback should return an image when index = 0");

            int width = img1.Width;
            int height = img1.Height;
            int bufferSize = sizeof(Single) * width * height;
            //bufferSize = 0; 

            #region initialize eigen images
            Image<Gray, Single>[] eigenImgsInit = new Image<Gray, float>[maxEigenObjs];
            for (int i = 0; i < eigenImgsInit.Length; i++)
                eigenImgsInit[i] = new Image<Gray, float>(width, height);
            IntPtr[] eigObjs = Array.ConvertAll<Image<Gray, Single>, IntPtr>(eigenImgsInit, delegate(Image<Gray, Single> img) { return img.Ptr; });
            #endregion

            avg = new Image<Gray, Single>(width, height);
            MUserData udata = new MUserData();
            IntPtr dataPtr; 
            int step;
            MCvSize size;
            CvInvoke.cvGetRawData(avg.Ptr, out dataPtr, out step, out size);
            udata.size1 = size;
            udata.step1 = step;

            CvInvoke.CvCallBack cb = delegate(int index, IntPtr buff, ref MUserData data)
            {
                Image<Gray, Byte> imgByte = callback(index);
                Image<Gray, Single> imgSingle = imgByte.Convert<Gray, Single>();
                
                    IntPtr orig;
                    //int step;
                    //MCvSize size;
                    CvInvoke.cvGetRawData(imgSingle.Ptr, out orig, out step, out size);
                    int dataWidth = sizeof(Single) * size.width;
                    for (int i = 0; i < size.height; i++)
                        Emgu.Utils.memcpy(new IntPtr(buff.ToInt64() + i * dataWidth), new IntPtr(orig.ToInt64() + i * step), dataWidth);
                    //data.addr1 = imgSingle.Ptr;
                    //data.size1 = size;
                    //data.step1 = dataWidth;

                    return 0;
                
            };

            CvInvoke.cvCalcEigenObjects(
                totalImages,
                cb,
                bufferSize,
                ref udata,
                ref termCrit,
                eigObjs,
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
        }*/

        public static void CalcEigenObjects(Image<Gray, Byte>[] inputImages, ref MCvTermCriteria termCrit, out Image<Gray, Single>[] eigenImages, out Image<Gray, Single> avg)
        {
            int width = inputImages[0].Width;
            int height = inputImages[0].Height;

            IntPtr[] inObjs = Array.ConvertAll<Image<Gray, Byte>, IntPtr>(inputImages, delegate(Image<Gray, Byte> img) { return img.Ptr; });

            int maxEigenObjs = termCrit.max_iter == 0 ? inputImages.Length : termCrit.max_iter;

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
                0,
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

        public static float[] EigenDecomposite(Image<Gray, Byte> src, Image<Gray, Single>[] eigenImages, Image<Gray, Single> avg)
        {
            return CvInvoke.cvEigenDecomposite(
                src.Ptr,
                Array.ConvertAll<Image<Gray, Single>, IntPtr>(eigenImages, delegate(Image<Gray, Single> img) { return img.Ptr; }),
                avg.Ptr);
        }
        #endregion

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

            return index;
        }
    }
}
