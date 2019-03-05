# import sys
# inp = sys.stdin
# out = sys.stdout
inp = open('input.txt', 'r')
out = open('output.txt', 'w')

n = int(inp.readline())
a = list(map(int, inp.readline().split()))

for i in range(0, n):
    j = i

    while j > 0 and a[j] < a[j - 1]:
        a[j], a[j - 1] = a[j - 1], a[j]
        j -= 1

    out.write(str(j + 1) + ' ')

out.write('\n')
out.write(' '.join(str(p) for p in a) + '\n')