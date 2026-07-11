//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;  
using System.Runtime.Serialization;

using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.Flann;
using Emgu.CV.Shape;
using Emgu.CV.Stitching;
using Emgu.CV.Text;
using Emgu.CV.Structure;
using Emgu.CV.Bioinspired;
using Emgu.CV.Dpm;
using Emgu.CV.ImgHash;
using Emgu.CV.Face;
using Emgu.CV.Freetype;

using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.Mcc;
using Emgu.CV.Models;
using Emgu.CV.Tiff;
//using Emgu.CV.WinForms;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
using Emgu.CV.XFeatures2D;
using Emgu.CV.XImgproc;
using Emgu.Util;

using System.Threading.Tasks;

//using Newtonsoft.Json;
using DetectorParameters = Emgu.CV.Aruco.DetectorParameters;
using DistType = Emgu.CV.CvEnum.DistType;
#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
#else
using Emgu.CV.ML;
using NUnit.Framework;
#endif


namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestModels
    {
        public static void DownloadManager_OnDownloadProgressChanged(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            if (totalBytesToReceive != null) 
                Trace.WriteLine(String.Format("{0} bytes downloaded.", bytesReceived));
            else
                Trace.WriteLine(String.Format("{0} of {1} bytes downloaded ({2}%)", bytesReceived, totalBytesToReceive, progressPercentage));
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestWeChatQRCode()
        {
            using (Mat m = EmguAssert.LoadMat("link_github_ocv.jpg"))
            using (Emgu.CV.Models.WeChatQRCodeDetector detector = new WeChatQRCodeDetector())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged);
                String text = detector.ProcessAndRender(m, m);
            }

        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestBarcodeDetector()
        {
            using (Mat m = EmguAssert.LoadMat("barcode_book.jpg"))
            using (Emgu.CV.Models.BarcodeDetectorModel detector = new BarcodeDetectorModel())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged);
                String text = detector.ProcessAndRender(m, m);
                Trace.WriteLine(text);
            }
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestYolo()
        {
            using (Mat m = EmguAssert.LoadMat("pedestrian.png"))
            using (Emgu.CV.Models.Yolo detector = new Yolo())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged, "YoloV8N");
                String text = detector.ProcessAndRender(m, m);
            }

        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestGpt2Tokenizer()
        {
            using (Gpt2Tokenizer tokenizer = new Gpt2Tokenizer())
            {
                await tokenizer.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(tokenizer.Initialized, "Failed to initialize the GPT-2 tokenizer.");

                String text = "hello world";
                int[] ids = tokenizer.Encode(text);
                //Known GPT-2 BPE ids for "hello" and " world"
                EmguAssert.AreEqual(2, ids.Length, "Unexpected token count.");
                EmguAssert.AreEqual(31373, ids[0], "Unexpected token id for 'hello'.");
                EmguAssert.AreEqual(995, ids[1], "Unexpected token id for ' world'.");

                String decoded = tokenizer.Decode(ids);
                EmguAssert.AreEqual(text, decoded, "Encode/Decode round trip failed.");
            }
        }

        /// <summary>
        /// Run the embedding network on the given token ids, returning the (1, count, hidden) embeddings.
        /// </summary>
        private static Mat VlmForwardEmbedding(Net embedNet, int[] tokenIds)
        {
            long[] ids = new long[tokenIds.Length];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = tokenIds[i];
            GCHandle handle = GCHandle.Alloc(ids, GCHandleType.Pinned);
            try
            {
                using (Mat inputIds = new Mat(new int[] { 1, ids.Length }, DepthType.Cv64S, handle.AddrOfPinnedObject()))
                {
                    embedNet.SetInput(inputIds, "input_ids");
                    return embedNet.Forward();
                }
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Create a 2D (rows x cols) header over a (1, rows, cols) tensor.
        /// </summary>
        private static Mat VlmTensorAsRows(Mat tensor)
        {
            int[] shape = tensor.SizeOfDimension;
            return new Mat(new int[] { shape[1], shape[2] }, tensor.Depth, tensor.DataPointer);
        }

        /// <summary>
        /// Run the language model on the (sequence, hidden) embeddings and return the
        /// argmax token id of the last sequence position.
        /// </summary>
        private static int VlmForwardLanguageModel(Net gemmaNet, Mat inputsEmbeds2D)
        {
            using (Mat inputsEmbeds = new Mat(
                new int[] { 1, inputsEmbeds2D.Rows, inputsEmbeds2D.Cols },
                DepthType.Cv32F,
                inputsEmbeds2D.DataPointer))
            {
                gemmaNet.SetInput(inputsEmbeds, "inputs_embeds");
                using (Mat logits = gemmaNet.Forward())
                {
                    int[] shape = logits.SizeOfDimension;   //(1, sequence, vocabulary)
                    int vocab = shape[2];
                    long lastRowOffset = (long)(shape[1] - 1) * vocab * sizeof(float);
                    using (Mat lastLogits = new Mat(
                        new int[] { 1, vocab },
                        DepthType.Cv32F,
                        new IntPtr(logits.DataPointer.ToInt64() + lastRowOffset)))
                    {
                        double minVal = 0, maxVal = 0;
                        Point minLoc = new Point(), maxLoc = new Point();
                        CvInvoke.MinMaxLoc(lastLogits, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                        return maxLoc.X;
                    }
                }
            }
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public void TestVlmPaliGemma2()
        {
            //C# port of opencv/samples/dnn/vlm_inference.py: PaliGemma2
            //vision-language inference. Given an image and a text prompt, it
            //generates a text response (e.g. a caption). The model is split into
            //three ONNX files: SigLIP vision encoder (image -> 256 image-feature
            //tokens), embedding (token ids -> text embeddings) and Gemma2
            //language model ([image_features | text_embeds] -> logits).
            //
            //The models are ~14 GB across ~200 files and the tokenizer descends
            //from a gated huggingface repository, so this test does not download
            //them. Set the EMGU_CV_PALIGEMMA2_MODEL_DIR environment variable to a
            //folder containing:
            //  vision_model.onnx                 - SigLIP vision encoder
            //  embedding.onnx, embedding.weight  - embedding + external weights
            //  gemma2_3b.onnx, onnx__MatMul_*,
            //  onnx__Mul_*                       - Gemma2 + external weights
            //  config.json, tokenizer.json       - OpenCV-format Gemma tokenizer
            //ONNX models: https://huggingface.co/nklskyoy/paligemma2-3b-pt-224-onnx
            //
            //config.json must be { "method": "SentencePiece" }: that tokenizer
            //method prepends the <bos> token on encode like the HuggingFace
            //Gemma tokenizer does; without <bos> the model generates a
            //degenerate response. tokenizer.json is the standard Gemma-2
            //tokenizer (e.g. from an ungated mirror such as
            //https://huggingface.co/unsloth/gemma-2-2b). With the dog416.png
            //test image and the "cap en\n" prompt the model responds
            //"dog and bike".
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_PALIGEMMA2_MODEL_DIR");
            if (String.IsNullOrEmpty(modelDir))
            {
                Console.WriteLine("TestVlmPaliGemma2 skipped: set the EMGU_CV_PALIGEMMA2_MODEL_DIR environment variable to the model folder to run this test.");
                return;
            }

            const int eosId = 1;
            const int maxNewTokens = 64;
            String prompt = "cap en\n";

            using (Tokenizer tokenizer = Tokenizer.Load(Path.Combine(modelDir, "config.json")))
            using (Net siglipNet = DnnInvoke.ReadNetFromONNX(Path.Combine(modelDir, "vision_model.onnx"), Emgu.CV.Dnn.EngineType.New))
            using (Net embedNet = DnnInvoke.ReadNetFromONNX(Path.Combine(modelDir, "embedding.onnx"), Emgu.CV.Dnn.EngineType.New))
            using (Net gemmaNet = DnnInvoke.ReadNetFromONNX(Path.Combine(modelDir, "gemma2_3b.onnx"), Emgu.CV.Dnn.EngineType.New))
            using (Mat image = EmguAssert.LoadMat("dog416.png"))
            //Resize to 224x224 and normalize to [-1, 1] in RGB NCHW order
            //(SigLIP: mean=0.5, std=0.5): (x/255 - 0.5)/0.5 = (x - 127.5)/127.5
            using (Mat pixelValues = DnnInvoke.BlobFromImage(
                image,
                1.0 / 127.5,
                new Size(224, 224),
                new MCvScalar(127.5, 127.5, 127.5),
                true,
                false))
            {
                //SigLIP vision encoder: image -> image-feature tokens (1, 256, 2304)
                siglipNet.SetInput(pixelValues, "pixel_values");
                using (Mat imageFeatures = siglipNet.Forward())
                {
                    List<int> generated = new List<int>();

                    //The growing (sequence, hidden) embedding matrix; the python
                    //sample's (1, sequence, hidden) inputs_embeds without the
                    //leading batch-1 dimension.
                    Mat inputsEmbeds = new Mat();
                    try
                    {
                        //Combine [image_features | text_embeds]
                        int[] tokens = tokenizer.Encode(prompt);
                        using (Mat textEmbeds = VlmForwardEmbedding(embedNet, tokens))
                        using (Mat imageFeatures2D = VlmTensorAsRows(imageFeatures))
                        using (Mat textEmbeds2D = VlmTensorAsRows(textEmbeds))
                        {
                            CvInvoke.VConcat(imageFeatures2D, textEmbeds2D, inputsEmbeds);
                        }

                        //Prefill
                        int newId = VlmForwardLanguageModel(gemmaNet, inputsEmbeds);
                        generated.Add(newId);

                        //Decode (no KV-cache: feed the full growing sequence each step)
                        for (int i = 0; i < maxNewTokens - 1; i++)
                        {
                            if (newId == eosId)
                                break;

                            using (Mat newEmbed = VlmForwardEmbedding(embedNet, new int[] { newId }))
                            using (Mat newEmbed2D = VlmTensorAsRows(newEmbed))
                            {
                                Mat grown = new Mat();
                                CvInvoke.VConcat(inputsEmbeds, newEmbed2D, grown);
                                inputsEmbeds.Dispose();
                                inputsEmbeds = grown;
                            }

                            newId = VlmForwardLanguageModel(gemmaNet, inputsEmbeds);
                            generated.Add(newId);
                        }
                    }
                    finally
                    {
                        inputsEmbeds.Dispose();
                    }

                    if (generated.Count > 0 && generated[generated.Count - 1] == eosId)
                        generated.RemoveAt(generated.Count - 1);

                    String response = tokenizer.Decode(generated.ToArray());
                    Console.WriteLine(String.Format("PaliGemma2 response: {0}", response));
                    EmguAssert.IsTrue(!String.IsNullOrWhiteSpace(response), "The vision language model generated an empty response.");
                }
            }
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestPedestrianDetector()
        {
            using (Mat m = EmguAssert.LoadMat("pedestrian.png"))
            using (Emgu.CV.Models.PedestrianDetector detector = new PedestrianDetector())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged);
                String text = detector.ProcessAndRender(m, m);
            }

        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestDnnSuperres()
        {
            using (Mat m = EmguAssert.LoadMat("pedestrian"))
            using (Emgu.CV.Models.Superres detector = new Models.Superres())
            {
                await detector.Init(Models.Superres.SuperresVersion.EdsrX2, DownloadManager_OnDownloadProgressChanged);
                String text = detector.ProcessAndRender(m, m);
            }

        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestDnnSSDFaceDetect()
        {
            using (Emgu.CV.Models.FaceAndLandmarkDetector detector = new Models.FaceAndLandmarkDetector())
            using (Mat img = EmguAssert.LoadMat("lena.jpg"))
            using (Mat result = new Mat())
            {
                await detector.Init(AutoTestModels.DownloadManager_OnDownloadProgressChanged);
                img.CopyTo(result);
                detector.ProcessAndRender(img, result);
                CvInvoke.Imwrite("rgb_ssd_facedetect.jpg", result);
            }
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestMACE()
        {
            using (MACE mace = new MACE(64))
            using (FaceDetectorYNModel detector = new FaceDetectorYNModel())
            {
                await detector.Init();
                using (VectorOfMat trainingFaces = new VectorOfMat())
                {
                    using (Mat img1 = EmguAssert.LoadMat("lena.jpg"))
                    {
                        foreach (var face in detector.Detect(img1))
                        {
                            using (Mat faceRegion = new Mat(img1, Rectangle.Round(face.Region)))
                            {
                                trainingFaces.Push(faceRegion);
                                using (Mat blurredFace1 = new Mat())
                                {
                                    CvInvoke.GaussianBlur(faceRegion, blurredFace1, new Size(3, 3), 1);
                                    trainingFaces.Push(blurredFace1);
                                }
                            }
                        }
                    }

                    mace.Train(trainingFaces);

                    using (Mat trainingImg1 = trainingFaces[0])
                    {
                        EmguAssert.IsTrue(mace.Same(trainingImg1));

                    }

                    String filePath = Path.Combine(Path.GetTempPath(), "mace.xml");
                    mace.Save(filePath);
                }
            }
        }
    }
}
