// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Core;


using System.Text;


public class CsvExporter: Exporter {
	public CsvExporter(Document doc, string fileName)
		:base( doc, fileName )
	{
	}

	public override void Export()
	{
		this.WriteLine( this.GetQuestionsAsCsv() );

		// Needed extra csv files
		SaveTextFile( BuildCsvFileName( FileNamePrefixOptions, this.FileName ), this.GetOptionsAsCsv() );
		SaveTextFile( BuildCsvFileName( FileNamePrefixAnswers, this.FileName ), this.GetAnswersAsCsv() );
	}

	private static string BuildCsvFileName(string prefix, string fileName )
	{
		// Decompose
		var fileExt = Path.GetExtension( fileName ) ?? ".csv";
		var name = Path.GetFileNameWithoutExtension( fileName ) ?? "file_name";
		var fileDir = Path.GetDirectoryName( fileName ) ?? ".";

		// Add the prefix
		name = prefix + name + fileExt;
		return Path.Combine( fileDir, name );
	}

	/// <summary>
	/// Gets all questions as csv.
	/// </summary>
	/// <returns>
	/// The questions as csv, in a string.
	/// </returns>
	public string GetQuestionsAsCsv()
	{
		int numQuestion = 1;
		var toret = new StringBuilder();

		foreach(Question q in this.Document) {
			toret.Append( numQuestion );
			toret.Append( ',' );
			toret.Append( '"' );
			toret.Append( q.Text );
			toret.Append( '"' );
			toret.AppendLine();

			++numQuestion;
		}

		return toret.ToString();
	}

	/// <summary>
	/// Gets all options as csv.
	/// </summary>
	/// <returns>
	/// The options as csv, in a string.
	/// </returns>
	public string GetOptionsAsCsv()
	{
		int numQuestion = 1;
		var toret = new StringBuilder();

		foreach(Question q in this.Document) {
			var answers = q.Answers;

			for(int numAnswer = 0; numAnswer < q.CountAnswers; ++numAnswer ) {
				toret.Append( numQuestion );
				toret.Append( ',' );
				toret.Append( numAnswer +1 );
				toret.Append( ',' );
				toret.Append( '"' );
				toret.Append( answers[ numAnswer ] );
				toret.Append( '"' );
				toret.AppendLine();
			}

			++numQuestion;
		}

		return toret.ToString();
	}

	/// <summary>
	/// Gets the number of each correct answer as csv.
	/// </summary>
	/// <returns>
	/// Each answer's numbers as csv, in a string.
	/// </returns>
	public string GetAnswersAsCsv()
	{
		int numQuestion = 1;
		var toret = new StringBuilder();

		foreach(Question q in this.Document) {
			toret.Append( numQuestion );
			toret.Append( ',' );
			toret.Append( q.CorrectAnswer +1 );
			toret.AppendLine();

			++numQuestion;
		}

		return toret.ToString();
	}
}
