irmovl $10, %eax
pushl %eax
irmovl data, %eax
pushl %eax
pushl %ebp
rrmovl %esp, %ebp
mrmovl 4(%ebp), %ecx
mrmovl 8(%ebp), %edx
xorl %eax, %eax
andl %edx, %edx
je le
loop:
mrmovl 0(%ecx), %esi
addl %esi, %eax
irmovl $4, %ebx
addl %ebx, %ecx
irmovl $-1, %ebx
addl %ebx, %edx
jne loop
le:
rrmovl %ebp, %esp
popl %ebp
halt

data:
.long 1
.long 2
.long 3
.long 4
.long 5
.long 6
.long 7
.long 8
.long 9
.long 10
