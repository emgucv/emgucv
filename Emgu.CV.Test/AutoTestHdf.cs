//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Hdf;
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

namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestHdf
    {
        private static bool IsHdfSupported()
        {
            String tmpFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".h5");
            try
            {
                using (HDF5 hdf = new HDF5(tmpFile))
                {
                }
                return true;
            }
            catch (CvException)
            {
                return false;
            }
            finally
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
            }
        }

        [Test]
        public void TestHDF5Attributes()
        {
            if (!IsHdfSupported())
                return;

            String tmpFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".h5");
            try
            {
                using (HDF5 hdf = new HDF5(tmpFile))
                {
                    hdf.AtWrite(42, "intAttribute");
                    EmguAssert.AreEqual(42, hdf.AtReadInt("intAttribute"));

                    hdf.AtWrite(3.14, "doubleAttribute");
                    EmguAssert.AreEqual(3.14, hdf.AtReadDouble("doubleAttribute"));

                    hdf.AtWrite("hello hdf5", "stringAttribute");
                    EmguAssert.AreEqual("hello hdf5", hdf.AtReadString("stringAttribute"));

                    EmguAssert.IsTrue(hdf.AtExists("intAttribute"));
                    hdf.AtDelete("intAttribute");
                    EmguAssert.IsFalse(hdf.AtExists("intAttribute"));

                    hdf.Close();
                }
            }
            finally
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
            }
        }

        [Test]
        public void TestHDF5Dataset()
        {
            if (!IsHdfSupported())
                return;

            String tmpFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".h5");
            try
            {
                using (HDF5 hdf = new HDF5(tmpFile))
                using (Mat data = new Mat(4, 4, DepthType.Cv32F, 1))
                using (Mat readBack = new Mat())
                {
                    data.SetTo(new Structure.MCvScalar(1.5));

                    hdf.GrCreate("group1");
                    EmguAssert.IsTrue(hdf.HlExist("group1"));

                    hdf.DsWrite(data, "dataset1");
                    EmguAssert.IsTrue(hdf.HlExist("dataset1"));

                    hdf.DsRead(readBack, "dataset1");
                    EmguAssert.IsFalse(readBack.IsEmpty);
                    EmguAssert.AreEqual(data.Size, readBack.Size);

                    hdf.Close();
                }
            }
            finally
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
            }
        }
    }
}
