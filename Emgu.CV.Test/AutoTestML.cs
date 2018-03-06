//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.ML;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;
#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
#else
using NUnit.Framework;
#endif

using MlEnum = Emgu.CV.ML.MlEnum;

namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestML
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

            using (KNearest knn = new KNearest())
            {
                knn.DefaultK = K;
                knn.IsClassifier = true;
                knn.Train(trainData, MlEnum.DataLayoutType.RowSample, trainClasses);
                //ParamDef[] defs =  knn.GetParams();


                for (int i = 0; i < img.Height; i++)
                {
                    for (int j = 0; j < img.Width; j++)
                    {
                        sample.Data[0, 0] = j;
                        sample.Data[0, 1] = i;

                        // estimates the response and get the neighbors' labels
                        float response = knn.Predict(sample);
                            //knn.FindNearest(sample, K, results, null, neighborResponses, null);

                        int accuracy = 0;
                        // compute the number of neighbors representing the majority
                        for (int k = 0; k < K; k++)
                        {
                            if (neighborResponses.Data[0, k] == response)
                                accuracy++;
                        }
                        // highlight the pixel depending on the accuracy (or confidence)
                        img[i, j] =
                            response == 1
                                ? (accuracy > 5 ? new Bgr(90, 0, 0) : new Bgr(90, 40, 0))
                                : (accuracy > 5 ? new Bgr(0, 90, 0) : new Bgr(40, 90, 0));
                    }
                }

                String knnModelStr;
                //save stat model to string
                using (FileStorage fs = new FileStorage(".yml", FileStorage.Mode.Write | FileStorage.Mode.Memory))
                {
                    knn.Write(fs);

                    knnModelStr = fs.ReleaseAndGetString();
                }

                //load stat model from string
                using (FileStorage fs = new FileStorage(knnModelStr, FileStorage.Mode.Read | FileStorage.Mode.Memory))
                {
                    KNearest knn2 = new KNearest();
                    knn2.Read(fs.GetRoot());
                }

                String knnModelStr2 = knn.SaveToString();
                KNearest knn3 = new KNearest();
                knn3.LoadFromString(knnModelStr2);

#if !NETFX_CORE
                String fileName = "knnModel.xml";
                //String fileName = Path.Combine(Path.GetTempPath(), "svmModel.xml");
                knn.Save(fileName);
                String text = File.ReadAllText(fileName);
                
#endif
            }

            // display the original training samples
            for (int i = 0; i < (trainSampleCount >> 1); i++)
            {
                PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
                img.Draw(new CircleF(p1, 2.0f), new Bgr(255, 100, 100), -1);
                PointF p2 = new PointF(trainData2[i, 0], trainData2[i, 1]);
                img.Draw(new CircleF(p2, 2.0f), new Bgr(100, 255, 100), -1);
            }

            //Emgu.CV.UI.ImageViewer.Show(img);
        }

        /*
        [Test]
        public void TestEMLegacy()
        {
           int N = 4; //number of clusters
           int N1 = (int) Math.Sqrt((double) 4);

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

           using (EMLegacy emModel1 = new EMLegacy())
           using (EMLegacy emModel2 = new EMLegacy())
           {
              EMParams parameters1 = new EMParams();
              parameters1.Nclusters = N;
              parameters1.CovMatType = Emgu.CV.ML.MlEnum.EmCovarianMatrixType.COV_MAT_DIAGONAL;
              parameters1.StartStep = Emgu.CV.ML.MlEnum.EM_INIT_STEP_TYPE.START_AUTO_STEP;
              parameters1.TermCrit = new MCvTermCriteria(10, 0.01);
              emModel1.Train(samples, parameters1, labels);

              EMParams parameters2 = new EMParams();
              parameters2.Nclusters = N;
              parameters2.CovMatType = Emgu.CV.ML.MlEnum.EmCovarianMatrixType.COV_MAT_GENERIC;
              parameters2.StartStep = Emgu.CV.ML.MlEnum.EM_INIT_STEP_TYPE.START_E_STEP;
              parameters2.TermCrit = new MCvTermCriteria(100, 1.0e-6);
              parameters2.Means = emModel1.Means;
              parameters2.Covs = emModel1.GetCovariances();
              parameters2.Weights = emModel1.Weights;

              emModel2.Train(samples, parameters2, labels);

              //TODO: Find out when saving of EM model will be enable
              //emModel2.Save("emModel.xml");

              #region Classify every image pixel
              for (int i = 0; i < img.Height; i++)
                 for (int j = 0; j < img.Width; j++)
                 {
                    sample.Data[0, 0] = i;
                    sample.Data[0, 1] = j;
                    int response = (int) emModel2.Predict(sample, null);

                    Bgr color = colors[response];

                    img[j, i] = new Bgr(color.Blue * 0.5, color.Green * 0.5, color.Red * 0.5);
                 }
              #endregion 

              #region draw the clustered samples
              for (int i = 0; i < nSamples; i++)
              {
                 img.Draw(new CircleF(new PointF(samples.Data[i, 0], samples.Data[i, 1]), 1), colors[labels.Data[i, 0]], 0);
              }
              #endregion 
           }
        }*/


        [Test]
        public void TestEM2()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int numberOfPoints = 2000;
            int dimensions = 20;
            int numberOfClusters = 10;

            //using (
            //   EM.Params p = new EM.Params(numberOfClusters, MlEnum.EmCovarianMatrixType.Diagonal,
            //      new MCvTermCriteria(100, 1.0e-6)))
            using (EM em = new EM())
            {
                em.ClustersNumber = numberOfClusters;
                em.CovarianceMatrixType = EM.CovarianMatrixType.Diagonal;
                em.TermCriteria = new MCvTermCriteria(100, 1.0e-6);
                //ParamDef[] parameters = em.GetParams();
                Matrix<int> labels = new Matrix<int>(numberOfPoints, 1);
                Matrix<float> featuresM = new Matrix<float>(numberOfPoints, dimensions);
                for (int i = 0; i < numberOfPoints; i++)
                for (int j = 0; j < dimensions; j++)
                    featuresM[i, j] = 100 * (float) r.NextDouble() - 50;

                em.Train(featuresM, MlEnum.DataLayoutType.RowSample, labels);
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

            //using (SVM.Params p = new SVM.Params(MlEnum.SvmType.CSvc, MlEnum.SvmKernelType.Linear, 0, 1, 0, 1, 0, 0, null, new MCvTermCriteria(100, 1.0e-6)))
            using (SVM model = new SVM())
            using (Matrix<int> trainClassesInt = trainClasses.Convert<int>())
            using (TrainData td = new TrainData(trainData, MlEnum.DataLayoutType.RowSample, trainClassesInt))
            {
                model.Type = SVM.SvmType.CSvc;
                model.SetKernel(SVM.SvmKernelType.Inter);
                model.Degree = 0;
                model.Gamma = 1;
                model.Coef0 = 0;
                model.C = 1;
                model.Nu = 0;
                model.P = 0;
                model.TermCriteria = new MCvTermCriteria(100, 1.0e-6);
                //bool trained = model.TrainAuto(td, 5);
                model.Train(td);
#if !NETFX_CORE
                String fileName = "svmModel.xml";
                //String fileName = Path.Combine(Path.GetTempPath(), "svmModel.xml");
                model.Save(fileName);

                SVM model2 = new SVM();
                FileStorage fs = new FileStorage(fileName, FileStorage.Mode.Read);
                model2.Read(fs.GetFirstTopLevelNode());

                if (File.Exists(fileName))
                    File.Delete(fileName);
#endif

                for (int i = 0; i < img.Height; i++)
                {
                    for (int j = 0; j < img.Width; j++)
                    {
                        sample.Data[0, 0] = j;
                        sample.Data[0, 1] = i;

                        float response = model.Predict(sample);

                        img[i, j] =
                            response == 1
                                ? new Bgr(90, 0, 0)
                                : response == 2
                                    ? new Bgr(0, 90, 0)
                                    : new Bgr(0, 0, 90);
                    }
                }
                Mat supportVectors = model.GetSupportVectors();
                //TODO: find out how to draw the support vectors
                Image<Gray, float> pts = supportVectors.ToImage<Gray, float>();
                PointF[] vectors = new PointF[supportVectors.Rows];
                GCHandle handler = GCHandle.Alloc(vectors, GCHandleType.Pinned);
                using (
                    Mat vMat = new Mat(supportVectors.Rows, supportVectors.Cols, DepthType.Cv32F, 1,
                        handler.AddrOfPinnedObject(), supportVectors.Cols * 4))
                {
                    supportVectors.CopyTo(vMat);
                }
                handler.Free();


                Mat supportVec = model.GetSupportVectors();
                /*
                for (int i = 0; i < c; i++)
                {
                   float[] v = model.GetSupportVector(i);
                   PointF p1 = new PointF(v[0], v[1]);
                   img.Draw(new CircleF(p1, 4), new Bgr(128, 128, 128), 2);
                }*/
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

            //Emgu.CV.UI.ImageViewer.Show(img);
        }

        #endregion


        [Test]
        public void TestNormalBayesClassifier()
        {
            Bgr[] colors = new Bgr[]
            {
                new Bgr(0, 0, 255),
                new Bgr(0, 255, 0),
                new Bgr(255, 0, 0)
            };
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

            using (TrainData td = new TrainData(trainData, MlEnum.DataLayoutType.RowSample, trainClasses))
            using (NormalBayesClassifier classifier = new NormalBayesClassifier())
            {
                //ParamDef[] defs = classifier.GetParams();
                classifier.Train(trainData, MlEnum.DataLayoutType.RowSample, trainClasses);
                classifier.Clear();
                classifier.Train(td);
#if !NETFX_CORE
                String fileName = Path.Combine(Path.GetTempPath(), "normalBayes.xml");
                classifier.Save(fileName);
                if (File.Exists(fileName))
                    File.Delete(fileName);
#endif

                #region Classify every image pixel

                for (int i = 0; i < img.Height; i++)
                for (int j = 0; j < img.Width; j++)
                {
                    sample.Data[0, 0] = i;
                    sample.Data[0, 1] = j;
                    int response = (int) classifier.Predict(sample, null);

                    Bgr color = colors[response - 1];

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

            //Emgu.CV.UI.ImageViewer.Show(img);
        }

        /*
        [Test]
        public void TestBoost()
        {
           using (Boost tree = new Boost())
           {
              MCvBoostParams param = MCvBoostParams.GetDefaultParameter();
           }
        }

        [Test]
        public void TestGBTrees()
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

           using (GBTrees classifier = new GBTrees())
           {
              classifier.Train(trainData, MlEnum.DataLayoutType.RowSample, trainClasses.Convert<float>(), null, null, MCvGBTreesParams.GetDefaultParameter(), false);

  #if !NETFX_CORE
              String fileName = Path.Combine(Path.GetTempPath(), "GBTrees.xml");
              classifier.Save(fileName);
              if (File.Exists(fileName))
                 File.Delete(fileName);
  #endif

              #region Classify every image pixel
              for (int i = 0; i < img.Height; i++)
                 for (int j = 0; j < img.Width; j++)
                 {
                    sample.Data[0, 0] = i;
                    sample.Data[0, 1] = j;
                    int response = (int) Math.Round(classifier.Predict(sample, null, null, MCvSlice.WholeSeq, false));

                    Bgr color = colors[response - 1];

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

           //Emgu.CV.UI.ImageViewer.Show(img);
        }*/

        private static void ReadLetterRecognitionData(out Matrix<float> data, out Matrix<float> response)
        {
            string[] rows = EmguAssert.ReadAllLines("letter-recognition.data");

            int varCount = rows[0].Split(',').Length - 1;
            data = new Matrix<float>(rows.Length, varCount);
            response = new Matrix<float>(rows.Length, 1);
            int count = 0;
            foreach (string row in rows)
            {
                string[] values = row.Split(',');
                Char c = Convert.ToChar(values[0]);
                response[count, 0] = Convert.ToInt32(c);
                for (int i = 1; i < values.Length; i++)
                    data[count, i - 1] = Convert.ToSingle(values[i]);
                count++;
            }
        }

        /*
                private static void ReadMushroomData(out Matrix<float> data, out Matrix<float> response)
                {
                   string[] rows = EmguAssert.ReadAllLines("agaricus-lepiota.data");
        
                   int varCount = rows[0].Split(',').Length - 1;
                   data = new Matrix<float>(rows.Length, varCount);
                   response = new Matrix<float>(rows.Length, 1);
                   int count = 0;
                   foreach (string row in rows)
                   {
                      string[] values = row.Split(',');
                      Char c = Convert.ToChar(values[0]);
                      response[count, 0] = Convert.ToInt32(c);
                      for (int i = 1; i < values.Length; i++)
                         data[count, i - 1] = Convert.ToByte(Convert.ToChar(values[i]));
                      count++;
                   }
                }
        
                [Test]
                public void TestDTreesMushroom()
                {
                   Matrix<float> data, response;
                   ReadMushroomData(out data, out response);
        
                   //Use the first 80% of data as training sample
                   int trainingSampleCount = (int) (data.Rows * 0.8);
        
                   Matrix<Byte> varType = new Matrix<byte>(data.Cols + 1, 1);
                   varType.SetValue((byte) MlEnum.VarType.Categorical); //the data is categorical
        
                   Matrix<byte> sampleIdx = new Matrix<byte>(data.Rows, 1);
                   using (Matrix<byte> sampleRows = sampleIdx.GetRows(0, trainingSampleCount, 1))
                      sampleRows.SetValue(255);
        
                   float[] priors = new float[] {1, 0.5f};
                   GCHandle priorsHandle = GCHandle.Alloc(priors, GCHandleType.Pinned);
        
                   MCvDTreeParams param = new MCvDTreeParams();
                   param.maxDepth = 8;
                   param.minSampleCount = 10;
                   param.regressionAccuracy = 0;
                   param.useSurrogates = true;
                   param.maxCategories = 15;
                   param.cvFolds = 10;
                   param.use1seRule = true;
                   param.truncatePrunedTree = true;
                   param.priors = priorsHandle.AddrOfPinnedObject();
        
                   using (DTree dtree = new DTree())
                   {
                      bool success = dtree.Train(
                         data,
                         Emgu.CV.ML.MlEnum.DataLayoutType.RowSample,
                         response,
                         null,
                         sampleIdx,
                         varType,
                         null,
                         param);
        
                      if (!success)
                         return;
                      double trainDataCorrectRatio = 0;
                      double testDataCorrectRatio = 0;
                      for (int i = 0; i < data.Rows; i++)
                      {
                         using (Matrix<float> sample = data.GetRow(i))
                         {
                            double r = dtree.Predict(sample, null, false).value;
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
        
                      trainDataCorrectRatio /= trainingSampleCount;
                      testDataCorrectRatio /= (data.Rows - trainingSampleCount);
        
                      EmguAssert.WriteLine(String.Format("Prediction accuracy for training data :{0}%", trainDataCorrectRatio * 100));
                      EmguAssert.WriteLine(String.Format("Prediction accuracy for test data :{0}%", testDataCorrectRatio * 100));
                   }
        
                   priorsHandle.Free();
                }*/

        [Test]
        public void TestRTreesLetterRecognition()
        {
            Matrix<float> data, response;
            ReadLetterRecognitionData(out data, out response);

            int trainingSampleCount = (int) (data.Rows * 0.8);

            Matrix<Byte> varType = new Matrix<byte>(data.Cols + 1, 1);
            varType.SetValue((byte) MlEnum.VarType.Numerical); //the data is numerical
            varType[data.Cols, 0] = (byte) MlEnum.VarType.Categorical; //the response is catagorical

            Matrix<byte> sampleIdx = new Matrix<byte>(data.Rows, 1);
            using (Matrix<byte> sampleRows = sampleIdx.GetRows(0, trainingSampleCount, 1))
                sampleRows.SetValue(255);

            using (RTrees forest = new RTrees())
            using (
                TrainData td = new TrainData(data, MlEnum.DataLayoutType.RowSample, response, null, sampleIdx, null,
                    varType))
            {
                forest.MaxDepth = 10;
                forest.MinSampleCount = 10;
                forest.RegressionAccuracy = 0.0f;
                forest.UseSurrogates = false;
                forest.MaxCategories = 15;
                forest.CalculateVarImportance = true;
                forest.ActiveVarCount = 4;
                forest.TermCriteria = new MCvTermCriteria(100, 0.01f);
                bool success = forest.Train(td);

                if (!success)
                    return;

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

                trainDataCorrectRatio /= trainingSampleCount;
                testDataCorrectRatio /= (data.Rows - trainingSampleCount);

                StringBuilder builder = new StringBuilder("Variable Importance: ");
                /*
                using (Matrix<float> varImportance = forest.VarImportance)
                {
                   for (int i = 0; i < varImportance.Cols; i++)
                   {
                      builder.AppendFormat("{0} ", varImportance[0, i]);
                   }
                }*/

                EmguAssert.WriteLine(String.Format("Prediction accuracy for training data :{0}%",
                    trainDataCorrectRatio * 100));
                EmguAssert.WriteLine(String.Format("Prediction accuracy for test data :{0}%", testDataCorrectRatio * 100));
                EmguAssert.WriteLine(builder.ToString());
            }
        }

        /*
        [Test]
        public void TestERTreesLetterRecognition()
        {
           Matrix<float> data, response;
           ReadLetterRecognitionData(out data, out response);

           int trainingSampleCount = (int) (data.Rows * 0.8);

           Matrix<Byte> varType = new Matrix<byte>(data.Cols + 1, 1);
           varType.SetValue((byte) MlEnum.VarType.Numerical); //the data is numerical
           varType[data.Cols, 0] = (byte) MlEnum.VarType.Categorical; //the response is catagorical

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
           param.termCrit.Type = Emgu.CV.CvEnum.TermCritType.Iter;

           using (ERTrees forest = new ERTrees())
           {
              bool success = forest.Train(
                 data.GetRows(0, trainingSampleCount, 1),
                 Emgu.CV.ML.MlEnum.DataLayoutType.RowSample,
                 response.GetRows(0, trainingSampleCount, 1),
                 null,
                 null, 
                 varType,
                 null,
                 param);

              if (!success)
                 return;

  #if !NETFX_CORE
              String fileName = Path.Combine(Path.GetTempPath(), "ERTree.xml");
              forest.Save(fileName);
              if (File.Exists(fileName))
                 File.Delete(fileName);
  #endif

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

              trainDataCorrectRatio /= trainingSampleCount;
              testDataCorrectRatio /= (data.Rows - trainingSampleCount);

              EmguAssert.WriteLine(String.Format("Prediction accuracy for training data :{0}%", trainDataCorrectRatio * 100));
              EmguAssert.WriteLine(String.Format("Prediction accuracy for test data :{0}%", testDataCorrectRatio * 100));

           }
        }*/

#if !ANDROID
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

            using (Matrix<int> layerSize = new Matrix<int>(new int[] {2, 5, 1}))
            using (Mat layerSizeMat = layerSize.Mat)

            using (TrainData td = new TrainData(trainData, MlEnum.DataLayoutType.RowSample, trainClasses))
            using (ANN_MLP network = new ANN_MLP())
            {
                network.SetLayerSizes(layerSizeMat);
                network.SetActivationFunction(ANN_MLP.AnnMlpActivationFunction.SigmoidSym, 0, 0);
                network.TermCriteria = new MCvTermCriteria(10, 1.0e-8);
                network.SetTrainMethod(ANN_MLP.AnnMlpTrainMethod.Backprop, 0.1, 0.1);
                network.Train(td, (int) Emgu.CV.ML.MlEnum.AnnMlpTrainingFlag.Default);

#if !NETFX_CORE
                String fileName = Path.Combine(Path.GetTempPath(), "ann_mlp_model.xml");
                network.Save(fileName);
                if (File.Exists(fileName))
                    File.Delete(fileName);
#endif

                for (int i = 0; i < img.Height; i++)
                {
                    for (int j = 0; j < img.Width; j++)
                    {
                        sample.Data[0, 0] = j;
                        sample.Data[0, 1] = i;
                        network.Predict(sample, prediction);

                        // estimates the response and get the neighbors' labels
                        float response = prediction.Data[0, 0];

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
                PointF p2 = new PointF((int) trainData2[i, 0], (int) trainData2[i, 1]);
                img.Draw(new CircleF(p2, 2), new Bgr(100, 255, 100), -1);
            }

            //Emgu.CV.UI.ImageViewer.Show(img);
        }
#endif


        [Test]
        public void TestKMeans()
        {
            int clustersCount = 5;
            int sampleCount = 300;
            int imageSize = 500;

            Bgr[] colors = new Bgr[]
            {
                new Bgr(0, 0, 255),
                new Bgr(0, 255, 0),
                new Bgr(255, 100, 100),
                new Bgr(255, 0, 255),
                new Bgr(0, 255, 255)
            };

            Image<Bgr, Byte> image = new Image<Bgr, byte>(imageSize, imageSize);

            #region generate random samples

            Matrix<float> points = new Matrix<float>(sampleCount, 1, 2);

            Matrix<int> clusters = new Matrix<int>(sampleCount, 1);
            Random r = new Random();
            for (int i = 0; i < clustersCount; i++)
            {
                Matrix<float> row = points.GetRows(i * (sampleCount / clustersCount),
                    (i + 1) * (sampleCount / clustersCount), 1);
                row.SetRandNormal(new MCvScalar(r.Next() % imageSize, r.Next() % imageSize),
                    new MCvScalar((r.Next() % imageSize) / 6, (r.Next() % imageSize) / 6));
            }
            using (ScalarArray ia = new ScalarArray(new MCvScalar()))
            {
                CvInvoke.AbsDiff(points, ia, points);
            }
            CvInvoke.RandShuffle(points, 1.0, 0);

            #endregion

            CvInvoke.Kmeans(
                points,
                2,
                clusters,
                new MCvTermCriteria(10, 1.0),
                5,
                CvEnum.KMeansInitType.PPCenters);

            for (int i = 0; i < sampleCount; i++)
            {
                PointF p = new PointF(points.Data[i, 0], points.Data[i, 1]);
                image.Draw(new CircleF(p, 1.0f), colors[clusters[i, 0]], 1);
            }

            //Emgu.CV.UI.ImageViewer.Show(image);
        }

        [Test]
        public void TestKMeans2()
        {
            int clustersCount = 2;
            int sampleCount = 300;
            int maxVal = 500;

            Random r = new Random();
            float[] values = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                values[i] = (float) (r.NextDouble() * maxVal);
            }
            using (VectorOfInt labels = new VectorOfInt())
            using (VectorOfFloat vd = new VectorOfFloat(values))
            {
                CvInvoke.Kmeans(vd, 2, labels, new MCvTermCriteria(1000, 0.00001), 2, KMeansInitType.RandomCenters);
            }
        }
    }
}
