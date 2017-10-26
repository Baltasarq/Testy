namespace Testy.Gui {
    using System.Text;
    using System.Diagnostics;
    
    using Core;
    
    public partial class DlgCheckTest: Gtk.Dialog {
        public DlgCheckTest(Gtk.Window wndParent, Document doc, int[] answers)
        {
            int numQuestions = answers.Length;

            this.Document = doc;
            this.QuestionNumber = -1;
            this.answers = answers;
            
            Debug.Assert( numQuestions == doc.CountQuestions,
                          "document count and answers count disagree: "
                            + this.Document.CountQuestions
                            + " / " + numQuestions );
            
            this.Build();

            // Count&display correct answers
            int numCorrect = 0;
            for(int i = 0; i < numQuestions; ++i) {
                var question = this.Document.Questions[ i ];

                if ( question.CorrectAnswer == this.answers[ i ] ) {
                    ++numCorrect;
                }
            }
           
            this.lblCorrect.Text = numCorrect + " /";

            // Build all decorations
            this.Modal = true;
            this.Title = this.Document.Title;
            this.TransientFor = wndParent;
        }
        
        /// <summary>
        /// Init this instance.
        /// </summary>
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
            
            this.Go();
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
            var strCorrection = new StringBuilder();
            var question = this.Document.Questions[ qn ];
            
            this.txtQuestion.Buffer.Text = question.Text;

            this.EnableQuestionControls();
            
            // Prepare correction
            if ( this.answers[ qn ] == question.CorrectAnswer ) {
                this.imgCorrection.Pixbuf = this.iconCheck.ScaleSimple( 16, 16, Gdk.InterpType.Bilinear );
                strCorrection.Append( question.Answers[ this.answers[ qn ] ] );
            } else {
                this.imgCorrection.Pixbuf = this.iconClose.ScaleSimple( 16, 16, Gdk.InterpType.Bilinear );

                for(int i = 0; i < question.Answers.Count; ++i) {
                    char prefix = ' ';

                    if ( i == this.answers[ qn ] ) {
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
            }

            this.txtCorrection.Buffer.Text = strCorrection.ToString();
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
            --this.QuestionNumber;

            if ( this.QuestionNumber < 0 ) {
                this.QuestionNumber = 0;
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
            ++this.QuestionNumber;

            if ( this.QuestionNumber >= MaxQuestions ) {
                this.QuestionNumber = MaxQuestions -1;
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
            this.QuestionNumber = this.cbQuestionNumber.Active;
            this.Go();
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
        public int[] Answers {
            get {
                return (int[]) this.answers.Clone();
            }
        }
        
        private int[] answers;
    }
}
