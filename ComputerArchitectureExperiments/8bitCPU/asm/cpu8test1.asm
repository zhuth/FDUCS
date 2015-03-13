; ex 4.1
MOV A0, #5AH
ST A0, 1234H

; ex 4.2
LD A1, 1234H
ST A1, 1234H

; ex 4.3
MOV A0, #5AH

; ex 4.4
MOV A1, #87H
ST A0, 100H

; ex 4.5
MOV A0, #5AH
MOV A1, #12H
ADD A0, A1

; ex 4.6
MOV A0, #5AH
MOV A3, #12H
SUB A0, A3

; ex 4.7
MOV A0, #5AH
MOV A2, #5AH
L1: 
ADD A0, A2
JMP L1