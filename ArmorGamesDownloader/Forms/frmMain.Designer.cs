namespace ArmorGamesDownloader
{
	partial class frmMain
	{
		/// <summary>
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.prBarStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblInput = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.cbCorrectName = new System.Windows.Forms.CheckBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Integration = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Integration_Opera = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Integration_Opera_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Integration_Opera_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Links = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_About = new System.Windows.Forms.ToolStripMenuItem();
            this.cbRunAfterSave = new System.Windows.Forms.CheckBox();
            this.btnRunLink = new System.Windows.Forms.Button();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnRunInDefault = new System.Windows.Forms.Button();
            this.cbOutput = new System.Windows.Forms.ComboBox();
            this.tlTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblNameValue = new System.Windows.Forms.Label();
            this.gBoxResult = new System.Windows.Forms.GroupBox();
            this.lblSizeValue = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.StatusStrip.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.gBoxResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbUrl
            // 
            this.tbUrl.AllowDrop = true;
            this.tbUrl.Location = new System.Drawing.Point(8, 52);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(658, 20);
            this.tbUrl.TabIndex = 2;
            this.tbUrl.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbUrl_DragDrop);
            this.tbUrl.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbUrl_DragEnter);
            this.tbUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUrl_KeyDown);
            this.tbUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbUrl_KeyUp);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prBarStatus,
            this.lblStatus,
            this.lblStatusTime});
            this.StatusStrip.Location = new System.Drawing.Point(0, 260);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(711, 22);
            this.StatusStrip.SizingGrip = false;
            this.StatusStrip.Stretch = false;
            this.StatusStrip.TabIndex = 4;
            // 
            // prBarStatus
            // 
            this.prBarStatus.Name = "prBarStatus";
            this.prBarStatus.Size = new System.Drawing.Size(100, 16);
            this.prBarStatus.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(221, 17);
            this.lblStatus.Text = "Введите адрес с игрой и нажмите Enter";
            // 
            // lblStatusTime
            // 
            this.lblStatusTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblStatusTime.Margin = new System.Windows.Forms.Padding(50, 3, 0, 2);
            this.lblStatusTime.Name = "lblStatusTime";
            this.lblStatusTime.Size = new System.Drawing.Size(116, 17);
            this.lblStatusTime.Text = "Затрачено времени";
            this.lblStatusTime.Visible = false;
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(5, 36);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(131, 13);
            this.lblInput.TabIndex = 7;
            this.lblInput.Text = "Адрес страницы с игрой";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(5, 83);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(101, 13);
            this.lblOutput.TabIndex = 8;
            this.lblOutput.Text = "Адрес файла игры";
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "SWF files|*.swf|Все файлы|*.*";
            this.saveDialog.RestoreDirectory = true;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bg_DoWork);
            this.bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bg_ProgressChanged);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bg_RunWorkerCompleted);
            // 
            // Timer
            // 
            this.Timer.Interval = 1000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // cbCorrectName
            // 
            this.cbCorrectName.AutoSize = true;
            this.cbCorrectName.Checked = true;
            this.cbCorrectName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCorrectName.Location = new System.Drawing.Point(53, 68);
            this.cbCorrectName.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.cbCorrectName.Name = "cbCorrectName";
            this.cbCorrectName.Size = new System.Drawing.Size(174, 17);
            this.cbCorrectName.TabIndex = 10;
            this.cbCorrectName.Text = "Корректировка имени файла";
            this.cbCorrectName.UseVisualStyleBackColor = true;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Integration,
            this.Menu_Help});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MenuStrip.Size = new System.Drawing.Size(711, 24);
            this.MenuStrip.TabIndex = 11;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Exit});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(48, 20);
            this.Menu_File.Text = "Файл";
            // 
            // Menu_Exit
            // 
            this.Menu_Exit.Name = "Menu_Exit";
            this.Menu_Exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.Menu_Exit.Size = new System.Drawing.Size(145, 22);
            this.Menu_Exit.Text = "Выход";
            this.Menu_Exit.Click += new System.EventHandler(this.Menu_Exit_Click);
            // 
            // Menu_Integration
            // 
            this.Menu_Integration.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Integration_Opera});
            this.Menu_Integration.Name = "Menu_Integration";
            this.Menu_Integration.Size = new System.Drawing.Size(84, 20);
            this.Menu_Integration.Text = "Интеграция";
            // 
            // Menu_Integration_Opera
            // 
            this.Menu_Integration_Opera.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Integration_Opera_Add,
            this.Menu_Integration_Opera_Delete});
            this.Menu_Integration_Opera.Name = "Menu_Integration_Opera";
            this.Menu_Integration_Opera.Size = new System.Drawing.Size(106, 22);
            this.Menu_Integration_Opera.Text = "Opera";
            // 
            // Menu_Integration_Opera_Add
            // 
            this.Menu_Integration_Opera_Add.Name = "Menu_Integration_Opera_Add";
            this.Menu_Integration_Opera_Add.Size = new System.Drawing.Size(182, 22);
            this.Menu_Integration_Opera_Add.Text = "Интегрировать";
            this.Menu_Integration_Opera_Add.Click += new System.EventHandler(this.Menu_Integration_Opera_Add_Click);
            // 
            // Menu_Integration_Opera_Delete
            // 
            this.Menu_Integration_Opera_Delete.Name = "Menu_Integration_Opera_Delete";
            this.Menu_Integration_Opera_Delete.Size = new System.Drawing.Size(182, 22);
            this.Menu_Integration_Opera_Delete.Text = "Убрать интеграцию";
            this.Menu_Integration_Opera_Delete.Click += new System.EventHandler(this.Menu_Integration_Opera_Delete_Click);
            // 
            // Menu_Help
            // 
            this.Menu_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Links,
            this.Menu_About});
            this.Menu_Help.Name = "Menu_Help";
            this.Menu_Help.Size = new System.Drawing.Size(65, 20);
            this.Menu_Help.Text = "Справка";
            // 
            // Menu_Links
            // 
            this.Menu_Links.Name = "Menu_Links";
            this.Menu_Links.Size = new System.Drawing.Size(158, 22);
            this.Menu_Links.Text = "Ссылки...";
            this.Menu_Links.Click += new System.EventHandler(this.Menu_Links_Click);
            // 
            // Menu_About
            // 
            this.Menu_About.Name = "Menu_About";
            this.Menu_About.Size = new System.Drawing.Size(158, 22);
            this.Menu_About.Text = "О программе...";
            this.Menu_About.Click += new System.EventHandler(this.Menu_About_Click);
            // 
            // cbRunAfterSave
            // 
            this.cbRunAfterSave.AutoSize = true;
            this.cbRunAfterSave.Location = new System.Drawing.Point(53, 91);
            this.cbRunAfterSave.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.cbRunAfterSave.Name = "cbRunAfterSave";
            this.cbRunAfterSave.Size = new System.Drawing.Size(202, 17);
            this.cbRunAfterSave.TabIndex = 12;
            this.cbRunAfterSave.Text = "Запустить файл после сохранения";
            this.cbRunAfterSave.UseVisualStyleBackColor = true;
            // 
            // btnRunLink
            // 
            this.btnRunLink.Enabled = false;
            this.btnRunLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRunLink.Location = new System.Drawing.Point(450, 56);
            this.btnRunLink.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.btnRunLink.Name = "btnRunLink";
            this.btnRunLink.Size = new System.Drawing.Size(193, 24);
            this.btnRunLink.TabIndex = 13;
            this.btnRunLink.Text = "Запустить ссылку в программе...";
            this.btnRunLink.UseVisualStyleBackColor = true;
            this.btnRunLink.Click += new System.EventHandler(this.btnRunLink_Click);
            // 
            // openDialog
            // 
            this.openDialog.Filter = "Executable files|*.exe";
            this.openDialog.InitialDirectory = "C:\\";
            this.openDialog.RestoreDirectory = true;
            // 
            // btnRunInDefault
            // 
            this.btnRunInDefault.Enabled = false;
            this.btnRunInDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRunInDefault.Location = new System.Drawing.Point(450, 87);
            this.btnRunInDefault.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.btnRunInDefault.Name = "btnRunInDefault";
            this.btnRunInDefault.Size = new System.Drawing.Size(193, 24);
            this.btnRunInDefault.TabIndex = 14;
            this.btnRunInDefault.Text = "Запустить ссылку в браузере";
            this.btnRunInDefault.UseVisualStyleBackColor = true;
            this.btnRunInDefault.Click += new System.EventHandler(this.btnRunInDefault_Click);
            // 
            // cbOutput
            // 
            this.cbOutput.FormattingEnabled = true;
            this.cbOutput.Location = new System.Drawing.Point(8, 99);
            this.cbOutput.Name = "cbOutput";
            this.cbOutput.Size = new System.Drawing.Size(658, 21);
            this.cbOutput.TabIndex = 16;
            this.cbOutput.SelectedIndexChanged += new System.EventHandler(this.cbOutput_SelectedIndexChanged);
            this.cbOutput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbOutput_KeyDown);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSave.Image = global::ArmorGamesDownloader.Properties.Resources.SaveBig;
            this.btnSave.Location = new System.Drawing.Point(9, 69);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(36, 36);
            this.btnSave.TabIndex = 9;
            this.tlTip.SetToolTip(this.btnSave, "Сохранить в файл...");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Enabled = false;
            this.btnCopy.FlatAppearance.BorderSize = 0;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCopy.Image = global::ArmorGamesDownloader.Properties.Resources.Copy;
            this.btnCopy.Location = new System.Drawing.Point(677, 96);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(25, 27);
            this.btnCopy.TabIndex = 6;
            this.btnCopy.Tag = "Скопировать ссылку в буфер обмена";
            this.tlTip.SetToolTip(this.btnCopy, "Скопировать ссылку в буфер обмена");
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStart.Image = global::ArmorGamesDownloader.Properties.Resources.Search;
            this.btnStart.Location = new System.Drawing.Point(675, 49);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(27, 27);
            this.btnStart.TabIndex = 3;
            this.tlTip.SetToolTip(this.btnStart, "Найти");
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(11, 20);
            this.lblName.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(60, 13);
            this.lblName.TabIndex = 17;
            this.lblName.Text = "Название:";
            // 
            // lblNameValue
            // 
            this.lblNameValue.AutoSize = true;
            this.lblNameValue.Location = new System.Drawing.Point(77, 20);
            this.lblNameValue.Name = "lblNameValue";
            this.lblNameValue.Size = new System.Drawing.Size(24, 13);
            this.lblNameValue.TabIndex = 18;
            this.lblNameValue.Text = "n/a";
            // 
            // gBoxResult
            // 
            this.gBoxResult.Controls.Add(this.lblSizeValue);
            this.gBoxResult.Controls.Add(this.lblSize);
            this.gBoxResult.Controls.Add(this.lblName);
            this.gBoxResult.Controls.Add(this.btnRunInDefault);
            this.gBoxResult.Controls.Add(this.lblNameValue);
            this.gBoxResult.Controls.Add(this.btnRunLink);
            this.gBoxResult.Controls.Add(this.btnSave);
            this.gBoxResult.Controls.Add(this.cbRunAfterSave);
            this.gBoxResult.Controls.Add(this.cbCorrectName);
            this.gBoxResult.Location = new System.Drawing.Point(8, 129);
            this.gBoxResult.Name = "gBoxResult";
            this.gBoxResult.Size = new System.Drawing.Size(654, 122);
            this.gBoxResult.TabIndex = 19;
            this.gBoxResult.TabStop = false;
            this.gBoxResult.Text = "Результат";
            // 
            // lblSizeValue
            // 
            this.lblSizeValue.AutoSize = true;
            this.lblSizeValue.Location = new System.Drawing.Point(101, 40);
            this.lblSizeValue.Name = "lblSizeValue";
            this.lblSizeValue.Size = new System.Drawing.Size(24, 13);
            this.lblSizeValue.TabIndex = 20;
            this.lblSizeValue.Text = "n/a";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(11, 40);
            this.lblSize.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(84, 13);
            this.lblSize.TabIndex = 19;
            this.lblSize.Text = "Размер файла:";
            // 
            // folderDialog
            // 
            this.folderDialog.Description = "Укажите папку с Opera";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 282);
            this.Controls.Add(this.gBoxResult);
            this.Controls.Add(this.cbOutput);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.MenuStrip);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbUrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "ArmorGamesDownloader";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.gBoxResult.ResumeLayout(false);
            this.gBoxResult.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbUrl;
		private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.Label lblInput;
		private System.Windows.Forms.Label lblOutput;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.SaveFileDialog saveDialog;
		public System.Windows.Forms.ToolStripProgressBar prBarStatus;
		private System.ComponentModel.BackgroundWorker bgWorker;
		private System.Windows.Forms.ToolStripStatusLabel lblStatusTime;
		private System.Windows.Forms.StatusStrip StatusStrip;
		private System.Windows.Forms.Timer Timer;
		private System.Windows.Forms.CheckBox cbCorrectName;
		private System.Windows.Forms.MenuStrip MenuStrip;
		private System.Windows.Forms.ToolStripMenuItem Menu_File;
		private System.Windows.Forms.ToolStripMenuItem Menu_Exit;
		private System.Windows.Forms.ToolStripMenuItem Menu_Integration;
		private System.Windows.Forms.ToolStripMenuItem Menu_Integration_Opera;
		private System.Windows.Forms.ToolStripMenuItem Menu_Integration_Opera_Add;
		private System.Windows.Forms.ToolStripMenuItem Menu_Integration_Opera_Delete;
		private System.Windows.Forms.CheckBox cbRunAfterSave;
		private System.Windows.Forms.Button btnRunLink;
		private System.Windows.Forms.OpenFileDialog openDialog;
		private System.Windows.Forms.Button btnRunInDefault;
		private System.Windows.Forms.ToolStripMenuItem Menu_Help;
        private System.Windows.Forms.ToolStripMenuItem Menu_About;
        private System.Windows.Forms.ToolStripMenuItem Menu_Links;
        private System.Windows.Forms.ComboBox cbOutput;
        private System.Windows.Forms.ToolTip tlTip;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblNameValue;
        private System.Windows.Forms.GroupBox gBoxResult;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblSizeValue;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
	}
}

