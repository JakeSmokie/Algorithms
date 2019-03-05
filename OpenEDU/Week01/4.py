# import sys
# inp = sys.stdin
# out = sys.stdout
inp = open('input.txt', 'r')
out = open('output.txt', 'w')

n = int(inp.readline())
a = list(map(float, inp.readline().split()))
b = sorted(((i + 1, a[i]) for i in range(0, n)), key=lambda x: x[1])
out.write(f'{b[0][0]} {b[n // 2][0]} {b[-1][0]}\n')
