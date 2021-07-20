//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.DepthAI;
using Emgu.CV.Dnn;
using Emgu.Util;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Emgu.CV.Models.DepthAI
{
    /// <summary>
    /// Mobilenet SSD model for depth AI
    /// </summary>
    public class MobilenetSsd
    {
        private static Config GetConfig(String blobFile, String blobConfigFile)
        {
            Config config = new Config();
            config.streams = new List<string>();
            config.streams.Add("metaout");
            config.streams.Add("previewout");
            config.depth = new Depth();
            config.depth.calibration_file = String.Empty;
            config.depth.left_mesh_file = System.IO.Path.Combine(Assembly.GetCallingAssembly().Location, "resources", "mesh_left.calib");
            config.depth.right_mesh_file = System.IO.Path.Combine(Assembly.GetCallingAssembly().Location, "resources", "mesh_right.calib");
            config.depth.padding_factor = 0.3;
            config.depth.depth_limit_m = 10.0;
            config.depth.median_kernel_size = 7;
            config.depth.lr_check = false;
            config.depth.warp_rectify = new WarpRectify();
            config.depth.warp_rectify.use_mesh = false;
            config.depth.warp_rectify.mirror_frame = true;
            config.depth.warp_rectify.edge_fill_color = 0;
            config.ai = new Ai();
            config.ai.blob_file = blobFile;
            config.ai.blob_file_config = blobConfigFile;
            config.ai.blob_file2 = String.Empty;
            config.ai.blob_file_config2 = String.Empty;
            config.ai.calc_dist_to_bb = true;
            config.ai.keep_aspect_ratio = true;
            config.ai.camera_input = "rgb";
            config.ai.shaves = 14;
            config.ai.cmx_slices = 14;
            config.ai.NN_engines = 1;
            config.ot = new Ot();
            config.ot.max_tracklets = 20;
            config.ot.confidence_threshold = 0.5;
            config.board_config = new BoardConfig();
            config.board_config.swap_left_and_right_cameras = true;
            config.board_config.left_fov_deg = 71.86;
            config.board_config.rgb_fov_deg = 68.7938;
            config.board_config.left_to_right_distance_cm = 9.0;
            config.board_config.left_to_rgb_distance_cm = 2.0;
            config.board_config.store_to_eeprom = false;
            config.board_config.clear_eeprom = false;
            config.board_config.override_eeprom = false;
            config.camera = new Camera();
            config.camera.rgb = new Camera.Setting();
            config.camera.rgb.resolution_h = 1080;
            config.camera.rgb.fps = 30.0;
            config.camera.mono = new Camera.Setting();
            config.camera.mono.resolution_h = 720;
            config.camera.mono.fps = 30.0;
            config.app = new App();
            config.app.sync_video_meta_streams = false;
            config.app.sync_sequence_numbers = false;
            config.app.usb_chunk_KiB = 64;
            return config;
        }

        private String _modelFolderName = "DaiMobilenetSSD";

        private DownloadableFile _blobFile;
        private DownloadableFile _blobConfigFile;
        private FileDownloadManager _manager;

        /// <summary>
        /// Download and initialize the mobilenet model for depth AI
        /// </summary>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <returns>Asyn task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            _blobFile = new DownloadableFile(
                "https://github.com/emgucv/models/raw/master/DepthAI/mobilenet-ssd/mobilenet-ssd.blob.sh14cmx14NCE1",
                _modelFolderName,
                "952C8AA1759CAB442D82781579EE4B9BA828CBED8AB9AD3C94D5222AA45DCA6E");
            _blobConfigFile = new DownloadableFile(
                "https://github.com/emgucv/models/raw/master/DepthAI/mobilenet-ssd/mobilenet-ssd.json",
                _modelFolderName,
                "606A965DDF539857D3477AED659A69948D8B310A20B38C3B794F2369ECC685FE");

            if (_manager == null)
            {
                _manager = new FileDownloadManager();
            }
            else
            {
                _manager.Clear();
            }

            _manager.AddFile(_blobFile);
            _manager.AddFile(_blobConfigFile);
            if (onDownloadProgressChanged != null)
                    _manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return _manager.Download();
#else
            await _manager.Download();
#endif

        }

        public Config ModelConfig
        {
            get
            {
                if (!_manager.AllFilesDownloaded)
                    return null;

                Config config = GetConfig(_blobFile.LocalFile, _blobConfigFile.LocalFile);
                return config;
            }
        }

        private static String[] ReadLabels(String blobConfigFile)
        {
            using (StreamReader sr = new StreamReader(blobConfigFile))
            {
                JObject jObject = JObject.Parse(sr.ReadToEnd());
                List<String> labels = new List<string>();
                if (jObject.ContainsKey("mappings"))
                {
                    var mappings = jObject["mappings"];
                    var token = mappings["labels"];
                    foreach (var l in token)
                        labels.Add(l.ToString());
                }
                
                return labels.ToArray();
            }
        }

        /// <summary>
        /// The labels for the detection
        /// </summary>
        public String[] Labels
        {
            get
            {
                if (_manager == null)
                    return null;
                if (!_manager.AllFilesDownloaded)
                    return null;
                
                return ReadLabels(_blobConfigFile.LocalFile);
            }
        }

    }
}
