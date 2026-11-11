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

        [DllImport("ucrtbase.dll")]
        private static extern int _open_osfhandle(IntPtr osfhandle, int flags);

        [DllImport("ucrtbase.dll")]
        private static extern int _dup(int fd);

        [DllImport("ucrtbase.dll")]
        private static extern int _dup2(int fd1, int fd2);

        [DllImport("ucrtbase.dll")]
        private static extern int _close(int fd);

        /// <summary>
        /// ONNX Runtime silently falls back to the CPU when its CUDA execution provider fails to load
        /// (for example because the provider needs CUDA libraries that are not present). The only sign is a
        /// native stderr warning, so this test captures it and also compares the CUDA output with the CPU output.
        /// </summary>
#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestYoloOrtCuda()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Assert.Inconclusive("Native stderr capture is only implemented for Windows.");
            if (!CvInvoke.HaveOnnxRuntime)
                Assert.Inconclusive("OpenCV was built without ONNX Runtime.");
            if (!CudaInvoke.HasCuda)
                Assert.Inconclusive("No CUDA device available.");

            using (Emgu.CV.Models.Yolo detector = new Yolo())
            {
                // Downloads yolo11n.onnx into the model folder.
                await detector.Init(Yolo.YoloVersion.Yolo11N, DownloadManager_OnDownloadProgressChanged);
            }

            String modelFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "emgu", "yolo", "yolo11n.onnx");
            if (!File.Exists(modelFile))
                Assert.Inconclusive("Model file not found: " + modelFile);

            String errFile = Path.Combine(Path.GetTempPath(), "emgu_ort_cuda_stderr_" + Guid.NewGuid().ToString("N") + ".txt");
            float[][] outputs = new float[2][];
            using (Mat image = EmguAssert.LoadMat("pedestrian.png"))
            using (Mat blob = new Mat())
            {
                DnnInvoke.BlobFromImage(image, blob, 1.0 / 255, new Size(640, 640), new MCvScalar(), true, false);
                using (FileStream fs = new FileStream(errFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    // Redirect native stderr (file descriptor 2) into the file for the duration of the run.
                    int savedStderr = _dup(2);
                    try
                    {
                        _dup2(_open_osfhandle(fs.SafeFileHandle.DangerousGetHandle(), 0), 2);
                        for (int i = 0; i < 2; i++)
                        {
                            using (Net net = DnnInvoke.ReadNetFromONNX(modelFile, EngineType.Ort))
                            {
                                if (i == 1)
                                    net.SetPreferableTarget(Target.Cuda);
                                net.SetInput(blob);
                                using (Mat result = net.Forward())
                                {
                                    outputs[i] = new float[result.Total.ToInt32() * result.NumberOfChannels];
                                    result.CopyTo(outputs[i]);
                                }
                            }
                        }
                    }
                    finally
                    {
                        _dup2(savedStderr, 2);
                        _close(savedStderr);
                    }
                }
            }

            // ORT writes its warning as UTF-16 when stderr is a file, so decode both possibilities.
            byte[] errBytes;
            using (FileStream errStream = new FileStream(errFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (MemoryStream errMemory = new MemoryStream())
            {
                errStream.CopyTo(errMemory);
                errBytes = errMemory.ToArray();
            }
            String errText = Encoding.Unicode.GetString(errBytes) + Encoding.UTF8.GetString(errBytes);
            try { File.Delete(errFile); } catch { }
            EmguAssert.IsTrue(
                !errText.Contains("CUDA EP unavailable"),
                "ONNX Runtime fell back to the CPU: " + errText);

            EmguAssert.IsTrue(outputs[0].Length > 0 && outputs[0].Length == outputs[1].Length, "Unexpected output size");
            double diff = 0, norm = 0;
            for (int i = 0; i < outputs[0].Length; i++)
            {
                double d = outputs[0][i] - outputs[1][i];
                diff += d * d;
                norm += (double)outputs[0][i] * outputs[0][i];
            }
            double relativeError = Math.Sqrt(diff / norm);
            Trace.WriteLine("ORT CUDA vs CPU relative L2 error: " + relativeError);
            EmguAssert.IsTrue(relativeError < 1e-2, "ORT CUDA output differs from the CPU output: " + relativeError);
        }

        private class OrtCudaCase
        {
            public String Name;
            public String File;
            public int Width, Height;
            public bool Gray;
            public bool Text;
            public bool LengthMayDiffer;
            public double Scale = 1.0 / 255;
            public bool SwapRB = true;
            public int[] Reshape;
        }

        private static void SetInt64Input(Net net, long[] values, String name)
        {
            GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
            try
            {
                using (Mat input = new Mat(new int[] { 1, values.Length }, DepthType.Cv64S, handle.AddrOfPinnedObject()))
                    net.SetInput(input, name);
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Run a model on ONNX Runtime, on the CPU or with Target.Cuda, with native stderr captured. Returns the
        /// first output and the average time of a forward pass (after two warm-up passes).
        /// </summary>
        private static float[] OrtForward(String modelFile, bool cuda, OrtCudaCase c, Mat image, out double msPerRun, out String stderrText)
        {
            String errFile = Path.Combine(Path.GetTempPath(), "emgu_ort_cuda_stderr_" + Guid.NewGuid().ToString("N") + ".txt");
            float[] output;
            double elapsedMs = 0;
            using (FileStream fs = new FileStream(errFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                int savedStderr = _dup(2);
                try
                {
                    _dup2(_open_osfhandle(fs.SafeFileHandle.DangerousGetHandle(), 0), 2);
                    using (Net net = DnnInvoke.ReadNetFromONNX(modelFile, EngineType.Ort))
                    using (Mat gray = new Mat())
                    using (Mat blob = new Mat())
                    {
                        if (cuda)
                            net.SetPreferableTarget(Target.Cuda);

                        long[] ids = new long[] { 0, 6, 1234, 2054, 2003, 1996, 3007, 1997, 2605, 1029, 2 };
                        long[] mask = new long[ids.Length];
                        for (int i = 0; i < mask.Length; i++)
                            mask[i] = 1;
                        long[] types = new long[ids.Length];

                        Mat reshaped = null;
                        Mat input = blob;
                        if (!c.Text)
                        {
                            Mat source = image;
                            if (c.Gray)
                            {
                                CvInvoke.CvtColor(image, gray, ColorConversion.Bgr2Gray);
                                source = gray;
                            }
                            DnnInvoke.BlobFromImage(source, blob, c.Scale, new Size(c.Width, c.Height), new MCvScalar(), c.SwapRB && !c.Gray, false);
                            if (c.Reshape != null)
                            {
                                reshaped = blob.Reshape(1, c.Reshape);
                                input = reshaped;
                            }
                        }

                        Func<Mat> forward = delegate ()
                        {
                            if (c.Text)
                            {
                                SetInt64Input(net, ids, "input_ids");
                                SetInt64Input(net, mask, "attention_mask");
                                SetInt64Input(net, types, "token_type_ids");
                            }
                            else
                                net.SetInput(input);
                            return net.Forward();
                        };

                        for (int i = 0; i < 2; i++)
                            forward().Dispose();
                        Stopwatch sw = Stopwatch.StartNew();
                        for (int i = 0; i < 5; i++)
                            forward().Dispose();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;

                        using (Mat result = forward())
                        using (Mat floatResult = new Mat())
                        {
                            // Some models (for example DISK) return integer outputs; compare them as floats.
                            result.ConvertTo(floatResult, DepthType.Cv32F);
                            output = new float[floatResult.Total.ToInt32() * floatResult.NumberOfChannels];
                            floatResult.CopyTo(output);
                        }
                        if (reshaped != null)
                            reshaped.Dispose();
                    }
                }
                finally
                {
                    _dup2(savedStderr, 2);
                    _close(savedStderr);
                }
            }
            msPerRun = elapsedMs / 5;

            byte[] errBytes;
            using (FileStream errStream = new FileStream(errFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (MemoryStream errMemory = new MemoryStream())
            {
                errStream.CopyTo(errMemory);
                errBytes = errMemory.ToArray();
            }
            stderrText = Encoding.Unicode.GetString(errBytes) + Encoding.UTF8.GetString(errBytes);
            try { File.Delete(errFile); } catch { }
            return output;
        }

        /// <summary>
        /// Run the ONNX models used by Emgu.CV.Models (other than YOLO) on ONNX Runtime with Target.Cuda and
        /// compare against the ONNX Runtime CPU output. Some models are downloaded by their Emgu.CV.Models
        /// classes; the others are tested only when their file is already in the emgu model folder.
        /// </summary>
#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestOrtCudaModels()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Assert.Inconclusive("Native stderr capture is only implemented for Windows.");
            if (!CvInvoke.HaveOnnxRuntime)
                Assert.Inconclusive("OpenCV was built without ONNX Runtime.");
            if (!CudaInvoke.HasCuda)
                Assert.Inconclusive("No CUDA device available.");

            // Download the models through their wrappers.
            using (Emgu.CV.Models.Dinov2 m = new Emgu.CV.Models.Dinov2())
                await m.Init(DownloadManager_OnDownloadProgressChanged);
            using (Emgu.CV.Models.PaddleOCR m = new Emgu.CV.Models.PaddleOCR())
                await m.Init(DownloadManager_OnDownloadProgressChanged);
            using (Emgu.CV.Models.MaskRcnn m = new Emgu.CV.Models.MaskRcnn())
                await m.Init(DownloadManager_OnDownloadProgressChanged);
            using (Emgu.CV.Models.MiniLM m = new Emgu.CV.Models.MiniLM())
                await m.Init(DownloadManager_OnDownloadProgressChanged);
            using (Emgu.CV.Models.MultilingualE5 m = new Emgu.CV.Models.MultilingualE5())
                await m.Init(DownloadManager_OnDownloadProgressChanged);

            // Input sizes and layouts follow what each model expects.
            List<OrtCudaCase> cases = new List<OrtCudaCase>
            {
                new OrtCudaCase { Name = "dinov2_small", File = "dinov2_small_onnx/model.onnx", Width = 224, Height = 224 },
                new OrtCudaCase { Name = "paddleocr_det", File = "paddleocr/ch_PP-OCRv4_det_infer.onnx", Width = 640, Height = 640 },
                new OrtCudaCase { Name = "paddleocr_rec", File = "paddleocr/ch_PP-OCRv4_rec_infer.onnx", Width = 320, Height = 48 },
                new OrtCudaCase { Name = "mask_rcnn", File = "mask_rcnn_onnx/MaskRCNN-10.onnx", Width = 800, Height = 800, Scale = 1.0, SwapRB = false, Reshape = new int[] { 3, 800, 800 } },
                new OrtCudaCase { Name = "all_minilm_l6_v2", File = "all_minilm_l6_v2_onnx/model.onnx", Text = true },
                new OrtCudaCase { Name = "multilingual_e5_small", File = "multilingual_e5_small_onnx/model.onnx", Text = true },
                // The models below are only tested when their files are already in the emgu model folder.
                new OrtCudaCase { Name = "DB_TD500_resnet50", File = "scene_text_detector/DB_TD500_resnet50.onnx", Width = 736, Height = 736 },
                new OrtCudaCase { Name = "crnn_cs_CN", File = "scene_text_detector/crnn_cs_CN.onnx", Width = 100, Height = 32 },
                new OrtCudaCase { Name = "sface", File = "face_recognizer_sface/face_recognition_sface_2021dec.onnx", Width = 112, Height = 112, Scale = 1.0, SwapRB = false },
                new OrtCudaCase { Name = "yunet_2023", File = "face_detector_yn/face_detection_yunet_2023mar.onnx", Width = 640, Height = 640, Scale = 1.0, SwapRB = false },
                new OrtCudaCase { Name = "FSRCNN_x2", File = "superres/FSRCNN_x2.onnx", Width = 128, Height = 128, Gray = true, Reshape = new int[] { 1, 128, 128, 1 } },
                new OrtCudaCase { Name = "xfeat", File = "xfeat/xfeat.onnx", Width = 640, Height = 640, Gray = true },
                // The output is a keypoint list whose length depends on a threshold, so CPU and CUDA can differ by a keypoint.
                new OrtCudaCase { Name = "disk", File = "disk/disk.onnx", Width = 640, Height = 480, LengthMayDiffer = true },
            };

            String modelRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "emgu");
            int tested = 0;
            using (Mat image = EmguAssert.LoadMat("pedestrian.png"))
            {
                foreach (OrtCudaCase c in cases)
                {
                    String modelFile = Path.Combine(modelRoot, c.File);
                    if (!File.Exists(modelFile))
                    {
                        Trace.WriteLine(c.Name + ": model file not found, skipped.");
                        continue;
                    }

                    double cpuMs, cudaMs;
                    String cpuErr, cudaErr;
                    float[] cpu = OrtForward(modelFile, false, c, image, out cpuMs, out cpuErr);
                    float[] cuda = OrtForward(modelFile, true, c, image, out cudaMs, out cudaErr);

                    EmguAssert.IsTrue(
                        !cudaErr.Contains("CUDA EP unavailable"),
                        c.Name + ": ONNX Runtime fell back to the CPU: " + cudaErr);

                    int length = Math.Min(cpu.Length, cuda.Length);
                    EmguAssert.IsTrue(length > 0, c.Name + ": empty output");
                    if (c.LengthMayDiffer)
                    {
                        // A keypoint list: one keypoint more or less shifts every later element, so only the size is compared.
                        EmguAssert.IsTrue(
                            Math.Abs(cpu.Length - cuda.Length) <= 0.01 * Math.Max(cpu.Length, cuda.Length),
                            c.Name + ": output size differs " + cpu.Length + " vs " + cuda.Length);
                        Trace.WriteLine(String.Format("{0}: CPU {1:F1} ms, CUDA {2:F1} ms, output size {3} vs {4}", c.Name, cpuMs, cudaMs, cpu.Length, cuda.Length));
                        tested++;
                        continue;
                    }
                    EmguAssert.IsTrue(cpu.Length == cuda.Length, c.Name + ": output size differs " + cpu.Length + " vs " + cuda.Length);

                    double diff = 0, norm = 0;
                    for (int i = 0; i < length; i++)
                    {
                        double d = cpu[i] - cuda[i];
                        diff += d * d;
                        norm += (double)cpu[i] * cpu[i];
                    }
                    double relativeError = norm > 0 ? Math.Sqrt(diff / norm) : 0;
                    Trace.WriteLine(String.Format("{0}: CPU {1:F1} ms, CUDA {2:F1} ms, relative L2 error {3:E2}", c.Name, cpuMs, cudaMs, relativeError));
                    EmguAssert.IsTrue(relativeError < 2e-2, c.Name + ": ORT CUDA output differs from the CPU output: " + relativeError);
                    tested++;
                }
            }

            if (tested == 0)
                Assert.Inconclusive("No model file found.");
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
        public async Task TestDinov2()
        {
            //DINOv2 visual similarity embeddings. Init downloads the model
            //(~85 MB) on the first run. Optionally set the
            //EMGU_CV_DINOV2_MODEL_DIR environment variable to reuse an already
            //populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_DINOV2_MODEL_DIR");

            using (Emgu.CV.Models.Dinov2 dino = new Emgu.CV.Models.Dinov2(modelDir))
            {
                await dino.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(dino.Initialized, "Failed to initialize the DINOv2 model.");

                //A cropped view of the same scene must embed closer to the
                //original than a completely different image.
                using (Mat dog = EmguAssert.LoadMat("dog416.png"))
                using (Mat unrelated = EmguAssert.LoadMat("lena.jpg"))
                using (Mat dogCropped = new Mat(dog, new Rectangle(20, 20, dog.Width - 40, dog.Height - 40)))
                {
                    float[] dogEmbedding = dino.EmbedImage(dog);
                    float[] croppedEmbedding = dino.EmbedImage(dogCropped);
                    float[] unrelatedEmbedding = dino.EmbedImage(unrelated);

                    double croppedSimilarity = Emgu.CV.Models.Dinov2.CosineSimilarity(dogEmbedding, croppedEmbedding);
                    double unrelatedSimilarity = Emgu.CV.Models.Dinov2.CosineSimilarity(dogEmbedding, unrelatedEmbedding);
                    Console.WriteLine(String.Format("DINOv2 similarity: cropped={0:F4}, unrelated={1:F4}", croppedSimilarity, unrelatedSimilarity));
                    EmguAssert.IsTrue(croppedSimilarity > unrelatedSimilarity,
                        "Expected the cropped view to be more similar to the original image than an unrelated image.");
                    EmguAssert.IsTrue(croppedSimilarity > 0.5,
                        String.Format("Expected a high similarity for the cropped view, got {0:F4}", croppedSimilarity));
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
        public async Task TestMiniLM()
        {
            //all-MiniLM-L6-v2 sentence embeddings. Init downloads the model
            //(~87 MB) on the first run. Optionally set the
            //EMGU_CV_MINILM_MODEL_DIR environment variable to reuse an already
            //populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_MINILM_MODEL_DIR");

            using (Emgu.CV.Models.MiniLM minilm = new Emgu.CV.Models.MiniLM(modelDir))
            {
                await minilm.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(minilm.Initialized, "Failed to initialize the MiniLM model.");

                //Tokenizer exactness against the huggingface BertTokenizer reference
                int[] catIds = minilm.Tokenize("The cat sits on the mat.");
                EmguAssert.IsTrue(
                    catIds.SequenceEqual(new int[] { 101, 1996, 4937, 7719, 2006, 1996, 13523, 1012, 102 }),
                    String.Format("Unexpected token ids for 'The cat sits on the mat.': [{0}]", String.Join(",", catIds)));
                int[] subwordIds = minilm.Tokenize("unbelievable preprocessing");
                EmguAssert.IsTrue(
                    subwordIds.SequenceEqual(new int[] { 101, 23653, 17463, 3217, 9623, 7741, 102 }),
                    String.Format("Unexpected token ids for 'unbelievable preprocessing': [{0}]", String.Join(",", subwordIds)));

                //Semantic similarity: a paraphrase must be closer than an
                //unrelated sentence.
                float[] reference = minilm.EmbedText("The cat sits on the mat.");
                float[] paraphrase = minilm.EmbedText("A kitten is resting on a rug.");
                float[] unrelated = minilm.EmbedText("Quantum computers use superposition to perform calculations.");

                double paraphraseSimilarity = Emgu.CV.Models.MiniLM.CosineSimilarity(reference, paraphrase);
                double unrelatedSimilarity = Emgu.CV.Models.MiniLM.CosineSimilarity(reference, unrelated);
                Console.WriteLine(String.Format("MiniLM similarity: paraphrase={0:F4}, unrelated={1:F4}", paraphraseSimilarity, unrelatedSimilarity));
                EmguAssert.IsTrue(paraphraseSimilarity > unrelatedSimilarity,
                    "Expected the paraphrase to be more similar than the unrelated sentence.");
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
        public async Task TestMultilingualE5()
        {
            //multilingual-e5-small sentence embeddings. Init downloads the
            //model (~450 MB) on the first run. Optionally set the
            //EMGU_CV_E5_MODEL_DIR environment variable to reuse an already
            //populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_E5_MODEL_DIR");

            using (Emgu.CV.Models.MultilingualE5 e5 = new Emgu.CV.Models.MultilingualE5(modelDir))
            {
                await e5.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(e5.Initialized, "Failed to initialize the multilingual E5 model.");

                //Tokenizer exactness against the huggingface XLMRobertaTokenizer
                //reference, including a non-latin script and accents.
                int[] catIds = e5.Tokenize("query: The cat sits on the mat.");
                EmguAssert.IsTrue(
                    catIds.SequenceEqual(new int[] { 0, 41, 1294, 12, 581, 7515, 1661, 7, 98, 70, 2589, 5, 2 }),
                    String.Format("Unexpected token ids for the english sentence: [{0}]", String.Join(",", catIds)));
                int[] chineseIds = e5.Tokenize("query: 猫坐在垫子上。");
                EmguAssert.IsTrue(
                    chineseIds.SequenceEqual(new int[] { 0, 41, 1294, 12, 6, 35076, 48533, 174564, 1344, 575, 30, 2 }),
                    String.Format("Unexpected token ids for the chinese sentence: [{0}]", String.Join(",", chineseIds)));
                int[] accentIds = e5.Tokenize("query: café naïve résumé");
                EmguAssert.IsTrue(
                    accentIds.SequenceEqual(new int[] { 0, 41, 1294, 12, 26216, 24, 9392, 272, 233482, 2 }),
                    String.Format("Unexpected token ids for the accented text: [{0}]", String.Join(",", accentIds)));

                //Cross-lingual semantics: the german translation must be closer
                //to the english sentence than an unrelated english sentence.
                float[] english = e5.EmbedText("The cat sits on the mat.");
                float[] german = e5.EmbedText("Die Katze sitzt auf der Matte.");
                float[] unrelated = e5.EmbedText("Quantum computers use superposition.");

                double germanSimilarity = Emgu.CV.Models.MultilingualE5.CosineSimilarity(english, german);
                double unrelatedSimilarity = Emgu.CV.Models.MultilingualE5.CosineSimilarity(english, unrelated);
                Console.WriteLine(String.Format("E5 similarity: german={0:F4}, unrelated={1:F4}", germanSimilarity, unrelatedSimilarity));
                EmguAssert.IsTrue(germanSimilarity > unrelatedSimilarity,
                    "Expected the german translation to be more similar than the unrelated sentence.");
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
        public async Task TestSiglip2()
        {
            //SigLIP 2 joint multilingual text / image embeddings. Init
            //downloads the model (~1.4 GB) on the first run. Optionally set the
            //EMGU_CV_SIGLIP2_MODEL_DIR environment variable to reuse an already
            //populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_SIGLIP2_MODEL_DIR");

            using (Emgu.CV.Models.Siglip2 siglip2 = new Emgu.CV.Models.Siglip2(modelDir))
            {
                await siglip2.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(siglip2.Initialized, "Failed to initialize the SigLIP 2 model.");

                //Tokenizer exactness against the huggingface GemmaTokenizerFast
                //reference (fixed 64 token sequence, eos=1 terminated, 0 padded).
                int[] dogIds = siglip2.Tokenize("a photo of a dog");
                int[] expectedDog = new int[] { 235250, 2686, 576, 476, 5929, 1 };
                EmguAssert.IsTrue(
                    dogIds.Length == 64
                    && dogIds.Take(expectedDog.Length).SequenceEqual(expectedDog)
                    && dogIds.Skip(expectedDog.Length).All(id => id == 0),
                    String.Format("Unexpected token ids for 'a photo of a dog': [{0}]", String.Join(",", dogIds.Take(10))));
                int[] helloIds = siglip2.Tokenize("Hello, World! 123");
                int[] expectedHello = new int[] { 4521, 235269, 3855, 235341, 235248, 235274, 235284, 235304, 1 };
                EmguAssert.IsTrue(
                    helloIds.Take(expectedHello.Length).SequenceEqual(expectedHello),
                    String.Format("Unexpected token ids for 'Hello, World! 123': [{0}]", String.Join(",", helloIds.Take(12))));

                //Zero-shot: the dog416.png image (a dog and a bike) must be
                //closer to the matching caption than to an unrelated one, and
                //the german caption must work as well (multilingual text tower).
                using (Mat image = EmguAssert.LoadMat("dog416.png"))
                {
                    float[] imageEmbedding = siglip2.EmbedImage(image);
                    float[] dogEmbedding = siglip2.EmbedText("a photo of a dog");
                    float[] germanDogEmbedding = siglip2.EmbedText("ein Foto von einem Hund");
                    float[] cityEmbedding = siglip2.EmbedText("a photo of a city at night");

                    double dogSimilarity = Emgu.CV.Models.Siglip2.CosineSimilarity(imageEmbedding, dogEmbedding);
                    double germanDogSimilarity = Emgu.CV.Models.Siglip2.CosineSimilarity(imageEmbedding, germanDogEmbedding);
                    double citySimilarity = Emgu.CV.Models.Siglip2.CosineSimilarity(imageEmbedding, cityEmbedding);
                    Console.WriteLine(String.Format("SigLIP2 similarity: dog={0:F4}, german dog={1:F4}, city={2:F4}", dogSimilarity, germanDogSimilarity, citySimilarity));
                    EmguAssert.IsTrue(dogSimilarity > citySimilarity,
                        "Expected the dog image to be more similar to the dog caption than to the city caption.");
                    EmguAssert.IsTrue(germanDogSimilarity > citySimilarity,
                        "Expected the dog image to be more similar to the german dog caption than to the city caption.");
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
        public async Task TestSmolVlm2()
        {
            //SmolVLM2 vision-language inference with KV-cached decoding. Init
            //downloads the model (~1.1 GB) on the first run. Optionally set the
            //EMGU_CV_SMOLVLM2_MODEL_DIR environment variable to reuse an
            //already populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_SMOLVLM2_MODEL_DIR");

            using (Mat image = EmguAssert.LoadMat("dog416.png"))
            using (Emgu.CV.Models.SmolVlm2 vlm = new Emgu.CV.Models.SmolVlm2(modelDir))
            {
                await vlm.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(vlm.Initialized, "Failed to initialize the SmolVLM2 model.");

                //Tokenizer exactness against the huggingface GPT2TokenizerFast
                //reference, including the individual digit splitting.
                int[] helloIds = vlm.Tokenize("Hello, World! 123");
                EmguAssert.IsTrue(
                    helloIds.SequenceEqual(new int[] { 19556, 28, 2260, 17, 216, 33, 34, 35 }),
                    String.Format("Unexpected token ids for 'Hello, World! 123': [{0}]", String.Join(",", helloIds)));
                int[] priceIds = vlm.Tokenize("The price is $19.99");
                EmguAssert.IsTrue(
                    priceIds.SequenceEqual(new int[] { 504, 4319, 314, 1885, 33, 41, 30, 41, 41 }),
                    String.Format("Unexpected token ids for 'The price is $19.99': [{0}]", String.Join(",", priceIds)));

                String response = vlm.Generate(image, "Describe this image briefly.", 48);
                Console.WriteLine(String.Format("SmolVLM2 response: {0}", response));
                EmguAssert.IsTrue(!String.IsNullOrWhiteSpace(response), "The vision language model generated an empty response.");
                EmguAssert.IsTrue(response.ToLowerInvariant().Contains("dog"),
                    String.Format("Expected the response to mention the dog, got: '{0}'", response));
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
        public async Task TestQwen3()
        {
            //Qwen3 0.6B text generation with KV-cached autoregressive decoding
            //in the non-thinking mode. Init downloads the model (~2.9 GB) on
            //the first run. Optionally set the EMGU_CV_QWEN3_MODEL_DIR
            //environment variable to reuse an already populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_QWEN3_MODEL_DIR");

            using (Qwen3 llm = new Qwen3(modelDir))
            {
                await llm.Init(Qwen3.Qwen3Version.Qwen3_0_6B, DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(llm.Initialized, "Failed to initialize the Qwen3 model.");

                String response = llm.Generate("What is OpenCV? Answer in one sentence.", 48);
                Console.WriteLine(String.Format("Qwen3 response: {0}", response));
                EmguAssert.IsTrue(!String.IsNullOrWhiteSpace(response), "The language model generated an empty response.");
                EmguAssert.IsTrue(!response.Contains("<think>"), "Expected the non-thinking mode, but the response contains a think block.");

                //Multi-turn chat memory. (A "my name" probe is not used here:
                //Qwen3 0.6B in the non-thinking mode misreads it as a question
                //about the assistant's own name, in torch as well.)
                llm.ResetChat();
                String turn1 = llm.Chat("My favorite color is turquoise.", 32);
                Console.WriteLine(String.Format("Qwen3 chat turn 1: {0}", turn1));
                String turn2 = llm.Chat("What is my favorite color?", 32);
                Console.WriteLine(String.Format("Qwen3 chat turn 2: {0}", turn2));
                EmguAssert.IsTrue(turn2.ToLowerInvariant().Contains("turquoise"), String.Format("Expected the chat session to remember the color, got: '{0}'", turn2));
            }
        }

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

            using (Net net = DnnInvoke.ReadNetFromONNX(manager.Files[0].LocalFile, Emgu.CV.Dnn.EngineType.OpenCV))
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
        public async Task TestQwen3Large()
        {
            //The Qwen3 1.7B variant (~7.6 GB download). Optionally set the
            //EMGU_CV_QWEN3_LARGE_MODEL_DIR environment variable to reuse an
            //already populated model folder.
            String modelDir = Environment.GetEnvironmentVariable("EMGU_CV_QWEN3_LARGE_MODEL_DIR");

            using (Qwen3 llm = new Qwen3(modelDir))
            {
                await llm.Init(Qwen3.Qwen3Version.Qwen3_1_7B, DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(llm.Initialized, "Failed to initialize the Qwen3 1.7B model.");

                var watch = Stopwatch.StartNew();
                String response = llm.Generate("What is OpenCV? Answer in one sentence.", 48);
                watch.Stop();
                Console.WriteLine(String.Format("Qwen3 1.7B response ({0} ms): {1}", watch.ElapsedMilliseconds, response));
                EmguAssert.IsTrue(!String.IsNullOrWhiteSpace(response), "The language model generated an empty response.");
                EmguAssert.IsTrue(!response.Contains("<think>"), "Expected the non-thinking mode, but the response contains a think block.");
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
                    //The recognizer sometimes reads the letter O as the digit 0 in a sans typeface, so fold 0 into O for the letter checks only.
                    String letters = recognized.Replace('0', 'O');
                    EmguAssert.IsTrue(letters.Contains("HELLO"), String.Format("Expected 'HELLO' in the recognized text, got: '{0}'", allText));
                    EmguAssert.IsTrue(letters.Contains("WORLD"), String.Format("Expected 'WORLD' in the recognized text, got: '{0}'", allText));
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
            using (Mat m = EmguAssert.LoadMat("pedestrian.png"))
            using (Emgu.CV.Models.Superres detector = new Models.Superres())
            {
                await detector.Init(Models.Superres.SuperresVersion.FsrcnnX2, DownloadManager_OnDownloadProgressChanged);
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
        public async Task TestDnnMaskRcnn()
        {
            using (Mat m = EmguAssert.LoadMat("pedestrian.png"))
            using (Mat result = new Mat())
            using (Emgu.CV.Models.MaskRcnn detector = new Models.MaskRcnn())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(detector.Initialized, "Failed to initialize the Mask R-CNN model.");
                m.CopyTo(result);
                Models.MaskedObject[] objects = detector.Detect(m);
                EmguAssert.IsTrue(objects.Length > 0, "Mask R-CNN should detect at least one object in the pedestrian image");
                foreach (Models.MaskedObject obj in objects)
                {
                    Trace.WriteLine(String.Format("{0} ({1:F2}) at {2}", obj.Label, obj.Confident, obj.Region));
                    obj.Render(result, detector.RenderColorRectangle, detector.RenderColorMask[obj.ClassId]);
                    obj.Dispose();
                }
                CvInvoke.Imwrite("mask_rcnn_result.jpg", result);
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

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestFaceRecognizerSF()
        {
            using (FaceDetectorYNModel detector = new FaceDetectorYNModel())
            using (FaceRecognizerSFModel recognizer = new FaceRecognizerSFModel())
            using (Mat img1 = EmguAssert.LoadMat("lena.jpg"))
            {
                await detector.Init(AutoTestModels.DownloadManager_OnDownloadProgressChanged);
                await recognizer.Init(AutoTestModels.DownloadManager_OnDownloadProgressChanged);

                // Detect faces in the reference image
                Mat faces1 = new Mat();
                detector.Detect(img1, faces1);
                EmguAssert.IsTrue(faces1.Rows > 0, "No face detected in reference image");

                using (Mat row1 = faces1.Row(0))
                {
                    // Same face against itself — cosine score must be above threshold
                    double scoreSame = recognizer.Match(img1, row1, img1, row1, FaceRecognizerSF.DisType.Cosine);
                    EmguAssert.IsTrue(scoreSame >= 0.363, $"Self-match cosine score too low: {scoreSame}");

                    // Extract feature and round-trip through MatchFeatures
                    using (Mat feature1 = new Mat())
                    using (Mat feature2 = new Mat())
                    {
                        recognizer.Feature(img1, row1, feature1);
                        recognizer.Feature(img1, row1, feature2);
                        double scoreFeature = recognizer.MatchFeatures(feature1, feature2, FaceRecognizerSF.DisType.Cosine);
                        EmguAssert.IsTrue(Math.Abs(scoreSame - scoreFeature) < 1e-4,
                            "MatchFeatures result differs from Match");
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
        public async Task TestAlikedModel()
        {
            using (AlikedModel aliked = new AlikedModel())
            using (Mat img = EmguAssert.LoadMat("box.png", ImreadModes.Grayscale))
            using (VectorOfKeyPoint keypoints = new VectorOfKeyPoint())
            using (Mat descriptors = new Mat())
            {
                await aliked.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(aliked.Initialized, "AlikedModel failed to initialize");
                aliked.DetectAndCompute(img, null, keypoints, descriptors, false);
                EmguAssert.IsTrue(keypoints.Size > 0, "AlikedModel should detect keypoints");
                EmguAssert.IsTrue(descriptors.Rows == keypoints.Size, "One descriptor per keypoint expected");
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
        public async Task TestDiskModel()
        {
            using (DiskModel disk = new DiskModel())
            using (Mat img = EmguAssert.LoadMat("box.png", ImreadModes.Grayscale))
            using (VectorOfKeyPoint keypoints = new VectorOfKeyPoint())
            using (Mat descriptors = new Mat())
            {
                await disk.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(disk.Initialized, "DiskModel failed to initialize");
                disk.DetectAndCompute(img, null, keypoints, descriptors, false);
                EmguAssert.IsTrue(keypoints.Size > 0, "DiskModel should detect keypoints");
                EmguAssert.IsTrue(descriptors.Rows == keypoints.Size, "One descriptor per keypoint expected");
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
        public async Task TestLightGlueMatcherModel()
        {
            using (AlikedModel aliked = new AlikedModel())
            using (LightGlueMatcherModel lightGlue = new LightGlueMatcherModel())
            using (Mat img1 = EmguAssert.LoadMat("box.png", ImreadModes.Grayscale))
            using (Mat img2 = EmguAssert.LoadMat("box_in_scene.png", ImreadModes.Grayscale))
            {
                await aliked.Init(DownloadManager_OnDownloadProgressChanged);
                await lightGlue.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(aliked.Initialized, "AlikedModel failed to initialize");
                EmguAssert.IsTrue(lightGlue.Initialized, "LightGlueMatcherModel failed to initialize");

                MKeyPoint[] kpts1, kpts2;
                using (Mat desc1 = new Mat())
                using (Mat desc2 = new Mat())
                using (VectorOfKeyPoint vkp1 = new VectorOfKeyPoint())
                using (VectorOfKeyPoint vkp2 = new VectorOfKeyPoint())
                {
                    aliked.DetectAndCompute(img1, null, vkp1, desc1, false);
                    aliked.DetectAndCompute(img2, null, vkp2, desc2, false);
                    kpts1 = vkp1.ToArray();
                    kpts2 = vkp2.ToArray();

                    EmguAssert.IsTrue(kpts1.Length > 0, "No keypoints detected in image 1");
                    EmguAssert.IsTrue(kpts2.Length > 0, "No keypoints detected in image 2");

                    using (Mat kptsMat1 = KeyPointsToMat(kpts1))
                    using (Mat kptsMat2 = KeyPointsToMat(kpts2))
                    using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
                    {
                        lightGlue.SetPairInfo(kptsMat1, kptsMat2, img1.Size, img2.Size);
                        lightGlue.KnnMatch(desc1, desc2, matches, 1);
                        EmguAssert.IsTrue(matches.Size > 0, "LightGlueMatcher should return matches");
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
        public async Task TestXFeatModel()
        {
            using (XFeatModel xfeat = new XFeatModel(500, 0.05f))
            using (Mat img = EmguAssert.LoadMat("box.png", ImreadModes.Grayscale))
            using (VectorOfKeyPoint keypoints = new VectorOfKeyPoint())
            using (Mat descriptors = new Mat())
            {
                await xfeat.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(xfeat.Initialized, "XFeatModel failed to initialize");
                EmguAssert.AreEqual(500, xfeat.MaxKeypoints);
                EmguAssert.AreEqual(640, xfeat.InputSize.Width);
                xfeat.DetectAndCompute(img, null, keypoints, descriptors, false);
                EmguAssert.IsTrue(keypoints.Size > 0, "XFeatModel should detect keypoints");
                EmguAssert.IsTrue(keypoints.Size <= 500, "XFeatModel should return at most MaxKeypoints keypoints");
                EmguAssert.IsTrue(descriptors.Rows == keypoints.Size, "One descriptor per keypoint expected");
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
        public async Task TestDiskLightGlueMatcherModel()
        {
            using (DiskModel disk = new DiskModel(2048))
            using (LightGlueMatcherModel lightGlue = new LightGlueMatcherModel(0.0f, Emgu.CV.Dnn.Backend.Default, Emgu.CV.Dnn.Target.Cpu, LightGlueType.Disk))
            using (Mat img1 = EmguAssert.LoadMat("box.png", ImreadModes.Grayscale))
            using (Mat img2 = EmguAssert.LoadMat("box_in_scene.png", ImreadModes.Grayscale))
            {
                await disk.Init(DownloadManager_OnDownloadProgressChanged);
                await lightGlue.Init(DownloadManager_OnDownloadProgressChanged);
                EmguAssert.IsTrue(disk.Initialized, "DiskModel failed to initialize");
                EmguAssert.IsTrue(lightGlue.Initialized, "LightGlueMatcherModel failed to initialize");

                EmguAssert.AreEqual(LightGlueNormalization.Model, lightGlue.KeypointNormalization);
                lightGlue.KeypointNormalization = LightGlueNormalization.MaxSide;
                EmguAssert.AreEqual(LightGlueNormalization.MaxSide, lightGlue.KeypointNormalization);
                lightGlue.KeypointNormalization = LightGlueNormalization.Model;

                using (Mat desc1 = new Mat())
                using (Mat desc2 = new Mat())
                using (VectorOfKeyPoint vkp1 = new VectorOfKeyPoint())
                using (VectorOfKeyPoint vkp2 = new VectorOfKeyPoint())
                {
                    disk.DetectAndCompute(img1, null, vkp1, desc1, false);
                    disk.DetectAndCompute(img2, null, vkp2, desc2, false);
                    MKeyPoint[] kpts1 = vkp1.ToArray();
                    MKeyPoint[] kpts2 = vkp2.ToArray();
                    EmguAssert.IsTrue(kpts1.Length > 0, "No keypoints detected in image 1");
                    EmguAssert.IsTrue(kpts2.Length > 0, "No keypoints detected in image 2");

                    using (Mat kptsMat1 = KeyPointsToMat(kpts1))
                    using (Mat kptsMat2 = KeyPointsToMat(kpts2))
                    using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
                    {
                        lightGlue.SetPairInfo(kptsMat1, kptsMat2, img1.Size, img2.Size);
                        lightGlue.KnnMatch(desc1, desc2, matches, 1);
                        EmguAssert.IsTrue(matches.Size > 0, "DISK LightGlueMatcher should return matches");
                    }
                }
            }
        }

        private static Mat KeyPointsToMat(MKeyPoint[] keypoints)
        {
            Mat m = new Mat(keypoints.Length, 2, CvEnum.DepthType.Cv32F, 1);
            float[] data = new float[keypoints.Length * 2];
            for (int i = 0; i < keypoints.Length; i++)
            {
                data[i * 2] = keypoints[i].Point.X;
                data[i * 2 + 1] = keypoints[i].Point.Y;
            }
            m.SetTo(data);
            return m;
        }
    }
}
