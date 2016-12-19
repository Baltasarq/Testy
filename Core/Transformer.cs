using System;
using System.Collections.ObjectModel;

namespace Testy.Core {
	/// <summary>
	/// Parent class for importers and exporters
	/// </summary>
	public abstract class Transformer {
		public const string TestFileExt = ".tst";

		public const string XmlLblTest = "test";
		public const string XmlLblQuestion = "question";
		public const string XmlLblText = "text";
		public const string XmlLblAnswer = "answer";
		public const string XmlLblCorrect = "correct";

		public const string FileNamePrefixOptions = "options_";
		public const string FileNamePrefixAnswers = "answers_";

		public enum Format { Text, Html, Csv, Excel, Xml };
		public static ReadOnlyCollection<string> FormatExt = new ReadOnlyCollection<string>(
			new []{ ".txt", ".html", ".csv", ".xls", TestFileExt }
		);

		protected Transformer(string fileName)
		{
			this.FileName = fileName;
		}

		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public string FileName {
			get;
			protected set;
		}
	}
}

