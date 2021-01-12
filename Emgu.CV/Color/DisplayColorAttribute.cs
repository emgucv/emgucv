//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL || UNITY_STANDALONE || UNITY_WSA || UNITY_EDITOR
using Color = UnityEngine.Color;
#else
using Color = System.Drawing.Color;
#endif

namespace Emgu.CV
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class DisplayColorAttribute : Attribute
    {
        /// <summary>
        /// The display color
        /// </summary>
        /// <param name="blue">blue</param>
        /// <param name="green">green</param>
        /// <param name="red">red</param>
        public DisplayColorAttribute(int blue, int green, int red)
        {
#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL || UNITY_STANDALONE || UNITY_WSA || UNITY_EDITOR
            _displayColor = new Color(red/255.0f, green/255.0f, blue/255.0f, 1.0f);
#else
            _displayColor = Color.FromArgb(red, green, blue);
#endif
        }


        private Color _displayColor;

        /// <summary>
        /// Get or set the display color
        /// </summary>
        public Color DisplayColor
        {
            get { return _displayColor; }
            set { _displayColor = value; }
        }
    }
}
