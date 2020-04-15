//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System;
using Emgu.CV;


namespace BuildInfo.NetCore.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(CvInvoke.BuildInformation);
        }
    }
}
