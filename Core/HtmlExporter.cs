using System;
using System.Text;
using System.Web;

namespace Testy.Core {
	/// <summary>
	/// Exports a given document to a HTML format.
	/// </summary>
	public class HtmlExporter: Exporter {
		public HtmlExporter(Document doc, string fileName)
			:base( doc, fileName )
		{
		}
        
        /// <summary>
        /// Converts the given question in the document into html.
        /// </summary>
        /// <returns>
        /// <param name="question">The <see cref="Question"/> to convert.</param>
        /// The html code in a string.
        /// </returns>
        private string FromQuestionToHtml(Question question)
        {
            var toret = new StringBuilder();
            
            // Add the question's text
            toret.Append( "<li>" );
            toret.AppendLine( HttpUtility.HtmlEncode( question.Text ) );
            
            // Add all the answers
            for(int i = 0; i < question.CountAnswers; ++i) {
                toret.Append( "<p>&nbsp;&nbsp;&nbsp;&nbsp;" );
                toret.Append( (char) ( 'a' + i ) );
                toret.Append( ") " );
                toret.Append( HttpUtility.HtmlEncode( question.GetAnswer( i ) ) );
                toret.AppendLine( "</p>" );
            }

            toret.Append( "</li>" );
            toret.AppendLine( "<p/>" );
            return toret.ToString();
        }
        
        /// <summary>
        /// Converts the document into HTML.
        /// </summary>
        /// <returns>
        /// The html code in a string.
        /// </returns>
        private string FromDocumentToHtml()
        {
            var toret = new StringBuilder();

            toret.AppendLine( "<h1>"
                                + HttpUtility.HtmlEncode( this.Document.Title )
                                + "</h1>" );
            toret.AppendLine( "<h2>"
                                + HttpUtility.HtmlEncode( 
                                    this.Document.Date.Date.ToShortDateString() )
                                + "</h2>" );
            toret.AppendLine( "<p>"
                                + HttpUtility.HtmlEncode( this.Document.Notes )
                                + "</p>" );
            toret.AppendLine( "<ol>" );
            
            foreach (var q in this.Document) {
                toret.AppendLine( FromQuestionToHtml( q ) );
            }

            toret.AppendLine( "</ol>" );
            return toret.ToString();
        }

        ///<summary>Exports the <see cref="Document"/> to HTML.</summary>        
		public override void Export()
		{
			this.WriteLine( "<html><head>" );
			this.WriteLine( "<meta charset='utf-8' />" );
			this.WriteLine( "</head><body>" );

			this.WriteLine( "<p><h2>Answers</h2>" );
			this.WriteLine( this.DrawAnswersBox() );
			this.WriteLine( "</p>" );

			this.WriteLine( "<p><h2>Questions</h2>" );
			this.WriteLine( this.FromDocumentToHtml() );
			this.WriteLine( "</p>" );

			this.WriteLine( "<p><h2>Solution</h2>" );
			this.WriteLine( this.DrawSolutionBox() );
			this.WriteLine( "</p>" );

			this.WriteLine( "</body></html>" );
		}

        /// <summary>Draws the answers box as HTML.</summary>
		private string DrawAnswersBox()
        {
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
					toret.AppendFormat( "<td style='background-color: lightgray'>{0}</td>", i + 1 );
				}
				toret.AppendLine();
				toret.AppendLine( "</tr>" );

				// numAnswer's rows
				for(int i = 0; i < numAnswers; ++i) {
					toret.Append( "<tr><td style='background-color: lightgray'>" );
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

        /// <summary>Draws the solution box as HTML.</summary>
		private string DrawSolutionBox()
        {
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
					toret.AppendFormat( "<td style='background-color: lightgray'>{0}</td>", i + 1 );
				}
				toret.AppendLine();
				toret.AppendLine( "</tr>" );

				// numAnswer's rows
				for(int i = 0; i < numAnswers; ++i) {
					toret.Append( "<tr><td style='background-color: lightgray'>" );
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

