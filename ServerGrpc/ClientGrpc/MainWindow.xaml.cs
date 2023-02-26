using ClientGrpc.Client;
using network.main;
using network.stream;
using network.unary;
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

            //id_text_box.Clear();
            //password_text_box.Clear();
            //nickname_text_box.Clear();

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

        private void init_channel_button_Click(object sender, RoutedEventArgs e)
        {
            if (_client == null)
            {
                InitClient();
            }

            if (string.IsNullOrEmpty(_serverAddr) == true)
            {
                MessageBox.Show("server ip is empty");
                return;
            }

            var id = id_text_box.Text;
            var password = password_text_box.Text;

            _client.InitChannel(_serverAddr, id, password);
            MessageBox.Show($"channel created");
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
            if (string.IsNullOrEmpty(message_text_box.Text) == true)
            {
                MessageBox.Show("message is empty");
                return;
            }
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
        private bool DispatchStream(StreamData data)
        {
            var recvString = $"stream recv: {data.Packet}";
            RichTextBoxString(recvString);
            switch (data.DataCase)
            {
                case StreamData.DataOneofCase.ConnectRes:
                    return DispatchStream_ConnectRes(data.ConnectRes);
                case StreamData.DataOneofCase.Disconnected:
                    return true;
                case StreamData.DataOneofCase.UserConnect:
                    return true;
                case StreamData.DataOneofCase.MessageRecv:
                    return true;
                default:
                    return false;
            }
        }

        private bool DispatchStream_ConnectRes(Stream_ConnectRes res)
        {
            MessageBox.Show($"stream recv: {network.types.StreamPacket.ConnectRes} status code: {res.Err}");
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
                default:
                    return false;
            }
        }

        private bool DispatchUnary_JoinRes(Unary_JoinRes res)
        {
            MessageBox.Show($"unary recv: {network.types.UnaryDataType.JoinRes} status code: {res.Err}");
            return true;
        }
        #endregion
    }
}
