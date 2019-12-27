using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MTextEditor
{
    static class Program
    {
        
        [DllImport("Project.dll")]
        public static extern void WriteCreateFile([In]char[] fileName, int size, [In, Out] char[] text);
        [DllImport("Project.dll")]
        public static extern void MyReadFromFile([In]char[] fileName, [In, Out] char[] text);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RM());
        }
    }
}
