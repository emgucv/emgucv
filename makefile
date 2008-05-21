CC=gmcs
CVTEST_SRC = Emgu.CV.Test/*.cs  
SVN_URL = https://emgucv.svn.sourceforge.net/svnroot/emgucv/trunk/
VERSION = 1.2.2.0

CV_RELEASE: Utils CV FORCE
	install -d release; cp Emgu.CV/README.txt Emgu.CV/Emgu.CV.License.txt lib/zlib.net.license.txt lib/zlib.net.dll bin/Emgu.CV.dll bin/Emgu.Utils.dll release; tar -cv release | gzip -c > Emgu.CV.Linux.Binary-${VERSION}.tar.gz; rm -rf release

Utils:  FORCE  
	make -C Emgu.Utils bin; cp Emgu.Utils/bin/Emgu.Utils.dll ./bin;

CV: Utils FORCE 
	make -C Emgu.CV bin; cp Emgu.CV/bin/Emgu.CV.dll ./bin;

CV_SRC:
	install -d src 
	svn export ${SVN_URL}Emgu.CV src/Emgu.CV
	svn export ${SVN_URL}Emgu.Utils src/Emgu.Utils
	install -d src/Solution/VS2005_MonoDevelop
	install -d src/Solution/VS2008
	svn export ${SVN_URL}Solution/VS2005_MonoDevelop/Emgu.CV_VS2005.sln src/Solution/VS2005_MonoDevelop/Emgu.CV_VS2005.sln
	svn export ${SVN_URL}Solution/VS2008/Emgu.CV_VS2008.sln src/Solution/VS2008/Emgu.CV_VS2008.sln
	svn export ${SVN_URL}Solution/VS2005_MonoDevelop/Emgu.CV.Example_VS2005.sln src/Solution/VS2005_MonoDevelop/Emgu.CV_VS2005.sln
	svn export ${SVN_URL}Solution/VS2008/Emgu.CV.Example_VS2008.sln src/Solution/VS2008/Emgu.CV_VS2008.sln
	svn export ${SVN_URL}Emgu.CV.Example src/Emgu.CV.Example
	svn export ${SVN_URL}README.txt src/README.txt
	install -d src/lib
	svn export ${SVN_URL}lib/zlib.net.dll src/lib/zlib.net.dll
	svn export ${SVN_URL}lib/zlib.net.license.txt src/lib/zlib.net.license.txt	
	install -d src/bin
	svn export ${SVN_URL}lib/cv100.dll src/bin/cv100.dll
	svn export ${SVN_URL}lib/cxcore100.dll src/bin/cxcore100.dll
	svn export ${SVN_URL}lib/cvaux100.dll src/bin/cvaux100.dll
	svn export ${SVN_URL}lib/cvcam100.dll src/bin/cvcam100.dll
	svn export ${SVN_URL}lib/highgui100.dll src/bin/highgui100.dll
	svn export ${SVN_URL}lib/cxts001.dll src/bin/cxts001.dll
	svn export ${SVN_URL}lib/libguide40.dll src/bin/libguide40.dll
	svn export ${SVN_URL}lib/opencv.license.txt src/bin/opencv.license.txt
	zip -r Emgu.CV-${VERSION}.source.zip src
	rm -rf src

UI: 	FORCE
	cd Emgu.UI; make bin; cp bin/Emgu.UI.dll ../bin; cd ..;

CVTest: CV UI $(CVTEST_SRC)
	$(CC) -target:library -r:System.Data -r:/usr/lib/mono/nunit22/nunit.framework -r:bin/Emgu.Utils.dll -r:bin/Emgu.UI.dll   -r:System.Windows.Forms -r:System.Drawing -r:bin/Emgu.CV.dll $(CVTEST_SRC) -out:bin/Emgu.CV.Test.dll 

Test: CVTest
	cd bin; nunit-console2 Emgu.CV.Test.dll; cd ..

FORCE:






