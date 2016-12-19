using System;
using System.Xml;

namespace Testy.Core {
	public class XmlImporter: Importer {
		public XmlImporter(string fileName)
			:base( fileName )
		{
		}

		/// <summary>
		/// Creates a new document, loading its contents from a XML file.
		/// </summary>
		public override Document Import()
		{
			var toret = new Document();

			toret.Clear();

			// Load the xml document
			var doc = new XmlDocument();
			doc.Load( this.FileName );

			// Read all questions
			XmlNodeList qlist = doc.GetElementsByTagName( XmlLblQuestion );
			foreach(XmlElement qnode in qlist)
			{
				var q = new Question();
				q.ClearAnswers();
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

				// Store question
				toret.Add( q );
			}

			return toret;
		}
	}
}

