#!/bin/sh
# $Id: autogen.sh 61 2007-12-11 20:13:48Z hobu $
#
# Autotools boostrapping script
#
# We deliberately limit this to automake and autoconf since
# we don't want geo_config.h.in regenerated automatically. 
#
giveup()
{
        echo
        echo "  Something went wrong, giving up!"
        echo
        mv geo_config.h.in.safe geo_config.h.in
        exit 1
}

cp geo_config.h.in geo_config.h.in.safe
OSTYPE=`uname -s`

for libtoolize in glibtoolize libtoolize; do
    LIBTOOLIZE=`which $libtoolize 2>/dev/null`
    if test "$LIBTOOLIZE"; then
        break;
    fi
done

#AMFLAGS="--add-missing --copy --force-missing"
AMFLAGS="--add-missing --copy"
if test "$OSTYPE" = "IRIX" -o "$OSTYPE" = "IRIX64"; then
   AMFLAGS=$AMFLAGS" --include-deps";
fi

echo "Running aclocal"
aclocal || giveup
#echo "Running autoheader"
#autoheader || giveup
echo "Running libtoolize"
$LIBTOOLIZE --force --copy || giveup
echo "Running automake"
automake $AMFLAGS # || giveup
echo "Running autoconf"
autoconf || giveup
mv geo_config.h.in.safe geo_config.h.in

echo "======================================"
echo "Now you are ready to run './configure'"
echo "======================================"
