using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;

namespace HughLineAndCircleDetection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Load the image from file
            Image<Bgr, Byte> stuff = new Image<Bgr, byte>("stuff.jpg");

            // returns vectors of circles for each channel
            Circle<float>[][] circles = stuff.HughCircles(
                            new Bgr(200.0, 200.0, 200.0), //canny threshold 
                            new Bgr(100.0, 100.0, 100.0), //canny threshold linking
                            8.0, //Resolution of the accumulator used to detect centers of the circles
                            1.0, //min distance 
                            0, //min radius
                            0 //max radius
                            );

            // returns vectors of lines for each channel
            LineSegment2D<int>[][] lines = stuff.HughLines(
                            new Bgr(50.0, 50.0, 50.0), //canny threshold 
                            new Bgr(200.0, 200.0, 200.0), //canny threshold linking
                            1, //Distance resolution in pixel-related units
                            Math.PI / 180.0, //Angle resolution measured in radians.
                            30, //threshold
                            50, //min Line width
                            10 //gap between lines
                            );

            for (int i = 0; i < new Bgr().Dimension; i++)
            {
                //set the color of the channel
                Bgr colorOfCurrentChannel = new Bgr();
                colorOfCurrentChannel[i] = 255.0;

                //draw the circles detected from the specific channel using its color
                foreach (Circle<float> cir in circles[i])
                    stuff.Draw<float>(cir, colorOfCurrentChannel, 1);

                //draw the lines detected from the specific channel using its color
                foreach (LineSegment2D<int> line in lines[i])
                    stuff.Draw(line, colorOfCurrentChannel, 1);
            }

            //display the image
            imageBox1.Image = stuff;

        }
    }
}