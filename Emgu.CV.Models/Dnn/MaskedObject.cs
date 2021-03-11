//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Dnn;
using Emgu.CV.Freetype;
using Emgu.CV.Models;
using Emgu.CV.Util;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace Emgu.CV.Models
{
    /// <summary>
    /// A DetectedObject associated with a mask
    /// </summary>
    public class MaskedObject : DetectedObject, IDisposable
    {
        /// <summary> Track whether Dispose has been called. </summary>
        private bool _disposed;

        private Mat _mask = new Mat();

        /// <summary>
        /// Create a new masked object
        /// </summary>
        /// <param name="classId">The class id</param>
        /// <param name="label">The label</param>
        /// <param name="confident">The confident</param>
        /// <param name="region">The region.</param>
        /// <param name="mask">The mask, we will make a copy of the mask and stored with the MaskedObject</param>
        public MaskedObject(int classId, String label, double confident, Rectangle region, Mat mask)
        {
            ClassId = classId;
            Label = label;
            Confident = confident;
            Region = region;
            mask.CopyTo(_mask);
        }

        /// <summary>
        /// Draw the detected object on the image
        /// </summary>
        /// <param name="image">The image to draw on</param>
        /// <param name="color">The color used for drawing the region</param>
        /// <param name="maskColor">The color used for drawing the mask</param>
        /// <param name="freetype2">Optional freetype2 object, if provided, it will be used to draw the label. If null, will use CvInvoke.PutText instead.</param>
        public virtual void Render(IInputOutputArray image, MCvScalar color, MCvScalar maskColor, Freetype2 freetype2 = null)
        {
            base.Render(image, color, freetype2);
            DrawMask(image, _mask, Region, maskColor);
        }


        private static void DrawMask(IInputOutputArray image, Mat mask, Rectangle rect, MCvScalar color)
        {
            using (Mat maskLarge = new Mat())
            using (Mat maskLargeInv = new Mat())
            using(InputArray iaImage = image.GetInputArray())
            using(Mat matImage = iaImage.GetMat())
            using (Mat subRegion = new Mat(matImage, rect))
            using (Mat largeColor = new Mat(
                subRegion.Size, 
                Emgu.CV.CvEnum.DepthType.Cv8U,
                3))
            {
                CvInvoke.Resize(mask, maskLarge, rect.Size);

                //give the mask at least 30% transparency
                using (ScalarArray sa = new ScalarArray(0.7))
                    CvInvoke.Min(sa, maskLarge, maskLarge);

                //Create the inverse mask for the original image
                using (ScalarArray sa = new ScalarArray(1.0))
                    CvInvoke.Subtract(sa, maskLarge, maskLargeInv);

                //The mask color
                largeColor.SetTo(color);
                if (subRegion.NumberOfChannels == 4)
                {
                    using (Mat bgrSubRegion = new Mat())
                    {
                        CvInvoke.CvtColor(subRegion, bgrSubRegion,
                            ColorConversion.Bgra2Bgr);
                        CvInvoke.BlendLinear(largeColor, bgrSubRegion, maskLarge,
                            maskLargeInv, bgrSubRegion);
                        CvInvoke.CvtColor(bgrSubRegion, subRegion,
                            ColorConversion.Bgr2Bgra);
                    }
                }
                else
                    CvInvoke.BlendLinear(largeColor, subRegion, maskLarge, maskLargeInv,
                        subRegion);
            }
        }

        /// <summary>
        /// Get the Mask
        /// </summary>
        public Mat Mask
        {
            get
            {
                return _mask;
            }
        }

        /// <summary>
        /// The dispose function that implements IDisposable interface
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary> 
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing"> If disposing equals false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed. </param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_disposed)
            {
                _disposed = true;

                // If disposing equals true, release all managed resources as well
                if (disposing)
                {
                    ReleaseManagedResources();
                }

                //release unmanaged resource.
                DisposeObject();
            }
        }

        /// <summary>
        /// Release the managed resources. This function will be called during the disposal of the current object.
        /// override ride this function if you need to call the Dispose() function on any managed IDisposable object created by the current object
        /// </summary>
        protected virtual void ReleaseManagedResources()
        {
            if (_mask != null)
            {
                _mask.Dispose();
                _mask = null;
            }
        }

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected void DisposeObject()
        {

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MaskedObject()
        {
            Dispose(false);
        }

    }
}
