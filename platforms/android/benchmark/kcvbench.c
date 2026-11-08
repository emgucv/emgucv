// Times OpenCV functions accelerated by KleidiCV through libcvextern.so's C API.
// Usage: kcvbench <path/to/libcvextern.so> [threads]
#include <dlfcn.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include <float.h>

typedef void Mat; typedef void IA; typedef void OA; typedef void IOA; typedef void Str;
typedef struct { int width, height; } Size;
typedef struct { int x, y; } Point;
typedef struct { double v[4]; } Scalar;

static Mat* (*MatCreate)(void);
static Mat* (*MatCreateWithData)(int, int, int, void*, size_t);
static IA* (*InFromMat)(Mat*);
static OA* (*OutFromMat)(Mat*);
static void (*SetNumThreads)(int);
static Str* (*StringCreate)(void);
static void (*StringGetCStr)(Str*, const char**, int*);
static void (*GetBuildInformation)(Str*);
static void (*CvtColor)(IA*, OA*, int, int, int);
static void (*Resize)(IA*, OA*, Size*, double, double, int);
static void (*GaussianBlur)(IA*, OA*, Size*, double, double, int, int);
static void (*Sobel)(IA*, OA*, int, int, int, int, double, double, int);
static void (*MedianBlur)(IA*, OA*, int);
static void (*Dilate)(IA*, OA*, IA*, Point*, int, int, Scalar*);
static void (*Erode)(IA*, OA*, IA*, Point*, int, int, Scalar*);
static void (*PyrDown)(IA*, OA*, Size*, int);
static void (*WarpPerspective)(IA*, OA*, IA*, Size*, int, int, Scalar*);
static void (*Flip)(IA*, OA*, int);
static void (*Transpose)(IA*, OA*);
static int (*CheckPendingError)(int*, Str*, Str*, Str*, int*);
static void (*MatGetSize)(Mat*, Size*);
static Mat* dst_mat;

#define LOAD(var, name) do { *(void**)&var = dlsym(h, name); if (!var) { fprintf(stderr, "missing %s\n", name); exit(1); } } while (0)

static double now_ms(void) { struct timespec t; clock_gettime(CLOCK_MONOTONIC, &t); return t.tv_sec * 1e3 + t.tv_nsec / 1e6; }
static int cmp(const void* a, const void* b) { double x = *(const double*)a, y = *(const double*)b; return (x > y) - (x < y); }

enum { W = 1920, H = 1080, CV_8UC1 = 0, CV_8UC3 = (3 - 1) << 5 /* OpenCV 5: CV_CN_SHIFT=5 */, CV_16S = 3, CV_64FC1 = 6 };
static OA* dst_oa;
static IA *gray_in, *bgr_in, *nv21_in, *empty_in, *persp_in;

static void run(const char* name, void (*fn)(void)) {
  enum { WARM = 5, N = 41 };
  double t[N];
  dst_mat = MatCreate();
  dst_oa = OutFromMat(dst_mat);  // reused across iterations, like a real pipeline
  fn();
  int status = 0, line = 0; Str *fn_name = StringCreate(), *msg = StringCreate(), *file = StringCreate();
  if (CheckPendingError(&status, fn_name, msg, file, &line)) {
    const char* m; int mn; StringGetCStr(msg, &m, &mn);
    printf("%-34s ERROR %d: %.*s\n", name, status, mn, m);
    return;
  }
  Size ds; MatGetSize(dst_mat, &ds);
  for (int i = 0; i < WARM; i++) fn();
  for (int i = 0; i < N; i++) { double s = now_ms(); fn(); t[i] = now_ms() - s; }
  qsort(t, N, sizeof(double), cmp);
  printf("%-34s %8.3f ms  (dst %dx%d)\n", name, t[N / 2], ds.width, ds.height);
}

static OA* out(void) { return dst_oa; }
static void b_gray(void) { CvtColor(bgr_in, out(), 6 /*BGR2GRAY*/, 0, 0); }
static void b_rgb(void) { CvtColor(bgr_in, out(), 4 /*BGR2RGB*/, 0, 0); }
static void b_nv21(void) { CvtColor(nv21_in, out(), 93 /*YUV2BGR_NV21*/, 0, 0); }
static void b_resize_down(void) { Size s = {W / 2, H / 2}; Resize(bgr_in, out(), &s, 0, 0, 1 /*LINEAR*/); }
static void b_resize_up(void) { Size s = {W * 2, H * 2}; Resize(gray_in, out(), &s, 0, 0, 1); }
static void b_gauss(void) { Size k = {5, 5}; GaussianBlur(gray_in, out(), &k, 0, 0, 4 /*REFLECT_101*/, 0); }
static void b_sobel(void) { Sobel(gray_in, out(), CV_16S, 1, 0, 3, 1, 0, 4); }
static void b_median(void) { MedianBlur(gray_in, out(), 5); }
static void b_dilate(void) { Point a = {-1, -1}; Scalar v = {{DBL_MAX, DBL_MAX, DBL_MAX, DBL_MAX}}; Dilate(gray_in, out(), empty_in, &a, 1, 0 /*CONSTANT*/, &v); }
static void b_erode(void) { Point a = {-1, -1}; Scalar v = {{DBL_MAX, DBL_MAX, DBL_MAX, DBL_MAX}}; Erode(gray_in, out(), empty_in, &a, 1, 0, &v); }
static void b_pyrdown(void) { Size s = {0, 0}; PyrDown(gray_in, out(), &s, 4); }
static void b_warp(void) { Size s = {W, H}; Scalar v = {{0}}; WarpPerspective(gray_in, out(), persp_in, &s, 1 /*LINEAR*/, 0, &v); }
static void b_flip(void) { Flip(bgr_in, out(), 1); }
static void b_transpose(void) { Transpose(gray_in, out()); }

int main(int argc, char** argv) {
  if (argc < 2) { fprintf(stderr, "usage: %s libcvextern.so [threads]\n", argv[0]); return 1; }
  void* h = dlopen(argv[1], RTLD_NOW);
  if (!h) { fprintf(stderr, "dlopen: %s\n", dlerror()); return 1; }
  LOAD(MatCreate, "cveMatCreate"); LOAD(MatCreateWithData, "cveMatCreateWithData");
  LOAD(InFromMat, "cveInputArrayFromMat"); LOAD(OutFromMat, "cveOutputArrayFromMat");
  LOAD(SetNumThreads, "cveSetNumThreads"); LOAD(StringCreate, "cveStringCreate");
  LOAD(StringGetCStr, "cveStringGetCStr"); LOAD(GetBuildInformation, "cveGetBuildInformation");
  LOAD(CvtColor, "cveCvtColor"); LOAD(Resize, "cveResize"); LOAD(GaussianBlur, "cveGaussianBlur");
  LOAD(Sobel, "cveSobel"); LOAD(MedianBlur, "cveMedianBlur"); LOAD(Dilate, "cveDilate");
  LOAD(Erode, "cveErode"); LOAD(PyrDown, "cvePyrDown"); LOAD(WarpPerspective, "cveWarpPerspective");
  LOAD(Flip, "cveFlip"); LOAD(Transpose, "cveTranspose");
  LOAD(CheckPendingError, "cveCheckPendingError"); LOAD(MatGetSize, "cveMatGetSize");

  Str* info = StringCreate(); GetBuildInformation(info);
  const char* s; int n; StringGetCStr(info, &s, &n);
  const char* hal = strstr(s, "Custom HAL:");
  if (hal) { const char* e = strchr(hal, '\n'); printf("%.*s\n", (int)(e ? e - hal : 40), hal); }
  else printf("Custom HAL: (none listed)\n");

  int threads = argc > 2 ? atoi(argv[2]) : 1;
  SetNumThreads(threads);
  printf("threads: %d, image: %dx%d, median of 41 runs\n", threads, W, H);

  srand(42);
  unsigned char* g = malloc(W * H); unsigned char* c = malloc(W * H * 3); unsigned char* y = malloc(W * H * 3 / 2);
  for (int i = 0; i < W * H; i++) g[i] = rand();
  for (int i = 0; i < W * H * 3; i++) c[i] = rand();
  for (int i = 0; i < W * H * 3 / 2; i++) y[i] = rand();
  static double persp[9] = {0.98, 0.05, 10, -0.04, 0.99, 12, 0.00001, 0.00002, 1};
  gray_in = InFromMat(MatCreateWithData(H, W, CV_8UC1, g, W));
  bgr_in = InFromMat(MatCreateWithData(H, W, CV_8UC3, c, W * 3));
  nv21_in = InFromMat(MatCreateWithData(H * 3 / 2, W, CV_8UC1, y, W));
  persp_in = InFromMat(MatCreateWithData(3, 3, CV_64FC1, persp, 3 * sizeof(double)));
  empty_in = InFromMat(MatCreate());

  run("cvtColor BGR->GRAY (8UC3)", b_gray);
  run("cvtColor BGR->RGB (8UC3)", b_rgb);
  run("cvtColor NV21->BGR", b_nv21);
  run("resize 1/2 linear (8UC3)", b_resize_down);
  run("resize 2x linear (8UC1)", b_resize_up);
  run("GaussianBlur 5x5 (8UC1)", b_gauss);
  run("Sobel dx 3x3 ->16S (8UC1)", b_sobel);
  run("medianBlur 5 (8UC1)", b_median);
  run("dilate 3x3 (8UC1)", b_dilate);
  run("erode 3x3 (8UC1)", b_erode);
  run("pyrDown (8UC1)", b_pyrdown);
  run("warpPerspective linear (8UC1)", b_warp);
  run("flip horizontal (8UC3)", b_flip);
  run("transpose (8UC1)", b_transpose);
  return 0;
}
