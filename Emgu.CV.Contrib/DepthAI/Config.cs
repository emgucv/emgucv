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

    /// <summary>
    /// Warp Rectify
    /// </summary>
    public class WarpRectify
    {
        /// <summary>
        /// Use mesh
        /// </summary>
        public bool use_mesh { get; set; }
        /// <summary>
        /// Mirror frame
        /// </summary>
        public bool mirror_frame { get; set; }
        /// <summary>
        /// Edge fill color
        /// </summary>
        public int edge_fill_color { get; set; }
    }

    /// <summary>
    /// Depth
    /// </summary>
    public class Depth
    {
        /// <summary>
        /// Calibration file
        /// </summary>
        public string calibration_file { get; set; }
        /// <summary>
        /// Left mesh file
        /// </summary>
        public string left_mesh_file { get; set; }
        /// <summary>
        /// Right mesh file
        /// </summary>
        public string right_mesh_file { get; set; }
        /// <summary>
        /// Padding factor
        /// </summary>
        public double padding_factor { get; set; }
        /// <summary>
        /// depth limit m
        /// </summary>
        public double depth_limit_m { get; set; }
        /// <summary>
        /// Median kernel size
        /// </summary>
        public int median_kernel_size { get; set; }
        /// <summary>
        /// Lr check
        /// </summary>
        public bool lr_check { get; set; }
        /// <summary>
        /// Warp rectify
        /// </summary>
        public WarpRectify warp_rectify { get; set; }
    }

    /// <summary>
    /// AI
    /// </summary>
    public class Ai
    {
        /// <summary>
        /// Blob file
        /// </summary>
        public string blob_file { get; set; }
        /// <summary>
        /// Blob file config
        /// </summary>
        public string blob_file_config { get; set; }
        /// <summary>
        /// Second blob file
        /// </summary>
        public string blob_file2 { get; set; }
        /// <summary>
        /// Second blob file config
        /// </summary>
        public string blob_file_config2 { get; set; }
        /// <summary>
        /// Calculate distance to bb
        /// </summary>
        public bool calc_dist_to_bb { get; set; }
        /// <summary>
        /// Keep aspect ratio
        /// </summary>
        public bool keep_aspect_ratio { get; set; }
        /// <summary>
        /// Camera input
        /// </summary>
        public string camera_input { get; set; }
        /// <summary>
        /// Shaves
        /// </summary>
        public int shaves { get; set; }
        /// <summary>
        /// CMX slices
        /// </summary>
        public int cmx_slices { get; set; }
        /// <summary>
        /// NN Engine
        /// </summary>
        public int NN_engines { get; set; }
    }

    /// <summary>
    /// OT
    /// </summary>
    public class Ot
    {
        /// <summary>
        /// Maximum tracklets
        /// </summary>
        public int max_tracklets { get; set; }
        /// <summary>
        /// Confident threshold
        /// </summary>
        public double confidence_threshold { get; set; }
    }

    /// <summary>
    /// Board config
    /// </summary>
    public class BoardConfig
    {
        /// <summary>
        /// Swap left and right cameras
        /// </summary>
        public bool swap_left_and_right_cameras { get; set; }
        /// <summary>
        /// Left FOV in degree
        /// </summary>
        public double left_fov_deg { get; set; }
        /// <summary>
        /// RGB FOV in degree
        /// </summary>
        public double rgb_fov_deg { get; set; }
        /// <summary>
        /// Left to right distance in cm
        /// </summary>
        public double left_to_right_distance_cm { get; set; }
        /// <summary>
        /// Left to RGB distance in cm
        /// </summary>
        public double left_to_rgb_distance_cm { get; set; }
        /// <summary>
        /// Store to eeprom
        /// </summary>
        public bool store_to_eeprom { get; set; }
        /// <summary>
        /// Clear eeprom
        /// </summary>
        public bool clear_eeprom { get; set; }
        /// <summary>
        /// Override eeprom
        /// </summary>
        public bool override_eeprom { get; set; }
    }

    /// <summary>
    /// Camera
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Create a new Camera config
        /// </summary>
        /// <param name="rgbSetting">RGB setting</param>
        /// <param name="monoSetting">Mono setting</param>
        public Camera(Setting rgbSetting = null, Setting monoSetting = null)
        {
            if (rgbSetting == null)
            {
                rgbSetting = new Setting { resolution_h = 1080, fps = 30.0 };
            }

            if (monoSetting == null)
            {
                monoSetting = new Setting { resolution_h = 720, fps = 30.0 };
            }
        }

        /// <summary>
        /// Camera setting
        /// </summary>
        public class Setting
        {
            /// <summary>
            /// Horizontal resolution
            /// </summary>
            public int resolution_h { get; set; }
            /// <summary>
            /// FPS
            /// </summary>
            public double fps { get; set; }
        }

        /// <summary>
        /// RGB setting
        /// </summary>
        public Setting rgb { get; set; }

        /// <summary>
        /// Mono setting
        /// </summary>
        public Setting mono { get; set; }
    }


    /// <summary>
    /// App
    /// </summary>
    public class App
    {
        /// <summary>
        /// Sync video meta streams
        /// </summary>
        public bool sync_video_meta_streams { get; set; }
        /// <summary>
        /// Sync sequence numbers
        /// </summary>
        public bool sync_sequence_numbers { get; set; }
        /// <summary>
        /// Usb chunk KiB
        /// </summary>
        public int usb_chunk_KiB { get; set; }
    }

    /// <summary>
    /// Config
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Streams
        /// </summary>
        public List<string> streams { get; set; }

        /// <summary>
        /// Depth
        /// </summary>
        public Depth depth { get; set; }

        /// <summary>
        /// Ai
        /// </summary>
        public Ai ai { get; set; }

        /// <summary>
        /// Ot
        /// </summary>
        public Ot ot { get; set; }

        /// <summary>
        /// Board config
        /// </summary>
        public BoardConfig board_config { get; set; }

        /// <summary>
        /// Camera
        /// </summary>
        public Camera camera { get; set; }

        /// <summary>
        /// App
        /// </summary>
        public App app { get; set; }
    }

}