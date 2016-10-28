using System;
using GtkUtil;
using Testy.Core;

namespace Testy.Gui {
	public class DlgExport: Gtk.Dialog {
		public DlgExport(Document doc, string fileName, Gtk.Window owner)
		{
			this.Document = doc;
			this.TransientFor = owner;
			this.Icon = owner.Icon;
			this.SetGeometryHints(
				this,
				new Gdk.Geometry() { MinHeight = 200, MinWidth = 400 },
				Gdk.WindowHints.MinSize
			);

			this.Build();

			this.edFileName.Text = fileName;
			this.SetPosition( Gtk.WindowPosition.CenterOnParent );
			this.UpdateExtensionHonoringFormat();
		}

		private void Build() {
			var frmFile = new Gtk.Frame( "" );
			var frmFormat = new Gtk.Frame( "" );
			var frmNumQuestions = new Gtk.Frame( "" );
			var hBoxFile = new Gtk.HBox( false, 5 );

			// Frame labels
			( (Gtk.Label) frmFile.LabelWidget ).Markup = "<b>File</b>";
			( (Gtk.Label) frmFormat.LabelWidget ).Markup = "<b>Format</b>";
			( (Gtk.Label) frmNumQuestions.LabelWidget ).Markup = "<b>Number of questions</b>";

			// File chooser
			this.edFileName = new Gtk.Entry();
			this.edFileName.IsEditable = false;
			this.btSave = new Gtk.Button( Gtk.Stock.SaveAs );
			this.btSave.Clicked += (sender, e) => this.OnSaveAsClicked();
			hBoxFile.PackStart( this.edFileName, true, true, 5 );
			hBoxFile.PackStart( this.btSave, false, false, 5 );
			frmFile.Add( hBoxFile );

			// Combo box of formats
			this.cbFormat = new Gtk.ComboBox( new string[] {} );

			foreach(string f in Enum.GetNames( typeof( Transformer.Format ) ) ) {
				this.cbFormat.AppendText( f );
			}

			this.cbFormat.Active = 0;
			this.cbFormat.Changed += (sender, e) => this.UpdateExtensionHonoringFormat();
			frmFormat.Add( this.cbFormat );

			// Spinner for number of questions
			this.sbNumQuestions = new Gtk.SpinButton( 1, this.Document.CountQuestions, 1 );
			this.sbNumQuestions.Value = this.Document.CountQuestions;
			frmNumQuestions.Add( this.sbNumQuestions );

			// Layout
			this.VBox.PackStart( frmFile, true, true, 5 );
			this.VBox.PackStart( frmFormat, true, false, 5 );
			this.VBox.PackStart( frmNumQuestions, true, false, 5 );

			// Buttons
			this.AddButton( Gtk.Stock.Cancel, Gtk.ResponseType.Cancel );
			this.AddButton( Gtk.Stock.Ok, Gtk.ResponseType.Ok );
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

			if ( !fileName.EndsWith( ext ) ) {
				this.edFileName.Text = System.IO.Path.ChangeExtension( fileName, ext );
			}
		}

		private void OnSaveAsClicked()
		{
			string fileName = this.edFileName.Text.Trim();
			string ext = Transformer.FormatExt[ this.cbFormat.Active ];
			bool chosen;

			chosen = Util.DlgSave(
				"Save exported file as...",
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
		private Gtk.Button btSave;
		private Gtk.ComboBox cbFormat;
		private Gtk.SpinButton sbNumQuestions;
	}
}

