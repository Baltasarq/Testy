// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


using Core;


public class DlgExport: Gtk.Dialog {
	public DlgExport(
				Document doc,
				string fileName,
				Gtk.Window owner,
				Dictionary<string, Gdk.Pixbuf?> icons)
	{
		this.icons = icons;
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
			Gdk.WindowHints.MinSize );

		this.Build();

		this.btClose = (Gtk.Button) this.AddButton( "", Gtk.ResponseType.Close );
		this.btSave = (Gtk.Button) this.AddButton( "", Gtk.ResponseType.Apply );
		this.btSave.Image = new Gtk.Image( icons[ "save" ] );
		this.btClose.Image = new Gtk.Image( icons[ "close" ] );

		this.edFileName.Text = fileName;
		this.SetPosition( Gtk.WindowPosition.CenterOnParent );
		this.UpdateExtensionHonoringFormat();
	}

	private void Build()
	{
		var frmFile = new Gtk.Frame( "" );
		var frmFormat = new Gtk.Frame( "" );
		var frmNumQuestions = new Gtk.Frame( "" );
		var hBoxFile = new Gtk.Box( Gtk.Orientation.Horizontal, 5 );
		var vbox = (Gtk.Box) this.Child;

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

	private readonly Gtk.Entry edFileName;
	private readonly Gtk.Button btSaveAs;
	private readonly Gtk.Button btSave;
	private readonly Gtk.Button btClose;
	private readonly Gtk.ComboBoxText cbFormat;
	private readonly Gtk.SpinButton sbNumQuestions;

	private readonly Dictionary<string, Gdk.Pixbuf?> icons;
}
