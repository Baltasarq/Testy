// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Core;


/// <summary>
/// Base class for all importers
/// </summary>
public abstract class Importer: Transformer, IDisposable {
	protected Importer(string fileName)
		:base( fileName )
	{
		this.reader = new StreamReader( this.FileName );
	}

	/// <summary>
	/// Returns the next line from the input.
	/// A null return value indicates EOF.
	/// </summary>
	/// <returns>The line, as a string, or null.</returns>
	public string ReadLine()
	{
		return this.reader.ReadLine() ?? "";
	}

	/// <summary>Determines whether the stream has ended.</summary>
	/// <returns>true if the stream is over, false otherwise.</returns>
	public bool IsEOF() => this.reader.EndOfStream;

	/// <summary>
	/// Loads a file given its name, in the specified format.
	/// </summary>
	/// <param name="fmt">The format the contents of the file are organized.</param>
	/// <param name="fileName">The file name, as a string.</param>
	public static Document Load(Transformer.Format fmt, string fileName) {
		Document toret;
		Importer? importer = null;

		try {
			if ( fmt == Format.Text ) {
				importer = new TextImporter( fileName );
				toret = importer.Import();
			}
			else
			if ( fmt == Format.Xml ) {
				importer = new XmlImporter( fileName );
				toret = importer.Import();
			} else {
				throw new ArgumentException( "unrecognized format option" );
			}
		} finally {
			importer?.Dispose();
		}

		return toret;
	}

	/// <summary>
	/// Imports from input.
	/// </summary>
	public abstract Document Import();

	public void Dispose() {
		this.reader.Close();
	}

	private StreamReader reader;
}
