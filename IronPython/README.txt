To use Emgu CV with IronPython, please follow the steps below:

1. Download IronPython Binary from its official web page:
http://www.codeplex.com/IronPython

2. Extract IronPython Binary to a folder

3. Install OpenCV 
 a. On Windows, copy the required OpenCV 2.1 dlls to the same Folder, please remember to install MSVCRT 9.0 as well
 b. On Linux, just install OpenCV package

4. Copy Emgu.CV.dll, Emgu.Util.dll and ZedGraph.dll to the same Folder

5. Copy EmguInit.py to the same Folder

6. Run ipy.exe (On Mono, call "mono ipy.exe") 

7. From the IronPython command line, run the following command:
 a. Import EmguInit
 b. from EmguInit import *

8. You are ready to go.