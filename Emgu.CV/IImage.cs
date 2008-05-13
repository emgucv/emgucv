using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Emgu.CV
{
    public interface IImage
    {
        [ExposableMethod(Exposable=false)]
        Bitmap AsBitmap();

        int Width 
        {
            [ExposableMethod(Exposable = false)]
            get;
        }

        int Height 
        {
            [ExposableMethod(Exposable = false)]
            get;
        }

        void _Not();

        IImage PyrUp();

        IImage PyrDown();

        IImage Laplace(int apertureSize);

        IImage ToGray();

        IImage Resize(int width, int height);

        /// <summary>
        /// The type of color for this image
        /// </summary>
        System.Type TypeOfColor
        {
            [ExposableMethod(Exposable = false)]
            get;
        }

        /// <summary>
        /// The type fo depth for this image
        /// </summary>
        System.Type TypeOfDepth
        {
            [ExposableMethod(Exposable = false)]
            get;
        }

        [ExposableMethod(Exposable = false)]
        ColorType GetColor(Point2D<int> position);

        [ExposableMethod(Exposable = false)]
        void Save(String fileName);
    }
}
