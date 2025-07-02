// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Gui;


using Core;


public partial class DlgTakeTest: Gtk.Dialog {
	public DlgTakeTest(
				Gtk.Window wndParent,
				Document doc,
				Dictionary<string, Gdk.Pixbuf?> icons)
	{
		this.icons = icons;
		this.lblTitle = new Gtk.Label();
		this.lblNumber = new Gtk.Label();
		this.cbAnswers = new Gtk.ComboBoxText();
		this.cbQuestionNumber = new Gtk.ComboBoxText();
		this.txtQuestion = new Gtk.TextView {
			Editable = false,
			WrapMode = Gtk.WrapMode.Word
		};

		this.btNext = new Gtk.ToolButton(
								new Gtk.Image( icons[ "next" ] ),
								"next" );
		this.btNext.Clicked += (o, e) => this.NextQuestion();

		this.btPrev = new Gtk.ToolButton(
								new Gtk.Image( icons[ "previous" ] ),
								"previous" );
		this.btPrev.Clicked += (o, e) => this.PreviousQuestion();

		this.btCheck = new Gtk.ToolButton(
								new Gtk.Image( icons[ "check" ] ),
								"check" );
		this.btCheck.Clicked += (o, e) => this.Check();

		this.btQuit = new Gtk.ToolButton(
								new Gtk.Image( icons[ "close" ] ),
								"close" );
		this.btQuit.Clicked += (o, e) => this.Quit();

		this.tbToolbar = new Gtk.Toolbar {
			this.btPrev,
			this.btNext,
			this.btCheck,
			this.btQuit
		};

		this.Document = doc;
		this.answers = new List<int>(
							Enumerable.Repeat( 0, this.Document.CountQuestions ) );
		this.QuestionNumber = -1;
		this.Build();

		// Build all decorations
		this.Modal = true;
		this.Title = this.Document.Title;
		this.TransientFor = wndParent;
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
		var vbox = (Gtk.Box) this.Child;

		this.BuildComboAnswers();

		vbox.PackStart( this.tbToolbar, false, false, 0 );
		vbox.PackStart( this.BuildQuestionView(), true, true, 5 );
		vbox.PackStart( this.cbAnswers, false, false, 5 );
		vbox.PackStart( this.BuildActionbar(), false, false, 0 );

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

	private readonly Dictionary<string, Gdk.Pixbuf?> icons;

	private readonly Gtk.ToolButton btNext;
	private readonly Gtk.ToolButton btPrev;
	private readonly Gtk.ToolButton btQuit;
	private readonly Gtk.ToolButton btCheck;

	private readonly Gtk.Toolbar tbToolbar;
	private readonly Gtk.ComboBoxText cbAnswers;
	private readonly Gtk.ComboBoxText cbQuestionNumber;
	private readonly Gtk.TextView txtQuestion;
	private readonly Gtk.Label lblTitle;
	private readonly Gtk.Label lblNumber;
}
