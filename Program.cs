using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversitySystem
{
    internal static class Program
    {
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialise the SQLite database (creates file + schema on first run, seeds demo data)
            DatabaseHelper.Initialize();

            Application.Run(new LoginForm());
        }
    }
}
