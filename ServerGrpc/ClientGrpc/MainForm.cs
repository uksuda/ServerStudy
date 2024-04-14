using ClientGrpc.Client;
using System.Text;
using System;
using System.Windows.Forms;
using Game.Main;
using Game.Stream;
using Game.Unary;

namespace ClientGrpc
{
    public partial class MainForm : Form
    {
        private GrpcClient _client;

        public MainForm()
        {
            InitializeComponent();
            InitUI();
        }

        public void InitUI()
        {
            rich_text_box_detail_info.Clear();
            label_token_value.ResetText();
            
            text_box_server_ip.Clear();

            text_box_id.Clear();
            text_box_password.Clear();
            text_box_nickname.Clear();

            text_box_command.Clear();
            text_box_message.Clear();
            text_box_params.Clear();

            ButtonEnableChange(false);
        }

        private void button_server_connect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(text_box_server_ip.Text) == true)
            {
                MessageBox.Show("server ip is empty");
                return;
            }

            try
            {
                _client = new GrpcClient();
                _client.InitChannel(text_box_server_ip.Text);
                _client.SetStreamCallBack(DispatchStream);

                ButtonEnableChange(true);
                RichTextBoxString($"{text_box_server_ip.Text} server connected");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error : {ex}");
            }
        }

        private async void button_join_Click(object sender, EventArgs e)
        {
            var req = new JoinReq
            {

            };

            var (code, msg, res) = await _client.Join(req);
            if (code != Game.Types.ResultCode.Success)
            {
                MessageBox.Show($"code: {code} msg: {msg} res: {res}");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Join result: {res}");
            sb.AppendLine($"Token: {res.Token}");
            RichTextBoxString(sb.ToString());
        }

        private async void button_login_Click(object sender, EventArgs e)
        {

        }

        private async void button_command_Click(object sender, EventArgs e)
        {
            var msg = new Unary_CommandReq
            {
                Command = "",
            };

            var unaryData = new UnaryData
            {
                Type = Game.Types.UnaryDataType.CommandReq,
                CommandReq = msg,
            };

            var (resMsg, res) = await _client.UnaryDataSend(unaryData);
            DispatchUnary(res);
        }

        private async void button_message_Click(object sender, EventArgs e)
        {
            var msg = new Stream_MessageSend
            {
                Message = "",
            };
            var data = new StreamMsg()
            {
                Packet = Game.Types.StreamPacket.MessageSend,
                MessageSend = msg,
            };

            try
            {
                await _client.SendMsg(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"message_btn_click error: {ex.Message}, {ex.InnerException}");
            }
        }

        private void RichTextBoxString(string s)
        {
            rich_text_box_detail_info.Focus();
            rich_text_box_detail_info.AppendText(s);
            rich_text_box_detail_info.ScrollToCaret();
        }

        private void ButtonEnableChange(bool enable)
        {
            button_join.Enabled = enable;
            button_login.Enabled = enable;
            button_command.Enabled = enable;
            button_message.Enabled = enable;
        }

        #region Dispatch Stream
        private bool DispatchStream(StreamMsg data)
        {
            var recvString = $"stream recv: {data.Packet}";
            RichTextBoxString(recvString);
            switch (data.DataCase)
            {
                case StreamMsg.DataOneofCase.ConnectRes:
                    return DispatchStream_ConnectRes(data.ConnectRes);
                case StreamMsg.DataOneofCase.Disconnected:
                    return DispatchStream_Disconnected(data.Disconnected);
                case StreamMsg.DataOneofCase.UserConnect:
                    return DispatchStream_UserConnect(data.UserConnect);
                case StreamMsg.DataOneofCase.UserDisconnect:
                    return DispatchStream_UserDisconnect(data.UserDisconnect);
                case StreamMsg.DataOneofCase.MessageRecv:
                    return DispatchStream_MessageRecv(data.MessageRecv);
                default:
                    return false;
            }
        }

        private bool DispatchStream_ConnectRes(Stream_ConnectRes res)
        {
            var resultStr = $"stream recv: {Game.Types.StreamPacket.ConnectRes} status code: {res.Result}";
            RichTextBoxString(resultStr);
            return true;
        }

        private bool DispatchStream_Disconnected(Stream_Disconnected res)
        {
            RichTextBoxString($"stream recv: {Game.Types.StreamPacket.Disconnected} status code: {res.Result}");
            return true;
        }

        private bool DispatchStream_UserConnect(Stream_UserConnect res)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"stream recv: {Game.Types.StreamPacket.UserConnect}");
            builder.AppendLine($"user: {res.ConnectUser.UserIndex}. {res.ConnectUser.Nickname}");
            RichTextBoxString(builder.ToString());
            return true;
        }

        private bool DispatchStream_UserDisconnect(Stream_UserDisconnect res)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"stream recv: {Game.Types.StreamPacket.UserDisconnect}");
            builder.AppendLine($"user: {res.UserIndex}");
            RichTextBoxString(builder.ToString());
            return true;
        }

        private bool DispatchStream_MessageRecv(Stream_MessageRecv res)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"stream recv: {Game.Types.StreamPacket.MessageRecv}");
            builder.AppendLine($"user: {res.SendUser.UserIndex}. {res.SendUser.Nickname}");
            builder.AppendLine($"message: {res.Message}");
            RichTextBoxString(builder.ToString());
            return true;
        }
        #endregion

        #region Dispatch Unary
        private bool DispatchUnary(UnaryData data)
        {
            var recvString = $"unary recv: {data.Type}";
            RichTextBoxString(recvString);

            switch (data.DataCase)
            {
                case UnaryData.DataOneofCase.CommandRes:
                    return DispatchUnary_CommandRes(data.CommandRes);
                default:
                    return false;
            }
        }

        private bool DispatchUnary_CommandRes(Unary_CommandRes res)
        {
            var resultStr = $"unary recv: {Game.Types.UnaryDataType.CommandRes} status code: {res.Result}\r\n";
            RichTextBoxString(resultStr + res.Result);
            return true;
        }
        #endregion
    }
}