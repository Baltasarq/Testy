// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


using System.Collections.ObjectModel;

using Core;


public partial class DlgTakeTest {
	/// <summary>Init this instance.</summary>
	private void Init()
	{
		// Prepare combobox of question numbers
		var model = new Gtk.ListStore( new [] { typeof( string ) } );

		for(int i = 0; i < this.Document.CountQuestions; ++i) {
			model.AppendValues( new []{ ( i +1 ).ToString() } );
		}

		// Last fixes
		this.cbQuestionNumber.Model = model;
		this.cbQuestionNumber.Active = 0;
		this.QuestionNumber = 0;
		this.cbAnswers.Show();
		this.cbAnswers.Sensitive = true;
		this.ShowAll();
	}

	private void EnableQuestionControls()
	{
		this.cbQuestionNumber.Active = this.QuestionNumber;
		this.btPrev.Sensitive = this.QuestionNumber > 0;
		this.btNext.Sensitive = this.QuestionNumber < ( this.Document.CountQuestions - 1 );
	}

	/// <summary>Go to the specified questionNumber.</summary>
	public void Go(int qn)
	{
		var question = this.Document.Questions[ qn ];

		this.EnableQuestionControls();

		// Prepare question info
		this.txtQuestion.Buffer.Text = question.Text;

		// Prepare answers
		( (Gtk.ListStore) this.cbAnswers.Model ).Clear();
		foreach(string answer in question.Answers) {
			this.cbAnswers.AppendText( answer );
		}

		this.cbAnswers.Active = ( qn >= this.answers.Count ) ?
													-1
													: this.answers[ qn ];
		return;
	}

	/// <summary>Goes to the current question</summary>
	public void Go()
	{
		this.Go( this.QuestionNumber );
	}

	///<summary>Closes the dialog for checking.</summary>
	private void Check()
	{
Console.WriteLine( "Check!!");
		this.Hide();
		this.Respond( Gtk.ResponseType.Ok );
	}

	/// <summary>Quit this dialog, called when the user presses exit.</summary>
	private void Quit()
	{
		this.Respond( Gtk.ResponseType.Close );
		this.Hide();
	}

	/// <summary>Previous question action.</summary>
	private void PreviousQuestion()
	{
		// Go backwards
		--this.QuestionNumber;

		if ( this.QuestionNumber < 0 ) {
			this.QuestionNumber = 0;
		} else {
			this.Go( this.QuestionNumber );
		}

		// Prepare UI
		this.EnableQuestionControls();
	}

	/// <summary>Next question action.</summary>
	private void NextQuestion()
	{
		int MaxQuestions = this.Document.CountQuestions;

		// Go forward
		++this.QuestionNumber;

		if ( this.QuestionNumber >= MaxQuestions ) {
			this.QuestionNumber = MaxQuestions -1;
		} else {
			this.Go( this.QuestionNumber );
		}

		// Prepare UI
		this.EnableQuestionControls();
	}

	/// <summary>The combo for question numbers has changed</summary>
	private void OnQuestionNumberChanged()
	{
		this.QuestionNumber = this.cbQuestionNumber.Active;
		this.Go();
	}

	/// <summary>Raised when the combobox for answers is cliked.</summary>
	private void OnAnswerChanged()
	{
		int newAnswer = this.cbAnswers.Active;

		if ( this.QuestionNumber > -1
		  && newAnswer > -1 )
		{
			this.answers[ this.QuestionNumber ] = newAnswer;
		}
	}

	/// <summary>The <see cref="Document"/> this dialog acts upon.</summary>
	public Document Document {
		get; private set;
	}

	/// <summary>Gets the current question number.</summary>
	public int QuestionNumber {
		get; private set;
	}

	/// <summary>The answers given by the user.</summary>
	/// <value>An int representing 0 for answer 'a', 1 for 'b'...</value>
	public ReadOnlyCollection<int> Answers => this.answers.AsReadOnly();

	private readonly List<int> answers;
}
