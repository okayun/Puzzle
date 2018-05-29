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
    int height, width;
    int[] vx = { -1, 0, 1, 0 };
    int[] vy = { 0, -1, 0, 1 };

    public SlidePuzzle()
    {
        form = new Form();
        form.Size = new Size(500, 500);
        form.Text = "Puzzle";
        form.KeyPreview = true;

        pics = new PictureBox[3, 3];

        for (int i = 0; i < 3; ++i)
        {
            int lx = 0, ly = i * 111;

            for (int j = 0; j < 3; ++j)
            {
                pics[i, j] = new PictureBox();

                if (i == 2 && j == 2)
                {
                    pics[i, j].Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\blank.png");
                    pics[i, j].Text = "blank";
                }
                else
                {
                    pics[i, j].Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\1-" + (i + 1) + "-" + (j + 1) + ".png");
                }

                pics[i, j].MouseClick += new MouseEventHandler(PicMove);
                pics[i, j].Location = new Point(lx, ly);
                pics[i, j].Size = new Size(111, 111);
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

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (pics[i, j].Left <= cp.X && cp.X < pics[i, j].Left + width && pics[i, j].Top <= cp.Y && cp.Y < pics[i, j].Top + height)
                {
                    for (int m = 0; m < 3; ++m)
                    {
                        for (int n = 0; n < 3; ++n)
                        {
                            if (i == m && j == n)
                            {
                                continue;
                            }

                            for (int k = 0; k < 4; ++k)
                            {
                                int x = pics[i, j].Left + vx[k] * width;
                                int y = pics[i, j].Top + vy[k] * height;

                                if (0 <= x && x < height * 3 && 0 <= y && y < width * 3)
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
