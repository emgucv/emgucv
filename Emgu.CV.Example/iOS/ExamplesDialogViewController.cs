//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using MonoTouch.Dialog;

namespace Example.iOS
{
   public class ExamplesDialogViewController : DialogViewController
   {
      public ExamplesDialogViewController ()
       : base (new RootElement ("Examples"))
      {
      }

      public override void ViewDidLoad ()
      {
         base.ViewDidLoad ();
         RootElement root = Root;


         Section examplesSection = new Section ();

         StyledStringElement helloWorldElement = new StyledStringElement ("Hello world");
         helloWorldElement.Tapped += () => {
            NavigationController.PushViewController (
                new HelloWorldUIViewController (),
                true
            );
         };
         examplesSection.Add (helloWorldElement);

         StyledStringElement planarSubdivisionElement = new StyledStringElement ("Planar Subdivision");
         planarSubdivisionElement.Tapped += () => {
            NavigationController.PushViewController (
                new PlanarSubdivisionDialogViewController (),
                true
            );
         };
         examplesSection.Add (planarSubdivisionElement);

         StyledStringElement faceDetectionElement = new StyledStringElement ("Face Detection");
         faceDetectionElement.Tapped += () => {
            NavigationController.PushViewController (
           new FaceDetectionDialogViewController (),
                true
            );
         };
         examplesSection.Add (faceDetectionElement);

         StyledStringElement featureMatchingElement = new StyledStringElement ("Feature Matching");
         featureMatchingElement.Tapped += () => {
            NavigationController.PushViewController (
                new FeatureMatchingDialogViewController (),
                true
            );
         };
         examplesSection.Add (featureMatchingElement);

         StyledStringElement pedestrianDetectionElement = new StyledStringElement ("Pedestrian Detection");
         pedestrianDetectionElement.Tapped += () => {
            NavigationController.PushViewController (
                new PedestrianDetectionDialogViewController (),
                true
            );
         };
         examplesSection.Add (pedestrianDetectionElement);


         StyledStringElement cameraElement = new StyledStringElement ("Camera");
         cameraElement.Tapped += () => {
            NavigationController.PushViewController (
               new CameraDialogViewController (),
               true);

         };
         examplesSection.Add (cameraElement);

         root.Add (examplesSection);
      }
   }
}

