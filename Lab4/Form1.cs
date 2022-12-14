using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Figures;

namespace Lab4
{
    public partial class Form1 : Form
    {
        private Stack<Operator> operators = new Stack<Operator>();
        private Stack<Operand> operands = new Stack<Operand>();

        int ci_count = 0;
        int rocket_count = 0;
        public Form1()
        {
            InitializeComponent();
            Init.bitmap = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            Init.pen = new Pen(Color.Black, 3);
            Init.pictureBox = pictureBox1;
            ShapeContainer figure_list = new ShapeContainer();
        }

        private void textBoxInputString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                operators.Clear();
                operands.Clear();
                try
                {
                    string sourceExpression = textBoxInputString.Text.Replace(" ", "");
                    for (int i = 0; i < sourceExpression.Length; i++)
                    {
                        char c = sourceExpression[i];
                        if (IsNotOperation(c))
                        {
                            if (!Char.IsDigit(c))
                            {
                                operands.Push(new Operand(c));
                                while (i < sourceExpression.Length - 1 && IsNotOperation(sourceExpression[i + 1]))
                                {
                                    string temp_str = operands.Pop().value.ToString() + sourceExpression[i + 1].ToString();
                                    operands.Push(new Operand(temp_str));
                                    i++;
                                }
                            }
                            else if (Char.IsDigit(c))
                            {
                                operands.Push(new Operand(c.ToString()));
                                while (i < sourceExpression.Length - 1 && Char.IsDigit(sourceExpression[i + 1])
                                    && IsNotOperation(sourceExpression[i + 1]))
                                {
                                    int temp_num = Convert.ToInt32(operands.Pop().value.ToString()) * 10 +
                                        (int)Char.GetNumericValue(sourceExpression[i + 1]);
                                    operands.Push(new Operand(temp_num.ToString()));
                                    i++;
                                }
                            }
                        }

                        else if ((c == 'C') || (c == 'M') || (c == 'D') || (c == 'N') || (c == 'R'))
                        {
                            if (operators.Count == 0)
                            {
                                operators.Push(OperatorContainer.FindOperator(c));
                            }
                        }
                        else if (sourceExpression[i] == '(')
                        {
                            operators.Push(OperatorContainer.FindOperator(sourceExpression[i]));
                        }
                        else if (c == ')')
                        {
                            do
                            {
                                if (operators.Peek().symbolOperator == '(')
                                {
                                    operators.Pop();
                                    break;
                                }
                                if (operators.Count == 0)
                                {
                                    break;
                                }
                            }
                            while (operators.Peek().symbolOperator != '(');
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Аргументы введены некорректно.");
                    comboBox1.Items.Add("Аргументы введены некорректно.");
                }
                try
                {
                    SelectingPerformingOperation(operators.Peek());
                }
                catch
                {
                    MessageBox.Show("Введенной операции не существует.");
                    comboBox1.Items.Add("Введенной операции не существует.");
                }
            }
        }
        private void SelectingPerformingOperation(Operator op)
        {
            if (op.symbolOperator == 'C')
            {
                if (operands.Count == 4)
                {
                    ci_count += 1;
                    int r = Convert.ToInt32(operands.Pop().value.ToString());
                    int y = Convert.ToInt32(operands.Pop().value.ToString());
                    int x = Convert.ToInt32(operands.Pop().value.ToString());
                    string name = operands.Pop().value.ToString();
                    if (Init.Coords_check(x, y, r * 2, r * 2))
                    {
                        Circle circle = new Circle(name, ci_count, x, y, r);
                        op = new Operator(circle.Draw, 'C');
                        ShapeContainer.AddFigure(circle);
                        comboBox1.Items.Add($"Круг {circle.name} отрисован.");
                        op.operatorMethod();
                    }
                    else
                    {
                        MessageBox.Show("Фигура вышла за границы.");
                        comboBox1.Items.Add("Фигура вышла за границы.");
                    }
                }
                else
                {
                    MessageBox.Show("Опертор C принимает 4 аргумента.");
                    comboBox1.Items.Add("Неверное число аргументов для оператора C.");
                }
            }
            else if (op.symbolOperator == 'R')
            {
                if (operands.Count == 3)
                {
                    ci_count += 1;
                    int y = Convert.ToInt32(operands.Pop().value.ToString());
                    int x = Convert.ToInt32(operands.Pop().value.ToString());
                    string name = operands.Pop().value.ToString();
                    if (Init.Coords_check(x, y, 100, 200))
                    {
                        Rocket rocket = new Rocket(name, rocket_count, x, y, 100, 200);
                        op = new Operator(rocket.Draw, 'R');
                        ShapeContainer.AddFigure(rocket);
                        comboBox1.Items.Add($"Ракета {rocket.name} отрисована.");
                        op.operatorMethod();
                    }
                    else
                    {
                        MessageBox.Show("Фигура вышла за границы.");
                        comboBox1.Items.Add("Фигура вышла за границы.");
                    }
                }
                else
                {
                    MessageBox.Show("Опертор R принимает 3 аргумента.");
                    comboBox1.Items.Add("Неверное число аргументов для оператора R.");
                }
            }
            else if (op.symbolOperator == 'N')
            {
                if (operands.Count == 4)
                {
                    Figure figure = null;
                    int b = Convert.ToInt32(operands.Pop().value.ToString());
                    int g = Convert.ToInt32(operands.Pop().value.ToString());
                    int r = Convert.ToInt32(operands.Pop().value.ToString());
                    string name = operands.Pop().value.ToString();
                    foreach (Figure f in ShapeContainer.figureList)
                    {
                        if (f.name == name)
                        {
                            figure = f;
                        }
                    }
                    if (figure != null)
                    {
                        figure.NewColor(r, g, b);
                        comboBox1.Items.Add($"Фигура {figure.name} успешно перекрашена.");
                    }
                    else
                    {
                        comboBox1.Items.Add($"Фигуры {name} не существует.");
                    }
                }
                else
                {
                    MessageBox.Show("Опертор N принимает 4 аргумента.");
                    comboBox1.Items.Add("Неверное число аргументов для оператора N.");
                }
            }
            else if (op.symbolOperator == 'M')
            {
                if (operands.Count == 3)
                {
                    Figure figure = null;
                    int y = Convert.ToInt32(operands.Pop().value.ToString());
                    int x = Convert.ToInt32(operands.Pop().value.ToString());
                    string name = operands.Pop().value.ToString();
                    foreach (Figure f in ShapeContainer.figureList)
                    {
                        if(f.name == name)
                        {
                            figure = f;
                        }
                    }
                    if (figure != null)
                    {
                        if (Init.Coords_check(figure.x + x, figure.y + y, figure.w, figure.h))
                        {
                            figure.MoveTo(x, y);
                            comboBox1.Items.Add($"Фигура {figure.name} успешно перемещена.");
                        }
                        else
                        {
                            MessageBox.Show("Фигура вышла за границы.");
                            comboBox1.Items.Add("Фигура вышла за границы.");
                        }
                    }
                    else
                    {
                        comboBox1.Items.Add($"Фигуры {name} не существует");
                    }
                }
                else
                {
                    MessageBox.Show("Опертор M принимает 3 аргумента.");
                    comboBox1.Items.Add("Неверное число аргументов для оператора M.");
                }
            }
            else if (op.symbolOperator == 'D')
            {
                if (operands.Count == 1)
                {
                    Figure figure = null;
                    string name = operands.Pop().value.ToString();
                    foreach (Figure f in ShapeContainer.figureList)
                    {
                        if (f.name == name)
                        {
                            figure = f;
                        }
                    }
                    if (figure != null)
                    {
                        figure.DeleteF(figure, true);
                        comboBox1.Items.Add($"Фигура {figure.name} удалена.");
                    }
                    else
                    {
                        comboBox1.Items.Add($"Фигуры {name} не существует.");
                    }
                }
                else
                {
                    MessageBox.Show("Опертор D принимает 1 аргумент.");
                    comboBox1.Items.Add("Неверное число аргументов для оператора D.");
                }
            }
        }
        private bool IsNotOperation(char item)
        {
            if (!(item == 'R' || item == 'C' || item == 'N' || item == 'M' || item == 'D' || item == ',' || item == '(' || item == ')'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
