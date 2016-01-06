using System;
using System.IO;

namespace Testy.Core {
	/// <summary>
	/// Exports a given document to a HTML format.
	/// </summary>
	public class HtmlExporter: Exporter {
		public HtmlExporter(Document doc, string fileName)
			:base( doc, fileName )
		{
		}

		public override void Export()
		{
			this.WriteLine( this.Document.ToHtml() );
		}
	}
}

