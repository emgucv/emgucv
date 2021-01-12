//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// CV Capture property identifier
    /// </summary>
    public enum CapProp
    {
        /// <summary>
        /// Turn the feature off (not controlled manually nor automatically)
        /// </summary>
        DC1394Off = -4,
        /// <summary>
        /// Set automatically when a value of the feature is set by the user
        /// </summary>
        DC1394ModeManual = -3,
        /// <summary>
        /// DC1394 mode auto
        /// </summary>
        DC1394ModeAuto = -2,
        /// <summary>
        /// DC1394 mode one push auto
        /// </summary>
        DC1394ModeOnePushAuto = -1,
        /// <summary>
        /// Film current position in milliseconds or video capture timestamp
        /// </summary>
        PosMsec = 0,
        /// <summary>
        /// 0-based index of the frame to be decoded/captured next
        /// </summary>
        PosFrames = 1,
        /// <summary>
        /// Relative position of the video file: 0=start of the film, 1=end of the film.
        /// </summary>
        PosAviRatio = 2,
        /// <summary>
        /// Width of frames in the video stream
        /// </summary>
        FrameWidth = 3,
        /// <summary>
        /// Height of frames in the video stream
        /// </summary>
        FrameHeight = 4,
        /// <summary>
        /// Frame rate 
        /// </summary>
        Fps = 5,
        /// <summary>
        /// 4-character code of codec
        /// </summary>
        FourCC = 6,
        /// <summary>
        /// Number of frames in video file
        /// </summary>
        FrameCount = 7,
        /// <summary>
        /// Format of the %Mat objects returned by VideoCapture::retrieve().
        /// </summary>
        Format = 8,
        /// <summary>
        /// Backend-specific value indicating the current capture mode.
        /// </summary>
        Mode = 9,
        /// <summary>
        /// Brightness of the image (only for those cameras that support).
        /// </summary>
        Brightness = 10,
        /// <summary>
        /// Contrast of the image (only for cameras).
        /// </summary>
        Contrast = 11,
        /// <summary>
        /// Saturation of the image (only for cameras).
        /// </summary>
        Saturation = 12,
        /// <summary>
        /// Hue of the image (only for cameras).
        /// </summary>
        Hue = 13,
        /// <summary>
        /// Gain of the image (only for those cameras that support).
        /// </summary>
        Gain = 14,
        /// <summary>
        /// Exposure (only for those cameras that support).
        /// </summary>
        Exposure = 15,
        /// <summary>
        /// Boolean flags indicating whether images should be converted to RGB.
        /// </summary>
        ConvertRgb = 16,
        /// <summary>
        /// Currently unsupported.
        /// </summary>
        WhiteBalanceBlueU = 17,
        /// <summary>
        /// Rectification flag for stereo cameras (note: only supported by DC1394 v 2.x backend currently).
        /// </summary>
        Rectification = 18,
        /// <summary>
        /// Monochrome
        /// </summary>
        Monochrome = 19,
        /// <summary>
        /// Sharpness
        /// </summary>
        Sharpness = 20,
        /// <summary>
        /// Exposure control done by camera, user can adjust reference level using this feature
        /// </summary>
        AutoExposure = 21,
        /// <summary>
        /// Gamma
        /// </summary>
        Gamma = 22,
        /// <summary>
        /// Temperature
        /// </summary>
        Temperature = 23,
        /// <summary>
        /// Trigger
        /// </summary>
        Trigger = 24,
        /// <summary>
        /// Trigger delay
        /// </summary>
        TriggerDelay = 25,
        /// <summary>
        /// White balance red v
        /// </summary>
        WhiteBalanceRedV = 26,
        /// <summary>
        /// Zoom
        /// </summary>
        Zoom = 27,
        /// <summary>
        /// Focus
        /// </summary>
        Focus = 28,
        /// <summary>
        /// GUID
        /// </summary>
        Guid = 29,
        /// <summary>
        /// ISO SPEED
        /// </summary>
        IsoSpeed = 30,
        /// <summary>
        /// MAX DC1394
        /// </summary>
        MaxDC1394 = 31,
        /// <summary>
        /// Backlight
        /// </summary>
        Backlight = 32,
        /// <summary>
        /// Pan
        /// </summary>
        Pan = 33,
        /// <summary>
        /// Tilt
        /// </summary>
        Tilt = 34,
        /// <summary>
        /// Roll
        /// </summary>
        Roll = 35,
        /// <summary>
        /// Iris
        /// </summary>
        Iris = 36,
        /// <summary>
        /// Pop up video/camera filter dialog (note: only supported by DSHOW backend currently. The property value is ignored)
        /// </summary>
        Settings = 37,
        /// <summary>
        /// Buffer size
        /// </summary>
        Buffersize = 38,
        /// <summary>
        /// Auto focus
        /// </summary>
        Autofocus = 39,
        /// <summary>
        /// Sample aspect ratio: num/den (num)
        /// </summary>
        SarNum = 40,
        /// <summary>
        /// Sample aspect ratio: num/den (den)
        /// </summary>
        SarDen = 41,
        /// <summary>
        /// Current backend (enum VideoCaptureAPIs). Read-only property
        /// </summary>
        Backend = 42,
        /// <summary>
        /// Video input or Channel Number (only for those cameras that support)
        /// </summary>
        Channel = 43,
        /// <summary>
        /// Enable/ disable auto white-balance
        /// </summary>
        AutoWb = 44,
        /// <summary>
        /// White-balance color temperature
        /// </summary>
        WbTemperature = 45,

        /// <summary>
        /// property for highgui class CvCapture_Android only
        /// </summary>
        Autograb = 1024,
        /// <summary>
        /// readonly, tricky property, returns cpnst char* indeed
        /// </summary>
        SupportedPreviewSizesString = 1025,
        /// <summary>
        /// readonly, tricky property, returns cpnst char* indeed
        /// </summary>
        PreviewFormat = 1026,

        /// <summary>
        /// OpenNI depth generator
        /// </summary>
        OpenniDepthGenerator = 1 << 31,
        /// <summary>
        /// OpenNI image generator
        /// </summary>
        OpenniImageGenerator = 1 << 30,
        /// <summary>
        /// OpenNI IR generator
        /// </summary>
        OpenniIRGenerator = 1 << 29,
        /// <summary>
        /// OpenNI map generators
        /// </summary>
        OpenniGeneratorsMask = OpenniDepthGenerator + OpenniImageGenerator + OpenniIRGenerator,

        /// <summary>
        /// Properties of cameras available through OpenNI interfaces
        /// </summary>
        OpenniOutputMode = 100,
        /// <summary>
        /// Properties of cameras available through OpenNI interfaces, in mm.
        /// </summary>
        OpenniFrameMaxDepth = 101,
        /// <summary>
        /// Properties of cameras available through OpenNI interfaces, in mm.
        /// </summary>
        OpenniBaseline = 102,
        /// <summary>
        /// Properties of cameras available through OpenNI interfaces, in pixels.
        /// </summary>
        OpenniFocalLength = 103,
        /// <summary>
        /// Flag that synchronizes the remapping depth map to image map
        /// by changing depth generator's view point (if the flag is "on") or
        /// sets this view point to its normal one (if the flag is "off").
        /// </summary>
        OpenniRegistration = 104,
        /// <summary>
        /// Flag that synchronizes the remapping depth map to image map
        /// by changing depth generator's view point (if the flag is "on") or
        /// sets this view point to its normal one (if the flag is "off").
        /// </summary>
        OpenniRegistrationOn = OpenniRegistration,
        /// <summary>
        /// Approx frame sync
        /// </summary>
        OpenniApproxFrameSync = 105,
        /// <summary>
        /// Max buffer size
        /// </summary>
        OpenniMaxBufferSize = 106,
        /// <summary>
        /// Circle buffer
        /// </summary>
        OpenniCircleBuffer = 107,
        /// <summary>
        /// Max time duration
        /// </summary>
        OpenniMaxTimeDuration = 108,
        /// <summary>
        /// Generator present
        /// </summary>
        OpenniGeneratorPresent = 109,
        /// <summary>
        /// OpenNI2 Sync
        /// </summary>
        Openni2Sync = 110,
        /// <summary>
        /// OpenNI2 Mirror
        /// </summary>
        Openni2Mirror = 111,

        /// <summary>
        /// Openni image generator present
        /// </summary>
        OpenniImageGeneratorPresent = OpenniImageGenerator + OpenniGeneratorPresent,
        /// <summary>
        /// Image generator output mode
        /// </summary>
        OpenniImageGeneratorOutputMode = OpenniImageGenerator + OpenniOutputMode,
        /// <summary>
        /// Depth generator present
        /// </summary>
        OpenniDepthGeneratorPresent = OpenniDepthGenerator + OpenniGeneratorPresent,
        /// <summary>
        /// Depth generator baseline, in mm.
        /// </summary>
        OpenniDepthGeneratorBaseline = OpenniDepthGenerator + OpenniBaseline,
        /// <summary>
        /// Depth generator focal length, in pixels.
        /// </summary>
        OpenniDepthGeneratorFocalLength = OpenniDepthGenerator + OpenniFocalLength,
        /// <summary>
        /// Openni generator registration
        /// </summary>
        OpenniDepthGeneratorRegistration = OpenniDepthGenerator + OpenniRegistration,
        /// <summary>
        /// Openni generator registration on
        /// </summary>
        OpenniDepthGeneratorRegistrationOn = OpenniDepthGeneratorRegistration,
        /// <summary>
        /// Openni IR generator present
        /// </summary>
        OpenniIRGeneratorPresent = OpenniIRGenerator + OpenniGeneratorPresent,

        /// <summary>
        /// Properties of cameras available through GStreamer interface. Default is 1
        /// </summary>
        GstreamerQueueLength = 200,

        /// <summary>
        /// Ip for enable multicast master mode. 0 for disable multicast
        /// </summary>
        PvapiMulticastip = 300,
        /// <summary>
        /// FrameStartTriggerMode: Determines how a frame is initiated
        /// </summary>
        PvapiFrameStartTriggerMode = 301,
        /// <summary>
        /// Horizontal sub-sampling of the image
        /// </summary>
        PvapiDecimationHorizontal = 302,
        /// <summary>
        /// Vertical sub-sampling of the image
        /// </summary>
        PvapiDecimationVertical = 303,
        /// <summary>
        /// Horizontal binning factor
        /// </summary>
        PvapiBinningX = 304,
        /// <summary>
        /// Vertical binning factor
        /// </summary>
        PvapiBinningY = 305,
        /// <summary>
        /// Pixel format
        /// </summary>
        PvapiPixelFormat = 306,

        /// <summary>
        /// Change image resolution by binning or skipping.
        /// </summary>
        XiDownsampling = 400,
        /// <summary>
        /// Output data format
        /// </summary>
        XiDataFormat = 401,
        /// <summary>
        /// Horizontal offset from the origin to the area of interest (in pixels).
        /// </summary>
        XiOffsetX = 402,
        /// <summary>
        /// Vertical offset from the origin to the area of interest (in pixels).
        /// </summary>
        XiOffsetY = 403,
        /// <summary>
        /// Defines source of trigger.
        /// </summary>
        XiTrgSource = 404,
        /// <summary>
        /// Generates an internal trigger. PRM_TRG_SOURCE must be set to TRG_SOFTWARE.
        /// </summary>
        XiTrgSoftware = 405,
        /// <summary>
        /// Selects general purpose input
        /// </summary>
        XiGpiSelector = 406,
        /// <summary>
        /// Set general purpose input mode
        /// </summary>
        XiGpiMode = 407,
        /// <summary>
        /// Get general purpose level
        /// </summary>
        XiGpiLevel = 408,
        /// <summary>
        /// Selects general purpose output
        /// </summary>
        XiGpoSelector = 409,
        /// <summary>
        /// Set general purpose output mode
        /// </summary>
        XiGpoMode = 410,
        /// <summary>
        /// Selects camera signaling LED
        /// </summary>
        XiLedSelector = 411,
        /// <summary>
        /// Define camera signaling LED functionality
        /// </summary>
        XiLedMode = 412,
        /// <summary>
        /// Calculates White Balance(must be called during acquisition)
        /// </summary>
        XiManualWb = 413,
        /// <summary>
        /// Automatic white balance
        /// </summary>
        XiAutoWb = 414,
        /// <summary>
        /// Automatic exposure/gain
        /// </summary>
        XiAeag = 415,
        /// <summary>
        /// Exposure priority (0.5 - exposure 50%, gain 50%).
        /// </summary>
        XiExpPriority = 416,
        /// <summary>
        /// Maximum limit of exposure in AEAG procedure
        /// </summary>
        XiAeMaxLimit = 417,
        /// <summary>
        /// Maximum limit of gain in AEAG procedure
        /// </summary>
        XiAgMaxLimit = 418,
        /// <summary>
        /// Average intensity of output signal AEAG should achieve(in %)
        /// </summary>
        XiAeagLevel = 419,
        /// <summary>
        /// Image capture timeout in milliseconds
        /// </summary>
        XiTimeout = 420,
        /// <summary>
        /// Exposure time in microseconds
        /// </summary>
        XiExposure = 421,
        /// <summary>
        /// Sets the number of times of exposure in one frame.
        /// </summary>
        XiExposureBurstCount = 422,
        /// <summary>
        /// Gain selector for parameter Gain allows to select different type of gains.
        /// </summary>
        XiGainSelector = 423,
        /// <summary>
        /// Gain in dB
        /// </summary>
        XiGain = 424,
        /// <summary>
        /// Change image downsampling type.
        /// </summary> 
        XiDownsamplingType = 426,
        /// <summary>
        /// Binning engine selector.
        /// </summary>
        XiBinningSelector = 427,
        /// <summary>
        /// Vertical Binning - number of vertical photo-sensitive cells to combine together.
        /// </summary>
        XiBinningVertical = 428,
        /// <summary>
        /// Horizontal Binning - number of horizontal photo-sensitive cells to combine together.
        /// </summary>
        XiBinningHorizontal = 429,
        /// <summary>
        /// Binning pattern type.
        /// </summary>
        XiBinningPattern = 430,
        /// <summary>
        /// Decimation engine selector.
        /// </summary>
        XiDecimationSelector = 431,
        /// <summary>
        /// Vertical Decimation - vertical sub-sampling of the image - reduces the vertical resolution of the image by the specified vertical decimation factor.
        /// </summary>
        XiDecimationVertical = 432,
        /// <summary>
        /// Horizontal Decimation - horizontal sub-sampling of the image - reduces the horizontal resolution of the image by the specified vertical decimation factor.
        /// </summary>
        XiDecimationHorizontal = 433,
        /// <summary>
        /// Decimation pattern type.
        /// </summary>
        XiDecimationPattern = 434,

        /// <summary>
        /// Selects which test pattern generator is controlled by the TestPattern feature.
        /// </summary>
        XiTestPatternGeneratorSelector = 587,
        /// <summary>
        /// Selects which test pattern type is generated by the selected generator.
        /// </summary>
        XiTestPattern = 588,

        /// <summary>
        /// Output data format.
        /// </summary>
        XiImageDataFormat = 435,
        /// <summary>
        /// Change sensor shutter type(CMOS sensor).
        /// </summary>
        XiShutterType = 436,
        /// <summary>
        /// Number of taps
        /// </summary>
        XiSensorTaps = 437,
        /// <summary>
        /// Automatic exposure/gain ROI offset X
        /// </summary>
        XiAeagRoiOffsetX = 439,
        /// <summary>
        /// Automatic exposure/gain ROI offset Y
        /// </summary>
        XiAeagRoiOffsetY = 440,
        /// <summary>
        /// Automatic exposure/gain ROI Width
        /// </summary>
        XiAeagRoiWidth = 441,
        /// <summary>
        /// Automatic exposure/gain ROI Height
        /// </summary>
        XiAeagRoiHeight = 442,
        /// <summary>
        /// Correction of bad pixels
        /// </summary>
        XiBpc = 445,
        /// <summary>
        /// White balance red coefficient
        /// </summary>
        XiWbKr = 448,
        /// <summary>
        /// White balance green coefficient
        /// </summary>
        XiWbKg = 449,
        /// <summary>
        /// White balance blue coefficient
        /// </summary>
        XiWbKb = 450,
        /// <summary>
        /// Width of the Image provided by the device (in pixels).
        /// </summary>
        XiWidth = 451,
        /// <summary>
        /// Height of the Image provided by the device (in pixels).
        /// </summary>
        XiHeight = 452,
        /// <summary>
        /// Selects Region in Multiple ROI which parameters are set by width, height, ... ,region mode
        /// </summary>
        XiRegionSelector = 589,
        /// <summary>
        /// Activates/deactivates Region selected by Region Selector
        /// </summary>
        XiRegionMode = 595,
        /// <summary>
        /// Set/get bandwidth(datarate)(in Megabits)
        /// </summary>
        XiLimitBandwidth = 459,
        /// <summary>
        /// Sensor output data bit depth.
        /// </summary>
        XiSensorDataBitDepth = 460,
        /// <summary>
        /// Device output data bit depth.
        /// </summary>
        XiOutputDataBitDepth = 461,
        /// <summary>
        /// bitdepth of data returned by function xiGetImage
        /// </summary>
        XiImageDataBitDepth = 462,
        /// <summary>
        /// Device output data packing (or grouping) enabled. Packing could be enabled if output_data_bit_depth &gt; 8 and packing capability is available.
        /// </summary>
        XiOutputDataPacking = 463,
        /// <summary>
        /// Data packing type. Some cameras supports only specific packing type.
        /// </summary>
        XiOutputDataPackingType = 464,
        /// <summary>
        /// Returns 1 for cameras that support cooling.
        /// </summary>
        XiIsCooled = 465,
        /// <summary>
        /// Start camera cooling.
        /// </summary>
        XiCooling = 466,
        /// <summary>
        /// Set sensor target temperature for cooling.
        /// </summary>
        XiTargetTemp = 467,
        /// <summary>
        /// Camera sensor temperature
        /// </summary>
        XiChipTemp = 468,
        /// <summary>
        /// Camera housing temperature
        /// </summary>
        XiHousTemp = 469,
        /// <summary>
        /// Camera housing back side temperature
        /// </summary>
        XiHousBackSideTemp = 590,
        /// <summary>
        /// Camera sensor board temperature
        /// </summary>
        XiSensorBoardTemp = 596,
        /// <summary>
        /// Mode of color management system.
        /// </summary>
        XiCms = 470,
        /// <summary>
        /// Enable applying of CMS profiles to xiGetImage (see XI_PRM_INPUT_CMS_PROFILE, XI_PRM_OUTPUT_CMS_PROFILE).
        /// </summary>
        XiApplyCms = 471,
        /// <summary>
        /// Returns 1 for color cameras.
        /// </summary>
        XiImageIsColor = 474,
        /// <summary>
        /// Returns color filter array type of RAW data.
        /// </summary>
        XiColorFilterArray = 475,
        /// <summary>
        /// Luminosity gamma
        /// </summary>
        XiGammay = 476,
        /// <summary>
        /// Chromaticity gamma
        /// </summary>
        XiGammac = 477,
        /// <summary>
        /// Sharpness Strength
        /// </summary>
        XiSharpness = 478,
        /// <summary>
        /// Color Correction Matrix element [0][0]
        /// </summary>
        XiCcMatrix00 = 479,
        /// <summary>
        /// Color Correction Matrix element [0][1]
        /// </summary>
        XiCcMatrix01 = 480,
        /// <summary>
        /// Color Correction Matrix element [0][2]
        /// </summary>
        XiCcMatrix02 = 481,
        /// <summary>
        /// Color Correction Matrix element [0][3]
        /// </summary>
        XiCcMatrix03 = 482,
        /// <summary>
        /// Color Correction Matrix element [1][0]
        /// </summary>
        XiCcMatrix10 = 483,
        /// <summary>
        /// Color Correction Matrix element [1][1]
        /// </summary>
        XiCcMatrix11 = 484,
        /// <summary>
        /// Color Correction Matrix element [1][2]
        /// </summary>
        XiCcMatrix12 = 485,
        /// <summary>
        /// Color Correction Matrix element [1][3]
        /// </summary>
        XiCcMatrix13 = 486,
        /// <summary>
        /// Color Correction Matrix element [2][0]
        /// </summary>
        XiCcMatrix20 = 487,
        /// <summary>
        /// Color Correction Matrix element [2][1]
        /// </summary>
        XiCcMatrix21 = 488,
        /// <summary>
        /// Color Correction Matrix element [2][2]
        /// </summary>
        XiCcMatrix22 = 489,
        /// <summary>
        /// Color Correction Matrix element [2][3]
        /// </summary>
        XiCcMatrix23 = 490,
        /// <summary>
        /// Color Correction Matrix element [3][0]
        /// </summary>
        XiCcMatrix30 = 491,
        /// <summary>
        /// Color Correction Matrix element [3][1]
        /// </summary>
        XiCcMatrix31 = 492,
        /// <summary>
        /// Color Correction Matrix element [3][2]
        /// </summary>
        XiCcMatrix32 = 493,
        /// <summary>
        /// Color Correction Matrix element [3][3]
        /// </summary>
        XiCcMatrix33 = 494,
        /// <summary>
        /// Set default Color Correction Matrix
        /// </summary>
        XiDefaultCcMatrix = 495,
        /// <summary>
        /// Selects the type of trigger.
        /// </summary>
        XiTrgSelector = 498,
        /// <summary>
        /// Sets number of frames acquired by burst. This burst is used only if trigger is set to FrameBurstStart
        /// </summary>
        XiAcqFrameBurstCount = 499,
        /// <summary>
        /// Enable/Disable debounce to selected GPI
        /// </summary>
        XiDebounceEn = 507,
        /// <summary>
        /// Debounce time (x * 10us)
        /// </summary>
        XiDebounceT0 = 508,
        /// <summary>
        /// Debounce time (x * 10us)
        /// </summary>
        XiDebounceT1 = 509,
        /// <summary>
        /// Debounce polarity (pol = 1 t0 - falling edge, t1 - rising edge)
        /// </summary>
        XiDebouncePol = 510,
        /// <summary>
        /// Status of lens control interface. This shall be set to XI_ON before any Lens operations.
        /// </summary>
        XiLensMode = 511,
        /// <summary>
        /// Current lens aperture value in stops. Examples: 2.8, 4, 5.6, 8, 11
        /// </summary>
        XiLensApertureValue = 512,
        /// <summary>
        /// Lens current focus movement value to be used by XI_PRM_LENS_FOCUS_MOVE in motor steps.
        /// </summary>
        XiLensFocusMovementValue = 513,
        /// <summary>
        /// Moves lens focus motor by steps set in XI_PRM_LENS_FOCUS_MOVEMENT_VALUE.
        /// </summary>
        XiLensFocusMove = 514,
        /// <summary>
        /// Lens focus distance in cm.
        /// </summary>
        XiLensFocusDistance = 515,
        /// <summary>
        /// Lens focal distance in mm.
        /// </summary>
        XiLensFocalLength = 516,
        /// <summary>
        /// Selects the current feature which is accessible by XI_PRM_LENS_FEATURE.
        /// </summary>
        XiLensFeatureSelector = 517,
        /// <summary>
        /// Allows access to lens feature value currently selected by XI_PRM_LENS_FEATURE_SELECTOR.
        /// </summary>
        XiLensFeature = 518,
        /// <summary>
        /// Return device model id
        /// </summary>
        XiDeviceModelId = 521,
        /// <summary>
        /// Return device serial number
        /// </summary>
        XiDeviceSn = 522,
        /// <summary>
        /// The alpha channel of RGB32 output image format.
        /// </summary>
        XiImageDataFormatRgb32Alpha = 529,
        /// <summary>
        /// Buffer size in bytes sufficient for output image returned by xiGetImage
        /// </summary>
        XiImagePayloadSize = 530,
        /// <summary>
        /// Current format of pixels on transport layer.
        /// </summary>
        XiTransportPixelFormat = 531,
        /// <summary>
        /// Sensor clock frequency in Hz.
        /// </summary>
        XiSensorClockFreqHz = 532,
        /// <summary>
        /// Sensor clock frequency index. Sensor with selected frequencies have possibility to set the frequency only by this index.
        /// </summary>
        XiSensorClockFreqIndex = 533,
        /// <summary>
        /// Number of output channels from sensor used for data transfer.
        /// </summary>
        XiSensorOutputChannelCount = 534,
        /// <summary>
        /// Define framerate in Hz
        /// </summary>
        XiFramerate = 535,
        /// <summary>
        /// Select counter
        /// </summary>
        XiCounterSelector = 536,
        /// <summary>
        /// Counter status
        /// </summary>
        XiCounterValue = 537,
        /// <summary>
        /// Type of sensor frames timing.
        /// </summary>
        XiAcqTimingMode = 538,
        /// <summary>
        /// Calculate and return available interface bandwidth(int Megabits)
        /// </summary>
        XiAvailableBandwidth = 539,
        /// <summary>
        /// Data move policy
        /// </summary>
        XiBufferPolicy = 540,
        /// <summary>
        /// Activates LUT.
        /// </summary>
        XiLutEn = 541,
        /// <summary>
        /// Control the index (offset) of the coefficient to access in the LUT.
        /// </summary>
        XiLutIndex = 542,
        /// <summary>
        /// Value at entry LUTIndex of the LUT
        /// </summary>
        XiLutValue = 543,
        /// <summary>
        /// Specifies the delay in microseconds (us) to apply after the trigger reception before activating it.
        /// </summary>
        XiTrgDelay = 544,
        /// <summary>
        /// Defines how time stamp reset engine will be armed
        /// </summary>
        XiTsRstMode = 545,
        /// <summary>
        /// Defines which source will be used for timestamp reset. Writing this parameter will trigger settings of engine (arming)
        /// </summary>
        XiTsRstSource = 546,
        /// <summary>
        /// Returns 1 if camera connected and works properly.
        /// </summary>
        XiIsDeviceExist = 547,
        /// <summary>
        /// Acquisition buffer size in buffer_size_unit. Default bytes.
        /// </summary>
        XiAcqBufferSize = 548,
        /// <summary>
        /// Acquisition buffer size unit in bytes. Default 1. E.g. Value 1024 means that buffer_size is in KiBytes
        /// </summary>
        XiAcqBufferSizeUnit = 549,
        /// <summary>
        /// Acquisition transport buffer size in bytes
        /// </summary>
        XiAcqTransportBufferSize = 550,
        /// <summary>
        /// Queue of field/frame buffers
        /// </summary>
        XiBuffersQueueSize = 551,
        /// <summary>
        /// Number of buffers to commit to low level
        /// </summary>
        XiAcqTransportBufferCommit = 552,
        /// <summary>
        /// GetImage returns most recent frame
        /// </summary>
        XiRecentFrame = 553,
        /// <summary>
        /// Resets the camera to default state.
        /// </summary>
        XiDeviceReset = 554,
        /// <summary>
        /// Correction of column FPN
        /// </summary>
        XiColumnFpnCorrection = 555,
        /// <summary>
        /// Correction of row FPN
        /// </summary>
        XiRowFpnCorrection = 591,
        /// <summary>
        /// Current sensor mode. Allows to select sensor mode by one integer. Setting of this parameter affects: image dimensions and downsampling.
        /// </summary>
        XiSensorMode = 558,
        /// <summary>
        /// Enable High Dynamic Range feature.
        /// </summary>
        XiHdr = 559,
        /// <summary>
        /// The number of kneepoints in the PWLR.
        /// </summary>
        XiHdrKneepointCount = 560,
        /// <summary>
        /// position of first kneepoint(in % of XI_PRM_EXPOSURE)
        /// </summary>
        XiHdrT1 = 561,
        /// <summary>
        /// position of second kneepoint (in % of XI_PRM_EXPOSURE)
        /// </summary>
        XiHdrT2 = 562,
        /// <summary>
        /// value of first kneepoint (% of sensor saturation)
        /// </summary>
        XiKneepoint1 = 563,
        /// <summary>
        /// value of second kneepoint (% of sensor saturation)
        /// </summary>
        XiKneepoint2 = 564,
        /// <summary>
        /// Last image black level counts. Can be used for Offline processing to recall it.
        /// </summary>
        XiImageBlackLevel = 565,
        /// <summary>
        /// Returns hardware revision number.
        /// </summary>
        XiHwRevision = 571,
        /// <summary>
        /// Set debug level
        /// </summary>
        XiDebugLevel = 572,
        /// <summary>
        /// Automatic bandwidth calculation,
        /// </summary>
        XiAutoBandwidthCalculation = 573,
        /// <summary>
        /// File number.
        /// </summary>
        XiFfsFileId = 594,
        /// <summary>
        /// Size of file.
        /// </summary>
        XiFfsFileSize = 580,
        /// <summary>
        /// Size of free camera FFS.
        /// </summary>
        XiFreeFfsSize = 581,
        /// <summary>
        /// Size of used camera FFS.
        /// </summary>
        XiUsedFfsSize = 582,
        /// <summary>
        /// Setting of key enables file operations on some cameras.
        /// </summary>
        XiFfsAccessKey = 583,
        /// <summary>
        /// Selects the current feature which is accessible by XI_PRM_SENSOR_FEATURE_VALUE.
        /// </summary>
        XiSensorFeatureSelector = 585,
        /// <summary>
        /// Allows access to sensor feature value currently selected by XI_PRM_SENSOR_FEATURE_SELECTOR.
        /// </summary>
        XiSensorFeatureValue = 586,


        /// <summary>
        /// Android flash mode
        /// </summary>
        AndroidFlashMode = 8001,
        /// <summary>
        /// Android focus mode
        /// </summary>
        AndroidFocusMode = 8002,
        /// <summary>
        /// Android white balance
        /// </summary>
        AndroidWhiteBalance = 8003,
        /// <summary>
        /// Android anti banding
        /// </summary>
        AndroidAntibanding = 8004,
        /// <summary>
        /// Android focal length
        /// </summary>
        AndroidFocalLength = 8005,
        /// <summary>
        /// Android focus distance near
        /// </summary>
        AndroidFocusDistanceNear = 8006,
        /// <summary>
        /// Android focus distance optimal
        /// </summary>
        AndroidFocusDistanceOptimal = 8007,
        /// <summary>
        /// Android focus distance far
        /// </summary>
        AndroidFocusDistanceFar = 8008,
        /// <summary>
        /// Android expose lock
        /// </summary>
        AndroidExposeLock = 8009,
        /// <summary>
        /// Android white balance lock
        /// </summary>
        AndroidWhitebalanceLock = 8010,

        /// <summary>
        /// iOS device focus
        /// </summary>
        IOSDeviceFocus = 9001,
        /// <summary>
        /// iOS device exposure
        /// </summary>
        IOSDeviceExposure = 9002,
        /// <summary>
        /// iOS device flash
        /// </summary>
        IOSDeviceFlash = 9003,
        /// <summary>
        /// iOS device white-balance 
        /// </summary>
        IOSDeviceWhitebalance = 9004,
        /// <summary>
        /// iOS device torch
        /// </summary>
        IOSDeviceTorch = 9005,

        /// <summary>
        /// Smartek Giganetix Ethernet Vision: frame offset X
        /// </summary>
        GigaFrameOffsetX = 10001,

        /// <summary>
        /// Smartek Giganetix Ethernet Vision: frame offset Y
        /// </summary>
        GigaFrameOffsetY = 10002,

        /// <summary>
        /// Smartek Giganetix Ethernet Vision: frame width max
        /// </summary>
        GigaFrameWidthMax = 10003,

        /// <summary>
        /// Smartek Giganetix Ethernet Vision: frame height max
        /// </summary>
        GigaFrameHeighMax = 10004,

        /// <summary>
        /// Smartek Giganetix Ethernet Vision: frame sens width
        /// </summary>
        GigaFrameSensWidth = 10005,

        /// <summary>
        /// Smartek Giganetix Ethernet Vision: frame sens height
        /// </summary>
        GigaFrameSensHeigh = 10006,

        /// <summary>
        /// Intelperc Profile Count
        /// </summary>
        IntelpercProfileCount = 11001,
        /// <summary>
        /// Intelperc Profile Idx
        /// </summary>
        IntelpercProfileIdx = 11002,
        /// <summary>
        /// Intelperc Depth Low Confidence Value
        /// </summary>
        IntelpercDepthLowConfidenceValue = 11003,
        /// <summary>
        /// Intelperc Depth Saturation Value
        /// </summary>
        IntelpercDepthSaturationValue = 11004,
        /// <summary>
        /// Intelperc Depth Confidence Threshold
        /// </summary>
        IntelpercDepthConfidenceThreshold = 11005,
        /// <summary>
        /// Intelperc Depth Focal Length Horz
        /// </summary>
        IntelpercDepthFocalLengthHorz = 11006,
        /// <summary>
        /// Intelperc Depth Focal Length Vert
        /// </summary>
        IntelpercDepthFocalLengthVert = 11007,

        /// <summary>
        /// Intelperc Depth Generator
        /// </summary>
        IntelpercDepthGenerator = 1 << 29,
        /// <summary>
        /// Intelperc Image Generator
        /// </summary>
        IntelpercImageGenerator = 1 << 28,
        /// <summary>
        /// Intelperc Generators Mask
        /// </summary>
        IntelpercGeneratorsMask = IntelpercDepthGenerator + IntelpercImageGenerator

    }

}
