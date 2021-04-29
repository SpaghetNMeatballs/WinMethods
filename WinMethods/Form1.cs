using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace WinMethods
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);
        [DllImport("user32.dll")]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool IsIconic(IntPtr hWnd);

        int[] id;
        const int SW_MINIMIZE = 6, SW_SHOWNORMAL = 1;
        Process curproc;

        public Form1()
        {
            InitializeComponent();
        }

        private void On_Load(object sender, EventArgs e)
        {
            List<int> idTemp = new List<int>();
            int currowind = 0;

            foreach (Process process in Process.GetProcesses())
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    comboBox1.Items.Add(String.Format("Процесс: {0} ID: {1} Название окна: {2}\r\n", process.ProcessName, process.Id, process.MainWindowTitle));
                    idTemp.Add(process.Id);
                    dataGridView1.Rows.Add();
                    DataGridViewRow currow = dataGridView1.Rows[currowind];
                    currow.Cells[0].Value = process.Id;
                    currow.Cells[1].Value = process.ProcessName;
                    currow.Cells[2].Value = String.Format("{0}:{1}:{2}", process.StartTime.Hour, process.StartTime.Minute, process.StartTime.Second);
                    currow.Cells[3].Value = process.Responding;
                    currowind++;
                }
            }
            id = idTemp.ToArray();
        }

        private void IndexChanged(object sender, EventArgs e)
        {
            curproc = Process.GetProcessById(id[comboBox1.SelectedIndex]);
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            SetWindowText(curproc.MainWindowHandle, textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsIconic(curproc.MainWindowHandle))
            {
                ShowWindow(curproc.MainWindowHandle, SW_SHOWNORMAL);
            }
            else
            {
                ShowWindow(curproc.MainWindowHandle, SW_MINIMIZE);
            }
        }
    }
}
