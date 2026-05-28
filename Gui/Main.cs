// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


using Testy.Core;

namespace Testy.Gui;


class Testy {
    [STAThread]
    static void Main(string[] args)
    {
        try {
            var app = new Gtk.Application( null, GLib.ApplicationFlags.None );

            app.Activated += (sender, e) => {
                var window = new MainWindow( app );
                window.Show();
            };

            app.Run( AppInfo.Id, args );
        } catch(Exception e)
        {
            GtkUtil.Misc.MsgError( null, "Critical error", e.Message );
        }
    }
}
