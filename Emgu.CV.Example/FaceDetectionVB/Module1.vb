'
'  Copyright (C) 2004-2013 by EMGU. All rights reserved.
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
      Dim img As New Image(Of Bgr, Byte)("lena.jpg")

      'Load the object detector
      Dim faceDetector As New CascadeClassifier("haarcascade_frontalface_default.xml")

      'Convert the image to Grayscale
      Dim imgGray As Image(Of Gray, Byte) = img.Convert(Of Gray, Byte)()

      For Each face As Rectangle In faceDetector.DetectMultiScale( _
                         imgGray, _
                         1.1, _
                         10, _
                         New Size(20, 20), _
                         Size.Empty)
         img.Draw(face, New Bgr(Color.White), 1)
      Next

      'Show the image
      UI.ImageViewer.Show(img)

   End Sub

End Module
