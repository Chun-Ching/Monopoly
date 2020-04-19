using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 期末專題報告
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int player = 0;                                       //玩家
        int point = 0;                                        //骰子點數
        bool d_e_game = false;                                //是否結束遊戲
        Random number = new Random();
        int[] money = new int[4];                             //存放玩家金額
        int[] lcn = new int[4];                               //紀錄玩家位置
        int[] pass_id = {1, 1, 1, 1 };                        //紀錄玩家抵達哪一行
        int[] land = new int[20];                             //存放此地，由哪位玩家購買

        //將標籤陣列化 ; 存放4種顏色
        Label[] crtl_lab = new Label[20];
        Color[] back_color = { Color.FromArgb(255, 192, 203), Color.FromArgb(122, 184, 204),
            Color.FromArgb(204, 163, 204), Color.FromArgb(255, 227, 132)};

        private void re_start()//重置
        {
            player = 1; point = 0; d_e_game = false;
            //重製所有陣列值
            for (int index = 0; index <= 3; index++)  money.SetValue(10000, index);
            for (int index = 0; index <= 3; index++) pass_id.SetValue(1, index);
            Array.Clear(lcn, 0, lcn.Length);
            Array.Clear(land, 0, land.Length);

            //將玩家label的location設為起點-六宿
            lab_p_1.Top = 520; lab_p_1.Left = 510;
            lab_p_2.Top = 520; lab_p_2.Left = 557;
            
            lab_player.Text = "下一位玩家 : " + player;
            crtl_lab[0] = label11; crtl_lab[1] = label12; crtl_lab[2] = label13; crtl_lab[3] = label14;
            crtl_lab[4] = label15; crtl_lab[5] = label16; crtl_lab[6] = label17; crtl_lab[7] = label18;
            crtl_lab[8] = label19; crtl_lab[9] = label20; crtl_lab[10] = label1; crtl_lab[11] = label2;
            crtl_lab[12] = label3; crtl_lab[13] = label4; crtl_lab[14] = label5; crtl_lab[15] = label6;
            crtl_lab[16] = label7; crtl_lab[17] = label8; crtl_lab[18] = label9; crtl_lab[19] = label10;

            for(int cc = 0; cc <= 19; cc++)
            {
                if (cc == 0 || cc == 5 || cc == 10 || cc == 15) crtl_lab[cc].BackColor = Color.FromArgb(192, 255, 192);
                else crtl_lab[cc].BackColor = Color.FromArgb(224, 224, 224);
            }
            show_money();
        }

        private void show_money()//顯示目前玩家金額
        {
            textBox1.Text = money[0].ToString();
            textBox2.Text = money[1].ToString();
            
        }

        private void end_game()//如有玩家金額 <= 0，結束
        {
            bool zero = false;
            int max = 0; int winer = 0;

            if (d_e_game == false)
            {       
                foreach (int i in money)
                {
                    if (i <= 0)
                    {
                        zero = true;
                        break;
                    }
                }
                if (zero == true)
                {
                    max = money[0];
                    for (int j = 0; j <= 1; j++)
                    {
                        if (max < money[j])
                        {
                            max = money[j];
                            winer = j;
                        }
                    }
                    MessageBox.Show("Winer : 第" + (winer + 1) + "號玩家", "End Game");
                    re_start();
                }             
            }
            else
            {
                max = money[0];
                for (int j = 0; j <= 1; j++)
                {
                    if (max < money[j])
                    {
                        max = money[j];
                        winer = j;
                    }
                }
                MessageBox.Show("Winer : 第" + (winer + 1) + "號玩家", "End Game");
                re_start();
            }
        }

        private void purchase()//買土地，LL是第幾格
        {
            if (lcn[player - 1] != 0 && lcn[player - 1] != 5 && lcn[player - 1] != 10 && lcn[player - 1] != 15)//起點不能買
            {
                if (land[lcn[player - 1]] == player)
                {
                    money[player - 1] += 100;
                    show_money();
                }
                else
                {
                    if (lab_player.Text == "下一位玩家 : " + 1) //玩家
                    {
                        if (land[lcn[player - 1]] == 0)//表示此地無人購買
                        {
                            DialogResult msg_buy = MessageBox.Show("你要購買此地嗎?\n將花費1000元喔!", "購買", MessageBoxButtons.YesNo);
                            if (msg_buy == DialogResult.Yes)//選擇要買，一塊地1000元
                            {
                                crtl_lab[lcn[player - 1]].BackColor = back_color[player - 1];
                                money[player - 1] -= 1000;
                                land[lcn[player - 1]] = player;
                                show_money(); end_game();
                            }
                        }
                        else//判斷踩到哪位玩家的土地，扣2倍的錢
                        {
                            MessageBox.Show("你踩到電腦玩家的土地!\n將從您的金額扣2000元");
                            money[player - 1] -= 2000;
                            money[land[lcn[player - 1]] - 1] += 2000;
                            show_money(); end_game();
                        }
                    }
                    else //電腦
                    {
                        if (land[lcn[player - 1]] == 0)//表示此地無人購買
                        {

                            crtl_lab[lcn[player - 1]].BackColor = back_color[player - 1];
                            money[player - 1] -= 1000;
                            land[lcn[player - 1]] = player;
                            show_money(); end_game();

                        }
                        else//判斷踩到哪位玩家的土地，扣2倍的錢
                        {
                            MessageBox.Show("電腦玩家採到你的土地!\n將給您2000元");
                            money[player - 1] -= 2000;
                            money[land[lcn[player - 1]] - 1] += 2000;
                            show_money(); end_game();
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //遊戲開始
            button2.Enabled = false;
            button1.Enabled = true;
            button3.Enabled = true;
            re_start();

         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //遊戲結束
            d_e_game = true;
            button2.Enabled = true;
            button1.Enabled = false;
            button3.Enabled = false;
            end_game();
            //強制結束
            /*
            int max = money[0]; int temp = 0; 
            for(int j = 0; j <= 3; j++)
            {
                if(max < money[j])
                {
                    max = money[j];
                    temp = j;
                }
            }
            MessageBox.Show("Winer是第 " + (temp+1) + " 號玩家", "WIN", MessageBoxButtons.OK);*/
            re_start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //骰子點數
            point = number.Next(1, 7);
            label26.Text = point.ToString();

            //紀錄玩家位置
            if (lcn[player - 1] + point > 19) lcn[player - 1] = lcn[player - 1] + point - 19 - 1;
            else lcn[player - 1] += point;
          
            timer1.Enabled = true;
            button1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            //玩家移動
            if (point <= 0)
            {        
                button1.Enabled = true;
                label26.Text = "";
                timer1.Enabled = false;    
                
                purchase();

                //換另一位玩家
                if (player >= 2)
                {
                    player = 1;
                }
                else
                {
                    player += 1;
                    button1.PerformClick();
                    
                    
                }

                lab_player.Text = "下一位玩家 : " + player;
            }
            else
            {
                switch (player)//玩家
                {
                    case 1:
                        switch (pass_id[player - 1])
                        {
                            case 1:
                                if ((lab_p_1.Left - 100) < 0) pass_id[player - 1] = 2;
                                else
                                {
                                    lab_p_1.Left -= 100;
                                    point--;
                                }
                                break;
                            case 2:
                                if ((lab_p_1.Top - 100) < 0) pass_id[player - 1] = 3;
                                else
                                {
                                    lab_p_1.Top -= 100;
                                    point--;
                                }
                                break;
                            case 3:
                                if ((lab_p_1.Left + 100) > 600) pass_id[player - 1] = 4;
                                else
                                {
                                    lab_p_1.Left += 100;
                                    point--;
                                }
                                break;
                            case 4:
                                if ((lab_p_1.Top + 100) > 600) pass_id[player - 1] = 1;
                                else
                                {
                                    lab_p_1.Top += 100;
                                    point--;
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (pass_id[player - 1])//通行號碼
                        {
                            case 1:
                                if ((lab_p_2.Left - 100) < 0) pass_id[player - 1] = 2;
                                else
                                {
                                    lab_p_2.Left -= 100;
                                    point--;
                                }
                                break;
                            case 2:
                                if ((lab_p_2.Top - 100) < 0) pass_id[player - 1] = 3;
                                else
                                {
                                    lab_p_2.Top -= 100;
                                    point--;
                                }
                                break;
                            case 3:
                                if ((lab_p_2.Left + 100) > 600) pass_id[player - 1] = 4;
                                else
                                {
                                    lab_p_2.Left += 100;
                                    point--;
                                }
                                break;
                            case 4:
                                if ((lab_p_2.Top + 100) > 600) pass_id[player - 1] = 1;
                                else
                                {
                                    lab_p_2.Top += 100;
                                    point--;
                                }
                                break;
                        }
                        break;
                                 
                }



                
            }

            
            

            }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //標題跑馬燈
            label28.Text = label28.Text.Substring(1) + label28.Text.Substring(0, 1);
        }
    }
}
