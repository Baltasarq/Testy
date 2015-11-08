using System;
using Gtk;


namespace Testy.Gui {

    class Testy
    {
        public static void Main(string[] args)
        {
            try {
                Application.Init();
                MainWindow win = new MainWindow();
                win.Show();
                Application.Run();
            } catch(Exception e)
            {
                System.Console.WriteLine( e.Message );
                GtkUtil.Util.MsgError( null, "Critical error", e.Message );
            }
        }
    }
}
