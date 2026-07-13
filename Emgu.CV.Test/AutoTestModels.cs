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

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestVlmPaliGemma2()
        {
            //PaliGemma2 vision-language inference, the C# equivalent of
            //opencv/samples/dnn/vlm_inference.py. Init downloads the models
            //(~15 GB) on the first run. Optionally set the
            //EMGU_CV_PALIGEMMA2_MODEL_DIR environment variable to reuse an
            //already populated model folder. With the dog416.png test image and
            //the "cap en\n" prompt the model responds "dog and bike".
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_PALIGEMMA2_MODEL_DIR");

            using (Mat image = EmguAssert.LoadMat("dog416.png"))
            using (VlmPaliGemma2 vlm = new VlmPaliGemma2(modelDir))
            {
                await vlm.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(vlm.Initialized, "Failed to initialize the PaliGemma2 model.");

                String response = vlm.Generate(image, "cap en\n");
                Console.WriteLine(String.Format("PaliGemma2 response: {0}", response));
                EmguAssert.IsTrue(!String.IsNullOrWhiteSpace(response), "The vision language model generated an empty response.");
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
        public async Task TestClip()
        {
            //CLIP ViT-B/32 joint text/image embeddings. Init downloads the
            //model (~580 MB) on the first run. Optionally set the
            //EMGU_CV_CLIP_MODEL_DIR environment variable to reuse an already
            //populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_CLIP_MODEL_DIR");

            using (Emgu.CV.Models.Clip clip = new Emgu.CV.Models.Clip(modelDir))
            {
                await clip.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(clip.Initialized, "Failed to initialize the CLIP model.");

                //Tokenizer exactness against the huggingface CLIPTokenizer reference
                int[] catIds = clip.Tokenize("a photo of a cat");
                EmguAssert.IsTrue(
                    catIds.SequenceEqual(new int[] { 49406, 320, 1125, 539, 320, 2368, 49407 }),
                    String.Format("Unexpected token ids for 'a photo of a cat': [{0}]", String.Join(",", catIds)));
                int[] helloIds = clip.Tokenize("Hello, World! 123");
                EmguAssert.IsTrue(
                    helloIds.SequenceEqual(new int[] { 49406, 3306, 267, 1002, 256, 272, 273, 274, 49407 }),
                    String.Format("Unexpected token ids for 'Hello, World! 123': [{0}]", String.Join(",", helloIds)));

                //Zero-shot: the dog416.png image (a dog and a bike) must be
                //closer to the matching caption than to an unrelated one.
                using (Mat image = EmguAssert.LoadMat("dog416.png"))
                {
                    float[] imageEmbedding = clip.EmbedImage(image);
                    float[] dogEmbedding = clip.EmbedText("a photo of a dog");
                    float[] cityEmbedding = clip.EmbedText("a photo of a city at night");

                    double dogSimilarity = Emgu.CV.Models.Clip.CosineSimilarity(imageEmbedding, dogEmbedding);
                    double citySimilarity = Emgu.CV.Models.Clip.CosineSimilarity(imageEmbedding, cityEmbedding);
                    Console.WriteLine(String.Format("CLIP similarity: dog={0:F4}, city={1:F4}", dogSimilarity, citySimilarity));
                    EmguAssert.IsTrue(dogSimilarity > citySimilarity,
                        "Expected the dog image to be more similar to the dog caption than to the city caption.");
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
        public async Task TestSiglip()
        {
            //SigLIP joint text/image embeddings. Init downloads the model
            //(~780 MB) on the first run. Optionally set the
            //EMGU_CV_SIGLIP_MODEL_DIR environment variable to reuse an already
            //populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_SIGLIP_MODEL_DIR");

            using (Emgu.CV.Models.Siglip siglip = new Emgu.CV.Models.Siglip(modelDir))
            {
                await siglip.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(siglip.Initialized, "Failed to initialize the SigLIP model.");

                //Tokenizer exactness against the huggingface SiglipTokenizer
                //reference (the fixed 64 token sequence is padded with </s>=1).
                int[] dogIds = siglip.Tokenize("a photo of a dog");
                int[] expectedDog = new int[] { 262, 266, 1304, 267, 262, 266, 1571, 1 };
                EmguAssert.IsTrue(
                    dogIds.Length == 64
                    && dogIds.Take(expectedDog.Length).SequenceEqual(expectedDog)
                    && dogIds.Skip(expectedDog.Length).All(id => id == 1),
                    String.Format("Unexpected token ids for 'a photo of a dog': [{0}]", String.Join(",", dogIds.Take(12))));
                int[] helloIds = siglip.Tokenize("Hello, World! 123");
                int[] expectedHello = new int[] { 14647, 459, 17061, 1 };
                EmguAssert.IsTrue(
                    helloIds.Take(expectedHello.Length).SequenceEqual(expectedHello),
                    String.Format("Unexpected token ids for 'Hello, World! 123': [{0}]", String.Join(",", helloIds.Take(8))));

                //Zero-shot: the dog416.png image (a dog and a bike) must be
                //closer to the matching caption than to an unrelated one.
                using (Mat image = EmguAssert.LoadMat("dog416.png"))
                {
                    float[] imageEmbedding = siglip.EmbedImage(image);
                    float[] dogEmbedding = siglip.EmbedText("a photo of a dog");
                    float[] cityEmbedding = siglip.EmbedText("a photo of a city at night");

                    double dogSimilarity = Emgu.CV.Models.Siglip.CosineSimilarity(imageEmbedding, dogEmbedding);
                    double citySimilarity = Emgu.CV.Models.Siglip.CosineSimilarity(imageEmbedding, cityEmbedding);
                    Console.WriteLine(String.Format("SigLIP similarity: dog={0:F4}, city={1:F4}", dogSimilarity, citySimilarity));
                    EmguAssert.IsTrue(dogSimilarity > citySimilarity,
                        "Expected the dog image to be more similar to the dog caption than to the city caption.");
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
        public async Task TestDnnKVCache()
        {
            //Smoke test for the Net KV-cache API: enable, reset and disable the
            //cache around forward passes. The model used here has no attention
            //layers, so the cache is functionally a no-op; the test validates
            //the API plumbing (see the gemma3/qwen dnn samples in OpenCV for
            //cached autoregressive generation with attention models).
            FileDownloadManager manager = new FileDownloadManager();
            manager.AddFile(
                "https://emgu-public.s3.amazonaws.com/paddleocr/ch_PP-OCRv4_det_infer.onnx",
                Path.Combine("emgu", "paddleocr"),
                "D2A7720D45A54257208B1E13E36A8479894CB74155A5EFE29462512D42F49DA9");
            manager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;
            await manager.Download();
            EmguAssert.IsTrue(manager.AllFilesDownloaded, "Failed to download the test model.");

            using (Net net = DnnInvoke.ReadNetFromONNX(manager.Files[0].LocalFile, Emgu.CV.Dnn.EngineType.New))
            using (Mat image = new Mat(64, 64, DepthType.Cv8U, 3))
            {
                image.SetTo(new MCvScalar(128, 128, 128));
                using (Mat blob = DnnInvoke.BlobFromImage(image, 1.0 / 255))
                {
                    net.EnableKVCache();
                    net.SetInput(blob);
                    using (Mat result = net.Forward())
                        EmguAssert.IsTrue(!result.IsEmpty, "Forward with KV-cache enabled returned an empty result.");

                    net.ResetKVCache();
                    net.SetInput(blob);
                    using (Mat result = net.Forward())
                        EmguAssert.IsTrue(!result.IsEmpty, "Forward after KV-cache reset returned an empty result.");

                    net.DisableKVCache();
                    net.SetInput(blob);
                    using (Mat result = net.Forward())
                        EmguAssert.IsTrue(!result.IsEmpty, "Forward with KV-cache disabled returned an empty result.");
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
        public async Task TestQwen25()
        {
            //Qwen2.5 0.5B text generation with KV-cached autoregressive
            //decoding, the C# equivalent of opencv/samples/dnn/qwen_inference.py.
            //Init downloads the model (~2.4 GB) on the first run. Optionally set
            //the EMGU_CV_QWEN25_MODEL_DIR environment variable to reuse an
            //already populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_QWEN25_MODEL_DIR");

            using (Qwen25 llm = new Qwen25(modelDir))
            {
                await llm.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(llm.Initialized, "Failed to initialize the Qwen2.5 model.");

                String response = llm.Generate("What is OpenCV? Answer in one sentence.", 48);
                Console.WriteLine(String.Format("Qwen2.5 response: {0}", response));
                EmguAssert.IsTrue(!String.IsNullOrWhiteSpace(response), "The language model generated an empty response.");

                //Multi-turn chat: the conversation history is kept in the
                //KV-cache, the second turn must remember the first one.
                llm.ResetChat();
                String turn1 = llm.Chat("Hello, my name is Canming.", 32);
                Console.WriteLine(String.Format("Qwen2.5 chat turn 1: {0}", turn1));
                String turn2 = llm.Chat("What is my name?", 32);
                Console.WriteLine(String.Format("Qwen2.5 chat turn 2: {0}", turn2));
                EmguAssert.IsTrue(turn2.Contains("Canming"), String.Format("Expected the chat session to remember the name, got: '{0}'", turn2));
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
        public async Task TestPaddleOCR()
        {
            using (Mat image = new Mat(200, 700, DepthType.Cv8U, 3))
            {
                image.SetTo(new MCvScalar(255, 255, 255));
                using (FontFace font = new FontFace("sans"))
                    CvInvoke.PutText(image, "HELLO WORLD 123", new Point(50, 120), new MCvScalar(0, 0, 0), font, 60);

                using (Emgu.CV.Models.PaddleOCR ocr = new Emgu.CV.Models.PaddleOCR())
                {
                    await ocr.Init(DownloadManager_OnDownloadProgressChanged);
                    EmguAssert.IsTrue(ocr.Initialized, "Failed to initialize the PaddleOCR model.");

                    var results = ocr.Recognize(image);
                    StringBuilder allText = new StringBuilder();
                    foreach (var result in results)
                    {
                        Console.WriteLine(String.Format("PaddleOCR: '{0}' (confidence {1:F3})", result.Text, result.Confidence));
                        allText.Append(result.Text);
                    }

                    String recognized = allText.ToString().Replace(" ", "").ToUpperInvariant();
                    EmguAssert.IsTrue(recognized.Contains("HELLO"), String.Format("Expected 'HELLO' in the recognized text, got: '{0}'", allText));
                    EmguAssert.IsTrue(recognized.Contains("WORLD"), String.Format("Expected 'WORLD' in the recognized text, got: '{0}'", allText));
                    EmguAssert.IsTrue(recognized.Contains("123"), String.Format("Expected '123' in the recognized text, got: '{0}'", allText));

                    String message = ocr.ProcessAndRender(image, image);
                    Console.WriteLine(message);
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
