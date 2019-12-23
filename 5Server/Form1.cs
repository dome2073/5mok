using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace _5Client
{

    enum STONE { none, black, white };

    public partial class Form1 : Form
    {
        //통신
        Socket mainSock; // 서버소켓
        IPAddress thisAddress;
        Socket clientSokect;
        delegate void AppendTextDelegate(Control ctrl, string s);
        AppendTextDelegate _textAppender;
        //클라이언트들의 소켓 정보를 담을 자료구조
        bool myturn = false;

        //----------------------------------------------------------------//
        int margin = 40;
        int 눈Size = 23; // gridSize
        int 돌Size = 28; // stoneSize
        int 화점Size = 10; // flowerSize

        List<Revive> lstRevive = new List<Revive>();
        int sequence = 0;         // 복기에 사용되는 순서
        bool reviveFlag = false;  // 복기 모드인지를 알리는 플래그

        Graphics g;
        Pen pen;
        Brush wBrush, bBrush;
        STONE[,] 바둑판 = new STONE[19, 19];
        bool flag = false;  // false = 검은 돌, true = 흰돌
        bool imageFlag = true;

        int stoneCnt = 1; // 수순
        Font font = new Font("맑은 고딕", 10);
        public Form1()
        {
            InitializeComponent();
            mainSock = new Socket(AddressFamily.InterNetwork,
               SocketType.Stream, ProtocolType.Tcp);
            _textAppender = new AppendTextDelegate(AppendText);


            this.Text = "오목게임_정대윤 - 서버";
            this.BackColor = Color.Orange;

            pen = new Pen(Color.Black);
            bBrush = new SolidBrush(Color.Black);
            wBrush = new SolidBrush(Color.White);



            this.ClientSize = new Size(2 * margin + 18 * 눈Size,
              2 * margin + 18 * 눈Size + menuStrip1.Height);

        }
        //통신--------------------------------------------------//
        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                mainSock.Close();
            }
            catch { }
        }

        void BeginStartServer(object sender, EventArgs e)
        {
            int port;
            if (!int.TryParse(txtPort.Text, out port))
            {
                MsgBoxHelper.Error("포트 번호가 잘못 입력되었거나 입력되지 않았습니다.");
                txtPort.Focus();
                txtPort.SelectAll();
                return;
            }
            if (thisAddress == null)
            { // 로컬호스트 주소를 사용한다.
                thisAddress = IPAddress.Loopback;
                //thisAddress = IPAddress.Parse("210.123.255.190");
                txtAddress.Text = thisAddress.ToString();
            }
            else
            {
                thisAddress = IPAddress.Parse(txtAddress.Text);
            }
            // 서버에서 클라이언트의 연결 요청을 대기하기 위해
            // 소켓을 열어둔다.
            IPEndPoint serverEP = new IPEndPoint(thisAddress, port);
            mainSock.Bind(serverEP);
            mainSock.Listen(10);
            AppendText(txtHistory, string.Format("서버 시작: @{0}", serverEP));
            // 비동기적으로 클라이언트의 연결 요청을 받는다.
            mainSock.BeginAccept(AcceptCallback, null);
        }
        void AcceptCallback(IAsyncResult ar)
        {
            // 클라이언트의 연결 요청을 수락한다.
            clientSokect = mainSock.EndAccept(ar);

            AsyncObject obj = new AsyncObject(4096);
            obj.WorkingSocket = clientSokect;

            // 텍스트박스에 클라이언트가 연결되었다고 써준다.
            AppendText(txtHistory, string.Format
            ("클라이언트 (@ {0})가 연결되었습니다.", clientSokect.RemoteEndPoint));


            //클라이언트에게 선공이라고 알림
            byte[] bDts = Encoding.UTF8.GetBytes("3:선공");
            clientSokect.Send(bDts);
            // 클라이언트의 데이터를 받는다.
            clientSokect.BeginReceive(obj.Buffer, 0, 4096, 0,
            DataReceived, obj);



        }
        void DataReceived(IAsyncResult ar)
        {
            // BeginReceive에서 추가적으로 넘어온 데이터를
            // AsyncObject 형식으로 변환한다.
            AsyncObject obj = (AsyncObject)ar.AsyncState;
            // as AsyncObject로 해도 됨
            // 데이터 수신을 끝낸다.
            int received = obj.WorkingSocket.EndReceive(ar);
            // 받은 데이터가 없으면(연결끊어짐) 끝낸다.
            if (received <= 0)
            {
                obj.WorkingSocket.Disconnect(false);
                obj.WorkingSocket.Close();
                return;
            }
            string id, msg;
            string x, y, stone, Return;
            // 텍스트로 변환한다.
            string text = Encoding.UTF8.GetString(obj.Buffer);
            // : 기준으로 짜른다.
            // tokens[0] - 보낸 사람 ID
            // tokens[1] - 보낸 메세지
            string[] tokens = text.Split(':');         
            if (tokens[0].Equals("0"))
            {
                Console.WriteLine(text);
                id = tokens[1];
                msg = tokens[2];
                AppendText(txtHistory, string.Format("[받음]{0}: {1}", id, msg));

            }
            else if (tokens[0].Equals("1"))
            {
                Console.WriteLine(text);
                x = tokens[1];
                y = tokens[2];
                stone = tokens[3];
                Return = tokens[4];
                Console.WriteLine("x:" + x);
                Console.WriteLine("y:" + x);
                Console.WriteLine("stone:" + stone);

                //턴확인
                if (Return.Equals("myturn"))
                {

                    myturn = true;
                }


                //그림판에 그림을 그려야됨

                int x1 = Convert.ToInt32(x);
                int y1 = Convert.ToInt32(y);
                Pannel1_Read(x1, y1, stone);
            }

            // 텍스트박스에 추가해준다.
            // 비동기식으로 작업하기 때문에 폼의 UI 스레드에서 작업을 해줘야 한다.
            // 따라서 대리자를 통해 처리한다.
            // 클라이언트에게 다시 전송

            // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
            obj.ClearBuffer();
            // 수신 대기
            obj.WorkingSocket.BeginReceive
            (obj.Buffer, 0, 4096, 0, DataReceived, obj);


        }
        void AppendText(Control ctrl, string s)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_textAppender, ctrl, s);
            else
            {
                string source = ctrl.Text;
                ctrl.Text = source + Environment.NewLine + s;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        // 텍스트 전송(채팅)
        private void OnSendData(object sender, EventArgs e)
        {
            // 서버가 대기중인지 확인한다.
            if (!mainSock.IsBound)
            {
                MsgBoxHelper.Warn("서버가 실행되고 있지 않습니다!");
                return;
            }
            // 보낼 텍스트
            string tts = txtTTS.Text.Trim();
            if (string.IsNullOrEmpty(tts))
            {
                MsgBoxHelper.Warn("텍스트가 입력되지 않았습니다!");
                txtTTS.Focus();
                return;
            }
            // 문자열을 utf8 형식의 바이트로 변환한다.
            byte[] bDts = Encoding.UTF8.GetBytes("0:Server" + ':' + tts);
            // 연결된  클라이언트에게 전송한다.
            try { clientSokect.Send(bDts); }
            catch
            {
                // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                try { clientSokect.Dispose(); } catch { }

            }
            // 전송 완료 후 텍스트박스에 추가하고, 원래의 내용은 지운다.
            AppendText(txtHistory, string.Format("[보냄]server: {0}", tts));
            txtTTS.Clear();
        }

        //오목 전송
        void OnSendData2(int x, int y, STONE stone)
        {
            // 서버가 대기중인지 확인한다.
            if (!mainSock.IsBound)
            {
                MsgBoxHelper.Warn("서버가 실행되고 있지 않습니다!");
                return;
            }

            String STONE = stone.ToString();
            Console.WriteLine("x : " + x);
            Console.WriteLine("Y : " + y);
            Console.WriteLine("색깔 1-검정, 2-흰돌 : " + stone);

            string turn = "myturn";
            byte[] bDts = Encoding.UTF8.GetBytes("1:" + x + ':' + y + ':' + STONE + ':' + turn + ':');
            //클라이언트에 전송한다.

            try { clientSokect.Send(bDts); }
            catch
            {
                // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                try { clientSokect.Dispose(); } catch { }

            }



        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 끝내기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); //끝내기
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);
            DrawBoard();
            DrawStone();
        }
        private void DrawBoard()
        {

            // panel1에 Graphics 객체 생성
            g = panel1.CreateGraphics();

            // 세로선 19개
            for (int i = 0; i < 19; i++)
            {
                g.DrawLine(pen, new Point(margin + i * 눈Size, margin),
                  new Point(margin + i * 눈Size, margin + 18 * 눈Size));
            }

            // 가로선 19개
            for (int i = 0; i < 19; i++)
            {
                g.DrawLine(pen, new Point(margin, margin + i * 눈Size),
                  new Point(margin + 18 * 눈Size, margin + i * 눈Size));
            }

            // 화점그리기
            for (int x = 3; x <= 15; x += 6)
                for (int y = 3; y <= 15; y += 6)
                {
                    g.FillEllipse(bBrush,
                      margin + 눈Size * x - 화점Size / 2,
                      margin + 눈Size * y - 화점Size / 2,
                      화점Size, 화점Size);
                }
        }

        public void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //자기 턴에만 그림을 그릴 수 있어야함.
            if (myturn == true)
            {
                if (reviveFlag == true)
                {
                    ReviveGame();
                    return;
                }

                // e.X는 픽셀단위, x는 바둑판 좌표
                int x = (e.X - margin + 눈Size / 2) / 눈Size;
                int y = (e.Y - margin + 눈Size / 2) / 눈Size;

                if (바둑판[x, y] != STONE.none)
                    return;

                // 바둑판[x,y] 에 돌을 그린다
                Rectangle r = new Rectangle(
                  margin + 눈Size * x - 돌Size / 2,
                  margin + 눈Size * y - 돌Size / 2,
                  돌Size, 돌Size);

                // 검은돌 차례
                if (flag == false)
                {
                    if (imageFlag == false)
                        g.FillEllipse(bBrush, r);
                    else
                    {
                        Bitmap bmp = new Bitmap("../../img/black.png");

                        g.DrawImage(bmp, r);

                    }

                    lstRevive.Add(new Revive(x, y, STONE.black, stoneCnt));
                    DrawStoneSequence(stoneCnt++, Brushes.White, r);
                    flag = true;
                    바둑판[x, y] = STONE.black;
                    OnSendData2(x, y, STONE.black);
                }
                else
                {
                    if (imageFlag == false)
                        g.FillEllipse(wBrush, r);
                    else
                    {
                        Bitmap bmp = new Bitmap("../../img/white.png");

                        g.DrawImage(bmp, r);

                    }
                    lstRevive.Add(new Revive(x, y, STONE.white, stoneCnt));
                    DrawStoneSequence(stoneCnt++, Brushes.Black, r);
                    flag = false;
                    바둑판[x, y] = STONE.white;
                    OnSendData2(x, y, STONE.white);

                }
                myturn = false;
                CheckOmok(x, y);

            }
            else
            {
                MsgBoxHelper.Error("당신의 턴이 아닙니다!");
            }

        }

        //마찬가지로 그림그림
        public void Pannel1_Read(int x, int y, string stone)
        {

            Invoke((MethodInvoker)delegate
            {

                if (reviveFlag == true)
                {
                    ReviveGame();
                    return;
                }

                if (바둑판[x, y] != STONE.none)
                    return;

                // 바둑판[x,y] 에 돌을 그린다
                Rectangle r = new Rectangle(
                  margin + 눈Size * x - 돌Size / 2,
                  margin + 눈Size * y - 돌Size / 2,
                  돌Size, 돌Size);

                // 검은돌 차례
                if (stone.Equals("black"))
                {
                    if (imageFlag == false)
                    {

                        g.FillEllipse(bBrush, r);
                    }
                    else
                    {
                        Bitmap bmp = new Bitmap("../../img/black.png");

                        g.DrawImage(bmp, r);

                    }

                    lstRevive.Add(new Revive(x, y, STONE.black, stoneCnt));
                    DrawStoneSequence(stoneCnt++, Brushes.White, r);
                    flag = true;
                    바둑판[x, y] = STONE.black;


                }
                else
                {
                    if (imageFlag == false)
                        g.FillEllipse(wBrush, r);
                    else
                    {
                        Bitmap bmp = new Bitmap("../../img/white.png");

                        g.DrawImage(bmp, r);

                    }
                    lstRevive.Add(new Revive(x, y, STONE.white, stoneCnt));
                    DrawStoneSequence(stoneCnt++, Brushes.Black, r);
                    flag = false;
                    바둑판[x, y] = STONE.white;



                }
                Console.WriteLine("x : " + x);
                Console.WriteLine("y : " + y);
                Console.WriteLine("돌색깔" + STONE.black);
                Console.WriteLine("돌색깔" + STONE.white);
                Console.WriteLine("현재오목 : " + stoneCnt);
                CheckOmok(x, y);
            });
        }

        //쓸모없는기능;;
        private void 그리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = false;
        }

        private void 이미지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = true;
        }



        private void DrawStone()
        {
            for (int x = 0; x < 19; x++)
                for (int y = 0; y < 19; y++)
                    if (바둑판[x, y] == STONE.white)
                        if (imageFlag == false)
                            g.FillEllipse(wBrush, margin + x * 눈Size - 돌Size / 2,
                              margin + y * 눈Size - 돌Size / 2, 돌Size, 돌Size);
                        else
                        {
                            Bitmap bmp = new Bitmap("../../img/white.png");
                            g.DrawImage(bmp, margin + x * 눈Size - 돌Size / 2,
                              margin + y * 눈Size - 돌Size / 2, 돌Size, 돌Size);
                        }

                    else if (바둑판[x, y] == STONE.black)
                        if (imageFlag == false)
                            g.FillEllipse(bBrush, margin + x * 눈Size - 돌Size / 2,
                              margin + y * 눈Size - 돌Size / 2, 돌Size, 돌Size);
                        else
                        {
                            Bitmap bmp = new Bitmap("../../img/black.png");
                            g.DrawImage(bmp, margin + x * 눈Size - 돌Size / 2,
                              margin + y * 눈Size - 돌Size / 2, 돌Size, 돌Size);
                        }
        }

        // 오목인지 체크하는 메서드
        private void CheckOmok(int x, int y)
        {



            int cnt = 1;

            // 오른쪽 방향
            for (int i = x + 1; i <= 18; i++)
                if (바둑판[i, y] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            // 왼쪽방향
            for (int i = x - 1; i >= 0; i--)
                if (바둑판[i, y] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            if (cnt == 5)
            {
                OmokComplete(x, y);
                return;
            }

            cnt = 1;

            // 아래 방향
            for (int i = y + 1; i <= 18; i++)
                if (바둑판[x, i] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            // 위 방향
            for (int i = y - 1; i >= 0; i--)
                if (바둑판[x, i] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            if (cnt == 5)
            {
                OmokComplete(x, y);
                return;
            }

            cnt = 1;

            // 대각선 오른쪽 위방향
            for (int i = x + 1, j = y - 1; i <= 18 && j >= 0; i++, j--)
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            // 대각선 왼쪽 아래 방향
            for (int i = x - 1, j = y + 1; i >= 0 && j <= 18; i--, j++)
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            if (cnt == 5)
            {
                OmokComplete(x, y);
                return;
            }

            cnt = 1;

            // 대각선 왼쪽 위방향
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            // 대각선 오른쪽 아래 방향
            for (int i = x + 1, j = y + 1; i <= 18 && j <= 18; i++, j++)
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;

            if (cnt == 5)
            {
                OmokComplete(x, y);
                return;
            }
        }

        // 오목이 되었을 떄 처리하는 루틴 
        private void OmokComplete(int x, int y)
        {
            saveGame();
            DialogResult res = MessageBox.Show(바둑판[x, y].ToString().ToUpper()
              + " Wins!\n새로운 게임을 시작할까요?", "게임 종료", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
                NewGame();
            else if (res == DialogResult.No)
                this.Close();
        }

        // 새로운 게임을 시작(초기화)
        private void NewGame()
        {
            imageFlag = true;
            flag = false;

            for (int x = 0; x < 19; x++)
                for (int y = 0; y < 19; y++)
                    바둑판[x, y] = STONE.none;

            panel1.Refresh();
            DrawBoard();
            DrawStone();
        }
        // 수순을 돌의 중앙에 써줍니다
        private void DrawStoneSequence(int v, Brush color, Rectangle r)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(v.ToString(), font, color, r, stringFormat);
        }
        // 저장된 파일을 읽고 수순대로 복기
        private void 복기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 파일 선택
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "../../Data/";
            ofd.Filter = "Omok files(*.omk)|*.omk";
            ofd.ShowDialog();
            string fileName = ofd.FileName;

            try
            {
                StreamReader r = File.OpenText(fileName);
                string line = "";

                // 파일 내용을 한 줄 씩 읽어서 lstRevive 리스트에 넣는다
                while ((line = r.ReadLine()) != null)
                {
                    string[] items = line.Split(' ');
                    Revive rev = new Revive(int.Parse(items[0]), int.Parse(items[1]),
                      items[2] == "black" ? STONE.black : STONE.white, int.Parse(items[3]));
                    lstRevive.Add(rev);
                }
                r.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // 복기 준비
            reviveFlag = true;
            sequence = 0;
            NewGame();
        }

        // 게임이 끝나면 게임내용을 저장 
        private void saveGame()
        {
            string filename = "../../Data/" + DateTime.Now.ToShortDateString() + "-"
              + DateTime.Now.Hour + DateTime.Now.Minute + ".omk";

            FileStream fs = new FileStream(filename, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);

            foreach (Revive item in lstRevive)
            {
                sw.WriteLine("{0} {1} {02} {03}", item.X, item.Y, item.stone, item.seq);
            }
            sw.Close();
            fs.Close();
        }
        // 복기 플래그가 true 일때, 마우스다운
        private void ReviveGame()
        {
            if (sequence < lstRevive.Count)
                DrawAStone(lstRevive[sequence++]);
        }

        // 복기 리스트에 있는 내용을 하나씩 그려준다
        private void DrawAStone(Revive item)
        {
            int x = item.X;
            int y = item.Y;
            STONE s = item.stone;
            int seq = item.seq;

            // 바둑판[x,y] 에 돌을 그린다
            Rectangle r = new Rectangle(
              margin + 눈Size * x - 돌Size / 2,
              margin + 눈Size * y - 돌Size / 2,
              돌Size, 돌Size);

            // 검은돌 차례
            if (s == STONE.black)
            {
                if (imageFlag == false)
                    g.FillEllipse(bBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../img/black.png");

                    g.DrawImage(bmp, r);


                }
                DrawStoneSequence(seq, Brushes.White, r);
                바둑판[x, y] = STONE.black;
            }
            else
            {
                if (imageFlag == false)
                    g.FillEllipse(wBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../img/white.png");
                    g.DrawImage(bmp, r);
                }
                DrawStoneSequence(seq, Brushes.Black, r);
                바둑판[x, y] = STONE.white;
            }
            CheckOmok(x, y);
        }
    }

    public static class MsgBoxHelper
    {
        public static DialogResult Warn(string s, MessageBoxButtons buttons =MessageBoxButtons.OK,params object[] args)
        {
            return MessageBox.Show(f(s, args), "경고", buttons, MessageBoxIcon.Exclamation);
        }
        public static DialogResult Error(string s, MessageBoxButtons buttons = MessageBoxButtons.OK, params object[] args)
        {
            return MessageBox.Show(f(s, args), "오류", buttons, MessageBoxIcon.Error);
        }
        public static DialogResult Info(string s, MessageBoxButtons buttons = MessageBoxButtons.OK,params object[] args)
        {
            return MessageBox.Show(f(s, args), "알림", buttons,MessageBoxIcon.Information);
        }
        public static DialogResult Show(string s,MessageBoxButtons buttons = MessageBoxButtons.OK,
        params object[] args)
        {
            return MessageBox.Show(f(s, args), "알림", buttons, 0);
        }
        static string f(string s, params object[] args)
        {
            if (args == null) return s;
            return string.Format(s, args);
        }
    }
    // 비동기 작업에서 사용하는 소켓과 해당 작업에 대한 데이터 버퍼를 저장하는 클래스
    public class AsyncObject
    {
        public byte[] Buffer;
        public Socket WorkingSocket;
        public readonly int BufferSize;
        public AsyncObject(int bufferSize)
        {
            BufferSize = bufferSize;
            Buffer = new byte[BufferSize];
        }

        public void ClearBuffer()
        {
            Array.Clear(Buffer, 0, BufferSize);
        }
    }
}
