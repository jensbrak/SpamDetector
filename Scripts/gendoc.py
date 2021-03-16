#!/usr/bin/python3
from pathlib import Path

readme = Path('..', 'README.md')
outdir = Path('..', 'src', 'assets', 'dist', 'doc')
wanted = ['about', 'configuration', 'further reading']
divide = '# '
inchdr = False
incnum = False


def split():
    with open(readme, 'r') as mdin:
        writing = False
        mdout = None
        chapter = 0
        written = 0
        for line in mdin:
            if line.startswith(divide):
                chapter += 1
                writing = False
                if mdout is not None:
                    mdout.close()
                header = line[len(divide):-1].lower()
                if header in wanted:
                    writing = True
                    mdbase = header.replace(' ', '_')
                    if incnum:
                        mdname = '{0:02}_{1}.md'.format(chapter, mdbase)
                    else:
                        mdname = '{0}.md'.format(mdbase)
                    mdpath = Path(outdir, mdname)
                    mdout = open(mdpath, 'w+')
                    if inchdr:
                        mdout.write(line)
                    written += 1
            else:
                if writing:
                    mdout.write(line)
        if mdout is not None:
            mdout.close()
        print('Splitted \'{0}\' and wrote {1} of {2} chapters in \'{3}\' (headers={4}, numbers={5})'
              .format(readme, written, chapter, outdir, inchdr, incnum))


if __name__ == "__main__":
    split()
