using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libtesseract_ccmain_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
