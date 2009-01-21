NAME = emgu-cv
VERSION = 1.5.0.0
PKG_NAME = ${NAME}-${VERSION}
LIB_NAME = Emgu.CV.dll
MCS_FLAG= -d:LINUX -r:System.ServiceModel -r:System.Xml -r:System.Drawing -r:System.Data  -target:library
MCS=gmcs ${MCS_FLAG} 
SRC= *.cs Shape/*.cs Properties/*.cs PointAndLine/*.cs PInvoke/*.cs PInvoke/CvType/*.cs Color/*.cs Capture/*.cs CameraCalibration/*.cs Reflection/*.cs
INSTALL_DIR = var/lib
EMGU_DIR = ${INSTALL_DIR}/emgu
PKG_DIR = ${EMGU_DIR}/bin

bin: ${SRC}
	${MCS} -r:../lib/zlib.net.dll -r:../bin/Emgu.Util.dll $(SRC) -out:../bin/${LIB_NAME}

rpmbin: 
	install -d ${PKG_DIR}
	$(MCS) -r:lib/zlib.net.dll -r:lib/Emgu.Util.dll $(SRC) -out:${PKG_DIR}/${LIB_NAME}

rpm: tar
	rpmbuild -tb ${PKG_NAME}.tar.gz
	cp ~/rpm/RPMS/i386/${PKG_NAME}*.rpm ./
	rm ${PKG_NAME}.tar.gz

tar: 
	install -d ${PKG_NAME}
	tar -c CV.snk makefile ${SRC} README.txt | tar -x -C ${PKG_NAME}
	echo VERSION: ${VERSION} > ${PKG_NAME}/${PKG_NAME}.spec
	cat ${NAME}.spec >> ${PKG_NAME}/${PKG_NAME}.spec
	install -d ${PKG_NAME}/lib
	install ../bin/Emgu.Util.dll ../lib/zlib.net.dll ${PKG_NAME}/lib
	tar -czf ${PKG_NAME}.tar.gz ${PKG_NAME}
	rm -rf ${PKG_NAME}



