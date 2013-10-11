/* geotifcp.c -- based on Sam Leffler's "tiffcp" code */

/*
 *  Original code had this copyright notice: 
 *
 * Copyright (c) 1988-1995 Sam Leffler
 * Copyright (c) 1991-1995 Silicon Graphics, Inc.
 *
 * and a lot of legal stuff denying liability for anything.
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

/* GeoTIFF overrides */

#include "geotiff.h"
#include "geo_normalize.h"
#include "geo_tiffp.h"
#include "geo_keyp.h"
#include "xtiffio.h"
#include "cpl_serv.h"

#define TIFFOpen XTIFFOpen
#define TIFFClose XTIFFClose

#if defined(VMS)
#define unlink delete
#endif

#define	streq(a,b)	(strcmp(a,b) == 0)
#define	strneq(a,b,n)	(strncmp(a,b,n) == 0)

#define	TRUE	1
#define	FALSE	0

int getopt();

static  int outtiled = -1;
static  uint32 tilewidth;
static  uint32 tilelength;
static  int convert_8_to_4 = 0;

static	uint16 config;
static	uint16 compression;
static	uint16 predictor;
static	uint16 fillorder;
static	uint32 rowsperstrip;
static	uint32 g3opts;
static	int ignore = FALSE;		/* if true, ignore read errors */
static	uint32 defg3opts = (uint32) -1;
static	int quality = 75;		/* JPEG quality */
static	int jpegcolormode = JPEGCOLORMODE_RGB;
static	uint16 defcompression = (uint16) -1;
static	uint16 defpredictor = (uint16) -1;
static 	char *geofile=(char *)0;
static  char *proj4_string = (char *) 0;
static  char *worldfile=(char *)0;

static  void ApplyWorldFile(const char *worldfile, TIFF *out);
static	int tiffcp(TIFF*, TIFF*);
static	int processCompressOptions(char*);
static	void usage(void);


int
main(int argc, char* argv[])
{
	uint16 defconfig = (uint16) -1;
	uint16 deffillorder = 0;
	uint32 deftilewidth = (uint32) -1;
	uint32 deftilelength = (uint32) -1;
	uint32 defrowsperstrip = (uint32) -1;
	uint32 diroff = 0;
	TIFF* in;
	TIFF* out;
	const char* mode = "w";
	int c;
	extern int optind;
	extern char* optarg;

	while ((c = getopt(argc, argv, "c:f:l:o:p:r:w:e:g:4:aistd")) != -1)
		switch (c) {
		case 'a':		/* append to output */
			mode = "a";
			break;
		case 'd':		/* down cast 8bit to 4bit */
                        convert_8_to_4 = 1;
			break;
		case 'c':		/* compression scheme */
			if (!processCompressOptions(optarg))
				usage();
			break;
                case 'e':
                        worldfile = optarg;
                        break;
		case 'f':		/* fill order */
			if (streq(optarg, "lsb2msb"))
				deffillorder = FILLORDER_LSB2MSB;
			else if (streq(optarg, "msb2lsb"))
				deffillorder = FILLORDER_MSB2LSB;
			else
				usage();
			break;
		case 'i':		/* ignore errors */
			ignore = TRUE;
			break;
		case 'g':		/* GeoTIFF metadata file */
			geofile = optarg;
			break;
		case '4':	       
			proj4_string = optarg;
			break;
		case 'l':		/* tile length */
			outtiled = TRUE;
			deftilelength = atoi(optarg);
			break;
		case 'o':		/* initial directory offset */
			diroff = strtoul(optarg, NULL, 0);
			break;
		case 'p':		/* planar configuration */
			if (streq(optarg, "separate"))
				defconfig = PLANARCONFIG_SEPARATE;
			else if (streq(optarg, "contig"))
				defconfig = PLANARCONFIG_CONTIG;
			else
				usage();
			break;
		case 'r':		/* rows/strip */
			defrowsperstrip = atoi(optarg);
			break;
		case 's':		/* generate stripped output */
			outtiled = FALSE;
			break;
		case 't':		/* generate tiled output */
			outtiled = TRUE;
			break;
		case 'w':		/* tile width */
			outtiled = TRUE;
			deftilewidth = atoi(optarg);
			break;
		case '?':
			usage();
			/*NOTREACHED*/
		}
	if (argc - optind < 2)
		usage();
	out = TIFFOpen(argv[argc-1], mode);
	if (out == NULL)
		return (-2);
	for (; optind < argc-1 ; optind++) {
		in = TIFFOpen(argv[optind], "r");
		if (in == NULL)
			return (-3);
		if (diroff != 0 && !TIFFSetSubDirectory(in, diroff)) {
			TIFFError(TIFFFileName(in),
			    "Error, setting subdirectory at %#x", diroff);
			(void) TIFFClose(out);
			return (1);
		}
		do {
			config = defconfig;
			compression = defcompression;
			predictor = defpredictor;
			fillorder = deffillorder;
			rowsperstrip = defrowsperstrip;
			tilewidth = deftilewidth;
			tilelength = deftilelength;
			g3opts = defg3opts;
			if (!tiffcp(in, out) || !TIFFWriteDirectory(out)) {
				(void) TIFFClose(out);
				return (1);
			}
		} while (TIFFReadDirectory(in));
		(void) TIFFClose(in);
	}
	(void) TIFFClose(out);
	return (0);
}

static void ApplyWorldFile(const char *worldfilename, TIFF *out)

{
    FILE	*tfw;
    double	pixsize[3], xoff, yoff, tiepoint[6], x_rot, y_rot;

    /* 
     * Read the world file.  Note we currently ignore rotational coefficients!
     */
    tfw = fopen( worldfilename, "rt" );
    if( tfw == NULL )
    {
        perror( worldfilename );
        return;
    }

    fscanf( tfw, "%lf", pixsize + 0 );
    fscanf( tfw, "%lf", &y_rot );
    fscanf( tfw, "%lf", &x_rot );
    fscanf( tfw, "%lf", pixsize + 1 );
    fscanf( tfw, "%lf", &xoff );
    fscanf( tfw, "%lf", &yoff );

    fclose( tfw );

    /*
     * Write out pixel scale, and tiepoint information.
     */
    if( x_rot == 0.0 && y_rot == 0.0 )
    {
        pixsize[1] = ABS(pixsize[1]);
        pixsize[2] = 0.0;
        TIFFSetField(out, GTIFF_PIXELSCALE, 3, pixsize);

        tiepoint[0] = 0.5;
        tiepoint[1] = 0.5;
        tiepoint[2] = 0.0;
        tiepoint[3] = xoff;
        tiepoint[4] = yoff;
        tiepoint[5] = 0.0;
        TIFFSetField(out, GTIFF_TIEPOINTS, 6, tiepoint);
    }
    else
    {
        double	adfMatrix[16];
        
        memset(adfMatrix,0,sizeof(double) * 16);
        
        adfMatrix[0] = pixsize[0];
        adfMatrix[1] = x_rot;
        adfMatrix[3] = xoff - (pixsize[0]+x_rot) * 0.5;
        adfMatrix[4] = y_rot;
        adfMatrix[5] = pixsize[1];
        adfMatrix[7] = yoff - (pixsize[1]+y_rot) * 0.5;
        adfMatrix[15] = 1.0;
        
        TIFFSetField( out, TIFFTAG_GEOTRANSMATRIX, 16, adfMatrix );
    }
}

static void InstallGeoTIFF(TIFF *out)
{
    GTIF *gtif=(GTIF*)0; /* GeoKey-level descriptor */
    FILE *fd;

    gtif = GTIFNew(out);
    if (!gtif)
    {
        printf("failed in GTIFNew\n");
        return;
    }

    if( geofile )
    {
        /* Install keys and tags */
        fd = fopen(geofile,"r");
        if( fd == NULL )
        {
            perror( geofile );
            exit( -1 );
        }
        if (!GTIFImport(gtif,0,fd))
        {
            fprintf(stderr,"Failure in GTIFImport\n");
            exit (-1);
        }
        fclose(fd);
    }
    else if( proj4_string )
    {
        if( !GTIFSetFromProj4(gtif,proj4_string) )
        {
            fprintf(stderr,"Failure in GTIFSetFromProj4\n");
            exit (-1);
        }
    }
    GTIFWriteKeys(gtif);
    GTIFFree(gtif);
    return;
}

static void CopyGeoTIFF(TIFF * in, TIFF *out)
{
    GTIF *gtif=(GTIF*)0; /* GeoKey-level descriptor */
    double *d_list = NULL;
    int16   d_list_count;

    /* read definition from source file. */
    gtif = GTIFNew(in);
    if (!gtif)
        return;

    if (TIFFGetField(in, GTIFF_TIEPOINTS, &d_list_count, &d_list))
        TIFFSetField(out, GTIFF_TIEPOINTS, d_list_count, d_list);
    if (TIFFGetField(in, GTIFF_PIXELSCALE, &d_list_count, &d_list))
        TIFFSetField(out, GTIFF_PIXELSCALE, d_list_count, d_list);
    if (TIFFGetField(in, GTIFF_TRANSMATRIX, &d_list_count, &d_list))
        TIFFSetField(out, GTIFF_TRANSMATRIX, d_list_count, d_list);
            
    /* Here we violate the GTIF abstraction to retarget on another file.
       We should just have a function for copying tags from one GTIF object
       to another. */
    gtif->gt_tif = out;
    gtif->gt_flags |= FLAG_FILE_MODIFIED;

    /* Install keys and tags */
    GTIFWriteKeys(gtif);
    GTIFFree(gtif);
    return;
}

static void
processG3Options(char* cp)
{
    if( (cp = strchr(cp, ':')) != NULL ) {
        if (defg3opts == (uint32) -1)
            defg3opts = 0;
        do {
            cp++;
            if (strneq(cp, "1d", 2))
                defg3opts &= ~GROUP3OPT_2DENCODING;
            else if (strneq(cp, "2d", 2))
                defg3opts |= GROUP3OPT_2DENCODING;
            else if (strneq(cp, "fill", 4))
                defg3opts |= GROUP3OPT_FILLBITS;
            else
                usage();
        } while( (cp = strchr(cp, ':')) != NULL );
    }
}

static int
processCompressOptions(char* opt)
{
    if (streq(opt, "none"))
        defcompression = COMPRESSION_NONE;
    else if (streq(opt, "packbits"))
        defcompression = COMPRESSION_PACKBITS;
    else if (strneq(opt, "jpeg", 4)) {
        char* cp = strchr(opt, ':');
        if (cp && isdigit(cp[1]))
            quality = atoi(cp+1);
        if (cp && strchr(cp, 'r'))
            jpegcolormode = JPEGCOLORMODE_RAW;
        defcompression = COMPRESSION_JPEG;
    } else if (strneq(opt, "g3", 2)) {
        processG3Options(opt);
        defcompression = COMPRESSION_CCITTFAX3;
    } else if (streq(opt, "g4"))
        defcompression = COMPRESSION_CCITTFAX4;
    else if (strneq(opt, "lzw", 3)) {
        char* cp = strchr(opt, ':');
        if (cp)
            defpredictor = atoi(cp+1);
        defcompression = COMPRESSION_LZW;
    } else if (strneq(opt, "zip", 3)) {
        char* cp = strchr(opt, ':');
        if (cp)
            defpredictor = atoi(cp+1);
        defcompression = COMPRESSION_DEFLATE;
    } else
        return (0);
    return (1);
}

char* stuff[] = {
"usage: gtiffcp [options] input... output",
"where options are:",
" -g file	install GeoTIFF metadata from <file>",
" -4 proj4_str	install GeoTIFF metadata from proj4 string",
" -e file	install positioning info from ESRI Worldfile <file>",
" -a		append to output instead of overwriting",
" -o offset	set initial directory offset",
" -p contig	pack samples contiguously (e.g. RGBRGB...)",
" -p separate	store samples separately (e.g. RRR...GGG...BBB...)",
" -s		write output in strips",
" -t		write output in tiles",
" -i		ignore read errors",
" -d		truncate 8 bitspersample to 4bitspersample",
"",
" -r #		make each strip have no more than # rows",
" -w #		set output tile width (pixels)",
" -l #		set output tile length (pixels)",
"",
" -f lsb2msb	force lsb-to-msb FillOrder for output",
" -f msb2lsb	force msb-to-lsb FillOrder for output",
"",
" -c lzw[:opts]	compress output with Lempel-Ziv & Welch encoding",
" -c zip[:opts]	compress output with deflate encoding",
" -c jpeg[:opts]compress output with JPEG encoding",
" -c packbits	compress output with packbits encoding",
" -c g3[:opts]	compress output with CCITT Group 3 encoding",
" -c g4		compress output with CCITT Group 4 encoding",
" -c none	use no compression algorithm on output",
"",
"Group 3 options:",
" 1d		use default CCITT Group 3 1D-encoding",
" 2d		use optional CCITT Group 3 2D-encoding",
" fill		byte-align EOL codes",
"For example, -c g3:2d:fill to get G3-2D-encoded data with byte-aligned EOLs",
"",
"JPEG options:",
" #		set compression quality level (0-100, default 75)",
" r		output color image as RGB rather than YCbCr",
"For example, -c jpeg:r:50 to get JPEG-encoded RGB data with 50% comp. quality",
"",
"LZW and deflate options:",
" #		set predictor value",
"For example, -c lzw:2 to get LZW-encoded data with horizontal differencing",
NULL
};

static void
usage(void)
{
	char buf[BUFSIZ];
	int i;

	setbuf(stderr, buf);
	for (i = 0; stuff[i] != NULL; i++)
		fprintf(stderr, "%s\n", stuff[i]);
	exit(-1);
}

static void
CheckAndCorrectColormap(TIFF* tif, int n, uint16* r, uint16* g, uint16* b)
{
	int i;

	for (i = 0; i < n; i++)
		if (r[i] >= 256 || g[i] >= 256 || b[i] >= 256)
			return;
	TIFFWarning(TIFFFileName(tif), "Scaling 8-bit colormap");
#define	CVT(x)		(((x) * ((1L<<16)-1)) / 255)
	for (i = 0; i < n; i++) {
		r[i] = CVT(r[i]);
		g[i] = CVT(g[i]);
		b[i] = CVT(b[i]);
	}
#undef CVT
}

#define	CopyField(tag, v) \
    if (TIFFGetField(in, tag, &v)) TIFFSetField(out, tag, v)
#define	CopyField2(tag, v1, v2) \
    if (TIFFGetField(in, tag, &v1, &v2)) TIFFSetField(out, tag, v1, v2)
#define	CopyField3(tag, v1, v2, v3) \
    if (TIFFGetField(in, tag, &v1, &v2, &v3)) TIFFSetField(out, tag, v1, v2, v3)
#define	CopyField4(tag, v1, v2, v3, v4) \
    if (TIFFGetField(in, tag, &v1, &v2, &v3, &v4)) TIFFSetField(out, tag, v1, v2, v3, v4)

static struct cpTag {
	uint16	tag;
	uint16	count;
	TIFFDataType type;
} tags[] = {
	{ TIFFTAG_SUBFILETYPE,		1, TIFF_LONG },
	{ TIFFTAG_THRESHHOLDING,	1, TIFF_SHORT },
	{ TIFFTAG_DOCUMENTNAME,		1, TIFF_ASCII },
	{ TIFFTAG_IMAGEDESCRIPTION,	1, TIFF_ASCII },
	{ TIFFTAG_MAKE,			1, TIFF_ASCII },
	{ TIFFTAG_MODEL,		1, TIFF_ASCII },
	{ TIFFTAG_ORIENTATION,		1, TIFF_SHORT },
	{ TIFFTAG_MINSAMPLEVALUE,	1, TIFF_SHORT },
	{ TIFFTAG_MAXSAMPLEVALUE,	1, TIFF_SHORT },
	{ TIFFTAG_XRESOLUTION,		1, TIFF_RATIONAL },
	{ TIFFTAG_YRESOLUTION,		1, TIFF_RATIONAL },
	{ TIFFTAG_PAGENAME,		1, TIFF_ASCII },
	{ TIFFTAG_XPOSITION,		1, TIFF_RATIONAL },
	{ TIFFTAG_YPOSITION,		1, TIFF_RATIONAL },
	{ TIFFTAG_GROUP4OPTIONS,	1, TIFF_LONG },
	{ TIFFTAG_RESOLUTIONUNIT,	1, TIFF_SHORT },
	{ TIFFTAG_PAGENUMBER,		2, TIFF_SHORT },
	{ TIFFTAG_SOFTWARE,		1, TIFF_ASCII },
	{ TIFFTAG_DATETIME,		1, TIFF_ASCII },
	{ TIFFTAG_ARTIST,		1, TIFF_ASCII },
	{ TIFFTAG_HOSTCOMPUTER,		1, TIFF_ASCII },
	{ TIFFTAG_WHITEPOINT,		1, TIFF_RATIONAL },
	{ TIFFTAG_PRIMARYCHROMATICITIES,(uint16) -1,TIFF_RATIONAL },
	{ TIFFTAG_HALFTONEHINTS,	2, TIFF_SHORT },
	{ TIFFTAG_BADFAXLINES,		1, TIFF_LONG },
	{ TIFFTAG_CLEANFAXDATA,		1, TIFF_SHORT },
	{ TIFFTAG_CONSECUTIVEBADFAXLINES,1, TIFF_LONG },
	{ TIFFTAG_INKSET,		1, TIFF_SHORT },
	{ TIFFTAG_INKNAMES,		1, TIFF_ASCII },
	{ TIFFTAG_DOTRANGE,		2, TIFF_SHORT },
	{ TIFFTAG_TARGETPRINTER,	1, TIFF_ASCII },
	{ TIFFTAG_SAMPLEFORMAT,		1, TIFF_SHORT },
	{ TIFFTAG_YCBCRCOEFFICIENTS,	(uint16) -1,TIFF_RATIONAL },
	{ TIFFTAG_YCBCRSUBSAMPLING,	2, TIFF_SHORT },
	{ TIFFTAG_YCBCRPOSITIONING,	1, TIFF_SHORT },
	{ TIFFTAG_REFERENCEBLACKWHITE,	(uint16) -1,TIFF_RATIONAL },
	{ TIFFTAG_EXTRASAMPLES,		(uint16) -1, TIFF_SHORT },
	{ TIFFTAG_SMINSAMPLEVALUE,	1, TIFF_DOUBLE },
	{ TIFFTAG_SMAXSAMPLEVALUE,	1, TIFF_DOUBLE },
};
#define	NTAGS	(sizeof (tags) / sizeof (tags[0]))

static void
cpOtherTags(TIFF* in, TIFF* out)
{
	struct cpTag *p;

	for (p = tags; p < &tags[NTAGS]; p++)
		switch (p->type) {
		case TIFF_SHORT:
			if (p->count == 1) {
				uint16 shortv;
				CopyField(p->tag, shortv);
			} else if (p->count == 2) {
				uint16 shortv1, shortv2;
				CopyField2(p->tag, shortv1, shortv2);
			} else if (p->count == (uint16) -1) {
				uint16 shortv1;
				uint16* shortav;
				CopyField2(p->tag, shortv1, shortav);
			}
			break;
		case TIFF_LONG:
			{ uint32 longv;
			  CopyField(p->tag, longv);
			}
			break;
		case TIFF_RATIONAL:
			if (p->count == 1) {
				float floatv;
				CopyField(p->tag, floatv);
			} else if (p->count == (uint16) -1) {
				float* floatav;
				CopyField(p->tag, floatav);
			}
			break;
		case TIFF_ASCII:
			{ char* stringv;
			  CopyField(p->tag, stringv);
			}
			break;
		case TIFF_DOUBLE:
			if (p->count == 1) {
				double doublev;
				CopyField(p->tag, doublev);
			} else if (p->count == (uint16) -1) {
				double* doubleav;
				CopyField(p->tag, doubleav);
			}
			break;
                default:
	                break;
		}
}

typedef int (*copyFunc)
    (TIFF* in, TIFF* out, uint32 l, uint32 w, uint16 samplesperpixel);
static	copyFunc pickCopyFunc(TIFF*, TIFF*, uint16, uint16);

static int
tiffcp(TIFF* in, TIFF* out)
{
	uint16 bitspersample, samplesperpixel, shortv;
	copyFunc cf;
	uint32 w, l;

	CopyField(TIFFTAG_IMAGEWIDTH, w);
	CopyField(TIFFTAG_IMAGELENGTH, l);
        if( convert_8_to_4 )
        {
            TIFFSetField(out, TIFFTAG_BITSPERSAMPLE, 4);
        }
        else
        {
            CopyField(TIFFTAG_BITSPERSAMPLE, bitspersample);
        }
	if (compression != (uint16)-1)
		TIFFSetField(out, TIFFTAG_COMPRESSION, compression);
	else
		CopyField(TIFFTAG_COMPRESSION, compression);
	if (compression == COMPRESSION_JPEG && jpegcolormode == JPEGCOLORMODE_RGB)
		TIFFSetField(out, TIFFTAG_PHOTOMETRIC, PHOTOMETRIC_YCBCR);
	else
		CopyField(TIFFTAG_PHOTOMETRIC, shortv);
	if (fillorder != 0)
		TIFFSetField(out, TIFFTAG_FILLORDER, fillorder);
	else
		CopyField(TIFFTAG_FILLORDER, shortv);
	CopyField(TIFFTAG_SAMPLESPERPIXEL, samplesperpixel);
	/*
	 * Choose tiles/strip for the output image according to
	 * the command line arguments (-tiles, -strips) and the
	 * structure of the input image.
	 */
	if (outtiled == -1)
		outtiled = TIFFIsTiled(in);
	if (outtiled) {
		/*
		 * Setup output file's tile width&height.  If either
		 * is not specified, use either the value from the
		 * input image or, if nothing is defined, use the
		 * library default.
		 */
		if (tilewidth == (uint32) -1)
			TIFFGetField(in, TIFFTAG_TILEWIDTH, &tilewidth);
		if (tilelength == (uint32) -1)
			TIFFGetField(in, TIFFTAG_TILELENGTH, &tilelength);
		TIFFDefaultTileSize(out, &tilewidth, &tilelength);
		TIFFSetField(out, TIFFTAG_TILEWIDTH, tilewidth);
		TIFFSetField(out, TIFFTAG_TILELENGTH, tilelength);
	} else {
		/*
		 * RowsPerStrip is left unspecified: use either the
		 * value from the input image or, if nothing is defined,
		 * use the library default.
		 */
		if (rowsperstrip == (uint32) -1)
			TIFFGetField(in, TIFFTAG_ROWSPERSTRIP, &rowsperstrip);
		rowsperstrip = TIFFDefaultStripSize(out, rowsperstrip);
		TIFFSetField(out, TIFFTAG_ROWSPERSTRIP, rowsperstrip);
	}
	if (config != (uint16) -1)
		TIFFSetField(out, TIFFTAG_PLANARCONFIG, config);
	else
		CopyField(TIFFTAG_PLANARCONFIG, config);
	if (g3opts != (uint32) -1)
		TIFFSetField(out, TIFFTAG_GROUP3OPTIONS, g3opts);
	else
		CopyField(TIFFTAG_GROUP3OPTIONS, g3opts);
	if (samplesperpixel <= 4) {
		uint16 *tr, *tg, *tb, *ta;
		CopyField4(TIFFTAG_TRANSFERFUNCTION, tr, tg, tb, ta);
	}
	{ uint16 *red, *green, *blue;
	  if (TIFFGetField(in, TIFFTAG_COLORMAP, &red, &green, &blue)) {
		CheckAndCorrectColormap(in, 1<<bitspersample, red, green, blue);
		TIFFSetField(out, TIFFTAG_COLORMAP, red, green, blue);
	  }
	}
/* SMinSampleValue & SMaxSampleValue */
	switch (compression) {
	case COMPRESSION_JPEG:
		TIFFSetField(out, TIFFTAG_JPEGQUALITY, quality);
		TIFFSetField(out, TIFFTAG_JPEGCOLORMODE, jpegcolormode);
		break;
	case COMPRESSION_LZW:
	case COMPRESSION_DEFLATE:
		if (predictor != (uint16)-1)
			TIFFSetField(out, TIFFTAG_PREDICTOR, predictor);
		else
			CopyField(TIFFTAG_PREDICTOR, predictor);
		break;
	}
	cpOtherTags(in, out);

	if (geofile || proj4_string )
            InstallGeoTIFF(out);
        else
            CopyGeoTIFF(in,out);

        if( worldfile )
            ApplyWorldFile( worldfile, out);

	cf = pickCopyFunc(in, out, bitspersample, samplesperpixel);
	return (cf ? (*cf)(in, out, l, w, samplesperpixel) : FALSE);
}

/*
 * Copy Functions.
 */
#define	DECLAREcpFunc(x) \
static int x(TIFF* in, TIFF* out, \
    uint32 imagelength, uint32 imagewidth, tsample_t spp)

#define	DECLAREreadFunc(x) \
static void x(TIFF* in, \
    unsigned char* buf, uint32 imagelength, uint32 imagewidth, tsample_t spp)
typedef void (*readFunc)(TIFF*, unsigned char*, uint32, uint32, tsample_t);

#define	DECLAREwriteFunc(x) \
static int x(TIFF* out, \
    unsigned char* buf, uint32 imagelength, uint32 imagewidth, tsample_t spp)
typedef int (*writeFunc)(TIFF*, unsigned char*, uint32, uint32, tsample_t);

/*
 * Contig -> contig by scanline for rows/strip change.
 */
DECLAREcpFunc(cpContig2ContigByRow)
{
	unsigned char *buf = (unsigned char *)_TIFFmalloc(TIFFScanlineSize(in));
	uint32 row;

	(void) imagewidth; (void) spp;
	for (row = 0; row < imagelength; row++) {
		if (TIFFReadScanline(in, buf, row, 0) < 0 && !ignore)
			goto done;
		if (TIFFWriteScanline(out, buf, row, 0) < 0)
			goto bad;
	}
done:
	_TIFFfree(buf);
	return (TRUE);
bad:
	_TIFFfree(buf);
	return (FALSE);
}

/*
 * Contig -> contig by scanline for rows/strip change.
 */
DECLAREcpFunc(cpContig2ContigByRow_8_to_4)
{
    unsigned char *buf_in = (unsigned char *)_TIFFmalloc(TIFFScanlineSize(in));
    unsigned char *buf_out = (unsigned char *)_TIFFmalloc(TIFFScanlineSize(out));
    uint32 row;

    printf( "Downsample\n" );

    (void) imagewidth; (void) spp;
    for (row = 0; row < imagelength; row++) {
        int i_in, i_out_byte;
            
        if (TIFFReadScanline(in, buf_in, row, 0) < 0 && !ignore)
            goto done;

        for( i_in = 0, i_out_byte = 0;
             i_in < imagewidth;
             i_in += 2, i_out_byte++ )
        {
            buf_out[i_out_byte] =
                (buf_in[i_in] & 0xf)*16 + (buf_in[i_in+1] & 0xf);
        }
        
        if (TIFFWriteScanline(out, buf_out, row, 0) < 0)
            goto bad;
    }
  done:
    _TIFFfree(buf_in);
    _TIFFfree(buf_out);
    return (TRUE);
  bad:
    _TIFFfree(buf_in);
    _TIFFfree(buf_out);
    return (FALSE);
}

/*
 * Strip -> strip for change in encoding.
 */
DECLAREcpFunc(cpDecodedStrips)
{
	tsize_t stripsize  = TIFFStripSize(in);
	unsigned char *buf = (unsigned char *)_TIFFmalloc(stripsize);

	(void) imagewidth; (void) spp;
	if (buf) {
		tstrip_t s, ns = TIFFNumberOfStrips(in);
		uint32 row = 0;
		for (s = 0; s < ns; s++) {
			tsize_t cc = (row + rowsperstrip > imagelength) ?
			    TIFFVStripSize(in, imagelength - row) : stripsize;
			if (TIFFReadEncodedStrip(in, s, buf, cc) < 0 && !ignore)
				break;
			if (TIFFWriteEncodedStrip(out, s, buf, cc) < 0) {
				_TIFFfree(buf);
				return (FALSE);
			}
			row += rowsperstrip;
		}
		_TIFFfree(buf);
		return (TRUE);
	}
	return (FALSE);
}

/*
 * Separate -> separate by row for rows/strip change.
 */
DECLAREcpFunc(cpSeparate2SeparateByRow)
{
	unsigned char *buf = (unsigned char *)_TIFFmalloc(TIFFScanlineSize(in));
	uint32 row;
	tsample_t s;

	(void) imagewidth;
	for (s = 0; s < spp; s++) {
		for (row = 0; row < imagelength; row++) {
			if (TIFFReadScanline(in, buf, row, s) < 0 && !ignore)
				goto done;
			if (TIFFWriteScanline(out, buf, row, s) < 0)
				goto bad;
		}
	}
done:
	_TIFFfree(buf);
	return (TRUE);
bad:
	_TIFFfree(buf);
	return (FALSE);
}

/*
 * Contig -> separate by row.
 */
DECLAREcpFunc(cpContig2SeparateByRow)
{
	unsigned char *inbuf = (unsigned char *)_TIFFmalloc(TIFFScanlineSize(in));
	unsigned char *outbuf = (unsigned char *)_TIFFmalloc(TIFFScanlineSize(out));
	register unsigned char *inp, *outp;
	register uint32 n;
	uint32 row;
	tsample_t s;

	/* unpack channels */
	for (s = 0; s < spp; s++) {
		for (row = 0; row < imagelength; row++) {
			if (TIFFReadScanline(in, inbuf, row, 0) < 0 && !ignore)
				goto done;
			inp = inbuf + s;
			outp = outbuf;
			for (n = imagewidth; n-- > 0;) {
				*outp++ = *inp;
				inp += spp;
			}
			if (TIFFWriteScanline(out, outbuf, row, s) < 0)
				goto bad;
		}
	}
done:
	if (inbuf) _TIFFfree(inbuf);
	if (outbuf) _TIFFfree(outbuf);
	return (TRUE);
bad:
	if (inbuf) _TIFFfree(inbuf);
	if (outbuf) _TIFFfree(outbuf);
	return (FALSE);
}

/*
 * Separate -> contig by row.
 */
DECLAREcpFunc(cpSeparate2ContigByRow)
{
	unsigned char *inbuf = (unsigned char *)_TIFFmalloc(TIFFScanlineSize(in));
	unsigned char *outbuf = (unsigned char *)_TIFFmalloc(TIFFScanlineSize(out));
	register unsigned char *inp, *outp;
	register uint32 n;
	uint32 row;
	tsample_t s;

	for (row = 0; row < imagelength; row++) {
		/* merge channels */
		for (s = 0; s < spp; s++) {
			if (TIFFReadScanline(in, inbuf, row, s) < 0 && !ignore)
				goto done;
			inp = inbuf;
			outp = outbuf + s;
			for (n = imagewidth; n-- > 0;) {
				*outp = *inp++;
				outp += spp;
			}
		}
		if (TIFFWriteScanline(out, outbuf, row, 0) < 0)
			goto bad;
	}
done:
	if (inbuf) _TIFFfree(inbuf);
	if (outbuf) _TIFFfree(outbuf);
	return (TRUE);
bad:
	if (inbuf) _TIFFfree(inbuf);
	if (outbuf) _TIFFfree(outbuf);
	return (FALSE);
}

static void
cpStripToTile(unsigned char* out, unsigned char* in,
	uint32 rows, uint32 cols, int outskew, int inskew)
{
	while (rows-- > 0) {
		uint32 j = cols;
		while (j-- > 0)
			*out++ = *in++;
		out += outskew;
		in += inskew;
	}
}

static void
cpContigBufToSeparateBuf(unsigned char* out, unsigned char* in,
	uint32 rows, uint32 cols, int outskew, int inskew, tsample_t spp)
{
	while (rows-- > 0) {
		uint32 j = cols;
		while (j-- > 0)
			*out++ = *in, in += spp;
		out += outskew;
		in += inskew;
	}
}

static void
cpSeparateBufToContigBuf(unsigned char* out, unsigned char* in,
	uint32 rows, uint32 cols, int outskew, int inskew, tsample_t spp)
{
	while (rows-- > 0) {
		uint32 j = cols;
		while (j-- > 0)
			*out = *in++, out += spp;
		out += outskew;
		in += inskew;
	}
}

static int
cpImage(TIFF* in, TIFF* out, readFunc fin, writeFunc fout,
	uint32 imagelength, uint32 imagewidth, tsample_t spp)
{
	int status = FALSE;
	unsigned char* buf = (unsigned char *)
	    _TIFFmalloc(TIFFRasterScanlineSize(in) * imagelength);
	if (buf) {
		(*fin)(in, buf, imagelength, imagewidth, spp);
		status = (fout)(out, buf, imagelength, imagewidth, spp);
		_TIFFfree(buf);
	}
	return (status);
}

DECLAREreadFunc(readContigStripsIntoBuffer)
{
	tsize_t scanlinesize = TIFFScanlineSize(in);
     	unsigned char *bufp = buf;
	uint32 row;

	(void) imagewidth; (void) spp;
	for (row = 0; row < imagelength; row++) {
		if (TIFFReadScanline(in, bufp, row, 0) < 0 && !ignore)
			break;
		bufp += scanlinesize;
	}
}

DECLAREreadFunc(readSeparateStripsIntoBuffer)
{
	tsize_t scanlinesize = TIFFScanlineSize(in);
	unsigned char* scanline = (unsigned char *) _TIFFmalloc(scanlinesize);

	(void) imagewidth;
	if (scanline) {
		unsigned char *bufp = buf;
		uint32 row;
		tsample_t s;

		for (row = 0; row < imagelength; row++) {
			/* merge channels */
			for (s = 0; s < spp; s++) {
				unsigned char* sp = scanline;
				unsigned char* bp = bufp + s;
				tsize_t n = scanlinesize;

				if (TIFFReadScanline(in, sp, row, s) < 0 && !ignore)
					goto done;
				while (n-- > 0)
					*bp = *bufp++, bp += spp;
			}
			bufp += scanlinesize;
		}
done:
		_TIFFfree(scanline);
	}
}

DECLAREreadFunc(readContigTilesIntoBuffer)
{
	unsigned char* tilebuf = (unsigned char *) _TIFFmalloc(TIFFTileSize(in));
	uint32 imagew = TIFFScanlineSize(in);
	uint32 tilew  = TIFFTileRowSize(in);
	int iskew = imagew - tilew;
	unsigned char *bufp = buf;
	uint32 tw, tl;
	uint32 row;

	(void) spp;
	if (tilebuf == 0)
		return;
	(void) TIFFGetField(in, TIFFTAG_TILEWIDTH, &tw);
	(void) TIFFGetField(in, TIFFTAG_TILELENGTH, &tl);
	for (row = 0; row < imagelength; row += tl) {
		uint32 nrow = (row+tl > imagelength) ? imagelength-row : tl;
		uint32 colb = 0;
		uint32 col;

		for (col = 0; col < imagewidth; col += tw) {
			if (TIFFReadTile(in, tilebuf, col, row, 0, 0) < 0 &&
			    !ignore)
				goto done;
			if (colb + tilew > imagew) {
				uint32 width = imagew - colb;
				uint32 oskew = tilew - width;
				cpStripToTile(bufp + colb,
					tilebuf, nrow, width,
					oskew + iskew, oskew);
			} else
				cpStripToTile(bufp + colb,
					tilebuf, nrow, tilew,
					iskew, 0);
			colb += tilew;
		}
		bufp += imagew * nrow;
	}
done:
	_TIFFfree(tilebuf);
}

DECLAREreadFunc(readSeparateTilesIntoBuffer)
{
	uint32 imagew = TIFFScanlineSize(in);
	uint32 tilew = TIFFTileRowSize(in);
	int iskew  = imagew - tilew;
	unsigned char* tilebuf = (unsigned char *) _TIFFmalloc(TIFFTileSize(in));
	unsigned char *bufp = buf;
	uint32 tw, tl;
	uint32 row;

	if (tilebuf == 0)
		return;
	(void) TIFFGetField(in, TIFFTAG_TILEWIDTH, &tw);
	(void) TIFFGetField(in, TIFFTAG_TILELENGTH, &tl);
	for (row = 0; row < imagelength; row += tl) {
		uint32 nrow = (row+tl > imagelength) ? imagelength-row : tl;
		uint32 colb = 0;
		uint32 col;

		for (col = 0; col < imagewidth; col += tw) {
			tsample_t s;

			for (s = 0; s < spp; s++) {
				if (TIFFReadTile(in, tilebuf, col, row, 0, s) < 0 && !ignore)
					goto done;
				/*
				 * Tile is clipped horizontally.  Calculate
				 * visible portion and skewing factors.
				 */
				if (colb + tilew > imagew) {
					uint32 width = imagew - colb;
					int oskew = tilew - width;
					cpSeparateBufToContigBuf(bufp+colb+s,
					    tilebuf, nrow, width,
					    oskew + iskew, oskew, spp);
				} else
					cpSeparateBufToContigBuf(bufp+colb+s,
					    tilebuf, nrow, tw,
					    iskew, 0, spp);
			}
			colb += tilew;
		}
		bufp += imagew * nrow;
	}
done:
	_TIFFfree(tilebuf);
}

DECLAREwriteFunc(writeBufferToContigStrips)
{
	tsize_t scanline = TIFFScanlineSize(out);
	uint32 row;

	(void) imagewidth; (void) spp;
	for (row = 0; row < imagelength; row++) {
		if (TIFFWriteScanline(out, buf, row, 0) < 0)
			return (FALSE);
		buf += scanline;
	}
	return (TRUE);
}

DECLAREwriteFunc(writeBufferToSeparateStrips)
{
	unsigned char *obuf = (unsigned char *) _TIFFmalloc(TIFFScanlineSize(out));
	tsample_t s;

	if (obuf == NULL)
		return (0);
	for (s = 0; s < spp; s++) {
		uint32 row;
		for (row = 0; row < imagelength; row++) {
			unsigned char* inp = buf + s;
			unsigned char* outp = obuf;
			uint32 n = imagewidth;

			while (n-- > 0)
				*outp++ = *inp, inp += spp;
			if (TIFFWriteScanline(out, obuf, row, s) < 0) {
				_TIFFfree(obuf);
				return (FALSE);
			}
		}
	}
	_TIFFfree(obuf);
	return (TRUE);

}

DECLAREwriteFunc(writeBufferToContigTiles)
{
	uint32 imagew = TIFFScanlineSize(out);
	uint32 tilew  = TIFFTileRowSize(out);
	int iskew = imagew - tilew;
	unsigned char* obuf = (unsigned char *) _TIFFmalloc(TIFFTileSize(out));
	unsigned char* bufp = buf;
	uint32 tl, tw;
	uint32 row;

	(void) spp;
	if (obuf == NULL)
		return (FALSE);
	(void) TIFFGetField(out, TIFFTAG_TILELENGTH, &tl);
	(void) TIFFGetField(out, TIFFTAG_TILEWIDTH, &tw);
	for (row = 0; row < imagelength; row += tilelength) {
		uint32 nrow = (row+tl > imagelength) ? imagelength-row : tl;
		uint32 colb = 0;
		uint32 col;

		for (col = 0; col < imagewidth; col += tw) {
			/*
			 * Tile is clipped horizontally.  Calculate
			 * visible portion and skewing factors.
			 */
			if (colb + tilew > imagew) {
				uint32 width = imagew - colb;
				int oskew = tilew - width;
				cpStripToTile(obuf, bufp + colb, nrow, width,
				    oskew, oskew + iskew);
			} else
				cpStripToTile(obuf, bufp + colb, nrow, tilew,
				    0, iskew);
			if (TIFFWriteTile(out, obuf, col, row, 0, 0) < 0) {
				_TIFFfree(obuf);
				return (FALSE);
			}
			colb += tilew;
		}
		bufp += nrow * imagew;
	}
	_TIFFfree(obuf);
	return (TRUE);
}

DECLAREwriteFunc(writeBufferToSeparateTiles)
{
	uint32 imagew = TIFFScanlineSize(out);
	tsize_t tilew  = TIFFTileRowSize(out);
	int iskew = imagew - tilew;
	unsigned char *obuf = (unsigned char*) _TIFFmalloc(TIFFTileSize(out));
	unsigned char *bufp = buf;
	uint32 tl, tw;
	uint32 row;

	if (obuf == NULL)
		return (FALSE);
	(void) TIFFGetField(out, TIFFTAG_TILELENGTH, &tl);
	(void) TIFFGetField(out, TIFFTAG_TILEWIDTH, &tw);
	for (row = 0; row < imagelength; row += tl) {
		uint32 nrow = (row+tl > imagelength) ? imagelength-row : tl;
		uint32 colb = 0;
		uint32 col;

		for (col = 0; col < imagewidth; col += tw) {
			tsample_t s;
			for (s = 0; s < spp; s++) {
				/*
				 * Tile is clipped horizontally.  Calculate
				 * visible portion and skewing factors.
				 */
				if (colb + tilew > imagew) {
					uint32 width = imagew - colb;
					int oskew = tilew - width;

					cpContigBufToSeparateBuf(obuf,
					    bufp + colb + s,
					    nrow, width,
					    oskew/spp, oskew + imagew, spp);
				} else
					cpContigBufToSeparateBuf(obuf,
					    bufp + colb + s,
					    nrow, tilewidth,
					    0, iskew, spp);
				if (TIFFWriteTile(out, obuf, col, row, 0, s) < 0) {
					_TIFFfree(obuf);
					return (FALSE);
				}
			}
			colb += tilew;
		}
		bufp += nrow * imagew;
	}
	_TIFFfree(obuf);
	return (TRUE);
}

/*
 * Contig strips -> contig tiles.
 */
DECLAREcpFunc(cpContigStrips2ContigTiles)
{
	return cpImage(in, out,
	    readContigStripsIntoBuffer,
	    writeBufferToContigTiles,
	    imagelength, imagewidth, spp);
}

/*
 * Contig strips -> separate tiles.
 */
DECLAREcpFunc(cpContigStrips2SeparateTiles)
{
	return cpImage(in, out,
	    readContigStripsIntoBuffer,
	    writeBufferToSeparateTiles,
	    imagelength, imagewidth, spp);
}

/*
 * Separate strips -> contig tiles.
 */
DECLAREcpFunc(cpSeparateStrips2ContigTiles)
{
	return cpImage(in, out,
	    readSeparateStripsIntoBuffer,
	    writeBufferToContigTiles,
	    imagelength, imagewidth, spp);
}

/*
 * Separate strips -> separate tiles.
 */
DECLAREcpFunc(cpSeparateStrips2SeparateTiles)
{
	return cpImage(in, out,
	    readSeparateStripsIntoBuffer,
	    writeBufferToSeparateTiles,
	    imagelength, imagewidth, spp);
}

/*
 * Contig strips -> contig tiles.
 */
DECLAREcpFunc(cpContigTiles2ContigTiles)
{
	return cpImage(in, out,
	    readContigTilesIntoBuffer,
	    writeBufferToContigTiles,
	    imagelength, imagewidth, spp);
}

/*
 * Contig tiles -> separate tiles.
 */
DECLAREcpFunc(cpContigTiles2SeparateTiles)
{
	return cpImage(in, out,
	    readContigTilesIntoBuffer,
	    writeBufferToSeparateTiles,
	    imagelength, imagewidth, spp);
}

/*
 * Separate tiles -> contig tiles.
 */
DECLAREcpFunc(cpSeparateTiles2ContigTiles)
{
	return cpImage(in, out,
	    readSeparateTilesIntoBuffer,
	    writeBufferToContigTiles,
	    imagelength, imagewidth, spp);
}

/*
 * Separate tiles -> separate tiles (tile dimension change).
 */
DECLAREcpFunc(cpSeparateTiles2SeparateTiles)
{
	return cpImage(in, out,
	    readSeparateTilesIntoBuffer,
	    writeBufferToSeparateTiles,
	    imagelength, imagewidth, spp);
}

/*
 * Contig tiles -> contig tiles (tile dimension change).
 */
DECLAREcpFunc(cpContigTiles2ContigStrips)
{
	return cpImage(in, out,
	    readContigTilesIntoBuffer,
	    writeBufferToContigStrips,
	    imagelength, imagewidth, spp);
}

/*
 * Contig tiles -> separate strips.
 */
DECLAREcpFunc(cpContigTiles2SeparateStrips)
{
	return cpImage(in, out,
	    readContigTilesIntoBuffer,
	    writeBufferToSeparateStrips,
	    imagelength, imagewidth, spp);
}

/*
 * Separate tiles -> contig strips.
 */
DECLAREcpFunc(cpSeparateTiles2ContigStrips)
{
	return cpImage(in, out,
	    readSeparateTilesIntoBuffer,
	    writeBufferToContigStrips,
	    imagelength, imagewidth, spp);
}

/*
 * Separate tiles -> separate strips.
 */
DECLAREcpFunc(cpSeparateTiles2SeparateStrips)
{
	return cpImage(in, out,
	    readSeparateTilesIntoBuffer,
	    writeBufferToSeparateStrips,
	    imagelength, imagewidth, spp);
}

/*
 * Select the appropriate copy function to use.
 */
static copyFunc
pickCopyFunc(TIFF* in, TIFF* out, uint16 bitspersample, uint16 samplesperpixel)
{
	uint16 shortv;
	uint32 w, l, tw, tl;
	int bychunk;

	(void) TIFFGetField(in, TIFFTAG_PLANARCONFIG, &shortv);
	if (shortv != config && bitspersample != 8 && samplesperpixel > 1) {
		fprintf(stderr,
"%s: Can not handle different planar configuration w/ bits/sample != 8\n",
		    TIFFFileName(in));
		return (NULL);
	}
	TIFFGetField(in, TIFFTAG_IMAGEWIDTH, &w);
	TIFFGetField(in, TIFFTAG_IMAGELENGTH, &l);
	if (TIFFIsTiled(out)) {
		if (!TIFFGetField(in, TIFFTAG_TILEWIDTH, &tw))
			tw = w;
		if (!TIFFGetField(in, TIFFTAG_TILELENGTH, &tl))
			tl = l;
		bychunk = (tw == tilewidth && tl == tilelength);
	} else if (TIFFIsTiled(in)) {
		TIFFGetField(in, TIFFTAG_TILEWIDTH, &tw);
		TIFFGetField(in, TIFFTAG_TILELENGTH, &tl);
		bychunk = (tw == w && tl == rowsperstrip);
	} else {
		uint32 irps = (uint32) -1L;
		TIFFGetField(in, TIFFTAG_ROWSPERSTRIP, &irps);
		bychunk = (rowsperstrip == irps);
	}
#define	T 1
#define	F 0
#define pack(a,b,c,d,e)	((long)(((a)<<11)|((b)<<3)|((c)<<2)|((d)<<1)|(e)))
	switch(pack(shortv,config,TIFFIsTiled(in),TIFFIsTiled(out),bychunk)) {
/* Strips -> Tiles */
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_CONTIG,   F,T,F):
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_CONTIG,   F,T,T):
		return cpContigStrips2ContigTiles;
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_SEPARATE, F,T,F):
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_SEPARATE, F,T,T):
		return cpContigStrips2SeparateTiles;
        case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_CONTIG,   F,T,F):
        case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_CONTIG,   F,T,T):
		return cpSeparateStrips2ContigTiles;
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_SEPARATE, F,T,F):
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_SEPARATE, F,T,T):
		return cpSeparateStrips2SeparateTiles;
/* Tiles -> Tiles */
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_CONTIG,   T,T,F):
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_CONTIG,   T,T,T):
		return cpContigTiles2ContigTiles;
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_SEPARATE, T,T,F):
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_SEPARATE, T,T,T):
		return cpContigTiles2SeparateTiles;
        case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_CONTIG,   T,T,F):
        case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_CONTIG,   T,T,T):
		return cpSeparateTiles2ContigTiles;
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_SEPARATE, T,T,F):
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_SEPARATE, T,T,T):
		return cpSeparateTiles2SeparateTiles;
/* Tiles -> Strips */
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_CONTIG,   T,F,F):
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_CONTIG,   T,F,T):
		return cpContigTiles2ContigStrips;
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_SEPARATE, T,F,F):
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_SEPARATE, T,F,T):
		return cpContigTiles2SeparateStrips;
        case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_CONTIG,   T,F,F):
        case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_CONTIG,   T,F,T):
		return cpSeparateTiles2ContigStrips;
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_SEPARATE, T,F,F):
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_SEPARATE, T,F,T):
		return cpSeparateTiles2SeparateStrips;
/* Strips -> Strips */
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_CONTIG,   F,F,F):
          if( convert_8_to_4 )
              return cpContig2ContigByRow_8_to_4;
          else
              return cpContig2ContigByRow;
          
	case pack(PLANARCONFIG_CONTIG,   PLANARCONFIG_CONTIG,   F,F,T):
          if( convert_8_to_4 )
              return cpContig2ContigByRow_8_to_4;
          else
              return cpDecodedStrips;
	case pack(PLANARCONFIG_CONTIG, PLANARCONFIG_SEPARATE,   F,F,F):
	case pack(PLANARCONFIG_CONTIG, PLANARCONFIG_SEPARATE,   F,F,T):
		return cpContig2SeparateByRow;
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_CONTIG,   F,F,F):
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_CONTIG,   F,F,T):
		return cpSeparate2ContigByRow;
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_SEPARATE, F,F,F):
	case pack(PLANARCONFIG_SEPARATE, PLANARCONFIG_SEPARATE, F,F,T):
		return cpSeparate2SeparateByRow;
	}
#undef pack
#undef F
#undef T
	fprintf(stderr, "tiffcp: %s: Don't know how to copy/convert image.\n",
	    TIFFFileName(in));
	return (NULL);
}
