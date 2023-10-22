using ClientGrpc.Client;
using Microsoft.VisualBasic.Devices;
using System.Text;
using System;
using System.Windows.Forms;
using Network.Main;
using Network.Stream;
using Network.Unary;

namespace ClientGrpc
{
    public partial class MainForm : Form
    {
        private string _serverAddr = "http://localhost:5041"; //"http://127.0.0.1:5042"
        private string _message = string.Empty;

        private GrpcClient _client;

        public MainForm()
        {
            InitializeComponent();
        }

        public void InitUI()
        {

        }

        public void InitClient()
        {
            if (_client == null)
            {
                _client = new GrpcClient();
            }

            _client.SetStreamCallBack(DispatchStream);
        }

        private void stream_open_Click(object sender, EventArgs e)
        {
            
        }

        private void ip_text_box_TextChanged(object sender, EventArgs e)
        {
            
        }

        private async void message_btn_Click(object sender, EventArgs e)
        {
            if (_client.IsReadyChannel() == false)
            {
                MessageBox.Show("you need to stream or join first");
                return;
            }

            var msg = new Stream_MessageSend
            {
                Message = "",
            };
            var data = new StreamMsg()
            {
                Packet = Network.Types.StreamPacket.MessageSend,
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

        private async void command_send_btn_Click(object sender, EventArgs e)
        {
            if (_client.IsReadyChannel() == false)
            {
                MessageBox.Show("you need to stream or join first");
                return;
            }

            var msg = new Unary_CommandReq
            {
                Command = "",
            };

            var unaryData = new UnaryData
            {
                Type = Network.Types.UnaryDataType.CommandReq,
                CommandReq = msg,
            };

            var result = await _client.UnaryDataSend(unaryData);
            DispatchUnary(result);
        }

        private async void join_req_button_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                InitClient();
            }

            if (_client.IsReadyChannel() == false)
            {
                //_client.InitChannel(_serverAddr, id, password);
            }

            var req = new JoinReq
            {
                
            };

            var result = await _client.Join(req);
            var sb = new StringBuilder();
            sb.AppendLine($"Join result: {result.Result}");
            sb.AppendLine($"Token: {result.Token}");

            RichTextBoxString(sb.ToString());
            //RichTextBoxString();
        }

        private void RichTextBoxString(string str)
        {
            
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
            var resultStr = $"stream recv: {Network.Types.StreamPacket.ConnectRes} status code: {res.Result}";
            RichTextBoxString(resultStr);
            return true;
        }

        private bool DispatchStream_Disconnected(Stream_Disconnected res)
        {
            RichTextBoxString($"stream recv: {Network.Types.StreamPacket.Disconnected} status code: {res.Result}");
            return true;
        }

        private bool DispatchStream_UserConnect(Stream_UserConnect res)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"stream recv: {Network.Types.StreamPacket.UserConnect}");
            builder.AppendLine($"user: {res.ConnectUser.UserIndex}. {res.ConnectUser.Nickname}");
            RichTextBoxString(builder.ToString());
            return true;
        }

        private bool DispatchStream_UserDisconnect(Stream_UserDisconnect res)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"stream recv: {Network.Types.StreamPacket.UserDisconnect}");
            builder.AppendLine($"user: {res.DisconnectUser.UserIndex}. {res.DisconnectUser.Nickname}");
            RichTextBoxString(builder.ToString());
            return true;
        }

        private bool DispatchStream_MessageRecv(Stream_MessageRecv res)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"stream recv: {Network.Types.StreamPacket.MessageRecv}");
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
            var resultStr = $"unary recv: {Network.Types.UnaryDataType.CommandRes} status code: {res.Result}\r\n";
            RichTextBoxString(resultStr + res.Result);
            return true;
        }
        #endregion
    }
}