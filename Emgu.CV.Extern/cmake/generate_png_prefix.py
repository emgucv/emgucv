#!/usr/bin/env python3
"""Generate a libpng PNG_PREFIX header (pngprefix.h) from png.h.

libpng supports renaming every public symbol via the PNG_PREFIX mechanism
(see pngpriv.h: #if defined(PNG_PREFIX) ... #include "pngprefix.h"). This
avoids "duplicate symbol" link errors when a second copy of libpng (linked
into cvextern.a) is combined with a host application that already statically
links its own libpng -- e.g. Unity's WebGL player runtime.
"""
import re
import sys


def main():
    if len(sys.argv) != 4:
        print("Usage: generate_png_prefix.py <png.h> <output> <prefix>", file=sys.stderr)
        return 1
    png_h_path, output_path, prefix = sys.argv[1], sys.argv[2], sys.argv[3]
    with open(png_h_path, "r") as f:
        text = f.read()
    pattern = re.compile(r'^PNG_EXPORTA?\(\s*\d+\s*,\s*[^,]+,\s*([A-Za-z_][A-Za-z0-9_]*)\s*,', re.MULTILINE)
    names = sorted(set(pattern.findall(text)))
    if not names:
        print("ERROR: no PNG_EXPORT symbols found in " + png_h_path, file=sys.stderr)
        return 1
    with open(output_path, "w") as f:
        f.write("#ifndef PNGPREFIX_H\n#define PNGPREFIX_H\n\n")
        for name in names:
            f.write("#define {0} {1}{0}\n".format(name, prefix))
        f.write("\n#endif /* PNGPREFIX_H */\n")
    print("Generated {0} with {1} renamed symbols".format(output_path, len(names)))
    return 0


if __name__ == "__main__":
    sys.exit(main())
