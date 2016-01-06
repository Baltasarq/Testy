using System;
using System.IO;
using System.Xml;

namespace Testy.Core {
	/// <summary>
	/// This is the native file format, Xml.
	/// </summary>
	public class XmlExporter: Exporter {
		public XmlExporter(Document doc, string fileName)
			:base( doc, fileName )
		{
		}

		public override void Export()
		{
			XmlDocument xdoc = this.ToXml();

			// Write Xml Document to string
			using (var stringWriter = new StringWriter() ) {
				using (var xmlTextWriter = XmlWriter.Create( stringWriter ) )
				{
					xdoc.WriteTo( xmlTextWriter );
					xmlTextWriter.Flush();
					this.WriteLine( stringWriter.GetStringBuilder().ToString() );
				}
			}

			return;
		}

		/// <summary>
		/// Converts all question information to a propietary Xml format
		/// </summary>
		/// <returns>
		/// The resulting xml document.
		/// </returns>
		public XmlDocument ToXml()
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
			foreach(Question q in this.Document) {
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
	}
}
