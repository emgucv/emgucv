Imports Emgu.CV
Imports Emgu.Util
Imports System.Windows.Forms

Module Module1

   Sub Main()
      Dim img As New Image(Of Bgr, Byte)("lena.jpg")
      Dim imgGray As New Image(Of Gray, Byte)(1,1)
      Dim objectToDetect As New HaarCascade("haarcascade_frontalface_alt2.xml")
      Dim objectDetected As New Rectangle(Of Double)

      imgGray = img.Convert(Of Gray, Byte)()

      for each rect as Rectangle(Of Double) in imgGray.DetectHaarCascade(objectToDetect)(0)
         img.Draw(rect, New Bgr(255, 255, 255), 1)
      Next

      Dim viewer = New UI.ImageViewer(img)
      System.Windows.Forms.Application.Run(viewer)

   End Sub

End Module
