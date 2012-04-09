using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libtesseract_ccutil_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
