// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


using Testy.Core;

namespace Testy.Gui;


public partial class MainWindow {
	private const string IconResourceExtension = ".png";

	private void LoadIconsFromAssets()
	{
		string[] actualResPaths = this.GetType().Assembly.GetManifestResourceNames();

		try {
			foreach(string actualPath in actualResPaths) {
				if ( actualPath.EndsWith( IconResourceExtension ) ) {
					// Find its name
					string name = actualPath[ 0.. ^IconResourceExtension.Length ];
					int posLastDot = name.LastIndexOf( '.' );

					if ( posLastDot >= 0 ) {
						// Get name and store
						name = name[ ( posLastDot + 1 ).. ].Trim().ToLower();
						this.icons.Add( name,
										Gdk.Pixbuf.LoadFromResource( actualPath ) );
					}
				}
			}

			this.Icon = this.icons[ AppInfo.Name.ToLower() ];
			this.tbToolbar.Style = Gtk.ToolbarStyle.Icons;
		} catch(Exception) {
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
										  Gtk.IconSize.Button )
						    .Pixbuf;

		this.actImport.Icon = this.icons.GetValueOrDefault( "import" ) ?? iconQuestion;
		this.actFind.Icon = this.icons.GetValueOrDefault( "find" ) ?? iconQuestion;
		this.actNew.Icon = this.icons.GetValueOrDefault( "new" ) ?? iconQuestion;
		this.actOpen.Icon = this.icons.GetValueOrDefault( "open" ) ?? iconQuestion;
		this.actSave.Icon = this.icons.GetValueOrDefault( "save" ) ?? iconQuestion;
		this.actExport.Icon = this.icons.GetValueOrDefault( "export" ) ?? iconQuestion;
		this.actClose.Icon = this.icons.GetValueOrDefault( "close" ) ?? iconQuestion;
		this.actQuit.Icon = this.icons.GetValueOrDefault( "exit" ) ?? iconQuestion;
		this.actAddQuestion.Icon = this.icons.GetValueOrDefault( "add" ) ?? iconQuestion;
		this.actRemoveQuestion.Icon = this.icons.GetValueOrDefault( "remove" ) ?? iconQuestion;
		this.actAddAnswer.Icon = this.icons.GetValueOrDefault( "add" ) ?? iconQuestion;
		this.actRemoveAnswer.Icon = this.icons.GetValueOrDefault( "remove" ) ?? iconQuestion;
		this.actShuffle.Icon = this.icons.GetValueOrDefault( "shuffle" ) ?? iconQuestion;
		this.actTakeTest.Icon = this.icons.GetValueOrDefault( "play" ) ?? iconQuestion;
		this.actAbout.Icon = this.icons.GetValueOrDefault( "about" ) ?? iconQuestion;
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

		this.LoadIconsFromAssets();
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
	}

	private readonly Dictionary<string, Gdk.Pixbuf?> icons;

	private readonly GtkUtil.UIAction actImport;
	private readonly GtkUtil.UIAction actFind;
	private readonly GtkUtil.UIAction actNew;
	private readonly GtkUtil.UIAction actOpen;
	private readonly GtkUtil.UIAction actAppend;
	private readonly GtkUtil.UIAction actSave;
	private readonly GtkUtil.UIAction actSaveAs;
	private readonly GtkUtil.UIAction actExport;
	private readonly GtkUtil.UIAction actClose;
	private readonly GtkUtil.UIAction actQuit;

	private readonly GtkUtil.UIAction actAddQuestion;
	private readonly GtkUtil.UIAction actRemoveQuestion;
	private readonly GtkUtil.UIAction actAddAnswer;
	private readonly GtkUtil.UIAction actRemoveAnswer;

	private readonly GtkUtil.UIAction actTakeTest;
	private readonly GtkUtil.UIAction actAbout;
	private readonly GtkUtil.UIAction actShuffle;

	private readonly Gtk.Toolbar tbToolbar;
	private readonly Gtk.MenuBar mbMainMenu;
	private readonly Gtk.Statusbar sbStatus;
	private readonly Gtk.Label lblStatusNumber;
	private readonly Gtk.Notebook nbDocPages;
	private readonly Gtk.TextView txtDocument;
	private readonly Gtk.SpinButton spNumberValidAnswer;
	private readonly Gtk.TextView edQuestionText;
	private readonly Gtk.TreeView tvDocument;
	private readonly Gtk.TreeView tvAnswers;
	private readonly Gtk.Button btAddAnswer;
	private readonly Gtk.Button btRemoveAnswer;
}
