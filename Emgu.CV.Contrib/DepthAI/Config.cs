//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.DepthAI
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class WarpRectify
    {
        public bool use_mesh { get; set; }
        public bool mirror_frame { get; set; }
        public int edge_fill_color { get; set; }
    }

    public class Depth
    {
        public string calibration_file { get; set; }
        public string left_mesh_file { get; set; }
        public string right_mesh_file { get; set; }
        public double padding_factor { get; set; }
        public double depth_limit_m { get; set; }
        public int median_kernel_size { get; set; }
        public bool lr_check { get; set; }
        public WarpRectify warp_rectify { get; set; }
    }

    public class Ai
    {
        public string blob_file { get; set; }
        public string blob_file_config { get; set; }
        public string blob_file2 { get; set; }
        public string blob_file_config2 { get; set; }
        public bool calc_dist_to_bb { get; set; }
        public bool keep_aspect_ratio { get; set; }
        public string camera_input { get; set; }
        public int shaves { get; set; }
        public int cmx_slices { get; set; }
        public int NN_engines { get; set; }
    }

    public class Ot
    {
        public int max_tracklets { get; set; }
        public double confidence_threshold { get; set; }
    }

    public class BoardConfig
    {
        public bool swap_left_and_right_cameras { get; set; }
        public double left_fov_deg { get; set; }
        public double rgb_fov_deg { get; set; }
        public double left_to_right_distance_cm { get; set; }
        public double left_to_rgb_distance_cm { get; set; }
        public bool store_to_eeprom { get; set; }
        public bool clear_eeprom { get; set; }
        public bool override_eeprom { get; set; }
    }

    public class Camera
    {
        public Camera(Setting rgbSetting = null, Setting monoSetting = null)
        {
            if (rgbSetting == null)
            {
                rgbSetting = new Setting {resolution_h = 1080, fps = 30.0};
            }

            if (monoSetting == null)
            {
                monoSetting = new Setting {resolution_h = 720, fps = 30.0};
            }
        }


        public class Setting
        {
            public int resolution_h { get; set; }
            public double fps { get; set; }
        }

        public Setting rgb { get; set; }
        public Setting mono { get; set; }
    }

    public class App
    {
        public bool sync_video_meta_streams { get; set; }
        public bool sync_sequence_numbers { get; set; }
        public int usb_chunk_KiB { get; set; }
    }

    public class Config
    {
        public List<string> streams { get; set; }
        public Depth depth { get; set; }
        public Ai ai { get; set; }
        public Ot ot { get; set; }
        public BoardConfig board_config { get; set; }
        public Camera camera { get; set; }
        public App app { get; set; }
    }



}