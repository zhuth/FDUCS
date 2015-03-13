start:
MOVH R7, #80H
MOVL R6, #0AH

; input a
LD1:
LD R5, [R7, #3]
BEQZ R5, LD1
LD R1, [R7, #1]

LD2:
LD R5, [R7, #3]
BEQZ R5, LD2
LD R2, [R7, #1]

; input b
LD3:
LD R5, [R7, #3]
BEQZ R5, LD3
LD R3, [R7, #1]

LD4:
LD R5, [R7, #3]
BEQZ R5, LD4
LD R4, [R7, #1]

; print a
STS1:
LD R5,[R7,#4]
BEQZ R5,STS1
ST R6, [R7, #2]

STS2:
LD R5,[R7,#4]
BEQZ R5,STS2
ST R6, [R7, #2]

ST1:
LD R5,[R7,#4]
BEQZ R5,ST1
ST R2, [R7, #2]

ADDI R1, #10H
ST2:
LD R5, [R7,#4]
BEQZ R5, ST2
ST R1, [R7, #2]
SUBI R1, #10H

; print "+"
STP1:
LD R5,[R7,#4]
BEQZ R5,STP1
ST R0,[R7,#2]

STP2:
LD R5,[R7,#4]
BEQZ R5,STP2
ST R6,[R7,#2]

; print b
ST3:
LD R5,[R7,#4]
BEQZ R5,ST3
ST R4, [R7, #2]

ADDI R3, #10H
ST4:
LD R5, [R7,#4]
BEQZ R5, ST4
ST R3, [R7, #2]
SUBI R3, #10H

; print "="
MOVL R6, #9
STE1:
LD R5,[R7,#4]
BEQZ R5,STE1
ST R6,[R7,#2]
MOVL R6, #10
STE2:
LD R5,[R7,#4]
BEQZ R5,STE2
ST R6,[R7,#2]

; calc a+b
; i.e., [R1 R2] + [R3 R4]
ADD R1, R3, R1
ADD R2, R4, R2
; [R1 R2]
SGE R5, R2, R6
BEQZ R5, NONADD1
ADDI R1, #1
SUBI R2, #10
NONADD1:
SGE R5, R1, R6
BEQZ R5, NONADD2
MOVL R3, #1
SUBI R1, #10
; [R3 R1 R2]
J PRINT3
NONADD2:
MOVL R3, #0

; print ans
PRINT3:
STL:
LD R5,[R7,#4]
BEQZ R5,STL
ST R2,[R7,#2]
STM:
LD R5,[R7,#4]
BEQZ R5,STM
ST R1,[R7,#2]
STH:
LD R5,[R7,#4]
BEQZ R5,STH
ADDI R3, #10H
ST R3,[R7,#2]

J START