using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    public class MotionHistory
    {
        private int _bufferMax;
        private int _diffThresh;
        private double _mhiDuration;
        private Image<Gray, Byte> _silh;
        private Image<Gray, Single> _mhi;
        private Image<Gray, Byte> _mask;
        private Image<Gray, Single> _orientation;
        private Image<Gray, Single> _segMask;
        private DateTime _initTime;
        private DateTime _lastTime;
        private double _maxTimeDelta;
        private double _minTimeDelta;

        private Queue<Image<Gray, Byte>> _buffer;

        public MotionHistory(int bufferCount, int diffThresh, double mhiDuration, double maxTimeDelta, double minTimeDelta)
        {
            _bufferMax = bufferCount;
            _buffer = new Queue<Image<Gray, Byte>>(_bufferMax);
            _diffThresh = diffThresh;
            _mhiDuration = mhiDuration;
            _initTime = DateTime.Now;
            _maxTimeDelta = maxTimeDelta;
            _minTimeDelta = minTimeDelta;
        }

        public void Update(Image<Gray, Byte> image, DateTime timestamp)
        {
            _lastTime = timestamp;
            TimeSpan ts = _lastTime.Subtract(_initTime);

            if (_buffer.Count == _bufferMax)
            {
                _buffer.Dequeue();
            }
            _buffer.Enqueue(image);

            if (_silh == null) _silh = image.BlankClone();
            if (_mhi == null) _mhi = new Image<Gray, float>(image.Width, image.Height);
            if (_mask == null) _mask = image.BlankClone();
            if (_orientation == null) _orientation = new Image<Gray, float>(image.Width, image.Height);

            CvInvoke.cvAbsDiff(image.Ptr, _buffer.Peek().Ptr, _silh.Ptr);
            CvInvoke.cvThreshold(_silh.Ptr, _silh.Ptr, _diffThresh, 1, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY);

            CvInvoke.cvUpdateMotionHistory(_silh.Ptr, _mhi, ts.TotalSeconds, _mhiDuration);
            CvInvoke.cvConvertScale(_mhi.Ptr, _mask.Ptr, 255.0 / _mhiDuration, (_mhiDuration - ts.TotalSeconds) * 255.0 / _mhiDuration);

            CvInvoke.cvCalcMotionGradient(_mhi.Ptr, _mask.Ptr, _orientation.Ptr, _maxTimeDelta, _minTimeDelta, 3);
        }

        public Seq<MCvConnectedComp> MotionComponents
        {
            get
            {
                TimeSpan ts = _lastTime.Subtract(_initTime);
                if (_segMask == null) _segMask = new Image<Gray, float>(_mhi.Width, _mhi.Height);
                MemStorage storage = new MemStorage();
                Seq<MCvConnectedComp> seq =
                    new Seq<MCvConnectedComp>(CvInvoke.cvSegmentMotion(_mhi, _segMask, storage, ts.TotalSeconds, _maxTimeDelta), storage);

                return seq;
            }
        }

        public void MotionInfo(MCvRect motionRectangle, out double angle, out double motionPixelCount)
        {
            TimeSpan ts = _lastTime.Subtract(_initTime);
            // select component ROI
            CvInvoke.cvSetImageROI(_silh, motionRectangle);
            CvInvoke.cvSetImageROI(_mhi, motionRectangle);
            CvInvoke.cvSetImageROI(_orientation, motionRectangle);
            CvInvoke.cvSetImageROI(_mask, motionRectangle);

            // calculate orientation
            angle = CvInvoke.cvCalcGlobalOrientation(_orientation.Ptr, _mask.Ptr, _mhi.Ptr, ts.TotalSeconds, _mhiDuration);
            angle = 360.0 - angle; // adjust for images with top-left origin

            motionPixelCount = CvInvoke.cvNorm(_silh.Ptr, IntPtr.Zero, CvEnum.NORM_TYPE.CV_L1, IntPtr.Zero); // calculate number of points within silhouette ROI

            CvInvoke.cvResetImageROI(_mhi);
            CvInvoke.cvResetImageROI(_orientation);
            CvInvoke.cvResetImageROI(_mask);
            CvInvoke.cvResetImageROI(_silh);

        }

        /*
        public void MotionDirections(out MCvRect[] motionRects, out double[] angles, out double[] motionPixelCounts)
        {
            List<MCvRect> motionRectList = new List<MCvRect>();
            List<double> angleList = new List<double>();
            List<double> motionPixelCountList = new List<double>();
            Seq<MCvConnectedComp> motions = MotionComponents;
            Image<Gray, Byte> res = new Image<Gray, byte>(_mhi.Width, _mhi.Height);

            foreach (MCvConnectedComp component in motions)
            {
                double angle, count;
                MotionInfo(component.rect, out angle, out count);

                motionRectList.Add(component.rect);
                angleList.Add(angle);
                motionPixelCountList.Add(count);
            }

            motionRects = motionRectList.ToArray();
            angles = angleList.ToArray();
            motionPixelCounts = motionPixelCountList.ToArray();
        }*/
    }
}
