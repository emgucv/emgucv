'
'  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
'

Imports Emgu.CV
Imports Emgu.CV.Structure
Imports Emgu.Util
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Runtime.InteropServices

Module Module1

   Sub Main()

      'Load the image from file
      Dim img As Mat
      img = CvInvoke.Imread("lena.jpg", CvEnum.LoadImageType.Color)

      'Load the object detector
      Dim faceDetector As New CascadeClassifier("haarcascade_frontalface_default.xml")

      'Convert the image to Grayscale
      Dim imgGray As New UMat()
      CvInvoke.CvtColor(img, imgGray, CvEnum.ColorConversion.Bgr2Gray)

      For Each face As Rectangle In faceDetector.DetectMultiScale( _
                         imgGray, _
                         1.1, _
                         10, _
                         New Size(20, 20), _
                         Size.Empty)
         CvInvoke.Rectangle(img, face, New MCvScalar(255, 255, 255))
      Next

      'Show the image
      UI.ImageViewer.Show(img)

   End Sub

End Module
