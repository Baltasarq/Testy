// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


public partial class DlgCheckTest: Gtk.Dialog {
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
        var vbox = (Gtk.Box) this.Child;

        vbox.PackStart( this.tbToolbar, false, false, 0 );
        vbox.PackStart( this.BuildQuestionView(), true, true, 5 );
        vbox.PackStart( this.BuildCorrectionView(), true, true, 5 );
        vbox.PackStart( this.BuildActionbar(), false, false, 0 );

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

    private readonly Gtk.ToolButton btNext;
    private readonly Gtk.ToolButton btPrev;
    private readonly Gtk.ToolButton btQuit;

    private readonly Gtk.Toolbar tbToolbar;
    private readonly Gtk.Image imgCorrection;
    private readonly Gtk.ComboBox cbQuestionNumber;
    private readonly Gtk.TextView txtQuestion;
    private readonly Gtk.TextView txtCorrection;
    private readonly Gtk.Label lblTitle;
    private readonly Gtk.Label lblNumber;
    private readonly Gtk.Label lblCorrect;
}
