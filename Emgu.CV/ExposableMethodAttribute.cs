using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExposableMethodAttribute : System.Attribute
    {
        private bool _exposable;

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

        public ExposableMethodAttribute()
        {
            _exposable = true;
        }
    }
}
