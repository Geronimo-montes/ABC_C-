using System;
using Gtk;

namespace Prueba_1
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.Prueba_1.Prueba_1", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}
