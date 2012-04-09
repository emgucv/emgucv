using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libtesseract_cube_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
