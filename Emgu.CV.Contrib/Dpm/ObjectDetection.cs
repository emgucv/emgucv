using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Emgu.CV.Dpm
{
    /// <summary>
    /// A DPM detection
    /// </summary>
    public struct ObjectDetection
    {
        /// <summary>
        /// rectangle
        /// </summary>
        public readonly Rectangle Rect;
        /// <summary>
        /// detection score
        /// </summary>
        public readonly float Score;
        /// <summary>
        /// class of the detection
        /// </summary>
        public readonly int ClassId;

        /// <summary>
        /// create a detection
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="score"></param>
        /// <param name="classId"></param>
        public ObjectDetection(Rectangle rect, float score, int classId)
        {
            Rect = rect;
            Score = score;
            ClassId = classId;
        }
    }
}
