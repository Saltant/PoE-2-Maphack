using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.ComponentModel;

namespace PoE2Maphack
{
    public partial class MainForm : Form
    {
        #region Fields
        const string GITHUB_URL = "https://github.com/Saltant/PoE-2-Maphack";
        readonly SoundPlayer beepOn;
        readonly SoundPlayer beepOff;
        readonly MemoryStream beepOnStream;
        readonly MemoryStream beepOffStream;
        #endregion

        #region Properties
        /// <summary>
        /// Static property providing access to the single instance of <see cref="MainForm"/>.
        /// Implements the Singleton pattern.
        /// The <see cref="DesignerSerializationVisibilityAttribute"/> hides this property from designer serialization.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static MainForm Instance { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for <see cref="MainForm"/>. Initializes form components, sets the title, subscribes to events,
        /// and starts the Maphack.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            Instance = this;
            Application.ApplicationExit += Application_OnApplicationExit;
            Text = $"{Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(a => a.Key == "DisplayName").Value} v{Assembly.GetExecutingAssembly().GetName().Version.ToString(2)}";
            Resize += MainForm_Resize;
            notifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;

            beepOn = new(beepOnStream = new(Saltant.PoE2Maphack.Resources.Beep_On));
            beepOff = new(beepOffStream = new(Saltant.PoE2Maphack.Resources.Beep_Off));

            Maphack.ChangeStateEvent += ChangeMaphackState;
            Maphack.Start();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handles the form resize event. Minimizes the form to the system tray when the window is minimized.
        /// </summary>
        void MainForm_Resize(object _1, EventArgs _2)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        /// <summary>
        /// Handles the double-click event on the system tray icon. Restores the form from the system tray.
        /// </summary>
        void NotifyIcon1_DoubleClick(object _1, EventArgs _2)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        /// <summary>
        /// Handles the click event on the GitHub link. Opens the GitHub repository in the default browser.
        /// </summary>
        void OnGithubLinkClick(object _1, LinkLabelLinkClickedEventArgs _2) => Process.Start(new ProcessStartInfo("cmd", $"/c start {GITHUB_URL}") { CreateNoWindow = true });

        /// <summary>
        /// Handles the click event on the "Activate" menu item. Activates the Maphack.
        /// </summary>
        void ToolStripMenuActivate_Click(object _1, EventArgs _2) => ChangeMaphackState(Maphack.MaphackState.Activate);

        /// <summary>
        /// Handles the click event on the "Deactivate" menu item. Deactivates the Maphack.
        /// </summary>
        void ToolStripMenuDeactivate_Click(object _1, EventArgs _2) => ChangeMaphackState(Maphack.MaphackState.Deactivate);

        /// <summary>
        /// Handles the click event on the "Exit" menu item. Hides the system tray icon and exits the application.
        /// </summary>
        void ToolStripMenuExit_Click(object _1, EventArgs _2)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        /// <summary>
        /// Handles the state change of Maphack via form buttons.
        /// </summary>
        void OnChangeStateClick(object _1, EventArgs _2)
        {
            if (!activateBtn.Checked && !deactivateBtn.Checked) return;

            ChangeMaphackState(activateBtn.Checked ? Maphack.MaphackState.Activate : Maphack.MaphackState.Deactivate);
        }

        /// <summary>
        /// Changes the state of Maphack and updates the UI based on the result.
        /// </summary>
        /// <param name="state">The state to which Maphack should be changed.</param>
        void ChangeMaphackState(Maphack.MaphackState state)
        {
            var result = Maphack.ChangeMaphackState(state);
            if (result.IsSuccess)
            {
                statusLabel.ForeColor = result.Message == "Activated" ? Color.Green : Color.Black;
                statusLabel.Text = $"Status: {result.Message}";
                switch (state)
                {
                    case Maphack.MaphackState.Activate:
                        beepOn.Play();
                        break;
                    case Maphack.MaphackState.Deactivate:
                        beepOff.Play();
                        break;
                }
            }
            else
            {
                MessageBox.Show(result.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the form closing event. Minimizes the form to the system tray instead of closing it.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        /// <summary>
        /// Handles the application exit event. Releases resources associated with sound playback.
        /// </summary>
        void Application_OnApplicationExit(object sender, EventArgs e)
        {
            beepOnStream?.Close();
            beepOnStream?.Dispose();
            beepOffStream?.Close();
            beepOffStream?.Dispose();
        }
        #endregion
    }
}
