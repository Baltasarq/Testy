using System;
using System.Text;
using Testy.Core;

namespace Testy.Gui
{
    public partial class DlgTakeTest : Gtk.Dialog
    {
        public enum Mode { Test, Correction };

        public DlgTakeTest(Gtk.Window wndParent, Document doc) {
            this.Build();
			
			// Set min size
			this.SetGeometryHints(
				this,
				new Gdk.Geometry() {
					MinWidth = 620,
					MinHeight = 460 },
				Gdk.WindowHints.MinSize
			);

			// Build all decorations
            this.doc = doc;
			this.answers = new int[ this.Document.CountQuestions ];
			this.workMode = Mode.Test;
			this.questionNumber = -1;
            this.Modal = true;
            this.Title = "Taking test: " + this.Document.Title;
            this.lblTitle.Text = this.Document.Title + ':';
			this.lblCorrect.Text = "";
			this.TransientFor = wndParent;
			this.lblNumber.Text = this.Document.CountQuestions.ToString();
			this.WindowPosition = Gtk.WindowPosition.CenterOnParent;
            this.Init();
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        protected void Init()
        {
			// Prepare vector of answers
            for(int i = 0; i < this.Answers.Length; ++i) {
                this.Answers[ i ] = 0;
            }
			
			// Prepare combobox of question numbers
			var model = new Gtk.ListStore( new Type[] { typeof( string ) } );
			
			for(int i = 0; i < this.Document.CountQuestions; ++i) {
				model.AppendValues( new string[]{ ( i +1 ).ToString() } );
			}
			
			// Last fixes
			this.cbQuestionNumber.Model = model;
			this.cbQuestionNumber.Active = 0;            
			this.questionNumber = 0;
            this.lblCorrect.Hide();
			this.cbAnswers.Show();
			this.cbAnswers.Sensitive = true;
            this.Go();
        }
        
        /// <summary>
        /// Go to the specified questionNumber.
        /// </summary>
        public void Go(int qn)
        {
			var question = this.Document.Questions[ qn ];
			var MaxQuestions = this.Document.CountQuestions;
			
            // Prepare view
			this.cbQuestionNumber.Active = this.QuestionNumber;
			this.goBackAction.Sensitive = ( this.QuestionNumber > 0 );
			this.goForwardAction.Sensitive = ( this.QuestionNumber < ( MaxQuestions -1 ) );
			
			// Prepare correction
			if ( this.WorkMode == Mode.Correction ) {
				if ( this.Answers[ qn ] == question.CorrectAnswer ) {
					this.imgCorrection.SetFromStock( "gtk-yes", Gtk.IconSize.Dialog );
					this.txtCorrection.Buffer.Text = "";
					this.expCorrection.Hide();
					this.VBox.SetChildPacking( this.hbxChkContainer, false, false, 5, Gtk.PackType.End );
				} else {
					this.imgCorrection.SetFromStock( "gtk-no", Gtk.IconSize.Dialog );
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
			}
            
            // Prepare question info
            this.txtQuestion.Buffer.Text = question.Text;
			var model = new Gtk.ListStore( new Type[] { typeof( string ) } );
			
			foreach(var answer in question.Answers) {
				model.AppendValues( new string[]{ answer } );
			}
			
			this.cbAnswers.Model = model;
			this.cbAnswers.Active = this.Answers[ qn ];
        }
		
		public void Go()
		{
			this.Go( this.questionNumber );
		}
                    
        /// <summary>
        /// Quit this dialog.
        /// </summary>
        protected void Quit()
        {
            this.Hide();
        }
        
        /// <summary>
        /// Quit action
        /// </summary>
        protected void OnQuit(object sender, System.EventArgs e)
        {
            this.Quit();
        }

		/// <summary>
        /// Previous question action.
        /// </summary>
		protected void OnPrev(object sender, System.EventArgs e)
		{
			int MaxQuestions = this.Document.CountQuestions;
			
			// Go backwards
			--this.questionNumber;
			
			if ( this.QuestionNumber < 0 ) {
				this.questionNumber = 0;
			} else {
				this.Go( this.QuestionNumber );
			}
			
			// Prepare UI
			this.goBackAction.Sensitive = ( this.QuestionNumber > 0 );
			this.goForwardAction.Sensitive = ( this.QuestionNumber < ( MaxQuestions -1 ) );
		}
		
		/// <summary>
        /// Next question action.
        /// </summary>
		protected void OnFwd(object sender, System.EventArgs e)
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
			this.goForwardAction.Sensitive = ( this.QuestionNumber < ( MaxQuestions -1 ) );
			this.goBackAction.Sensitive = ( this.QuestionNumber > 0 );
		}

		/// <summary>
		/// The combo for question numbers has changed
		/// </summary>
		protected void OnQuestionNumberChanged(object sender, System.EventArgs e)
		{
			this.questionNumber = this.cbQuestionNumber.Active;
			this.Go();
		}
		
		/// <summary>
		/// Raised when the combobox for answers is cliked.
		/// </summary>
		protected void OnAnswerChanged(object sender, System.EventArgs e)
		{
			if ( this.QuestionNumber > -1 ) {
				this.Answers[ this.QuestionNumber ] = this.cbAnswers.Active;
			}
		}
		
		/// <summary>
		/// Changes the UI in order to go for correction mode.
		/// </summary>
		public void SetCorrectionMode()
		{
			int numCorrect = 0;
			
			// Prepare UI (first part)
			this.Hide();
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

			
			// Prepare UI
			this.cbAnswers.Hide();
			this.lblCorrect.Text = numCorrect.ToString() + " /";
			this.lblCorrect.Show();
			this.questionNumber = 0;
			this.Show();
			this.Go();
		}

		/// <summary>
		/// Builds the check framework.
		/// </summary>
		public void BuildCheckFramework()
		{
			this.txtCorrection = new Gtk.TextView();
			this.imgCorrection = new Gtk.Image( "gtk-yes", Gtk.IconSize.Dialog );
			this.expCorrection = new Gtk.Expander( "Check" );
			this.hbxChkContainer = new Gtk.HBox( false, 5 );
			var scrl = new Gtk.ScrolledWindow();
			
			// Build the view
			scrl.Add( this.txtCorrection );
			this.expCorrection.Add( scrl );
			this.hbxChkContainer.PackStart( this.imgCorrection, false, false, 5 );
			this.hbxChkContainer.PackEnd( this.expCorrection, true, true, 5 );
			this.VBox.Add( this.hbxChkContainer );
			
			
			// Prepare the txtCorrection TextView
	        Pango.FontDescription font = new Pango.FontDescription();
	        font.Family = "Monospace";
	        txtCorrection.ModifyFont( font );
			txtCorrection.WrapMode = Gtk.WrapMode.None;
			txtCorrection.Editable = false;
			
			// Prepare the expander
			this.expCorrection.Activated += delegate(System.Object o, System.EventArgs e) {
				bool isExpanded = this.expCorrection.Expanded;
				
				this.VBox.SetChildPacking( this.hbxChkContainer, isExpanded, isExpanded, 5, Gtk.PackType.End );
			};

			this.hbxChkContainer.ShowAll();
			return;
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
		
		private Gtk.TextView txtCorrection;
		private Gtk.Image imgCorrection;
		private Gtk.Expander expCorrection;
		private Gtk.HBox hbxChkContainer;
    }
}

