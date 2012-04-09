using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libtesseract_api_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
