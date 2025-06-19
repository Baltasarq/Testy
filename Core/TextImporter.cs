// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Core;


/// <summary>
/// Imports a test from plain text.
/// Format:
/// 1. question?
/// a) answer1
/// b) answer2
/// 2. question?
/// - answer1
/// - answer2
/// </summary>
public class TextImporter: Importer {
	public TextImporter(string fileName)
		:base( fileName )
	{
	}

	/// <summary>
	/// Determines whether the given s is a question.
	/// </summary>
	/// <returns><c>true</c> if s is a question; otherwise, <c>false</c>.</returns>
	/// <param name="s">The line to check. It is modified if it is a question.</param>
	private static bool IsQuestion(ref string s)
	{
		bool toret = false;
		int pos = 0;

		while( pos < s.Length
			&& char.IsDigit( s[ pos ] ) )
		{
			++pos;
		}

		toret = ( pos < s.Length && s[ pos ] == '.' );

		if ( toret ) {
			s = s.Substring( pos + 1 );
		}

		return toret;
	}

	/// <summary>
	/// Determines whether this given string is an answer.
	/// </summary>
	/// <returns><c>true</c> if the parameter is an answer; otherwise, <c>false</c>.</returns>
	/// <param name="s">The line to check. This line is modified it is an answer.</param>
	private static bool IsAnswer(ref string s)
	{
		bool toret;

		if ( s[ 0 ] == '-' ) {
			toret = true;
			s = s.Substring( 1 );
		} else {
			toret = ( char.IsLetterOrDigit( s[ 0 ] ) && s[ 1 ] == ')' );
			if ( toret ) {
				s = s.Substring( 2 );
			}
		}

		return toret;
	}

	public override Document Import()
	{
		var toret = new Document();
		Question? q = null;
		string line;

		toret.Clear();
		line = this.ReadLine();

		while ( !this.IsEOF() ) {
			line = line.Trim();

			if ( line.Length > 0 ) {
				if ( IsQuestion( ref line ) ) {
					if ( q != null ) {
						toret.Add( q );
					}

					q = new Question( line );
					q.ClearAnswers();
				}
				else
				if ( IsAnswer( ref line ) ) {
					q ??= new Question( "Missing question." );
					q.AddAnswer( line );
				}
			}

			line = this.ReadLine();
		}

		if ( q is not null ) {
			toret.Add( q );
		}

		return toret;
	}
}
