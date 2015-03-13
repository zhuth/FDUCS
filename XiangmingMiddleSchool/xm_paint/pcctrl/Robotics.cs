/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012/3/11
 * Time: 13:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.IO.Ports;
using Fischer;

namespace pcctrl
{
	public enum Directions {
		STOP = 0, CW, CCW
	}
	
	/// <summary>
	/// Description of Robotics.
	/// </summary>
	public class Robotics
	{
		private byte[] penXdir = { 0x10, 0x20, 0x40, 0x80 }; // 笔，高四位，左右动
		private byte[] penYdir = { 0x01, 0x02, 0x04, 0x08 }; // 笔，低四位，前后动
		private int penXState = 0, penYState = 0, interval = 100;
		
		private SerialPort sp = null;
		
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="COM_Fischer">慧鱼控制板COM口</param>
		/// <param name="COM_Motors">单片机COM口</param>
		/// <param name="RotateInterval">两次驱动步进电机之间的间隔</param>
		public Robotics(string COM_Fischer, string COM_Motors, int RotateInterval)
		{
			interval = RotateInterval;
			try {
				sp = new SerialPort(COM_Motors, 2400);
				sp.DataBits = 8;
				sp.Parity = Parity.None;
				sp.StopBits = StopBits.One;
				if (!sp.IsOpen) sp.Open();
			} catch(Exception) {
				throw new Exception("单片机串口连接错误");
			}
			
			Demo.Init(COM_Fischer);
		}
		
		public void Close() {
			sp.Close();
			Demo.Terminate();
		}
		
		/// <summary>
		/// 旋转画笔，两个自由度同速度转动
		/// </summary>
		/// <param name="Xdegrees"></param>
		/// <param name="Ydegrees"></param>
		public void PenTurn(int Xdegrees, int Ydegrees) {
			int xinc = Xdegrees > 0 ? 1 : -1, yinc = Ydegrees > 0 ? 1 : -1;
			Xdegrees = Math.Abs(Xdegrees); Ydegrees = Math.Abs(Ydegrees);
			int xtrs = Xdegrees > 0 ? (Xdegrees * 16 / 90) : -1, ytrs = Ydegrees > 0 ? (Ydegrees * 16 / 90) : -1, mtrs = Math.Max(Math.Min(xtrs, ytrs), 0);
			
			for(int i = 0; i < mtrs; ++i) {
				sendToPen();
				penXState = (penXState + xinc + 4) % 4;
				penYState = (penYState + yinc + 4) % 4;
			}
					
			for(int i = mtrs; i <= xtrs; ++i) {
				sendToPen();
				penXState = (penXState + xinc + 4) % 4;
			}
			
			for(int i = mtrs; i <= ytrs; ++i) {
				sendToPen();
				penYState = (penYState + yinc + 4) % 4;
			}
		}
		
		private void sendToPen() {
			try {
				sp.Write(new byte[]{ (byte)( penXdir[penXState] | penYdir[penYState]  ) }, 0, 1);
				Thread.Sleep(interval);
			} catch (Exception) {}
		}
		
	}
}
