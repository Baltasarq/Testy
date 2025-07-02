// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


using System.Text;
using System.Diagnostics;

using Core;
using System.Collections.ObjectModel;


public partial class DlgCheckTest: Gtk.Dialog {
    public DlgCheckTest(
                Gtk.Window wndParent,
                Document doc,
                ReadOnlyCollection<int> answers,
                Dictionary<string, Gdk.Pixbuf?> icons)
    {
        int numQuestions = answers.Count;

        this.icons = icons;
        this.txtQuestion = new Gtk.TextView {
            Editable = false,
            WrapMode = Gtk.WrapMode.Word
        };

        this.txtCorrection = new Gtk.TextView {
            Editable = false,
            WrapMode = Gtk.WrapMode.Word
        };

        this.cbQuestionNumber = new Gtk.ComboBoxText();
        this.lblTitle = new Gtk.Label();
        this.lblCorrect = new Gtk.Label();
        this.lblNumber = new Gtk.Label();
        this.imgCorrection = new Gtk.Image( this.icons[ "check" ] );

        this.btNext = new Gtk.ToolButton(
								new Gtk.Image( icons[ "next" ] ),
								"next" );
		this.btNext.Clicked += (o, e) => this.NextQuestion();

		this.btPrev = new Gtk.ToolButton(
								new Gtk.Image( icons[ "previous" ] ),
								"previous" );
		this.btPrev.Clicked += (o, e) => this.PreviousQuestion();

		this.btQuit = new Gtk.ToolButton(
								new Gtk.Image( icons[ "close" ] ),
								"close" );
		this.btQuit.Clicked += (o, e) => this.Quit();

        this.tbToolbar = [
            this.btPrev,
            this.btNext,
            this.btQuit ];

        this.Document = doc;
        this.QuestionNumber = -1;
        this.answers = answers.AsReadOnly();

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
        var model = new Gtk.ListStore([ typeof( string ) ]);

        for(int i = 0; i < this.Document.CountQuestions; ++i) {
            model.AppendValues([ ( i + 1 ).ToString() ]);
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
        this.btPrev.Sensitive = this.QuestionNumber > 0;
        this.btNext.Sensitive = this.QuestionNumber < ( this.Document.CountQuestions - 1 );
    }

    /// <summary>Go to the specified questionNumber.</summary>
    public void Go(int qn)
    {
        var strCorrection = new StringBuilder();
        var question = this.Document.Questions[ qn ];

        this.txtQuestion.Buffer.Text = question.Text;

        this.EnableQuestionControls();

        // Prepare correction
        if ( this.answers[ qn ] == question.CorrectAnswer ) {
            this.imgCorrection.Pixbuf = this.icons[ "check" ];
            strCorrection.Append( question.Answers[ this.answers[ qn ] ] );
        } else {
            this.imgCorrection.Pixbuf = this.icons[ "close" ];

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

    /// <summary>Goes to the current question.</summary>
    public void Go()
    {
        this.Go( this.QuestionNumber );
    }

    /// <summary>Quit this dialog.</summary>
    private void Quit()
    {
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

    private readonly ReadOnlyCollection<int> answers;
    private readonly Dictionary<string, Gdk.Pixbuf?> icons;
}
