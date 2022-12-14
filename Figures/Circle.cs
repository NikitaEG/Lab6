using System.Drawing;

namespace Figures
{
    public class Circle : Ellipse
    {
        public Circle(int id, int x, int y, int r) : base(id, x, y, r, r)
        {
            this.name = "Окружность";
        }
        public Circle(string name, int id, int x, int y, int r) : base(id, x, y, r, r)
        {
            this.name = name;
        }
        public void NewColor(int r, int g, int b)
        {
            this.DeleteF(this, false);
            this.color = Color.FromArgb(r, g, b);
            Init.pen = new Pen(this.color, 3);
            this.Draw();
            Init.pen = new Pen(Color.Black, 3);
        }
    }
}
