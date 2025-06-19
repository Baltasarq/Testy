// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Core;


/// <summary>
/// Base class for all exporters
/// </summary>
public abstract class Exporter: Transformer, IDisposable {
	/// <summary>
	/// Initializes a new instance of the <see cref="Testy.Core.Exporter"/> class.
	/// </summary>
	/// <param name="doc">The Document object to export.</param>
	/// <param name="fileName">The name of the file to export to.</param>
	protected Exporter(Document doc, string fileName)
		:base( fileName )
	{
		this.doc = doc;
		this.writer = new StreamWriter( fileName );
	}

	/// <summary>
	/// Save the document in the specified fmt and fileName.
	/// </summary>
	/// <param name='doc'>
	/// The document to save, as a Document object.
	/// </param>
	/// <param name='fmt'>
	/// The format of the file.
	/// </param>
	/// <param name='fileName'>
	/// The file name, as a string.
	/// </param>
	/// <exception cref='ArgumentException'>
	/// Thrown when the format is invalid.
	/// </exception>
	public static void Save(Document doc, Format fmt, string fileName)
	{
		Exporter? exporter = null;

		try {
			if ( fmt == Format.Text ) {
				exporter = new TextExporter( doc, fileName );
				exporter.Export();
			}
			else
			if ( fmt == Format.Html ) {
				exporter = new HtmlExporter( doc, fileName );
				exporter.Export();
			}
			else
			if ( fmt == Format.Csv ) {
				exporter = new CsvExporter( doc, fileName );
				exporter.Export();
			}
			else
			if ( fmt == Format.Excel ) {
				exporter = new ExcelExporter( doc, fileName );
				exporter.Export();
			}
			else
			if ( fmt == Format.Xml ) {
				exporter = new XmlExporter( doc, fileName );
				exporter.Export();
			}
			else {
				throw new ArgumentException( "unrecognized format option" );
			}
		} finally {
			exporter?.Dispose();
		}

		return;
	}

	/// <summary>
	/// Writes the line s in the output.
	/// </summary>
	/// <param name="s">The line, as a string.</param>
	public void WriteLine(string s) {
		this.writer.WriteLine( s );
	}

	public void Dispose() {
		this.writer.Close();
	}

	/// <summary>
	/// Finds out the maximum number of answers in the document
	/// </summary>
	/// <returns>The max answers, as an int.</returns>
	protected int FindMaxAnswers() {
		int toret = 0;

		foreach(Question q in this.Document.Questions) {
			if ( toret < q.CountAnswers ) {
				toret = q.CountAnswers;
			}
		}

		return toret;
	}

	/// <summary>
	/// Exports the document to the specified output.
	/// </summary>
	public abstract void Export();

	/// <summary>
	/// Saves a text file with the given contents.
	/// </summary>
	/// <param name='fileName'>
	/// The file name, as a string.
	/// </param>
	/// <param name='contents'>
	/// The contents,as a string.
	/// </param>
	protected static void SaveTextFile(string fileName, string contents)
	{
		using var outfile = new StreamWriter( fileName );

		outfile.Write( contents );
	}

	/// <summary>
	/// Gets the document to export.
	/// </summary>
	/// <value>The document, as a Document object.</value>
	public Document Document {
		get {
			return this.doc;
		}
	}

	private readonly Document doc;
	private readonly StreamWriter writer;
}
