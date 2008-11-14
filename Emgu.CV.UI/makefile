NAME = emgu-cv-ui
VERSION = 1.0.0.0
PKG_NAME = ${NAME}-${VERSION}
LIB_NAME = Emgu.CV.UI.dll
MCS_FLAG= -d:LINUX -r:System.Xml -r:System.Drawing -r:System.Data -r:System.Windows.Forms -target:library
MCS=gmcs ${MCS_FLAG} 
SRC= *.cs Properties/*.cs 
INSTALL_DIR = var/lib
EMGU_DIR = ${INSTALL_DIR}/emgu
PKG_DIR = ${EMGU_DIR}/bin

bin: ${SRC}
	${MCS} -r:../lib/zlib.net.dll -r:../lib/ZedGraph.dll -r:../bin/Emgu.Util.dll -r:../bin/Emgu.CV.dll $(SRC) -out:../bin/${LIB_NAME}

rpmbin: 
	install -d ${PKG_DIR}
	$(MCS) -r:lib/zlib.net.dll -r:lib/Emgu.Util.dll $(SRC) -out:${PKG_DIR}/${LIB_NAME}

rpm: tar
	rpmbuild -tb ${PKG_NAME}.tar.gz
	cp ~/rpm/RPMS/i386/${PKG_NAME}*.rpm ./
	rm ${PKG_NAME}.tar.gz

tar: 
	install -d ${PKG_NAME}
	tar -c CV.UI.snk makefile ${SRC} README.txt | tar -x -C ${PKG_NAME}
	echo VERSION: ${VERSION} > ${PKG_NAME}/${PKG_NAME}.spec
	cat ${NAME}.spec >> ${PKG_NAME}/${PKG_NAME}.spec
	install -d ${PKG_NAME}/lib
	install ../bin/Emgu.CV.dll ../bin/Emgu.Util.dll ../lib/zlib.net.dll ${PKG_NAME}/lib
	tar -czf ${PKG_NAME}.tar.gz ${PKG_NAME}
	rm -rf ${PKG_NAME}
