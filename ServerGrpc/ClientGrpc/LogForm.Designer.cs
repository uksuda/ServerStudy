namespace ClientGrpc
{
    partial class LogForm
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
            richTextBox_logform = new System.Windows.Forms.RichTextBox();
            button_clear_logform = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // richTextBox_logform
            // 
            richTextBox_logform.Location = new System.Drawing.Point(12, 12);
            richTextBox_logform.Name = "richTextBox_logform";
            richTextBox_logform.Size = new System.Drawing.Size(482, 426);
            richTextBox_logform.TabIndex = 0;
            richTextBox_logform.Text = "";
            // 
            // button_clear_logform
            // 
            button_clear_logform.Location = new System.Drawing.Point(510, 12);
            button_clear_logform.Name = "button_clear_logform";
            button_clear_logform.Size = new System.Drawing.Size(119, 34);
            button_clear_logform.TabIndex = 1;
            button_clear_logform.Text = "Clear Log";
            button_clear_logform.UseVisualStyleBackColor = true;
            // 
            // LogForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(button_clear_logform);
            Controls.Add(richTextBox_logform);
            Name = "LogForm";
            Text = "LogForm";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_logform;
        private System.Windows.Forms.Button button_clear_logform;
    }
}