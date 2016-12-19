using System.IO;
using System.Xml;

namespace Testy.Core {
	public class ExcelExporter: Exporter {
		public const string ExcelXmlLblNamespace = "xmlns";
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

		public ExcelExporter(Document doc, string fileName)
			:base( doc, fileName )
		{
		}

		public override void Export()
		{
			XmlDocument doc = this.ToExcel();

			// Convert Xml Document to string
			using ( var stringWriter = new StringWriter() ) {
				using ( var xmlTextWriter = XmlWriter.Create( stringWriter ) )
				{
					doc.WriteTo( xmlTextWriter );
					xmlTextWriter.Flush();
					this.WriteLine( stringWriter.GetStringBuilder().ToString() );
				}
			}

			return;
		}

		/// <summary>
		/// Creates the excel table for questions, inside a worksheet node.
		/// </summary>
		/// <param name='wsQuestions'>
		/// The worksheet node for questions.
		/// </param>
		private void CreateExcelTableForQuestions(XmlNode wsQuestions)
		{
			int numQuestion = 1;

			var tblQuestions = wsQuestions.OwnerDocument.CreateElement( ExcelXmlLblTable );
			wsQuestions.AppendChild( tblQuestions );

			foreach(Question q in this.Document) {
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

			foreach(Question q in this.Document) {
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

			foreach(Question q in this.Document) {
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
			root.SetAttribute( ExcelXmlLblNamespace + ":o", "urn:schemas-microsoft-com:office:office" );
			root.SetAttribute( ExcelXmlLblNamespace + ":x", "urn:schemas-microsoft-com:office:excel" );
			root.SetAttribute( ExcelXmlLblNamespace + ':' + ExcelXmlLblSelector, "urn:schemas-microsoft-com:office:spreadsheet" );
			root.SetAttribute( ExcelXmlLblNamespace + ":html", "http://www.w3.org/TR/REC-html40" );

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
	}
}

