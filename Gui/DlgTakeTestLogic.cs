using System.Text;
using Testy.Core;

namespace Testy.Gui {
	public partial class DlgTakeTest {
		public enum Mode { Test, Correction };

		/// <summary>
		/// Init this instance.
		/// </summary>
		private void Init()
		{
			// Prepare vector of answers
			this.answers = new int[ this.Document.CountQuestions ];
			for(int i = 0; i < this.Answers.Length; ++i) {
				this.Answers[ i ] = 0;
			}

			// Prepare combobox of question numbers
			var model = new Gtk.ListStore( new [] { typeof( string ) } );

			for(int i = 0; i < this.Document.CountQuestions; ++i) {
				model.AppendValues( new []{ ( i +1 ).ToString() } );
			}

			// Last fixes
			this.cbQuestionNumber.Model = model;
			this.cbQuestionNumber.Active = 0;            
			this.questionNumber = 0;
			this.lblCorrect.Hide();
			this.cbAnswers.Show();
			this.cbAnswers.Sensitive = true;
		}

		private void EnableQuestionControls()
		{
			this.cbQuestionNumber.Active = this.QuestionNumber;
			this.actPrev.Sensitive = ( this.QuestionNumber > 0 );
			this.actNext.Sensitive = ( this.QuestionNumber < ( this.Document.CountQuestions - 1 ) );
		}

		/// <summary>
		/// Go to the specified questionNumber.
		/// </summary>
		public void Go(int qn)
		{
			var question = this.Document.Questions[ qn ];

			this.EnableQuestionControls();

			// Prepare question info
			this.txtQuestion.Buffer.Text = question.Text;

			// Prepare correction
			if ( this.WorkMode == Mode.Correction ) {
				if ( this.Answers[ qn ] == question.CorrectAnswer ) {
					this.imgCorrection.Pixbuf = this.iconCheck.ScaleSimple( 16, 16, Gdk.InterpType.Bilinear );
					this.txtCorrection.Buffer.Text = "";
					this.expCorrection.Hide();
					this.VBox.SetChildPacking( this.hbxChkContainer, false, false, 5, Gtk.PackType.End );
				} else {
					this.imgCorrection.Pixbuf = this.iconClose.ScaleSimple( 16, 16, Gdk.InterpType.Bilinear );
					this.expCorrection.Expanded = true;
					var strCorrection = new StringBuilder();

					for(int i = 0; i < question.Answers.Count; ++i) {
						char prefix = ' ';

						if ( i == this.Answers[ qn ] ) {
							prefix = 'X';
						}
						else
						if ( i == question.CorrectAnswer ) {
							prefix = '*';
						}

						strCorrection.Append( '\n' );
						strCorrection.Append( ' ' );
						strCorrection.Append( prefix );
						strCorrection.Append( ' ' );
						strCorrection.Append( question.Answers[ i ] );
					}

					this.txtCorrection.Buffer.Text = strCorrection.ToString();
					this.expCorrection.Show();
					this.VBox.SetChildPacking( this.hbxChkContainer, true, true, 5, Gtk.PackType.End );
				}
			} else {
				// Prepare answers
				( (Gtk.ListStore) this.cbAnswers.Model ).Clear();
				foreach(string answer in question.Answers) {
					this.cbAnswers.AppendText( answer );
				}

				this.cbAnswers.Active = this.Answers[ qn ];
			}

			return;
		}

		/// <summary>
		/// Goes to the current question
		/// </summary>
		public void Go()
		{
			this.Go( this.QuestionNumber );
		}

		/// <summary>
		/// Quit this dialog.
		/// </summary>
		private void Quit()
		{
			this.Hide();
		}

		/// <summary>
		/// Previous question action.
		/// </summary>
		private void PreviousQuestion()
		{
			// Go backwards
			--this.questionNumber;

			if ( this.QuestionNumber < 0 ) {
				this.questionNumber = 0;
			} else {
				this.Go( this.QuestionNumber );
			}

			// Prepare UI
			this.EnableQuestionControls();
		}

		/// <summary>
		/// Next question action.
		/// </summary>
		private void NextQuestion()
		{
			int MaxQuestions = this.Document.CountQuestions;

			// Go forward
			++this.questionNumber;

			if ( this.QuestionNumber >= MaxQuestions ) {
				this.questionNumber = MaxQuestions -1;
			} else {
				this.Go( this.QuestionNumber );
			}

			// Prepare UI
			this.EnableQuestionControls();
		}

		/// <summary>
		/// The combo for question numbers has changed
		/// </summary>
		private void OnQuestionNumberChanged()
		{
			this.questionNumber = this.cbQuestionNumber.Active;
			this.Go();
		}

		/// <summary>
		/// Raised when the combobox for answers is cliked.
		/// </summary>
		private void OnAnswerChanged()
		{
			int newAnswer = this.cbAnswers.Active;

			if ( this.QuestionNumber > -1
			  && newAnswer > -1 )
			{
				this.Answers[ this.QuestionNumber ] = newAnswer;
			}
		}

		/// <summary>
		/// Changes the UI in order to go for correction mode.
		/// </summary>
		public void SetCorrectionMode()
		{
			int numCorrect = 0;

			// Prepare UI (first part)
			this.VBox.Hide();
			this.workMode = Mode.Correction;
			this.Title = "Correcting test: " + this.Document.Title;

			// Count correct answers
			for(int i = 0; i < this.Document.CountQuestions; ++i) {
				var question = this.Document.Questions[ i ];

				if ( question.CorrectAnswer == this.Answers[ i ] ) {
					++numCorrect;
				}
			}

			// Build check framework
			this.BuildCheckFramework();
			this.VBox.Show();

			// Prepare UI
			this.cbAnswers.Hide();
			this.lblCorrect.Text = numCorrect + " /";
			this.questionNumber = 0;
			this.Go();
		}

		public int[] Answers {
			get { return this.answers; }
		}

		public Document Document {
			get { return this.doc; }
		}

		public Mode WorkMode {
			get { return this.workMode; }
		}

		public int QuestionNumber {
			get { return this.questionNumber; }
		}

		private Document doc;
		private int[] answers;
		private Mode workMode;
		private int questionNumber;
	}
}

