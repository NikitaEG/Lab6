﻿using System.Drawing;

namespace Figures
{
    abstract public class Figure
    {
        public int x;
        public int y;
        public int w;
        public int h;
        public int id;
        public string name;
        public Point[] points;
        public Color color = Color.Black;
        public Figure(int id, int x, int y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.name = "Фигура";
        }
        abstract public void Draw();
        abstract public void Draw(Graphics g);
        virtual public void MoveTo(int x, int y)
        {
            this.x += x;
            this.y += y;
            this.DeleteF(this, false);
            this.Draw();
        }
        public void DeleteF(Figure figure, bool flag = true)
        {
            Graphics g = Graphics.FromImage(Init.bitmap);
            ShapeContainer.figureList.Remove(figure);
            Init.Clear();
            Init.pictureBox.Image = Init.bitmap;
            foreach (Figure f in ShapeContainer.figureList)
            {
                f.Draw();
            }
            if (flag == false)
            {
                ShapeContainer.figureList.Add(figure);
            }
        }
        public virtual void NewColor(int r, int g, int b)
        {
            this.DeleteF(this, false);
            this.color = Color.FromArgb(r, g, b);
            Init.pen = new Pen(this.color, 3);
            this.Draw();
        }
        public virtual void ColorChange(int r, int g, int b)
        {
            this.color = Color.FromArgb(r, g, b);
        }
        virtual public string Name
        {
            get
            {
                return this.name + " " + this.id.ToString();
            }
        }
    }
}
