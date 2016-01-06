using System;
using System.IO;

namespace Testy.Core {
	/// <summary>
	/// Exports a given document in text format.
	/// </summary>
	public class TextExporter: Exporter {
		public TextExporter(Document doc, string fileName)
			:base( doc, fileName )
		{
		}

		public override void Export()
		{
			this.WriteLine( this.Document.ToString() );
		}
	}
}

