
// This file has been generated by the GUI designer. Do not modify.
namespace Testy.Gui
{
	public partial class MainWindow
	{
		private global::Gtk.UIManager UIManager;
		
		private global::Gtk.Action FileAction;
		
		private global::Gtk.Action EditAction;
		
		private global::Gtk.Action HelpAction;
		
		private global::Gtk.Action aboutAction;
		
		private global::Gtk.Action newAction;
		
		private global::Gtk.Action openAction;
		
		private global::Gtk.Action saveAction;
		
		private global::Gtk.Action closeAction;
		
		private global::Gtk.Action quitAction;
		
		private global::Gtk.Action addAction;
		
		private global::Gtk.Action removeAction;
		
		private global::Gtk.Action convertAction;
		
		private global::Gtk.Action addAnswerAction;
		
		private global::Gtk.Action removeAnswerAction;
		
		private global::Gtk.Action saveAsAction;
		
		private global::Gtk.Action ToolsAction;
		
		private global::Gtk.Action takeTestAction;
		
		private global::Gtk.VBox vbox1;
		
		private global::Gtk.MenuBar menubar1;
		
		private global::Gtk.Toolbar toolbar1;
		
		private global::Gtk.Notebook nbDocPages;
		
		private global::Gtk.HPaned hpMainPane;
		
		private global::Gtk.Frame frame3;
		
		private global::Gtk.Alignment GtkAlignment2;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		
		private global::Gtk.TreeView tvDocument;
		
		private global::Gtk.Label frDocument;
		
		private global::Gtk.VBox vbox2;
		
		private global::Gtk.Frame frQuestion;
		
		private global::Gtk.Alignment GtkAlignment;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow1;
		
		private global::Gtk.TextView edQuestionText;
		
		private global::Gtk.Label GtkLabel5;
		
		private global::Gtk.Frame frame2;
		
		private global::Gtk.Alignment GtkAlignment1;
		
		private global::Gtk.VBox vbox3;
		
		private global::Gtk.HBox hbox1;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow2;
		
		private global::Gtk.TreeView tvAnswers;
		
		private global::Gtk.VBox vbox4;
		
		private global::Gtk.Button btAddAnswer;
		
		private global::Gtk.Button btRemoveAnswer;
		
		private global::Gtk.HBox hbox2;
		
		private global::Gtk.Label label1;
		
		private global::Gtk.SpinButton spNumberValidAnswer;
		
		private global::Gtk.Alignment alignment1;
		
		private global::Gtk.Label frAnswers;
		
		private global::Gtk.Label label2;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow3;
		
		private global::Gtk.TextView txtDocument;
		
		private global::Gtk.Label label3;
		
		private global::Gtk.Statusbar stStatusBar;
		
		private global::Gtk.Label lblStatusNumber;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Testy.Gui.MainWindow
			this.UIManager = new global::Gtk.UIManager ();
			global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
			this.FileAction = new global::Gtk.Action ("FileAction", "_File", null, null);
			this.FileAction.ShortLabel = "_File";
			w1.Add (this.FileAction, null);
			this.EditAction = new global::Gtk.Action ("EditAction", "_Edit", null, null);
			this.EditAction.ShortLabel = "_Edit";
			w1.Add (this.EditAction, null);
			this.HelpAction = new global::Gtk.Action ("HelpAction", "_Help", null, null);
			this.HelpAction.ShortLabel = "_Help";
			w1.Add (this.HelpAction, null);
			this.aboutAction = new global::Gtk.Action ("aboutAction", "_About", null, "gtk-about");
			this.aboutAction.ShortLabel = "_About";
			w1.Add (this.aboutAction, null);
			this.newAction = new global::Gtk.Action ("newAction", "_New", null, "gtk-new");
			this.newAction.ShortLabel = "_New";
			w1.Add (this.newAction, null);
			this.openAction = new global::Gtk.Action ("openAction", "_Open", null, "gtk-open");
			this.openAction.ShortLabel = "_Open";
			w1.Add (this.openAction, null);
			this.saveAction = new global::Gtk.Action ("saveAction", "_Save", null, "gtk-save");
			this.saveAction.ShortLabel = "_Save";
			w1.Add (this.saveAction, null);
			this.closeAction = new global::Gtk.Action ("closeAction", "_Close", null, "gtk-close");
			this.closeAction.ShortLabel = "_Close";
			w1.Add (this.closeAction, null);
			this.quitAction = new global::Gtk.Action ("quitAction", "_Quit", null, "gtk-quit");
			this.quitAction.ShortLabel = "_Quit";
			w1.Add (this.quitAction, null);
			this.addAction = new global::Gtk.Action ("addAction", "_Add question", null, "gtk-add");
			this.addAction.ShortLabel = "_Add question";
			w1.Add (this.addAction, "<Control>plus");
			this.removeAction = new global::Gtk.Action ("removeAction", "_Remove question", null, "gtk-remove");
			this.removeAction.ShortLabel = "_Remove question";
			w1.Add (this.removeAction, "<Control>minus");
			this.convertAction = new global::Gtk.Action ("convertAction", "_Export", null, "gtk-convert");
			this.convertAction.ShortLabel = "_Export";
			w1.Add (this.convertAction, null);
			this.addAnswerAction = new global::Gtk.Action ("addAnswerAction", "Add answer to current question", null, "gtk-add");
			this.addAnswerAction.ShortLabel = "Add answer to current question";
			w1.Add (this.addAnswerAction, "<Primary>asterisk");
			this.removeAnswerAction = new global::Gtk.Action ("removeAnswerAction", "Remove current answer in current question", null, "gtk-remove");
			this.removeAnswerAction.ShortLabel = "Remove current answer in current question";
			w1.Add (this.removeAnswerAction, "<Primary>underscore");
			this.saveAsAction = new global::Gtk.Action ("saveAsAction", "Save _As", null, "gtk-save-as");
			this.saveAsAction.ShortLabel = "Save _As";
			w1.Add (this.saveAsAction, null);
			this.ToolsAction = new global::Gtk.Action ("ToolsAction", "_Tools", null, null);
			this.ToolsAction.ShortLabel = "_Tools";
			w1.Add (this.ToolsAction, null);
			this.takeTestAction = new global::Gtk.Action ("takeTestAction", "_Take test", null, "gtk-media-play");
			this.takeTestAction.ShortLabel = "_Take test";
			w1.Add (this.takeTestAction, null);
			this.UIManager.InsertActionGroup (w1, 0);
			this.AddAccelGroup (this.UIManager.AccelGroup);
			this.Name = "Testy.Gui.MainWindow";
			this.Title = "MainWindow";
			this.Icon = global::Gdk.Pixbuf.LoadFromResource ("Testy.Res.testy.ico");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child Testy.Gui.MainWindow.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString (@"<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'><menuitem name='newAction' action='newAction'/><menuitem name='openAction' action='openAction'/><menuitem name='saveAction' action='saveAction'/><menuitem name='saveAsAction' action='saveAsAction'/><menuitem name='convertAction' action='convertAction'/><menuitem name='closeAction' action='closeAction'/><menuitem name='quitAction' action='quitAction'/></menu><menu name='EditAction' action='EditAction'><menuitem name='addAction' action='addAction'/><menuitem name='removeAction' action='removeAction'/><menuitem name='addAnswerAction' action='addAnswerAction'/><menuitem name='removeAnswerAction' action='removeAnswerAction'/></menu><menu name='ToolsAction' action='ToolsAction'><menuitem name='takeTestAction' action='takeTestAction'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='aboutAction' action='aboutAction'/></menu></menubar></ui>");
			this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
			this.menubar1.Name = "menubar1";
			this.vbox1.Add (this.menubar1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menubar1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString (@"<ui><toolbar name='toolbar1'><toolitem name='newAction' action='newAction'/><toolitem name='openAction' action='openAction'/><toolitem name='saveAction' action='saveAction'/><toolitem name='closeAction' action='closeAction'/><separator/><toolitem name='addAction' action='addAction'/><toolitem name='removeAction' action='removeAction'/><separator/><toolitem name='takeTestAction' action='takeTestAction'/></toolbar></ui>");
			this.toolbar1 = ((global::Gtk.Toolbar)(this.UIManager.GetWidget ("/toolbar1")));
			this.toolbar1.Name = "toolbar1";
			this.toolbar1.ShowArrow = false;
			this.vbox1.Add (this.toolbar1);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.toolbar1]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.nbDocPages = new global::Gtk.Notebook ();
			this.nbDocPages.CanFocus = true;
			this.nbDocPages.Name = "nbDocPages";
			this.nbDocPages.CurrentPage = 0;
			// Container child nbDocPages.Gtk.Notebook+NotebookChild
			this.hpMainPane = new global::Gtk.HPaned ();
			this.hpMainPane.CanFocus = true;
			this.hpMainPane.Name = "hpMainPane";
			this.hpMainPane.Position = 170;
			// Container child hpMainPane.Gtk.Paned+PanedChild
			this.frame3 = new global::Gtk.Frame ();
			this.frame3.Name = "frame3";
			this.frame3.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child frame3.Gtk.Container+ContainerChild
			this.GtkAlignment2 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment2.Name = "GtkAlignment2";
			this.GtkAlignment2.LeftPadding = ((uint)(12));
			// Container child GtkAlignment2.Gtk.Container+ContainerChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.tvDocument = new global::Gtk.TreeView ();
			this.tvDocument.CanFocus = true;
			this.tvDocument.Name = "tvDocument";
			this.GtkScrolledWindow.Add (this.tvDocument);
			this.GtkAlignment2.Add (this.GtkScrolledWindow);
			this.frame3.Add (this.GtkAlignment2);
			this.frDocument = new global::Gtk.Label ();
			this.frDocument.Name = "frDocument";
			this.frDocument.LabelProp = "<b>Test</b>";
			this.frDocument.UseMarkup = true;
			this.frame3.LabelWidget = this.frDocument;
			this.hpMainPane.Add (this.frame3);
			global::Gtk.Paned.PanedChild w7 = ((global::Gtk.Paned.PanedChild)(this.hpMainPane [this.frame3]));
			w7.Resize = false;
			// Container child hpMainPane.Gtk.Paned+PanedChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.frQuestion = new global::Gtk.Frame ();
			this.frQuestion.Name = "frQuestion";
			this.frQuestion.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child frQuestion.Gtk.Container+ContainerChild
			this.GtkAlignment = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment.Name = "GtkAlignment";
			this.GtkAlignment.LeftPadding = ((uint)(12));
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.edQuestionText = new global::Gtk.TextView ();
			this.edQuestionText.CanFocus = true;
			this.edQuestionText.Name = "edQuestionText";
			this.edQuestionText.AcceptsTab = false;
			this.edQuestionText.WrapMode = ((global::Gtk.WrapMode)(2));
			this.GtkScrolledWindow1.Add (this.edQuestionText);
			this.GtkAlignment.Add (this.GtkScrolledWindow1);
			this.frQuestion.Add (this.GtkAlignment);
			this.GtkLabel5 = new global::Gtk.Label ();
			this.GtkLabel5.Name = "GtkLabel5";
			this.GtkLabel5.LabelProp = "<b>Question</b>";
			this.GtkLabel5.UseMarkup = true;
			this.frQuestion.LabelWidget = this.GtkLabel5;
			this.vbox2.Add (this.frQuestion);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.frQuestion]));
			w11.Position = 0;
			// Container child vbox2.Gtk.Box+BoxChild
			this.frame2 = new global::Gtk.Frame ();
			this.frame2.Name = "frame2";
			this.frame2.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child frame2.Gtk.Container+ContainerChild
			this.GtkAlignment1 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment1.Name = "GtkAlignment1";
			this.GtkAlignment1.LeftPadding = ((uint)(12));
			// Container child GtkAlignment1.Gtk.Container+ContainerChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
			this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
			this.tvAnswers = new global::Gtk.TreeView ();
			this.tvAnswers.CanFocus = true;
			this.tvAnswers.Name = "tvAnswers";
			this.GtkScrolledWindow2.Add (this.tvAnswers);
			this.hbox1.Add (this.GtkScrolledWindow2);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.GtkScrolledWindow2]));
			w13.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.btAddAnswer = new global::Gtk.Button ();
			this.btAddAnswer.CanFocus = true;
			this.btAddAnswer.Name = "btAddAnswer";
			this.btAddAnswer.UseStock = true;
			this.btAddAnswer.UseUnderline = true;
			this.btAddAnswer.Label = "gtk-add";
			this.vbox4.Add (this.btAddAnswer);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.btAddAnswer]));
			w14.Position = 0;
			w14.Expand = false;
			w14.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.btRemoveAnswer = new global::Gtk.Button ();
			this.btRemoveAnswer.CanFocus = true;
			this.btRemoveAnswer.Name = "btRemoveAnswer";
			this.btRemoveAnswer.UseStock = true;
			this.btRemoveAnswer.UseUnderline = true;
			this.btRemoveAnswer.Label = "gtk-remove";
			this.vbox4.Add (this.btRemoveAnswer);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.btRemoveAnswer]));
			w15.Position = 1;
			w15.Expand = false;
			w15.Fill = false;
			this.hbox1.Add (this.vbox4);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox4]));
			w16.Position = 1;
			w16.Expand = false;
			w16.Fill = false;
			this.vbox3.Add (this.hbox1);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox1]));
			w17.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = "Correct answer:";
			this.hbox2.Add (this.label1);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.label1]));
			w18.Position = 0;
			w18.Expand = false;
			w18.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.spNumberValidAnswer = new global::Gtk.SpinButton (1D, 100D, 1D);
			this.spNumberValidAnswer.CanFocus = true;
			this.spNumberValidAnswer.Name = "spNumberValidAnswer";
			this.spNumberValidAnswer.Adjustment.PageIncrement = 10D;
			this.spNumberValidAnswer.ClimbRate = 1D;
			this.spNumberValidAnswer.Numeric = true;
			this.spNumberValidAnswer.Value = 1D;
			this.hbox2.Add (this.spNumberValidAnswer);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.spNumberValidAnswer]));
			w19.Position = 1;
			w19.Expand = false;
			w19.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.alignment1 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment1.Name = "alignment1";
			this.hbox2.Add (this.alignment1);
			global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.alignment1]));
			w20.Position = 2;
			this.vbox3.Add (this.hbox2);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox2]));
			w21.Position = 1;
			w21.Expand = false;
			w21.Fill = false;
			this.GtkAlignment1.Add (this.vbox3);
			this.frame2.Add (this.GtkAlignment1);
			this.frAnswers = new global::Gtk.Label ();
			this.frAnswers.Name = "frAnswers";
			this.frAnswers.LabelProp = "<b>Answers</b>";
			this.frAnswers.UseMarkup = true;
			this.frame2.LabelWidget = this.frAnswers;
			this.vbox2.Add (this.frame2);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.frame2]));
			w24.Position = 1;
			this.hpMainPane.Add (this.vbox2);
			this.nbDocPages.Add (this.hpMainPane);
			// Notebook tab
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.LabelProp = "Edit";
			this.nbDocPages.SetTabLabel (this.hpMainPane, this.label2);
			this.label2.ShowAll ();
			// Container child nbDocPages.Gtk.Notebook+NotebookChild
			this.GtkScrolledWindow3 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow3.Name = "GtkScrolledWindow3";
			this.GtkScrolledWindow3.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow3.Gtk.Container+ContainerChild
			this.txtDocument = new global::Gtk.TextView ();
			this.txtDocument.Buffer.Text = "...";
			this.txtDocument.CanFocus = true;
			this.txtDocument.Name = "txtDocument";
			this.txtDocument.Editable = false;
			this.GtkScrolledWindow3.Add (this.txtDocument);
			this.nbDocPages.Add (this.GtkScrolledWindow3);
			global::Gtk.Notebook.NotebookChild w28 = ((global::Gtk.Notebook.NotebookChild)(this.nbDocPages [this.GtkScrolledWindow3]));
			w28.Position = 1;
			// Notebook tab
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.LabelProp = "Document";
			this.nbDocPages.SetTabLabel (this.GtkScrolledWindow3, this.label3);
			this.label3.ShowAll ();
			this.vbox1.Add (this.nbDocPages);
			global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.nbDocPages]));
			w29.Position = 2;
			// Container child vbox1.Gtk.Box+BoxChild
			this.stStatusBar = new global::Gtk.Statusbar ();
			this.stStatusBar.Name = "stStatusBar";
			this.stStatusBar.Spacing = 6;
			// Container child stStatusBar.Gtk.Box+BoxChild
			this.lblStatusNumber = new global::Gtk.Label ();
			this.lblStatusNumber.Name = "lblStatusNumber";
			this.lblStatusNumber.LabelProp = "label2";
			this.stStatusBar.Add (this.lblStatusNumber);
			global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(this.stStatusBar [this.lblStatusNumber]));
			w30.Position = 2;
			w30.Expand = false;
			w30.Fill = false;
			this.vbox1.Add (this.stStatusBar);
			global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.stStatusBar]));
			w31.Position = 3;
			w31.Expand = false;
			w31.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 470;
			this.DefaultHeight = 354;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.aboutAction.Activated += new global::System.EventHandler (this.OnAbout);
			this.newAction.Activated += new global::System.EventHandler (this.OnNew);
			this.openAction.Activated += new global::System.EventHandler (this.OnOpen);
			this.saveAction.Activated += new global::System.EventHandler (this.OnSave);
			this.closeAction.Activated += new global::System.EventHandler (this.OnClose);
			this.quitAction.Activated += new global::System.EventHandler (this.OnQuit);
			this.addAction.Activated += new global::System.EventHandler (this.OnAddQuestion);
			this.removeAction.Activated += new global::System.EventHandler (this.OnRemoveQuestion);
			this.convertAction.Activated += new global::System.EventHandler (this.OnExport);
			this.addAnswerAction.Activated += new global::System.EventHandler (this.OnAddAnswer);
			this.removeAnswerAction.Activated += new global::System.EventHandler (this.OnRemoveAnswer);
			this.saveAsAction.Activated += new global::System.EventHandler (this.OnSaveAs);
			this.takeTestAction.Activated += new global::System.EventHandler (this.OnTakeTest);
			this.nbDocPages.SwitchPage += new global::Gtk.SwitchPageHandler (this.OnCurrentPageChanged);
			this.tvDocument.CursorChanged += new global::System.EventHandler (this.OnQuestionChanged);
			this.edQuestionText.FocusOutEvent += new global::Gtk.FocusOutEventHandler (this.OnTextQuestionEditingFinished);
			this.tvAnswers.RowActivated += new global::Gtk.RowActivatedHandler (this.OnAnswerChosen);
			this.btAddAnswer.Clicked += new global::System.EventHandler (this.OnAddAnswer);
			this.btRemoveAnswer.Clicked += new global::System.EventHandler (this.OnRemoveAnswer);
			this.spNumberValidAnswer.ValueChanged += new global::System.EventHandler (this.OnCorrectAnswerChanged);
			this.spNumberValidAnswer.Changed += new global::System.EventHandler (this.OnCorrectAnswerChanged);
		}
	}
}