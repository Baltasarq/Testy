using System;
using Testy.Core;

namespace Testy.Gui {
	public partial class DlgTakeTest: Gtk.Dialog {
		public DlgTakeTest(Gtk.Window wndParent, Document doc) {
			this.doc = doc;
			this.workMode = Mode.Test;
			this.questionNumber = -1;
			this.Build();

			// Build all decorations
			this.Modal = true;
			this.Title = "Taking test: " + this.Document.Title;
			this.TransientFor = wndParent;
		}

		private void BuildActions() {
			this.actNext = new Gtk.Action( "next", "_Next", "next question", Gtk.Stock.MediaNext );
			this.actNext.Activated += (sender, e) => this.NextQuestion();

			this.actPrev = new Gtk.Action( "next", "_Previous", "previous question", Gtk.Stock.MediaPrevious );
			this.actPrev.Activated += (sender, e) => this.PreviousQuestion();

			this.actQuit = new Gtk.Action( "quit", "_Quit", "quit", Gtk.Stock.Quit );
			this.actQuit.Activated += (sender, e) => this.Quit();

			this.actCheck = new Gtk.Action( "check", "_Check", "check answers", Gtk.Stock.Apply );
			this.actCheck.Activated += (sender, e) => this.SetCorrectionMode();
		}

		private void BuildToolbar() {
			this.tbToolbar = new Gtk.Toolbar();

			this.tbToolbar.Add( this.actPrev.CreateToolItem() );
			this.tbToolbar.Add( this.actNext.CreateToolItem() );
			this.tbToolbar.Add( this.actCheck.CreateToolItem() );
			this.tbToolbar.Add( this.actQuit.CreateToolItem() );
		}

		private void BuildQuestionView() {
			this.scrlQuestion = new Gtk.ScrolledWindow();

			this.txtQuestion = new Gtk.TextView() { Editable = false };
			this.scrlQuestion.AddWithViewport( this.txtQuestion );
		}

		private void BuildComboAnswers() {
			this.cbAnswers = new Gtk.ComboBox( new string[]{} );
			this.cbAnswers.Changed += (sender, e) => this.OnAnswerChanged();
		}

		private void BuildActionbar() {
			this.lblTitle = new Gtk.Label();
			this.lblCorrect = new Gtk.Label();
			this.lblNumber = new Gtk.Label();
			this.hbxChkContainer = new Gtk.HBox( false, 5 );

			this.cbQuestionNumber = new Gtk.ComboBox( new string[]{} );
			this.cbQuestionNumber.Changed += (sender, e) => this.OnQuestionNumberChanged();
			this.lblTitle.Text = this.Document.Title + ':';
			this.lblCorrect.Text = "";
			this.lblNumber.Text = this.Document.CountQuestions.ToString();

			this.hbxChkContainer.PackStart( this.cbQuestionNumber, false, false, 5 );
			this.hbxChkContainer.PackEnd( this.lblNumber, false, false, 5 );
			this.hbxChkContainer.PackEnd( this.lblCorrect, false, false, 5 );
			this.hbxChkContainer.PackEnd( this.lblTitle, false, false, 5 );
		}

		private void Build() {
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
				new Gdk.Geometry() {
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
			this.imgCorrection = new Gtk.Image( "gtk-yes", Gtk.IconSize.Dialog );
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
			Pango.FontDescription font = new Pango.FontDescription();
			font.Family = "Monospace";
			txtCorrection.ModifyFont( font );
			txtCorrection.WrapMode = Gtk.WrapMode.None;
			txtCorrection.Editable = false;

			// Prepare the expander
			this.expCorrection.Activated += delegate(System.Object o, System.EventArgs e) {
				bool isExpanded = this.expCorrection.Expanded;

				this.VBox.SetChildPacking( this.hbxChkContainer, isExpanded, isExpanded, 5, Gtk.PackType.End );
			};

			this.hbxChkContainer.ShowAll();
			return;
		}

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

