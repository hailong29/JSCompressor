using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Yahoo.Yui.Compressor;

namespace js压缩工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            Console.WriteLine(paths[0]);
            foreach (var item in paths) {
                parsePath(item);
            }

        }

        /// <summary>
        /// 一个递归,用来简化所有拖入的文件及文件夹
        /// </summary>
        /// <param name="item"></param>
        private void parsePath(string item) {
            string type = GetFileType(item);
            switch (type)
            {
                case "js":
                    SaveMinJS(item, item);
                    break;
                case "css":
                    SaveMinCSS(item, item);
                    break;
                case null:
                    DirectoryInfo TheFolder = new DirectoryInfo(item);
                    if (!TheFolder.Exists)
                        return;
                    //遍历文件
                    foreach (FileInfo NextFile in TheFolder.GetFiles())
                    {
                        string heatmappath = NextFile.FullName;
                        parsePath(heatmappath);
                    }
                    foreach (DirectoryInfo dirs in TheFolder.GetDirectories())
                    {
                        string heatmappath = dirs.FullName;
                        parsePath(heatmappath);
                    }
                    break;
                default:
                    break;
            }
        }

        private void SaveMinJS(string filePath, string newFilePath) {
            //Console.WriteLine(GetFilename(filePath));

            JavaScriptCompressor compressor = new JavaScriptCompressor();
            Console.WriteLine("默认编码：" + compressor.Encoding.EncodingName);
            Console.WriteLine("默认文件类型：" + compressor.ContentType);
            //使用utf-8 编码文件
            compressor.Encoding = Encoding.UTF8;

            string source = File.ReadAllText(filePath);
            source = compressor.Compress(source);
            File.WriteAllText(newFilePath, source);
            Console.WriteLine("覆盖原文件");
        }

        private void SaveMinCSS(string filePath, string newFilePath)
        {
            CssCompressor compressor = new CssCompressor();
            Console.WriteLine("默认编码：" + compressor.ContentType);

            string source = File.ReadAllText(filePath);
            source = compressor.Compress(source);
            File.WriteAllText(newFilePath, source);
            Console.WriteLine("覆盖原文件");
        }


        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static string GetFilename(string strFileName)
        {
            int i = strFileName.LastIndexOf("\\");
            string s = strFileName.Substring(i + 1);
            return s;
        }

        /// <summary>
        /// 获取文件格式
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static string GetFileType(string strFileName)
        {
            int i = strFileName.LastIndexOf(".");
            //文件夹时返回""
            if (i == -1) {
                return null;
            }
            string s = strFileName.Substring(i + 1);
            return s;
        }

        /// <summary>
        /// 判断路径是否是一个目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDir(string path)
        {
            string name = GetFilename(path);
            return GetFileType(name) == null;
        }

    }
}
