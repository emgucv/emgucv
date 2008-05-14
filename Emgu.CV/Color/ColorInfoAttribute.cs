using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    /// <summary>
    /// Attributes used to specify color information
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class ColorInfoAttribute: System.Attribute
    {
        /// <summary>
        /// The code which is used for color conversion
        /// </summary>
        private String _conversionCodeName;

        /// <summary>
        /// The code which is used for color conversion
        /// </summary>
        public String ConversionCodeName
        {
            get { return _conversionCodeName; }
            set { _conversionCodeName = value; }
        }

        /// <summary>
        /// The code which is used for color conversion
        /// </summary>
        public ColorInfoAttribute()
        {
            _conversionCodeName = String.Empty;
        }
    }
}
