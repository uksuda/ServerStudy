using ClientGrpc.Client;
using network.main;
using network.stream;
using network.unary;
using System.Reflection;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

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

            message_text_box.Clear();
            password_text_box.Clear();
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
            _client.InitChannel(_serverAddr);
        }

        private bool ConnectClient()
        {
            if (string.IsNullOrEmpty(ip_text_box.Text) == true)
            {
                MessageBox.Show("server ip txt box is empty");
                return false;
            }

            _serverAddr = ip_text_box.Text;
            if (string.IsNullOrEmpty(_serverAddr) == true)
            {
                MessageBox.Show("server address is empty");
                return false;
            }

            // TODO: send connect req
            return true;
        }

        private void connect_btn_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectClient() == false)
            {
                MessageBox.Show("fail to connect server");
                return;
            }

            MessageBox.Show("Success connect");
        }

        private void stream_open_Click(object sender, RoutedEventArgs e)
        {
            if (_client.StreamOpen() == false)
            {
                MessageBox.Show($"fail to stream open");
                return;
            }
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

            var data = new StreamData
            {
                Packet = network.types.StreamPacket.TestReq,
                TestReq = new network.stream.Stream_TestReq
                {
                    Test = message_text_box.Text,
                },
            };

            await _client.SendMsg(data);
        }

        private async void Unary_data_send_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ip_text_box.Text) == true)
            {
                MessageBox.Show("server address is empty");
                return;
            }

            var req = new UnaryData
            {
                Type = network.types.UnaryDataType.SampleReq,
                SampleReq = new Unary_SampleReq { S = "unary sample req", },
            };


            var res = await _client.UnaryDataSend(req);
            DispatchUnary(res);
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

            //MessageBox.Show($"stream recv: {data.Packet}");

            switch (data.DataCase)
            {
                case StreamData.DataOneofCase.TestRes:                    
                    return DispatchStream_TestRes(data.TestRes);
                case StreamData.DataOneofCase.ConnectRes:
                    return DispatchStream_ConnectRes(data.ConnectRes);
                default:
                    return false;
            }
        }

        private bool DispatchStream_TestRes(Stream_TestRes msg)
        {
            RichTextBoxString($"statuc code: {msg.Err}");
            RichTextBoxString(msg.Response);
            return true;
        }

        private bool DispatchStream_ConnectRes(Stream_ConnectRes msg)
        {
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
                case UnaryData.DataOneofCase.SampleRes:
                    return DispatchUnary_SampleRes(data.SampleRes);
                default:
                    return false;
            }
        }

        private bool DispatchUnary_SampleRes(Unary_SampleRes data)
        {
            return true;
        }
        #endregion
    }
}
