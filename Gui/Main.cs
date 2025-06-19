// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


class Testy {
    [STAThread]
    static void Main(string[] args)
    {
        try {
            var app = new Gtk.Application( "com.devbaltasarq.testy", GLib.ApplicationFlags.None );
            Gtk.Application.Init();

            var win = new MainWindow( app );
            win.Show();

            Gtk.Application.Run();
        } catch(Exception e)
        {
            GtkUtil.Misc.MsgError( null, "Critical error", e.Message );
        }
    }
}
