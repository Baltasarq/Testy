using System;
using System.IO;
using System.Text;

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
			this.WriteLine( "<html><head>" );
			this.WriteLine( "<meta charset='utf-8' />" );
			this.WriteLine( "</head><body>" );

			this.WriteLine( "<p><h2>Answers</h2>" );
			this.WriteLine( this.DrawAnswersBox() );
			this.WriteLine( "</p>" );

			this.WriteLine( "<p><h2>Questions</h2>" );
			this.WriteLine( this.Document.ToHtml() );
			this.WriteLine( "</p>" );

			this.WriteLine( "<p><h2>Solution</h2>" );
			this.WriteLine( this.DrawSolutionBox() );
			this.WriteLine( "</p>" );

			this.WriteLine( "</body></html>" );
		}

		private string DrawAnswersBox() {
			var toret = new StringBuilder();
			int numAnswers = this.FindMaxAnswers();
			int numQuestions = this.Document.CountQuestions;
			int numQuestionsDrawn = 0;
			const int MaxQuestionsPerLine = 25;

			while( numQuestionsDrawn < numQuestions ) {
				int maxQuestionsToPrintThisTime = 
					Math.Min( numQuestionsDrawn + MaxQuestionsPerLine, this.Document.CountQuestions );

				// Header
				toret.AppendLine( "<table width='80%' border='1'>"  );
				toret.Append( "<tr><td>&nbsp</td>" );
				for(int i = numQuestionsDrawn; i < maxQuestionsToPrintThisTime; ++i) {
					toret.AppendFormat( "<td style='background-color:lightgray'>{0}</td>", i + 1 );
				}
				toret.AppendLine();
				toret.AppendLine( "</tr>" );

				// numAnswer's rows
				for(int i = 0; i < numAnswers; ++i) {
					toret.Append( "<tr><td style='background-color:lightgray'>" );
					toret.Append( (char) ( 'a' + i ) );
					toret.Append( "</td>" );
					for(int j = numQuestionsDrawn; j < maxQuestionsToPrintThisTime; ++j) {
						toret.Append( "<td>&nbsp;</td>" );
					}

					toret.AppendLine( "</tr>" );
					toret.AppendLine();
				}
				toret.AppendLine( "</table>" );
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

			while( numQuestionsDrawn < numQuestions ) {
				int maxQuestionsToPrintThisTime = 
					Math.Min( numQuestionsDrawn + MaxQuestionsPerLine, questions.Count );

				// Header
				toret.AppendLine( "<table width='80%' border='1'>"  );
				toret.Append( "<tr><td>&nbsp</td>" );
				for(int i = numQuestionsDrawn; i < maxQuestionsToPrintThisTime; ++i) {
					toret.AppendFormat( "<td style='background-color:lightgray'>{0}</td>", i + 1 );
				}
				toret.AppendLine();
				toret.AppendLine( "</tr>" );

				// numAnswer's rows
				for(int i = 0; i < numAnswers; ++i) {
					toret.Append( "<tr><td style='background-color:lightgray'>" );
					toret.Append( (char) ( 'a' + i ) );
					toret.Append( "</td>" );
					for(int j = numQuestionsDrawn; j < maxQuestionsToPrintThisTime; ++j) {
						toret.Append( "<td>" );
						if ( questions[ j ].CorrectAnswer == i ) {
							toret.Append( "X" );
						} else {
							toret.Append( "&nbsp;" );
						}
						toret.Append( "</td>" );
					}

					toret.AppendLine( "</tr>" );
					toret.AppendLine();
				}
				toret.AppendLine( "</table>" );
				numQuestionsDrawn += maxQuestionsToPrintThisTime;
			}

			toret.AppendLine();
			return toret.ToString();
		}
	}
}

