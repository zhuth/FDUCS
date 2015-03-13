library IEEE; 
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.STD_LOGIC_ARITH.ALL;
use IEEE.STD_LOGIC_UNSIGNED.ALL;

entity CPU16 is
 Port (
		CI : out std_logic_vector(31 downto 0);
		DB : inout std_logic_vector(15 downto 0);
		AB : buffer std_logic_vector(15 downto 0); 
		MUX : in std_logic_vector(0 to 2); 
		MCLK : buffer std_logic;	
		MWR, MRD, IOW, IOR : out std_logic;
		PRIX, KRIX, RUN, RESET, CLK : in std_logic);	
end CPU16;

architecture Behavioral of CPU16 is

	-- instruction list
	signal NOP: std_logic_vector(4 downto 0) := "00000";
	signal MOVL: std_logic_vector(4 downto 0) := "00001";
	signal MOVH: std_logic_vector(4 downto 0) := "00010";
	signal LD: std_logic_vector(4 downto 0) := "00011";
	signal ST: std_logic_vector(4 downto 0) := "00100";
	signal SLLI: std_logic_vector(4 downto 0) := "00101";
	signal MOV: std_logic_vector(4 downto 0) := "00110";
	signal OP: std_logic_vector(4 downto 0) := "01000";
	signal ADDI: std_logic_vector(4 downto 0) := "01001";
	signal SUBI: std_logic_vector(4 downto 0) := "01010";
	signal ORI: std_logic_vector(4 downto 0) := "01011";
	signal BEQZ: std_logic_vector(4 downto 0) := "01100";
	signal BNEQ: std_logic_vector(4 downto 0) := "01101";
	signal J: std_logic_vector(4 downto 0) := "01110";
	signal JAL: std_logic_vector(4 downto 0) := "01111";
	signal MOVPC: std_logic_vector(4 downto 0) := "10000";

	-- registers
	signal R0, R1, R2, R3, R4, R5, R6, R7: std_logic_vector(15 downto 0);
	
	-- i/o control signals
	signal CWR, CRD, MEW, MER: std_logic;

	signal PC: std_logic_vector(15 downto 0);			  -- program counter
	
	-- inter-stage registers	and stage signals
	signal IF_I, ID_I, EX_I, MEM_I: std_logic_vector(4 downto 0); -- instruction code for each stage
	signal IF_NPC, IF_IR: std_logic_vector(15 downto 0);
	signal ID_A, ID_B, ID_Imm, ID_IR: std_logic_vector(15 downto 0);
	signal ID_COND: std_logic;
	signal ID_D, EX_D: std_logic_vector(2 downto 0);		  -- reg destination for ID and EX
	signal EX_ALU, EX_B, EX_IR: std_logic_vector(15 downto 0);
	signal WB_REGW: std_logic; -- ID_COND: jump; WB_REGW: register write enable, '0' valid
	signal MEM_LMD, MEM_ALU, MEM_IR: std_logic_vector(15 downto 0);
	
	-- signals for registers
	signal ROUTN, ROUTM, Imm: std_logic_vector(15 downto 0);	-- ALU
	signal ADD_PC: std_logic_vector(15 downto 0); -- ADD_PC output (for MUX_PC)  
	signal ALU: std_logic_vector(15 downto 0); -- ALU output
	
	signal MUX_COND, MUX_PC, MUX_R: std_logic_vector(15 downto 0); -- muxes for ID_COND, PC and Registers
	signal LMD: std_logic_vector(15 downto 0); -- memory data loaded
	signal ALU_INA, ALU_INB: std_logic_vector(15 downto 0); -- ALU inputs
	signal RR: std_logic_vector(15 downto 0); -- data to write back
	signal RRW: std_logic_vector(2 downto 0); -- write back reg
	signal D, N, M: std_logic_vector(2 downto 0); -- subscript for regs in OP instructions 
	signal BCOND, LD_STALL: std_logic;
	signal COMP: std_logic_vector(15 downto 0); -- ROUTN - ROUTM

begin

	pMCLK: process(MCLK, CLK) begin
		if ((RUN = '0') or (RESET = '0')) then
			MCLK <= '0';
		elsif (CLK'event and CLK = '0') then
			MCLK <= not MCLK;
		end if;	
	end process pMCLK;

	-- fetch stage
	pIF: process(RESET, MCLK) begin
		if (RESET = '0') then
			PC <= "0000000000000000";
			IF_IR <= "0000000000000000";
			IF_I <= "00000";
			IF_NPC <= "0000000000000000";
		elsif (MCLK'event and MCLK = '0') then
			if (LD_STALL = '0') then
				if (ID_COND = '1') then -- jump, insert bubble
					PC <= MUX_PC;
					IF_IR <= "0000000000000000";
					IF_I <= "00000";
				elsif (EX_I = ST or EX_I = LD) then -- visiting memory, insert bubble
					IF_IR <= "0000000000000000";
					IF_I <= "00000";
				else
					PC <= MUX_PC;
					IF_NPC <= MUX_PC;
					IF_IR <= DB;
					IF_I <= DB(15 downto 11);
				end if;
			end if;
		end if;
	end process pIF;
	
	Imm <= (IF_IR(4)&IF_IR(4)&IF_IR(4)&IF_IR(4)&IF_IR(4)&IF_IR(4)&IF_IR(4)&IF_IR(4)&IF_IR(4)&IF_IR(4)&IF_IR(4)) & IF_IR(4 downto 0) when IF_I = LD or IF_I = ST or IF_I = SLLI else
		 (IF_IR(7)&IF_IR(7)&IF_IR(7)&IF_IR(7)&IF_IR(7)&IF_IR(7)&IF_IR(7)&IF_IR(7)) & IF_IR(7 downto 0) when IF_I = ADDI or IF_I = SUBI or IF_I = ORI or IF_I = MOVH or IF_I = MOVL or IF_I = BEQZ or IF_I = BNEQ else
		 (IF_IR(10)&IF_IR(10)&IF_IR(10)&IF_IR(10)&IF_IR(10)) & IF_IR(10 downto 0) when IF_I = J or IF_I = JAL else
		 "0000000000000000"; 
	ID_COND <= '1' when IF_I = J or IF_I = JAL or (IF_I = BEQZ and MUX_COND = "0000000000000000") or (IF_I = BNEQ and MUX_COND /= "0000000000000000") or IF_I = MOVPC else
		  '0';

	MUX_PC <= ADD_PC when ID_COND = '1' else PC + 1;
	ADD_PC <= IF_NPC + Imm;	

	D <= IF_IR(10 downto 8) when IF_I = MOVH or IF_I = MOVL or IF_I = ADDI or IF_I = SUBI or IF_I = ORI else
		IF_IR(4 downto 2) when IF_I = OP else
		"111" when IF_I = JAL else
		IF_IR(7 downto 5);  -- D as is in R[D]
	N <= IF_IR(10 downto 8); -- N as is in R[N]
	M <= IF_IR(7 downto 5); -- M as is in R[M]

	-- decode stage
	pID: process(RESET, MCLK)  
	begin
		if (RESET = '0') then
			ID_IR <= "0000000000000000";
			ID_I <= "00000";
			ID_A <= "0000000000000000";
			ID_B <= "0000000000000000";
			ID_Imm <= "0000000000000000";
			ID_D <= "000";	
		elsif (MCLK'event and MCLK = '0') then -- forwarding
			if (N = ID_D and (ID_I = LD or ID_I = SLLI or ID_I = ADDI or ID_I = SUBI or ID_I = ORI or ID_I = MOVH or ID_I = MOVL or ID_I = OP or ID_I = MOV or ID_I = JAL)) then
				ID_A <= ALU;
			elsif (N = EX_D and (EX_I = SLLI or EX_I = ADDI or EX_I = SUBI or EX_I = ORI or EX_I = MOVH or EX_I = MOVL or EX_I = OP or EX_I = MOV or EX_I = JAL)) then
				ID_A <= EX_ALU;
			elsif (N = EX_D and EX_I = LD)  then
				ID_A <= LMD;
			else
				ID_A <= ROUTN;
			end if;

			if (M = ID_D and (ID_I = LD or ID_I = SLLI or ID_I = ADDI or ID_I = SUBI or ID_I = ORI or ID_I = MOVH or ID_I = MOVL or ID_I = OP or ID_I = MOV or ID_I = JAL)) then
				ID_B <= ALU;
			elsif (M = EX_D and (EX_I = SLLI or EX_I = ADDI or EX_I = SUBI or EX_I = ORI or EX_I = MOVH or EX_I = MOVL or EX_I = OP or EX_I = MOV or EX_I = JAL)) then
				ID_B <= EX_ALU;
			elsif (M = EX_D and EX_I = LD)  then
				ID_B <= LMD;
			else
				ID_B <= ROUTM;
			end if;

			if (LD_STALL = '1') then -- insert bubble
			 	ID_IR <= "0000000000000000";
				ID_I <= "00000";
			else
				ID_IR <= IF_IR;
				ID_Imm <= Imm;		 	
				ID_I <= IF_I;
				ID_D <= D;
			end if;
		end if;
	end process pID;

	---- decode reg references
	ROUTN <= 
	  R0 when (IF_IR(10 downto 8) = "000") else
		R1 when (IF_IR(10 downto 8) = "001") else
		R2 when (IF_IR(10 downto 8) = "010") else
		R3 when (IF_IR(10 downto 8) = "011") else
		R4 when (IF_IR(10 downto 8) = "100") else
		R5 when (IF_IR(10 downto 8) = "101") else
		R6 when (IF_IR(10 downto 8) = "110") else
		R7;								  -- R[n] in OP instructions

	ROUTM <= 
	  R0 when(IF_IR(7 downto 5) = "000") else
		R1 when(IF_IR(7 downto 5) = "001") else
		R2 when(IF_IR(7 downto 5) = "010") else
		R3 when(IF_IR(7 downto 5) = "011") else
		R4 when(IF_IR(7 downto 5) = "100") else
		R5 when(IF_IR(7 downto 5) = "101") else
		R6 when(IF_IR(7 downto 5) = "110") else
		R7;								  -- R[m] in OP instructions
		
	---- decide whether there is a jump
	BCOND <= '1' when N = ID_D and(ID_I = LD or ID_I = SLLI or ID_I = ADDI or ID_I = SUBI or ID_I = ORI or ID_I = MOVH or ID_I = MOVL or ID_I = OP or ID_I = MOV or ID_I = JAL) else
		 '0';	 

	MUX_COND <= ALU when BCOND = '1' else
			 EX_ALU when BCOND = '0' and N = EX_D and (EX_I = SLLI or EX_I = ADDI or EX_I = SUBI or EX_I = ORI or EX_I = MOVH or EX_I = MOVL or EX_I = OP or EX_I = MOV or EX_I = JAL) else
			 LMD when BCOND = '0' and N = EX_D and EX_I = LD else
			 ROUTN;

	---- decide stall?
	LD_STALL <= '1' when ID_I = LD and((ID_D = N and (IF_I = LD or IF_I = ST or IF_I = SLLI or IF_I = MOV or IF_I = MOVPC or IF_I = OP or IF_I = BEQZ or IF_I = BNEQ)) or (ID_D = M and (IF_I = ST or IF_I = OP))) else
		  	'0';

	-- execute stage
	pEX: process(RESET, MCLK)
	begin
		if (RESET = '0') then
			EX_IR <= "0000000000000000";
			EX_I <= "00000";
			EX_ALU <= "0000000000000000";	
			EX_B <= "0000000000000000";	
			EX_D <= "000";		
		elsif (MCLK'event and MCLK = '0') then
			EX_IR <= ID_IR;
			EX_I <= ID_I;
			EX_ALU <= ALU;
			EX_B <= ID_B;
			EX_D <= ID_D; 
		end if;
	end process pEX;

	---- implements main ALU
	ALU_INA <= ID_A;
	ALU_INB <= ID_B when ID_I = OP else -- choose from ID_B and ID_Imm, depends on OP or other kinds of instructions
			 ID_Imm;
	COMP <= ALU_INA - ALU_INB; -- for SGE use
	ALU <= "0000000000000001" when (ID_I = OP and ID_IR(1 downto 0) = "00" and COMP(15) = '0') else -- SGE
		 "0000000000000000" when (ID_I = OP and ID_IR(1 downto 0) = "00" and COMP(15) = '1') else	-- SGE
		 ALU_INA - ALU_INB when (ID_I = OP and ID_IR(1 downto 0) = "10") or ID_I = SUBI else
		 ALU_INA and ALU_INB when (ID_I = OP and ID_IR(1 downto 0) = "11") else
		 ALU_INA or ALU_INB when ID_I = ORI else
		 to_stdlogicvector(to_bitvector(ALU_INA) sll conv_integer(ALU_INB)) when ID_I = SLLI else -- SLLI
		 ALU_INA when ID_I = MOVPC else -- MOVPC
		 ALU_INB when ID_I = MOVH or ID_I = MOVL else -- MOVH, MOVL
		 ALU_INA + ALU_INB;

	-- memory stage
	pMEM: process(RESET, MCLK)  
	begin
		if (RESET = '0') then
			MEM_IR <= "0000000000000000";
			MEM_I <= "00000";
			MEM_LMD <= "0000000000000000";
			MEM_ALU <= "0000000000000000";
		elsif (MCLK'event and MCLK = '0') then
			MEM_IR <= EX_IR;
			MEM_I <= EX_I;		
			MEM_ALU <= EX_ALU;
			MEM_LMD <= LMD;
		end if;
	end process pMEM;

	-- write back stage
	pWB: process(RESET, MCLK)  
	begin
		if (RESET = '0') then
			R0 <= "0000000000000000";
			R1 <= "0000000000000000";
			R2 <= "0000000000000000";
			R3 <= "0000000000000000";
			R4 <= "0000000000000000";
			R5 <= "0000000000000000";
			R6 <= "0000000000000000";
			R7 <= "0000000000000000";	
		elsif ((MCLK'event and MCLK = '1') and WB_REGW = '0') then
			case RRW is
				when "000" => R0 <= RR;
				when "001" => R1 <= RR;
				when "010" => R2 <= RR;
				when "011" => R3 <= RR;
				when "100" => R4 <= RR;
				when "101" => R5 <= RR;
				when "110" => R6 <= RR;
				when "111" => R7 <= RR;
				when others => null;
			end case;	
		end if;
	end process pWB;
	
	--end of stages

	-- i/o controls
	MER <= '0' when EX_I = LD else '1';	---- memory read, '0' valid
	MEW <= '0' when EX_I = ST else '1';	---- memory write, '0' valid

	-- same as in cpu8
	CRD <= MER or not MCLK;
	CWR <= MEW or not MCLK;
	
	MRD <= '0' when (CRD = '0' or CWR = '1') and AB(15) = '0' and CLK = '1' else '1';
	MWR <= '0' when CWR = '0' and AB(15) = '0' else '1';						

	IOR <= not AB(15) or not AB(0) or AB(1) or CRD or not CLK;		
	IOW <= not AB(15) or not AB(1) or AB(0) or CWR or not CLK;	

	DB <= EX_B when EX_I = ST else "ZZZZZZZZZZZZZZZZ"; -- triple state
	AB <= EX_ALU when MER = '0' or MEW = '0' else PC;  -- address bus, for writting or for reading or for instruction

	-- determines which data source is adpoted
	MUX_R <= MEM_LMD when MEM_I = LD else
		 MEM_ALU;

	-- data to be written to register
	RR <= MUX_R(7 downto 0) & "00000000" when MEM_I = MOVH else
		 "00000000" & MUX_R(7 downto 0) when MEM_I = MOVL else
		 MUX_R;

	-- get destination register id
	RRW <= MEM_IR(10 downto 8) when MEM_I = MOVH or MEM_I = MOVL or MEM_I = ADDI or MEM_I = SUBI or MEM_I = ORI else
		 MEM_IR(4 downto 2) when MEM_I = OP else
		 "111" when MEM_I = JAL else
		 MEM_IR(7 downto 5);
		 
	-- determines whether needs to write RR to register[RRW]
	WB_REGW <= '1' when MEM_I = ST or MEM_I = MOVPC or MEM_I = J or MEM_I = BEQZ or MEM_I = BNEQ or MEM_I = NOP else
		  '0';
			
	LMD <= "000000000000000" & PRIX when EX_ALU(15) = '1' and EX_ALU(2) = '1' else
		 "000000000000000" & KRIX when EX_ALU(15) = '1' and EX_ALU(1) = '1' and EX_ALU(0) = '1' else
		 DB; -- loads PRIX, KRIX, or MEM data

	-- debug info
	CI(31 downto 16) <= PC when MUX = "000" else
					R1 when MUX = "001" else
					IF_NPC when MUX = "010" else
					IF_IR when MUX = "011" else
					ID_A when MUX = "100" else
					ID_Imm when MUX = "101" else
					EX_B when MUX = "110" else
					MEM_LMD;
					
	CI(15 downto 8) <= R0(7 downto 0) when MUX = "000" else
				 R2(15 downto 8) when MUX = "001" else
				 R2(7 downto 0) when MUX = "010" else
				 R3(7 downto 0) when MUX = "011" else
				 R4(7 downto 0) when MUX = "100" else
				 R5(7 downto 0) when MUX = "101" else
				 R6(7 downto 0) when MUX = "110" else
				 R7(7 downto 0);
				 
	CI(7 downto 3) <= IF_I when MUX = "000" else
				  ID_I when MUX = "001" else
				  EX_I when MUX = "010" else
				  MEM_I;
					
	CI(2) <= KRIX when MUX = "000" else
		 PRIX when MUX = "001" else
		 CRD when MUX = "010" else 
		 CWR when MUX = "011" else
		 MER when MUX = "100" else
		 MEW when MUX = "101" else
		 ID_COND when MUX = "110" else
		 WB_REGW;
	
	CI(1) <= LD_STALL;

end Behavioral;
