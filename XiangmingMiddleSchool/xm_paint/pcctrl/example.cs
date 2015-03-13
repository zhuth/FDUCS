//=============================================================================
// Disclaimer - Exclusion of Liability
//
// This software is distributed in the hope that it will be useful,but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE. It can be used an modified by anyone
// free of any license obligations or authoring rights.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Fischer {

    public enum MotorIdx { MOTOR_1 = 0, MOTOR_2, MOTOR_3, MOTOR_4 };
    public enum InpIdx { I1 = 0, I2, I3, I4, I5, I6, I7, I8 };
    public enum CntIdx { COUNT_1 = 0, COUNT_2, COUNT_3, COUNT_4 };

    public class Demo {

        const int  N_MOTOR   = 4;
        const bool MOTOR_ON  = true;
        const bool MOTOR_OFF = false;

        const int  DUTY_MIN  = 0;
        const int  DUTY_MAX  = 512;

        static uint ftHandle = 0;

        public static void Init(string ComPortName) {

        	//  get library version
            Console.WriteLine("\nftMscLib {0}" + DLL.ftLibVersionStr());
            Console.WriteLine("\nPlease, press the right or left button on ROBO TX to stop the demo...");

            //  library initialization
            uint errCode = DLL.InitLib();

            //  COM port name
            Console.WriteLine(String.Format("\n\nOpen ComPort '{0}' ...", ComPortName));

            //  open COM port
            ftHandle = DLL.OpenComDevice(ComPortName, 38400, ref errCode);

            if (errCode == (uint)FTERR.FTLIB_ERR_SUCCESS) {

                Console.WriteLine("Connected to ROBO TX Controller ...");

                //  init values in TransferArea to default
                InitTAValues();

                //  starting transferarea
                errCode = DLL.StartTransferArea(ftHandle);

                if (errCode == (uint)FTERR.FTLIB_ERR_SUCCESS) {
                    Console.WriteLine("TransferArea was started and runs...");
                    //  motor run and stop by distance < 15
                    // MotorStop();
                    // Thread.Sleep(50);
                }
                else {
                    //  error case
                    Console.WriteLine("Error: TransferArea was not started !");
                	throw new Exception("连接慧鱼控制板时错误。TransferArea");
                }

                DLL.SetMotorConfig(ftHandle, (int)TA_ID.TA_LOCAL, (int)MotorIdx.MOTOR_1, MOTOR_ON);
                DLL.SetMotorConfig(ftHandle, (int)TA_ID.TA_LOCAL, (int)MotorIdx.MOTOR_2, MOTOR_ON);
                DLL.SetMotorConfig(ftHandle, (int)TA_ID.TA_LOCAL, (int)MotorIdx.MOTOR_3, MOTOR_ON);
                DLL.SetMotorConfig(ftHandle, (int)TA_ID.TA_LOCAL, (int)MotorIdx.MOTOR_4, MOTOR_ON);
                
            }
            else {
                //  error case
                Console.WriteLine("Error: No interface available (Port '" + ComPortName + "')");
                throw new Exception("连接慧鱼控制板时错误。");
            }
        }
        
        public static void Terminate() {
            //  stop TransferArea
            DLL.StopTransferArea(ftHandle);
			
            //  close COM port
            Console.WriteLine("Closing ComPort...");
            uint errCode = DLL.CloseDevice(ftHandle);
                
            //  close library
            DLL.CloseLib();    
        }

        /*-----------------------------------------------------------------------------
         *  InitTAValues  
         *---------------------------------------------------------------------------*/
        private static void InitTAValues() {
            //  set all motors ON with duty= 0
            for (int mtrIdx = 0; mtrIdx < N_MOTOR; mtrIdx++) {
                DLL.SetMotorValues(ftHandle, (int)TA_ID.TA_LOCAL, mtrIdx, 0, 0, false);
                DLL.SetMotorConfig(ftHandle, (int)TA_ID.TA_LOCAL, mtrIdx, MOTOR_ON);
            }
        }

        /*-----------------------------------------------------------------------------
         *  MotorStop  
         *---------------------------------------------------------------------------*/
        public static void MotorGo(MotorIdx motor, int speed, int durationMs) {
        	if (durationMs < 0) {
        		DLL.SetMotorValues(ftHandle, (int)TA_ID.TA_LOCAL, (int)motor, 0, speed, false);
        		durationMs = -durationMs; 
        	} else {
        		DLL.SetMotorValues(ftHandle, (int)TA_ID.TA_LOCAL, (int)motor, speed, 0, false);
        	}
            //  waiting for settings to ROBO TX Controller
            Thread.Sleep(durationMs);
            DLL.SetMotorValues(ftHandle, (int)TA_ID.TA_LOCAL, (int)motor, 0, 0, false);
            Thread.Sleep(5);
        }
    }
}
