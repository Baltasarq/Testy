using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Testy.Core {

	/// <summary>
	/// A document test is holding some information, and specially, all questions of the test.
	/// </summary>
    public class Document: IEnumerable<Question>
    {	
		public const string TestFileExt = ".tst";
		
		public const string FileNamePrefixQuestions = "questions_";
		public const string FileNamePrefixOptions = "options_";
		public const string FileNamePrefixAnswers = "answers_";
		
		public const string XmlLblTest = "test";
		public const string XmlLblQuestion = "question";
		public const string XmlLblText = "text";
		public const string XmlLblAnswer = "answer";
		public const string XmlLblCorrect = "correct";

		public const string XmlLblNamespace = "xmlns";
		public const string ExcelXmlLblWorksheet = "Worksheet";
		public const string ExcelXmlLblWorkbook = "Workbook";
		public const string ExcelXmlLblTable = "Table";
		public const string ExcelXmlLblRow = "Row";
		public const string ExcelXmlLblData = "Data";
		public const string ExcelXmlLblCell = "Cell";
		public const string ExcelXmlLblSelector = "ss";
		public const string ExcelXmlLblType = "Type";
		public const string ExcelXmlLblString = "String";
		public const string ExcelXmlLblNumber = "Number";
		public const string ExcelXmlLblName = "Name";
		
		public enum Format { Text, Html, Csv, /* Excel,*/ Xml };
		
		public static ReadOnlyCollection<string> FormatExt = new ReadOnlyCollection<string>(
		      new string[]{ ".txt", ".html", ".csv", /*".xls",*/ TestFileExt }
		);

		
        public Document()
        {
			this.Date = DateTime.Now;
			this.Title = "Test";
			
			// Prepare questions
			this.questions = new List<Question>();
			this.AddDefaultQuestion();
        }
		
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
			using (StreamWriter outfile = new StreamWriter( fileName ) )
	        {
				outfile.Write( contents );
	        }
		}
		
		protected static string BuildCsvFileName(string prefix, string fileName )
		{
			// Decompose
			var fileExt = System.IO.Path.GetExtension( fileName );
			var name = System.IO.Path.GetFileNameWithoutExtension( fileName );
			var fileDir = System.IO.Path.GetDirectoryName( fileName );
			
			// Add the prefix
			name = prefix + name;
			name += fileExt;
			
			return ( Path.Combine( fileDir, name ) );
		}
		
		/// <summary>
		/// Save the document in the specified fmt and fileName.
		/// </summary>
		/// <param name='fmt'>
		/// The format of the file.
		/// </param>
		/// <param name='fileName'>
		/// The file name, as a string.
		/// </param>
		/// <exception cref='ArgumentException'>
		/// Thrown when the format is invalid.
		/// </exception>
		public void Save(Format fmt, string fileName)
		{
			if ( fmt == Format.Text ) {
				SaveTextFile( fileName, this.ToString() );
			}
			else
			if ( fmt == Format.Html ) {
				SaveTextFile( fileName, this.ToHtml() );
			}
			else
			if ( fmt == Format.Csv ) {
				SaveTextFile( BuildCsvFileName( FileNamePrefixQuestions, fileName ), this.GetQuestionsAsCsv() );
				SaveTextFile( BuildCsvFileName( FileNamePrefixOptions, fileName ), this.GetOptionsAsCsv() );
				SaveTextFile( BuildCsvFileName( FileNamePrefixAnswers, fileName ), this.GetAnswersAsCsv() );
			}
/*			else
			if ( fmt == Format.Excel ) {
				string contents;
				XmlDocument doc = this.ToExcel();

				// Convert Xml Document to string
				using ( var stringWriter = new StringWriter() ) {
					using ( var xmlTextWriter = XmlWriter.Create( stringWriter ) )
					{
					    doc.WriteTo( xmlTextWriter );
					    xmlTextWriter.Flush();
					    contents = stringWriter.GetStringBuilder().ToString();
					}
				}
				
				SaveTextFile( fileName, contents );
			}
*/
			else
			if ( fmt == Format.Xml ) {
				string contents;
				XmlDocument doc = this.ToXml();
				
				// Convert Xml Document to string
				using ( var stringWriter = new StringWriter() ) {
					using ( var xmlTextWriter = XmlWriter.Create( stringWriter ) )
					{
					    doc.WriteTo( xmlTextWriter );
					    xmlTextWriter.Flush();
					    contents = stringWriter.GetStringBuilder().ToString();
					}
				}
				
				SaveTextFile( fileName, contents );
			}
			else {
				throw new ArgumentException( "unrecognized format option" );
			}
			
			return;
		}
		
		/// <summary>
		/// Creates a new document, loading its contents from a XML file.
		/// </summary>
		/// <param name='fileName'>
		/// The file name, as a string.
		/// </param>
		public static Document Load(string fileName)
		{
			Document toret = new Document();
			
			// Load the xml document
			var doc = new XmlDocument();
			doc.Load( fileName );
			
			// Read all questions
			XmlNodeList qlist = doc.GetElementsByTagName( XmlLblQuestion );
			foreach(XmlElement qnode in qlist)
			{
				var q = new Question();
				XmlNodeList answerList = qnode.GetElementsByTagName( XmlLblAnswer );
				XmlNodeList textList = qnode.GetElementsByTagName( XmlLblText );
				
				// Get text
				if ( textList.Count == 1 ) {
					q.Text = textList[ 0 ].InnerText;
				} else {
					throw new XmlException( "nonsense: more than one text label" );
				}
				
				// Get answers
				int numAnswer = 0;
				foreach(XmlElement answerNode in answerList) {
					XmlAttribute correct = answerNode.GetAttributeNode( XmlLblCorrect );
					
					if ( correct != null )
					{
						// Store new answer
						q.AddAnswer( answerNode.InnerText );
						if ( Convert.ToInt32( correct.Value ) > 0 ) {
							q.CorrectAnswer = numAnswer;
						}
					} else {
						throw new XmlException( "missing attributes in answer" );
					}
					
					++numAnswer;
				}
				
				// Store question, erasing the default answer
				q.RemoveAnswer( 0 );
				toret.Add( q );
			}
			
            // Erase the default question
            toret.RemoveAt( 0 );
			return toret;
		}
		
		/// <summary>
		/// Gets or sets the questions at the same time, as a ReadOnlyCollection.
		/// </summary>
		/// <value>
		/// The questions, ReadOnlyCollection<Question>.
		/// </value>
		public ReadOnlyCollection<Question> Questions {
			get {
				return new ReadOnlyCollection<Question>( this.questions.ToArray() );
			}
			set {
				this.questions.Clear();
				this.questions.AddRange( value );
			}
		}
		
		/// <summary>
		/// Gets or sets the <see cref="Testy.Core.Question"/> with the specified index #i.
		/// </summary>
		/// <param name='i'>
		/// The index of the question
		/// </param>
		public Question this[int i]
		{
			get {
				return this.questions[ i ];
			}
			set {
				this.questions[ i ] = value;
			}
		}
		
		/// <summary>
		/// Gets the count of all questions in the document.
		/// </summary>
		/// <value>
		/// The questions count, as an int.
		/// </value>
		public int CountQuestions {
			get {
				return this.questions.Count;
			}
		}
		
		/// <summary>
		/// Add the specified question.
		/// </summary>
		/// <param name='questionTxt'>
		/// The question text. A question with that text and no answers is created.
		/// </param>
		public void Add(string questionTxt)
		{
			this.Add( new Question( questionTxt ) );
		}
		
		/// <summary>
		/// Add the specified question to the document.
		/// </summary>
		/// <param name='q'>
		/// A question, in the form of a Question object.
		/// </param>
		public void Add(Question q)
		{
			this.questions.Add( q );
		}
		
		public void RemoveAt(int index)
		{
			if ( index >= 0
			  && index < this.CountQuestions )
			{
				this.questions.RemoveAt( index );
				
				if ( this.CountQuestions == 0 ) {
					this.AddDefaultQuestion();
				}
			} else {
				throw new ArgumentException( "Invalid question index: " + index.ToString() );
			}
		}
		
		/// <summary>
		/// Adds the default question (so there is at least one question).
		/// </summary>
		protected void AddDefaultQuestion()
		{
			this.Add( "Q?" );
		}
		
		IEnumerator<Question> IEnumerable<Question>.GetEnumerator()
		{
			foreach(var c in this.questions) {
				yield return c;
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach(var c in this.questions) {
				yield return c;
			}
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Testy.Core.Document"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Testy.Core.Document"/>.
		/// </returns>
		public override string ToString()
        {
            int numQuestion = 1;
            var toret = new StringBuilder();
			
            foreach (var q in this) {
                toret.Append( String.Format( "{0, 4}", numQuestion ) );
                toret.Append( ". " );
				toret.Append( q.ToString() );
				toret.AppendLine();

                ++numQuestion;
			}
			
			return toret.ToString();
		}
		
		/// <summary>
		/// Creates the excel table for questions, inside a worksheet node.
		/// </summary>
		/// <param name='wsQuestions'>
		/// The worksheet node for questions.
		/// </param>
		protected void CreateExcelTableForQuestions(XmlNode wsQuestions)
		{
			int numQuestion = 1;
			
			var tblQuestions = wsQuestions.OwnerDocument.CreateElement( ExcelXmlLblTable );
			wsQuestions.AppendChild( tblQuestions );
			
			foreach(var q in this) {
				var row = wsQuestions.OwnerDocument.CreateElement( ExcelXmlLblRow );
				tblQuestions.AppendChild( row );
				
				// Question number
				var cellForQuestionNumber = wsQuestions.OwnerDocument.CreateElement( ExcelXmlLblCell );
				row.AppendChild( cellForQuestionNumber );
				
				var dataForQuestionNumber = wsQuestions.OwnerDocument.CreateElement( ExcelXmlLblData );
				dataForQuestionNumber.SetAttribute( ExcelXmlLblSelector + ':' + ExcelXmlLblType, ExcelXmlLblNumber );
				cellForQuestionNumber.AppendChild( dataForQuestionNumber );
				
				dataForQuestionNumber.InnerText = numQuestion.ToString();
				
				// Question text
				var cellForQuestionText = wsQuestions.OwnerDocument.CreateElement( ExcelXmlLblCell );
				row.AppendChild( cellForQuestionText );
				
				var dataForQuestionText = wsQuestions.OwnerDocument.CreateElement( ExcelXmlLblData );
				dataForQuestionText.SetAttribute( ExcelXmlLblType, ExcelXmlLblSelector, ExcelXmlLblString );
//				dataForQuestionText.SetAttribute( ExcelXmlLblSelector + ':' + ExcelXmlLblType, ExcelXmlLblString );
				cellForQuestionText.AppendChild( dataForQuestionText );
				
				dataForQuestionText.InnerText = q.Text;

				++numQuestion;
			}
			
			return;
		}
		
		/// <summary>
		/// Creates the excel table for answers, inside a worksheet node.
		/// </summary>
		/// <param name='wsAnswers'>
		/// The worksheet node for answers.
		/// </param>
		protected void CreateExcelTableForAnswers(XmlNode wsAnswers)
		{
			int numQuestion = 1;
			
			var tblAnswers = wsAnswers.OwnerDocument.CreateElement( ExcelXmlLblTable );
			wsAnswers.AppendChild( tblAnswers );
			
			foreach(var q in this) {
				for(int answerNumber = 0; answerNumber < q.CountAnswers; ++answerNumber) {
					var row = wsAnswers.OwnerDocument.CreateElement( ExcelXmlLblRow );
					tblAnswers.AppendChild( row );
					
					// Question number
					var cellForQuestionNumber = wsAnswers.OwnerDocument.CreateElement( ExcelXmlLblCell );
					row.AppendChild( cellForQuestionNumber );
					
					var dataForQuestionNumber = wsAnswers.OwnerDocument.CreateElement( ExcelXmlLblData );
					dataForQuestionNumber.SetAttribute( ExcelXmlLblSelector, ExcelXmlLblType, ExcelXmlLblNumber );
					cellForQuestionNumber.AppendChild( dataForQuestionNumber );
					
					dataForQuestionNumber.InnerText = numQuestion.ToString();
					
					// Answer number
					var cellForAnswerIndex = wsAnswers.OwnerDocument.CreateElement( ExcelXmlLblCell );
					row.AppendChild( cellForAnswerIndex );
					
					var dataForAnswerIndex = wsAnswers.OwnerDocument.CreateElement( ExcelXmlLblData );
					dataForAnswerIndex.SetAttribute( ExcelXmlLblSelector, ExcelXmlLblType, ExcelXmlLblNumber );
					cellForAnswerIndex.AppendChild( dataForAnswerIndex );
					
					dataForAnswerIndex.InnerText = ( answerNumber +1 ).ToString();
					
					// Answer text
					var cellForAnswerText = wsAnswers.OwnerDocument.CreateElement( ExcelXmlLblCell );
					row.AppendChild( cellForAnswerText );
					
					var dataForAnswerText = wsAnswers.OwnerDocument.CreateElement( ExcelXmlLblData );
					dataForAnswerText.SetAttribute( ExcelXmlLblSelector, ExcelXmlLblType, ExcelXmlLblString );
					cellForAnswerText.AppendChild( dataForAnswerText );
					
					dataForAnswerText.InnerText = q.Answers[ answerNumber ];
				}

				++numQuestion;
			}
			
			return;
		}
		
		/// <summary>
		/// Creates the excel table for correct answers, inside a worksheet node.
		/// </summary>
		/// <param name='wsCorrectAnswers'>
		/// The worksheet node for correct answers.
		/// </param>
		protected void CreateExcelTableForCorrectAnswers(XmlNode wsCorrectAnswers)
		{
			int numQuestion = 1;
			
			var tblQuestions = wsCorrectAnswers.OwnerDocument.CreateElement( ExcelXmlLblTable );
			wsCorrectAnswers.AppendChild( tblQuestions );
			
			foreach(var q in this) {
				var row = wsCorrectAnswers.OwnerDocument.CreateElement( ExcelXmlLblRow );
				tblQuestions.AppendChild( row );
				
				// Question number
				var cellForQuestionNumber = wsCorrectAnswers.OwnerDocument.CreateElement( ExcelXmlLblCell );
				row.AppendChild( cellForQuestionNumber );
				
				var dataForQuestionNumber = wsCorrectAnswers.OwnerDocument.CreateElement( ExcelXmlLblData );
				dataForQuestionNumber.SetAttribute( ExcelXmlLblSelector, ExcelXmlLblType, ExcelXmlLblNumber );
				cellForQuestionNumber.AppendChild( dataForQuestionNumber );
				
				dataForQuestionNumber.InnerText = numQuestion.ToString();
				
				// Correct answer number
				var cellForCorrectAnswerNumber = wsCorrectAnswers.OwnerDocument.CreateElement( ExcelXmlLblCell );
				row.AppendChild( cellForCorrectAnswerNumber );
				
				var dataForCorrectAnswerNumber = wsCorrectAnswers.OwnerDocument.CreateElement( ExcelXmlLblData );
				dataForCorrectAnswerNumber.SetAttribute( ExcelXmlLblSelector,ExcelXmlLblType, ExcelXmlLblNumber );
				cellForCorrectAnswerNumber.AppendChild( dataForCorrectAnswerNumber );
				
				dataForCorrectAnswerNumber.InnerText = ( q.CorrectAnswer +1 ).ToString();

				++numQuestion;
			}
			
			return;
		}
		
		/// <summary>
		/// Returns a <see cref="System.Xml.XmlDocument"/> that represents the current <see cref="Testy.Core.Document"/>.
		/// as an excel 2003 (xml, .xls) document.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Xml.XmlDocument"/> that represents the current <see cref="Testy.Core.Document"/>.
		/// </returns>
		public XmlDocument ToExcel()
        {
            var toret = new XmlDocument();
			
			// Add encoding declaration
			XmlDeclaration xmlDeclaration = toret.CreateXmlDeclaration( "1.0", "utf-8", null);
			toret.AppendChild( xmlDeclaration );
			
			// Add root label
			var root = toret.CreateElement( ExcelXmlLblWorkbook );
			toret.AppendChild( root );
//			root.SetAttribute( XmlLblNamespace, "urn:schemas-microsoft-com:office:spreadsheet" );
			root.SetAttribute( XmlLblNamespace + ":o", "urn:schemas-microsoft-com:office:office" );
			root.SetAttribute( XmlLblNamespace + ":x", "urn:schemas-microsoft-com:office:excel" );
			root.SetAttribute( XmlLblNamespace + ':' + ExcelXmlLblSelector, "urn:schemas-microsoft-com:office:spreadsheet" );
			root.SetAttribute( XmlLblNamespace + ":html", "http://www.w3.org/TR/REC-html40" );
			
			// Add first worksheet
			var wsQuestions = toret.CreateElement( ExcelXmlLblWorksheet );
			wsQuestions.SetAttribute( ExcelXmlLblSelector + ':' + ExcelXmlLblName, "Questions" );
			root.AppendChild( wsQuestions );
			
			this.CreateExcelTableForQuestions( wsQuestions );
			
			// Add second worksheet
			var wsAnswers = toret.CreateElement( ExcelXmlLblWorksheet );
			wsAnswers.SetAttribute( ExcelXmlLblSelector + ':' + ExcelXmlLblName, "Answers" );
			root.AppendChild( wsAnswers );
			
            this.CreateExcelTableForAnswers( wsAnswers );
			
			// Add third worksheet
			var wsCorrect = toret.CreateElement( ExcelXmlLblWorksheet );
			wsCorrect.SetAttribute( ExcelXmlLblSelector + ':' + ExcelXmlLblName, "Correct answers" );
			root.AppendChild( wsCorrect );
			
			this.CreateExcelTableForCorrectAnswers( wsCorrect );

			return toret;
		}
		
		/// <summary>
		/// Converts all question information to a propietary Xml format
		/// </summary>
		/// <returns>
		/// The resulting xml document.
		/// </returns>
		protected XmlDocument ToXml()
		{
			int questionNumber = 1;
			var toret = new XmlDocument();
			var root = toret.CreateElement( XmlLblTest );
			
			// Add encoding declaration
			XmlDeclaration xmlDeclaration = toret.CreateXmlDeclaration( "1.0", "utf-8", null);
			toret.AppendChild( xmlDeclaration );
			
			// Add main node
			toret.AppendChild( root );
			
			// Add one node for each question
			foreach(var q in this) {
				// Create the node for this question
				var questionNode = toret.CreateElement( XmlLblQuestion );
				root.AppendChild( questionNode );

				// Create the text subnode for this question
				var textNode = toret.CreateElement( XmlLblText );
				questionNode.AppendChild( textNode );
				textNode.InnerText = q.Text;
				
				for(int answerNumber = 0; answerNumber < q.CountAnswers; ++answerNumber) {
					var answerNode = toret.CreateElement( XmlLblAnswer );
					questionNode.AppendChild( answerNode );
					
					// Store answer
					answerNode.InnerText = q.Answers[ answerNumber ];
					var attrCorrect = toret.CreateAttribute( XmlLblCorrect );
					attrCorrect.InnerText = ( ( answerNumber == q.CorrectAnswer ) ? 1: 0 ).ToString();
					answerNode.Attributes.Append( attrCorrect );
				}
				
				++questionNumber;
			}
			
			
			return toret;
		}

        /// <summary>
        /// Generates a html version.
        /// </summary>
        /// <returns>
        /// The html code in a string.
        /// </returns>
        public string ToHtml()
        {
            var toret = new StringBuilder();

            toret.AppendLine( "<html><head>" );
            toret.AppendLine( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            toret.AppendLine( "</head><body>" );
            toret.AppendLine( "<h1>" + this.Title + "</h1>" );
            toret.AppendLine( "<h2>" + this.Date.Date.ToShortDateString() + "</h2>" );
            toret.AppendLine( "<p>" + this.Notes + "</p>" );
            toret.AppendLine( "<ol>" );
            
            foreach (var q in this) {
                toret.AppendLine( q.ToHtml() );
            }

            toret.AppendLine( "</ol>" );
            toret.AppendLine( "</body></html>" );
            return toret.ToString();
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
			
			foreach(var q in this) {
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
			
			foreach(var q in this) {
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
			
			foreach(var q in this) {
				toret.Append( numQuestion );
				toret.Append( ',' );
				toret.Append( q.CorrectAnswer +1 );
				toret.AppendLine();
				
				++numQuestion;
			}
			
			return toret.ToString();
		}
				
		/// <summary>
		/// Gets or sets the title of the document test.
		/// </summary>
		/// <value>
		/// The title, as a string.
		/// </value>
		public string Title {
			get; set;
		}
		
		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		/// <value>
		/// The date, as a DateTime object.
		/// </value>
		public DateTime Date {
			get; set;
		}
		
		/// <summary>
		/// Gets or sets the notes related to the document test.
		/// </summary>
		/// <value>
		/// The notes, as a string.
		/// </value>
		public string Notes {
			get; set;
		}
		
		private List<Question> questions;
    }
}

