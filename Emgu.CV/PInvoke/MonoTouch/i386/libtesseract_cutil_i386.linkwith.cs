using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libtesseract_cutil_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
