using System;
using System.Threading;
using System.Windows;

namespace TalaRagaApp
{
    public partial class App : Application
    {
        [STAThread]  // The main thread should already be STA in WPF apps
        public static void Main()
        {
            App app = new App();
            app.InitializeComponent();
            app.Run(new MainWindow()); // Main window

        }

        // Sample method to run in the new thread
       
    }
}
