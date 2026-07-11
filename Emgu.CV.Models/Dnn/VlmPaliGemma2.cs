//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// PaliGemma2 vision-language model. Given an image and a text prompt, it
    /// generates a text response (e.g. a caption). The model is split into three
    /// ONNX files: the SigLIP vision encoder (image -> 256 image-feature
    /// tokens), the embedding network (prompt token ids -> text embeddings) and
    /// the Gemma2 language model ([image_features | text_embeds] -> logits).
    /// All three networks run on the new dnn engine.
    ///
    /// Init downloads the model files (~15 GB across ~300 files) into the model
    /// folder: the ONNX models from
    /// https://huggingface.co/nklskyoy/paligemma2-3b-pt-224-onnx and the
    /// standard Gemma-2 tokenizer.json from an ungated mirror of the gated
    /// PaliGemma2 repository. Files already present in the folder are not
    /// downloaded again, so a pre-populated folder (e.g. an absolute path
    /// passed to the constructor) is loaded as-is.
    /// </summary>
    public class VlmPaliGemma2 : DisposableObject
    {
        private const int EosId = 1;

        private const String BaseUrl = "https://huggingface.co/nklskyoy/paligemma2-3b-pt-224-onnx/resolve/main/";

        /// <summary>
        /// The external data files referenced by gemma2_3b.onnx; they must reside next to it.
        /// </summary>
        private static readonly String[] GemmaExternalDataFiles = new String[]
        {
            "onnx__MatMul_8880",
            "onnx__MatMul_8887",
            "onnx__MatMul_8888",
            "onnx__MatMul_8911",
            "onnx__MatMul_8916",
            "onnx__MatMul_8917",
            "onnx__MatMul_8918",
            "onnx__MatMul_8923",
            "onnx__MatMul_8930",
            "onnx__MatMul_8931",
            "onnx__MatMul_8954",
            "onnx__MatMul_8959",
            "onnx__MatMul_8960",
            "onnx__MatMul_8961",
            "onnx__MatMul_8966",
            "onnx__MatMul_8973",
            "onnx__MatMul_8974",
            "onnx__MatMul_8997",
            "onnx__MatMul_9002",
            "onnx__MatMul_9003",
            "onnx__MatMul_9004",
            "onnx__MatMul_9009",
            "onnx__MatMul_9016",
            "onnx__MatMul_9017",
            "onnx__MatMul_9040",
            "onnx__MatMul_9045",
            "onnx__MatMul_9046",
            "onnx__MatMul_9047",
            "onnx__MatMul_9052",
            "onnx__MatMul_9059",
            "onnx__MatMul_9060",
            "onnx__MatMul_9083",
            "onnx__MatMul_9088",
            "onnx__MatMul_9089",
            "onnx__MatMul_9090",
            "onnx__MatMul_9095",
            "onnx__MatMul_9102",
            "onnx__MatMul_9103",
            "onnx__MatMul_9126",
            "onnx__MatMul_9131",
            "onnx__MatMul_9132",
            "onnx__MatMul_9133",
            "onnx__MatMul_9138",
            "onnx__MatMul_9145",
            "onnx__MatMul_9146",
            "onnx__MatMul_9169",
            "onnx__MatMul_9174",
            "onnx__MatMul_9175",
            "onnx__MatMul_9176",
            "onnx__MatMul_9181",
            "onnx__MatMul_9188",
            "onnx__MatMul_9189",
            "onnx__MatMul_9212",
            "onnx__MatMul_9217",
            "onnx__MatMul_9218",
            "onnx__MatMul_9219",
            "onnx__MatMul_9224",
            "onnx__MatMul_9231",
            "onnx__MatMul_9232",
            "onnx__MatMul_9255",
            "onnx__MatMul_9260",
            "onnx__MatMul_9261",
            "onnx__MatMul_9262",
            "onnx__MatMul_9267",
            "onnx__MatMul_9274",
            "onnx__MatMul_9275",
            "onnx__MatMul_9298",
            "onnx__MatMul_9303",
            "onnx__MatMul_9304",
            "onnx__MatMul_9305",
            "onnx__MatMul_9310",
            "onnx__MatMul_9317",
            "onnx__MatMul_9318",
            "onnx__MatMul_9341",
            "onnx__MatMul_9346",
            "onnx__MatMul_9347",
            "onnx__MatMul_9348",
            "onnx__MatMul_9353",
            "onnx__MatMul_9360",
            "onnx__MatMul_9361",
            "onnx__MatMul_9384",
            "onnx__MatMul_9389",
            "onnx__MatMul_9390",
            "onnx__MatMul_9391",
            "onnx__MatMul_9396",
            "onnx__MatMul_9403",
            "onnx__MatMul_9404",
            "onnx__MatMul_9427",
            "onnx__MatMul_9432",
            "onnx__MatMul_9433",
            "onnx__MatMul_9434",
            "onnx__MatMul_9439",
            "onnx__MatMul_9446",
            "onnx__MatMul_9447",
            "onnx__MatMul_9470",
            "onnx__MatMul_9475",
            "onnx__MatMul_9476",
            "onnx__MatMul_9477",
            "onnx__MatMul_9482",
            "onnx__MatMul_9489",
            "onnx__MatMul_9490",
            "onnx__MatMul_9513",
            "onnx__MatMul_9518",
            "onnx__MatMul_9519",
            "onnx__MatMul_9520",
            "onnx__MatMul_9525",
            "onnx__MatMul_9532",
            "onnx__MatMul_9533",
            "onnx__MatMul_9556",
            "onnx__MatMul_9561",
            "onnx__MatMul_9562",
            "onnx__MatMul_9563",
            "onnx__MatMul_9568",
            "onnx__MatMul_9575",
            "onnx__MatMul_9576",
            "onnx__MatMul_9599",
            "onnx__MatMul_9604",
            "onnx__MatMul_9605",
            "onnx__MatMul_9606",
            "onnx__MatMul_9611",
            "onnx__MatMul_9618",
            "onnx__MatMul_9619",
            "onnx__MatMul_9642",
            "onnx__MatMul_9647",
            "onnx__MatMul_9648",
            "onnx__MatMul_9649",
            "onnx__MatMul_9654",
            "onnx__MatMul_9661",
            "onnx__MatMul_9662",
            "onnx__MatMul_9685",
            "onnx__MatMul_9690",
            "onnx__MatMul_9691",
            "onnx__MatMul_9692",
            "onnx__MatMul_9697",
            "onnx__MatMul_9704",
            "onnx__MatMul_9705",
            "onnx__MatMul_9728",
            "onnx__MatMul_9733",
            "onnx__MatMul_9734",
            "onnx__MatMul_9735",
            "onnx__MatMul_9740",
            "onnx__MatMul_9747",
            "onnx__MatMul_9748",
            "onnx__MatMul_9771",
            "onnx__MatMul_9776",
            "onnx__MatMul_9777",
            "onnx__MatMul_9778",
            "onnx__MatMul_9783",
            "onnx__MatMul_9790",
            "onnx__MatMul_9791",
            "onnx__MatMul_9814",
            "onnx__MatMul_9819",
            "onnx__MatMul_9820",
            "onnx__MatMul_9821",
            "onnx__MatMul_9826",
            "onnx__MatMul_9833",
            "onnx__MatMul_9834",
            "onnx__MatMul_9857",
            "onnx__MatMul_9862",
            "onnx__MatMul_9863",
            "onnx__MatMul_9864",
            "onnx__MatMul_9869",
            "onnx__MatMul_9876",
            "onnx__MatMul_9877",
            "onnx__MatMul_9900",
            "onnx__MatMul_9905",
            "onnx__MatMul_9906",
            "onnx__MatMul_9907",
            "onnx__MatMul_9912",
            "onnx__MatMul_9919",
            "onnx__MatMul_9920",
            "onnx__MatMul_9943",
            "onnx__MatMul_9948",
            "onnx__MatMul_9949",
            "onnx__MatMul_9950",
            "onnx__MatMul_9955",
            "onnx__MatMul_9962",
            "onnx__MatMul_9963",
            "onnx__MatMul_9986",
            "onnx__MatMul_9991",
            "onnx__MatMul_9992",
            "onnx__MatMul_9993",
            "onnx__MatMul_9998",
            "onnx__Mul_8879",
            "onnx__Mul_8913",
            "onnx__Mul_8915",
            "onnx__Mul_8920",
            "onnx__Mul_8922",
            "onnx__Mul_8956",
            "onnx__Mul_8958",
            "onnx__Mul_8963",
            "onnx__Mul_8965",
            "onnx__Mul_8999",
            "onnx__Mul_9001",
            "onnx__Mul_9006",
            "onnx__Mul_9008",
            "onnx__Mul_9042",
            "onnx__Mul_9044",
            "onnx__Mul_9049",
            "onnx__Mul_9051",
            "onnx__Mul_9085",
            "onnx__Mul_9087",
            "onnx__Mul_9092",
            "onnx__Mul_9094",
            "onnx__Mul_9128",
            "onnx__Mul_9130",
            "onnx__Mul_9135",
            "onnx__Mul_9137",
            "onnx__Mul_9171",
            "onnx__Mul_9173",
            "onnx__Mul_9178",
            "onnx__Mul_9180",
            "onnx__Mul_9214",
            "onnx__Mul_9216",
            "onnx__Mul_9221",
            "onnx__Mul_9223",
            "onnx__Mul_9257",
            "onnx__Mul_9259",
            "onnx__Mul_9264",
            "onnx__Mul_9266",
            "onnx__Mul_9300",
            "onnx__Mul_9302",
            "onnx__Mul_9307",
            "onnx__Mul_9309",
            "onnx__Mul_9343",
            "onnx__Mul_9345",
            "onnx__Mul_9350",
            "onnx__Mul_9352",
            "onnx__Mul_9386",
            "onnx__Mul_9388",
            "onnx__Mul_9393",
            "onnx__Mul_9395",
            "onnx__Mul_9429",
            "onnx__Mul_9431",
            "onnx__Mul_9436",
            "onnx__Mul_9438",
            "onnx__Mul_9472",
            "onnx__Mul_9474",
            "onnx__Mul_9479",
            "onnx__Mul_9481",
            "onnx__Mul_9515",
            "onnx__Mul_9517",
            "onnx__Mul_9522",
            "onnx__Mul_9524",
            "onnx__Mul_9558",
            "onnx__Mul_9560",
            "onnx__Mul_9565",
            "onnx__Mul_9567",
            "onnx__Mul_9601",
            "onnx__Mul_9603",
            "onnx__Mul_9608",
            "onnx__Mul_9610",
            "onnx__Mul_9644",
            "onnx__Mul_9646",
            "onnx__Mul_9651",
            "onnx__Mul_9653",
            "onnx__Mul_9687",
            "onnx__Mul_9689",
            "onnx__Mul_9694",
            "onnx__Mul_9696",
            "onnx__Mul_9730",
            "onnx__Mul_9732",
            "onnx__Mul_9737",
            "onnx__Mul_9739",
            "onnx__Mul_9773",
            "onnx__Mul_9775",
            "onnx__Mul_9780",
            "onnx__Mul_9782",
            "onnx__Mul_9816",
            "onnx__Mul_9818",
            "onnx__Mul_9823",
            "onnx__Mul_9825",
            "onnx__Mul_9859",
            "onnx__Mul_9861",
            "onnx__Mul_9866",
            "onnx__Mul_9868",
            "onnx__Mul_9902",
            "onnx__Mul_9904",
            "onnx__Mul_9909",
            "onnx__Mul_9911",
            "onnx__Mul_9945",
            "onnx__Mul_9947",
            "onnx__Mul_9952",
            "onnx__Mul_9954",
            "onnx__Mul_9988",
            "onnx__Mul_9990",
            "onnx__Mul_9995",
            "onnx__Mul_9997",
        };

        private readonly String _modelFolderName;

        private Tokenizer _tokenizer = null;
        private Net _siglipNet = null;
        private Net _embedNet = null;
        private Net _gemmaNet = null;

        /// <summary>
        /// Create a PaliGemma2 vision-language model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again, so a pre-populated folder is loaded as-is.</param>
        public VlmPaliGemma2(String modelFolderName = null)
        {
            _modelFolderName = modelFolderName ?? Path.Combine("emgu", "paligemma2_3b_pt_224_onnx");
        }

        /// <summary>
        /// Return true if the model is initialized.
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _tokenizer != null && _siglipNet != null && _embedNet != null && _gemmaNet != null;
            }
        }

        private void LoadModels(String modelFolder)
        {
            //The tokenizer config is tiny and fixed, so it is written locally
            //instead of downloaded. The "SentencePiece" method prepends the
            //<bos> token on encode like the HuggingFace Gemma tokenizer does;
            //without <bos> the model generates a degenerate response.
            String configPath = Path.Combine(modelFolder, "config.json");
            if (!File.Exists(configPath))
                File.WriteAllText(configPath, "{ \"method\": \"SentencePiece\" }");

            if (_tokenizer == null)
                _tokenizer = Tokenizer.Load(configPath);
            if (_siglipNet == null)
                _siglipNet = DnnInvoke.ReadNetFromONNX(Path.Combine(modelFolder, "vision_model.onnx"), EngineType.New);
            if (_embedNet == null)
                _embedNet = DnnInvoke.ReadNetFromONNX(Path.Combine(modelFolder, "embedding.onnx"), EngineType.New);
            if (_gemmaNet == null)
                _gemmaNet = DnnInvoke.ReadNetFromONNX(Path.Combine(modelFolder, "gemma2_3b.onnx"), EngineType.New);
        }

        private FileDownloadManager CreateDownloadManager()
        {
            FileDownloadManager manager = new FileDownloadManager();

            //The Gemma-2 tokenizer from an ungated mirror of the gated
            //PaliGemma2 repository.
            manager.AddFile(
                "https://huggingface.co/unsloth/gemma-2-2b/resolve/main/tokenizer.json",
                _modelFolderName,
                "3F289BC05132635A8BC7ACA7AA21255EFD5E18F3710F43E3CDB96BCD41BE4922");

            manager.AddFile(BaseUrl + "siglip/onnx/vision_model.onnx", _modelFolderName);
            manager.AddFile(BaseUrl + "embedding/onnx/embedding.onnx", _modelFolderName);
            manager.AddFile(BaseUrl + "embedding/onnx/embedding.weight", _modelFolderName);
            manager.AddFile(BaseUrl + "gemma/onnx/gemma2_3b.onnx", _modelFolderName);
            foreach (String externalDataFile in GemmaExternalDataFiles)
                manager.AddFile(BaseUrl + "gemma/onnx/" + externalDataFile, _modelFolderName);

            return manager;
        }

        /// <summary>
        /// Download the model files (~15 GB, skipping the files already present
        /// in the model folder) and load the tokenizer and the three ONNX
        /// networks.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (!Initialized)
            {
                FileDownloadManager manager = CreateDownloadManager();
                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
                yield return manager.Download();
                if (manager.AllFilesDownloaded)
                    LoadModels(Path.GetDirectoryName(manager.Files[0].LocalFile));
            }
        }
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (!Initialized)
            {
                FileDownloadManager manager = CreateDownloadManager();
                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
                await manager.Download();
                if (manager.AllFilesDownloaded)
                    await Task.Run(() => LoadModels(Path.GetDirectoryName(manager.Files[0].LocalFile)));
            }
        }
#endif

        /// <summary>
        /// Run the embedding network on the given token ids, returning the (1, count, hidden) embeddings.
        /// </summary>
        private Mat ForwardEmbedding(int[] tokenIds)
        {
            long[] ids = new long[tokenIds.Length];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = tokenIds[i];
            GCHandle handle = GCHandle.Alloc(ids, GCHandleType.Pinned);
            try
            {
                using (Mat inputIds = new Mat(new int[] { 1, ids.Length }, DepthType.Cv64S, handle.AddrOfPinnedObject()))
                {
                    _embedNet.SetInput(inputIds, "input_ids");
                    return _embedNet.Forward();
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
        private static Mat TensorAsRows(Mat tensor)
        {
            int[] shape = tensor.SizeOfDimension;
            return new Mat(new int[] { shape[1], shape[2] }, tensor.Depth, tensor.DataPointer);
        }

        /// <summary>
        /// Run the language model on the (sequence, hidden) embeddings and
        /// return the argmax token id of the last sequence position.
        /// </summary>
        private int ForwardLanguageModel(Mat inputsEmbeds2D)
        {
            using (Mat inputsEmbeds = new Mat(
                new int[] { 1, inputsEmbeds2D.Rows, inputsEmbeds2D.Cols },
                DepthType.Cv32F,
                inputsEmbeds2D.DataPointer))
            {
                _gemmaNet.SetInput(inputsEmbeds, "inputs_embeds");
                using (Mat logits = _gemmaNet.Forward())
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

        /// <summary>
        /// Generate a text response for the given image and prompt.
        /// </summary>
        /// <param name="image">The input image.</param>
        /// <param name="prompt">The task prompt, e.g. "cap en\n" to caption in English.</param>
        /// <param name="maxNewTokens">Maximum number of new tokens to generate.</param>
        /// <returns>The generated text response.</returns>
        public String Generate(IInputArray image, String prompt = "cap en\n", int maxNewTokens = 64)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

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
                _siglipNet.SetInput(pixelValues, "pixel_values");
                using (Mat imageFeatures = _siglipNet.Forward())
                {
                    List<int> generated = new List<int>();

                    //The growing (sequence, hidden) embedding matrix; the
                    //(1, sequence, hidden) inputs_embeds tensor without the
                    //leading batch-1 dimension.
                    Mat inputsEmbeds = new Mat();
                    try
                    {
                        //Combine [image_features | text_embeds]
                        int[] tokens = _tokenizer.Encode(prompt);
                        using (Mat textEmbeds = ForwardEmbedding(tokens))
                        using (Mat imageFeatures2D = TensorAsRows(imageFeatures))
                        using (Mat textEmbeds2D = TensorAsRows(textEmbeds))
                        {
                            CvInvoke.VConcat(imageFeatures2D, textEmbeds2D, inputsEmbeds);
                        }

                        //Prefill
                        int newId = ForwardLanguageModel(inputsEmbeds);
                        generated.Add(newId);

                        //Decode (no KV-cache: feed the full growing sequence each step)
                        for (int i = 0; i < maxNewTokens - 1; i++)
                        {
                            if (newId == EosId)
                                break;

                            using (Mat newEmbed = ForwardEmbedding(new int[] { newId }))
                            using (Mat newEmbed2D = TensorAsRows(newEmbed))
                            {
                                Mat grown = new Mat();
                                CvInvoke.VConcat(inputsEmbeds, newEmbed2D, grown);
                                inputsEmbeds.Dispose();
                                inputsEmbeds = grown;
                            }

                            newId = ForwardLanguageModel(inputsEmbeds);
                            generated.Add(newId);
                        }
                    }
                    finally
                    {
                        inputsEmbeds.Dispose();
                    }

                    if (generated.Count > 0 && generated[generated.Count - 1] == EosId)
                        generated.RemoveAt(generated.Count - 1);

                    return _tokenizer.Decode(generated.ToArray());
                }
            }
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling Generate.
        /// </summary>
        public void Clear()
        {
            if (_tokenizer != null)
            {
                _tokenizer.Dispose();
                _tokenizer = null;
            }

            if (_siglipNet != null)
            {
                _siglipNet.Dispose();
                _siglipNet = null;
            }

            if (_embedNet != null)
            {
                _embedNet.Dispose();
                _embedNet = null;
            }

            if (_gemmaNet != null)
            {
                _gemmaNet.Dispose();
                _gemmaNet = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this vision language model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }
    }
}
