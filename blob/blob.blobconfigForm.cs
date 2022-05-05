using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace blob
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class blobConfigForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox comboBoxBlobVis;
		private System.Windows.Forms.TextBox textBox_targetDirectory;
		private System.Windows.Forms.TextBox textBox_proc_Filename;
		private System.Windows.Forms.TextBox textBox_proc_Arguments;
		private System.Windows.Forms.ComboBox comboBox_PostExtractStartupType;
		private System.Windows.Forms.ComboBox comboBox_targetDirectoryAppend;
		private System.Windows.Forms.ComboBox comboBox_CreateType;
		private System.Windows.Forms.ComboBox comboBox_DeBlobVis;
		private System.Windows.Forms.CheckBox checkBox_deleteTarget_afterrun;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkBox_deleteSource_afterex;
		private System.Windows.Forms.CheckBox checkBox_ReqAdmin;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label label1;

		public blobConfigForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//


			this.comboBox_CreateType.SelectedIndex = (int)Program.blobConfig.mergeType;
			this.comboBoxBlobVis.SelectedIndex = (int)Program.blobConfig.mergeVisibility;
			this.comboBox_DeBlobVis.SelectedIndex = (int)Program.blobConfig.splitVisibility;

			
			this.textBox_targetDirectory.Text = Program.blobConfig.splitDestination;
			this.textBox_proc_Filename.Text = Program.blobConfig.startUpFile;
			this.textBox_proc_Arguments.Text = Program.blobConfig.startUpArguments;
			
			
			this.comboBox_targetDirectoryAppend.SelectedIndex = (int)Program.blobConfig.splitDestinationAppend;
			this.comboBox_PostExtractStartupType.SelectedIndex = (int)Program.blobConfig.startupType;

			if (this.comboBox_PostExtractStartupType.SelectedIndex == 0)
			{
				this.textBox_proc_Filename.Enabled = false;
				this.textBox_proc_Arguments.Enabled = false;
				this.checkBox_deleteTarget_afterrun.Enabled = false;
				this.checkBox_ReqAdmin.Enabled = false;
			}

			this.checkBox_deleteTarget_afterrun.Checked = Program.blobConfig.startUpDeleteAfter;
			this.checkBox_ReqAdmin.Checked = Program.blobConfig.startUpRequireAdmin;
			this.checkBox_deleteSource_afterex.Checked = Program.blobConfig.splitDeleteAfter;
			
			this.Text += " v"+blob.BlobReflection.GetEntryAssemblyVersion();
		}

		/// <summary>
		/// Clean up any resources being used.
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(blobConfigForm));
			this.comboBox_CreateType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboBoxBlobVis = new System.Windows.Forms.ComboBox();
			this.comboBox_DeBlobVis = new System.Windows.Forms.ComboBox();
			this.checkBox_deleteSource_afterex = new System.Windows.Forms.CheckBox();
			this.checkBox_deleteTarget_afterrun = new System.Windows.Forms.CheckBox();
			this.textBox_targetDirectory = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboBox_targetDirectoryAppend = new System.Windows.Forms.ComboBox();
			this.textBox_proc_Filename = new System.Windows.Forms.TextBox();
			this.comboBox_PostExtractStartupType = new System.Windows.Forms.ComboBox();
			this.textBox_proc_Arguments = new System.Windows.Forms.TextBox();
			this.checkBox_ReqAdmin = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboBox_CreateType
			// 
			this.comboBox_CreateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox_CreateType.Items.AddRange(new object[] {
																	 "Blob",
																	 "User exe",
																	 "Admin exe"});
			this.comboBox_CreateType.Location = new System.Drawing.Point(88, 48);
			this.comboBox_CreateType.Name = "comboBox_CreateType";
			this.comboBox_CreateType.Size = new System.Drawing.Size(152, 21);
			this.comboBox_CreateType.TabIndex = 2;
			this.comboBox_CreateType.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Output Type:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Blob Vis";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Deblob vis";
			// 
			// comboBoxBlobVis
			// 
			this.comboBoxBlobVis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxBlobVis.Items.AddRange(new object[] {
																 "Console",
																 "None"});
			this.comboBoxBlobVis.Location = new System.Drawing.Point(88, 8);
			this.comboBoxBlobVis.Name = "comboBoxBlobVis";
			this.comboBoxBlobVis.Size = new System.Drawing.Size(152, 21);
			this.comboBoxBlobVis.TabIndex = 6;
			this.comboBoxBlobVis.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			// 
			// comboBox_DeBlobVis
			// 
			this.comboBox_DeBlobVis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox_DeBlobVis.Items.AddRange(new object[] {
																	"Console",
																	"None"});
			this.comboBox_DeBlobVis.Location = new System.Drawing.Point(88, 72);
			this.comboBox_DeBlobVis.Name = "comboBox_DeBlobVis";
			this.comboBox_DeBlobVis.Size = new System.Drawing.Size(152, 21);
			this.comboBox_DeBlobVis.TabIndex = 7;
			this.comboBox_DeBlobVis.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
			// 
			// checkBox_deleteSource_afterex
			// 
			this.checkBox_deleteSource_afterex.Location = new System.Drawing.Point(8, 64);
			this.checkBox_deleteSource_afterex.Name = "checkBox_deleteSource_afterex";
			this.checkBox_deleteSource_afterex.Size = new System.Drawing.Size(208, 24);
			this.checkBox_deleteSource_afterex.TabIndex = 8;
			this.checkBox_deleteSource_afterex.Text = "Delete source after extract";
			this.checkBox_deleteSource_afterex.CheckStateChanged += new System.EventHandler(this.checkBox_deleteSource_afterex_CheckStateChanged);
			// 
			// checkBox_deleteTarget_afterrun
			// 
			this.checkBox_deleteTarget_afterrun.Location = new System.Drawing.Point(8, 120);
			this.checkBox_deleteTarget_afterrun.Name = "checkBox_deleteTarget_afterrun";
			this.checkBox_deleteTarget_afterrun.Size = new System.Drawing.Size(208, 24);
			this.checkBox_deleteTarget_afterrun.TabIndex = 9;
			this.checkBox_deleteTarget_afterrun.Text = "Delete target after run";
			this.checkBox_deleteTarget_afterrun.CheckStateChanged += new System.EventHandler(this.checkBox_deleteTarget_afterrun_CheckStateChanged);
			// 
			// textBox_targetDirectory
			// 
			this.textBox_targetDirectory.Location = new System.Drawing.Point(8, 16);
			this.textBox_targetDirectory.Name = "textBox_targetDirectory";
			this.textBox_targetDirectory.Size = new System.Drawing.Size(216, 20);
			this.textBox_targetDirectory.TabIndex = 10;
			this.textBox_targetDirectory.Text = ".";
			this.textBox_targetDirectory.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.comboBox_targetDirectoryAppend,
																					this.textBox_targetDirectory,
																					this.checkBox_deleteSource_afterex});
			this.groupBox1.Location = new System.Drawing.Point(8, 96);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(232, 96);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Extract dir";
			// 
			// comboBox_targetDirectoryAppend
			// 
			this.comboBox_targetDirectoryAppend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox_targetDirectoryAppend.Items.AddRange(new object[] {
																				"AppendOrginal",
																				"AppendFilename",
																				"None"});
			this.comboBox_targetDirectoryAppend.Location = new System.Drawing.Point(8, 40);
			this.comboBox_targetDirectoryAppend.Name = "comboBox_targetDirectoryAppend";
			this.comboBox_targetDirectoryAppend.Size = new System.Drawing.Size(216, 21);
			this.comboBox_targetDirectoryAppend.TabIndex = 12;
			this.comboBox_targetDirectoryAppend.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
			// 
			// textBox_proc_Filename
			// 
			this.textBox_proc_Filename.Location = new System.Drawing.Point(8, 72);
			this.textBox_proc_Filename.Name = "textBox_proc_Filename";
			this.textBox_proc_Filename.Size = new System.Drawing.Size(208, 20);
			this.textBox_proc_Filename.TabIndex = 13;
			this.textBox_proc_Filename.Text = "textBox_proc_Filename";
			this.textBox_proc_Filename.TextChanged += new System.EventHandler(this.textBox_proc_Filename_TextChanged);
			// 
			// comboBox_PostExtractStartupType
			// 
			this.comboBox_PostExtractStartupType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox_PostExtractStartupType.Items.AddRange(new object[] {
																				 "none",
																				 "ProcessNormal",
																				 "ProcessNoWindow"});
			this.comboBox_PostExtractStartupType.Location = new System.Drawing.Point(8, 16);
			this.comboBox_PostExtractStartupType.Name = "comboBox_PostExtractStartupType";
			this.comboBox_PostExtractStartupType.Size = new System.Drawing.Size(216, 21);
			this.comboBox_PostExtractStartupType.TabIndex = 14;
			this.comboBox_PostExtractStartupType.SelectedIndexChanged += new System.EventHandler(this.comboBox_PostExtractStartupType_SelectedIndexChanged);
			// 
			// textBox_proc_Arguments
			// 
			this.textBox_proc_Arguments.Location = new System.Drawing.Point(8, 96);
			this.textBox_proc_Arguments.Name = "textBox_proc_Arguments";
			this.textBox_proc_Arguments.Size = new System.Drawing.Size(208, 20);
			this.textBox_proc_Arguments.TabIndex = 15;
			this.textBox_proc_Arguments.Text = "textBox_proc_Arguments";
			this.textBox_proc_Arguments.TextChanged += new System.EventHandler(this.textBox_proc_Arguments_TextChanged);
			// 
			// checkBox_ReqAdmin
			// 
			this.checkBox_ReqAdmin.Location = new System.Drawing.Point(8, 40);
			this.checkBox_ReqAdmin.Name = "checkBox_ReqAdmin";
			this.checkBox_ReqAdmin.Size = new System.Drawing.Size(216, 24);
			this.checkBox_ReqAdmin.TabIndex = 16;
			this.checkBox_ReqAdmin.Text = "Request Admin";
			this.checkBox_ReqAdmin.CheckStateChanged += new System.EventHandler(this.checkBox_shell_as_admin_CheckStateChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.comboBox_PostExtractStartupType,
																					this.textBox_proc_Arguments,
																					this.textBox_proc_Filename,
																					this.checkBox_ReqAdmin,
																					this.checkBox_deleteTarget_afterrun});
			this.groupBox2.Location = new System.Drawing.Point(8, 200);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(232, 152);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Run";
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(8, 360);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(48, 16);
			this.linkLabel1.TabIndex = 18;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Uninstall";
			// 
			// blobConfigForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 381);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.linkLabel1,
																		  this.groupBox2,
																		  this.groupBox1,
																		  this.comboBox_DeBlobVis,
																		  this.comboBoxBlobVis,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.comboBox_CreateType});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "blobConfigForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Blob config";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.blobconfigForm_Closing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void blobconfigForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Program.blobConfig.ObjectToDisk(Program.configfile);
		}

		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Program.blobConfig.mergeType = (MergeType)this.comboBox_CreateType.SelectedIndex;
		}

		private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Program.blobConfig.mergeVisibility = (MergeVisibility)this.comboBoxBlobVis.SelectedIndex;
		}

		private void comboBox3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Program.blobConfig.splitVisibility = (SplitVisibility)this.comboBox_DeBlobVis.SelectedIndex;
		}

		private void comboBox4_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Program.blobConfig.splitDestinationAppend = (SplitDestinationAppend)this.comboBox_targetDirectoryAppend.SelectedIndex;
		}

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			Program.blobConfig.splitDestination = this.textBox_targetDirectory.Text;
		}

		private void textBox_proc_Filename_TextChanged(object sender, System.EventArgs e)
		{
			Program.blobConfig.startUpFile = this.textBox_proc_Filename.Text;
		}

		private void textBox_proc_Arguments_TextChanged(object sender, System.EventArgs e)
		{
			Program.blobConfig.startUpArguments = this.textBox_proc_Arguments.Text;
		}

		private void comboBox_PostExtractStartupType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			Program.blobConfig.startupType = (StartUpType)this.comboBox_PostExtractStartupType.SelectedIndex;

			if (this.comboBox_PostExtractStartupType.SelectedIndex == 0)
			{
				this.textBox_proc_Filename.Enabled = false;
				this.textBox_proc_Arguments.Enabled = false;
				this.checkBox_deleteTarget_afterrun.Enabled = false;
			}
			else
			{
				this.textBox_proc_Filename.Enabled = true;
				this.textBox_proc_Arguments.Enabled = true;
				this.checkBox_deleteTarget_afterrun.Enabled = true;
			}

			if (this.comboBox_PostExtractStartupType.SelectedIndex == 0)
			{
				this.checkBox_ReqAdmin.Enabled = false;
			}
			else
			{
				this.checkBox_ReqAdmin.Enabled = true;
			}
		}

		private void checkBox_shell_as_admin_CheckStateChanged(object sender, System.EventArgs e)
		{
			
				Program.blobConfig.startUpRequireAdmin = ((CheckBox)sender).Checked;
		}

		private void checkBox_deleteTarget_afterrun_CheckStateChanged(object sender, System.EventArgs e)
		{
			
				Program.blobConfig.startUpDeleteAfter = ((CheckBox)sender).Checked;
		}

		private void checkBox_deleteSource_afterex_CheckStateChanged(object sender, System.EventArgs e)
		{
				Program.blobConfig.splitDeleteAfter = ((CheckBox)sender).Checked;
		}




	}
}
