using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libtesseract_neural_networks_i386.a", LinkTarget.Simulator, ForceLoad = true, IsCxx = true)]
