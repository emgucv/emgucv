//#define SHOW_IMAGE
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using NUnit.Framework;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;

namespace Emgu.CV.ML.UnitTest
{
   [TestFixture]
   public class UnitTests
   {
      [Test]
      public void TestKNearest()
      {
         int K = 10;
         int trainSampleCount = 100;

         #region Generate the training data and classes

         Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
         Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 1);

         Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

         Matrix<float> sample = new Matrix<float>(1, 2);

         Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount >> 1, 1);
         trainData1.SetRandNormal(new MCvScalar(200), new MCvScalar(50));
         Matrix<float> trainData2 = trainData.GetRows(trainSampleCount >> 1, trainSampleCount, 1);
         trainData2.SetRandNormal(new MCvScalar(300), new MCvScalar(50));

         Matrix<float> trainClasses1 = trainClasses.GetRows(0, trainSampleCount >> 1, 1);
         trainClasses1.SetValue(1);
         Matrix<float> trainClasses2 = trainClasses.GetRows(trainSampleCount >> 1, trainSampleCount, 1);
         trainClasses2.SetValue(2);
         #endregion
         
         Matrix<float> results, neighborResponses;
         results = new Matrix<float>(sample.Rows, 1);
         neighborResponses = new Matrix<float>(sample.Rows, K);
         //dist = new Matrix<float>(sample.Rows, K);

         using (KNearest knn = new KNearest(trainData, trainClasses, null, false, K))
         {
            for (int i = 0; i < img.Height; i++)
            {
               for (int j = 0; j < img.Width; j++)
               {
                  sample.Data[0, 0] = j;
                  sample.Data[0, 1] = i;

                  //Matrix<float> nearestNeighbors = new Matrix<float>(K* sample.Rows, sample.Cols);
                  // estimates the response and get the neighbors' labels
                  float response = knn.FindNearest(sample, K, results, null, neighborResponses, null);

                  int accuracy = 0;
                  // compute the number of neighbors representing the majority
                  for (int k = 0; k < K; k++)
                  {
                     if (neighborResponses.Data[0, k] == response)
                        accuracy++;
                  }
                  // highlight the pixel depending on the accuracy (or confidence)
                  img[i, j] = 
                     response == 1 ?
                        (accuracy > 5 ? new Bgr(90, 0, 0) : new Bgr(90, 40, 0)) :
                        (accuracy > 5 ? new Bgr(0, 90, 0) : new Bgr(40, 90, 0));
               }
            }
         }

         // display the original training samples
         for (int i = 0; i < (trainSampleCount >> 1); i++)
         {
            PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
            img.Draw(new CircleF(p1, 2.0f), new Bgr(255, 100, 100), -1);
            PointF p2 = new PointF(trainData2[i, 0], trainData2[i, 1]);
            img.Draw(new CircleF(p2, 2.0f), new Bgr(100, 255, 100), -1);
         }

#if SHOW_IMAGE
         Emgu.CV.UI.ImageViewer.Show(img);
#endif
      }

      [Test]
      public void TestEM()
      {
         int N = 4; //number of clusters
         int N1 = (int)Math.Sqrt((double)4);

         Bgr[] colors = new Bgr[] { 
            new Bgr(0, 0, 255), 
            new Bgr(0, 255, 0),
            new Bgr(0, 255, 255),
            new Bgr(255, 255, 0)};

         int nSamples = 100;

         Matrix<float> samples = new Matrix<float>(nSamples, 2);
         Matrix<Int32> labels = new Matrix<int>(nSamples, 1);
         Image<Bgr, Byte> img = new Image<Bgr,byte>(500, 500);
         Matrix<float> sample = new Matrix<float>(1, 2);

         CvInvoke.cvReshape(samples.Ptr, samples.Ptr, 2, 0);
         for (int i = 0; i < N; i++)
         {
            Matrix<float> rows = samples.GetRows(i * nSamples / N, (i + 1) * nSamples / N, 1);
            double scale = ((i % N1) + 1.0) / (N1 + 1);
            MCvScalar mean = new MCvScalar(scale * img.Width, scale * img.Height);
            MCvScalar sigma = new MCvScalar(30, 30);
            rows.SetRandNormal(mean, sigma);
         }
         CvInvoke.cvReshape(samples.Ptr, samples.Ptr, 1, 0);

         using (EM emModel1 = new EM())
         using (EM emModel2 = new EM())
         {
            EMParams parameters1 = new EMParams();
            parameters1.Nclusters = N;
            parameters1.CovMatType = Emgu.CV.ML.MlEnum.EM_COVARIAN_MATRIX_TYPE.COV_MAT_DIAGONAL;
            parameters1.StartStep = Emgu.CV.ML.MlEnum.EM_INIT_STEP_TYPE.START_AUTO_STEP;
            parameters1.TermCrit = new MCvTermCriteria(10, 0.01);
            emModel1.Train(samples, null, parameters1, labels);

            EMParams parameters2 = new EMParams();
            parameters2.Nclusters = N;
            parameters2.CovMatType = Emgu.CV.ML.MlEnum.EM_COVARIAN_MATRIX_TYPE.COV_MAT_GENERIC;
            parameters2.StartStep = Emgu.CV.ML.MlEnum.EM_INIT_STEP_TYPE.START_E_STEP;
            parameters2.TermCrit = new MCvTermCriteria(100, 1.0e-6);
            parameters2.Means = emModel1.GetMeans();
            parameters2.Covs = emModel1.GetCovariances();
            parameters2.Weights = emModel1.GetWeights();
                        
            emModel2.Train(samples, null, parameters2, labels);
            
            #region Classify every image pixel
            for (int i = 0; i < img.Height; i++)
               for (int j = 0; j < img.Width; j++)
               {
                  sample.Data[0, 0] = i;
                  sample.Data[0, 1] = j;
                  int response = (int) emModel2.Predict(sample, null);

                  Bgr color = colors[response];
                  
                  img[j, i] = new Bgr(color.Blue*0.5, color.Green * 0.5, color.Red * 0.5 );
               }
            #endregion 

            #region draw the clustered samples
            for (int i = 0; i < nSamples; i++)
            {
               img.Draw(new CircleF(new PointF(samples.Data[i, 0], samples.Data[i, 1]), 1), colors[labels.Data[i, 0]], 0);
            }
            #endregion 

#if SHOW_IMAGE
         Emgu.CV.UI.ImageViewer.Show(img);
#endif
         }
      }

      #region contribution from Albert G
      [Test]
      public void TestSVM()
      {
         int trainSampleCount = 150;
         int sigma = 60;

         #region Generate the training data and classes

         Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
         Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 1);

         Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

         Matrix<float> sample = new Matrix<float>(1, 2);

         Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount / 3, 1);
         trainData1.GetCols(0, 1).SetRandNormal(new MCvScalar(100), new MCvScalar(sigma));
         trainData1.GetCols(1, 2).SetRandNormal(new MCvScalar(300), new MCvScalar(sigma));

         Matrix<float> trainData2 = trainData.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
         trainData2.SetRandNormal(new MCvScalar(400), new MCvScalar(sigma));

         Matrix<float> trainData3 = trainData.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
         trainData3.GetCols(0, 1).SetRandNormal(new MCvScalar(300), new MCvScalar(sigma));
         trainData3.GetCols(1, 2).SetRandNormal(new MCvScalar(100), new MCvScalar(sigma));

         Matrix<float> trainClasses1 = trainClasses.GetRows(0, trainSampleCount / 3, 1);
         trainClasses1.SetValue(1);
         Matrix<float> trainClasses2 = trainClasses.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
         trainClasses2.SetValue(2);
         Matrix<float> trainClasses3 = trainClasses.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
         trainClasses3.SetValue(3);

         #endregion

         using (SVM model = new SVM())
         {
            SVMParams p = new SVMParams();
            p.KernelType = Emgu.CV.ML.MlEnum.SVM_KERNEL_TYPE.LINEAR;
            p.SVMType = Emgu.CV.ML.MlEnum.SVM_TYPE.C_SVC;
            p.C = 1;
            p.TermCrit = new MCvTermCriteria(100, 0.00001);

            //bool trained = model.Train(trainData, trainClasses, null, null, p);
            bool trained = model.TrainAuto(trainData, trainClasses, null, null, p.MCvSVMParams, 5);
            
            for (int i = 0; i < img.Height; i++)
            {
               for (int j = 0; j < img.Width; j++)
               {
                  sample.Data[0, 0] = j;
                  sample.Data[0, 1] = i;

                  float response = model.Predict(sample);

                  img[i, j] =
                     response == 1 ? new Bgr(90, 0, 0) :
                     response == 2 ? new Bgr(0, 90, 0) :
                     new Bgr(0, 0, 90);
               }
            }

            int c = model.GetSupportVectorCount();
            for (int i = 0; i < c; i++)
            {
               float[] v = model.GetSupportVector(i);
               PointF p1 = new PointF(v[0], v[1]);
               img.Draw(new CircleF(p1, 4), new Bgr(128, 128, 128), 2);
            }
         }

         // display the original training samples
         for (int i = 0; i < (trainSampleCount / 3); i++)
         {
            PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
            img.Draw(new CircleF(p1, 2.0f), new Bgr(255, 100, 100), -1);
            PointF p2 = new PointF(trainData2[i, 0], trainData2[i, 1]);
            img.Draw(new CircleF(p2, 2.0f), new Bgr(100, 255, 100), -1);
            PointF p3 = new PointF(trainData3[i, 0], trainData3[i, 1]);
            img.Draw(new CircleF(p3, 2.0f), new Bgr(100, 100, 255), -1);
         }

#if SHOW_IMAGE
         Emgu.CV.UI.ImageViewer.Show(img);
#endif
      }
      #endregion

      [Test]
      public void TestNormalBayesClassifier()
      {
         Bgr[] colors = new Bgr[] { 
            new Bgr(0, 0, 255), 
            new Bgr(0, 255, 0),
            new Bgr(255, 0, 0)};
         int trainSampleCount = 150;

         #region Generate the training data and classes
         Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
         Matrix<int> trainClasses = new Matrix<int>(trainSampleCount, 1);

         Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

         Matrix<float> sample = new Matrix<float>(1, 2);

         Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount / 3, 1);
         trainData1.GetCols(0, 1).SetRandNormal(new MCvScalar(100), new MCvScalar(50));
         trainData1.GetCols(1, 2).SetRandNormal(new MCvScalar(300), new MCvScalar(50));

         Matrix<float> trainData2 = trainData.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
         trainData2.SetRandNormal(new MCvScalar(400), new MCvScalar(50));

         Matrix<float> trainData3 = trainData.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
         trainData3.GetCols(0, 1).SetRandNormal(new MCvScalar(300), new MCvScalar(50));
         trainData3.GetCols(1, 2).SetRandNormal(new MCvScalar(100), new MCvScalar(50));

         Matrix<int> trainClasses1 = trainClasses.GetRows(0, trainSampleCount / 3, 1);
         trainClasses1.SetValue(1);
         Matrix<int> trainClasses2 = trainClasses.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
         trainClasses2.SetValue(2);
         Matrix<int> trainClasses3 = trainClasses.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
         trainClasses3.SetValue(3);
         #endregion

         using (NormalBayesClassifier classifier = new NormalBayesClassifier() )
         {
            classifier.Train(trainData, trainClasses, null, null, false);
            #region Classify every image pixel
            for (int i = 0; i < img.Height; i++)
               for (int j = 0; j < img.Width; j++)
               {
                  sample.Data[0, 0] = i;
                  sample.Data[0, 1] = j;
                  int response = (int) classifier.Predict(sample, null);

                  Bgr color = colors[response -1];

                  img[j, i] = new Bgr(color.Blue * 0.5, color.Green * 0.5, color.Red * 0.5);
               }
            #endregion 
         }

         // display the original training samples
         for (int i = 0; i < (trainSampleCount / 3); i++)
         {
            PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
            img.Draw(new CircleF(p1, 2.0f), colors[0], -1);
            PointF p2 = new PointF(trainData2[i, 0], trainData2[i, 1]);
            img.Draw(new CircleF(p2, 2.0f), colors[1], -1);
            PointF p3 = new PointF(trainData3[i, 0], trainData3[i, 1]);
            img.Draw(new CircleF(p3, 2.0f), colors[2], -1);
         }

#if SHOW_IMAGE
         Emgu.CV.UI.ImageViewer.Show(img);
#endif
      }

      [Test]
      public void TestDTree()
      {
         using (DTree tree = new DTree())
         {
            MCvDTreeParams param = MCvDTreeParams.GetDefaultParameter();
         }
      }

      [Test]
      public void TestBoost()
      {
         using (Boost tree = new Boost())
         {
            MCvBoostParams param = MCvBoostParams.GetDefaultParameter();
         }
      }

      private void ReadLetterRecognitionData(out Matrix<float> data, out Matrix<float> response)
      {
         string[] rows = System.IO.File.ReadAllLines("letter-recognition.data");
         
         int varCount = rows[0].Split(',').Length - 1;
         data = new Matrix<float>(rows.Length, varCount);
         response = new Matrix<float>(rows.Length, 1);
         int count = 0;
         foreach ( string row in rows )
         {
            string[] values = row.Split(',');
            Char c = System.Convert.ToChar(values[0]);
            response[count, 0] = System.Convert.ToInt32(c);
            for (int i = 1; i < values.Length; i++)
               data[count, i - 1] = System.Convert.ToSingle(values[i]);
            count++;
         }
      }

      //TODO: Check why the following code do not work correctly
      [Test]
      public void TestRTrees()
      {
         Matrix<float> data, response;
         ReadLetterRecognitionData(out data, out response);

         int trainingSampleCount = (int)(data.Rows * 0.8);

         Matrix<Byte> varType = new Matrix<byte>(data.Cols + 1, 1);
         varType.SetValue((byte)MlEnum.VAR_TYPE.NUMERICAL); //the data is numerical
         varType[data.Cols, 0] = (byte) MlEnum.VAR_TYPE.CATEGORICAL; //the response is catagorical

         Matrix<byte> sampleIdx = new Matrix<byte>(data.Rows, 1);
         using (Matrix<byte> sampleRows = sampleIdx.GetRows(0, trainingSampleCount, 1))
         {
            sampleRows.SetValue(255);
         }

         MCvRTParams param = new MCvRTParams();
         param.maxDepth = 10;
         param.minSampleCount = 10;
         param.regressionAccuracy = 0.0f;
         param.useSurrogates = false;
         param.maxCategories = 15;
         param.priors = IntPtr.Zero;
         param.calcVarImportance = true;
         param.nactiveVars = 4;
         param.termCrit = new MCvTermCriteria(100, 0.01f);
         param.termCrit.type = Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_ITER;

         using (RTrees forest = new RTrees())
         {
            bool success = forest.Train(data, Emgu.CV.ML.MlEnum.DATA_LAYOUT_TYPE.ROW_SAMPLE,
                           response, null, sampleIdx, varType, null, param);

            if (!success) return;
            //forest.Train(data, Emgu.CV.ML.MlEnum.DATA_LAYOUT_TYPE.ROW_SAMPLE, response, null, null, varType, null, param);
            double trainDataCorrectRatio = 0;
            double testDataCorrectRatio = 0;
            for (int i = 0; i < data.Rows; i++)
            {
               using (Matrix<float> sample = data.GetRow(i))
               {
                  double r = forest.Predict(sample, null);
                  r = Math.Abs(r - response[i, 0]);
                  if (r < 1.0e-5)
                  {
                     if (i < trainingSampleCount)
                        trainDataCorrectRatio++;
                     else
                        testDataCorrectRatio++;
                  }
               }
            }

            Matrix<float> varImportance = forest.GetVarImportance();
            trainDataCorrectRatio /= trainingSampleCount;
            testDataCorrectRatio /= (data.Rows - trainingSampleCount);
         }
      }

      [Test]
      public void TestANN_MLP()
      {
         int trainSampleCount = 100;

         #region Generate the traning data and classes
         Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
         Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 1);

         Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

         Matrix<float> sample = new Matrix<float>(1, 2);
         Matrix<float> prediction = new Matrix<float>(1, 1);

         Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount >> 1, 1);
         trainData1.SetRandNormal(new MCvScalar(200), new MCvScalar(50));
         Matrix<float> trainData2 = trainData.GetRows(trainSampleCount >> 1, trainSampleCount, 1);
         trainData2.SetRandNormal(new MCvScalar(300), new MCvScalar(50));

         Matrix<float> trainClasses1 = trainClasses.GetRows(0, trainSampleCount >> 1, 1);
         trainClasses1.SetValue(1);
         Matrix<float> trainClasses2 = trainClasses.GetRows(trainSampleCount >> 1, trainSampleCount, 1);
         trainClasses2.SetValue(2);
         #endregion

         Matrix<int> layerSize = new Matrix<int>(new int[] { 2, 5, 1 });

         MCvANN_MLP_TrainParams parameters = new MCvANN_MLP_TrainParams();
         parameters.term_crit = new MCvTermCriteria(10, 1.0e-8);
         parameters.train_method = Emgu.CV.ML.MlEnum.ANN_MLP_TRAIN_METHOD.BACKPROP;
         parameters.bp_dw_scale = 0.1;
         parameters.bp_moment_scale = 0.1;

         using (ANN_MLP network = new ANN_MLP(layerSize, Emgu.CV.ML.MlEnum.ANN_MLP_ACTIVATION_FUNCTION.SIGMOID_SYM, 1.0, 1.0))
         {
            network.Train(trainData, trainClasses, null, null, parameters, Emgu.CV.ML.MlEnum.ANN_MLP_TRAINING_FLAG.DEFAULT);

            for (int i = 0; i < img.Height; i++)
            {
               for (int j = 0; j < img.Width; j++)
               {
                  sample.Data[0, 0] = j;
                  sample.Data[0, 1] = i;
                  network.Predict(sample, prediction);

                  // estimates the response and get the neighbors' labels
                  float response = prediction.Data[0,0];

                  // highlight the pixel depending on the accuracy (or confidence)
                  img[i, j] = response < 1.5 ? new Bgr(90, 0, 0) : new Bgr(0, 90, 0);
               }
            }
         }

         // display the original training samples
         for (int i = 0; i < (trainSampleCount >> 1); i++)
         {
            PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
            img.Draw(new CircleF(p1, 2), new Bgr(255, 100, 100), -1);
            PointF p2 = new PointF((int)trainData2[i, 0], (int)trainData2[i, 1]);
            img.Draw(new CircleF(p2, 2), new Bgr(100, 255, 100), -1);
         }
#if SHOW_IMAGE
         Emgu.CV.UI.ImageViewer.Show(img);
#endif
      }
   }
}
