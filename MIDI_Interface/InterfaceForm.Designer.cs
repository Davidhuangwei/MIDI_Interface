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
            this.Button1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Button1
            // 
            this.Button1.BackColor = System.Drawing.Color.Red;
            this.Button1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Button1.Location = new System.Drawing.Point(12, 12);
            this.Button1.Name = "Button1";
            this.Button1.ReadOnly = true;
            this.Button1.Size = new System.Drawing.Size(48, 13);
            this.Button1.TabIndex = 0;
            this.Button1.Text = "Button 1";
            this.Button1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // InterfaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 364);
            this.Controls.Add(this.Button1);
            this.Name = "InterfaceForm";
            this.Text = "InterfaceForm";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Resize += new System.EventHandler(this.Form_Resize);

        }

        #endregion

        internal System.Windows.Forms.TextBox Button1;

    }
}