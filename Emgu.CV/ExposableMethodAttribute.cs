using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    /// <summary>
    /// Attribute used by ImageBox to generate Operation Menu
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExposableMethodAttribute : System.Attribute
    {
        private bool _exposable;

        /// <summary>
        /// Get or Set the Exposable value, if true, this function will be displayed in Operation Menu of ImageBox
        /// </summary>
        public bool Exposable
        {
            get
            {
                return _exposable;
            }
            set
            {
                _exposable = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ExposableMethodAttribute()
        {
            _exposable = true;
        }
    }
}
