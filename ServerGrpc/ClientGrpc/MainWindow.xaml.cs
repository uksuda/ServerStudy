using ClientGrpc.Client;
using network.main;
using network.stream;
using network.unary;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ClientGrpc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _serverAddr = "http://localhost:5041"; //"http://127.0.0.1:5042"
        private string _message = string.Empty;

        private GrpcClient _client;

        private FlowDocument _flowDoc;
        private Paragraph _paragraph;
        private Run _run;

        public MainWindow()
        {
            InitializeComponent();
            InitUI();
            InitClient();
        }

        public void InitUI()
        {
            ip_text_box.Clear();
            ip_text_box.Text = _serverAddr;

            id_text_box.Clear();
            password_text_box.Clear();
            nickname_text_box.Clear();
            command_text_box.Clear();
            message_text_box.Clear();

            message_rich_text_box.Document.Blocks.Clear();
            
            message_rich_text_box.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            message_rich_text_box.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            _flowDoc = message_rich_text_box.Document;
            _paragraph = new Paragraph();
            _run = new Run();
        }

        public void InitClient()
        {
            if (_client == null)
            {
                _client = new GrpcClient();
            }

            _client.SetStreamCallBack(DispatchStream);
        }

        private void stream_open_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(id_text_box.Text) == true ||
                string.IsNullOrEmpty(password_text_box.Text) == true ||
                string.IsNullOrEmpty(nickname_text_box.Text) == true)
            {
                MessageBox.Show($"you need id, password, nickname");
                return;
            }

            var id = id_text_box.Text;
            var password = password_text_box.Text;
            var nickname = nickname_text_box.Text;

            if (_client.IsReadyChannel() == false)
            {
                _client.InitChannel(_serverAddr, id, password);
            }

            if (_client.StreamOpen() == false)
            {
                MessageBox.Show($"fail to stream open");
                return;
            }

            MessageBox.Show($"stream open success");
        }

        private void ip_text_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _serverAddr = ip_text_box.Text;
        }

        private async void message_btn_Click(object sender, RoutedEventArgs e)
        {
            if (_client.IsReadyChannel() == false)
            {
                MessageBox.Show("you need to stream or join first");
                return;
            }

            if (string.IsNullOrEmpty(message_text_box.Text) == true)
            {
                MessageBox.Show("message is empty");
                return;
            }

            var msg = new Stream_MessageSend
            {
                Message = message_text_box.Text,
            };
            var data = new StreamMsg()
            {
                Packet = network.types.StreamPacket.MessageSend,
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

        private async void command_send_btn_Click(object sender, RoutedEventArgs e)
        {
            if (_client.IsReadyChannel() == false)
            {
                MessageBox.Show("you need to stream or join first");
                return;
            }

            if (string.IsNullOrEmpty(command_text_box.Text) == true)
            {
                MessageBox.Show("command text is empty");
                return;
            }

            var msg = new Unary_CommandReq
            {
                Command = command_text_box.Text,
            };

            var unaryData = new UnaryData
            {
                Type = network.types.UnaryDataType.CommandReq,
                CommandReq = msg,
            };

            var result = await _client.UnaryDataSend(unaryData);
            DispatchUnary(result);
        }

        private async void join_req_button_Click(object sender, RoutedEventArgs e)
        {
            if (_client == null)
            {
                InitClient();
            }

            if (string.IsNullOrEmpty(id_text_box.Text) == true ||
                string.IsNullOrEmpty(password_text_box.Text) == true ||
                string.IsNullOrEmpty(nickname_text_box.Text) == true)
            {
                MessageBox.Show($"you need id, password, nickname");
                return;
            }

            var id = id_text_box.Text;
            var password = password_text_box.Text;
            var nickname = nickname_text_box.Text;

            if (_client.IsReadyChannel() == false)
            {
                _client.InitChannel(_serverAddr, id, password);
            }

            var req = new Unary_JoinReq
            {
                Id = id,
                Password = password,
                Nickname = nickname,
            };

            var unaryData = new UnaryData
            {
                Type = network.types.UnaryDataType.JoinReq,
                JoinReq = req,
            };

            var result = await _client.UnaryDataSend(unaryData);
            DispatchUnary(result);
        }

        private void RichTextBoxString(string str)
        {
            _run.Text += str + '\n';
            _paragraph.Inlines.Add(_run);
            _flowDoc.Blocks.Add(_paragraph);
            message_rich_text_box.Document = _flowDoc;
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
            var resultStr = $"stream recv: {network.types.StreamPacket.ConnectRes} status code: {res.Result}";
            RichTextBoxString(resultStr);
            return true;
        }

        private bool DispatchStream_Disconnected(Stream_Disconnected res)
        {
            RichTextBoxString($"stream recv: {network.types.StreamPacket.Disconnected} status code: {res.Result}");
            return true;
        }

        private bool DispatchStream_UserConnect(Stream_UserConnect res)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"stream recv: {network.types.StreamPacket.UserConnect}");
            builder.AppendLine($"user: {res.ConnectUser.UserIndex}. {res.ConnectUser.Nickname}");
            RichTextBoxString(builder.ToString());
            return true;
        }

        private bool DispatchStream_UserDisconnect(Stream_UserDisconnect res)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"stream recv: {network.types.StreamPacket.UserDisconnect}");
            builder.AppendLine($"user: {res.DisconnectUser.UserIndex}. {res.DisconnectUser.Nickname}");
            RichTextBoxString(builder.ToString());
            return true;
        }

        private bool DispatchStream_MessageRecv(Stream_MessageRecv res)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"stream recv: {network.types.StreamPacket.MessageRecv}");
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
                case UnaryData.DataOneofCase.JoinRes:
                    return DispatchUnary_JoinRes(data.JoinRes);
                case UnaryData.DataOneofCase.CommandRes:
                    return DispatchUnary_CommandRes(data.CommandRes);
                default:
                    return false;
            }
        }

        private bool DispatchUnary_JoinRes(Unary_JoinRes res)
        {
            var resultStr = $"unary recv: {network.types.UnaryDataType.JoinRes} status code: {res.Result}";
            RichTextBoxString(resultStr);
            return true;
        }

        private bool DispatchUnary_CommandRes(Unary_CommandRes res)
        {
            var resultStr = $"unary recv: {network.types.UnaryDataType.CommandRes} status code: {res.Result}\r\n";
            RichTextBoxString(resultStr + res.Result);
            return true;
        }
        #endregion

        private void nickname_text_box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
