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

            // Creating a new thread that needs to be STA
            Thread t = new Thread(new ThreadStart(DoWork));
            t.SetApartmentState(ApartmentState.STA);  // Set STA mode
            t.Start();
        }

        // Sample method to run in the new thread
        private static void DoWork()
        {
            // Work that requires STA mode, like accessing UI elements or using certain COM components
            MessageBox.Show("This is running in a separate STA thread!");
        }
    }
}
