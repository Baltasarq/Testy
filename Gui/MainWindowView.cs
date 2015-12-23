using System;
using System.IO;

namespace Testy.Gui {
	public partial class MainWindow: Gtk.Window {
		private void BuildIcons() {
			try {
				this.iconTesty = Gdk.Pixbuf.LoadFromResource( "Testy.Res.testy.png" );
			} catch(Exception) {
				this.iconTesty = null;
			}
		}

		private void BuildActions() {
			this.actImport = new Gtk.Action( "import", "_Import", "import questions in text file", Gtk.Stock.Convert );
			this.actImport.Activated += (sender, e) => this.Import();

			this.actNew = new Gtk.Action( "new", "_New", "new test", Gtk.Stock.New );
			this.actNew.Activated += (sender, e) => this.New();

			this.actOpen = new Gtk.Action( "open", "_Open", "open test", Gtk.Stock.Open );
			this.actOpen.Activated += (sender, e) => this.Open();

			this.actAppend = new Gtk.Action( "append", "_Append", "append another test questions", Gtk.Stock.Apply );
			this.actAppend.Activated += (sender, e) => this.Append();

			this.actSave = new Gtk.Action( "save", "_Save", "save test", Gtk.Stock.Save );
			this.actSave.Activated += (sender, e) => this.Save();

			this.actSaveAs = new Gtk.Action( "save-as", "Sa_ve as...", "save test as...", Gtk.Stock.SaveAs );
			this.actSaveAs.Activated += (sender, e) => this.SaveAs();

			this.actExport = new Gtk.Action( "export", "_Export", "export test as...", Gtk.Stock.Convert );
			this.actExport.Activated += (sender, e) => this.Export();

			this.actClose = new Gtk.Action( "close", "_Close", "close test", Gtk.Stock.Close );
			this.actClose.Activated += (sender, e) => this.Close();

			this.actQuit = new Gtk.Action( "quit", "_Quit", "quit", Gtk.Stock.Quit );
			this.actQuit.Activated += (sender, e) => this.Quit();

			this.actAddQuestion = new Gtk.Action( "add-question", "_Add question", "add question", Gtk.Stock.Add );
			this.actAddQuestion.Activated += (sender, e) => this.AddQuestion();

			this.actRemoveQuestion = new Gtk.Action( "remove-question", "_Remove question", "remove question", Gtk.Stock.Remove );
			this.actRemoveQuestion.Activated += (sender, e) => this.RemoveQuestion();

			this.actAddAnswer = new Gtk.Action( "add-answer", "Add a_nswer", "add answer", Gtk.Stock.Add );
			this.actAddAnswer.Activated += (sender, e) => this.AddAnswer();

			this.actRemoveAnswer = new Gtk.Action( "remove-answer", "Remove ans_wer", "remove answer", Gtk.Stock.Remove );
			this.actRemoveAnswer.Activated += (sender, e) => this.RemoveAnswer();

			this.actTakeTest = new Gtk.Action( "take-test", "_Take test", "take test", Gtk.Stock.MediaPlay );
			this.actTakeTest.Activated += (sender, e) => this.TakeTest();

			this.actAbout = new Gtk.Action( "about", "_About...", "about...", Gtk.Stock.About );
			this.actAbout.Activated += (sender, e) => this.About();
		}

		private void BuildMenu() {
			var accel = new Gtk.AccelGroup();
			this.mbMainMenu = new Gtk.MenuBar();

			// File
			var miFile = new Gtk.MenuItem( "_File" );
			var mFile = new Gtk.Menu();
			miFile.Submenu = mFile;
			var opNew = this.actNew.CreateMenuItem();
			opNew.AddAccelerator( "activate", accel, new Gtk.AccelKey(
				Gdk.Key.n, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );
			var opOpen = this.actOpen.CreateMenuItem();
			opOpen.AddAccelerator( "activate", accel, new Gtk.AccelKey(
				Gdk.Key.o, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );
			var opSave = this.actSave.CreateMenuItem();
			opSave.AddAccelerator( "activate", accel, new Gtk.AccelKey(
				Gdk.Key.s, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );
			var opQuit = this.actQuit.CreateMenuItem();
			opQuit.AddAccelerator( "activate", accel, new Gtk.AccelKey(
				Gdk.Key.q, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );
			mFile.Add( opNew );
			mFile.Add( this.actImport.CreateMenuItem() );
			mFile.Add( opOpen );
			mFile.Add( this.actAppend.CreateMenuItem() );
			mFile.Add( opSave );
			mFile.Add( this.actSaveAs.CreateMenuItem() );
			mFile.Add( this.actExport.CreateMenuItem() );
			mFile.Add( this.actClose.CreateMenuItem() );
			mFile.Add( opQuit );

			// Edit
			var miEdit = new Gtk.MenuItem( "_Edit" );
			var mEdit = new Gtk.Menu();
			miEdit.Submenu = mEdit;
			mEdit.Add( this.actAddQuestion.CreateMenuItem() );
			mEdit.Add( this.actRemoveQuestion.CreateMenuItem() );
			mEdit.Add( this.actAddAnswer.CreateMenuItem() );
			mEdit.Add( this.actRemoveAnswer.CreateMenuItem() );

			// Tools
			var miTools = new Gtk.MenuItem( "_Tools" );
			var mTools = new Gtk.Menu();
			miTools.Submenu = mTools;
			mTools.Add( this.actTakeTest.CreateMenuItem() );

			// About
			var miAbout = new Gtk.MenuItem( "_About" );
			var mAbout = new Gtk.Menu();
			miAbout.Submenu = mAbout;
			mAbout.Add( this.actAbout.CreateMenuItem() );

			// Finish
			this.mbMainMenu.Add( miFile );
			this.mbMainMenu.Add( miEdit );
			this.mbMainMenu.Add( miTools );
			this.mbMainMenu.Add( miAbout );
		}

		private void BuildToolbar() {
			this.tbToolbar = new Gtk.Toolbar();

			this.tbToolbar.Add( this.actNew.CreateToolItem() );
			this.tbToolbar.Add( this.actOpen.CreateToolItem() );
			this.tbToolbar.Add( this.actSave.CreateToolItem() );
			this.tbToolbar.Add( this.actClose.CreateToolItem() );
			this.tbToolbar.Add( new Gtk.SeparatorToolItem() );
			this.tbToolbar.Add( this.actAddQuestion.CreateToolItem() );
			this.tbToolbar.Add( this.actRemoveQuestion.CreateToolItem() );
			this.tbToolbar.Add( new Gtk.SeparatorToolItem() );
			this.tbToolbar.Add( this.actTakeTest.CreateToolItem() );
		}

		private void BuildStatusbar() {
			this.sbStatus = new Gtk.Statusbar();

			this.lblStatusNumber = new Gtk.Label( "0" );
			this.lblStatusNumber.UseMarkup = true;
			this.sbStatus.PackEnd( this.lblStatusNumber, false, false, 5 );
			this.sbStatus.Push( 0, "Ready" );
		}

		private void BuildNotebook() {
			var vBox = new Gtk.VBox( false, 5 );
			var hBox = new Gtk.HPaned();
			this.nbDocPages = new Gtk.Notebook();
			this.nbDocPages.SwitchPage += (o, args) => this.OnCurrentPageChanged();

			// Text view for the document
			this.txtDocument = new Gtk.TextView() { Editable = false };
			this.txtDocument.FocusOutEvent += (o, args) => this.StoreQuestionText();

			// Test treeview
			this.tvDocument = new Gtk.TreeView();
			var swScroll = new Gtk.ScrolledWindow();
			var frmTest = new Gtk.Frame( "Test" );
			( (Gtk.Label) frmTest.LabelWidget ).Markup = "<b>Test</b>";
			frmTest.Add( swScroll );
			swScroll.AddWithViewport( this.tvDocument );

			// Frame question
			var frmQuestion = new Gtk.Frame( "Question" );
			var swScrolledQuestion = new Gtk.ScrolledWindow();
			( (Gtk.Label) frmQuestion.LabelWidget ).Markup = "<b>Question</b>";
			this.edQuestionText = new Gtk.TextView();
			swScrolledQuestion.AddWithViewport( edQuestionText );
			frmQuestion.Add( swScrolledQuestion );
			vBox.PackStart( frmQuestion, false, false, 5 );

			// Frame answers
			var bttAnswers = new Gtk.VButtonBox();
			var vBoxAnswers = new Gtk.VBox( false, 5 );
			var frmAnswer = new Gtk.Frame( "Answer" );
			var hBoxAnswers = new Gtk.HBox( false, 5 );
			var hBoxCorrect = new Gtk.HBox( false, 5 );
			var swScrolledAnswers = new Gtk.ScrolledWindow();
			( (Gtk.Label) frmAnswer.LabelWidget ).Markup = "<b>Answer</b>";
			this.tvAnswers = new Gtk.TreeView();
			swScrolledAnswers.Add( this.tvAnswers );
			this.btAddAnswer = new Gtk.Button( Gtk.Stock.Add );
			this.btAddAnswer.Clicked += (sender, e) => this.AddAnswer();
			this.btRemoveAnswer = new Gtk.Button( Gtk.Stock.Remove );
			this.btRemoveAnswer.Clicked += (sender, e) => this.RemoveAnswer();
			bttAnswers.Add( this.btAddAnswer );
			bttAnswers.Add( this.btRemoveAnswer );
			bttAnswers.Layout = Gtk.ButtonBoxStyle.Center;
			bttAnswers.Spacing = 20;
			hBoxAnswers.PackStart( swScrolledAnswers, true, true, 5 );
			hBoxAnswers.PackStart( bttAnswers, false, false, 5 );
			vBoxAnswers.PackStart( hBoxAnswers, true, true, 5 );
			this.spNumberValidAnswer = new Gtk.SpinButton( 1, 20, 1 );
			this.spNumberValidAnswer.ChangeValue += (o, args) => this.OnCorrectAnswerChanged();
			hBoxCorrect.PackStart( new Gtk.Label( "Correct answer:" ), false, false, 5 );
			hBoxCorrect.PackStart( this.spNumberValidAnswer, false, false, 5 );
			vBoxAnswers.PackStart( hBoxCorrect, false, false, 5 );
			frmAnswer.Add( vBoxAnswers );
			vBox.PackStart( frmAnswer, true, true, 5 );

			// Layout
			hBox.Pack1( frmTest, false, false );
			hBox.Pack2( vBox, false, false );
			this.nbDocPages.AppendPage( hBox, new Gtk.Label( "Edit" ) );
			this.nbDocPages.AppendPage( this.txtDocument, new Gtk.Label( "Document" ) );
			this.nbDocPages.Page = 0;
		}

		private void Build() {
			var vBox = new Gtk.VBox( false, 0 );

			this.BuildIcons();
			this.BuildActions();
			this.BuildMenu();
			this.BuildToolbar();
			this.BuildNotebook();
			this.BuildStatusbar();

			vBox.PackStart( this.mbMainMenu, false, false, 0 );
			vBox.PackStart( this.tbToolbar, false, false, 0 );
			vBox.PackStart( this.nbDocPages, true, true, 0 );
			vBox.PackStart( this.sbStatus, false, false, 0 );
			this.Add( vBox );
			this.ShowAll();

			// Prepare
			this.SetGeometryHints(
				this,
				new Gdk.Geometry() {
					MinWidth = 640,
					MinHeight = 480 },
				Gdk.WindowHints.MinSize
			);
			this.DeleteEvent += (o, args) => this.OnTerminateWindow( args );
			this.Icon = this.iconTesty;
		}

		private Gdk.Pixbuf iconTesty;
		private Gtk.Action actImport;
		private Gtk.Action actNew;
		private Gtk.Action actOpen;
		private Gtk.Action actAppend;
		private Gtk.Action actSave;
		private Gtk.Action actSaveAs;
		private Gtk.Action actExport;
		private Gtk.Action actClose;
		private Gtk.Action actQuit;

		private Gtk.Action actAddQuestion;
		private Gtk.Action actRemoveQuestion;
		private Gtk.Action actAddAnswer;
		private Gtk.Action actRemoveAnswer;

		private Gtk.Action actTakeTest;
		private Gtk.Action actAbout;

		private Gtk.Toolbar tbToolbar;
		private Gtk.MenuBar mbMainMenu;
		private Gtk.Statusbar sbStatus;
		private Gtk.Label lblStatusNumber;
		private Gtk.Notebook nbDocPages;
		private Gtk.TextView txtDocument;
		private Gtk.SpinButton spNumberValidAnswer;
		private Gtk.TextView edQuestionText;
		private Gtk.TreeView tvDocument;
		private Gtk.TreeView tvAnswers;
		private Gtk.Button btAddAnswer;
		private Gtk.Button btRemoveAnswer;
	}
}
