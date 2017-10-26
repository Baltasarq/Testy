using Testy.Core;

namespace Testy.Gui {
	public partial class DlgTakeTest: Gtk.Dialog {
		public DlgTakeTest(Gtk.Window wndParent, Document doc)
		{
			this.Document = doc;
			this.QuestionNumber = -1;
			this.Build();

			// Build all decorations
			this.Modal = true;
			this.Title = this.Document.Title;
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

			this.iconClose = new Gdk.Pixbuf(
				System.Reflection.Assembly.GetEntryAssembly(),
				"Testy.Res.close.png", 32, 32 );
                
            this.iconCheck = new Gdk.Pixbuf(
                System.Reflection.Assembly.GetEntryAssembly(),
                "Testy.Res.check.png", 32, 32 );
            Gtk.IconTheme.AddBuiltinIcon( "tsty-check", 32, this.iconCheck );
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
			this.actCheck.Activated += (sender, e) => this.Check();
		}

		private void BuildToolbar()
		{
            this.tbToolbar = new Gtk.Toolbar {
                this.actPrev.CreateToolItem (),
                this.actNext.CreateToolItem (),
                this.actCheck.CreateToolItem (),
                this.actQuit.CreateToolItem ()
            };
        }

		private Gtk.ScrolledWindow BuildQuestionView()
		{
			this.txtQuestion = new Gtk.TextView {
                Editable = false,
                WrapMode = Gtk.WrapMode.Word
            };

            var scrlQuestion = new Gtk.ScrolledWindow();            
			scrlQuestion.AddWithViewport( this.txtQuestion );

            return scrlQuestion;
		}

		private void BuildComboAnswers()
		{
			this.cbAnswers = Gtk.ComboBox.NewText();
			this.cbAnswers.Changed += (sender, e) => this.OnAnswerChanged();
		}

		private Gtk.HBox BuildActionbar()
		{
			this.lblTitle = new Gtk.Label();
			this.lblNumber = new Gtk.Label();

			this.cbQuestionNumber = Gtk.ComboBox.NewText();
			this.cbQuestionNumber.Changed += (sender, e) => this.OnQuestionNumberChanged();
			this.lblTitle.Text = this.Document.Title + ':';
			this.lblNumber.Text = this.Document.CountQuestions.ToString();

            var hbxChkContainer = new Gtk.HBox( false, 5 );
			hbxChkContainer.PackStart( this.cbQuestionNumber, false, false, 5 );
			hbxChkContainer.PackEnd( this.lblNumber, false, false, 5 );
			hbxChkContainer.PackEnd( this.lblTitle, false, false, 5 );
            
            return hbxChkContainer;
		}

		private void Build()
		{
			this.BuildIcons();
			this.BuildActions();
			this.BuildToolbar();
			this.BuildComboAnswers();

			this.VBox.PackStart( this.tbToolbar, false, false, 0 );
			this.VBox.PackStart( this.BuildQuestionView(), true, true, 5 );
			this.VBox.PackStart( this.cbAnswers, false, false, 5 );
			this.VBox.PackStart( this.BuildActionbar(), false, false, 0 );

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
            this.DeleteEvent += (sender, e) => this.Quit();
			this.ShowAll();
		}

		private Gdk.Pixbuf iconNext;
		private Gdk.Pixbuf iconPrevious;
		private Gdk.Pixbuf iconClose;
        private Gdk.Pixbuf iconCheck;

		private Gtk.Action actNext;
		private Gtk.Action actPrev;
		private Gtk.Action actQuit;
		private Gtk.Action actCheck;

		private Gtk.Toolbar tbToolbar;
		private Gtk.ComboBox cbAnswers;
		private Gtk.ComboBox cbQuestionNumber;
		private Gtk.TextView txtQuestion;
		private Gtk.Label lblTitle;
		private Gtk.Label lblNumber;
	}
}

