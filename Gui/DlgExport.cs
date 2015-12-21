using System;
using GtkUtil;
using Testy.Core;

namespace Testy.Gui {
	public class DlgExport: Gtk.Dialog {
		public DlgExport(string fileName, Gtk.Window owner)
		{
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
		}

		private void Build() {
			var frmFile = new Gtk.Frame( "" );
			var frmFormat = new Gtk.Frame( "" );
			var hBoxFile = new Gtk.HBox( false, 5 );

			// Frame labels
			( (Gtk.Label) frmFile.LabelWidget ).Markup = "<b>File</b>";
			( (Gtk.Label) frmFormat.LabelWidget ).Markup = "<b>Format</b>";

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

			foreach(string f in Enum.GetNames( typeof( Document.Format ) ) ) {
				this.cbFormat.AppendText( f );
			}

			this.cbFormat.Active = 0;
			this.cbFormat.Changed += (sender, e) => this.UpdateExtensionHonoringFormat();
			frmFormat.Add( this.cbFormat );

			// Layout
			this.VBox.PackStart( frmFile, true, true, 5 );
			this.VBox.PackStart( frmFormat, true, false, 5 );

			// Buttons
			this.AddButton( Gtk.Stock.Cancel, Gtk.ResponseType.Cancel );
			this.AddButton( Gtk.Stock.Ok, Gtk.ResponseType.Ok );
			this.ShowAll();
		}

		public Document.Format OutputFormat {
			get {
				return (Document.Format) this.cbFormat.Active;
			}
		}

		public string FileName {
			get {
				return this.edFileName.Text;
			}
		}

		private void UpdateExtensionHonoringFormat()
		{
			var ext = Document.FormatExt[ this.cbFormat.Active ];
			string fileName = this.edFileName.Text;

			if ( !fileName.EndsWith( ext ) ) {
				fileName = System.IO.Path.ChangeExtension( fileName, ext );
				this.edFileName.Text = fileName;
			}
		}

		private void OnSaveAsClicked()
		{
			string fileName = this.edFileName.Text.Trim();
			string ext = Document.FormatExt[ this.cbFormat.Active ];
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

		private Gtk.Entry edFileName;
		private Gtk.Button btSave;
		private Gtk.ComboBox cbFormat;
	}
}

