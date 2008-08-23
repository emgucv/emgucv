CC=gmcs
CVTEST_SRC = Emgu.CV.Test/*.cs  
SVN_URL = https://emgucv.svn.sourceforge.net/svnroot/emgucv/trunk/
VERSION = 1.3.0.0
VS2005_FOLDER=Solution/VS2005_MonoDevelop/
VS2008_FOLDER=Solution/VS2008/
CV_DLLS=cv100.dll cxcore100.dll cvaux100.dll cvcam100.dll highgui100.dll cxts001.dll libguide40.dll opencv.license.txt
CV_CHECKOUT=Emgu.CV Emgu.Util ${VS2005_FOLDER}Emgu.CV.sln ${VS2005_FOLDER}Emgu.CV.Example.sln ${VS2008_FOLDER}Emgu.CV.sln ${VS2008_FOLDER}Emgu.CV.Example.sln Emgu.CV.Example README.txt lib/zlib.net.dll lib/zlib.net.license.txt lib/ZedGraph.dll lib/ZedGraph.license.txt

CV_RELEASE: Util CV FORCE
	install -d release; cp Emgu.CV/README.txt Emgu.CV/Emgu.CV.License.txt lib/zlib.net.license.txt lib/zlib.net.dll bin/Emgu.CV.dll bin/Emgu.Util.dll release; tar -cv release | gzip -c > Emgu.CV.Linux.Binary-${VERSION}.tar.gz; rm -rf release

Util:  FORCE  
	make -C Emgu.$@ bin; 

CV: Util FORCE 
	make -C Emgu.$@ bin;

UI: 	FORCE
	make -C Emgu.$@ bin; cp Emgu.$@/bin/Emgu.$@.dll ./bin;

CV_SRC:
	install -d src 
	install -d src/${VS2005_FOLDER}
	install -d src/${VS2008_FOLDER}
	install -d src/lib
	install -d src/bin
	svn export ${SVN_URL}Emgu.CV src/Emgu.CV
	svn export ${SVN_URL}Emgu.Util src/Emgu.Util
	svn export ${SVN_URL}${VS2005_FOLDER}Emgu.CV.sln src/${VS2005_FOLDER}Emgu.CV.sln
	svn export ${SVN_URL}${VS2008_FOLDER}Emgu.CV.sln src/${VS2008_FOLDER}Emgu.CV.sln
	svn export ${SVN_URL}${VS2005_FOLDER}Emgu.CV.Example.sln src/${VS2005_FOLDER}Emgu.CV.Example.sln
	svn export ${SVN_URL}${VS2008_FOLDER}Emgu.CV.Example.sln src/${VS2008_FOLDER}Emgu.CV.Example.sln
	svn export ${SVN_URL}Emgu.CV.Example src/Emgu.CV.Example
	svn export ${SVN_URL}README.txt src/README.txt
	svn export ${SVN_URL}lib/zlib.net.dll src/lib/zlib.net.dll
	svn export ${SVN_URL}lib/zlib.net.license.txt src/lib/zlib.net.license.txt	
	svn export ${SVN_URL}lib/ZedGraph.dll src/lib/ZedGraph.dll
	svn export ${SVN_URL}lib/ZedGraph.license.txt src/lib/ZedGraph.license.txt	

	svn export ${SVN_URL}lib/cv100.dll src/bin/cv100.dll
	svn export ${SVN_URL}lib/cxcore100.dll src/bin/cxcore100.dll
	svn export ${SVN_URL}lib/cvaux100.dll src/bin/cvaux100.dll
	svn export ${SVN_URL}lib/cvcam100.dll src/bin/cvcam100.dll
	svn export ${SVN_URL}lib/highgui100.dll src/bin/highgui100.dll
	svn export ${SVN_URL}lib/cxts001.dll src/bin/cxts001.dll
	svn export ${SVN_URL}lib/libguide40.dll src/bin/libguide40.dll
	svn export ${SVN_URL}lib/opencv.license.txt src/bin/opencv.license.txt
	zip -r Emgu.CV.SourceAndExamples-${VERSION}.zip src
	rm -rf src

CVTest: CV UI $(CVTEST_SRC)
	$(CC) -target:library -r:System.Data -r:/usr/lib/mono/nunit22/nunit.framework -r:bin/Emgu.Util.dll -r:bin/Emgu.UI.dll   -r:System.Windows.Forms -r:System.Drawing -r:bin/Emgu.CV.dll $(CVTEST_SRC) -out:bin/Emgu.CV.Test.dll 

Test: CVTest
	cd bin; nunit-console2 Emgu.CV.Test.dll; cd ..

FORCE:






