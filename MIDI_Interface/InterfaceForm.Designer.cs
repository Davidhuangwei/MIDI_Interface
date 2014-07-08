namespace MIDI_Interface
{
    partial class InterfaceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button1 = new System.Windows.Forms.Label();
            this.Button2 = new System.Windows.Forms.Label();
            this.Fader1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Fader1)).BeginInit();
            this.SuspendLayout();
            // 
            // Button1
            // 
            this.Button1.BackColor = System.Drawing.Color.Red;
            this.Button1.Location = new System.Drawing.Point(12, 12);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(48, 13);
            this.Button1.TabIndex = 0;
            this.Button1.Text = "Button 1";
            this.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Button2
            // 
            this.Button2.BackColor = System.Drawing.Color.Red;
            this.Button2.Location = new System.Drawing.Point(66, 12);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(48, 13);
            this.Button2.TabIndex = 0;
            this.Button2.Text = "Button 2";
            this.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Fader1
            // 
            this.Fader1.Location = new System.Drawing.Point(12, 77);
            this.Fader1.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.Fader1.Name = "Fader1";
            this.Fader1.Size = new System.Drawing.Size(47, 20);
            this.Fader1.TabIndex = 1;
            this.Fader1.ValueChanged += new System.EventHandler(this.Fader1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fader 1";
            // 
            // InterfaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 364);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Fader1);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Button1);
            this.Name = "InterfaceForm";
            this.Text = "InterfaceForm";
            this.Resize += new System.EventHandler(this.Form_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Fader1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Button1;
        internal System.Windows.Forms.Label Button2;
        private System.Windows.Forms.NumericUpDown Fader1;
        private System.Windows.Forms.Label label1;

    }
}