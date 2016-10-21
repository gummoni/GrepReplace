using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GrepReplace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void brReplace_Click(object sender, EventArgs e)
        {
            var searchWord = textBox3.Text;
            var replaceWord = textBox4.Text;
            var files = GetFiles();
            textBox5.Text += $"{searchWord},{replaceWord}\r\n";
            foreach (var filename in files)
            {
                Replace(filename, searchWord, replaceWord);
            }
        }

        /// <summary>
        /// 対象ファイル一覧
        /// </summary>
        /// <returns></returns>
        string[] GetFiles()
        {
            return Directory.GetFiles(textBox1.Text, textBox2.Text, SearchOption.AllDirectories);
        }

        public void Replace(string filename, string searchWord, string replaceWord)
        {
            var before = File.ReadAllLines(filename);
            var after = before.Select(_ => _.Replace(searchWord, replaceWord)).ToArray();
            File.WriteAllLines(filename, after);
        }
    }
}
