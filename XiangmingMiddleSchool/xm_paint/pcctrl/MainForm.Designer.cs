/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012/3/11
 * Time: 11:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace pcctrl
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(245, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(79, 29);
			this.button1.TabIndex = 0;
			this.button1.Text = "连接";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(12, 34);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(105, 20);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.Text = "COM3";
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(123, 34);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(106, 20);
			this.comboBox2.TabIndex = 2;
			this.comboBox2.Text = "COM4";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "慧鱼控制板COM口号";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(121, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "画笔控制板COM口号";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(330, 12);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(79, 29);
			this.button2.TabIndex = 5;
			this.button2.Text = "断开";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(121, 72);
			this.numericUpDown1.Minimum = new decimal(new int[] {
									100,
									0,
									0,
									-2147483648});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(76, 21);
			this.numericUpDown1.TabIndex = 6;
			this.numericUpDown1.Value = new decimal(new int[] {
									5,
									0,
									0,
									0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.NumericUpDown1ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 74);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(108, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "画笔每步X旋转度数";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 101);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(108, 12);
			this.label4.TabIndex = 9;
			this.label4.Text = "画笔每步Y旋转度数";
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Location = new System.Drawing.Point(121, 99);
			this.numericUpDown2.Maximum = new decimal(new int[] {
									7200,
									0,
									0,
									0});
			this.numericUpDown2.Minimum = new decimal(new int[] {
									7200,
									0,
									0,
									-2147483648});
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(76, 21);
			this.numericUpDown2.TabIndex = 8;
			this.numericUpDown2.Value = new decimal(new int[] {
									5,
									0,
									0,
									0});
			this.numericUpDown2.ValueChanged += new System.EventHandler(this.NumericUpDown2ValueChanged);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(226, 68);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(182, 90);
			this.textBox1.TabIndex = 10;
			this.textBox1.Text = "[A/D] 画笔X顺/逆时针转\r\n[W/S] 画笔Y顺/逆时针转\r\n[U/I] 画笔向上/下\r\n[J/K] 画笔开/合\r\n[←/→] 手臂逆/顺时针转\r\n[↑/↓" +
			"] 手臂前进/后退";
			this.textBox1.Enter += new System.EventHandler(this.TextBox1Enter);
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyDown);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(19, 136);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(177, 21);
			this.label5.TabIndex = 11;
			this.label5.Text = "点击窗体空白处以使用快捷键";
			this.label5.Click += new System.EventHandler(this.Label5Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(426, 312);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.numericUpDown2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "pcctrl";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyDown);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainFormKeyPress);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button button1;
	}
}
