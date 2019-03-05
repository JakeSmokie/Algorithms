inp = open('input.txt', 'r')
out = open('output.txt', 'w')

a, b = map(int, inp.readline().split())
out.write(str(a + b))
