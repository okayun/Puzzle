using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
//using System.Collections.Generic;

class Program
{

    [STAThread]
    public static void Main()
    {
        new SlidePuzzle();
    }
}

class SlidePuzzle
{
    Form form;
    PictureBox[,] pics;
    int height, width; // 画像 1 枚あたりの縦, 横の長さ
    int H, W; // H × W 枚の画像
    int[] vx = { -1, 0, 1, 0 };
    int[] vy = { 0, -1, 0, 1 };

    public SlidePuzzle()
    {
        form = new Form();
        form.Size = new Size(500, 500);
        form.Text = "Puzzle";
        form.KeyPreview = true;

        H = W = 3;

        pics = new PictureBox[H, W];

        for (int i = 0; i < H; ++i)
        {
            int lx = 0, ly = i * 100;

            for (int j = 0; j < W; ++j)
            {
                pics[i, j] = new PictureBox();

                if (i == 2 && j == 2)
                {
                    // ここ相対パスにしたいけどなんかエラー出る
                    pics[i, j].Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\blank.png");
                    pics[i, j].Text = "blank";
                }
                else
                {
                    // ここも
                    //pics[i, j].Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\1-" + (i + 1) + "-" + (j + 1) + ".png");
                    pics[i, j].Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\m" + i + "" + j + ".png");
                    //String filename = ".\\img\\1-" + (i + 1) + "-" + (j + 1) + ".png";
                    //pics[i, j].Image = Image.FromFile(filename);
                }

                pics[i, j].MouseClick += new MouseEventHandler(PicMove);
                pics[i, j].Location = new Point(lx, ly);
                pics[i, j].Size = new Size(100, 100);
                lx += pics[i, j].Width;
                pics[i, j].Parent = form;
            }
        }

        height = pics[0, 0].Height;
        width = pics[0, 0].Width;

        Application.Run(form);
    }

    void PicMove(object sender, MouseEventArgs e)
    {
        if (e == null)
        {
            throw new ArgumentNullException(nameof(e));
        }

        Point cp = form.PointToClient(Cursor.Position);

        for (int i = 0; i < H; ++i)
        {
            for (int j = 0; j < W; ++j)
            {
                if (pics[i, j].Left <= cp.X && cp.X < pics[i, j].Left + width && pics[i, j].Top <= cp.Y && cp.Y < pics[i, j].Top + height)
                {
                    for (int m = 0; m < H; ++m)
                    {
                        for (int n = 0; n < W; ++n)
                        {
                            if (i == m && j == n)
                            {
                                continue;
                            }

                            for (int k = 0; k < 4; ++k)
                            {
                                int x = pics[i, j].Left + vx[k] * width;
                                int y = pics[i, j].Top + vy[k] * height;

                                if (0 <= x && x < height * H && 0 <= y && y < width * W)
                                {
                                    if (pics[m, n].Left == x && pics[m, n].Top == y && pics[m, n].Text == "blank")
                                    {
                                        Point tmp = pics[i, j].Location;
                                        pics[i, j].Location = pics[m, n].Location;
                                        pics[m, n].Location = tmp;
                                        i = j = m = n = 2;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }
}
