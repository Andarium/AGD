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

[assembly: CLSCompliant(true)]

namespace ArmorGamesDownloader
{
    public partial class frmMain : Form
    {
        #region Variables
        String stringUrl;
        ErrorStateEnum ErrorState = ErrorStateEnum.NoError;
        DateTime DT = new DateTime(0);
        String[] arguments;
        FolderBrowserDialog fileBrowserDialog = new FolderBrowserDialog();
        GameData GD;
        #endregion

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
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
            this.Text = Wnd.Name;
            arguments = args;
            fileBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            fileBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Opera\\";
            fileBrowserDialog.Description = "Укажите папку с Opera";
        } 
        
        private void cbOutput_KeyDown(object sender, KeyEventArgs e)
        {
            /*if ((e.KeyData != (Keys.Control | Keys.C)))
                e.SuppressKeyPress = true;*/
            e.SuppressKeyPress = true;
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
            }
        }
        private void cbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOutput.Items.Count == 0 | GD == null) return;
            GD.CurrentIndex = cbOutput.SelectedIndex;
            lblNameValue.Text = (GD.CurrentCorrectName != GD.CurrentOriginalName) ? GD.CurrentCorrectName + " / " + GD.CurrentOriginalName : GD.CurrentOriginalName;
            lblSizeValue.Text = (GD.CurrentFileSize == null) ? "n/a" : Extractor.FileSizeToString((Int32)GD.CurrentFileSize);
        }
        
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (tbUrl.Text.Length < 1) MessageBox.Show("Поле адреса не может быть пустым!");
            else
            {
                cbOutput.Text = "";
                cbOutput.Items.Clear();
                btnCopy.Enabled = btnSave.Enabled = btnStart.Enabled = false;
                StatusTimer.Visible = Timer.Enabled = true;
                stringUrl = tbUrl.Text;
                lblNameValue.Text = lblSizeValue.Text = "n/a";
                DT = new DateTime(0);
                StatusTimer.Text = "Затраченное время: " + DT.ToString("HH:mm:ss");
                bgWorker.RunWorkerAsync();
            }
        }
        private void btnRunLink_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Shift && Directory.GetFiles(@"C:\Windows\", "Adobe Flash Player*.exe", SearchOption.TopDirectoryOnly).Length > 0)
            {
                String[] files = Directory.GetFiles(@"C:\Windows\", "Adobe Flash Player*.exe", SearchOption.TopDirectoryOnly);
                if (File.Exists(files[files.Length - 1])) System.Diagnostics.Process.Start(files[files.Length - 1], cbOutput.Text);
            }
            else if (openDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                System.Diagnostics.Process.Start(openDialog.FileName, cbOutput.Text);
        }
        private void btnRunInDefault_Click(object sender, EventArgs e) { System.Diagnostics.Process.Start(cbOutput.Text); }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (cbOutput.Text.Length == 0) return;
            try
            {
                Clipboard.SetText(cbOutput.Text);
                Status.Text = "Ссылка скопирована в буфер обмена.";
                StatusStrip.Update();
            }
            catch(Exception z)
            {
                Status.Text = z.Message + " / " + z.InnerException.Message;
            }
        }
        private void btnCopy_EnabledChanged(object sender, EventArgs e) { btnRunLink.Enabled = btnRunInDefault.Enabled = btnCopy.Enabled; }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (cbName.Checked) saveDialog.FileName = GD.CurrentCorrectName;
            else saveDialog.FileName = GD.CurrentOriginalName;
            if (ModifierKeys == Keys.Shift)
                if (Directory.Exists("D:\\[Files]\\[Flash Games]\\"))
                    saveDialog.InitialDirectory = "D:\\[Files]\\[Flash Games]\\";
            if (saveDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                StatusProgressBar.Visible = true;
                StatusProgressBar.Value = 0;
                Status.Text = "Скачивание файла...";
                StatusStrip.Update();
                btnStart.Enabled = false;
                btnSave.Enabled = false;
                WebClient WC = new WebClient();
                WC.DownloadFileCompleted += new AsyncCompletedEventHandler(WC_DownloadFileCompleted);
                WC.DownloadProgressChanged += new DownloadProgressChangedEventHandler(WC_DownloadProgressChanged);
                WC.DownloadFileAsync(new Uri(cbOutput.Text), saveDialog.FileName);
            }

        }
        private void btnSave_EnabledChanged(object sender, EventArgs e) { cbName.Enabled = cbRun.Enabled = btnSave.Enabled; }

        private void WC_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled) return;
            HttpWebRequest gameFile = (HttpWebRequest)WebRequest.Create(cbOutput.Text);
            HttpWebResponse gameFileResponse = null;
            try
            {
                gameFileResponse = (HttpWebResponse)gameFile.GetResponse();
            }
            catch(WebException z)
            {
                MessageBox.Show(this, z.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                btnSave.Enabled = true;
                StatusProgressBar.Visible = false;
                Status.Text = "Ошибка скачивания файла!";
                StatusStrip.Update();
                File.Delete(saveDialog.FileName);
                return;
            }
            File.SetLastWriteTime(saveDialog.FileName, gameFileResponse.LastModified);
            btnStart.Enabled = true;
            btnSave.Enabled = true;
            StatusProgressBar.Visible = false;
            Status.Text = "Скачивание файла завершено.";
            StatusStrip.Update();
            if (cbRun.Checked) System.Diagnostics.Process.Start(saveDialog.FileName);
        }
        private void WC_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            StatusProgressBar.Value = e.ProgressPercentage;
            Status.Text = "Скачивание файла...";
            StatusStrip.Update();
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.Enabled = true;
            Timer.Enabled = false;
            StatusTimer.Visible = false;
            StatusStrip.Update();
            if (e.Result != null)
            {
                /*if (stringUrl.Contains("twotowersgames")) tbOutput.Text = (e.Result.ToString()).Replace("-exclusive", "");
                else tbOutput.Text = e.Result.ToString();*/
                cbOutput.Items.Clear();
                if (GD.FileList.Count == 0) return;
                cbOutput.Items.AddRange(GD.FileList.Select(x=>x.Link).ToArray());
                cbOutput.SelectedIndex = 0;
                saveDialog.FileName = GD.CurrentOriginalName;
                btnCopy.Enabled = true;
                btnSave.Enabled = true;
            }

            if (GD.ExternalLink)
            {
                tbUrl.Text = GD.CurrentLink;
                btnStart.PerformClick();
            }
        }
        private void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 10:
                    {
                        Status.Text = "Соединение...";
                        break;
                    }
                case 60:
                    {
                        Status.Text = "Соединение успешно. Поиск файла...";
                        break;
                    }
                case 80:
                    {
                        Status.Text = "Файл найден.";
                        break;
                    }
                case 100:
                    {
                        if (ErrorState == ErrorStateEnum.NoError)
                            Status.Text = "Файл найден.";
                        else if (ErrorState == ErrorStateEnum.ConnectionError)
                            Status.Text = "Ошибка соединения.";
                        else if (ErrorState == ErrorStateEnum.FileNotFound)
                        {
                            if (GD.Target == SiteNameEnum.Kongregate)
                                Status.Text = "Файл игры не найден. Вероятно, что это MMO или 3D игра (Kongregate).";
                            else if (GD.Target == SiteNameEnum.Arcadetown)
                                Status.Text = "Файл игры не найден. Вероятно, что игра платная (Arcadetown)";
                            else
                                Status.Text = "На данной странице .swf файл не найден, проверьте введенный адрес.";
                        }
                        else if (ErrorState == ErrorStateEnum.UrlFormatError)
                            Status.Text = "Ошибка. Неверный формат URL.";
                        else if (ErrorState == ErrorStateEnum.Inception)
                            Status.Text = "Найдена внешняя ссылка.";
                        break;
                    }
                default:
                    {
                        Status.Text = "";
                        break;
                    }
            }
            StatusStrip.Update();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DT = DT.AddSeconds(1);
            StatusTimer.Text = "Затраченное время: " + DT.ToString("HH:mm:ss");
            StatusStrip.Update();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            if (arguments.Length > 0 && !bgWorker.IsBusy)
            {
                tbUrl.Text = arguments[0];
                btnStart.PerformClick();
            }
        }

        private void MenuIntegrationOperaAdd_Click(object sender, EventArgs e)
        {
            String[] f = { "--------------------666", "Item, \"ArmorGames Downloader\"=Execute program, \"" + Application.ExecutablePath + "\",\"%l\"" };
            if (fileBrowserDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                String path = fileBrowserDialog.SelectedPath + "\\ui\\standard_menu.ini";
                if (!File.Exists(path)) MessageBox.Show(this, "Файл standard_menu.ini не найден.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    List<String> str = File.ReadAllLines(path).ToList();
                    if (str.Contains(f[0])) str.RemoveRange(str.IndexOf(f[0]), 2);
                    if (str.Contains(f[0])) str.RemoveRange(str.IndexOf(f[0]), 2); //Два вхождения "интеграции": для обычной ссылки и для ссылки-изображения
                    str.InsertRange(str.IndexOf("[Gadget Link Popup Menu]") - 1, f);
                    str.InsertRange(str.IndexOf("[Turbo Image Link Popup Menu]") - 1, f);
                    File.WriteAllLines(path, str.ToArray());
                    MessageBox.Show(this, "Интеграция завершена");
                }
            }
        }
        private void MenuIntegrationOperaDelete_Click(object sender, EventArgs e)
        {
            String f = "--------------------666";
            if (fileBrowserDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                String path = fileBrowserDialog.SelectedPath + "\\ui\\standard_menu.ini";
                if (!File.Exists(path)) MessageBox.Show(this, "Файл standard_menu.ini не найден.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void MenuExit_Click(object sender, EventArgs e) { Application.Exit(); }
        private void MenuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Sorry, but the Princess is in another castle!", "Error 404", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
        private void Menu_Links_Click(object sender, EventArgs e)
        {
            new frmLinks().ShowDialog(this);
        }        
        
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
            if (e.Data.GetDataPresent(DataFormats.Text) & (e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
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

        
    }
    
}
