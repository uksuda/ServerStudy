namespace ClientGrpc
{
    partial class HelpForm
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
            richTextBox_helpform_help = new System.Windows.Forms.RichTextBox();
            button_helpform_command_help = new System.Windows.Forms.Button();
            button_helpform_stream_help = new System.Windows.Forms.Button();
            button_helpform_param_help = new System.Windows.Forms.Button();
            button_helpform_clear_help = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // richTextBox_helpform_help
            // 
            richTextBox_helpform_help.Location = new System.Drawing.Point(12, 12);
            richTextBox_helpform_help.Name = "richTextBox_helpform_help";
            richTextBox_helpform_help.Size = new System.Drawing.Size(499, 426);
            richTextBox_helpform_help.TabIndex = 0;
            richTextBox_helpform_help.Text = "";
            // 
            // button_helpform_command_help
            // 
            button_helpform_command_help.Location = new System.Drawing.Point(517, 12);
            button_helpform_command_help.Name = "button_helpform_command_help";
            button_helpform_command_help.Size = new System.Drawing.Size(120, 47);
            button_helpform_command_help.TabIndex = 1;
            button_helpform_command_help.Text = "Command";
            button_helpform_command_help.UseVisualStyleBackColor = true;
            // 
            // button_helpform_stream_help
            // 
            button_helpform_stream_help.Location = new System.Drawing.Point(517, 74);
            button_helpform_stream_help.Name = "button_helpform_stream_help";
            button_helpform_stream_help.Size = new System.Drawing.Size(120, 47);
            button_helpform_stream_help.TabIndex = 2;
            button_helpform_stream_help.Text = "Stream";
            button_helpform_stream_help.UseVisualStyleBackColor = true;
            // 
            // button_helpform_param_help
            // 
            button_helpform_param_help.Location = new System.Drawing.Point(517, 137);
            button_helpform_param_help.Name = "button_helpform_param_help";
            button_helpform_param_help.Size = new System.Drawing.Size(120, 47);
            button_helpform_param_help.TabIndex = 3;
            button_helpform_param_help.Text = "Param";
            button_helpform_param_help.UseVisualStyleBackColor = true;
            // 
            // button_helpform_clear_help
            // 
            button_helpform_clear_help.Location = new System.Drawing.Point(517, 200);
            button_helpform_clear_help.Name = "button_helpform_clear_help";
            button_helpform_clear_help.Size = new System.Drawing.Size(120, 47);
            button_helpform_clear_help.TabIndex = 4;
            button_helpform_clear_help.Text = "Clear";
            button_helpform_clear_help.UseVisualStyleBackColor = true;
            // 
            // HelpForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(713, 450);
            Controls.Add(button_helpform_clear_help);
            Controls.Add(button_helpform_param_help);
            Controls.Add(button_helpform_stream_help);
            Controls.Add(button_helpform_command_help);
            Controls.Add(richTextBox_helpform_help);
            Name = "HelpForm";
            Text = "HelpForm";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_helpform_help;
        private System.Windows.Forms.Button button_helpform_command_help;
        private System.Windows.Forms.Button button_helpform_stream_help;
        private System.Windows.Forms.Button button_helpform_param_help;
        private System.Windows.Forms.Button button_helpform_clear_help;
    }
}