using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
//using XLabs.Forms.Controls;

namespace Emgu.CV.XamarinForms
{
	public class AboutPage : ContentPage
	{
		public AboutPage ()
		{

         String openclTxt = String.Format("Has OpenCL: {0}", CvInvoke.HaveOpenCL);

		   String lineBreak = "<br/>";
         if (CvInvoke.HaveOpenCL)
         {
            openclTxt = String.Format("{0}{1}Use OpenCL: {2}{3}{4}{5}",
               openclTxt, lineBreak,
               CvInvoke.UseOpenCL, lineBreak,
               CvInvoke.OclGetPlatformsSummary(), lineBreak);
         }

         Content = new StackLayout {
				Children = {
               new WebView()
               {
                  WidthRequest =  1000,
                  HeightRequest = 1000,
                  Source =  new HtmlWebViewSource()
                  {
                     Html =
                     @"<html>
<body>
<H3> Emgu CV Examples </H3>
<a href=http://www.emgu.com>Visit our website</a> <br/><br/>
<a href=mailto:support@emgu.com>Email Support</a> <br/><br/>"
                     + openclTxt + @"
</body>
</html>"
                  }
               }
				}
			};
		}
	}
}
