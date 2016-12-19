using System;
using System.Text;

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
			this.WriteLine( "Answers" );
			this.WriteLine( this.DrawAnswersBox() );
			this.WriteLine( "" );
			this.WriteLine( this.Document.ToString() );
			this.WriteLine( "" );
			this.WriteLine( "Solution" );
			this.WriteLine( this.DrawSolutionBox() );
		}

		private string DrawAnswersBox() {
			var toret = new StringBuilder();
			int numAnswers = this.FindMaxAnswers();
			int numQuestions = this.Document.CountQuestions;
			int numQuestionsDrawn = 0;
			const int MaxQuestionsPerLine = 25;
			string horizontalLine = new string( '-', ( 3 * MaxQuestionsPerLine ) + 4 );
			string doubleHorizontalLine = new string( '=', ( 3 * MaxQuestionsPerLine ) + 4 );

			while( numQuestionsDrawn < numQuestions ) {
				int maxQuestionsToPrintThisTime = 
					Math.Min( numQuestionsDrawn + MaxQuestionsPerLine, this.Document.CountQuestions );

				// Header
				toret.AppendLine( doubleHorizontalLine );
				toret.Append( "| |" );
				for(int i = numQuestionsDrawn; i < maxQuestionsToPrintThisTime; ++i) {
					toret.AppendFormat( "{0, 2}", i + 1 );
					toret.Append( '|' );
				}
				toret.AppendLine();
				toret.AppendLine( horizontalLine );

				// numAnswer's rows
				for(int i = 0; i < numAnswers; ++i) {
					toret.Append( '|' );
					toret.Append( (char) ( 'a' + i ) );
					toret.Append( '|' );
					for(int j = numQuestionsDrawn; j < maxQuestionsToPrintThisTime; ++j) {
						toret.Append( "  |" );
					}

					toret.AppendLine();
					toret.AppendLine( horizontalLine );
				}
				toret.AppendLine( doubleHorizontalLine );
				numQuestionsDrawn += maxQuestionsToPrintThisTime;
			}

			toret.AppendLine();
			return toret.ToString();
		}

		private string DrawSolutionBox() {
			var questions = this.Document.Questions;
			var toret = new StringBuilder();
			int numAnswers = this.FindMaxAnswers();
			int numQuestions = questions.Count;
			int numQuestionsDrawn = 0;
			const int MaxQuestionsPerLine = 25;
			string horizontalLine = new string( '-', ( 3 * MaxQuestionsPerLine ) + 4 );
			string doubleHorizontalLine = new string( '=', ( 3 * MaxQuestionsPerLine ) + 4 );

			while( numQuestionsDrawn < numQuestions ) {
				int maxQuestionsToPrintThisTime = 
					Math.Min( numQuestionsDrawn + MaxQuestionsPerLine, questions.Count );

				// Header
				toret.AppendLine( doubleHorizontalLine );
				toret.Append( "| |" );
				for(int i = numQuestionsDrawn; i < maxQuestionsToPrintThisTime; ++i) {
					toret.AppendFormat( "{0, 2}", i + 1 );
					toret.Append( '|' );
				}
				toret.AppendLine();
				toret.AppendLine( horizontalLine );

				// numAnswer's rows
				for(int i = 0; i < numAnswers; ++i) {
					toret.Append( '|' );
					toret.Append( (char) ( 'a' + i ) );
					toret.Append( '|' );

					for(int j = numQuestionsDrawn; j < maxQuestionsToPrintThisTime; ++j) {
						if ( questions[ j ].CorrectAnswer == i ) {
							toret.Append( " X|" );
						} else {
							toret.Append( "  |" );
						}
					}

					toret.AppendLine();
					toret.AppendLine( horizontalLine );
				}
				toret.AppendLine( doubleHorizontalLine );
				numQuestionsDrawn += maxQuestionsToPrintThisTime;
			}

			toret.AppendLine();
			return toret.ToString();
		}
	}
}

