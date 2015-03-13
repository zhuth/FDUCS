MOV A0, #FFH
AND A1, #0			; A1 set to 0, CY set to 0
RRC A0 ; s1			; 1111 1111 => 0111 1111 = 7F, CY = 1
RRC A0			; 0111 1111 => 1011 1111 = BF, CY = 1
RRC A0			; 1011 1111 => 1101 1111 = DF, CY = 1
AND A0, #F0H ; s2		; A0 set to D0, CY = 0
MOV A1, #10H			; 
MOV @A1, A0			; write D0 to @7E10
CPL @A1 			; 1101 0000 => 0010 1111 = 2F on @7E10
MOV A1, @A1			; load @7E10 to A1 [2F]

MOV A2, #2			; A2 set to 2
ST A2, 200H
ADD A2, 200H			; A2 set to 4
ADD A2, 200H			; A2 set to 6

MOV A0, #1
MOV A1, #2
SUB A0, A1
JLE le				; yes, jump to le
MOV A0, #10H
le:
JC cc				; CY=1, jump to cc
MOV A0, #20H
cc:
JHS hs				; CY=1, go on
MOV A0, #30H			; A0 set to 30
hs:
MOV A0, #FFH			; A0 set to FF
JMP hs