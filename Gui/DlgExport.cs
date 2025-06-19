// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


using Core;


public class DlgExport: Gtk.Dialog {
	public DlgExport(Document doc, string fileName, Gtk.Window owner)
	{
		this.edFileName = new Gtk.Entry { IsEditable = false };
		this.btSaveAs = new Gtk.Button( "..." );
		this.cbFormat = new Gtk.ComboBoxText();
		this.sbNumQuestions = new Gtk.SpinButton( 1, doc.CountQuestions, 1 );

		this.Document = doc;
		this.TransientFor = owner;
		this.Icon = owner.Icon;
		this.SetGeometryHints(
			this,
			new Gdk.Geometry { MinHeight = 200, MinWidth = 400 },
			Gdk.WindowHints.MinSize
		);

		this.Build();

		this.btSave = new Gtk.Button( new Gtk.Image( this.iconSave ) );
		this.btClose = new Gtk.Button( new Gtk.Image( this.iconClose ) );

		this.edFileName.Text = fileName;
		this.SetPosition( Gtk.WindowPosition.CenterOnParent );
		this.UpdateExtensionHonoringFormat();
	}

	private void BuildIcons()
	{
		try {
			this.iconSave = Gdk.Pixbuf.LoadFromResource( "Testy.save.png" );
			this.iconClose = Gdk.Pixbuf.LoadFromResource( "Testy.close.png" );
		} catch(Exception) {
			// Ignored, no icons.
		}
	}

	private void Build()
	{
		var frmFile = new Gtk.Frame( "" );
		var frmFormat = new Gtk.Frame( "" );
		var frmNumQuestions = new Gtk.Frame( "" );
		var vbox = new Gtk.Box( Gtk.Orientation.Vertical, 5 );
		var hBoxFile = new Gtk.Box( Gtk.Orientation.Horizontal, 5 );

		this.BuildIcons();

		// Frame labels
		( (Gtk.Label) frmFile.LabelWidget ).Markup = "<b>File</b>";
		( (Gtk.Label) frmFormat.LabelWidget ).Markup = "<b>Format</b>";
		( (Gtk.Label) frmNumQuestions.LabelWidget ).Markup = "<b>Number of questions</b>";

		// File chooser
		this.btSaveAs.Clicked += (sender, e) => this.OnSaveAsClicked();
		hBoxFile.PackStart( this.edFileName, true, true, 5 );
		hBoxFile.PackStart( this.btSaveAs, false, false, 5 );
		frmFile.Add( hBoxFile );

		// Combo box of formats
		foreach(string f in Enum.GetNames( typeof( Transformer.Format ) ) ) {
			this.cbFormat.AppendText( f );
		}

		this.cbFormat.Active = 0;
		this.cbFormat.Changed += (sender, e) => this.UpdateExtensionHonoringFormat();
		frmFormat.Add( this.cbFormat );

		// Spinner for number of questions
		this.sbNumQuestions.Value = this.Document.CountQuestions;
		frmNumQuestions.Add( this.sbNumQuestions );

		// Layout
		vbox.PackStart( frmFile, true, true, 5 );
		vbox.PackStart( frmFormat, true, false, 5 );
		vbox.PackStart( frmNumQuestions, true, false, 5 );
		this.Add( vbox );

		// Buttons
		this.AddActionWidget( btClose, Gtk.ResponseType.Close );
		this.AddActionWidget( btSave, Gtk.ResponseType.Close );
		this.ShowAll();
	}

	public Transformer.Format OutputFormat {
		get {
			return (Transformer.Format) this.cbFormat.Active;
		}
	}

	public string FileName {
		get {
			return this.edFileName.Text;
		}
	}

	public int NumQuestions {
		get {
			return (int) this.sbNumQuestions.Value;
		}
	}

	private void UpdateExtensionHonoringFormat()
	{
		var ext = Transformer.FormatExt[ this.cbFormat.Active ];
		string fileName = this.edFileName.Text.Trim();

		if ( !fileName.EndsWith( ext, StringComparison.InvariantCulture ) )
		{
			this.edFileName.Text = System.IO.Path.ChangeExtension( fileName, ext );
		}
	}

	private void OnSaveAsClicked()
	{
		string fileName = this.edFileName.Text.Trim();
		string ext = Transformer.FormatExt[ this.cbFormat.Active ];
		bool chosen;

		chosen = GtkUtil.Misc.DlgSave(
			AppInfo.Name,
			"Save exported file as...",
			this,
			ref fileName,
			"*" + ext
		);

		if ( chosen ) {
			this.edFileName.Text = fileName;
			this.UpdateExtensionHonoringFormat();
		}
	}

	/// <summary>
	/// Gets the document.
	/// </summary>
	/// <value>The document.</value>
	public Document Document {
		get; private set;
	}

	private Gtk.Entry edFileName;
	private Gtk.Button btSaveAs;
	private Gtk.Button btSave;
	private Gtk.Button btClose;
	private Gtk.ComboBoxText cbFormat;
	private Gtk.SpinButton sbNumQuestions;

	private Gdk.Pixbuf? iconSave;
	private Gdk.Pixbuf? iconClose;
}
