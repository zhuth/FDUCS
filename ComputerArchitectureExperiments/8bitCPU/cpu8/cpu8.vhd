library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.STD_LOGIC_ARITH.ALL;
use IEEE.STD_LOGIC_UNSIGNED.ALL;

entity cpu8 is
    port (
		CO : in std_logic_vector(31 downto 0);
		CI : out std_logic_vector(31 downto 0);
		DB : inout std_logic_vector(7 downto 0);
		AB : buffer std_logic_vector(15 downto 0); 
		MUX : in std_logic_vector(0 to 2); 
		MCLK : buffer std_logic;	
		MWR, MRD, IOW, IOR : out std_logic;
		PRIX, KRIX, RUN, RESET, CLK : in std_logic);	
end cpu8;

architecture Behavioral of cpu8 is

-- micro-command controlled signals
-- #SIGS
signal GT, GC, WRE, MXE, MXB1, MXB0, MXC1, MXC0, S2, S1, S0, CP, GI, GA1, AHS, GA2, PINC, PLD2, PLD1, PLD0, MPLD, CRDX, CWRX, SSP1, SSP0, OB : std_logic;
-- #/SIGS

signal CRD, CWR, CC, CT, WRC, PCK, CCK, CCI, CA1, CA2, SCK, PRST, MCLR, MPCK, MICK, COUT, CY, LE, AREL, PLD : std_logic;
signal RS1, RS2, RS : std_logic_vector(1 downto 0);
signal IR, ACT, TMP, ROUT, ADRL, ADRH, FF : std_logic_vector(7 downto 0);
signal FTMP : std_logic_vector(8 downto 0);
signal R0, R1, R2, R3 : std_logic_vector(7 downto 0);
signal MIR : std_logic_vector(31 downto 0);
signal PC, SP : std_logic_vector(15 downto 0);
signal MXD, S : std_logic_vector(2 downto 0);
signal MD, MPC : std_logic_vector(9 downto 0);

begin

-- init R/W, I/O signals
CRD <= CRDX or not MCLK;
CWR <= CWRX or not MCLK;

MRD <= AB(15) or CRD;
MWR <= AB(15) or CWR or (not CLK);
IOW <= (not AB(15)) or (not AB(1)) or CWR or (not CLK);
IOR <= (not AB(15)) or (not AB(0)) or CRD or (not CLK);	   

WRC <= MCLK;	--Ai
PCK <= MCLK;	--PC
CC <= MCLK;	--ACT
CT <= MCLK;	--TMP
CCI <= MCLK;	--IR
CA1 <= MCLK;	--ADRH
CA2 <= MCLK;	--ADRL
SCK <= MCLK;	--SP
CCK <= MCLK;	--CY

PRST <= RESET;

-- for register selection
-- RS1(2)<=IR(2);
-- ii
RS1(1)<=IR(1);
RS1(0)<=IR(0);

-- RS2(2)<=IR(5);
-- jj
RS2(1)<=IR(3);
RS2(0)<=IR(2);

RS <=	RS1 when MXE='0' else
		RS2 when MXE='1' else
		"ZZ";

-- parse micro-commands
-- #MIRS
GT<=MIR(0);
GC<=MIR(1);
WRE<=MIR(2);
MXE<=MIR(3);
MXB1<=MIR(4);
MXB0<=MIR(5);
MXC1<=MIR(6);
MXC0<=MIR(7);
S2<=MIR(8);
S1<=MIR(9);
S0<=MIR(10);
CP<=MIR(11);
GI<=MIR(12);
GA1<=MIR(13);
AHS<=MIR(14);
GA2<=MIR(15);
PINC<=MIR(16);
PLD2<=MIR(17);
PLD1<=MIR(18);
PLD0<=MIR(19);
MPLD<=MIR(20);
CRDX<=MIR(21);
CWRX<=MIR(22);
SSP1<=MIR(23);
SSP0<=MIR(24);
OB<=MIR(25);
-- #/MIRS

-- clocks
pMCLR: process (MCLK,RUN,RESET) 
begin
	if(RESET='0') then MCLR<='0';
	elsif(MCLK'event and MCLK='1') then MCLR<=RUN;
	end if;
end process;

pMCLK: process (MCLK,CLK,RUN,RESET) 
begin
	if(RUN='0') or (RESET='0') then MCLK<='0';
	elsif(CLK'event and CLK='0') then MCLK<=not MCLK;
	end if;
end process;

MPCK <= not MCLK and CLK;
MICK <= not MPCK;

-- register TMP
regT: process (CT,GT,DB)
begin
	if(CT'event and CT='1') then 
		if(GT='0') then TMP <= DB; end if;
	end if;
end process;

-- register ACT
regC: process (CC,GC,DB)
begin
	if(CC'event and CC='1') then 
		if(GC='0') then ACT <= DB; end if;
	end if;
end process;

-- alu
S <= S2 & S1 & S0;
FTMP <= ("0"&ACT) + ("0"&TMP) when S = "000" else
	 ("0"&ACT) - ("0"&TMP) when S = "001" else
	 ("0"&ACT) when S = "010" else
	 ("0"&TMP) when S = "011" else
	 ACT(0) & CY & ACT(7 downto 1) when S = "100" else
	 ("0"&ACT) and ("0"&TMP) when S = "101" else
	 not ("1"&ACT) when S = "110" else
	 "000000000";
FF <= FTMP(7 downto 0);
COUT <= FTMP(8);

-- registers heap
process(WRC,WRE,RS,DB)
begin
	if(WRC'event and WRC='0') then 
		if WRE='0' then
			case RS is
				when "00" => R0<=DB;
				when "01" => R1<=DB;
				when "10" => R2<=DB;
				when others => R3<=DB;
			end case;
		end if;
	end if;
end process ;

ROUT <=	R0 when RS="00" else
		R1 when RS="01"  else
		R2 when RS="10" else
		R3;
		
-- jxx
process (CCK,CP,MXE,COUT,DB)
begin
	if(CCK'event and CCK='0') then
		if(CP='0') then 
			if(MXE='0') then CY <= COUT;
			else CY <= DB(0);
			end if;
			if(FF="00000000") then LE<='1'; 
			else LE<= FF(7); 
			end if;
		end if;
	end if;
end process;

-- addressing
AB <=	PC when MXC1='0' and MXC0='0' else
		(ADRH & ADRL) when MXC1='0' and MXC0='1' else
		SP when MXC1='1' and MXC0='0' else 
		"0000000000000000";

-- program counter
MXD <= PLD2 & PLD1 & PLD0;
PLD <=	'0'when MXD="000" else
		CY when MXD="001" else     -- rel8
		not CY when MXD="010" else -- rel8
		not KRIX when MXD="011" else
		not PRIX when MXD="100" else
		'1' when MXD="101" else
		LE when MXD="110" else     -- rel8
		'0';
AREL <=	'1' when MXD="001" else    -- rel8
		'1' when MXD="010" else	  -- rel8
		'1' when MXD="110" else    -- rel8
		'0';
			
process (PC,AB,PCK,PRST,PLD,PINC)
begin 
	if(PRST='0') then PC<="0000000000000000";
	elsif(PCK'event and PCK='0') then
		if(PLD='1' and AREL='1') then 
			PC<=AB+(ADRL(7)&ADRL(7)&ADRL(7)&ADRL(7)&ADRL(7)&ADRL(7)&ADRL(7)&ADRL(7)&ADRL);
		elsif(PLD='1' and AREL='0') then
			PC<=AB;
		elsif(PINC='0') then PC<=PC+1;
		end if;
	end if;
end process;

-- databus
process (CCI,GI,DB)
begin
	if(CCI'event and CCI='0') then
		if(GI='0') then IR<=DB; end if;
	end if;
end process;

process(MXB1,MXB0,PC,FF,OB,ROUT)
begin
	if OB='0' then
		if MXB1='0' and MXB0='0' then DB<=FF;
		elsif MXB1='0' and MXB0='1' then DB<=ROUT;
		elsif MXB1='1' and MXB0='0' then DB<=PC(15 downto 8);
		elsif MXB1='1' and MXB0='1' then DB<=PC(7 downto 0);
		end if;
	else DB<="ZZZZZZZZ";
	end if;
end process;

process (DB,CA1,GA1,AHS)
begin
	if(AHS='0') then ADRH<="01111110";
	elsif(CA1'event and CA1='0') then
		if(GA1='0') then ADRH<=DB; end if;
	end if;
end process;

process (DB,CA2,GA2)
begin
	if(CA2'event and CA2='0') then
		if(GA2='0') then ADRL<=DB; end if;
	end if;
end process;

-- stack
process (SP,SCK,SSP0,SSP1,RESET)
begin
	if((SSP1='1' and SSP0='1') or RESET='0') then SP<="0111111111111111";
	elsif(SCK'event and SCK='0') then
		if(SSP1='0' and SSP0='1') then SP <= SP-1;
		elsif(SSP0='0' and SSP1='1') then SP <= SP+1;
		end if;
	end if;
end process;
		
-- micro-commands
process (MPCK,MCLR,MPLD)
begin
	if(MCLR='0') then MPC<="0000000000";
	elsif(MPCK'event and MPCK='1') then
		if(MPLD='0') then MPC<=MD;
		else MPC<=MPC+1;
		end if;
	end if;
end process;

CI(9 downto 0)<=MPC;

process(MICK) 
begin
	if(MICK'event and MICK='1') then 
		MIR<=CO;
	end if;
end process;

MD(2 downto 0) <= "111";
MD(7 downto 3) <= "0"&IR(7 downto 4) when IR(7 downto 6)="00" else
			   IR(7 downto 3);
MD(9 downto 8) <= "00";
	
-- debug info
CI(31 downto 24) <=	ACT when MUX="000" else
				PC(15 downto 8) when MUX="001" else
				ADRH when MUX="010" else
				R0 when MUX="011" else
				R2 when MUX="100" else
				SP(15 downto 8) when MUX="101" else
				-- R6 when MUX="110" else
				TMP;

CI(23 downto 16) <=	IR when MUX="000" else
				PC(7 downto 0) when MUX="001" else
				ADRL when MUX="010" else
				R1 when MUX="011" else
				R3 when MUX="100" else
				SP(7 downto 0) when MUX="101" else
				-- R7 when MUX="110" else
				ACT;

CI(15 downto 10) <= KRIX & PRIX & LE & CY & MCLR & RESET;

end Behavioral;

