using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

class Program
{

    [STAThread]
    public static void Main()
    {
        while (true)
        {
            new SlidePuzzle();
            new Conguraturation();
            // 終わるかどうか聞くやつを実装したい
        }
    }
}

class Answer
{
    Form form;
    PictureBox pic;

    public Answer()
    {
        form = new Form();
        pic = new PictureBox();
        pic.Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\fx_cat.png");
        pic.Size = new Size(pic.Image.Width, pic.Image.Height);
        form.Size = new Size(pic.Image.Width, pic.Image.Height);
        pic.Parent = form;

        form.ShowDialog();
    }
}

class Conguraturation
{
    Form form;
    System.Random rnd;
    PictureBox image, answer;

    public Conguraturation()
    {
        form = new Form();
        rnd = new System.Random();
        image = new PictureBox();
        answer = new PictureBox();
        // ぬくもりィ！！！
        int k = 0;

        double prob = rnd.NextDouble();

        if (prob < 0.1)
        {
            // ぬくもりィ！！！
            k = 500;
            image.Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\graduate.png");
        }
        else if (0.1 < prob && prob < 0.4)
        {
            // ぬくもりィ！！！
            k = 480;
            image.Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\hanataba.png");
        }
        else
        {
            // ぬくもりィ！！！
            k = 570;
            image.Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\conguraturation.png");
        }

        answer.Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\fx_cat.png");

        image.Parent = form;
        image.Size = new Size(image.Image.Width, image.Image.Height);
        // ぬくもりィ！！！
        image.Location = new Point((k - image.Image.Width), 0);
        form.Text = "おめでとう！！";

        answer.Parent = form;
        answer.Size = new Size(answer.Image.Width, answer.Image.Height);
        // ぬくもりィ！！！
        answer.Location = new Point((585 - answer.Width) / 2, image.Image.Height);

        // ぬくもりィ！！！
        form.Size = new Size(600, image.Height + answer.Height + 39);

        form.ShowDialog();
    }
}

class SlidePuzzle
{
    Form form;
    PictureBox[,] pics;
    KeyValuePair<int, Image>[] rndpics; // ペア
    //Image[] imgs; // 初期盤面をシャッフルする用
    System.Random rnd;

    int height, width; // 画像 1 枚あたりの縦, 横の長さ
    int H, W; // H × W 枚の画像
    int[] vx = { -1, 0, 1, 0 };
    int[] vy = { 0, -1, 0, 1 };

    public SlidePuzzle()
    {
        form = new Form();
        // ぬくもりィ！！！
        form.Size = new Size(565, 589);
        form.Text = "Puzzle - FX で金を溶かした猫 -";
        form.KeyPreview = true;

        // ぬくもりィ！！！
        H = W = 5;

        pics = new PictureBox[H, W];
        rndpics = new KeyValuePair<int, Image>[H * W - 1];
        int idx = 0;

        for (int i = 0; i < H; ++i)
        {
            // ぬくもりィ！！！
            int lx = 0, ly = i * 110;

            for (int j = 0; j < W; ++j)
            {
                pics[i, j] = new PictureBox();

                if (i == H - 1 && j == W - 1)
                {
                    // ここ相対パスにしたいけどなんかエラー出る
                    /*pics[i, j].Image*/
                    pics[i, j].Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\blank.png");
                    pics[i, j].Text = idx.ToString();
                }
                else
                {
                    // ここも
                    //pics[i, j].Image = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\1-" + (i + 1) + "-" + (j + 1) + ".png");
                    /*pics[i, j].Image*/
                    //rndpics[idx].Value = Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\muramatsu_puzzle_" + (i + 1) + "_" + (j + 1) + ".png");
                    //rndpics[idx].Key = idx++;
                    rndpics[idx] = new KeyValuePair<int, Image>(idx, Image.FromFile(@"C:\Users\yamaoka\source\repos\Puzzle\img\fx_cat_" + (i + 1) + "_" + (j + 1) + ".png"));
                    idx++;
                    //String filename = ".\\img\\1-" + (i + 1) + "-" + (j + 1) + ".png";
                    //pics[i, j].Image = Image.FromFile(filename);
                }

                pics[i, j].MouseClick += new MouseEventHandler(PicMove);
                pics[i, j].Location = new Point(lx, ly);
                // ぬくもりィ！！！
                pics[i, j].Size = new Size(110, 110);
                lx += pics[i, j].Width;
                pics[i, j].Parent = form;
            }
        }

        // 盤面をシャッフル
        rndpics = shuffle(rndpics);

        // 解けない盤面なら 1 箇所 SWAP する
        if (!Check(rndpics))
        {
            Swap<KeyValuePair<int, Image>>(ref rndpics[0], ref rndpics[1]);
        }

        idx = 0;

        for (int i = 0; i < H; ++i)
        {
            for (int j = 0; j < W; ++j)
            {
                if (i == H - 1 && j == W - 1) continue;
                pics[i, j].Image = rndpics[idx].Value;
                pics[i, j].Text = rndpics[idx++].Key.ToString();
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

        // どの画像がクリックされたのか調べる
        for (int i = 0; i < H; ++i)
        {
            for (int j = 0; j < W; ++j)
            {
                // 見つけた場合
                if (pics[i, j].Left <= cp.X && cp.X < pics[i, j].Left + width && pics[i, j].Top <= cp.Y && cp.Y < pics[i, j].Top + height)
                {
                    // 上下左右のマスに空白のマスがあるかどうかを調べる
                    for (int m = 0; m < H; ++m)
                    {
                        for (int n = 0; n < W; ++n)
                        {
                            if (i == m && j == n)
                            {
                                continue;
                            }

                            // (m, n) のマスが (i, j) のマスに隣接しているかどうかを確かめる
                            // 上下左右方向のマスを調べる
                            for (int k = 0; k < 4; ++k)
                            {
                                int x = pics[i, j].Left + vx[k] * width;
                                int y = pics[i, j].Top + vy[k] * height;

                                if (0 <= x && x < height * H && 0 <= y && y < width * W)
                                {
                                    //Console.WriteLine((H * W - 1).ToString());
                                    if (pics[m, n].Left == x && pics[m, n].Top == y)
                                    {
                                        /*
                                        Console.WriteLine("check");
                                        Console.WriteLine("coodinate = " + pics[m, n].Left + " " + pics[m, n].Top);
                                        Console.WriteLine("text = " + pics[m, n].Text);
                                        Console.WriteLine("id = " + (H * W - 1).ToString());
                                        Console.WriteLine("is equal = " + (pics[m, n].Text == (H * W - 1).ToString()));
                                        */
                                        if (pics[m, n].Text == (H * W - 1).ToString())
                                        {
                                            /*
                                            Point tmp = pics[i, j].Location;
                                            pics[i, j].Location = pics[m, n].Location;
                                            pics[m, n].Location = tmp;
                                            */
                                            Image tmp = pics[i, j].Image;
                                            pics[i, j].Image = pics[m, n].Image;
                                            pics[m, n].Image = tmp;

                                            String tmps = pics[i, j].Text;
                                            pics[i, j].Text = pics[m, n].Text;
                                            pics[m, n].Text = tmps;
                                            pics[i, j].Invalidate();
                                            pics[m, n].Invalidate();
                                            i = j = m = n = H;
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

        // 解けていたら終了
        if (IsSolved())
        {
            form.Visible = false;
            form.Close();
        }
    }

    bool IsSolved()
    {
        int idx = 0;
        for (int i = 0; i < H; ++i)
        {
            for (int j = 0; j < W; ++j)
            {
                if (pics[i, j].Text != idx.ToString())
                {
                    return false;
                }
                idx++;
            }
        }
        return true;
    }

    void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    // シャッフル
    KeyValuePair<int, Image>[] shuffle(KeyValuePair<int, Image>[] pics)
    {
        rnd = new System.Random();
        int n = pics.Length;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            KeyValuePair<int, Image> tmp = pics[k];
            pics[k] = pics[n];
            pics[n] = tmp;
        }

        return pics;
    }

    // 転倒数を計算
    // 偶数だったら解けるので true
    // そうでないとき false
    bool Check(KeyValuePair<int, Image>[] pics)
    {
        int inversion = 0;

        for (int i = 1; i < H * W - 1; ++i)
        {
            for (int j = 0; j < i; ++j)
            {
                if (pics[i].Key < pics[j].Key)
                {
                    inversion++;
                }
            }
        }

        return (inversion % 2 == 0);
    }
}
