// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


public partial class DlgCheckTest: Gtk.Dialog {
    private void BuildIcons()
    {
        try {
            this.iconNext = Gdk.Pixbuf.LoadFromResource( "Testy.next.png" );
            this.iconPrevious = Gdk.Pixbuf.LoadFromResource( "Testy.previous.png" );
            this.iconClose = Gdk.Pixbuf.LoadFromResource( "Testy.close.png" );
            this.iconCheck = Gdk.Pixbuf.LoadFromResource( "Testy.check.png" );

            this.tbToolbar.Style = Gtk.ToolbarStyle.Icons;
        } catch(Exception) {
            this.tbToolbar.Style = Gtk.ToolbarStyle.Text;
        }
    }

    private void BuildActions()
    {
        // Next
        this.actNext.Activated += (sender, e) => this.NextQuestion();

        if ( this.iconNext is not null ) {
            this.actNext.Icon = this.iconNext;
        }

        // Previous
        this.actPrev.Activated += (sender, e) => this.PreviousQuestion();

        if ( this.iconPrevious is not null ) {
            this.actPrev.Icon = this.iconPrevious;
        }

        // Quit
        this.actQuit.Activated += (sender, e) => this.Quit();

        if ( this.iconClose is not null ) {
            this.actQuit.Icon = this.iconClose;
        }
    }

    private Gtk.ScrolledWindow BuildQuestionView()
    {
        return [ this.txtQuestion ];
    }

    private Gtk.ScrolledWindow BuildCorrectionView()
    {
        var scrlCorrection = new Gtk.ScrolledWindow();

        // Modify font for mono
        var font = new Pango.FontDescription () { Family = "Monospace" };
        this.txtCorrection.ModifyFont( font );

        scrlCorrection.Add( this.txtCorrection );
        return scrlCorrection;
    }

    private Gtk.Box BuildActionbar()
    {
        this.cbQuestionNumber.Changed += (sender, e) => this.OnQuestionNumberChanged();
        this.lblTitle.Text = this.Document.Title + ':';
        this.lblCorrect.Text = "";
        this.lblNumber.Text = this.Document.CountQuestions.ToString();

        var hbxChkContainer = new Gtk.Box( Gtk.Orientation.Horizontal, 5 );

        hbxChkContainer.PackStart( this.imgCorrection, false, false, 5 );
        hbxChkContainer.PackStart( this.cbQuestionNumber, false, false, 5 );
        hbxChkContainer.PackEnd( this.lblNumber, false, false, 5 );
        hbxChkContainer.PackEnd( this.lblCorrect, false, false, 5 );
        hbxChkContainer.PackEnd( this.lblTitle, false, false, 5 );

        return hbxChkContainer;
    }

    /// <summary>
    /// Builds the check framework.
    /// </summary>
    private void Build()
    {
        var vbox = new Gtk.Box( Gtk.Orientation.Vertical, 5 );

        this.BuildIcons();
        this.BuildActions();

        vbox.PackStart( this.tbToolbar, false, false, 0 );
        vbox.PackStart( this.BuildQuestionView(), true, true, 5 );
        vbox.PackStart( this.BuildCorrectionView(), true, true, 5 );
        vbox.PackStart( this.BuildActionbar(), false, false, 0 );
        this.Add( vbox );

        // Set min size
        this.SetGeometryHints(
            this,
            new Gdk.Geometry {
                MinWidth = 640,
                MinHeight = 480 },
            Gdk.WindowHints.MinSize );

        this.WindowPosition = Gtk.WindowPosition.CenterOnParent;
        this.Shown += (sender, e) => this.Init();
        this.DeleteEvent += (sender, e) => this.Quit();
        this.ShowAll();
    }

    private Gdk.Pixbuf? iconNext;
    private Gdk.Pixbuf? iconPrevious;
    private Gdk.Pixbuf? iconClose;
    private Gdk.Pixbuf? iconCheck;

    private GtkUtil.UIAction actNext;
    private GtkUtil.UIAction actPrev;
    private GtkUtil.UIAction actQuit;

    private Gtk.Toolbar tbToolbar;
    private Gtk.Image imgCorrection;
    private Gtk.ComboBox cbQuestionNumber;
    private Gtk.TextView txtQuestion;
    private Gtk.TextView txtCorrection;
    private Gtk.Label lblTitle;
    private Gtk.Label lblNumber;
    private Gtk.Label lblCorrect;
}
