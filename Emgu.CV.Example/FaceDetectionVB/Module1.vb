'
'  Copyright (C) 2004-2012 by EMGU. All rights reserved.
'

Imports Emgu.CV
Imports Emgu.CV.Structure
Imports Emgu.Util
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Runtime.InteropServices

Module Module1

   Sub Main()

      If Not IsPlaformCompatable() Then
         Return
      End If

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

   ' Check if both the managed and unmanaged code are compiled for the same architecture
   Public Function IsPlaformCompatable() As Boolean
      Dim clrBitness As Integer = Marshal.SizeOf(GetType(IntPtr)) * 8
      If clrBitness <> CvInvoke.UnmanagedCodeBitness Then
         MessageBox.Show(String.Format("Platform mismatched: CLR is {0} bit, C++ code is {1} bit." _
            + "Please consider recompiling the executable with the same platform target as C++ code.", _
            clrBitness, CvInvoke.UnmanagedCodeBitness))
         Return False
      End If
      Return True
   End Function


End Module
