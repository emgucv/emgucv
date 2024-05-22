using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

namespace DynamicImage.Asp.NetCore.Controllers
{
    /// <summary>
    /// Represents a controller that handles file-related HTTP requests in the DynamicImage.Asp.NetCore application.
    /// </summary>
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        /// <summary>
        /// Asynchronously gets an image with the current time displayed on it.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the action result for the HTTP GET operation.</returns>
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            using (Mat img = new Mat( 100, 800, DepthType.Cv8U, 3))
            {
                img.SetTo(new Emgu.CV.Structure.MCvScalar(0, 0, 0));
                CvInvoke.PutText(
                    img,
                    String.Format("The time is: {0}", DateTime.Now.ToString()),
                    new System.Drawing.Point(20, img.Height - 20),
                    Emgu.CV.CvEnum.FontFace.HersheyPlain,
                    2.0,
                    new Emgu.CV.Structure.MCvScalar(255.0, 255.0, 255.0),
                    1, LineType.EightConnected, false);
                using (VectorOfByte vb = new VectorOfByte())
                {
                    CvInvoke.Imencode(".jpg", img, vb);
                    return File(vb.ToArray(), "image/jpeg");
                }

            }
            
        }
    }
}