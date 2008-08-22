NAME = emgu-util
VERSION = 1.1.0.0
PKG_NAME = ${NAME}-${VERSION}
MCS_FLAG = -target:library -r:System.Windows.Forms  -d:LINUX 
MCS=gmcs ${MCS_FLAG}
UTIL_SRC=  *.cs Properties/AssemblyInfo.cs 
INSTALL_DIR = var/lib
EMGU_DIR = ${INSTALL_DIR}/emgu
PKG_DIR = ${EMGU_DIR}/bin

bin: ${UTIL_SRC}
	install -d bin
	$(MCS)  $(UTIL_SRC) -out:bin/Emgu.Util.dll

rpmbin: 
	install -d ${PKG_DIR}
	$(MCS)  $(UTIL_SRC) -out:${PKG_DIR}/Emgu.Util.dll

rpm: tar
	rpmbuild -tb ${PKG_NAME}.tar.gz
	cp ~/rpm/RPMS/i386/${PKG_NAME}*.rpm ./
	rm ${PKG_NAME}.tar.gz

tar: 
	install -d ${PKG_NAME}
	tar -c Util.key makefile ${UTIL_SRC} README | tar -x -C ${PKG_NAME}
	echo VERSION: ${VERSION} > ${PKG_NAME}/${PKG_NAME}.spec
	cat ${NAME}.spec >> ${PKG_NAME}/${PKG_NAME}.spec
	tar -czf ${PKG_NAME}.tar.gz ${PKG_NAME}
	rm -rf ${PKG_NAME}



