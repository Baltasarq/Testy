// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Core;


using System.Text;
using System.Collections;
using System.Collections.ObjectModel;


/// <summary>
/// A document test is holding some information, and specially, all questions of the test.
/// </summary>
public class Document: IDisposable, IEnumerable<Question> {
	public Document()
	{
		this.Date = DateTime.Now;
		this.Title = "Test";

		// Prepare questions
		this.questions = new List<Question>();
		this.AddDefaultQuestion();
	}

	/// <summary>
	/// Save the document in the specified fmt and fileName.
	/// </summary>
	/// <param name='fmt'>
	/// The format of the file (Xml, Html...).
	/// </param>
	/// <param name='fileName'>
	/// The file name, as a string.
	/// </param>
	/// <exception cref='ArgumentException'>
	/// Thrown when the format is invalid.
	/// </exception>
	/// <seealso cref="Testy.Core.Exporter"/>
	public void Save(Transformer.Format fmt, string fileName) {
		Exporter.Save( this, fmt, fileName );
	}

	/// <summary>
	/// Imports the test from a file, in a given format
	/// </summary>
	/// <param name = "fmt">The format of the file (Xml, Html...).</param>
	/// <param name="fileName">The name of the file, as a string.</param>
	/// <seealso cref="Testy.Core.Importer"/>
	public static Document Load(Transformer.Format fmt, string fileName)
	{
		return Importer.Load( fmt, fileName );
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

	/// <summary>
	/// Adds the given range of questions.
	/// </summary>
	/// <param name="vq">A collection of questions.</param>
	public void AddRange(IEnumerable<Question> vq)
	{
		this.questions.AddRange( vq );
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
			throw new ArgumentException( "Invalid question index: " + index );
		}
	}

	/// <summary>
	/// Adds the default question (so there is at least one question).
	/// </summary>
	private void AddDefaultQuestion()
	{
		this.Add( "Q?" );
	}

	/// <summary>
	/// Shuffles all questions
	/// </summary>
	public void Shuffle()
	{
		int target = this.questions.Count;
		var shuffledQuestions = new List<Question>( target );

		// Create a shuffled questions list
		foreach (int n in new RandomSequence( target ).Sequence) {
			Question q = this.questions[ n ];

			q.Shuffle();
			shuffledQuestions.Add( q );
		}

		this.questions = shuffledQuestions;
		return;
	}

	/// <summary>
	/// Clear all questions.
	/// </summary>
	public void Clear() {
		this.questions.Clear();
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
		get; private set;
	}

	/// <summary>
	/// Gets or sets the notes related to the document test.
	/// </summary>
	/// <value>
	/// The notes, as a string.
	/// </value>
	public string Notes {
		get; set;
	} = string.Empty;

	public void Dispose() {
		this.questions.Clear();
	}

	private List<Question> questions;
}
