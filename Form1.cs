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
            Parallel.ForEach(files, filename =>
            {
                Replace(filename, searchWord, replaceWord);
            });
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

        private void button1_Click(object sender, EventArgs e)
        {
            var lines = textBox5.Text.Split('\n').Select(_ => _.Replace("\r", ""));
            var files = GetFiles();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var rows = line.Split(',');
                foreach (var filename in files)
                {
                    if (filename.Contains(rows[0]))
                    {
                        var outname = filename.Replace(rows[0], rows[1]);
                        File.Move(filename, outname);
                    }
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            var text = textBox5.Text;
            File.WriteAllText("log.txt", text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("log.txt"))
            {
                var text = File.ReadAllText("log.txt");
                textBox5.Text = text;
            }
        }
    }
}
