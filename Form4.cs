using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using OfficeOpenXml;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace DehotomiaM
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        string[,] list = new string[50, 50];

        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            dataGridViewA.Rows.Clear();
            dataGridViewA.Columns.Clear();
            dataGridViewB.Rows.Clear();
            if (!int.TryParse(textBox1.Text, out n))
            {
                throw new ArgumentException("Некорректные значения входных данных");
            }
            dataGridViewA.ColumnCount = n;
            dataGridViewA.RowCount = n;
            dataGridViewB.RowCount = n;
            var RandomNumber = new Random((int)Stopwatch.GetTimestamp());
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double number;
                    number = RandomNumber.Next(-50, 50) - 10 + 15;
                    dataGridViewA.Rows[i].Cells[j].Value = i + number * j + number;
                }
                dataGridViewA.Columns[i].HeaderCell.Value = $"X{i + 1}";
            }
            for (int i = 0; i < n; i++) // по всем строкам
            {
                double num;
                num = RandomNumber.Next(-20, 50) - 10 + 15;
                dataGridViewB.Rows[i].Cells[0].Value = num;
                dataGridViewB.Columns[0].HeaderCell.Value = "B";
            }
        }

        private int ExportExcel()
        {
            // Выбрать путь и имя файла в диалоговом окне
            OpenFileDialog ofd = new OpenFileDialog();
            // Задаем расширение имени файла по умолчанию (открывается папка с программой)
            ofd.DefaultExt = "*.xls;*.xlsx";
            // Задаем строку фильтра имен файлов, которая определяет варианты
            ofd.Filter = "файл Excel (Spisok.xlsx)|*.xlsx";
            // Задаем заголовок диалогового окна
            ofd.Title = "Выберите файл базы данных";
            if (!(ofd.ShowDialog() == DialogResult.OK)) // если файл БД не выбран -> Выход
                return 0;
            Excel.Application ObjWorkExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(ofd.FileName);
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];//получить 1-й лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//последнюю ячейку
                                                                                                // размеры базы
            int lastColumn = (int)lastCell.Column;
            int lastRow = (int)lastCell.Row;
            // Перенос в промежуточный массив класса Form1: string[,] list = new string[50, 5]; 
            for (int j = 0; j < 5; j++) //по всем колонкам
                for (int i = 0; i < lastRow; i++) // по всем строкам
                    list[i, j] = ObjWorkSheet.Cells[i + 1, j + 1].ToString(); //считываем данные
            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из Excel
            GC.Collect(); // убрать за собой
            return lastRow;
        }
        public int NewExportExcel(string filePath)
        {
            // Выбрать путь и имя файла в диалоговом окне
            OpenFileDialog ofd = new OpenFileDialog();
            // Задаем расширение имени файла по умолчанию (открывается папка с программой)
            ofd.DefaultExt = "*.xlsx";
            // Задаем строку фильтра имен файлов, которая определяет варианты
            ofd.Filter = "файл Excel (Spisok.xlsx)|*.xlsx";
            // Задаем заголовок диалогового окна
            ofd.Title = "Выберите файл базы данных";
            if (!(ofd.ShowDialog() == DialogResult.OK)) // если файл БД не выбран -> Выход
                return 0;

            using (var package = new ExcelPackage(new FileInfo(ofd.FileName)))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var worksheet = package.Workbook.Worksheets[0]; //получить 1-й лист
                var lastCell = worksheet.Cells[worksheet.Dimension.End.Row, worksheet.Dimension.End.Column]; //последнюю ячейку

                // размеры базы
                int lastColumn = lastCell.Start.Column;
                int lastRow = lastCell.Start.Row;

                if (lastColumn != lastRow)
                {
                    throw new Exception("Матрица неквадратная");
                }
                // Перенос в промежуточный массив класса Form1: string[,] list = new string[50, 5]; 
                for (int j = 0; j < lastColumn; j++) //по всем колонкам
                    for (int i = 0; i < lastRow; i++) // по всем строкам
                        list[i, j] = worksheet.Cells[i + 1, j + 1].Text; //считываем данные
                return lastRow;
            }
        }
        public int NewExportExcelVecotr(string filePath)
        {
            // Выбрать путь и имя файла в диалоговом окне
            OpenFileDialog ofd = new OpenFileDialog();
            // Задаем расширение имени файла по умолчанию (открывается папка с программой)
            ofd.DefaultExt = "*.xlsx";
            // Задаем строку фильтра имен файлов, которая определяет варианты
            ofd.Filter = "файл Excel (Spisok.xlsx)|*.xlsx";
            // Задаем заголовок диалогового окна
            ofd.Title = "Выберите файл базы данных";
            if (!(ofd.ShowDialog() == DialogResult.OK)) // если файл БД не выбран -> Выход
                return 0;

            using (var package = new ExcelPackage(new FileInfo(ofd.FileName)))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var worksheet = package.Workbook.Worksheets[1]; //получить 1-й лист
                var lastCell = worksheet.Cells[worksheet.Dimension.End.Row, worksheet.Dimension.End.Column]; //последнюю ячейку

                // размеры базы

                int lastColumn = lastCell.Start.Column;
                int lastRow = lastCell.Start.Row;
                dataGridViewB.Rows.Clear();
                dataGridViewB.RowCount = lastRow;

                if (lastColumn != 1)
                {
                    throw new Exception("Вектор некорректный");
                }
                // Перенос в промежуточный массив класса Form1: string[,] list = new string[50, 5]; 
                for (int i = 0; i < lastRow; i++)
                { //по всем колонкам

                    dataGridViewB.Rows[i].Cells[0].Value = worksheet.Cells[i + 1, 1].Text;
                    dataGridViewB.Columns[0].HeaderCell.Value = "B";
                }
                return lastRow;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int n = NewExportExcel("D:\\visualstudio\\slae\\Metods-SLAE\\Metods-SLAE\\1.xlsx");
            dataGridViewA.Rows.Clear();
            dataGridViewA.ColumnCount = n;
            dataGridViewA.RowCount = n;
            for (int i = 0; i < n; i++) // по всем строкам
            {
                for (int j = 0; j < n; j++)
                {
                    dataGridViewA.Rows[i].Cells[j].Value = list[i, j];
                }//по всем колонкам
                dataGridViewA.Columns[i].HeaderCell.Value = $"X{i + 1}";
            }
            textBox1.Text = n.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int n = NewExportExcelVecotr("D:\\visualstudio\\slae\\Metods-SLAE\\Metods-SLAE\\2.xlsx");

        }
        public void myGauss(ref double[,] a, ref double[] b, ref double[] x, int n)
        {
            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = k + 1; j < n; j++)
                    {
                        a[i, j] = a[i, j] - a[k, j] * (a[i, k] / a[k, k]);
                    }
                    b[i] = b[i] - b[k] * a[i, k] / a[k, k];
                }
            }

            double sum = 0;

            for (int k = n - 1; k >= 0; k--)
            {
                sum = 0;
                for (int j = k + 1; j < n; j++)
                {
                    sum = sum + a[k, j] * x[j];
                }
                x[k] = (b[k] - sum) / a[k, k];
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int n;
                if (!int.TryParse(textBox1.Text, out n))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                dataGridViewA.ColumnCount = n;
                dataGridViewA.RowCount = n;
                dataGridViewB.RowCount = n;
                if (checkBox2.Checked)
                {
                    double[,] A = new double[n, n];
                    double[] B = new double[n];
                    double[] X = new double[n];

                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            A[i, j] = Convert.ToInt32(dataGridViewA[j, i].Value);
                        }
                        B[i] = Convert.ToInt32(dataGridViewB[0, i].Value);
                    }
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    myGauss(ref A, ref B, ref X, n);
                    stopwatch.Stop();
                    double time = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000000000;
                    textBox2.AppendText("Метод Крамера: " + "\r\n");
                    textBox2.AppendText($"Время выполнения: {time} мс\n" + "\r\n");
                    for (int i = 0; i < n; i++)
                    {
                        textBox2.AppendText($"  x{i + 1} = {X[i].ToString("F2")}\n");
                    }

                }
                if (checkBox1.Checked)
                {
                    double[,] A = new double[n, n];
                    double[] B = new double[n];
                    double[] X = new double[n];
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            A[i, j] = Convert.ToInt32(dataGridViewA[j, i].Value);
                        }
                        B[i] = Convert.ToInt32(dataGridViewB[0, i].Value);
                    }
                    Stopwatch stopwatch2 = new Stopwatch();
                    stopwatch2.Start();
                    //X = SolveEquations(A, B);
                    X = Cramer(A, B);
                    stopwatch2.Stop();
                    double time = (double)stopwatch2.ElapsedTicks / Stopwatch.Frequency * 1000000000;
                    textBox2.AppendText("\r\n\r\n" + "Метод Гаусса: " + "\r\n");
                    textBox2.AppendText($"Время выполнения: {time} мс\n" + "\r\n");
                    for (int i = 0; i < X.Length; i++)
                    {
                        textBox2.AppendText($"  x{i + 1} = {X[i].ToString("F2")}\n");
                    }

                }
                if (checkBox3.Checked)
                {
                    double[,] A = new double[n, n];
                    double[] B = new double[n];
                    double[] X = new double[n];
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            A[i, j] = Convert.ToInt32(dataGridViewA[j, i].Value);
                        }
                        B[i] = Convert.ToInt32(dataGridViewB[0, i].Value);
                    }
                    Stopwatch stopwatch3 = new Stopwatch();
                    stopwatch3.Start();
                    myJordanGauss(ref A, ref B, ref X, n);
                    stopwatch3.Stop();
                    double time = (double)stopwatch3.ElapsedTicks / Stopwatch.Frequency * 1000000000;
                    textBox2.AppendText("\r\n\r\n" + "Метод Жордана-Гаусса: " + "\r\n");
                    textBox2.AppendText($"Время выполнения: {time} мс\n" + "\r\n");

                    for (int i = 0; i < n; i++)
                    {
                        textBox2.AppendText($"  x{i + 1} = {X[i].ToString("F2")}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }


        }
        public double[] SolveEquations(double[,] A, double[] B)
        {
            int n = A.GetLength(0);
            double[] X = new double[n];

            double detA = Determinant(A);

            if (detA == 0)
            {
                // Система уравнений не имеет решений
                return null;
            }

            for (int k = 0; k < n; k++)
            {
                double[,] Ak = ReplaceColumn(A, B, k);
                double detAk = Determinant(Ak);

                X[k] = detAk / detA;
            }

            return X;
        }

        public double Determinant(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double det = 0;

            if (n == 1)
            {
                return matrix[0, 0];
            }
            else if (n == 2)
            {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            else
            {
                for (int k = 0; k < n; k++)
                {
                    double[,] subMatrix = new double[n - 1, n - 1];

                    for (int i = 1; i < n; i++)
                    {
                        int jNew = 0;

                        for (int j = 0; j < n; j++)
                        {
                            if (j == k)
                            {
                                continue;
                            }

                            subMatrix[i - 1, jNew] = matrix[i, j];
                            jNew++;
                        }
                    }

                    det += Math.Pow(-1, k) * matrix[0, k] * Determinant(subMatrix);
                }
            }

            return det;
        }

        public double[,] ReplaceColumn(double[,] matrix, double[] column, int columnIndex)
        {
            int n = matrix.GetLength(0);
            double[,] result = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j == columnIndex)
                    {
                        result[i, j] = column[i];
                    }
                    else
                    {
                        result[i, j] = matrix[i, j];
                    }
                }
            }

            return result;
        }
        public void myJordanGauss(ref double[,] a, ref double[] b, ref double[] x, int n)
        {
            for (int k = 0; k < n; k++)
            {
                double div = a[k, k];

                for (int j = k; j < n; j++)
                {
                    a[k, j] /= div;
                }

                b[k] /= div;

                for (int i = 0; i < n; i++)
                {
                    if (i != k)
                    {
                        double mult = a[i, k];

                        for (int j = k; j < n; j++)
                        {
                            a[i, j] -= mult * a[k, j];
                        }

                        b[i] -= mult * b[k];
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                x[i] = b[i];
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        static double[,] Multiply(double[,] a, double[] b)
        {
            int rowsA = a.GetLength(0);
            int colsA = a.GetLength(1);
            int colsB = b.Length;

            double[,] c = new double[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    for (int k = 0; k < colsA; k++)
                    {
                        c[i, j] += a[i, k] * b[k];
                    }
                }
            }

            return c;
        }

        static double[,] Multiply(double[,] a, double b)
        {
            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            double[,] c = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    c[i, j] = a[i, j] * b;
                }
            }

            return c;
        }

        static double Determinant(double[,] a, int n)
        {
            if (n == 1)
            {
                return a[0, 0];
            }

            double det = 0;
            int sign = 1;

            for (int i = 0; i < n; i++)
            {
                det += sign * a[0, i] * Determinant(ExtractMatrix(a, 0, i), n - 1);
                sign *= -1;
            }

            return det;
        }

        static double[,] ExtractMatrix(double[,] a, int rowToExtract, int colToExtract)
        {
            int rows = a.GetLength(0);
            int cols = a.GetLength(1);

            double[,] b = new double[rows - 1, cols - 1];

            for (int i = 0; i < rows; i++)
            {
                if (i == rowToExtract)
                {
                    continue;
            }

            for (int j = 0; j<cols; j++)
            {
                if (j == colToExtract)
                {
                    continue;
                }

            int newI = i < rowToExtract ? i : i - 1;
            int newJ = j < colToExtract ? j : j - 1;

            b[newI, newJ] = a[i, j];
            }
        }
        return b;
    }

    static double[,] CofactorMatrix(double[,] a, int n)
{
    double[,] b = new double[n, n];

    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            b[i, j] = Math.Pow(-1, i + j) * Determinant(ExtractMatrix(a, i, j), n - 1);
        }
    }

    return b;
}

    static double[,] AdjointMatrix(double[,] a, int n)
    {
        double[,] b = new double[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                b[i, j] = CofactorMatrix(a, n)[j, i];
            }
        }

        return b;
    }

    static double[,] InverseMatrix(double[,] a, int n)
    {
        double det = Determinant(a, n);

        if (det == 0)
        {
            throw new Exception("The matrix is singular.");
        }

        double[,] adjoint = AdjointMatrix(a, n);

        double[,] inv = Multiply(adjoint, 1 / det);

        return inv;
    }

    static double[,] Transpose(double[,] a, int n)
    {
        double[,] b = new double[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                b[i, j] = a[j, i];
            }
        }

        return b;
    }

    static double[] Cramer(double[,] a, double[] b) { 
        int n = a.GetLength(0);

        if (n != b.Length)
        {
            throw new Exception("The number of columns in the matrix must be equal to the length of the vector.");
        }

        double[,] aug = new double[n, n + 1];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                aug[i, j] = a[i, j];
            }

            aug[i, n] = b[i];
        }

        double det = Determinant(aug, n);

        if (det == 0)
        {
            throw new Exception("The matrix is singular.");
        }

        double[,] inv = InverseMatrix(aug, n);

        double[] result = new double[n];

        for (int i = 0; i < n; i++)
        {
            result[i] = inv[i, n - 1];
        }

        return result;
        }
    }
}