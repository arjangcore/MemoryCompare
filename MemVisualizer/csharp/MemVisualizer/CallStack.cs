using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MemVisualizer
{
	public class CallStack : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox Settings;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox mMapPath;
		private System.Windows.Forms.Label MapLabel;
        private System.Windows.Forms.Button mAddrBrowseButton;
		private System.Windows.Forms.Button mMapBrowse;
		private System.Windows.Forms.Button mGO;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox mResultText;
		private System.Windows.Forms.Button mExitButton;
        private System.Windows.Forms.OpenFileDialog mAddrFileDialog;
		private System.Windows.Forms.OpenFileDialog mMapFileDialog;
		private System.Windows.Forms.Label mAddr2lineLabel;
        private System.Windows.Forms.TextBox mAddr2Path;

		/// <summary>
		/// designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CallStack()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CallStack));
            this.Settings = new System.Windows.Forms.GroupBox();
            this.mAddrBrowseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mAddr2Path = new System.Windows.Forms.TextBox();
            this.mAddr2lineLabel = new System.Windows.Forms.Label();
            this.mExitButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mResultText = new System.Windows.Forms.TextBox();
            this.mGO = new System.Windows.Forms.Button();
            this.mMapBrowse = new System.Windows.Forms.Button();
            this.mMapPath = new System.Windows.Forms.TextBox();
            this.MapLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mMapFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mAddrFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.Settings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Settings
            // 
            this.Settings.Controls.Add(this.mAddrBrowseButton);
            this.Settings.Controls.Add(this.label2);
            this.Settings.Controls.Add(this.mAddr2Path);
            this.Settings.Controls.Add(this.mAddr2lineLabel);
            this.Settings.Controls.Add(this.mExitButton);
            this.Settings.Controls.Add(this.groupBox1);
            this.Settings.Controls.Add(this.mGO);
            this.Settings.Controls.Add(this.mMapBrowse);
            this.Settings.Controls.Add(this.mMapPath);
            this.Settings.Controls.Add(this.MapLabel);
            this.Settings.Controls.Add(this.label1);
            this.Settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Settings.Location = new System.Drawing.Point(0, 10);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(696, 494);
            this.Settings.TabIndex = 1;
            this.Settings.TabStop = false;
            this.Settings.Text = "Setup";
            // 
            // mAddrBrowseButton
            // 
            this.mAddrBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mAddrBrowseButton.Location = new System.Drawing.Point(600, 68);
            this.mAddrBrowseButton.Name = "mAddrBrowseButton";
            this.mAddrBrowseButton.Size = new System.Drawing.Size(83, 26);
            this.mAddrBrowseButton.TabIndex = 12;
            this.mAddrBrowseButton.Text = "Browse";
            this.mAddrBrowseButton.Click += new System.EventHandler(this.mAddrBrowseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "PS3(.elf / .self) | PC(.pdb)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mAddr2Path
            // 
            this.mAddr2Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mAddr2Path.Location = new System.Drawing.Point(96, 71);
            this.mAddr2Path.Name = "mAddr2Path";
            this.mAddr2Path.Size = new System.Drawing.Size(488, 20);
            this.mAddr2Path.TabIndex = 10;
            // 
            // mAddr2lineLabel
            // 
            this.mAddr2lineLabel.Location = new System.Drawing.Point(16, 66);
            this.mAddr2lineLabel.Name = "mAddr2lineLabel";
            this.mAddr2lineLabel.Size = new System.Drawing.Size(59, 40);
            this.mAddr2lineLabel.TabIndex = 9;
            this.mAddr2lineLabel.Text = "Addr2line for PS3";
            this.mAddr2lineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mExitButton
            // 
            this.mExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mExitButton.Location = new System.Drawing.Point(600, 465);
            this.mExitButton.Name = "mExitButton";
            this.mExitButton.Size = new System.Drawing.Size(83, 23);
            this.mExitButton.TabIndex = 8;
            this.mExitButton.Text = "Exit";
            this.mExitButton.Click += new System.EventHandler(this.mExitButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.mResultText);
            this.groupBox1.Location = new System.Drawing.Point(8, 146);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(680, 312);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stack";
            // 
            // mResultText
            // 
            this.mResultText.AcceptsReturn = true;
            this.mResultText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mResultText.Location = new System.Drawing.Point(3, 16);
            this.mResultText.Multiline = true;
            this.mResultText.Name = "mResultText";
            this.mResultText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.mResultText.Size = new System.Drawing.Size(674, 293);
            this.mResultText.TabIndex = 0;
            this.mResultText.WordWrap = false;
            // 
            // mGO
            // 
            this.mGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mGO.Location = new System.Drawing.Point(511, 465);
            this.mGO.Name = "mGO";
            this.mGO.Size = new System.Drawing.Size(83, 23);
            this.mGO.TabIndex = 6;
            this.mGO.Text = "Look Up";
            this.mGO.Click += new System.EventHandler(this.mGO_Click);
            // 
            // mMapBrowse
            // 
            this.mMapBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mMapBrowse.Location = new System.Drawing.Point(600, 14);
            this.mMapBrowse.Name = "mMapBrowse";
            this.mMapBrowse.Size = new System.Drawing.Size(83, 26);
            this.mMapBrowse.TabIndex = 5;
            this.mMapBrowse.Text = "Browse";
            this.mMapBrowse.Click += new System.EventHandler(this.mMapBrowse_Click);
            // 
            // mMapPath
            // 
            this.mMapPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mMapPath.Location = new System.Drawing.Point(95, 17);
            this.mMapPath.Name = "mMapPath";
            this.mMapPath.Size = new System.Drawing.Size(488, 20);
            this.mMapPath.TabIndex = 4;
            // 
            // MapLabel
            // 
            this.MapLabel.Location = new System.Drawing.Point(16, 20);
            this.MapLabel.Name = "MapLabel";
            this.MapLabel.Size = new System.Drawing.Size(73, 20);
            this.MapLabel.TabIndex = 3;
            this.MapLabel.Text = "Game elf/self file";
            this.MapLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CallStack
            // 
            this.AcceptButton = this.mGO;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.mExitButton;
            this.ClientSize = new System.Drawing.Size(696, 494);
            this.Controls.Add(this.Settings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CallStack";
            this.Text = "Call Stack";
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public void SetAddr2linePath (string s)
		{
			mAddr2Path.Text = s;
		}

		public void SetMapPath(string s)
		{
			mMapPath.Text = s;
		}

		public string GetAddr2linePath()
		{
			return mAddr2Path.Text;
		}

		public string GetMapPath()
		{
			return mMapPath.Text;
		}


		public void SetLog(MemManager.Log.Log lg)
		{
			if (mLog != lg)
			{
				mLog = lg;
				mDirty = true;
			}
		}

		public void SetEntryToLookUp(string s)
		{
			mCallStack = s;
			if (!mDirty)
				mResultText.Text = mLookUp.LookUp(mCallStack);
		}

		private void mGO_Click(object sender, System.EventArgs e)
		{
			
			if (!System.IO.File.Exists(mAddr2Path.Text))
			{
                string err = String.Format("Could not find \"{0}\" please set to correct path", mAddr2Path.Text);
				System.Windows.Forms.MessageBox.Show(err, "Error");
				return;
			}
			mLookUp.SetExeType(MemVisualizer.ExeType.ADDR2LINE);
			
			if (!System.IO.File.Exists(mMapPath.Text))
			{
				string err = String.Format("Could not find \"{0}\" please set to correct path", mMapPath.Text);
				System.Windows.Forms.MessageBox.Show(err, "Error");
				return;
			}
			


			// Do it..
			Cursor.Current = Cursors.WaitCursor;

			if (mLastAddr2linePath != mAddr2Path.Text)
			{
				mLastAddr2linePath = mAddr2Path.Text;
				mDirty = true;
			}
			if (mLastMapPath != mMapPath.Text)
			{
				mLastMapPath = mMapPath.Text;
				mDirty = true;
			}
			if (mDirty)
			{
				mLookUp.Clear();
				
				//Select the exe based on the map file
				if (mLookUp.GetExeType() == MemVisualizer.ExeType.ADDR2LINE)
					mLookUp.SetExePath(mAddr2Path.Text);
			
				mLookUp.SetMapPath("\""+mMapPath.Text+"\"");
				mLookUp.Index(mLog);
				mDirty = false;
			}
			mResultText.Text = mLookUp.LookUp(mCallStack);

			Cursor.Current = Cursors.Default;
		}

		static bool mDirty = true;
		static MemManager.Log.Log mLog = null;
		static CallStackLookup mLookUp = new CallStackLookup();
		static string mLastMapPath;
		string mCallStack;
		static string mLastAddr2linePath;

		private void mExitButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void mAddrBrowseButton_Click(object sender, EventArgs e)
        {
            mAddrFileDialog.Filter = "Addr2line paths (*.exe)|*.exe";
            mAddrFileDialog.FilterIndex = 1;
            mAddrFileDialog.FileName = mAddr2Path.Text;
            DialogResult res = mAddrFileDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                SetAddr2linePath(mAddrFileDialog.FileName);
            }
        }

		private void mMapBrowse_Click(object sender, System.EventArgs e)
		{
			mMapFileDialog.Filter = "PDB/ELF paths (*.pdb,*.elf,*.self)|*.pdb;*.elf;*.self";
			mMapFileDialog.FilterIndex = 1;
			mMapFileDialog.FileName = mMapPath.Text;
			DialogResult res = mMapFileDialog.ShowDialog();
			if (res == DialogResult.OK)
			{
				SetMapPath(mMapFileDialog.FileName);
			}		
		}


	}
}
