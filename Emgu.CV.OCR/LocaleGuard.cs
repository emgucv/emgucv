//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.OCR
{
    /// <summary>
    /// This class set the locale to specific values and revert it back to the old values when the object is disposed.
    /// </summary>
    public class LocaleGuard : DisposableObject
    {
        /// <summary>
        /// The locale category
        /// </summary>
        public enum LocaleCategory
        {
            /// <summary>
            /// All
            /// </summary>
            All = 0,
            /// <summary>
            /// Collate
            /// </summary>
            Collate = 1,
            /// <summary>
            /// Ctype
            /// </summary>
            Ctype = 2,
            /// <summary>
            /// Monetary
            /// </summary>
            Monetary = 3,
            /// <summary>
            /// Numeric
            /// </summary>
            Numeric = 4,
            /// <summary>
            /// Time
            /// </summary>
            Time = 5
        }

        private LocaleCategory _category;
        private String _locale;
        private String _oldLocale;
        
        /// <summary>
        /// Create a locale guard to set the locale to specific value. Will revert locale back to previous value when the object is disposed.
        /// </summary>
        /// <param name="category">The locale category</param>
        /// <param name="locale">The locale</param>
        public LocaleGuard(LocaleCategory category, String locale)
        {
            _category = category;
            _locale = locale;
            _oldLocale = OcrInvoke.SetLocale(_category, null);
            if (locale != _oldLocale)
                OcrInvoke.SetLocale(_category, _locale);
        }

        /// <summary>
        /// Revert back to the old locale
        /// </summary>
        protected override void DisposeObject()
        {
            if (_oldLocale != _locale)
                OcrInvoke.SetLocale(_category, _oldLocale);
        }
    }


    public static partial class OcrInvoke
    {
        /// <summary>
        /// The setlocale function installs the specified system locale or its portion as the new C locale. The modifications remain in effect and influences the execution of all locale-sensitive C library functions until the next call to setlocale. If locale is a null pointer, setlocale queries the current C locale without modifying it.
        /// </summary>
        /// <param name="category">Locale category identifier</param>
        /// <param name="locale">System-specific locale identifier. Can be "" for the user-preferred locale or "C" for the minimal locale</param>
        /// <returns>String identifying the C locale after applying the changes, if any, or null pointer on failure. A copy of the returned string along with the category used in this call to std::setlocale may be used later in the program to restore the locale back to the state at the end of this call.</returns>
        public static String SetLocale(LocaleGuard.LocaleCategory category, String locale = null)
        {
            IntPtr oldLocalePtr;
            if (locale == null)
            {
                oldLocalePtr = stdSetlocale(category, IntPtr.Zero);
                return oldLocalePtr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(oldLocalePtr);
            }
            IntPtr localePtr = Marshal.StringToHGlobalAnsi(locale);
            try
            {
                oldLocalePtr = stdSetlocale(category, localePtr);
            }
            finally
            {
                Marshal.FreeHGlobal(localePtr);
            }
            return oldLocalePtr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(oldLocalePtr);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr stdSetlocale(
            LocaleGuard.LocaleCategory category,
            IntPtr locale);
    }
}

