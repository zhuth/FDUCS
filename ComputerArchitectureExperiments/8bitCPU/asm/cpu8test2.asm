; MOV Ai, #data
MOV A0, #1
MOV A1, #2

; MOV @Ai, Aj
MOV @A1, A0

; MOV Ai, @Aj
MOV A2, @A1

; ADD Ai, Aj
ADD A0, A1

; SUB Ai, Aj
SUB A0, A1
JLE le
MOV A0, #10H
le:
JC cc
MOV A0, #20H
cc:
JHS hs
MOV A0, #30H
hs:

; ST Ai, addr
ST A0, 100H

; LD Ai, addr
LD A3, 100H

; ADD Ai, addr
ADD A2, 100H

; JNKB addr
; JNPB addr
; tested in cpu8printa.asm

; CALL addr / RET
CALL ADD1

PUSH A0
POP A1
RRC A1

MOV A0, #1
CPL @A0
MOV A0, @A0

; AND Ai, #data
MOV A3, #7
AND A3, #6

END:
ADD A0, #1
JMP END

ADD1:
ADD A0, #1
RET