using Testy.Core;

namespace Testy.Gui {
	public partial class DlgTakeTest: Gtk.Dialog {
		public DlgTakeTest(Gtk.Window wndParent, Document doc)
		{
			this.doc = doc;
			this.workMode = Mode.Test;
			this.questionNumber = -1;
			this.Build();

			// Build all decorations
			this.Modal = true;
			this.Title = "Taking test: " + this.Document.Title;
			this.TransientFor = wndParent;
		}

		private void BuildIcons()
		{
			this.iconNext = new Gdk.Pixbuf(
				System.Reflection.Assembly.GetEntryAssembly(),
				"Testy.Res.next.png", 32, 32 );
			Gtk.IconTheme.AddBuiltinIcon( "tsty-next", 32, this.iconNext );

			this.iconPrevious = new Gdk.Pixbuf(
				System.Reflection.Assembly.GetEntryAssembly(),
				"Testy.Res.previous.png", 32, 32 );
			Gtk.IconTheme.AddBuiltinIcon( "tsty-previous", 32, this.iconPrevious );

			this.iconCheck = new Gdk.Pixbuf(
				System.Reflection.Assembly.GetEntryAssembly(),
				"Testy.Res.check.png", 32, 32 );
			Gtk.IconTheme.AddBuiltinIcon( "tsty-check", 32, this.iconCheck );

			this.iconClose = new Gdk.Pixbuf(
				System.Reflection.Assembly.GetEntryAssembly(),
				"Testy.Res.close.png", 32, 32 );

		}

		private void BuildActions()
		{
			this.actNext = new Gtk.Action( "next", "_Next", "next question", "next" ) { IconName = "tsty-next" };
			this.actNext.Activated += (sender, e) => this.NextQuestion();

			this.actPrev = new Gtk.Action( "previous", "_Previous", "previous question", "previous" ) { IconName = "tsty-previous" };
			this.actPrev.Activated += (sender, e) => this.PreviousQuestion();

			this.actQuit = new Gtk.Action( "quit", "_Quit", "quit", "quit" ) { IconName = "tsty-exit" };
			this.actQuit.Activated += (sender, e) => this.Quit();

			this.actCheck = new Gtk.Action( "check", "_Check", "check answers", "check" ) { IconName = "tsty-check" };
			this.actCheck.Activated += (sender, e) => this.SetCorrectionMode();
		}

		private void BuildToolbar()
		{
			this.tbToolbar = new Gtk.Toolbar();

			this.tbToolbar.Add( this.actPrev.CreateToolItem() );
			this.tbToolbar.Add( this.actNext.CreateToolItem() );
			this.tbToolbar.Add( this.actCheck.CreateToolItem() );
			this.tbToolbar.Add( this.actQuit.CreateToolItem() );
		}

		private void BuildQuestionView()
		{
			this.scrlQuestion = new Gtk.ScrolledWindow();

			this.txtQuestion = new Gtk.TextView { Editable = false };
			this.scrlQuestion.AddWithViewport( this.txtQuestion );
		}

		private void BuildComboAnswers()
		{
			this.cbAnswers = Gtk.ComboBox.NewText();
			this.cbAnswers.Changed += (sender, e) => this.OnAnswerChanged();
		}

		private void BuildActionbar()
		{
			this.lblTitle = new Gtk.Label();
			this.lblCorrect = new Gtk.Label();
			this.lblNumber = new Gtk.Label();
			this.hbxChkContainer = new Gtk.HBox( false, 5 );

			this.cbQuestionNumber = Gtk.ComboBox.NewText();
			this.cbQuestionNumber.Changed += (sender, e) => this.OnQuestionNumberChanged();
			this.lblTitle.Text = this.Document.Title + ':';
			this.lblCorrect.Text = "";
			this.lblNumber.Text = this.Document.CountQuestions.ToString();

			this.hbxChkContainer.PackStart( this.cbQuestionNumber, false, false, 5 );
			this.hbxChkContainer.PackEnd( this.lblNumber, false, false, 5 );
			this.hbxChkContainer.PackEnd( this.lblCorrect, false, false, 5 );
			this.hbxChkContainer.PackEnd( this.lblTitle, false, false, 5 );
		}

		private void Build()
		{
			this.BuildIcons();
			this.BuildActions();
			this.BuildToolbar();
			this.BuildQuestionView();
			this.BuildComboAnswers();
			this.BuildActionbar();

			this.VBox.PackStart( this.tbToolbar, false, false, 0 );
			this.VBox.PackStart( this.scrlQuestion, true, true, 0 );
			this.VBox.PackStart( this.cbAnswers, false, false, 0 );
			this.VBox.PackStart( this.hbxChkContainer, false, false, 0 );

			// Set min size
			this.SetGeometryHints(
				this,
				new Gdk.Geometry {
					MinWidth = 640,
					MinHeight = 480 },
				Gdk.WindowHints.MinSize
			);

			this.WindowPosition = Gtk.WindowPosition.CenterOnParent;
			this.ActionArea.Hide();
			this.Shown += (sender, e) => this.Init();
			this.ShowAll();
		}

		/// <summary>
		/// Builds the check framework.
		/// </summary>
		private void BuildCheckFramework()
		{
			this.actCheck.Sensitive = false;
			this.txtCorrection = new Gtk.TextView();
			this.imgCorrection = new Gtk.Image( "tsty-check" );
			this.expCorrection = new Gtk.Expander( "Check" );
			this.hbxChkContainer = new Gtk.HBox( false, 5 );
			var scrl = new Gtk.ScrolledWindow();

			// Build the view
			scrl.Add( this.txtCorrection );
			this.expCorrection.Add( scrl );
			this.hbxChkContainer.PackStart( this.imgCorrection, false, false, 5 );
			this.hbxChkContainer.PackEnd( this.expCorrection, true, true, 5 );
			this.VBox.Add( this.hbxChkContainer );


			// Prepare the txtCorrection TextView
			var font = new Pango.FontDescription();
			font.Family = "Monospace";
			txtCorrection.ModifyFont( font );
			txtCorrection.WrapMode = Gtk.WrapMode.None;
			txtCorrection.Editable = false;

			// Prepare the expander
			this.expCorrection.Activated += (o, e) => {
				bool isExpanded = this.expCorrection.Expanded;
				this.VBox.SetChildPacking( this.hbxChkContainer, isExpanded, isExpanded, 5, Gtk.PackType.End );
			};

			this.hbxChkContainer.ShowAll();
			return;
		}

		private Gdk.Pixbuf iconNext;
		private Gdk.Pixbuf iconPrevious;
		private Gdk.Pixbuf iconCheck;
		private Gdk.Pixbuf iconClose;

		private Gtk.Action actNext;
		private Gtk.Action actPrev;
		private Gtk.Action actQuit;
		private Gtk.Action actCheck;

		private Gtk.Toolbar tbToolbar;
		private Gtk.TextView txtCorrection;
		private Gtk.Image imgCorrection;
		private Gtk.Expander expCorrection;
		private Gtk.HBox hbxChkContainer;
		private Gtk.ComboBox cbAnswers;
		private Gtk.ComboBox cbQuestionNumber;
		private Gtk.ScrolledWindow scrlQuestion;
		private Gtk.TextView txtQuestion;
		private Gtk.Label lblTitle;
		private Gtk.Label lblNumber;
		private Gtk.Label lblCorrect;

	}
}

