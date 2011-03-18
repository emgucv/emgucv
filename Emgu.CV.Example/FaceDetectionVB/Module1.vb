'
'  Copyright (C) 2004-2011 by EMGU. All rights reserved.
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
      Dim objectToDetect As New HaarCascade("haarcascade_frontalface_alt2.xml")

      'Convert the image to Grayscale
      Dim imgGray As Image(Of Gray, Byte) = img.Convert(Of Gray, Byte)()

      For Each face As MCvAvgComp In imgGray.DetectHaarCascade( _
         objectToDetect, _
         1.1, _
         50, _
         CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, _
         New Size(20, 20))(0)
         img.Draw(face.rect, New Bgr(Color.White), 1)
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
