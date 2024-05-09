using Game.Main;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClientGrpc
{
    public partial class HelpForm : Form
    {
        enum HelpType
        {
            None = 0,
            Command,
            Stream,
            Param
        }

        private StringBuilder _helpBuilder = new StringBuilder();

        public HelpForm()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            richTextBox_helpform_help.Clear();
            richTextBox_helpform_help.ReadOnly = true;
            richTextBox_helpform_help.BorderStyle = BorderStyle.None;
            richTextBox_helpform_help.TabStop = false;
            _helpBuilder.Clear();
        }

        private void ResultToLogBox(string s)
        {
            richTextBox_helpform_help.AppendText(Environment.NewLine);
            richTextBox_helpform_help.AppendText(s);
            richTextBox_helpform_help.SelectionStart = richTextBox_helpform_help.TextLength;
            richTextBox_helpform_help.ScrollToCaret();
            richTextBox_helpform_help.Update();
        }

        #region Button Callback
        private void button_helpform_command_help_Click(object sender, EventArgs e)
        {
            MakeHelp(HelpType.Command);
            ResultToLogBox(_helpBuilder.ToString());
        }

        private void button_helpform_stream_help_Click(object sender, EventArgs e)
        {
            MakeHelp(HelpType.Stream);
            ResultToLogBox(_helpBuilder.ToString());
        }

        private void button_helpform_param_help_Click(object sender, EventArgs e)
        {
            MakeHelp(HelpType.Param);
            ResultToLogBox(_helpBuilder.ToString());
        }

        private void button_helpform_clear_Click(object sender, EventArgs e)
        {
            richTextBox_helpform_help.Clear();
        }
        #endregion

        private void MakeHelp(HelpType type)
        {
            _helpBuilder.Clear();
            switch (type)
            {
                case HelpType.Command:
                    {
                        var values = Enum.GetValues(typeof(Game.Types.UnaryDataType));
                        foreach (var e in values)
                        {
                            var name = e.ToString();
                            var num = (int)e;
                            _helpBuilder.AppendLine($"name: {name} field number: {num}");
                        }
                    }
                    break;
                case HelpType.Stream:
                    {
                        var values = Enum.GetValues(typeof(Game.Types.StreamPacket));
                        foreach (var e in values)
                        {
                            var name = e.ToString();
                            var num = (int)e;
                            _helpBuilder.AppendLine($"name: {name} field number: {num}");
                        }
                    }
                    break;
                case HelpType.Param:
                    {
                        _helpBuilder.AppendLine("param separated by ';'");
                        _helpBuilder.AppendLine("-------------- Command -------------");
                        var oneof_unary = UnaryData.Descriptor.Oneofs.Where(x => x.Name.Equals("data")).FirstOrDefault();
                        foreach (var o in oneof_unary.Fields)
                        {
                            //var name = o.Name;
                            //var number = o.FieldNumber;
                            _helpBuilder.AppendLine($"- {o.FieldNumber} - {o.Name}");

                            var fields = o.MessageType.Fields;
                            foreach (var of in fields.InFieldNumberOrder())
                            {
                                //var f_num = of.FieldNumber;
                                //var f_name = of.Name;
                                //var f_type = of.FieldType;
                                _helpBuilder.AppendLine($"-- {of.FieldNumber} : {of.FieldType} : {of.Name}");
                            }
                            _helpBuilder.AppendLine();
                        }

                        _helpBuilder.AppendLine();
                        _helpBuilder.AppendLine();

                        _helpBuilder.AppendLine("-------------- Stream -------------");
                        var oneof_stream = StreamData.Descriptor.Oneofs.Where(x => x.Name.Equals("data")).FirstOrDefault();
                        foreach (var o in oneof_stream.Fields)
                        {
                            _helpBuilder.AppendLine($"- {o.FieldNumber} - {o.Name}");
                            var fields = o.MessageType.Fields;
                            foreach (var of in fields.InFieldNumberOrder())
                            {
                                //var f_num = of.FieldNumber;
                                //var f_name = of.Name;
                                //var f_type = of.FieldType;
                                _helpBuilder.AppendLine($"-- {of.FieldNumber} : {of.FieldType} : {of.Name}");
                            }
                            _helpBuilder.AppendLine();
                        }
                    }
                    break;
                default:
                    return;
            }
        }
    }
}
