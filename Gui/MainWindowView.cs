// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


public partial class MainWindow {
	private void BuildIcons()
	{
		try {
			this.Icon =
			this.iconTesty = Gdk.Pixbuf.LoadFromResource( "Testy.testy.png" );
			this.iconAbout = Gdk.Pixbuf.LoadFromResource( "Testy.about.png" );
			this.iconAdd = Gdk.Pixbuf.LoadFromResource( "Testy.add.png" );
			this.iconClose = Gdk.Pixbuf.LoadFromResource( "Testy.close.png" );
			this.iconExit = Gdk.Pixbuf.LoadFromResource( "Testy.exit.png" );
			this.iconExport = Gdk.Pixbuf.LoadFromResource( "Testy.export.png" );
			this.iconFind = Gdk.Pixbuf.LoadFromResource( "Testy.find.png" );
			this.iconImport = Gdk.Pixbuf.LoadFromResource( "Testy.import.png" );
			this.iconNew = Gdk.Pixbuf.LoadFromResource( "Testy.new.png" );
			this.iconOpen = Gdk.Pixbuf.LoadFromResource( "Testy.open.png" );
			this.iconRemove = Gdk.Pixbuf.LoadFromResource( "Testy.remove.png" );
			this.iconSave = Gdk.Pixbuf.LoadFromResource( "Testy.save.png" );
			this.iconPlay = Gdk.Pixbuf.LoadFromResource( "Testy.play.png" );
			this.iconShuffle = Gdk.Pixbuf.LoadFromResource( "Testy.shuffle.png" );

			this.tbToolbar.Style = Gtk.ToolbarStyle.Icons;
		} catch(Exception) {
			this.iconTesty = null;
			this.tbToolbar.Style = Gtk.ToolbarStyle.Text;
		}
	}

	private void BuildActions()
	{
		this.actImport.Activated += (sender, e) => this.Import();
		this.actFind.Activated += (sender, e) => this.Find();
		this.actNew.Activated += (sender, e) => this.New();
		this.actOpen.Activated += (sender, e) => this.Open();
		this.actAppend.Activated += (sender, e) => this.Append();
		this.actSave.Activated += (sender, e) => this.Save();
		this.actSaveAs.Activated += (sender, e) => this.SaveAs();
		this.actExport.Activated += (sender, e) => this.Export();
		this.actClose.Activated += (sender, e) => this.CloseDocument();
		this.actQuit.Activated += (sender, e) => this.Quit();
		this.actAddQuestion.Activated += (sender, e) => this.AddQuestion();
		this.actRemoveQuestion.Activated += (sender, e) => this.RemoveQuestion();
		this.actAddAnswer.Activated += (sender, e) => this.AddAnswer();
		this.actRemoveAnswer.Activated += (sender, e) => this.RemoveAnswer();
		this.actShuffle.Activated += (sender, e) => this.Shuffle();
		this.actTakeTest.Activated += (sender, e) => this.TakeTest();
		this.actAbout.Activated += (sender, e) => this.About();

		var iconQuestion = new Gtk.Image( "question-outline-symbolic",
										  Gtk.IconSize.Button );

		this.actImport.Icon = this.iconImport ?? iconQuestion.Pixbuf;
		this.actFind.Icon = this.iconFind ?? iconQuestion.Pixbuf;
		this.actNew.Icon = this.iconNew ?? iconQuestion.Pixbuf;
		this.actOpen.Icon = this.iconOpen ?? iconQuestion.Pixbuf;
		this.actAppend.Icon = this.iconAdd ?? iconQuestion.Pixbuf;
		this.actSave.Icon = this.iconSave ?? iconQuestion.Pixbuf;
		this.actExport.Icon = this.iconExport ?? iconQuestion.Pixbuf;
		this.actClose.Icon = this.iconClose ?? iconQuestion.Pixbuf;
		this.actQuit.Icon = this.iconExit ?? iconQuestion.Pixbuf;
		this.actAddQuestion.Icon = this.iconAdd ?? iconQuestion.Pixbuf;
		this.actRemoveQuestion.Icon = this.iconRemove ?? iconQuestion.Pixbuf;
		this.actAddAnswer.Icon = this.iconAdd ?? iconQuestion.Pixbuf;
		this.actRemoveAnswer.Icon = this.iconRemove ?? iconQuestion.Pixbuf;
		this.actShuffle.Icon = this.iconShuffle ?? iconQuestion.Pixbuf;
		this.actTakeTest.Icon = this.iconPlay ?? iconQuestion.Pixbuf;
		this.actAbout.Icon = this.iconAbout ?? iconQuestion.Pixbuf;
	}

	private void BuildMenu()
	{
		// File
		var miFile = new Gtk.MenuItem( "_File" );
		var mFile = new Gtk.Menu();
		miFile.Submenu = mFile;

		var opNew = this.actNew.CreateMenuItem();
		this.actNew.SetAccelerator( Gdk.Key.N, Gdk.ModifierType.ControlMask );

		var opOpen = this.actOpen.CreateMenuItem();
		this.actOpen.SetAccelerator( Gdk.Key.O, Gdk.ModifierType.ControlMask );

		var opSave = this.actSave.CreateMenuItem();
		this.actSave.SetAccelerator( Gdk.Key.S, Gdk.ModifierType.ControlMask );

		var opQuit = this.actQuit.CreateMenuItem();
		this.actQuit.SetAccelerator( Gdk.Key.Q, Gdk.ModifierType.ControlMask );

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
		this.actAddQuestion.SetAccelerator( Gdk.Key.plus,
											Gdk.ModifierType.ControlMask );
		mEdit.Add( this.actRemoveQuestion.CreateMenuItem() );
		this.actRemoveQuestion.SetAccelerator( Gdk.Key.minus,
											Gdk.ModifierType.ControlMask );
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
		this.tbToolbar.Add( this.actNew.CreateToolButton() );
		this.tbToolbar.Add( this.actOpen.CreateToolButton() );
		this.tbToolbar.Add( this.actSave.CreateToolButton() );
		this.tbToolbar.Add( this.actClose.CreateToolButton() );
		this.tbToolbar.Add( new Gtk.SeparatorToolItem () );
		this.tbToolbar.Add( this.actAddQuestion.CreateToolButton() );
		this.tbToolbar.Add( this.actRemoveQuestion.CreateToolButton() );
		this.tbToolbar.Add( new Gtk.SeparatorToolItem () );
		this.tbToolbar.Add( this.actTakeTest.CreateToolButton() );
	}

	private void BuildStatusbar()
	{
		this.sbStatus.PackEnd( this.lblStatusNumber, false, false, 5 );
		this.sbStatus.Push( 0, "Ready" );
	}

	private Gtk.Frame BuildEditQuestionFrame()
	{
		var toret = new Gtk.Frame( "Question" );

		var swScrolledQuestion = new Gtk.ScrolledWindow();
		( (Gtk.Label) toret.LabelWidget ).Markup = "<b>Question</b>";
		this.edQuestionText.KeyReleaseEvent += (o, args) => this.OnQuestionTextChanged();
		this.edQuestionText.FocusOutEvent += (o, args) => this.OnQuestionTextChanged();
		swScrolledQuestion.Add( edQuestionText );
		toret.Add( swScrolledQuestion );

		return toret;
	}

	private Gtk.Frame BuildEditAnswersFrame()
	{
		// The treeview for answers
		var swScrolledAnswers = new Gtk.ScrolledWindow{ this.tvAnswers };

		// Add correct answer spin button
		this.spNumberValidAnswer.ValueChanged +=
								(o, args) => this.OnCorrectAnswerChanged();

		// Add answer button
		this.btAddAnswer.Clicked += (sender, e) => this.AddAnswer();

		// Remove answer button
		this.btRemoveAnswer.Clicked += (sender, e) => this.RemoveAnswer();

		// Answers control buttons
		var bttAnswers = new Gtk.ButtonBox( Gtk.Orientation.Horizontal ) {
			Spacing = 20,
			Layout = Gtk.ButtonBoxStyle.Center
		};

		bttAnswers.Add( this.btAddAnswer );
		bttAnswers.Add( this.btRemoveAnswer );

		// Correct answer control
		var hBoxCorrect = new Gtk.Box( Gtk.Orientation.Horizontal, 5 );
		hBoxCorrect.PackStart( new Gtk.Label( "Correct answer:" ), false, false, 5 );
		hBoxCorrect.PackStart( this.spNumberValidAnswer, false, false, 5 );
		hBoxCorrect.PackEnd( bttAnswers, false, false, 5 );

		// Final layout
		var vBoxAnswers = new Gtk.Box( Gtk.Orientation.Vertical, 5 );
		vBoxAnswers.PackStart( swScrolledAnswers, true, true, 5 );
		vBoxAnswers.PackStart( hBoxCorrect, false, false, 5 );

		// Frame
		var toret = new Gtk.Frame( "Answer" );
		( (Gtk.Label) toret.LabelWidget ).Markup = "<b>Answer</b>";
		toret.Add( vBoxAnswers );

		return toret;
	}

	private Gtk.Paned BuildEditDocumentFrame()
	{
		var toret = new Gtk.Paned( Gtk.Orientation.Vertical );

		toret.Pack1( this.BuildEditQuestionFrame(), false, false );
		toret.Pack2( this.BuildEditAnswersFrame(), true, true );

		return toret;
	}

	private Gtk.ScrolledWindow BuildDocumentTextView()
	{
		this.txtDocument.FocusOutEvent += (o, args) => this.StoreQuestionText();
		return new Gtk.ScrolledWindow { this.txtDocument };
	}

	private void BuildNotebook()
	{
		var hBoxTestAndDocument = new Gtk.Paned( Gtk.Orientation.Horizontal );
		this.nbDocPages.SwitchPage += (o, args) => this.OnCurrentPageChanged();

		// Test treeview
		var swScroll = new Gtk.ScrolledWindow();
		var frmTest = new Gtk.Frame( "Test" );
		( (Gtk.Label) frmTest.LabelWidget ).Markup = "<b>Test</b>";
		frmTest.Add( swScroll );
		swScroll.Add( this.tvDocument );

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
		var vBox = new Gtk.Box( Gtk.Orientation.Vertical, 5 );

		this.BuildIcons();
		this.BuildActions();
		this.BuildMenu();
		this.BuildToolbar();
		this.BuildNotebook();
		this.BuildStatusbar();

		vBox.PackStart( this.mbMainMenu, false, false, 2 );
		vBox.PackStart( this.tbToolbar, false, false, 2 );
		vBox.PackStart( this.nbDocPages, true, true, 2 );
		vBox.PackStart( this.sbStatus, false, false, 2 );
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

	private Gdk.Pixbuf? iconAbout;
	private Gdk.Pixbuf? iconImport;
	private Gdk.Pixbuf? iconExport;
	private Gdk.Pixbuf? iconFind;
	private Gdk.Pixbuf? iconNew;
	private Gdk.Pixbuf? iconOpen;
	private Gdk.Pixbuf? iconAdd;
	private Gdk.Pixbuf? iconPlay;
	private Gdk.Pixbuf? iconShuffle;
	private Gdk.Pixbuf? iconRemove;
	private Gdk.Pixbuf? iconSave;
	private Gdk.Pixbuf? iconClose;
	private Gdk.Pixbuf? iconExit;
	private Gdk.Pixbuf? iconTesty;

	private GtkUtil.UIAction actImport;
	private GtkUtil.UIAction actFind;
	private GtkUtil.UIAction actNew;
	private GtkUtil.UIAction actOpen;
	private GtkUtil.UIAction actAppend;
	private GtkUtil.UIAction actSave;
	private GtkUtil.UIAction actSaveAs;
	private GtkUtil.UIAction actExport;
	private GtkUtil.UIAction actClose;
	private GtkUtil.UIAction actQuit;

	private GtkUtil.UIAction actAddQuestion;
	private GtkUtil.UIAction actRemoveQuestion;
	private GtkUtil.UIAction actAddAnswer;
	private GtkUtil.UIAction actRemoveAnswer;

	private GtkUtil.UIAction actTakeTest;
	private GtkUtil.UIAction actAbout;
	private GtkUtil.UIAction actShuffle;

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
