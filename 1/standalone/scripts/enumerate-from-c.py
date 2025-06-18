import os, sys

def places(x):
    p = 0
    while x > 0:
        x //= 10
        p += 1
    return p

n = len(sys.argv)
if n > 2:
    print("Incorrect number of arguments.")
    exit()

path = r'{}'.format(sys.argv[1])

# Assumes a naming scheme of "nx", where n = pitch [c, d, e, f, g, a, b] and x = octave [-1, 0, 1, 2, 3, ...]
PITCH_ORDER = ["c", "d", "e", "f", "g", "a", "b"]

# Each file is split into (name, extension)
files = [os.path.splitext(file) for file in os.listdir(path) if os.path.isfile(os.path.join(path, file))]
files.sort(key=lambda f: 0.1*PITCH_ORDER.index(f[0][0]) + int(f[0][1:]))

n = len(files)
p = places(n)
for i,f in enumerate(files):
    j = i + 1
    p2 = places(j)
    prefix = (p-p2)*"0" + str(j)
    full = f[0] + f[1]
    os.rename(os.path.join(path, full), os.path.join(path, prefix + "_" + full))
