using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using ArmorGamesDownloader.Properties;
using ArmorGamesDownloader.Extensions;

[assembly: CLSCompliant(true)]

namespace ArmorGamesDownloader
{
    public partial class frmMain : Form
    {
        #region Variables
        private String stringUrl;
        private String[] arguments;
        private DateTime DT;
        private GameData GD;        
        private WebClient WC;
        private ErrorStateEnum ErrorState = ErrorStateEnum.NoError;
        #endregion

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == UnsafeNativeMethods.WM_COPYDATA)
            {
                //те самые параметры с которыми запустили второй экземпляр            
                string msg = UnsafeNativeMethods.MessageToString(m);
                //или 
                arguments = UnsafeNativeMethods.MessageToArray(m);
                if (!bgWorker.IsBusy)
                {
                    tbUrl.Text = arguments[0];
                    btnStart.PerformClick();
                }
            }
        }

        public frmMain(String[] args)
        {
            InitializeComponent();
            InitializeStuff(args);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (arguments.Length > 0 && !bgWorker.IsBusy)
            {
                tbUrl.Text = arguments[0];
                btnStart.PerformClick();
            }
        }

        private void InitializeStuff(String [] args)
        {
            this.Text = Wnd.Name;
            arguments = args;
            folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Opera\\";
            WC = new WebClient() { Proxy = null };
            WC.DownloadFileCompleted += new AsyncCompletedEventHandler(WC_DownloadFileCompleted);
            WC.DownloadProgressChanged += new DownloadProgressChangedEventHandler(WC_DownloadProgressChanged);
        }

        private void SetEnableState()
        {
            btnStart.Enabled = !bgWorker.IsBusy;
            btnCopy.Enabled = !bgWorker.IsBusy & cbOutput.Items.Count > 0;
            lblStatusTime.Visible = Timer.Enabled = bgWorker.IsBusy;
            btnSave.Enabled = !WC.IsBusy & !bgWorker.IsBusy & cbOutput.Items.Count > 0;
            btnRunInDefault.Enabled = btnRunLink.Enabled = !bgWorker.IsBusy & cbOutput.Items.Count > 0;
            prBarStatus.Visible = WC.IsBusy;
            StatusStrip.Update();
        }


        
        private void cbOutput_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case (Keys.Control | Keys.C):
                case (Keys.Left):
                case (Keys.Right):
                case (Keys.Up):
                case (Keys.Down):
                case (Keys.Shift | Keys.Left):
                case (Keys.Shift | Keys.Right):
                    {
                        e.SuppressKeyPress = false;
                        break;
                    }
                default:
                    e.SuppressKeyPress = true;
                    break;
            }
            
        }
        private void cbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOutput.Items.Count == 0 | GD == null) return;
            GD.CurrentIndex = cbOutput.SelectedIndex;

            lblNameValue.Text = (GD.CurrentCorrectName != GD.CurrentOriginalName) 
                ? GD.CurrentCorrectName + " / " + GD.CurrentOriginalName 
                : GD.CurrentOriginalName;

            lblSizeValue.Text = (GD.CurrentFileSize == null) 
                ? "n/a" 
                : Extractor.GetSizeString((Int32)GD.CurrentFileSize);
        }

        #region Buttons Stuff
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (tbUrl.Text.Length < 1) MessageBox.Show(Resources.strErrorEmptyUrl);
            else
            {
                cbOutput.Text = "";
                cbOutput.Items.Clear();
                
                stringUrl = tbUrl.Text;
                lblNameValue.Text = lblSizeValue.Text = "n/a";
                DT = DateTime.Now;
                lblStatusTime.Text = Resources.strElapsedTime + (DateTime.Now - DT).ToString(@"hh\:mm\:ss");
                bgWorker.RunWorkerAsync();
                SetEnableState();
            }
        }
        private void btnRunLink_Click(object sender, EventArgs e)
        {
            String[] files = Directory.GetFiles(@"C:\Windows\", "Adobe Flash Player*.exe", SearchOption.TopDirectoryOnly);
            if (ModifierKeys == Keys.Shift && files.Length > 0)
                Process.Start(files.Last(), cbOutput.Text);
            else if (openDialog.ShowDialog(this) == DialogResult.OK)
                Process.Start(openDialog.FileName, cbOutput.Text);
        }
        private void btnRunInDefault_Click(object sender, EventArgs e) 
        { 
            Process.Start(cbOutput.Text); 
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (cbOutput.Text.Length == 0) return;
            try
            {
                Clipboard.SetText(cbOutput.Text);
                lblStatus.Text = Resources.strCopiedToClipboard;
            }
            catch(Exception z)
            {
                lblStatus.Text = z.Message + " / " + z.InnerException.Message;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDialog.FileName = (cbCorrectName.Checked)
                ? GD.CurrentCorrectName
                : GD.CurrentOriginalName;

            if (ModifierKeys.HasFlag(Keys.Shift))
                if (Directory.Exists(Resources.strAndaDir))
                    saveDialog.InitialDirectory = Resources.strAndaDir;

            if (saveDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                prBarStatus.Value = 0;
                lblStatus.Text = Resources.strDownloadingFile;                
                WC.DownloadFileAsync(new Uri(cbOutput.Text), saveDialog.FileName);
                SetEnableState();
            }
        }
        #endregion

        private void WC_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                HttpWebRequest gameFile = (HttpWebRequest)WebRequest.Create(cbOutput.Text);
                HttpWebResponse gameFileResponse = null;
                try
                {
                    gameFileResponse = (HttpWebResponse)gameFile.GetResponse();
                }
                catch (WebException z)
                {
                    MessageBox.Show(this, z.Message, Resources.strError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetEnableState();
                    lblStatus.Text = Resources.strDownloadError;
                    StatusStrip.Update();
                    try
                    {
                        File.Delete(saveDialog.FileName);
                    }
                    catch
                    { }; //пусто
                    return;
                }
                File.SetLastWriteTime(saveDialog.FileName, gameFileResponse.LastModified);
                lblStatus.Text = Resources.strDownloadSuccess;                
                if (cbRunAfterSave.Checked) System.Diagnostics.Process.Start(saveDialog.FileName);
            }
            SetEnableState();
        }
        private void WC_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            prBarStatus.Value = e.ProgressPercentage;
            lblStatus.Text = Resources.strDownloadingFile + " " +
                Extractor.GetSizeString(e.BytesReceived, 2, false) + " / " + 
                Extractor.GetSizeString(e.TotalBytesToReceive, 2); ;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblStatusTime.Text = Resources.strElapsedTime + (DateTime.Now - DT).ToString(@"hh\:mm\:ss");
            //StatusStrip.Update();
        }
        

        #region Menu Stuff
        private void Menu_Integration_Opera_Add_Click(object sender, EventArgs e)
        {
            String[] f = { Resources.strOperaPrefix, "Item, \"ArmorGames Downloader\"=Execute program, \"" + Application.ExecutablePath + "\",\"%l\"" };
            if (folderDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                String path = folderDialog.SelectedPath + "\\ui\\standard_menu.ini";
                if (!File.Exists(path)) MessageBox.Show(this, Resources.strIntegrationOperaNotFound, Resources.strError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    List<String> str = File.ReadAllLines(path).ToList();
                    if (str.Contains(f[0])) str.RemoveRange(str.IndexOf(f[0]), 2);
                    if (str.Contains(f[0])) str.RemoveRange(str.IndexOf(f[0]), 2); //Два вхождения "интеграции": для обычной ссылки и для ссылки-изображения
                    str.InsertRange(str.IndexOf("[Gadget Link Popup Menu]") - 1, f);
                    str.InsertRange(str.IndexOf("[Turbo Image Link Popup Menu]") - 1, f);
                    File.WriteAllLines(path, str.ToArray());
                    MessageBox.Show(this, Resources.strIntegrationSuccess);
                }
            }
        }
        private void Menu_Integration_Opera_Delete_Click(object sender, EventArgs e)
        {
            String f = Resources.strOperaPrefix;
            if (folderDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                String path = folderDialog.SelectedPath + "\\ui\\standard_menu.ini";
                if (!File.Exists(path)) MessageBox.Show(this, Resources.strIntegrationOperaNotFound, Resources.strError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    List<String> str = File.ReadAllLines(path).ToList();
                    if (str.Contains(f))
                    {
                        str.RemoveRange(str.IndexOf(f), 2);
                        str.RemoveRange(str.IndexOf(f), 2);
                        File.WriteAllLines(path, str.ToArray());
                    }
                    MessageBox.Show(this, "Интеграция удалена.");
                }
            }
        }
        private void Menu_Exit_Click(object sender, EventArgs e) 
        { 
            Application.Exit(); 
        }
        private void Menu_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Sorry, but the Princess is in another castle!", "Error 404", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
        private void Menu_Links_Click(object sender, EventArgs e)
        {
            new frmLinks().ShowDialog(this);
        }
        #endregion

        #region TextBox Url stuff
        private void tbUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                e.SuppressKeyPress = true;
        }
        private void tbUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnStart.PerformClick();
        }
        private void tbUrl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text) & e.AllowedEffect.HasFlag(DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
        }
        private void tbUrl_DragDrop(object sender, DragEventArgs e)
        {
            String Url = e.Data.GetData(DataFormats.Text) as String;
            if (Url != null)
            {
                tbUrl.Text = Url;
                btnStart.PerformClick();
            }
        }
        #endregion

    }
    
}
