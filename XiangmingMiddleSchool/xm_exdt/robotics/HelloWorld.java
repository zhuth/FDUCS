import lejos.nxt.*;
import lejos.nxt.comm.*;
import java.io.*;

public class HelloWorld {
	private static class Robot   
	{		
		public static void Go() {		// 向前走
			LCD.drawString("Go forward           ",0, 0);
			int count = 0;
			Motor.B.resetTachoCount();
			Motor.B.forward(); Motor.C.forward();
			Motor.B.setSpeed(300); Motor.C.setSpeed(300);
			while (count < 720) count = Motor.B.getTachoCount();         
			Motor.B.stop(); Motor.C.stop();
			LCD.drawString("Stay still.         ",0, 0);
		}
		
		public static void StartRotateSensor() {		// 开始旋转传感器下面的马达
			LCD.drawString("Sensor R            ",0, 0);
			Motor.A.resetTachoCount();
			Motor.A.forward();
			Motor.A.setSpeed(100);
		}
		
		public static void StopRotateSensor() {		// 停止旋转传感器下面的马达
			LCD.drawString("Sensor stopped.      ",0, 0);
			Motor.A.stop();
			int count = 0;
			Motor.A.resetTachoCount();
			Motor.A.backward();
			Motor.A.setSpeed(300);
			while (count < 180) count = Motor.A.getTachoCount();         // -180 ?
			Motor.A.stop();
		}
		
		public static int getSensorRotate() {		// 获取旋转传感器下面的马达转过的度数
			return Motor.A.getTachoCount();
		}
		
	}

	static Boolean running = true;
	static UltrasonicSensor us = new UltrasonicSensor(SensorPort.S1);
	static NXTConnection connection = null;
	static DataOutputStream dataOut = null;
	
	private static void sendData(int data) {
		LCD.clear();
		LCD.drawInt(data,0, 0);
		try { dataOut.writeInt(data); } 
		catch (IOException e) { System.out.println("-write error " + e); }
	}
	
	public static void main (String[] args) {
	
		LCD.clear();
		LCD.refresh();
		LCD.drawString("Waiting for BT...",0, 0);

		connection = Bluetooth.waitForConnection();
		
		LCD.drawString("Init...          ",0, 0);
		
	    Button.ENTER.addButtonListener(new ButtonListener() {
			public void buttonPressed(Button b) {
				running = false;
			}
			
			public void buttonReleased(Button b) {
				running = false;
			}
		});
		
		dataOut = connection.openDataOutputStream();

		while(running)
		{
			LCD.drawString("Running...     ",0, 0);
			int count = 0;
			Robot.Go();
			sendData(-128);
			Robot.StartRotateSensor();
			while (count < 180 && running) {
				count = Robot.getSensorRotate();
				int dist = us.getDistance();
				sendData(dist);
			}
			Robot.StopRotateSensor();
			sendData(-127);
		}

	}
}

