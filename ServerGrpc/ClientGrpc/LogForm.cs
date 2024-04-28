using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGrpc
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            richTextBox_logform.Clear();
        }

        private void button_clear_logform_Click(object sender, EventArgs e)
        {
            richTextBox_logform.Clear();
        }

        public void RichTextBoxString(string s)
        {
            richTextBox_logform.Focus();
            richTextBox_logform.AppendText(s);
            richTextBox_logform.ScrollToCaret();

            //richTextBox_logform.AppendText(Environment.NewLine);
            //richTextBox_logform.AppendText(s);
            //richTextBox_logform.SelectionStart = rich_text_box_detail_info.TextLength;
            //richTextBox_logform.ScrollToCaret();
            //richTextBox_logform.Update();
        }
    }
}
