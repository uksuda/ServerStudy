namespace ClientGrpc
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rich_text_box_detail_info = new System.Windows.Forms.RichTextBox();
            button_join = new System.Windows.Forms.Button();
            text_box_server_ip = new System.Windows.Forms.TextBox();
            label_server_ip = new System.Windows.Forms.Label();
            text_box_id = new System.Windows.Forms.TextBox();
            label_id = new System.Windows.Forms.Label();
            label_command = new System.Windows.Forms.Label();
            text_box_command = new System.Windows.Forms.TextBox();
            label_message = new System.Windows.Forms.Label();
            text_box_message = new System.Windows.Forms.TextBox();
            label_password = new System.Windows.Forms.Label();
            text_box_password = new System.Windows.Forms.TextBox();
            label_nickname = new System.Windows.Forms.Label();
            text_box_nickname = new System.Windows.Forms.TextBox();
            label_params = new System.Windows.Forms.Label();
            text_box_params = new System.Windows.Forms.TextBox();
            label_token = new System.Windows.Forms.Label();
            label_token_value = new System.Windows.Forms.Label();
            button_login = new System.Windows.Forms.Button();
            button_command = new System.Windows.Forms.Button();
            button_message = new System.Windows.Forms.Button();
            button_server_connect = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // rich_text_box_detail_info
            // 
            rich_text_box_detail_info.Location = new System.Drawing.Point(12, 12);
            rich_text_box_detail_info.Name = "rich_text_box_detail_info";
            rich_text_box_detail_info.Size = new System.Drawing.Size(474, 481);
            rich_text_box_detail_info.TabIndex = 0;
            rich_text_box_detail_info.Text = "";
            // 
            // button_join
            // 
            button_join.Location = new System.Drawing.Point(767, 119);
            button_join.Name = "button_join";
            button_join.Size = new System.Drawing.Size(110, 23);
            button_join.TabIndex = 1;
            button_join.Text = "Join";
            button_join.UseVisualStyleBackColor = true;
            button_join.Click += new System.EventHandler(this.button_join_Click);
            // 
            // text_box_server_ip
            // 
            text_box_server_ip.Location = new System.Drawing.Point(587, 12);
            text_box_server_ip.Name = "text_box_server_ip";
            text_box_server_ip.Size = new System.Drawing.Size(162, 23);
            text_box_server_ip.TabIndex = 2;
            // 
            // label_server_ip
            // 
            label_server_ip.AutoSize = true;
            label_server_ip.Location = new System.Drawing.Point(514, 15);
            label_server_ip.Name = "label_server_ip";
            label_server_ip.Size = new System.Drawing.Size(54, 15);
            label_server_ip.TabIndex = 3;
            label_server_ip.Text = "Server Ip";
            // 
            // text_box_id
            // 
            text_box_id.Location = new System.Drawing.Point(587, 119);
            text_box_id.Name = "text_box_id";
            text_box_id.Size = new System.Drawing.Size(162, 23);
            text_box_id.TabIndex = 4;
            // 
            // label_id
            // 
            label_id.AutoSize = true;
            label_id.Location = new System.Drawing.Point(514, 122);
            label_id.Name = "label_id";
            label_id.Size = new System.Drawing.Size(19, 15);
            label_id.TabIndex = 5;
            label_id.Text = "ID";
            // 
            // label_command
            // 
            label_command.AutoSize = true;
            label_command.Location = new System.Drawing.Point(514, 286);
            label_command.Name = "label_command";
            label_command.Size = new System.Drawing.Size(64, 15);
            label_command.TabIndex = 7;
            label_command.Text = "Command";
            // 
            // text_box_command
            // 
            text_box_command.Location = new System.Drawing.Point(587, 283);
            text_box_command.Name = "text_box_command";
            text_box_command.Size = new System.Drawing.Size(162, 23);
            text_box_command.TabIndex = 6;
            // 
            // label_message
            // 
            label_message.AutoSize = true;
            label_message.Location = new System.Drawing.Point(514, 331);
            label_message.Name = "label_message";
            label_message.Size = new System.Drawing.Size(53, 15);
            label_message.TabIndex = 9;
            label_message.Text = "Message";
            // 
            // text_box_message
            // 
            text_box_message.Location = new System.Drawing.Point(587, 328);
            text_box_message.Name = "text_box_message";
            text_box_message.Size = new System.Drawing.Size(162, 23);
            text_box_message.TabIndex = 8;
            // 
            // label_password
            // 
            label_password.AutoSize = true;
            label_password.Location = new System.Drawing.Point(514, 159);
            label_password.Name = "label_password";
            label_password.Size = new System.Drawing.Size(59, 15);
            label_password.TabIndex = 11;
            label_password.Text = "PassWord";
            // 
            // text_box_password
            // 
            text_box_password.Location = new System.Drawing.Point(587, 156);
            text_box_password.Name = "text_box_password";
            text_box_password.Size = new System.Drawing.Size(162, 23);
            text_box_password.TabIndex = 10;
            // 
            // label_nickname
            // 
            label_nickname.AutoSize = true;
            label_nickname.Location = new System.Drawing.Point(514, 198);
            label_nickname.Name = "label_nickname";
            label_nickname.Size = new System.Drawing.Size(63, 15);
            label_nickname.TabIndex = 13;
            label_nickname.Text = "NickName";
            // 
            // text_box_nickname
            // 
            text_box_nickname.Location = new System.Drawing.Point(587, 195);
            text_box_nickname.Name = "text_box_nickname";
            text_box_nickname.Size = new System.Drawing.Size(162, 23);
            text_box_nickname.TabIndex = 12;
            // 
            // label_params
            // 
            label_params.AutoSize = true;
            label_params.Location = new System.Drawing.Point(514, 380);
            label_params.Name = "label_params";
            label_params.Size = new System.Drawing.Size(46, 15);
            label_params.TabIndex = 15;
            label_params.Text = "Params";
            // 
            // text_box_params
            // 
            text_box_params.Location = new System.Drawing.Point(587, 377);
            text_box_params.Name = "text_box_params";
            text_box_params.Size = new System.Drawing.Size(162, 23);
            text_box_params.TabIndex = 14;
            // 
            // label_token
            // 
            label_token.AutoSize = true;
            label_token.Location = new System.Drawing.Point(514, 56);
            label_token.Name = "label_token";
            label_token.Size = new System.Drawing.Size(76, 15);
            label_token.TabIndex = 16;
            label_token.Text = "Server Token";
            // 
            // label_token_value
            // 
            label_token_value.AutoSize = true;
            label_token_value.Location = new System.Drawing.Point(596, 56);
            label_token_value.Name = "label_token_value";
            label_token_value.Size = new System.Drawing.Size(41, 15);
            label_token_value.TabIndex = 18;
            label_token_value.Text = "empty";
            // 
            // button_login
            // 
            button_login.Location = new System.Drawing.Point(767, 156);
            button_login.Name = "button_login";
            button_login.Size = new System.Drawing.Size(110, 23);
            button_login.TabIndex = 19;
            button_login.Text = "Login";
            button_login.UseVisualStyleBackColor = true;
            button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // button_command
            // 
            button_command.Location = new System.Drawing.Point(767, 283);
            button_command.Name = "button_command";
            button_command.Size = new System.Drawing.Size(110, 23);
            button_command.TabIndex = 21;
            button_command.Text = "Command";
            button_command.UseVisualStyleBackColor = true;
            button_command.Click += new System.EventHandler(this.button_command_Click);
            // 
            // button_message
            // 
            button_message.Location = new System.Drawing.Point(767, 328);
            button_message.Name = "button_message";
            button_message.Size = new System.Drawing.Size(110, 23);
            button_message.TabIndex = 22;
            button_message.Text = "Message";
            button_message.UseVisualStyleBackColor = true;
            button_message.Click += new System.EventHandler(this.button_message_Click);
            // 
            // button_server_connect
            // 
            button_server_connect.Location = new System.Drawing.Point(767, 12);
            button_server_connect.Name = "button_server_connect";
            button_server_connect.Size = new System.Drawing.Size(110, 23);
            button_server_connect.TabIndex = 23;
            button_server_connect.Text = "Connect";
            button_server_connect.UseVisualStyleBackColor = true;
            button_server_connect.Click += new System.EventHandler(this.button_server_connect_Click);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(895, 505);
            Controls.Add(button_server_connect);
            Controls.Add(button_message);
            Controls.Add(button_command);
            Controls.Add(button_login);
            Controls.Add(label_token_value);
            Controls.Add(label_token);
            Controls.Add(label_params);
            Controls.Add(text_box_params);
            Controls.Add(label_nickname);
            Controls.Add(text_box_nickname);
            Controls.Add(label_password);
            Controls.Add(text_box_password);
            Controls.Add(label_message);
            Controls.Add(text_box_message);
            Controls.Add(label_command);
            Controls.Add(text_box_command);
            Controls.Add(label_id);
            Controls.Add(text_box_id);
            Controls.Add(label_server_ip);
            Controls.Add(text_box_server_ip);
            Controls.Add(button_join);
            Controls.Add(rich_text_box_detail_info);
            Name = "MainForm";
            Text = "Main";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.RichTextBox rich_text_box_detail_info;
        private System.Windows.Forms.Button button_join;
        private System.Windows.Forms.TextBox text_box_server_ip;
        private System.Windows.Forms.Label label_server_ip;
        private System.Windows.Forms.TextBox text_box_id;
        private System.Windows.Forms.Label label_id;
        private System.Windows.Forms.Label label_command;
        private System.Windows.Forms.TextBox text_box_command;
        private System.Windows.Forms.Label label_message;
        private System.Windows.Forms.TextBox text_box_message;
        private System.Windows.Forms.Label label_password;
        private System.Windows.Forms.TextBox text_box_password;
        private System.Windows.Forms.Label label_nickname;
        private System.Windows.Forms.TextBox text_box_nickname;
        private System.Windows.Forms.Label label_params;
        private System.Windows.Forms.TextBox text_box_params;
        private System.Windows.Forms.Label label_token;
        private System.Windows.Forms.Label label_token_value;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.Button button_command;
        private System.Windows.Forms.Button button_message;
        private System.Windows.Forms.Button button_server_connect;
    }
}