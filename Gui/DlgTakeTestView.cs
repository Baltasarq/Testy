// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


using Core;


public partial class DlgTakeTest: Gtk.Dialog {
	public DlgTakeTest(Gtk.Window wndParent, Document doc)
	{
		this.lblTitle = new Gtk.Label();
		this.lblNumber = new Gtk.Label();
		this.cbAnswers = new Gtk.ComboBoxText();
		this.cbQuestionNumber = new Gtk.ComboBoxText();
		this.txtQuestion = new Gtk.TextView {
			Editable = false,
			WrapMode = Gtk.WrapMode.Word
		};

		this.actNext = new GtkUtil.UIAction( "next", "_Next", "next question" );
		this.actPrev = new GtkUtil.UIAction( "previous", "_Previous", "previous question" );
		this.actQuit = new GtkUtil.UIAction( "close", "_Close", "close" );
		this.actCheck = new GtkUtil.UIAction( "check", "_Check", "check answers" );

		this.tbToolbar = new Gtk.Toolbar {
			this.actPrev.CreateToolButton(),
			this.actNext.CreateToolButton(),
			this.actCheck.CreateToolButton(),
			this.actQuit.CreateToolButton()
		};

		this.Document = doc;
		this.answers = new List<int>( doc.CountQuestions );
		this.QuestionNumber = -1;
		this.Build();

		// Build all decorations
		this.Modal = true;
		this.Title = this.Document.Title;
		this.TransientFor = wndParent;
	}

	private void BuildIcons()
	{
		try {
			this.iconNext = Gdk.Pixbuf.LoadFromResource( "Testy.next.png" );
			this.iconPrevious = Gdk.Pixbuf.LoadFromResource( "Testy.previous.png" );
			this.iconCheck = Gdk.Pixbuf.LoadFromResource( "Testy.check.png" );

			this.tbToolbar.Style = Gtk.ToolbarStyle.Text;
		} catch(Exception) {
			this.tbToolbar.Style = Gtk.ToolbarStyle.Text;
		}
	}

	private void BuildActions()
	{
		this.actNext.Activated += (sender, e) => this.NextQuestion();
		this.actPrev.Activated += (sender, e) => this.PreviousQuestion();
		this.actQuit.Activated += (sender, e) => this.Quit();
		this.actCheck.Activated += (sender, e) => this.Check();

		if ( this.iconNext is not null ) {
			this.actNext.Icon = this.iconNext;
		}

		if ( this.iconPrevious is not null ) {
			this.actPrev.Icon = this.iconPrevious;
		}

		if ( this.iconCheck is not null ) {
			this.actCheck.Icon = this.iconCheck;
		}
	}

	private Gtk.ScrolledWindow BuildQuestionView()
	{
		return [ this.txtQuestion ];
	}

	private void BuildComboAnswers()
	{
		this.cbAnswers.Changed += (sender, e) => this.OnAnswerChanged();
	}

	private Gtk.Box BuildActionbar()
	{
		this.cbQuestionNumber.Changed += (sender, e) => this.OnQuestionNumberChanged();
		this.lblTitle.Text = this.Document.Title + ':';
		this.lblNumber.Text = this.Document.CountQuestions.ToString();

		var hbxChkContainer = new Gtk.Box( Gtk.Orientation.Horizontal, 5 );
		hbxChkContainer.PackStart( this.cbQuestionNumber, false, false, 5 );
		hbxChkContainer.PackEnd( this.lblNumber, false, false, 5 );
		hbxChkContainer.PackEnd( this.lblTitle, false, false, 5 );

		return hbxChkContainer;
	}

	private void Build()
	{
		var vbox = new Gtk.Box( Gtk.Orientation.Vertical, 5 );

		this.BuildIcons();
		this.BuildActions();
		this.BuildComboAnswers();

		vbox.PackStart( this.tbToolbar, false, false, 0 );
		vbox.PackStart( this.BuildQuestionView(), true, true, 5 );
		vbox.PackStart( this.cbAnswers, false, false, 5 );
		vbox.PackStart( this.BuildActionbar(), false, false, 0 );
		this.Add( vbox );

		// Set min size
		this.SetGeometryHints(
			this,
			new Gdk.Geometry {
				MinWidth = 640,
				MinHeight = 480 },
			Gdk.WindowHints.MinSize
		);

		this.WindowPosition = Gtk.WindowPosition.CenterOnParent;
		this.Shown += (sender, e) => this.Init();
		this.DeleteEvent += (sender, e) => this.Quit();
		this.ShowAll();
	}

	private Gdk.Pixbuf? iconNext;
	private Gdk.Pixbuf? iconPrevious;
	private Gdk.Pixbuf? iconCheck;

	private GtkUtil.UIAction actNext;
	private GtkUtil.UIAction actPrev;
	private GtkUtil.UIAction actQuit;
	private GtkUtil.UIAction actCheck;

	private Gtk.Toolbar tbToolbar;
	private Gtk.ComboBoxText cbAnswers;
	private Gtk.ComboBoxText cbQuestionNumber;
	private Gtk.TextView txtQuestion;
	private Gtk.Label lblTitle;
	private Gtk.Label lblNumber;
}
