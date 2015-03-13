/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012/3/11
 * Time: 11:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using Fischer;

namespace pcctrl
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public static Robotics robot = null;
		private int dx = 5, dy = 1000;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			button2.Enabled = false;
			numericUpDown1.Value = dx; numericUpDown2.Value = dy;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if (robot != null) {
				try {
					robot.Close();
				} catch (Exception) {}
			}
			robot = new Robotics(comboBox1.Text, comboBox2.Text, 0);
			button2.Enabled = true;
			button1.Enabled = false;
			MessageBox.Show("连接成功！");
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			if (robot != null) {
				try {
					robot.Close();
				} catch (Exception) {}
			}
			button1.Enabled = true; button2.Enabled = false;
		}
		
		void MainFormKeyPress(object sender, KeyPressEventArgs e)
		{
			
		}
		
		void dealKeyDown(object k) {
			switch((Keys)k) {
				case Keys.A:
					robot.PenTurn(dx, 0);
					break;
				case Keys.D:
					robot.PenTurn(-dx, 0);
					break;
				case Keys.S:
					robot.PenTurn(0, dy);
					break;
				case Keys.W:
					robot.PenTurn(0, -dy);
					break;
				case Keys.U:
					Demo.MotorGo(MotorIdx.MOTOR_2, 512, 100);
					break;
				case Keys.I:
					Demo.MotorGo(MotorIdx.MOTOR_2, 512, -100);
					break;
				case Keys.J:
					Demo.MotorGo(MotorIdx.MOTOR_4, 512, 100);
					break;
				case Keys.K:
					Demo.MotorGo(MotorIdx.MOTOR_4, 200, -100);
					break;
				case Keys.Left:
					Demo.MotorGo(MotorIdx.MOTOR_3, 350, 100);
					break;
				case Keys.Right:
					Demo.MotorGo(MotorIdx.MOTOR_3, 350, -100);
					break;
				case Keys.Up:
					Demo.MotorGo(MotorIdx.MOTOR_1, 400, 100);
					break;
				case Keys.Down:
					Demo.MotorGo(MotorIdx.MOTOR_1, 400, -100);
					break;
			}
		}
		
		void MainFormKeyDown(object sender, KeyEventArgs e)
		{	
			Thread nkey = new Thread(new ParameterizedThreadStart(dealKeyDown));
			nkey.Start(e.KeyCode);
		}
		
		void TextBox1Enter(object sender, EventArgs e)
		{
			this.Focus();
		}
		
		void Label5Click(object sender, EventArgs e)
		{
			this.Focus();
		}
		
		void NumericUpDown1ValueChanged(object sender, EventArgs e)
		{
			dx = (int)numericUpDown1.Value;
		}
		
		void NumericUpDown2ValueChanged(object sender, EventArgs e)
		{
			dy = (int)numericUpDown2.Value;
		}
	}
}
