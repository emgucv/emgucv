Imports Emgu.CV
Imports Emgu.CV.Structure
Imports Emgu.Util
Imports System.Windows.Forms
Imports System.Drawing


Module Module1

   Sub Main()
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

End Module
