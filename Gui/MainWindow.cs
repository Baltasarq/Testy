using System;
using System.IO;
using Gtk;
using System.Collections.ObjectModel;
using Testy.Core;
using GtkUtil;

namespace Testy.Gui {

public partial class MainWindow: Gtk.Window {

	public static ReadOnlyCollection<string> HeaderDocumentEn = new ReadOnlyCollection<string>(
			new string[] { "#", "Questions" }
	);
		
	public static ReadOnlyCollection<string> HeaderDocumentEs = new ReadOnlyCollection<string>(
			new string[] { "#", "Preguntas" }
	);
		
	public static ReadOnlyCollection<string> HeaderAnswersEn = new ReadOnlyCollection<string>(
			new string[] { "#", "Answers" }
	);
		
	public static ReadOnlyCollection<string> HeaderAnswersEs = new ReadOnlyCollection<string>(
			new string[] { "#", "Respuestas" }
	);
		
	public const string DefaultFileName = "test" + Document.TestFileExt;
	
    public MainWindow(): base(Gtk.WindowType.Toplevel)
    {
        // Prepare window
        this.Build();
        this.SetGeometryHints(
				this,
				new Gdk.Geometry() {
					MinWidth = 620,
					MinHeight = 460 },
				Gdk.WindowHints.MinSize
		);
		this.DeactivateGui();
			
		// Prepare tree views
		this.tvwDocument = new TableTextView( this.tvDocument, HeaderDocumentEn.Count );
		this.tvwDocument.Headers = HeaderDocumentEn;
		this.tvDocument.ButtonReleaseEvent += OnQuestionChanged;
		this.tvwDocument.SetEditable( 1, false );
		this.tvwAnswers  = new TableTextView( this.tvAnswers, HeaderDocumentEn.Count );
		this.tvwAnswers.Headers = HeaderAnswersEn;
		this.tvwAnswers.tableChanged += OnAnswerChanged;

        // Prepare the txtDocument TextView
        Pango.FontDescription font = new Pango.FontDescription();
        font.Family = "Monospace";
        this.txtDocument.ModifyFont( font );
			
		return;
    }
		
	/// <summary>
	/// Update the view of the current question
	/// </summary>
	/// <param name='row'>
	/// The number of the question edited.
	/// </param>
	/// <param name='col'>
	/// The column (should always be 1).
	/// </param>
	/// <param name='value'>
	/// The new text for that question.
	/// </param>
	protected void OnQuestionChanged(object obj, EventArgs args)
	{
		int row;
		int col;
			
		this.tvwDocument.GetCurrentCell( out row, out col );
			
		if ( row >= 0
		  && row < this.Document.CountQuestions )
		{
			this.UpdateViewAt( row );
		} else {
			this.ReportNoRow( row );
		}
	}
		
	/// <summary>
	/// Update the text about a given answer
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
	protected void OnAnswerChanged(int row, int col, string value)
	{
		int questionNumber = this.GetQuestionBeingEdited();
			
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
	}
		
	/// <summary>
	/// The user tries to change the value of the correct answer
	/// </summary>
	protected void OnCorrectAnswerChanged(object sender, System.EventArgs e)
    {
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
	}
		
	/// <summary>
	/// Stores the question text.
	/// </summary>
	protected void StoreQuestionText()
	{
		if ( this.Document != null ) {
			int row = this.CurrentQuestion;
			
			if ( row >= 0 ) {
				// Update the text for that question
				this.Document[ row ].Text = this.edQuestionText.Buffer.Text;
					
				// Update document's view question
				this.tvwDocument.Set( row, 1, this.Document[ row ].Text );
					
				Util.UpdateUI();
			} else {
				this.ReportNoRow( row );
			}
		}

		return;
	}
		
	protected void OnAnswerChosen(object o, Gtk.RowActivatedArgs args)
	{
		int answerNumber;
		int col;
			
		this.tvwAnswers.GetCurrentCell( out answerNumber, out col );
		this.spNumberValidAnswer.Value = answerNumber +1;
	}
    
	/// <summary>
	/// Manages closing the application.
	/// </summary>
    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        this.Quit( );
        a.RetVal = true;
    }
		
	/// <summary>
	/// Quit this application.
	/// </summary>
	protected void Quit()
	{
		this.Close();
		Application.Quit();
	}
		
	/// <summary>
	/// Changes the name of the window title to the given file name.
	/// </summary>
	/// <param name='fn'>
	/// The file name, as a string.
	/// </param>
	protected void ChangeWindowTitleToFileName(string fn)
	{
		string title;
			
		if ( fn != null ) {
			fn = fn.Trim();
		}
			
		if ( fn != null
		  && fn.Length > 0 )
		{
			title = fn + " - " + AppInfo.Name;
		} else {
			title = AppInfo.Name;
		}
			
		this.Title = title;
	}
		
	/// <summary>
	/// Creates a new test document.
	/// </summary> 
	protected void New()
	{
		// Store the current test
		this.StoreQuestionText();
		this.Close();

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
	protected void CreateNewDefaultFileName()
	{
		string fileName = System.IO.Path.GetFileNameWithoutExtension( DefaultFileName );

		// Build file name
		fileName += numberOfDocuments.ToString();
		fileName += System.IO.Path.GetExtension( DefaultFileName );
			
		this.fileName = System.IO.Path.Combine(
				            Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ),
			                fileName
        );
		
		// Prepare next one
		++numberOfDocuments;
		this.nameGiven = false;
	}
		
	/// <summary>
	/// Save this document.
	/// </summary>
	protected void Save()
	{
		try {
			if ( this.Document != null ) {
				if ( this.NameGiven ) {
					this.Document.Save( Document.Format.Xml, fileName );
				} else {
					this.SaveAs();
				}
			}
		} catch(Exception exc)
		{
			Util.MsgError(this, AppInfo.Name, "Error saving: " + fileName + "\n" + exc.Message );
		}
			
		return;
	}
		
	/// <summary>
	/// Saves the document with another name.
	/// </summary>
	protected void SaveAs()
	{
		string fileNameAux = fileName;
			
		if ( this.Document != null )
		{
			bool fileNameChosen = Util.DlgSave(
					"Save test as...",
					"Save test as...",
					this,
					ref fileNameAux,
					"*" + Document.TestFileExt
			);
				
			if ( fileNameChosen ) {
				// Store the name
				this.nameGiven = true;
				fileName = fileNameAux;
				
				// Change extension
				if ( !fileName.EndsWith( Document.TestFileExt ) ) {
					fileName = System.IO.Path.ChangeExtension( fileName, Document.TestFileExt );
				}
					
				// Update title
				this.ChangeWindowTitleToFileName( fileName );
				this.Save();
			}
		}
			
		return;
	}
	
	/// <summary>
	/// Open a new document instance.
	/// </summary>
	protected void Open()
	{
		// Store the current test
		this.StoreQuestionText();
		this.Close();
			
		try {
			string fileNameAux = fileName;

			bool fileNameChosen = Util.DlgOpen(
					"Load test",
					"Load test",
					this,
					ref fileNameAux,
					"*" + Document.TestFileExt
			);
					
			if ( fileNameChosen ) {
				this.nameGiven = true;
				fileName = fileNameAux;
				this.Document = Document.Load( fileName );
					
				// Prepare UI
				this.ActivateGui();
				this.ChangeWindowTitleToFileName( fileName );
				this.UpdateView();
			}
		} catch(Exception exc)
		{
			Util.MsgError(this, AppInfo.Name, "Error loading: " + fileName + "\n" + exc.Message );
			this.Document = null;
			this.DeactivateGui();
		}
	}
		
	/// <summary>
	/// Close this document, revert UI to deactivate.
	/// </summary>
	protected void Close()
	{
		this.Save();
		this.Document = null;
		this.DeactivateGui();
	}
		
	/// <summary>
	/// Shows the "about" info
	/// </summary>
	protected void OnAbout(object sender, System.EventArgs e)
	{
		var aboutDlg = new AboutDialog();

		aboutDlg.ProgramName = AppInfo.Name;
		aboutDlg.Logo = this.Icon;
		aboutDlg.Version = AppInfo.Version;
		aboutDlg.Authors = new string[]{ AppInfo.Author };
		aboutDlg.Website = AppInfo.Website;
		aboutDlg.Icon = this.Icon;
		aboutDlg.License = "MIT";

		aboutDlg.TransientFor = this;
		aboutDlg.WindowPosition = WindowPosition.CenterOnParent;
		aboutDlg.Run();
		aboutDlg.Destroy();
		return;
	}
	
	/// <summary>
	/// Quit action
	/// </summary>
	protected void OnQuit(object sender, System.EventArgs e)
	{
		this.Quit();
	}
	
	/// <summary>
	/// New document action.
	/// </summary>
	protected void OnNew(object sender, System.EventArgs e)
	{
		this.New();
	}
	
	/// <summary>
	/// Open action
	/// </summary>
	protected void OnOpen(object sender, System.EventArgs e)
	{
		this.Open();
	}
	
	/// <summary>
	/// Save action.
	/// </summary>
	protected void OnSave(object sender, System.EventArgs e)
	{
		this.Save();
	}
	
	/// <summary>
	/// SaveAs action
	/// </summary>
	protected void OnSaveAs(object sender, System.EventArgs e)
	{
		this.SaveAs();
	}
	
	/// <summary>
	/// Take test. Presents questions to the user.
	/// </summary>
	protected void OnTakeTest(object sender, System.EventArgs e)
	{
		// Show the test modal window
		var dlgTest = new DlgTakeTest( this, this.Document );
		this.takeTestAction.Sensitive = false;
		dlgTest.ShowAll();
		Gtk.ResponseType result = (Gtk.ResponseType) dlgTest.Run();
			
		if ( result == Gtk.ResponseType.Ok ) {
			this.Show();
			dlgTest.SetCorrectionMode();
			dlgTest.Run();
		}
		
		dlgTest.Destroy();
		this.Show();
		this.takeTestAction.Sensitive = true;
		return;
	}

	
	/// <summary>
	/// Export into html
	/// </summary>
	protected void OnExport(object sender, System.EventArgs e)
	{
		if ( this.Document != null ) {
			var dlg = new DlgExport( fileName );
			dlg.SetPosition( WindowPosition.CenterOnParent );
			dlg.Modal = true;
            dlg.Icon = this.Icon;
            bool chosen = ( (Gtk.ResponseType) dlg.Run() == Gtk.ResponseType.Ok );
				
			if ( chosen ) {
				this.SetStatus( "Saving..." );
	            this.Document.Save( dlg.OutputFormat, dlg.FileName );
				this.SetStatus();
			}
				
			dlg.Destroy();
        } else {
            this.ReportNoDocument();
        }
	}
	
	/// <summary>
	/// Close action
	/// </summary>
	protected void OnClose(object sender, System.EventArgs e)
	{
		this.Close();
	}
	
	/// <summary>
	/// Action add question
	/// </summary>
	protected void OnAddQuestion(object sender, System.EventArgs e)
	{
		if ( this.Document != null ) {
			// Store the previous question text
			this.StoreQuestionText();
				
			// Add one more question
			this.Document.Add( "Q?: " + ( this.Document.CountQuestions +1).ToString() );
				
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
	protected void OnAddAnswer(object sender, System.EventArgs e)
	{
		if ( this.Document != null ) {
			// Store the previous question text
			this.StoreQuestionText();
				
			if ( this.CurrentQuestion >= 0 ) {
				var currentq = this.Document[ this.CurrentQuestion ];
				var answerText = "A: " + ( currentq.CountAnswers +1).ToString();

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
	protected void OnRemoveQuestion(object sender, System.EventArgs e)
	{
		int questionNumber;

		if ( this.Document != null ) {
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
	protected void OnRemoveAnswer(object sender, System.EventArgs e)
	{
		int questionNumber = this.CurrentQuestion;
		int answerNumber;
		int col;
			
		// Store the previous question text
		this.StoreQuestionText();
			
		if ( questionNumber >= 0 ) {
			var q = this.Document[ questionNumber ];
				
			this.tvwAnswers.GetCurrentCell( out answerNumber, out col );

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
	
	protected void OnCurrentPageChanged(object o, SwitchPageArgs args)
    {
		if ( this.nbDocPages.CurrentPage == 1 )
		{
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
	
	protected void ActivateGui()
	{
		this.SetGuiActive( ( this.Document != null ) );
	}
		
	protected void DeactivateGui()
	{
		this.SetGuiActive( false );
	}
		
	/// <summary>
	/// Updates the complete view for a given document.
	/// </summary>
	protected void UpdateView()
	{
		this.UpdateDocumentView();
		this.UpdateViewAt( 0 );
		this.CurrentQuestion = 0;
	}
		
	/// <summary>
	/// Updates the document view.
	/// </summary>
	protected void UpdateDocumentView()
	{
		if ( this.Document != null ) {
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
	protected void UpdateViewAt(int index)
	{
		if ( this.Document != null ) {
			
			if ( index >= 0
			  && index < this.Document.Questions.Count )
			{
				Question q = this.Document[ index ];
				this.CurrentQuestion = index;
				var answers = q.Answers;
					
				// Add question text
				edQuestionText.Buffer.Text = q.Text;
					
				// Add answers
				tvAnswers.Hide();
				tvwAnswers.RemoveAllRows();
				for(int i = 0; i < answers.Count; ++i) {
					tvwAnswers.AppendRow();
					tvwAnswers.Set( i, 1, answers[ i ] );
				}
				tvAnswers.Show();
					
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
		
	protected void ReportNoRow(int index)
	{
		Util.MsgError( this, AppInfo.Name, "No row at: " + index.ToString() );
	}
		
	protected void ReportNoDocument()
	{
		Util.MsgError( this, AppInfo.Name, "No document available." );
	}
		
	protected void SetGuiActive(bool active = true)
	{
		// Actions
		this.saveAction.Sensitive = active;
		this.convertAction.Sensitive = active;
		this.addAction.Sensitive = active;
		this.removeAction.Sensitive = active;
		this.closeAction.Sensitive = active;
		this.removeAnswerAction.Sensitive = active;
		this.addAnswerAction.Sensitive = active;
		this.takeTestAction.Sensitive = active;
			
		// Frames
		this.nbDocPages.Visible = active;
		this.nbDocPages.CurrentPage = 0;
		this.stStatusBar.Visible = active;
			
		// Status
		if ( active ) {
			this.SetStatus( "Ready" );
		}
			
		// Title
		if ( !active ) {
			this.ChangeWindowTitleToFileName( null );
		}
	}
		
	/// <summary>
	/// Removes previous status (typically sets it to cleared).
	/// </summary>
	public void SetStatus()
	{
		stStatusBar.Pop( 0 );
	}
	
	/// <summary>
	/// Sets the status to a given message.
	/// </summary>
	/// <param name='statusMsg'>
	/// Status message, as a string.
	/// </param>
	public void SetStatus(string statusMsg)
	{
		this.stStatusBar.Push( 0, statusMsg );
		this.UpdateNumberOfQuestions();
		Util.UpdateUI();
	}
		
	/// <summary>
	/// Updates the number of questions in the view
	/// </summary>
	protected void UpdateNumberOfQuestions()
	{
		if ( this.Document != null ) {
			this.lblStatusNumber.Text = this.Document.CountQuestions.ToString();
		}
	}
		
	protected void OnTextQuestionEditingFinished(object o, Gtk.FocusOutEventArgs args)
	{
		this.StoreQuestionText();
	}

	/// <summary>
	/// Imports a text file with questions
	/// </summary>
	private void OnImport(object sender, EventArgs e)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Appends a second file of questions, in Testy format.
	/// </summary>
	private void OnAppend(object sender, EventArgs e)
	{
		if ( this.Document == null ) {
			Util.MsgError(this, AppInfo.Name, "No document present." );
			goto End;
		}

		// Store the current test
		this.StoreQuestionText();

		try {
			string fileNameAux = this.fileName;

			bool fileNameChosen = Util.DlgOpen(
				"Append test",
				"Append test",
				this,
				ref fileNameAux,
				"*" + Document.TestFileExt
			);

			if ( fileNameChosen ) {
				// Append
				using (Document extDoc = Document.Load( fileNameAux )) {
					foreach (Question q in extDoc.Questions) {
						this.Document.Add( q );
					}
				}

				// Prepare UI
				this.UpdateView();
			}
		} catch(Exception exc)
		{
			Util.MsgError(this, AppInfo.Name, "Error loading: " + fileName + "\n" + exc.Message );
		}

		End:
		return;
	}
		
	/// <summary>
	/// Gets the question being edited, checking the active one in the treeview of questions.
	/// </summary>
	/// <returns>
	/// The question being edited, as an integer.
	/// </returns>
	protected int GetQuestionBeingEdited()
	{
		int row = -1;
		int col;
			
		if ( this.Document != null ) {
			// Retrieve question being edited
			this.tvwDocument.GetCurrentCell( out row, out col );
	
			// Adjust upper limit
			if ( row >= this.Document.CountQuestions ) {
				row = -1;
			}
		}
			
		return row;
	
	}

	public Document Document {
		get; set;
	}
		
	public bool NameGiven {
		get {
			return this.nameGiven;
		}
	}
		
	public int CurrentQuestion {
		get; set;
	}
		
	private TableTextView tvwDocument;
	private TableTextView tvwAnswers;
	private string fileName;
	private bool nameGiven;
	private static int numberOfDocuments = 1;

}
}
