using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MyLibrary;

namespace MethodsLibrary
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            foreach (Control control in Controls)
            {
                if (control is GroupBox groupBox)
                    foreach (Control groupControl in groupBox.Controls)
                        if (groupControl is RadioButton radioButton)
                            radioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
                else if (control is RadioButton otherRadioButton)
                    otherRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            }
        }

        private void UncheckOtherRadioButtons(RadioButton selectedRadioButton)
        {
            foreach (Control control in Controls)
            {
                if (control is GroupBox groupBox)
                    foreach (Control groupControl in groupBox.Controls)
                        if (groupControl is RadioButton radioButton && radioButton != selectedRadioButton)
                            radioButton.Checked = false;
                else if (control is RadioButton otherRadioButton && otherRadioButton != selectedRadioButton)
                    otherRadioButton.Checked = false;
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selectedRadioButton = (RadioButton)sender;
            UncheckOtherRadioButtons(selectedRadioButton);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] a = new double[dataGridView1.Rows.Count - 1];
            double[] b = new double[dataGridView1.Rows.Count - 1];
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    a[i] = Convert.ToDouble(dataGridView1[1, i].Value);
                    b[i] = Convert.ToDouble(dataGridView1[2, i].Value);
                }

            double h = double.Parse(textBox1.Text);

            chart1.Series.Clear();
            chart1.Series.Add("График заданной функции: cos(x)/x");
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.ChartAreas[0].AxisX.Maximum = b[0];
            chart1.ChartAreas[0].AxisX.Minimum = a[0];

            for (double x = a[0]; x <= b[0]; x += h)
                chart1.Series[0].Points.AddXY(x, Solve.Func(x));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double[] x0 = new double[dataGridView1.Rows.Count - 1];
            double[] a = new double[dataGridView1.Rows.Count - 1];
            double[] b = new double[dataGridView1.Rows.Count - 1];
            double h = double.Parse(textBox1.Text);
            int maxIterations = int.Parse(textBox2.Text);
            
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    x0[i] = Convert.ToDouble(dataGridView1[0, i].Value);
                    a[i] = Convert.ToDouble(dataGridView1[1, i].Value);
                    b[i] = Convert.ToDouble(dataGridView1[2, i].Value);
                }

            double[] x;
            KorniVivod.Text = "";

            if (radioButton1.Checked)
            {
                x = new double[] { NonLinearEquationsLibrary.BisectionMethod(maxIterations, a[0], b[0], h, Solve.Func) };
                KorniVivod.Text += "Метод половинного деления (Дихотомии/бисекции). Решение: " + "\r\n" + Math.Round(x[0], 8).ToString() + "\r\n";
            }
            else if (radioButton2.Checked)
            {
                x = new double[] { NonLinearEquationsLibrary.SecantMethod(maxIterations, a[0], b[0], h, Solve.Func) };
                KorniVivod.Text += "Метод секущих (Хорд). Решение: " + "\r\n" + Math.Round(x[0], 8).ToString() + "\r\n";
            }
            else if (radioButton3.Checked)
            {
                x = NonLinearEquationsLibrary.NewtonMethod(maxIterations, x0, h, new Func<double[], double>[] { Solve.Func1, Solve.Func2 });
                KorniVivod.Text += "Метод Ньютона для решения систем нелинейных уравнений. Решение: " + "\r\n";
                foreach (double value in x)
                    KorniVivod.Text += Math.Round(value, 8).ToString() + "\r\n";
            }
            else if (radioButton4.Checked)
            {
                x = NonLinearEquationsLibrary.NewtonMethod(maxIterations, x0, h, new Func<double[], double>[] { Solve.Func6, Solve.Func7 });
                KorniVivod.Text = "Метод секущих для решения систем нелинейных уравнений. Решение: " + "\r\n";
                foreach (double value in x)
                    KorniVivod.Text += Math.Round(value, 8).ToString() + "\r\n";
            }
            else if (radioButton5.Checked)
            {
                x = NonLinearEquationsLibrary.ZeidelMethod(maxIterations, x0, h, new Func<double[], double>[] { Solve.Func8, Solve.Func9 });
                KorniVivod.Text = "Метод Зейделя для решения систем нелинейных уравнений. Решение: " + "\r\n";
                foreach (double value in x)
                    KorniVivod.Text += Math.Round(value, 8).ToString() + "\r\n";
            }
        }
    }
}
