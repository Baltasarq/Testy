using System;

namespace Testy.Gui {
	public partial class MainWindow: Gtk.Window {
		private void BuildIcons()
        {
			try {
				this.iconTesty = Gdk.Pixbuf.LoadFromResource( "Testy.Res.testy.png" );
				this.iconAbout = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.about.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-about", 32, this.iconAbout );

				this.iconAdd = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.add.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-add", 32, this.iconAdd );

				this.iconClose = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.close.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-close", 32, this.iconClose );

				this.iconExit = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.exit.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-exit", 32, this.iconExit );

				this.iconExport = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.export.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-export", 32, this.iconExport );

				this.iconFind = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.find.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-find", 32, this.iconFind );

				this.iconImport = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.import.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-import", 32, this.iconImport );

				this.iconNew = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.new.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-new", 32, this.iconNew );

				this.iconOpen = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.open.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-open", 32, this.iconOpen );

				this.iconRemove = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.remove.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-remove", 32, this.iconRemove );

				this.iconSave = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.save.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-save", 32, this.iconSave );

				this.iconPlay = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.play.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-play", 32, this.iconPlay );

				this.iconShuffle = new Gdk.Pixbuf(
					System.Reflection.Assembly.GetEntryAssembly(),
					"Testy.Res.shuffle.png", 32, 32 );
				Gtk.IconTheme.AddBuiltinIcon( "tsty-shuffle", 32, this.iconShuffle );
			} catch(Exception) {
				this.iconTesty = null;
			}
		}

		private void BuildActions()
		{
			this.actImport = new Gtk.Action( "import", "_Import", "import questions in text file", "import" ) { IconName = "tsty-import" };
			this.actImport.Activated += (sender, e) => this.Import();

			this.actFind = new Gtk.Action( "find", "_Find", "find in test", "find" ) { IconName = "tsty-find" };
			this.actFind.Activated += (sender, e) => this.Find();

			this.actNew = new Gtk.Action( "new", "_New", "new test", "new" ) { IconName = "tsty-new" };
			this.actNew.Activated += (sender, e) => this.New();

			this.actOpen = new Gtk.Action( "open", "_Open", "open test", "open" ) { IconName = "tsty-open" };
			this.actOpen.Activated += (sender, e) => this.Open();

			this.actAppend = new Gtk.Action( "append", "_Append", "append another test questions", "append" ) { IconName = "tsty-add" };
			this.actAppend.Activated += (sender, e) => this.Append();

			this.actSave = new Gtk.Action( "save", "_Save", "save test", "save" ) { IconName = "tsty-save" };
			this.actSave.Activated += (sender, e) => this.Save();

			this.actSaveAs = new Gtk.Action( "save-as", "Sa_ve as...", "save test as...", "save-as" ) { IconName = "tsty-save" };
			this.actSaveAs.Activated += (sender, e) => this.SaveAs();

			this.actExport = new Gtk.Action( "export", "_Export", "export test as...", "export"  ) { IconName = "tsty-export" };
			this.actExport.Activated += (sender, e) => this.Export();

			this.actClose = new Gtk.Action( "close", "_Close", "close test", "close" ) { IconName = "tsty-close" };
			this.actClose.Activated += (sender, e) => this.Close();

			this.actQuit = new Gtk.Action( "quit", "_Quit", "quit", "quit"  ) { IconName = "tsty-exit" };
			this.actQuit.Activated += (sender, e) => this.Quit();

			this.actAddQuestion = new Gtk.Action( "add-question", "_Add question", "add question", "add"  ) { IconName = "tsty-add" };
			this.actAddQuestion.Activated += (sender, e) => this.AddQuestion();

			this.actRemoveQuestion = new Gtk.Action( "remove-question", "_Remove question", "remove question", "remove-question" ) { IconName = "tsty-remove" };
			this.actRemoveQuestion.Activated += (sender, e) => this.RemoveQuestion();

			this.actAddAnswer = new Gtk.Action( "add-answer", "Add a_nswer", "add answer", "add-answer" ) { IconName = "tsty-add" };
			this.actAddAnswer.Activated += (sender, e) => this.AddAnswer();

			this.actRemoveAnswer = new Gtk.Action( "remove-answer", "Remove ans_wer", "remove answer", "remove-answer" ) { IconName = "tsty-remove" };
			this.actRemoveAnswer.Activated += (sender, e) => this.RemoveAnswer();

			this.actShuffle = new Gtk.Action( "shuffle", "_Shuffle", "Shuffle questions", "shuffle" ) { IconName = "tsty-shuffle" };
			this.actShuffle.Activated += (sender, e) => this.Shuffle();

			this.actTakeTest = new Gtk.Action( "take-test", "_Take test", "take test", "take-test" ) { IconName = "tsty-play" };
			this.actTakeTest.Activated += (sender, e) => this.TakeTest();

			this.actAbout = new Gtk.Action( "about", "_About...", "about...", "about" ) { IconName = "tsty-about" };
			this.actAbout.Activated += (sender, e) => this.About();
		}

		private void BuildMenu()
		{
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
			mTools.Add( this.actShuffle.CreateMenuItem() );
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

		private void BuildToolbar()
        {
            this.tbToolbar = new Gtk.Toolbar {
                this.actNew.CreateToolItem (),
                this.actOpen.CreateToolItem (),
                this.actSave.CreateToolItem (),
                this.actClose.CreateToolItem (),
                new Gtk.SeparatorToolItem (),
                this.actAddQuestion.CreateToolItem (),
                this.actRemoveQuestion.CreateToolItem (),
                new Gtk.SeparatorToolItem (),
                this.actTakeTest.CreateToolItem ()
            };
        }

		private void BuildStatusbar()
        {
			this.sbStatus = new Gtk.Statusbar();

            this.lblStatusNumber = new Gtk.Label( "0" ) {
                UseMarkup = true
            };
            
            this.sbStatus.PackEnd( this.lblStatusNumber, false, false, 5 );
			this.sbStatus.Push( 0, "Ready" );
		}
        
        private Gtk.Frame BuildEditQuestionFrame()
        {
            var toret = new Gtk.Frame( "Question" );
            
            var swScrolledQuestion = new Gtk.ScrolledWindow();
            ( (Gtk.Label) toret.LabelWidget ).Markup = "<b>Question</b>";
            this.edQuestionText = new Gtk.TextView { WrapMode = Gtk.WrapMode.Word };
            this.edQuestionText.KeyReleaseEvent += (o, args) => this.OnQuestionTextChanged();
            this.edQuestionText.FocusOutEvent += (o, args) => this.OnQuestionTextChanged();
            swScrolledQuestion.AddWithViewport( edQuestionText );
            toret.Add( swScrolledQuestion );
            
            return toret;
        }
        
        private Gtk.Frame BuildEditAnswersFrame()
        {
            // The treeview for answers
            this.tvAnswers = new Gtk.TreeView();
            var swScrolledAnswers = new Gtk.ScrolledWindow{ this.tvAnswers };

            // Add correct answer spin button
            this.spNumberValidAnswer = new Gtk.SpinButton( 1, 20, 1 );
            this.spNumberValidAnswer.ValueChanged +=
                                    (o, args) => this.OnCorrectAnswerChanged();

            // Add answer button            
            this.btAddAnswer = new Gtk.Button(
                new Gtk.Image( this.iconAdd.ScaleSimple( 16, 16, Gdk.InterpType.Bilinear ) ) );
            this.btAddAnswer.Clicked += (sender, e) => this.AddAnswer();
            
            // Remove answer button
            this.btRemoveAnswer = new Gtk.Button(
                new Gtk.Image( this.iconRemove.ScaleSimple( 16, 16, Gdk.InterpType.Bilinear ) ) );
            this.btRemoveAnswer.Clicked += (sender, e) => this.RemoveAnswer();
            
            // Answers control buttons
            var bttAnswers = new Gtk.HButtonBox{
                Spacing = 20,
                Layout = Gtk.ButtonBoxStyle.Center
            };

            bttAnswers.Add( this.btAddAnswer );
            bttAnswers.Add( this.btRemoveAnswer );
            
            // Correct answer control
            var hBoxCorrect = new Gtk.HBox( false, 5 );
            hBoxCorrect.PackStart( new Gtk.Label( "Correct answer:" ), false, false, 5 );
            hBoxCorrect.PackStart( this.spNumberValidAnswer, false, false, 5 );
            hBoxCorrect.PackEnd( bttAnswers, false, false, 5 );

            // Final layout
            var vBoxAnswers = new Gtk.VBox( false, 5 );            
            vBoxAnswers.PackStart( swScrolledAnswers, true, true, 5 );
            vBoxAnswers.PackStart( hBoxCorrect, false, false, 5 );
            
            // Frame
            var toret = new Gtk.Frame( "Answer" );
            ( (Gtk.Label) toret.LabelWidget ).Markup = "<b>Answer</b>";
            toret.Add( vBoxAnswers );

            return toret;
        }
        
        private Gtk.VPaned BuildEditDocumentFrame()
        {
            var toret = new Gtk.VPaned();

            toret.Pack1( this.BuildEditQuestionFrame(), false, false );
            toret.Pack2( this.BuildEditAnswersFrame(), true, true );
            
            return toret;
        }
        
        private Gtk.ScrolledWindow BuildDocumentTextView()
        {
            this.txtDocument = new Gtk.TextView {
                Editable = false,
                WrapMode = Gtk.WrapMode.Word
            };
            
            this.txtDocument.FocusOutEvent += (o, args) => this.StoreQuestionText();
            
            return new Gtk.ScrolledWindow { this.txtDocument };
        }

		private void BuildNotebook()
        {
			var hBoxTestAndDocument = new Gtk.HPaned();
			this.nbDocPages = new Gtk.Notebook();
			this.nbDocPages.SwitchPage += (o, args) => this.OnCurrentPageChanged();

			// Test treeview
			this.tvDocument = new Gtk.TreeView();
			var swScroll = new Gtk.ScrolledWindow();
			var frmTest = new Gtk.Frame( "Test" );
			( (Gtk.Label) frmTest.LabelWidget ).Markup = "<b>Test</b>";
			frmTest.Add( swScroll );
			swScroll.AddWithViewport( this.tvDocument );

			// Layout
			hBoxTestAndDocument.Pack1( frmTest, true, true );
			hBoxTestAndDocument.Pack2( this.BuildEditDocumentFrame(), false, false );
			this.nbDocPages.AppendPage(
                            hBoxTestAndDocument,
                            new Gtk.Label( "Edit" ) );
			this.nbDocPages.AppendPage(
                            this.BuildDocumentTextView(),
                            new Gtk.Label( "Document" ) );
			this.nbDocPages.Page = 0;
		}

		private void Build()
        {
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
				new Gdk.Geometry { MinWidth = 640, MinHeight = 480 },
				Gdk.WindowHints.MinSize
			);
			this.DeleteEvent += (o, args) => this.OnTerminateWindow( args );
			this.Icon = this.iconTesty;
		}

		private Gdk.Pixbuf iconAbout;
		private Gdk.Pixbuf iconImport;
		private Gdk.Pixbuf iconExport;
		private Gdk.Pixbuf iconFind;
		private Gdk.Pixbuf iconNew;
		private Gdk.Pixbuf iconOpen;
		private Gdk.Pixbuf iconAdd;
		private Gdk.Pixbuf iconPlay;
		private Gdk.Pixbuf iconShuffle;
		private Gdk.Pixbuf iconRemove;
		private Gdk.Pixbuf iconSave;
		private Gdk.Pixbuf iconClose;
		private Gdk.Pixbuf iconExit;
		private Gdk.Pixbuf iconTesty;

		private Gtk.Action actImport;
		private Gtk.Action actFind;
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
		private Gtk.Action actShuffle;

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
