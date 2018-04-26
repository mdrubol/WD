using MetroFramework.Components;
using MetroFramework.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WD.DataAccess.Context;
using WD.XLC.Domain.Entities;
using WD.XLC.Domain.Helpers;
using WD.XLC.WIN.Controls;
namespace WD.XLC.WIN
{
    public class VerticalProgress : MetroProgressBar
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | 0x4;
                return cp;
            }
        }
    }
    partial class DashboardForm
    {
        private readonly int max = 0;
        private readonly bool runLoader = false;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                this.notifyIconDashBoard.Dispose();
                Utility.DisposeConfig(max);
                components.Dispose();
            }
            base.Dispose(disposing);
            // Suppress finalization.
            System.GC.SuppressFinalize(this);
        }
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.dashBoardControl = new MetroFramework.Controls.MetroTabControl();
            this.connectionPage = new MetroFramework.Controls.MetroTabPage();
            this.connectionSplitContainer = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.serverPage = new MetroFramework.Controls.MetroTabPage();
            this.serverPageSplitContainer = new System.Windows.Forms.SplitContainer();
            this.btnAddServer = new MetroFramework.Controls.MetroButton();
            this.templatePage = new MetroFramework.Controls.MetroTabPage();
            this.dashBoardSplitContainer = new System.Windows.Forms.SplitContainer();
            this.mappingPage = new MetroFramework.Controls.MetroTabPage();
            this.LoaderPage = new MetroFramework.Controls.MetroTabPage();
            this.instancePage = new MetroFramework.Controls.MetroTabPage();
            this.instanceSplitContainer = new System.Windows.Forms.SplitContainer();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.notifyIconDashBoard = new System.Windows.Forms.NotifyIcon(this.components);
            this.progressBar1 = new WD.XLC.WIN.VerticalProgress();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.dashBoardControl.SuspendLayout();
            this.connectionPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.connectionSplitContainer)).BeginInit();
            this.connectionSplitContainer.Panel2.SuspendLayout();
            this.connectionSplitContainer.SuspendLayout();
            this.serverPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverPageSplitContainer)).BeginInit();
            this.serverPageSplitContainer.Panel1.SuspendLayout();
            this.serverPageSplitContainer.SuspendLayout();
            this.templatePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dashBoardSplitContainer)).BeginInit();
            this.dashBoardSplitContainer.SuspendLayout();
            this.instancePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.instanceSplitContainer)).BeginInit();
            this.instanceSplitContainer.Panel2.SuspendLayout();
            this.instanceSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            // 
            // dashBoardControl
            // 
            this.dashBoardControl.Controls.Add(this.connectionPage);
            this.dashBoardControl.Controls.Add(this.serverPage);
            this.dashBoardControl.Controls.Add(this.templatePage);
            this.dashBoardControl.Controls.Add(this.mappingPage);
            this.dashBoardControl.Controls.Add(this.LoaderPage);
            this.dashBoardControl.Controls.Add(this.instancePage);
            this.dashBoardControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashBoardControl.Location = new System.Drawing.Point(25, 60);
            this.dashBoardControl.Name = "dashBoardControl";
            this.dashBoardControl.SelectedIndex = 0;
            this.dashBoardControl.Size = new System.Drawing.Size(931, 530);
            this.dashBoardControl.TabIndex = 0;
            this.dashBoardControl.UseSelectable = true;
            this.dashBoardControl.SelectedIndexChanged += new System.EventHandler(this.dashBoardControl_SelectedIndexChanged);
            // 
            // connectionPage
            // 
            this.connectionPage.Controls.Add(this.connectionSplitContainer);
            this.connectionPage.HorizontalScrollbarBarColor = true;
            this.connectionPage.HorizontalScrollbarHighlightOnWheel = false;
            this.connectionPage.HorizontalScrollbarSize = 10;
            this.connectionPage.Location = new System.Drawing.Point(4, 38);
            this.connectionPage.Name = "connectionPage";
            this.connectionPage.Padding = new System.Windows.Forms.Padding(5);
            this.connectionPage.Size = new System.Drawing.Size(923, 488);
            this.connectionPage.TabIndex = 0;
            this.connectionPage.Text = "Global Connection";
            this.connectionPage.VerticalScrollbarBarColor = true;
            this.connectionPage.VerticalScrollbarHighlightOnWheel = false;
            this.connectionPage.VerticalScrollbarSize = 10;
            // 
            // connectionSplitContainer
            // 
            this.connectionSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectionSplitContainer.Location = new System.Drawing.Point(5, 5);
            this.connectionSplitContainer.Name = "connectionSplitContainer";
            // 
            // connectionSplitContainer.Panel2
            // 
            this.connectionSplitContainer.Panel2.Controls.Add(this.listBox1);
            this.connectionSplitContainer.Size = new System.Drawing.Size(913, 478);
            this.connectionSplitContainer.SplitterDistance = 360;
            this.connectionSplitContainer.TabIndex = 2;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(549, 478);
            this.listBox1.TabIndex = 0;
            // 
            // serverPage
            // 
            this.serverPage.Controls.Add(this.serverPageSplitContainer);
            this.serverPage.HorizontalScrollbarBarColor = true;
            this.serverPage.HorizontalScrollbarHighlightOnWheel = false;
            this.serverPage.HorizontalScrollbarSize = 10;
            this.serverPage.Location = new System.Drawing.Point(4, 38);
            this.serverPage.Name = "serverPage";
            this.serverPage.Padding = new System.Windows.Forms.Padding(10);
            this.serverPage.Size = new System.Drawing.Size(923, 488);
            this.serverPage.TabIndex = 1;
            this.serverPage.Text = "Destination Server";
            this.serverPage.VerticalScrollbarBarColor = true;
            this.serverPage.VerticalScrollbarHighlightOnWheel = false;
            this.serverPage.VerticalScrollbarSize = 10;
            // 
            // serverPageSplitContainer
            // 
            this.serverPageSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverPageSplitContainer.Location = new System.Drawing.Point(10, 10);
            this.serverPageSplitContainer.Name = "serverPageSplitContainer";
            // 
            // serverPageSplitContainer.Panel1
            // 
            this.serverPageSplitContainer.Panel1.Controls.Add(this.btnAddServer);
            this.serverPageSplitContainer.Size = new System.Drawing.Size(903, 468);
            this.serverPageSplitContainer.SplitterDistance = 106;
            this.serverPageSplitContainer.TabIndex = 2;
            // 
            // btnAddServer
            // 
            this.btnAddServer.Location = new System.Drawing.Point(13, 49);
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size(75, 23);
            this.btnAddServer.TabIndex = 0;
            this.btnAddServer.Text = "Add Server";
            this.btnAddServer.UseSelectable = true;
            this.btnAddServer.Click += new System.EventHandler(this.btnAddServer_Click);
            // 
            // templatePage
            // 
            this.templatePage.AutoScroll = true;
            this.templatePage.Controls.Add(this.dashBoardSplitContainer);
            this.templatePage.HorizontalScrollbar = true;
            this.templatePage.HorizontalScrollbarBarColor = true;
            this.templatePage.HorizontalScrollbarHighlightOnWheel = false;
            this.templatePage.HorizontalScrollbarSize = 10;
            this.templatePage.Location = new System.Drawing.Point(4, 38);
            this.templatePage.Name = "templatePage";
            this.templatePage.Padding = new System.Windows.Forms.Padding(5);
            this.templatePage.Size = new System.Drawing.Size(923, 488);
            this.templatePage.TabIndex = 2;
            this.templatePage.Text = "Template";
            this.templatePage.VerticalScrollbar = true;
            this.templatePage.VerticalScrollbarBarColor = true;
            this.templatePage.VerticalScrollbarHighlightOnWheel = false;
            this.templatePage.VerticalScrollbarSize = 10;
            // 
            // dashBoardSplitContainer
            // 
            this.dashBoardSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashBoardSplitContainer.Location = new System.Drawing.Point(5, 5);
            this.dashBoardSplitContainer.Name = "dashBoardSplitContainer";
            // 
            // dashBoardSplitContainer.Panel1
            // 
            this.dashBoardSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(5);
            this.dashBoardSplitContainer.Size = new System.Drawing.Size(913, 478);
            this.dashBoardSplitContainer.SplitterDistance = 279;
            this.dashBoardSplitContainer.TabIndex = 1;
            // 
            // mappingPage
            // 
            this.mappingPage.HorizontalScrollbarBarColor = true;
            this.mappingPage.HorizontalScrollbarHighlightOnWheel = false;
            this.mappingPage.HorizontalScrollbarSize = 10;
            this.mappingPage.Location = new System.Drawing.Point(4, 38);
            this.mappingPage.Name = "mappingPage";
            this.mappingPage.Padding = new System.Windows.Forms.Padding(10);
            this.mappingPage.Size = new System.Drawing.Size(923, 488);
            this.mappingPage.TabIndex = 3;
            this.mappingPage.Text = "Mapping";
            this.mappingPage.VerticalScrollbarBarColor = true;
            this.mappingPage.VerticalScrollbarHighlightOnWheel = false;
            this.mappingPage.VerticalScrollbarSize = 10;
            // 
            // LoaderPage
            // 
            this.LoaderPage.HorizontalScrollbarBarColor = true;
            this.LoaderPage.HorizontalScrollbarHighlightOnWheel = false;
            this.LoaderPage.HorizontalScrollbarSize = 10;
            this.LoaderPage.Location = new System.Drawing.Point(4, 38);
            this.LoaderPage.Name = "LoaderPage";
            this.LoaderPage.Padding = new System.Windows.Forms.Padding(10);
            this.LoaderPage.Size = new System.Drawing.Size(923, 488);
            this.LoaderPage.TabIndex = 4;
            this.LoaderPage.Text = "Loader";
            this.LoaderPage.VerticalScrollbarBarColor = true;
            this.LoaderPage.VerticalScrollbarHighlightOnWheel = false;
            this.LoaderPage.VerticalScrollbarSize = 10;
            // 
            // instancePage
            // 
            this.instancePage.Controls.Add(this.instanceSplitContainer);
            this.instancePage.HorizontalScrollbarBarColor = true;
            this.instancePage.HorizontalScrollbarHighlightOnWheel = false;
            this.instancePage.HorizontalScrollbarSize = 10;
            this.instancePage.Location = new System.Drawing.Point(4, 38);
            this.instancePage.Name = "instancePage";
            this.instancePage.Padding = new System.Windows.Forms.Padding(10);
            this.instancePage.Size = new System.Drawing.Size(923, 488);
            this.instancePage.TabIndex = 5;
            this.instancePage.Text = "Instance Configuration";
            this.instancePage.VerticalScrollbarBarColor = true;
            this.instancePage.VerticalScrollbarHighlightOnWheel = false;
            this.instancePage.VerticalScrollbarSize = 10;
            // 
            // instanceSplitContainer
            // 
            this.instanceSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instanceSplitContainer.Location = new System.Drawing.Point(10, 10);
            this.instanceSplitContainer.Name = "instanceSplitContainer";
            // 
            // instanceSplitContainer.Panel2
            // 
            this.instanceSplitContainer.Panel2.Controls.Add(this.listBox2);
            this.instanceSplitContainer.Size = new System.Drawing.Size(903, 468);
            this.instanceSplitContainer.SplitterDistance = 510;
            this.instanceSplitContainer.TabIndex = 2;
            // 
            // listBox2
            // 
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(0, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(389, 468);
            this.listBox2.TabIndex = 0;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // notifyIconDashBoard
            // 
            this.notifyIconDashBoard.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconDashBoard.Icon")));
            this.notifyIconDashBoard.Text = "notifyIconDashBoard";
            this.notifyIconDashBoard.Visible = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.progressBar1.Location = new System.Drawing.Point(20, 60);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(5, 530);
            this.progressBar1.TabIndex = 1;
            // 
            // DashboardForm
            // 
         
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.ApplyImageInvert = true;
            //this.BackImage = global::WD.XLC.WIN.Properties.Resources.WD;
            //this.BackImagePadding = new System.Windows.Forms.Padding(160, 20, 0, 0);
            //this.BackMaxSize = 30;
            //this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            // this.ShadowType = MetroFramework.Forms.MetroFormShadowType.SystemShadow;
            // this.StyleManager = this.metroStyleManager;
            this.ClientSize = new System.Drawing.Size(976, 610);
            this.Controls.Add(this.dashBoardControl);
            this.Controls.Add(this.progressBar1);
            this.Icon = global::WD.XLC.WIN.Properties.Resources.favicon;
            this.Name = "DashboardForm";
          
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.dashBoardControl.ResumeLayout(false);
            this.connectionPage.ResumeLayout(false);
            this.connectionSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.connectionSplitContainer)).EndInit();
            this.connectionSplitContainer.ResumeLayout(false);
            this.serverPage.ResumeLayout(false);
            this.serverPageSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serverPageSplitContainer)).EndInit();
            this.serverPageSplitContainer.ResumeLayout(false);
            this.templatePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dashBoardSplitContainer)).EndInit();
            this.dashBoardSplitContainer.ResumeLayout(false);
            this.instancePage.ResumeLayout(false);
            this.instanceSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.instanceSplitContainer)).EndInit();
            this.instanceSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        #region Metro
        private MetroStyleManager metroStyleManager;
        private MetroTabControl dashBoardControl;
        private MetroTabPage serverPage;
        private MetroTabPage templatePage;
        private MetroTabPage mappingPage;
        private MetroTabPage LoaderPage;
        private MetroTabPage connectionPage;
        private MetroTabPage instancePage;

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private SplitContainer dashBoardSplitContainer;
        private NotifyIcon notifyIconDashBoard;
        private SplitContainer serverPageSplitContainer;
        private MetroButton btnAddServer;
        private SplitContainer connectionSplitContainer;
        private SplitContainer instanceSplitContainer;
        private ListBox listBox1;
        private ListBox listBox2;
        private VerticalProgress progressBar1;

        

    }
}