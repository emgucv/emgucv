//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.DepthAI;

namespace Emgu.CV.Models.DepthAI
{
    public class MobilenetSsd
    {
        public static Config GetConfig(String blobFile, String blobFileConfig)
        {
            Config config = new Config();
            config.streams = new List<string>();
            config.streams.Add("metaout");
            config.streams.Add("previewout");
            config.depth = new Depth();
            config.depth.calibration_file = String.Empty;
            config.depth.left_mesh_file = System.IO.Path.Combine( Assembly.GetCallingAssembly().Location, "resources", "mesh_left.calib" );
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
            config.ai.blob_file =
                "D:\\sourceforge\\depthai\\resources\\nn\\mobilenet-ssd\\mobilenet-ssd.blob.sh14cmx14NCE1";
            config.ai.blob_file_config = "D:\\sourceforge\\depthai\\resources\\nn\\mobilenet-ssd\\mobilenet-ssd.json";
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

        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            /*
            if (_yoloDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                if (version == YoloVersion.YoloV3Spp)
                {
                    manager.AddFile(
                        "https://pjreddie.com/media/files/yolov3-spp.weights",
                        _modelFolderName);
                    manager.AddFile(
                        "https://github.com/pjreddie/darknet/raw/master/cfg/yolov3-spp.cfg",
                        _modelFolderName);
                }
                else if (version == YoloVersion.YoloV3)
                {
                    manager.AddFile(
                        "https://pjreddie.com/media/files/yolov3.weights",
                        _modelFolderName);
                    manager.AddFile(
                        "https://github.com/pjreddie/darknet/raw/master/cfg/yolov3.cfg",
                        _modelFolderName);
                }
                else if (version == YoloVersion.YoloV3Tiny)
                {
                    manager.AddFile(
                        "https://pjreddie.com/media/files/yolov3-tiny.weights",
                        _modelFolderName);
                    manager.AddFile(
                        "https://github.com/pjreddie/darknet/raw/master/cfg/yolov3-tiny.cfg",
                        _modelFolderName);
                }

                manager.AddFile("https://github.com/pjreddie/darknet/raw/master/data/coco.names",
                    _modelFolderName);

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
                await manager.Download();
                _yoloDetector = DnnInvoke.ReadNetFromDarknet(manager.Files[1].LocalFile, manager.Files[0].LocalFile);
                _labels = File.ReadAllLines(manager.Files[2].LocalFile);
            }*/
        }

    }
}
