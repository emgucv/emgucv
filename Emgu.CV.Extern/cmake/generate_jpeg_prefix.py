#!/usr/bin/env python3
"""Generate a libjpeg-turbo symbol-rename header (jpegprefix.h) from its headers.

Unlike libpng, libjpeg-turbo has no built-in PNG_PREFIX-equivalent mechanism.
This instead force-includes the generated header (via -include) ahead of
every libjpeg-turbo/opencv_imgcodecs translation unit, relying on plain C
preprocessor text substitution: #define jpeg_foo emgu_jpeg_foo renames every
occurrence of jpeg_foo -- declaration, definition, and call sites alike --
throughout the compiled sources.

Scans every header under libjpeg-turbo's source directory (not just
jpeglib.h) for the EXTERN(...) linkage macro, since libjpeg-turbo uses it
both for the true public API (jpeglib.h) and for symbols that only need
external linkage between its own .c files (jpegint.h, jchuff.h, jsimd.h,
etc.) -- renaming all of them is what actually guarantees no collision,
not just the handful of symbols a given link happens to report as
duplicates.

Two more symbol sources beyond plain EXTERN(...) declarations, found by
actually tracing a real duplicate-symbol report down to its root cause
(the first version of this script didn't catch either):

- A handful of public data tables (jpeg_natural_order, jpeg_zigzag_order,
  jpeg_aritab, jpeg_nbits_table) are declared with a plain `extern`, not
  the EXTERN(...) macro, since they're not functions.
- jsamplecomp.h -- libjpeg-turbo's 8/12/16-bit sample-depth dispatch layer
  -- redefines the underscore-prefixed EXTERN(...) names (_jpeg_idct_islow
  etc.) to bit-depth-specific final names (jpeg_idct_islow / jpeg12_idct_islow
  / jpeg16_idct_islow) *after* this header is force-included, silently
  overriding the underscore-name rename for anything that goes through it.
  The right-hand side of those redefinitions is what actually ends up in
  the compiled object file, so those need renaming too, not the
  underscore-prefixed left-hand names. jpegapicomp.h has a similarly-shaped
  `#define _jpeg_foo bar` block, but its targets (image_width, jpeg_width,
  etc.) are struct field access macros, not linker symbols -- deliberately
  not scanned here, since renaming those would rewrite unrelated struct
  member access throughout the codebase rather than a symbol name.
"""
import glob
import os
import re
import sys


def main():
    if len(sys.argv) != 4:
        print("Usage: generate_jpeg_prefix.py <libjpeg-turbo-src-dir> <output> <prefix>", file=sys.stderr)
        return 1
    src_dir, output_path, prefix = sys.argv[1], sys.argv[2], sys.argv[3]
    extern_pattern = re.compile(r'^EXTERN\([^)]*\)\s*\*?\s*([A-Za-z_][A-Za-z0-9_]*)\s*\(', re.MULTILINE)
    data_pattern = re.compile(r'^extern\s+(?:const\s+)?[A-Za-z_][A-Za-z0-9_]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*(?:\[[^\]]*\])?\s*;', re.MULTILINE)
    alias_pattern = re.compile(r'^#define\s+_jpeg[A-Za-z0-9_]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*$', re.MULTILINE)
    names = set()
    for path in sorted(glob.glob(os.path.join(src_dir, "*.h"))):
        with open(path, "r") as f:
            text = f.read()
        names.update(extern_pattern.findall(text))
        names.update(data_pattern.findall(text))
        if os.path.basename(path) == "jsamplecomp.h":
            names.update(alias_pattern.findall(text))
    names = sorted(names)
    if not names:
        print("ERROR: no EXTERN(...) symbols found under " + src_dir, file=sys.stderr)
        return 1
    with open(output_path, "w") as f:
        f.write("#ifndef EMGU_JPEG_PREFIX_H\n#define EMGU_JPEG_PREFIX_H\n\n")
        for name in names:
            f.write("#define {0} {1}{0}\n".format(name, prefix))
        f.write("\n#endif /* EMGU_JPEG_PREFIX_H */\n")
    print("Generated {0} with {1} renamed symbols".format(output_path, len(names)))
    return 0


if __name__ == "__main__":
    sys.exit(main())
