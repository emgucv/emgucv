using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libtesseract_textord_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
