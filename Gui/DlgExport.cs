using System;
using Testy.Core;
using Gtk;

using GtkUtil;

namespace Testy.Gui {

	public partial class DlgExport : Gtk.Dialog {

		public DlgExport(string fileName)
		{
			// Prepare window
			this.Build();
			this.Title = "Export";
			
			// Prepare combobox
			this.OnFormatChanged( null, null );
			ListStore model = new ListStore( new Type[]{ typeof( string ) } );
			foreach(var f in Enum.GetNames( typeof( Document.Format ) ) ) {
				model.AppendValues( new string[]{ f } );
			}
			
			this.cbFormat.Model = model;
			this.cbFormat.Active = 0;
			
			// Prepare file name
			this.edFileName.Text = fileName;
			this.UpdateExtensionHonoringFormat();
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

		protected void OnFormatChanged(object sender, System.EventArgs e)
		{
			this.UpdateExtensionHonoringFormat();
		}
		
		protected void UpdateExtensionHonoringFormat()
		{
			var ext = Document.FormatExt[ this.cbFormat.Active ];
			string fileName = this.edFileName.Text;
			
			if ( !fileName.EndsWith( ext ) ) {
				fileName = System.IO.Path.ChangeExtension( fileName, ext );
				this.edFileName.Text = fileName;
			}
		}

		protected void OnSaveAsClicked(object sender, System.EventArgs e)
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
	}
}

