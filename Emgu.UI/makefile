NAME = emgu-ui
VERSION = 0.0.0.1
PKG_NAME = ${NAME}-${VERSION}
MCS_FLAG= -target:library -r:System.Windows.Forms -r:System.Data -r:System.Drawing
MCS=gmcs ${MCS_FLAG}
UI_SRC= *.cs
INSTALL_DIR = var/lib
EMGU_DIR = ${INSTALL_DIR}/emgu
PKG_DIR = ${EMGU_DIR}/bin

bin: ${UI_SRC}
	${MCS} ${UI_SRC} -out:../bin/Emgu.UI.dll

rpmbin: 
	install -d ${PKG_DIR}
	${MCS} ${UI_SRC} -out:${PKG_DIR}/Emgu.UI.dll

rpm: tar
	rpmbuild -tb ${PKG_NAME}.tar.gz
	cp ~/rpm/RPMS/i386/${PKG_NAME}*.rpm ./
	rm ${PKG_NAME}.tar.gz

tar: 
	install -d ${PKG_NAME}
	tar -c UI.key makefile ${UI_SRC} README | tar -x -C ${PKG_NAME}
	echo VERSION: ${VERSION} > ${PKG_NAME}/${PKG_NAME}.spec
	cat ${NAME}.spec >> ${PKG_NAME}/${PKG_NAME}.spec
	tar -czf ${PKG_NAME}.tar.gz ${PKG_NAME}
	rm -rf ${PKG_NAME}
