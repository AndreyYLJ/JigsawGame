using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Jigsaw
{
    public partial class Form1 : Form
    {
        #region
        int Tipscout = 0;
        int Tip1 = -1;
        int Tip2 = -1;
        readonly Color COLOR_SELECT = Color.OrangeRed;//图片方块为被选中的边框颜色
        readonly Color COLOR_MOUSEMOVE = Color.MediumBlue;//鼠标移入方块的边框颜色
        int ALL = 100;
        int OldIGameState;
        int GameLevel = 0;//0--6
        readonly int[,] Difficulty = new int[,] {{ 3, 4 }, { 4, 6 },{ 5,6 },{ 5,7 },{ 6,8 } } ;//4，6//不同难度对应的行数和列数
        bool BPlaySound = true;//是否开启声音
        string[] GameState = new string[] { "等待", "运行", "胜利", "失败" };
        int IGameState = 5;//游戏状态 0对应等待 1运行 2 胜利 3 失败 4暂停 5开始界面
        //int IGameLeaveTime = 0; //游戏剩余时间
        bool BUseMyPicture = false;
        Bitmap[] BmpGrids = new Bitmap[63];//24

        int IWidth = 960, IHeight = 540;//图片尺寸 850 420
        Point PLeftTop = new Point(0, 0);
        Point PRightButton = new Point(1280, 697);

        int IRowCount = 3, IColCount = 4;//图片初始化被分成2*3
        string StrMyPicturePath = "";//自选图片的路径
        Bitmap BmpRate;//保存将原始图片缩放成960*540的位图
        Bitmap OldBmpRate;
        int[] NewIndex = new int[63];//
        int GameLeacveSeconds = 5;//游戏开始前有5秒钟观察图片

        int OldMouseIndex = -1;
        int NewMouseIndex = -1;

        int FirstSelectIndex = -1;
        int SecondSelectIndex = -1;

        string PictureName="";
        bool BGMFlag = true;

        int BWidth = 260, BHeight = 110;
        [DllImport("winmm")]
        public static extern bool PlaySound(string szSound, int hMod, int i);


        #endregion


        
        public Form1()
        {
            InitializeComponent();

            
            PLeftTop.X = 0;
            PLeftTop.Y = 0;
            PRightButton.X = ClientRectangle.Width;
            PRightButton.Y = ClientRectangle.Height - 27;//12
            IWidth = this.ClientRectangle.Width+1;//设置图片大小的尺寸
            IHeight = this.ClientRectangle.Height - 27;//12

            BWidth =  Convert.ToInt32(PRightButton.X / 4.923);
            BHeight = Convert.ToInt32(PRightButton.Y / 6.227);
            //开始界面
            this.button1.Size = new Size(BWidth, BHeight);
            this.button2.Size = new Size(BWidth, BHeight);
            this.button3.Size = new Size(BWidth, BHeight);
            this.button4.Size = new Size(BWidth, BHeight);
            //newgame
            this.button5.Size = new Size(BWidth, BHeight);
            this.button6.Size = new Size(BWidth, BHeight);
            this.button7.Size = new Size(BWidth, BHeight);
            //difficulty
            this.button8.Size = new Size(BWidth/10*7, BHeight/10*7);
            this.button9.Size = new Size(BWidth/10*7, BHeight/10*7);
            this.button10.Size = new Size(BWidth/10*7, BHeight/10*7);
            this.button11.Size = new Size(BWidth/10*7, BHeight/10*7);
            this.button12.Size = new Size(BWidth / 10 * 7, BHeight / 10 * 7);

            //setting
            this.flowLayoutPanel5.Size = new Size(BWidth, BHeight);
            this.flowLayoutPanel6.Size = new Size(BWidth/2, BHeight);
            this.flowLayoutPanel7.Size = new Size(BWidth, BHeight);
            this.flowLayoutPanel8.Size = new Size(BWidth / 2, BHeight);
            this.label1.Size = new Size(BWidth/3, BHeight);
            this.radioButton1.Size = new Size(BWidth / 3, BHeight / 3);
            this.radioButton2.Size = new Size(BWidth / 3, BHeight / 3);
            this.label2.Size = new Size(BWidth / 3, BHeight);
            this.radioButton3.Size = new Size(BWidth / 3, BHeight / 3);
            this.radioButton4.Size = new Size(BWidth / 3, BHeight / 3);
            this.button13.Size = new Size(BWidth, BHeight);

            //help
            this.label3.Size = new Size(BWidth, 3 * BHeight);
            this.button14.Size = new Size(BWidth, BHeight);





            this.flowLayoutPanel2.Location = new Point(PRightButton.X/10*7, PRightButton.Y/5);
            this.flowLayoutPanel2.Size = new Size(BWidth*2, BHeight * 5);

            this.flowLayoutPanel1.Location = new Point(PRightButton.X / 10 * 7, PRightButton.Y / 5);//新游戏
            this.flowLayoutPanel1.Size = new Size(BWidth * 2, BHeight * 5);
            this.flowLayoutPanel3.Location = new Point(PRightButton.X / 10 * 7, PRightButton.Y / 5);//难度
            this.flowLayoutPanel3.Size = new Size(BWidth , BHeight * 5);
            this.flowLayoutPanel4.Location = new Point(PRightButton.X / 10 * 7, PRightButton.Y / 5);//音效
            this.flowLayoutPanel4.Size = new Size(BWidth*2, BHeight * 4);
            this.flowLayoutPanel9.Location = new Point(PRightButton.X / 10 * 7, PRightButton.Y / 5);//help
            this.flowLayoutPanel9.Size = new Size(BWidth * 2, BHeight * 5);

   

            //底色进度条
            this.label4.Location = new Point(0, PRightButton.Y);
            this.label4.Size = new Size(PRightButton.X, 10);
            this.label5.Location = new Point(0, PRightButton.Y);
            this.label5.Size = new Size(200, 10);

            //暂停界面
            this.flowLayoutPanel10.Location = new Point(PRightButton.X / 10 * 7, PRightButton.Y / 5);
            this.flowLayoutPanel10.Size = new Size(BWidth * 2, BHeight * 5);
            this.button15.Size = new Size(BWidth, BHeight);
            this.button16.Size = new Size(BWidth, BHeight);
            this.button17.Size = new Size(BWidth, BHeight);
            this.button18.Size = new Size(BWidth, BHeight);

            //失败界面
            this.flowLayoutPanel11.Location = new Point((IWidth-BWidth)/2, PRightButton.Y / 5);
            this.flowLayoutPanel11.Size = new Size(BWidth +3, BHeight *4+11);
            this.button19.Size = new Size(BWidth, BHeight);
            this.button20.Size = new Size(BWidth, BHeight);
            this.button21.Size = new Size(BWidth, BHeight);
            this.label6.Size = new Size(BWidth,BHeight);

            //胜利界面
            this.flowLayoutPanel12.Location = new Point((IWidth - BWidth) / 2, PRightButton.Y / 5);
            this.flowLayoutPanel12.Size = new Size(BWidth + 3, BHeight * 4 + 11);
            this.button22.Size = new Size(BWidth, BHeight);
            this.button23.Size = new Size(BWidth, BHeight);
            this.button24.Size = new Size(BWidth, BHeight);
            this.label7.Size = new Size(BWidth, BHeight);

            this.label8.Size = new Size(BWidth, BHeight);
            this.label8.Location = new Point((IWidth - BWidth) / 2, (IHeight - BHeight) / 2);
        }

  


        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.LoadPicture();
            this.InitGrids();
            this.GameTimer.Start();     
        }

        private void LoadPicture()
        {
            Bitmap BmpOriginal;

            if(IGameState==0)
            if (!BUseMyPicture)//系统随机自选图片
            {
                PictureName = GetRandom.GetRandonInt(28).ToString();
                BmpOriginal = LoadBitmap.LoadBmp(PictureName);//0-29
            }
            else
                BmpOriginal = new Bitmap(StrMyPicturePath);

           else
            {
                PictureName = "BGP";
                BmpOriginal = LoadBitmap.LoadBmp(PictureName);
            }
            BmpRate = new Bitmap(BmpOriginal, new Size(IWidth, IHeight+22));//缩放成屏幕大小22
            if (IGameState == 0)
                OldBmpRate = BmpRate;
        }


        private void InitGrids()
        {
            IRowCount = Difficulty[GameLevel, 0];
            IColCount = Difficulty[GameLevel, 1];


            for (int row=0;row<IRowCount;row++)
            for(int col=0;col<IColCount;col++)
            {
                BmpGrids[col + row * IColCount] = BmpRate.Clone(new Rectangle(col * IWidth / IColCount, row * IHeight / IRowCount, IWidth / IColCount, IHeight / IRowCount), BmpRate.PixelFormat);

            }
            this.InitNewIndex();
            ALL = 5 + GameLevel * 2;
            GameLeacveSeconds=ALL;
            this.label5.BackColor = Color.Orange;
            if(IGameState==0)
                PlayMusic("Begin.mp3");
            if (IGameState == 5)
                PlayMusicWait("Wait.flac");
            if (!BGMFlag)
                axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void DrawGame(Graphics graphics)
        {

            //画网格和网格图或者  只花一张整体图
            if (IGameState == 0 || IGameState == 1 || IGameState == 2 || IGameState == 3)
            {
                for (int i = 0; i < IRowCount * IColCount; i++)
                {
                    graphics.DrawImage(BmpGrids[NewIndex[i]], PLeftTop.X + (i % IColCount) * IWidth / IColCount, PLeftTop.Y + (i / IColCount) * IHeight / IRowCount);
                }
                Pen PenWindow = new Pen(SystemColors.Window, 2);
                for (int row = 1; row < IRowCount; row++)
                {
                    graphics.DrawLine(PenWindow, PLeftTop.X, PLeftTop.Y + row * IHeight / IRowCount, PRightButton.X, PLeftTop.Y + row * IHeight / IRowCount);
                }
                for (int col = 1; col < IColCount; col++)
                {
                    graphics.DrawLine(PenWindow, PLeftTop.X + col * IWidth / IColCount, PLeftTop.Y, PLeftTop.X + col * IWidth / IColCount, PLeftTop.Y + IHeight);
                }
            }
            else
                graphics.DrawImage(BmpRate, PLeftTop.X, PLeftTop.Y);

           
            //画提示框
            if (IGameState == 1)
                if (Tip1!=-1||Tip2!=-1)
                {
                    Pen Tiphelp = new Pen(Color.Red, 3);

                    Point PtSquareTip1LT = new Point(Tip1 % IColCount * (IWidth) / IColCount, Tip1 / IColCount * (IHeight) / IRowCount);
                    Point PtSquareTip1RT = new Point(PtSquareTip1LT.X + (IWidth) / IColCount, PtSquareTip1LT.Y);
                    Point PtSquareTip1LB = new Point(PtSquareTip1LT.X, PtSquareTip1LT.Y + (IHeight) / IRowCount);
                    Point PtSquareTip1RB = new Point(PtSquareTip1RT.X, PtSquareTip1LB.Y);

                    graphics.DrawLine(Tiphelp, PtSquareTip1LT, PtSquareTip1RT);
                    graphics.DrawLine(Tiphelp, PtSquareTip1RT, PtSquareTip1RB);
                    graphics.DrawLine(Tiphelp, PtSquareTip1RB, PtSquareTip1LB);
                    graphics.DrawLine(Tiphelp, PtSquareTip1LB, PtSquareTip1LT);

                    Point PtSquareTip2LT = new Point(Tip2 % IColCount * (IWidth) / IColCount, Tip2 / IColCount * (IHeight) / IRowCount);
                    Point PtSquareTip2RT = new Point(PtSquareTip2LT.X + (IWidth) / IColCount, PtSquareTip2LT.Y);
                    Point PtSquareTip2LB = new Point(PtSquareTip2LT.X, PtSquareTip2LT.Y + (IHeight) / IRowCount);
                    Point PtSquareTip2RB = new Point(PtSquareTip2RT.X, PtSquareTip2LB.Y);

                    graphics.DrawLine(Tiphelp, PtSquareTip2LT, PtSquareTip2RT);
                    graphics.DrawLine(Tiphelp, PtSquareTip2RT, PtSquareTip2RB);
                    graphics.DrawLine(Tiphelp, PtSquareTip2RB, PtSquareTip2LB);
                    graphics.DrawLine(Tiphelp, PtSquareTip2LB, PtSquareTip2LT);
                }


            //画第一次选中
            if (IGameState == 1 || IGameState == 3)
                if (FirstSelectIndex >= 0)
                {
                    Pen PenMouseMove = new Pen(Color.DarkRed, 3);

                    Point PtSquareLT = new Point(FirstSelectIndex % IColCount * (IWidth) / IColCount, FirstSelectIndex / IColCount * (IHeight) / IRowCount);
                    Point PtSquareRT = new Point(PtSquareLT.X + (IWidth) / IColCount, PtSquareLT.Y);
                    Point PtSquareLB = new Point(PtSquareLT.X, PtSquareLT.Y + (IHeight) / IRowCount);
                    Point PtSquareRB = new Point(PtSquareRT.X, PtSquareLB.Y);

                    graphics.DrawLine(PenMouseMove, PtSquareLT, PtSquareRT);
                    graphics.DrawLine(PenMouseMove, PtSquareRT, PtSquareRB);
                    graphics.DrawLine(PenMouseMove, PtSquareRB, PtSquareLB);
                    graphics.DrawLine(PenMouseMove, PtSquareLB, PtSquareLT);
                }

            //画鼠标位置
            if (IGameState == 1)
            if (NewMouseIndex >= 0)
                {
                    Pen PenMouseMove = new Pen(COLOR_MOUSEMOVE, 3);

                    Point PtSquareLT = new Point(NewMouseIndex % IColCount * (IWidth) / IColCount, NewMouseIndex / IColCount * (IHeight) / IRowCount);
                    Point PtSquareRT = new Point(PtSquareLT.X + (IWidth) / IColCount, PtSquareLT.Y);
                    Point PtSquareLB = new Point(PtSquareLT.X, PtSquareLT.Y + (IHeight) / IRowCount);
                    Point PtSquareRB = new Point(PtSquareRT.X, PtSquareLB.Y);

                    graphics.DrawLine(PenMouseMove, PtSquareLT, PtSquareRT);
                    graphics.DrawLine(PenMouseMove, PtSquareRT, PtSquareRB);
                    graphics.DrawLine(PenMouseMove, PtSquareRB, PtSquareLB);
                    graphics.DrawLine(PenMouseMove, PtSquareLB, PtSquareLT);
                }


            if (IGameState == 2)
            {
                this.flowLayoutPanel12.Show();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.axWindowsMediaPlayer2.Ctlcontrols.pause();
            IGameState = 0;
            StrMyPicturePath = openFileDialog1.FileName;
            this.LoadPicture();
            this.InitGrids();
            this.label4.Show();//底色
            this.label5.Show();//进度条颜色
            this.GameTimer.Start();
            this.flowLayoutPanel1.Hide();
            this.Invalidate();
        }

      

        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap BufferBmp = new Bitmap(this. ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
            Graphics g = Graphics.FromImage(BufferBmp);
            
            this.DrawGame(g);

            e.Graphics.DrawImage(BufferBmp, 0, 0);
            g.Dispose();
            base.OnPaint(e);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (IGameState == 0 || IGameState == 1)
            {
                GameLeacveSeconds--;
                if((ALL - GameLeacveSeconds) * 1.0 / ALL>=0.9)
                    this.label5.BackColor = Color.Red;
                this.label5.Size = new Size(Convert.ToInt32(PRightButton.X * ((ALL - GameLeacveSeconds) * 1.0 / ALL)), 10);
            }
            if(GameLeacveSeconds==0)
            {
                if(IGameState==1)
                {
                    IGameState = 3;
                    OldMouseIndex = -1;
                    NewMouseIndex = -1;
                    FirstSelectIndex = -1;
                    SecondSelectIndex = -1;
                    this.axWindowsMediaPlayer1.Ctlcontrols.pause();
                    PlayMusic("Fail.flac");
                    if (!BPlaySound)
                    {
                        this.axWindowsMediaPlayer1.Ctlcontrols.stop();
                  
                    }
                    this.GameTimer.Stop();
                    this.flowLayoutPanel11.Show();
                    this.Invalidate();
                }
                
                if (IGameState==0)
                {
                    if (BGMFlag)
                    {
                        Play("Pause");
                    }
                    PlayMusic("BGM.flac");
                    if (!BGMFlag)
                    {
                        axWindowsMediaPlayer1.Ctlcontrols.stop();
                    }
                    IGameState = 1;
                    Tipscout = 1 + GameLevel * 2;
                    ALL = 45 + GameLevel * 65;
                    GameLeacveSeconds = ALL;
                    this.label5.BackColor = Color.Orange;
                    this.RandomNewIndex();
                    this.Invalidate();
                }
            }
        }

        private void InitNewIndex()
        {
            for (int i = 0; i < 63; i++)
                NewIndex[i] = i;
        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IGameState != 1)
                return;
            NewMouseIndex = this.GetIndex(e);
            if (NewMouseIndex != OldMouseIndex)
                this.Invalidate();
            OldMouseIndex = NewMouseIndex;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(IGameState==1)
            if(e.Button==MouseButtons.Left)
            {
                if(FirstSelectIndex>=0&&SecondSelectIndex<0)
                {
                        if (Tip2 != -1)
                        {
                            Tip1 = -1;
                            Tip2 = -1;
                        }
                    SecondSelectIndex = NewMouseIndex;
                }
                if(FirstSelectIndex<0&&SecondSelectIndex<0&&NewMouseIndex>=0)
                {
                    FirstSelectIndex = NewMouseIndex;
                    if(BPlaySound)
                    {
                        Play("FirstClick");
                    }
                }

                if(FirstSelectIndex!=SecondSelectIndex&&FirstSelectIndex>=0&&SecondSelectIndex>=0)
                {
                    int temp = NewIndex[FirstSelectIndex];
                    NewIndex[FirstSelectIndex] = NewIndex[SecondSelectIndex];
                    NewIndex[SecondSelectIndex] = temp;
                    if(BPlaySound)
                    {
                        Play("Exchange");
                    }
                    if(IsVictory())
                    {
                        IGameState = 2;
                        OldMouseIndex = -1;
                        NewMouseIndex = -1;
                        FirstSelectIndex = -1;
                        SecondSelectIndex = -1;
                        if(BPlaySound)
                        {
                            this.axWindowsMediaPlayer1.Ctlcontrols.pause();
                            PlayMusic("Victory.wav");
                        }
                        this.GameTimer.Stop();
                    }
                    FirstSelectIndex = -1;
                    SecondSelectIndex = -1;
                    this.Invalidate();
                }
                //第一二次相同
                if(FirstSelectIndex==SecondSelectIndex&&FirstSelectIndex>=0&&SecondSelectIndex>=0)
                {
                    FirstSelectIndex = -1;
                    SecondSelectIndex = -1;
                    if(BPlaySound)
                    {
                        Play("Cancel");
                    }
                    this.Invalidate();
                }
            }
        }

        private void RandomNewIndex()
        {
            int nums = IRowCount * IColCount;
            while (nums > 1)
            {
                int index = GetRandom.GetRandonInt(nums);
                int temp = NewIndex[index];
                NewIndex[index] = NewIndex[nums - 1];
                NewIndex[nums - 1] = temp;
                nums--;
            }
        }

        int GetIndex(MouseEventArgs e)
        {

            System.Console.WriteLine((e.X - PLeftTop.X) / (this.ClientRectangle.Width / IColCount) + (e.Y - PLeftTop.Y) / (this.ClientRectangle.Height / IRowCount) * IColCount);
            if (e.X >= PLeftTop.X && e.X < PLeftTop.X + this.ClientRectangle.Width && e.Y >= PLeftTop.Y && e.Y < PLeftTop.Y + this.ClientRectangle.Height)
                return (e.X - PLeftTop.X) / (this.ClientRectangle.Width / IColCount) + (e.Y - PLeftTop.Y) / (this.ClientRectangle.Height / IRowCount) * IColCount;
            return -1;
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void PlayMusic(string musicName)
        {
  
            this.axWindowsMediaPlayer1.URL = Application.StartupPath + "\\GameSounds\\" + musicName;
        }
        private void PlayMusicWait(string musicName)
        {

            this.axWindowsMediaPlayer2.URL = Application.StartupPath + "\\GameSounds\\" + musicName;
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel2.Hide();
            this.flowLayoutPanel1.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer2.Ctlcontrols.pause();
            IGameState = 0;
            this.label4.Show();//底色
            this.label5.Show();//进度条颜色
            this.flowLayoutPanel1.Hide();
            BUseMyPicture = false;
            this.LoadPicture();
            this.InitGrids();
            this.Invalidate();
            this.GameTimer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            flowLayoutPanel2.Hide();
            flowLayoutPanel3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel2.Hide();
            this.flowLayoutPanel4.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            BGMFlag = true;
            this.axWindowsMediaPlayer2.Ctlcontrols.play();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel9.Hide();
            this.flowLayoutPanel2.Show();
        }

        private void flowLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            BPlaySound = true;
        }

        private void flowLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (IGameState == 5)
            {
                this.flowLayoutPanel4.Hide();
                this.flowLayoutPanel2.Show();
            }
            else if(IGameState==4)
            {
                this.flowLayoutPanel4.Hide();
                this.flowLayoutPanel10.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel2.Hide();
            this.flowLayoutPanel9.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            GameLevel = 0;
            this.flowLayoutPanel3.Hide();
            this.flowLayoutPanel2.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            GameLevel = 1;
            this.flowLayoutPanel3.Hide();
            this.flowLayoutPanel2.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            GameLevel = 2;
            this.flowLayoutPanel3.Hide();
            this.flowLayoutPanel2.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            GameLevel = 3;
            this.flowLayoutPanel3.Hide();
            this.flowLayoutPanel2.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            GameLevel = 4;
            this.flowLayoutPanel3.Hide();
            this.flowLayoutPanel2.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Hide();
            this.flowLayoutPanel2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BUseMyPicture = true;
            this.axWindowsMediaPlayer1.Ctlcontrols.pause();
            openFileDialog1.Filter = "(*.bmp)|*.bmp|(*.jpg)|*.jpg";
            openFileDialog1.ShowDialog();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.P)
            {
               
                if (IGameState == 0 || IGameState == 1)
                {
                    OldIGameState = IGameState;
                    IGameState = 4;//变成暂停
                    this.axWindowsMediaPlayer1.Ctlcontrols.pause();
                    if(BGMFlag)
                    this.axWindowsMediaPlayer2.Ctlcontrols.play();
                    this.GameTimer.Stop();

                    //加载背景和控件
                    LoadPicture();
                    this.label4.Hide();
                    this.label5.Hide();
                    this.flowLayoutPanel10.Show();
                    this.Invalidate();
                    
                }
                else if(IGameState==4)
                {
                    this.axWindowsMediaPlayer2.Ctlcontrols.pause();
                    if(BGMFlag)
                    this.axWindowsMediaPlayer1.Ctlcontrols.play();
                    IGameState = OldIGameState;
                    this.flowLayoutPanel10.Hide();
                    this.label4.Show();
                    this.label5.Show();

                    this.Invalidate();
                    this.GameTimer.Start();
                }
            }

            if(e.KeyData==Keys.Escape)
            {
                System.Environment.Exit(0);
            }

            if(e.KeyData==Keys.Space)
            {
                e.Handled = true;
                if(IGameState==0)
                    GameLeacveSeconds = 1;
                if (IGameState == 1)
                {
                    GetHelp();
                    this.Invalidate();
                }

            }

        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel10.Hide();
            this.flowLayoutPanel4.Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer2.Ctlcontrols.pause();
            if(BGMFlag)
            this.axWindowsMediaPlayer1.Ctlcontrols.play();
            IGameState = OldIGameState;
            this.flowLayoutPanel10.Hide();
            this.label4.Show();
            this.label5.Show();
   
            this.Invalidate();
            this.GameTimer.Start();
            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer2.Ctlcontrols.pause();
            this.axWindowsMediaPlayer1.Ctlcontrols.play();
            IGameState = 0;
            this.label4.Show();//底色
            this.label5.Show();//进度条颜色
            this.flowLayoutPanel10.Hide();
            BmpRate = OldBmpRate;
            this.InitGrids();
            this.Invalidate();
            this.GameTimer.Start();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.pause();
            IGameState = 5;
            this.flowLayoutPanel10.Hide();
            this.flowLayoutPanel2.Show();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer2.Ctlcontrols.pause();
            IGameState = 0;
            this.label4.Show();//底色
            this.label5.Show();//进度条颜色
            this.flowLayoutPanel11.Hide();
            BmpRate = OldBmpRate;
            this.InitGrids();
            this.Invalidate();
            this.GameTimer.Start();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.stop();
            if(BGMFlag)
            this.axWindowsMediaPlayer2.Ctlcontrols.play();
            IGameState = 5;
            this.flowLayoutPanel11.Hide();
            this.label4.Hide();
            this.label5.Hide();
            LoadPicture();
            this.flowLayoutPanel2.Show();
            this.Invalidate();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.stop();
            if(BGMFlag)
            this.axWindowsMediaPlayer2.Ctlcontrols.play();
            this.label4.Hide();
            this.label5.Hide();
            IGameState = 5;
            this.flowLayoutPanel12.Hide();
            LoadPicture();
            this.flowLayoutPanel2.Show();
            this.Invalidate();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            
            IGameState = 0;
            this.label4.Show();//底色
            this.label5.Show();//进度条颜色
            this.flowLayoutPanel12.Hide();
            BUseMyPicture = false;
            this.LoadPicture();
            this.InitGrids();
            this.Invalidate();
            this.GameTimer.Start();
        }

        private void axWindowsMediaPlayer2_StatusChange(object sender, EventArgs e)
        {
            if ((int)this.axWindowsMediaPlayer2.playState == 1)
            {
                if (BGMFlag)
                {
                    System.Threading.Thread.Sleep(1000);
                    axWindowsMediaPlayer2.Ctlcontrols.play();
                }
            }
        }

        private void axWindowsMediaPlayer1_StatusChange(object sender, EventArgs e)
        {
            
            if((int)this.axWindowsMediaPlayer1.playState==1)
            {
                if (IGameState == 1)
                    if (BPlaySound)
                    {
                        this.axWindowsMediaPlayer1.Ctlcontrols.play();
                        System.Threading.Thread.Sleep(1000);
                    }
                }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            BPlaySound = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.BGMFlag = false;
            this.axWindowsMediaPlayer2.Ctlcontrols.pause();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label8.Hide();
            this.timer1.Stop();
        }

        private void Play(string waveName)
        {
            PlaySound(Application.StartupPath + "\\GameSounds\\" + waveName + ".wav", 0, 1);
        }

        private bool IsVictory()
        {
            for(int row=0;row<IRowCount;row++)
                for(int col=0;col<IColCount;col++)
                {
                    if (NewIndex[col + row * IColCount] != col + row * IColCount)
                        return false;
                }
            return true;
        }
        
        private void GetHelp()
        {
            this.label8.Text = "Tips: " + Tipscout;
            if (Tipscout > 0)
            {
                Tipscout--;
                FirstSelectIndex = -1;
                int i = GetRandom.GetRandonInt(IColCount * IRowCount);
                while (NewIndex[i] == i)
                {
                    i = GetRandom.GetRandonInt(IColCount * IRowCount);
                }
                Tip1 = i;
                Tip2 = NewIndex[i];
            }
            this.label8.Show();
            this.timer1.Start();
        }
    }
}
