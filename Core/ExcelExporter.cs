// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Core;


using System.Xml;


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
		using var stringWriter = new StringWriter();
		using var xmlTextWriter = XmlWriter.Create( stringWriter );

		doc.WriteTo( xmlTextWriter );
		xmlTextWriter.Flush();
		this.WriteLine( stringWriter.GetStringBuilder().ToString() );

		return;
	}

	/// <summary>
	/// Creates the excel table for questions, inside a worksheet node.
	/// </summary>
	/// <param name="doc">The output xml document.</param>
	/// <param name='wsQuestions'>The worksheet node for questions.</param>
	private void CreateExcelTableForQuestions(XmlDocument doc, XmlNode wsQuestions)
	{
		int numQuestion = 1;

		var tblQuestions = doc.CreateElement( ExcelXmlLblTable );
		wsQuestions.AppendChild( tblQuestions );

		foreach(Question q in this.Document) {
			var row = doc.CreateElement( ExcelXmlLblRow );
			tblQuestions.AppendChild( row );

			// Question number
			var cellForQuestionNumber = doc.CreateElement( ExcelXmlLblCell );
			row.AppendChild( cellForQuestionNumber );

			var dataForQuestionNumber = doc.CreateElement( ExcelXmlLblData );
			dataForQuestionNumber.SetAttribute( ExcelXmlLblSelector + ':' + ExcelXmlLblType, ExcelXmlLblNumber );
			cellForQuestionNumber.AppendChild( dataForQuestionNumber );

			dataForQuestionNumber.InnerText = numQuestion.ToString();

			// Question text
			var cellForQuestionText = doc.CreateElement( ExcelXmlLblCell );
			row.AppendChild( cellForQuestionText );

			var dataForQuestionText = doc.CreateElement( ExcelXmlLblData );
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
	/// <param name="doc">The output xml document.</param>
	/// <param name='wsAnswers'>The worksheet node for answers.</param>
	protected void CreateExcelTableForAnswers(XmlDocument doc, XmlNode wsAnswers)
	{
		int numQuestion = 1;

		var tblAnswers = doc.CreateElement( ExcelXmlLblTable );
		wsAnswers.AppendChild( tblAnswers );

		foreach(Question q in this.Document) {
			for(int answerNumber = 0; answerNumber < q.CountAnswers; ++answerNumber) {
				var row = doc.CreateElement( ExcelXmlLblRow );
				tblAnswers.AppendChild( row );

				// Question number
				var cellForQuestionNumber = doc.CreateElement( ExcelXmlLblCell );
				row.AppendChild( cellForQuestionNumber );

				var dataForQuestionNumber = doc.CreateElement( ExcelXmlLblData );
				dataForQuestionNumber.SetAttribute( ExcelXmlLblSelector, ExcelXmlLblType, ExcelXmlLblNumber );
				cellForQuestionNumber.AppendChild( dataForQuestionNumber );

				dataForQuestionNumber.InnerText = numQuestion.ToString();

				// Answer number
				var cellForAnswerIndex = doc.CreateElement( ExcelXmlLblCell );
				row.AppendChild( cellForAnswerIndex );

				var dataForAnswerIndex = doc.CreateElement( ExcelXmlLblData );
				dataForAnswerIndex.SetAttribute( ExcelXmlLblSelector, ExcelXmlLblType, ExcelXmlLblNumber );
				cellForAnswerIndex.AppendChild( dataForAnswerIndex );

				dataForAnswerIndex.InnerText = ( answerNumber +1 ).ToString();

				// Answer text
				var cellForAnswerText = doc.CreateElement( ExcelXmlLblCell );
				row.AppendChild( cellForAnswerText );

				var dataForAnswerText = doc.CreateElement( ExcelXmlLblData );
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
	/// <param name="doc">The output xml document.</param>
	/// <param name='wsCorrectAnswers'>The worksheet node for correct answers.</param>
	protected void CreateExcelTableForCorrectAnswers(XmlDocument doc, XmlNode wsCorrectAnswers)
	{
		int numQuestion = 1;

		var tblQuestions = doc.CreateElement( ExcelXmlLblTable );

		wsCorrectAnswers.AppendChild( tblQuestions );

		foreach(Question q in this.Document) {
			var row = doc.CreateElement( ExcelXmlLblRow );
			tblQuestions.AppendChild( row );

			// Question number
			var cellForQuestionNumber = doc.CreateElement( ExcelXmlLblCell );
			row.AppendChild( cellForQuestionNumber );

			var dataForQuestionNumber = doc.CreateElement( ExcelXmlLblData );
			dataForQuestionNumber.SetAttribute( ExcelXmlLblSelector, ExcelXmlLblType, ExcelXmlLblNumber );
			cellForQuestionNumber.AppendChild( dataForQuestionNumber );

			dataForQuestionNumber.InnerText = numQuestion.ToString();

			// Correct answer number
			var cellForCorrectAnswerNumber = doc.CreateElement( ExcelXmlLblCell );
			row.AppendChild( cellForCorrectAnswerNumber );

			var dataForCorrectAnswerNumber = doc.CreateElement( ExcelXmlLblData );
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
		XmlDeclaration xmlDeclaration = toret.CreateXmlDeclaration( "1.0", "utf-8", null );
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

		this.CreateExcelTableForQuestions( toret, wsQuestions );

		// Add second worksheet
		var wsAnswers = toret.CreateElement( ExcelXmlLblWorksheet );
		wsAnswers.SetAttribute( ExcelXmlLblSelector + ':' + ExcelXmlLblName, "Answers" );
		root.AppendChild( wsAnswers );

		this.CreateExcelTableForAnswers( toret, wsAnswers );

		// Add third worksheet
		var wsCorrect = toret.CreateElement( ExcelXmlLblWorksheet );
		wsCorrect.SetAttribute( ExcelXmlLblSelector + ':' + ExcelXmlLblName, "Correct answers" );
		root.AppendChild( wsCorrect );

		this.CreateExcelTableForCorrectAnswers( toret, wsCorrect );

		return toret;
	}
}
