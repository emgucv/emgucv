To use Emgu CV with IronPython, please follow the steps below:

1. Download IronPython Binary from its official web page:
http://www.codeplex.com/IronPython

2. Extract the IronPython Binary to a folder

3. Install the OpenCV 
 a. On Windows, copy the required OpenCV 1.1 dlls to the same Folder, please remember to install MSVCRT 8.0 as well
 b. On Linux, just install OpenCV package

4. Copy Emgu.CV.dll, Emgu.Util.dll, ZedGraph.dll and zlib.net.dll to the same Folder

5. Copy EmguInit.py to the same Folder

6. Run the ipy.exe program. (On Mono, call "mono ipy.exe") 

7. From the IronPython command line, run the following command:
 a. Import EmguInit
 b. from EmguInit import *

8. You are ready to go.