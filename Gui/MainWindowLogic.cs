// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


using System;
using System.Linq;
using System.Collections.ObjectModel;

using Core;


public partial class MainWindow: Gtk.ApplicationWindow {
	public static ReadOnlyCollection<string> HeaderDocumentEn =
		new ( [ "#", "Questions" ] );

	public static ReadOnlyCollection<string> HeaderDocumentEs =
		new ( [ "#", "Preguntas" ] );

	public static ReadOnlyCollection<string> HeaderAnswersEn =
		new ( [ "#", "Answers" ] );

	public static ReadOnlyCollection<string> HeaderAnswersEs =
		new ( [ "#", "Respuestas" ] );

	public const string DefaultFileName = "test" + Transformer.TestFileExt;

	public MainWindow(Gtk.Application app)
		: base( app )
	{
		this.tvAnswers = new Gtk.TreeView();
		this.spNumberValidAnswer = new Gtk.SpinButton( 1, 20, 1 );
		this.txtDocument = new Gtk.TextView {
			Editable = false,
			WrapMode = Gtk.WrapMode.Word
		};
		this.nbDocPages = new Gtk.Notebook();
		this.tvDocument = new Gtk.TreeView();
		this.mbMainMenu = new Gtk.MenuBar();
		this.tbToolbar = new Gtk.Toolbar();
		this.sbStatus = new Gtk.Statusbar();
		this.lblStatusNumber = new Gtk.Label( "0" ) { UseMarkup = true };
		this.edQuestionText = new Gtk.TextView { WrapMode = Gtk.WrapMode.Word };

		this.fileName = string.Empty;

		this.actImport = new GtkUtil.UIAction( "import", "_Import", "import questions in text file" );
		this.actFind = new GtkUtil.UIAction( "find", "_Find", "find in test" );
		this.actNew = new GtkUtil.UIAction( "new", "_New", "new test" );
		this.actOpen = new GtkUtil.UIAction( "open", "_Open", "open test" );
		this.actAppend = new GtkUtil.UIAction( "append", "_Append", "append another test questions" );
		this.actSave = new GtkUtil.UIAction( "save", "_Save", "save test" );
		this.actSaveAs = new GtkUtil.UIAction( "save-as", "Sa_ve as...", "save test as..." );
		this.actExport = new GtkUtil.UIAction( "export", "_Export", "export test as..." );
		this.actClose = new GtkUtil.UIAction( "close", "_Close", "close test" );
		this.actQuit = new GtkUtil.UIAction( "quit", "_Quit", "quit" );
		this.actAddQuestion = new GtkUtil.UIAction( "add-question", "_Add question", "add question" );
		this.actRemoveQuestion = new GtkUtil.UIAction( "remove-question", "_Remove question", "remove question" );
		this.actAddAnswer = new GtkUtil.UIAction( "add-answer", "Add a_nswer", "add answer" );
		this.actRemoveAnswer = new GtkUtil.UIAction( "remove-answer", "Remove ans_wer", "remove answer" );
		this.actShuffle = new GtkUtil.UIAction( "shuffle", "_Shuffle", "Shuffle questions" );
		this.actTakeTest = new GtkUtil.UIAction( "take-test", "_Take test", "take test" );
		this.actAbout = new GtkUtil.UIAction( "about", "_About...", "about..." );

		this.btAddAnswer = new Gtk.Button( new Gtk.Image( this.actAddAnswer.Icon ));
		this.btRemoveAnswer = new Gtk.Button( new Gtk.Image( this.actRemoveAnswer.Icon ));

		// Prepare window
		this.Build();
		this.DeactivateGui();

		// Prepare document's tree view
		this.tvwDocument =
			new GtkUtil.TableTextView( this.tvDocument, HeaderDocumentEn.Count ) {
				Headers = HeaderDocumentEn
			};
		this.tvwDocument.SetEditable( 1, false );
		this.tvDocument.ButtonReleaseEvent += (sender, e) => this.OnQuestionFocusChangedTo();

		// Prepare answer's tree view
		this.tvwAnswers =
			new GtkUtil.TableTextView( this.tvAnswers, HeaderDocumentEn.Count ) {
			Headers = HeaderAnswersEn
		};
		this.tvwAnswers.TableChanged += this.OnAnswerChanged;

		// Prepare the txtDocument TextView
		var font = new Pango.FontDescription{ Family = "Monospace" };
		this.txtDocument.ModifyFont( font );
	}

	/// <summary>
	/// Update the view of the current question
	/// </summary>
	private void OnQuestionFocusChangedTo()
	{
		if ( this.Document is not null ) {
			this.tvwDocument.GetCurrentCell(out int row, out int col);

			if ( row >= 0
			&& row < this.Document.CountQuestions )
			{
				this.UpdateViewAt( row );
			} else {
				this.ReportNoRow( row );
			}
		} else {
			this.ReportNoDocument();
		}

		return;
	}

	/// <summary>
	/// Triggered when the text of the question is changed
	/// </summary>
	private void OnQuestionTextChanged()
	{
		if ( this.Document is not null ) {
			int questionNumber = this.GetQuestionBeingEdited();

			if ( questionNumber >= 0 ) {
				var q = this.Document[ questionNumber ];
				q.Text = this.edQuestionText.Buffer.Text;
				this.tvwDocument.Set( questionNumber, 1, q.Text );
			} else {
				this.ReportNoRow( questionNumber );
			}
		} else {
			this.ReportNoDocument();
		}

		return;
	}

	/// <summary>
	/// Updates the text about a given answer
	/// </summary>
	/// <param name='row'>
	/// The number of the answer edited.
	/// </param>
	/// <param name='col'>
	/// The column (should always be 1).
	/// </param>
	/// <param name='value'>
	/// The new text for that answer.
	/// </param>
	private void OnAnswerChanged(int row, int col, string value)
	{
		int questionNumber = this.GetQuestionBeingEdited();

		if ( this.Document is not null ) {
			if ( questionNumber >= 0 ) {
				var q = this.Document[ questionNumber ];

				if ( row < q.CountAnswers ) {
					q.ModifyAnswerAt( row, value );
					CurrentQuestion = questionNumber;
				} else {
					this.ReportNoRow( row );
				}
			} else {
				this.ReportNoRow( questionNumber );
			}
		} else {
			this.ReportNoDocument();
		}

		return;
	}

	/// <summary>
	/// The user tries to change the value of the correct answer
	/// </summary>
	private void OnCorrectAnswerChanged()
	{
		if ( this.Document is not null ) {
			int num = (int) this.spNumberValidAnswer.Value;

			// Retrieve the question object
			var q = this.Document[ this.CurrentQuestion ];

			// Correct lower limit
			if ( num < 1 ) {
				this.spNumberValidAnswer.Value = 1;
				num = 1;
			}

			// Correct upper limit
			if ( num > q.CountAnswers ) {
				num = q.CountAnswers;
				this.spNumberValidAnswer.Value = num;
			}

			this.StoreQuestionText();
			q.CorrectAnswer = num -1;
			this.UpdateViewAt( this.CurrentQuestion );
		} else {
			this.ReportNoDocument();
		}
	}

	/// <summary>
	/// Stores the question text.
	/// </summary>
	private void StoreQuestionText()
	{
		if ( this.Document is not null ) {
			int row = this.CurrentQuestion;

			if ( row >= 0 ) {
				// Update the text for that question
				this.Document[ row ].Text = this.edQuestionText.Buffer.Text;

				// Update document's view question
				this.tvwDocument.Set( row, 1, this.Document[ row ].Text );

				GtkUtil.Misc.UpdateUI();
			} else {
				this.ReportNoRow( row );
			}
		}

		return;
	}

	/// <summary>
	/// Manages closing the application.
	/// </summary>
	private void OnTerminateWindow(GLib.SignalArgs args)
	{
		this.Quit();
		args.RetVal = true;
	}

	/// <summary>
	/// Quit this application.
	/// </summary>
	private void Quit()
	{
		this.CloseDocument();
		Gtk.Application.Quit();
	}

	/// <summary>
	/// Changes the name of the window title to the given file name.
	/// </summary>
	/// <param name='fn'>
	/// The file name, as a string.
	/// </param>
	private void ChangeWindowTitleToFileName(string fn = "")
	{
		string title;

		fn = fn.Trim();
		if ( !string.IsNullOrEmpty( fn ) ) {
			fn = fn.Trim();
		}

		if ( !string.IsNullOrEmpty( fn ) ) {
			title = fn + " - " + AppInfo.Name;
		} else {
			title = AppInfo.Name;
		}

		this.Title = title;
	}

	/// <summary>
	/// Creates a new test document.
	/// </summary>
	private void New()
	{
		// Store the current test
		this.StoreQuestionText();
		this.CloseDocument();

		// Prepare new file name
		this.CreateNewDefaultFileName();
		this.ChangeWindowTitleToFileName( fileName );

		// Create the new test
		this.Document = new Document();
		this.ActivateGui();
		this.UpdateView();
	}

	/// <summary>
	/// Creates a new default file name.
	/// </summary>
	private void CreateNewDefaultFileName()
	{
		string fName = System.IO.Path.GetFileNameWithoutExtension( DefaultFileName );

		// Build file name
		fName += numberOfDocuments.ToString();
		fName += System.IO.Path.GetExtension( DefaultFileName );

		this.fileName = System.IO.Path.Combine(
			Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ),
			fName
		);

		// Prepare next one
		++numberOfDocuments;
		this.NameGiven = false;
	}

	/// <summary>
	/// Save this document.
	/// </summary>
	private void Save()
	{
		try {
			if ( this.Document is not null ) {
				if ( this.NameGiven ) {
					this.Document.Save( Transformer.Format.Xml, fileName );
				} else {
					this.SaveAs();
				}
			} else {
				this.ReportNoDocument();
			}
		} catch(Exception exc)
		{
			this.ShowError( "Error saving: "
								   	+ fileName + "\n" + exc.Message );
		}

		return;
	}

	/// <summary>
	/// Saves the document with another name.
	/// </summary>
	private void SaveAs()
	{
		string fileNameAux = fileName;

		if ( this.Document is not null ) {
			bool fileNameChosen = GtkUtil.Misc.DlgSave(
				"Save test as...",
				"Save test as...",
				this,
				ref fileNameAux,
				"*" + Transformer.TestFileExt
			);

			if ( fileNameChosen ) {
				// Store the name
				this.NameGiven = true;
				fileName = fileNameAux;

				// Change extension
				if ( !fileName.EndsWith( Transformer.TestFileExt, StringComparison.InvariantCulture ) )
				{
					fileName = System.IO.Path.ChangeExtension( fileName, Transformer.TestFileExt );
				}

				// Update title
				this.ChangeWindowTitleToFileName( fileName );
				this.Save();
			}
		} else {
			this.ReportNoDocument();
		}

		return;
	}

	/// <summary>
	/// Open a new document instance.
	/// </summary>
	private void Open()
	{
		// Store the current test
		this.StoreQuestionText();
		this.CloseDocument();

		try {
			string fileNameAux = fileName;

			bool fileNameChosen = GtkUtil.Misc.DlgOpen(
				"Load test",
				"Load test",
				this,
				ref fileNameAux,
				"*" + Transformer.TestFileExt
			);

			if ( fileNameChosen ) {
				this.NameGiven = true;
				fileName = fileNameAux;
				this.Document = Document.Load( Transformer.Format.Xml, fileName );

				// Prepare UI
				this.ActivateGui();
				this.ChangeWindowTitleToFileName( fileName );
				this.UpdateView();
			}
		} catch(Exception exc)
		{
			this.ShowError(  "Error loading: " + fileName + "\n" + exc.Message );
			this.Document = null;
			this.DeactivateGui();
		}
	}

	/// <summary>
	/// Close this document, revert UI to deactivate.
	/// </summary>
	private void CloseDocument()
	{
		if ( this.Document is not null
	      && GtkUtil.Misc.Ask( this, AppInfo.Name, "Do you want to save: " + fileName ) )
		{
			this.Save();
		}

		this.Document = null;
		this.DeactivateGui();
	}

	/// <summary>
	/// Shows the "about" info
	/// </summary>
	private void About()
	{
		var aboutDlg = new Gtk.AboutDialog () {
			ProgramName = AppInfo.Name,
			Logo = this.Icon,
			Version = AppInfo.Version,
			Authors = [ AppInfo.Author ],
			Website = AppInfo.Website,
			Icon = this.Icon,
			License = "MIT License",

			TransientFor = this,
			WindowPosition = Gtk.WindowPosition.CenterOnParent
		};

		aboutDlg.Run();
		aboutDlg.Destroy();
	}

	/// <summary>
	/// Take test. Presents questions to the user.
	/// </summary>
	private void TakeTest()
	{
		if ( this.Document is not null ) {
			this.actTakeTest.Sensitive = false;

			// Show the test modal window
			using var dlgTest = new DlgTakeTest( this, this.Document );
			var result = (Gtk.ResponseType) dlgTest.Run();

			if ( result == Gtk.ResponseType.Ok ) {
                using var dlgChk =
                    new DlgCheckTest( this, this.Document, dlgTest.Answers );
                dlgChk.Run();
            }

			this.actTakeTest.Sensitive = true;
		}
		return;
	}


	/// <summary>
	/// Export into html
	/// </summary>
	private void Export()
	{
		if ( this.Document is not null ) {
			var dlg = new DlgExport( this.Document, this.fileName, this );
			bool chosen = ( (Gtk.ResponseType) dlg.Run() != Gtk.ResponseType.Close );

			while ( chosen ) {
				Document doc = this.Document;
				int numQuestions = Math.Min( doc.CountQuestions, dlg.NumQuestions );

				if ( numQuestions < this.Document.CountQuestions ) {
					doc = new Document();
					doc.Clear();			// Erases the default question
					this.Document.Shuffle();
					doc.AddRange( this.Document.Questions.Take( numQuestions ) );
					this.UpdateView();
				}

				this.SetStatus( "Saving..." );
				doc.Save( dlg.OutputFormat, dlg.FileName );
				this.SetStatus();
				GtkUtil.Misc.MsgInfo( this, AppInfo.Name, "File generated: " + dlg.FileName );
				chosen = ( (Gtk.ResponseType) dlg.Run() != Gtk.ResponseType.Close );
			}

			dlg.Destroy();
		} else {
			this.ReportNoDocument();
		}

		return;
	}

	/// <summary>
	/// Action add question
	/// </summary>
	protected void AddQuestion()
	{
		if ( this.Document is not null ) {
			// Store the previous question text
			this.StoreQuestionText();

			// Add one more question
			this.Document.Add( "Q?: " + ( this.Document.CountQuestions + 1 ) );

			// Update tree view for document
			int numRow = this.tvwDocument.NumRows;
			this.tvwDocument.AppendRow();
			this.tvwDocument.Set( numRow, 1, this.Document[ numRow ].Text );
			this.tvwDocument.SetCurrentCell( numRow, 1 );

			// Update frame of answers
			this.UpdateViewAt( numRow );
		} else {
			this.ReportNoDocument();
		}
	}

	/// <summary>
	/// Action add answer
	/// </summary>
	private void AddAnswer()
	{
		if ( this.Document is not null ) {
			// Store the previous question text
			this.StoreQuestionText();

			if ( this.CurrentQuestion >= 0 ) {
				var currentq = this.Document[ this.CurrentQuestion ];
				var answerText = "A: " + ( currentq.CountAnswers + 1 );

				currentq.AddAnswer( answerText );
				this.UpdateViewAt( this.CurrentQuestion );
				this.tvwAnswers.SetCurrentCell( currentq.CountAnswers -1, 1 );
			} else {
				this.ReportNoRow( this.CurrentQuestion );
			}
		} else {
			this.ReportNoDocument();
		}
	}

	/// <summary>
	/// Action remove current question
	/// </summary>
	private void RemoveQuestion()
	{
		int questionNumber;

		if ( this.Document is not null ) {
			// Store the previous question text
			this.StoreQuestionText();

			// Retrieve current question
			questionNumber = CurrentQuestion;

			if ( questionNumber >= 0 ) {
				this.Document.RemoveAt( questionNumber );

				// Adapt question number
				questionNumber = Math.Min( questionNumber, this.Document.CountQuestions -1 );

				// Update frames
				this.UpdateDocumentView();
				this.tvwDocument.SetCurrentCell( questionNumber, 1 );
				this.UpdateViewAt( questionNumber );
			} else {
				this.ReportNoRow( questionNumber );
			}
		} else {
			this.ReportNoDocument();
		}
	}

	/// <summary>
	/// Action remove answer
	/// </summary>
	private void RemoveAnswer()
	{
		if ( this.Document is not null ) {
			int questionNumber = this.CurrentQuestion;

			// Store the previous question text
			this.StoreQuestionText();

			if ( questionNumber >= 0 ) {
				var q = this.Document[ questionNumber ];

				this.tvwAnswers.GetCurrentCell( out int answerNumber, out int col );

				if ( answerNumber >= 0
					&& answerNumber < q.CountAnswers )
				{
					q.RemoveAnswer( answerNumber );

					// Update views
					answerNumber = Math.Min( answerNumber, q.CountAnswers -1 );
					this.UpdateViewAt( questionNumber );
					this.tvwAnswers.SetCurrentCell( answerNumber, 1 );
				} else {
					this.ReportNoRow( answerNumber );
				}
			} else {
				this.ReportNoRow( questionNumber );
			}
		}
	}

	private void OnCurrentPageChanged()
	{
		if ( this.Document is not null ) {
			if ( this.nbDocPages.CurrentPage == 1 ) {
				// Visualizar el documento
				this.SetStatus( "Thinking..." );
				this.txtDocument.Buffer.Text = this.Document.ToString();
				this.SetStatus();
			}
			else
			if ( this.nbDocPages.CurrentPage == 0 )
			{
				this.txtDocument.Buffer.Text = "...";
			}
		}
	}

	private void ActivateGui()
	{
		this.SetGuiActive( this.Document is not null );
	}

	private void DeactivateGui()
	{
		this.SetGuiActive( false );
	}

	/// <summary>
	/// Updates the complete view for a given document.
	/// </summary>
	private void UpdateView()
	{
		this.UpdateDocumentView();
		this.UpdateViewAt( 0 );
		this.CurrentQuestion = 0;
	}

	/// <summary>
	/// Updates the document view.
	/// </summary>
	private void UpdateDocumentView()
	{
		if ( this.Document is not null ) {
			// Prepare the document tree view
			this.tvDocument.Hide();
			this.tvwDocument.RemoveAllRows();

			// Fill in all questions
			foreach(var q in this.Document) {
				this.tvwDocument.AppendRow();
				this.tvwDocument.Set( this.tvwDocument.NumRows -1, 1, q.Text );
			}

			// Show again the treeview
			this.tvwDocument.SetCurrentCell( 0, 1 );
			this.tvDocument.Show();
		} else {
			this.ReportNoDocument();
		}
	}

	/// <summary>
	/// Updates the answers frame when a
	/// </summary>
	/// <param name='index'>
	/// Index.
	/// </param>
	private void UpdateViewAt(int index)
	{
		if ( this.Document is not null ) {
			if ( index >= 0
	          && index < this.Document.Questions.Count )
			{
				Question q = this.Document[ index ];
				this.CurrentQuestion = index;
				var answers = q.Answers;

				// Add question text
				edQuestionText.Buffer.Text = q.Text;

				// Add answers
				this.tvAnswers.Hide();
				this.tvwAnswers.RemoveAllRows();
				for(int i = 0; i < answers.Count; ++i) {
					this.tvwAnswers.AppendRow();
					this.tvwAnswers.Set( i, 1, answers[ i ] );
				}
				this.tvAnswers.Show();

				// Configure correct answer spinbutton
				this.spNumberValidAnswer.SetRange( 1, answers.Count );
				this.spNumberValidAnswer.Value = q.CorrectAnswer +1;

				// Update other items of the view
				this.UpdateNumberOfQuestions();
			} else {
				this.ReportNoRow( index );
			}

		} else {
			this.ReportNoDocument();
		}
	}

	private void SetGuiActive(bool active = true)
	{
		// Actions
		this.actSave.Sensitive = active;
		this.actExport.Sensitive = active;
		this.actAddQuestion.Sensitive = active;
		this.actRemoveQuestion.Sensitive = active;
		this.actClose.Sensitive = active;
		this.actAddAnswer.Sensitive = active;
		this.actRemoveAnswer.Sensitive = active;
		this.actTakeTest.Sensitive = active;
		this.actShuffle.Sensitive = active;

		// Frames
		this.nbDocPages.Visible = active;
		this.nbDocPages.CurrentPage = 0;
		this.sbStatus.Visible = active;

		// Status
		if ( active ) {
			this.SetStatus( "Ready" );
		}

		// Title
		if ( !active ) {
			this.ChangeWindowTitleToFileName();
		}
	}

	/// <summary>
	/// Removes previous status (typically sets it to cleared).
	/// </summary>
	public void SetStatus()
	{
		sbStatus.Pop( 0 );
	}

	/// <summary>
	/// Sets the status to a given message.
	/// </summary>
	/// <param name='statusMsg'>
	/// Status message, as a string.
	/// </param>
	public void SetStatus(string statusMsg)
	{
		this.sbStatus.Push( 0, statusMsg );
		this.UpdateNumberOfQuestions();
		GtkUtil.Misc.UpdateUI();
	}

	/// <summary>
	/// Updates the number of questions in the view
	/// </summary>
	private void UpdateNumberOfQuestions()
	{
		if ( this.Document is not null ) {
			this.lblStatusNumber.Text = this.Document.CountQuestions.ToString();
		}
	}


	/// <summary>
	/// Imports a text file with questions
	/// </summary>
	private void Import()
	{
		string fileNameAux = this.fileName;

		if ( this.Document is not null ) {
			this.CloseDocument();
		}

		try {
			bool fileNameChosen = GtkUtil.Misc.DlgOpen(
				"Append test",
				"Append test",
				this,
				ref fileNameAux,
				"*" + Transformer.FormatExt[ (int) Transformer.Format.Text ]
			);

			if ( fileNameChosen ) {
				try {
					// Import and update UI
					this.Document = Document.Load( Transformer.Format.Text, fileNameAux );
					this.ActivateGui();
					this.UpdateView();
				} catch(Exception exc) {
					this.Document = null;
					this.DeactivateGui();
					this.ShowError(  exc.Message );
				}
			}
		} catch(Exception exc)
		{
			this.ShowError( "Error loading: " + fileName + "\n" + exc.Message );
		}

		this.UpdateView();
	}

	/// <summary>
	/// Appends a second file of questions, in Testy format.
	/// </summary>
	private void Append()
	{
		if ( this.Document == null ) {
			this.ReportNoDocument();
			goto End;
		}

		// Store the current test
		this.StoreQuestionText();

		try {
			string fileNameAux = this.fileName;

			bool fileNameChosen = GtkUtil.Misc.DlgOpen(
				"Append test",
				"Append test",
				this,
				ref fileNameAux,
				"*" + Transformer.TestFileExt
			);

			if ( fileNameChosen ) {
				// Append
				using (Document extDoc = Document.Load( Transformer.Format.Xml, fileNameAux )) {
					this.Document.AddRange( extDoc.Questions );
				}

				// Prepare UI
				this.UpdateView();
			}
		} catch(Exception exc)
		{
			this.ShowError( "Error loading: " + fileName + "\n" + exc.Message );
		}

		End:
		return;
	}

	/// <summary>
	/// Shuffles all questions
	/// </summary>
	private void Shuffle() {
		if ( this.Document is null ) {
			this.ReportNoDocument();
		} else {
			this.Document.Shuffle();
			this.UpdateView();
		}

		return;
	}

	/// <summary>
	/// Find in this test.
	/// </summary>
	private void Find() {
		if ( this.Document is null ) {
			this.ReportNoDocument();
		} else {
			//this.Document.Shuffle();
			this.UpdateView();
		}

		return;
	}

	/// <summary>
	/// Gets the question being edited, checking the active one in the treeview of questions.
	/// </summary>
	/// <returns>
	/// The question being edited, as an integer.
	/// </returns>
	private int GetQuestionBeingEdited()
	{
		int row = -1;

		if ( this.Document is not null ) {
			// Retrieve question being edited
			this.tvwDocument.GetCurrentCell( out row, out int col );

			// Adjust upper limit
			if ( row >= this.Document.CountQuestions ) {
				row = -1;
			}
		}

		return row;

	}

	/// <summary>
	/// Triggered when the user changes the correct answer for a given question.
	/// </summary>
	private void OnAnswerChosen()
	{
		this.tvwAnswers.GetCurrentCell( out int answerNumber, out int col );
		this.spNumberValidAnswer.Value = answerNumber + 1;
	}

	private void ReportNoRow(int index)
	{
		this.ShowError( "No row at: " + index );
	}

	private void ReportNoDocument()
	{
		this.ShowError( "No document available." );
	}

	private void ShowError(string msg)
	{
		this.ShowError( msg );
	}

	public Document? Document {
		get; private set;
	}

	public bool NameGiven {
		get; private set;
	}

	public int CurrentQuestion {
		get; private set;
	}

	private readonly GtkUtil.TableTextView tvwDocument;
	private readonly GtkUtil.TableTextView tvwAnswers;
	private string fileName;
	private static int numberOfDocuments = 1;
}
