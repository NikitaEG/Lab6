using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Figures;

namespace Figures
{
    public class Rocket : Rectangle
    {
        public List<Figure> figures;
        public Matrix matrix = new Matrix();
        public Rocket(string name, int id, int x, int y, int w, int h) : base(id, x, y, w, h)
        {
            this.name = name;
            figures = new List<Figure>();
            Rectangle r1 = new Rectangle(0, x + 25, y + 50, 50, 100);
            Square sq1 = new Square(0, x, y + 125, 25);
            Square sq2 = new Square(0, x + 75, y + 125, 25);
            Circle ci1 = new Circle(0, x + 35, y + 60, 15);
            Circle ci2 = new Circle(0, x + 35, y + 100, 15);
            Ellipse el1 = new Ellipse(0, x + 40, y + 150, 10, 25);
            Ellipse el2 = new Ellipse(0, x + 20, y + 150, 10, 25);
            Ellipse el3 = new Ellipse(0, x + 60, y + 150, 10, 25);
            Point p1 = new Point(x + 25, y + 50);
            Point p2 = new Point(x + 50, y);
            Point p3 = new Point(x + 75, y + 50);
            Point[] points = { p1, p2, p3 };
            Triangle t1 = new Triangle(0, points);
            Figure[] f_list = { r1, sq1, sq2, ci1, ci2, el1, t1, el2, el3 };
            figures.AddRange(f_list);
        }
        public override void Draw()
        {
            for (int i = 0; i < 9; i++)
            {
                Graphics g = Graphics.FromImage(Init.bitmap);
                Figure figure = figures[i];
                if (i == 7)
                {
                    matrix.RotateAt(20, new PointF(x + 30, y + 170));
                    g.Transform = matrix;
                    figure.Draw(g);
                    matrix.RotateAt(-20, new PointF(x + 30, y + 170));
                }
                else if (i == 8)
                {
                    matrix.RotateAt(-20, new PointF(x + 70, y + 170));
                    g.Transform = matrix;
                    figure.Draw(g);
                    matrix.RotateAt(20, new PointF(x + 70, y + 170));
                }
                else
                {
                    figure.Draw();
                }

            }
        }
        public override void NewColor(int r, int g, int b)
        {
            for (int i = 0; i < 9; i++)
            {
                Figure figure = figures[i];
                figure.ColorChange(r, g, b);
            }
            this.DeleteF(this, false);
            this.Draw();
        }
        public override void MoveTo(int x, int y)
        {
            Init.Clear();
            this.x += x;
            this.y += y;
            for (int i = 0; i < 9; i++)
            {
                Figure figure = figures[i];
                if (figure.ToString() == "Figures.Triangle")
                {
                    for (int j = 0; j < figure.points.Length; j++)
                    {
                        figure.points[j].X += x;
                        figure.points[j].Y += y;
                    }
                }
                else
                {
                    figure.x += x;
                    figure.y += y;
                }
            }
            Draw();
            foreach (Figure f in ShapeContainer.figureList)
            {
                f.Draw();
            }
        }
    }
}
