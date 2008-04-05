CC=gmcs
CVTEST_SRC = CVTest/*.cs  
SVN_URL = http://svn/svn/IQM/trunk/
 
Utils:  FORCE  
	make -C Emgu.Utils bin; cp Emgu.Utils/bin/Emgu.Utils.dll ./bin;

CV: Utils FORCE 
	make -C Emgu.CV bin; cp Emgu.CV/bin/Emgu.CV.dll ./bin;

CV_RELEASE: Utils CV FORCE
	install -d release; cp Emgu.CV/README.txt Emgu.CV/Emgu.CV.License.txt lib/zlib.net.license.txt lib/zlib.net.dll bin/Emgu.CV.dll bin/Emgu.Utils.dll release; tar -cv release | gzip -c > Emgu.CV.Linux.Binary.tar.gz; rm -rf release

CV_SRC:
	install -d src 
	svn export ${SVN_URL}Emgu.CV src/Emgu.CV
	svn export ${SVN_URL}Emgu.Utils src/Emgu.Utils
	svn export ${SVN_URL}Emgu.CV_VS2005.sln src/Emgu.CV_VS2005.sln
	svn export ${SVN_URL}Emgu.CV_VS2008.sln src/Emgu.CV_VS2008.sln
	install -d src/lib
	svn export ${SVN_URL}lib/zlib.net.dll src/lib/zlib.net.dll
	zip -r Emgu.CV.source.zip src
	rm -rf src

CV_EXAMPLE:
	svn export ${SVN_URL}Emgu.CV.Example example
	cp Emgu.CV/README.txt example
	zip -r Emgu.CV.Windows.Example.zip example
	rm -rf example

UI: 	FORCE
	cd Emgu.UI; make bin; cp bin/Emgu.Utils.dll ../bin; cd ..;

CVTest: CV UI Data   $(CVTEST_SRC)
	$(CC) -target:library -r:System.Data -r:/usr/lib/mono/1.0/nunit.framework.dll -r:bin/Emgu.Utils.dll -r:bin/Emgu.Data.dll -r:bin/Emgu.UI.dll   -r:/usr/lib/mono/2.0/System.Windows.Forms.dll -r:/usr/lib/mono/2.0/System.Drawing.dll -r:bin/Emgu.CV.dll $(CVTEST_SRC) -out:bin/Emgu.CV.Test.dll 

Test: CVTest
	cd bin; nunit-console2 Emgu.CV.Test.dll; cd ..

FORCE:

