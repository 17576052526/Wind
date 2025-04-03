﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class VerifyCode
    {
        /// <summary>
        /// 根据字符串生成验证码图片（字节）
        /// </summary>
        public static byte[] CreateImage(string TestStr)
        {
            Bitmap image = new Bitmap(TestStr.Length * 18, 25);  //设置生成图片的高度和宽度
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);   //白色填充
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体 
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();
            //随机输出噪点
            //for (int i = 0; i < 50; i++)
            //{
            //    int x = rand.Next(image.Width);
            //    int y = rand.Next(image.Height);
            //    g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            //}
            //画噪线
            for (int i = 0; i < 4; i++)    //每次循环画一条线   x轴和Y轴指定一个点，两个点指定一条线
            {
                //随机生成噪线的起点和终点
                int x1 = rand.Next(100);
                int y1 = rand.Next(30);
                int x2 = rand.Next(100);
                int y2 = rand.Next(30);
                Color col = c[rand.Next(c.Length)];   //获取颜色
                g.DrawLine(new Pen(col), x1, y1, x2, y2);
            }
            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < TestStr.Length; i++)
            {
                string fon = font[rand.Next(font.Length)];
                Color col = c[rand.Next(c.Length)];

                Font f = new Font(fon, 13, FontStyle.Italic);  //定义字体，大写，加粗
                Brush b = new SolidBrush(col);  //字体颜色
                int x = 0 + (i * 15);
                int y = i % 2 == 0 ? 2 : 0;
                g.DrawString(TestStr.Substring(i, 1), f, b, x, y);  //指定的字符绘制到指定的位置
            }

            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            g.Dispose();
            image.Dispose();

            return ms.ToArray();
        }

        /// <summary>
        /// 生成随机字符串， size：字符串的数量
        /// </summary>
        public static string RandomStr(int size)
        {
            char[] charArr = { '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rand = new Random();
            string str = "";
            for (int i = 0; i < size; i++) { str += charArr[rand.Next(charArr.Length)]; }
            return str;
        }
    }
}
