pushl %ebp
rrmovl %esp, %ebp
mrmovl 8(%ebp), %ecx
mrmovl 12(%ebp), %edx
xorl %eax, %eax
andl %edx, %edx
je exit
la: mrmovl 0(%ecx), %esi
addl %esi, %eax
irmovl $4, %ebx
addl %ebx, %ecx
irmovl $-1, %ebx
addl %ebx, %edx
jne la
exit:
rrmovl %ebp, %esp
popl %ebp
ret
