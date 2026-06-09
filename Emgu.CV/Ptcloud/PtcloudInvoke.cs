//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
        /// <summary>
        /// The function loads point cloud from the specified file and returns it. If the cloud cannot be read, throws an error. Vertex coordinates, normals and colors are returned as they are saved in the file even if these arrays have different sizes and their elements do not correspond to each other (which is typical for OBJ files for example)
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <param name="verticies">Vertex coordinates, each value contains 3 floats</param>
        /// <param name="normals">Per-vertex normals, each value contains 3 floats</param>
        /// <param name="rgb">Per-vertex colors, each value contains 3 floats</param>
        public static void LoadPointCloud(String filename, IOutputArray verticies, IOutputArray normals = null, IOutputArray rgb = null)
        {
            using (CvString csFileName = new CvString(filename))
            using (OutputArray oaVerticies = verticies.GetOutputArray())
            using (OutputArray oaNormals = normals == null ? OutputArray.GetEmpty() : normals.GetOutputArray())
            using (OutputArray oaRgb = rgb == null ? OutputArray.GetEmpty() : rgb.GetOutputArray())
            {
                cveLoadPointCloud(csFileName, oaVerticies, oaNormals, oaRgb);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveLoadPointCloud(IntPtr filename, IntPtr vertices, IntPtr normals, IntPtr rgb);

        /// <summary>
        /// Saves a point cloud to a specified file.
        /// The function saves point cloud to the specified file. File format is chosen based on the filename extension.
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <param name="vertices">Vertex coordinates, each value contains 3 floats</param>
        /// <param name="normals">Per-vertex normals, each value contains 3 floats</param>
        /// <param name="rgb">Per-vertex colors, each value contains 3 floats</param>
        public static void SavePointCloud(String filename, IInputArray vertices, IInputArray normals = null, IInputArray rgb = null)
        {
            using (CvString csFileName = new CvString(filename))
            using (InputArray iaVertices = vertices.GetInputArray())
            using (InputArray iaNormals = normals == null ? InputArray.GetEmpty() : normals.GetInputArray())
            using (InputArray iaRgb = rgb == null ? InputArray.GetEmpty() : rgb.GetInputArray())
            {
                cveSavePointCloud(csFileName, iaVertices, iaNormals, iaRgb);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSavePointCloud(IntPtr filename, IntPtr vertices, IntPtr normals, IntPtr rgb);

        /// <summary>
        /// Loads a mesh from a file.
        /// The function loads mesh from the specified file and returns it.
        /// If the mesh cannot be read, throws an error Vertex attributes (i.e.space and texture coodinates, normals and colors) are returned in same-sized arrays with corresponding elements having the same indices.
        /// This means that if a face uses a vertex with a normal or a texture coordinate with different indices (which is typical for OBJ files for example), this vertex will be duplicated for each face it uses.
        /// Currently, the following file formats are supported:
        /// Wavefront obj file *.obj(ONLY TRIANGULATED FACES);
        /// Polygon File Format*.ply
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <param name="vertices">vertex coordinates, each value contains 3 floats</param>
        /// <param name="indices">Per-face list of vertices, each value is a vector of ints</param>
        /// <param name="normals">Per-vertex normals, each value contains 3 floats</param>
        /// <param name="colors">Per-vertex colors, each value contains 3 floats</param>
        /// <param name="texCoords">Per-vertex texture coordinates, each value contains 2 or 3 floats</param>
        public static void LoadMesh(
            String filename,
            IOutputArray vertices,
            IOutputArrayOfArrays indices,
            IOutputArray normals = null,
            IOutputArray colors = null,
            IOutputArray texCoords = null)
        {
            using (CvString csFilename = new CvString(filename))
            using (OutputArray oaVertices = vertices.GetOutputArray())
            using (OutputArray oaIndices = indices.GetOutputArray())
            using (OutputArray oaNormals = normals == null ? OutputArray.GetEmpty() : normals.GetOutputArray())
            using (OutputArray oaColors = colors == null ? OutputArray.GetEmpty() : colors.GetOutputArray())
            using (OutputArray oaTexCoords = texCoords == null ? OutputArray.GetEmpty() : texCoords.GetOutputArray())
            {
                cveLoadMesh(csFilename, oaVertices, oaIndices, oaNormals, oaColors, oaTexCoords);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveLoadMesh(
            IntPtr filename,
            IntPtr vertices,
            IntPtr indices,
            IntPtr normals,
            IntPtr colors,
            IntPtr texCoords);

        /// <summary>
        /// Saves a mesh to a specified file.
        /// The function saves mesh to the specified file.File format is chosen based on the filename extension.
        /// </summary>
        /// <param name="filename">Name of the file.</param>
        /// <param name="vertices">Vertex coordinates, each value contains 3 floats</param>
        /// <param name="indices">Per-face list of vertices, each value is a vector of ints</param>
        /// <param name="normals">Per-vertex normals, each value contains 3 floats</param>
        /// <param name="colors">Per-vertex colors, each value contains 3 floats</param>
        /// <param name="texCoords">Per-vertex texture coordinates, each value contains 2 or 3 floats</param>
        public static void SaveMesh(
            String filename,
            IInputArray vertices,
            IInputArrayOfArrays indices,
            IInputArray normals = null,
            IInputArray colors = null,
            IInputArray texCoords = null)
        {
            using (CvString csFilename = new CvString(filename))
            using (InputArray iaVertices = vertices.GetInputArray())
            using (InputArray iaIndices = indices.GetInputArray())
            using (InputArray iaNormals = normals == null ? InputArray.GetEmpty() : normals.GetInputArray())
            using (InputArray iaColors = colors == null ? InputArray.GetEmpty() : colors.GetInputArray())
            using (InputArray iaTexCoords = texCoords == null ? InputArray.GetEmpty() : texCoords.GetInputArray())
            {
                cveSaveMesh(
                    csFilename,
                    iaVertices,
                    iaIndices,
                    iaNormals,
                    iaColors,
                    iaTexCoords);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSaveMesh(
            IntPtr filename,
            IntPtr vertices,
            IntPtr indices,
            IntPtr normals,
            IntPtr colors,
            IntPtr texCoords);
    }
}
