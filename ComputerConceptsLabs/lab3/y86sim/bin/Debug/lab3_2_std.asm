irmovl $128, %edx
irmovl $3, %ecx
rmmovl %ecx, 0(%edx)
irmovl $10, %ebx
mrmovl 0(%ebx), %eax
addl %ebx, %eax
halt
