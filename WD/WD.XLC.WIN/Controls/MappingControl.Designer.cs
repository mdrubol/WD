using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WD.DataAccess.Context;
using WD.XLC.Domain.Entities;
namespace WD.XLC.WIN.Controls
{
    partial class MappingControl
    {
        public delegate AppConfig AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e);
        public AfterSelect AfterSelected;
        public delegate void SaveChange(List<ColumnConfig> input_rows, string id);
        public SaveChange SaveChanges;
        private readonly int max;

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
                components.Dispose();
            }
            base.Dispose(disposing);
            // Suppress finalization.
            System.GC.SuppressFinalize(this);
        }
        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MappingControl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.treeImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Format = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsPrimary = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.btnUp = new System.Windows.Forms.PictureBox();
            this.btnDown = new System.Windows.Forms.PictureBox();
            this.btnUpdated = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDown)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(819, 617);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.treeImageList;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(160, 617);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // treeImageList
            // 
            this.treeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.treeImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.treeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnUpdated);
            this.splitContainer2.Size = new System.Drawing.Size(655, 617);
            this.splitContainer2.SplitterDistance = 581;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(655, 581);
            this.splitContainer3.SplitterDistance = 625;
            this.splitContainer3.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.ColumnName,
            this.DataType,
            this.Format,
            this.Length,
            this.DefaultValue,
            this.IsPrimary});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(625, 581);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            // 
            // Index
            // 
            this.Index.DataPropertyName = "Index";
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Visible = false;
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnName.DataPropertyName = "ColumnName";
            this.ColumnName.HeaderText = "CV Column";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // DataType
            // 
            this.DataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DataType.DataPropertyName = "DataType";
            this.DataType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DataType.HeaderText = "Data Type";
            this.DataType.Items.AddRange(new object[] {
            "bool",
            "byte",
            "datetime",
            "decimal",
            "double",
            "float",
            "int",
            "int16",
            "int32",
            "int64",
            "long",
            "string"});
            this.DataType.Name = "DataType";
            this.DataType.Sorted = true;
            // 
            // Format
            // 
            this.Format.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Format.DataPropertyName = "Format";
            this.Format.HeaderText = "Format";
            this.Format.Name = "Format";
            this.Format.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Length
            // 
            this.Length.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Length.DataPropertyName = "Length";
            this.Length.HeaderText = "Length";
            this.Length.Name = "Length";
            this.Length.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DefaultValue
            // 
            this.DefaultValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DefaultValue.DataPropertyName = "DefaultValue";
            this.DefaultValue.HeaderText = "DefaultValue";
            this.DefaultValue.Name = "DefaultValue";
            this.DefaultValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // IsPrimary
            // 
            this.IsPrimary.DataPropertyName = "IsPrimary";
            this.IsPrimary.HeaderText = "IsPrimary";
            this.IsPrimary.Name = "IsPrimary";
            this.IsPrimary.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.btnUp);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.btnDown);
            this.splitContainer4.Size = new System.Drawing.Size(26, 581);
            this.splitContainer4.SplitterDistance = 290;
            this.splitContainer4.TabIndex = 0;
            // 
            // btnUp
            // 
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUp.ErrorImage = ((System.Drawing.Image)(resources.GetObject("btnUp.ErrorImage")));
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(0, 274);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(26, 16);
            this.btnUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnUp.TabIndex = 0;
            this.btnUp.TabStop = false;
            this.btnUp.Visible = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(0, 0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(26, 16);
            this.btnDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnDown.TabIndex = 1;
            this.btnDown.TabStop = false;
            this.btnDown.Visible = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUpdated
            // 
            this.btnUpdated.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUpdated.Location = new System.Drawing.Point(0, 9);
            this.btnUpdated.Name = "btnUpdated";
            this.btnUpdated.Size = new System.Drawing.Size(655, 23);
            this.btnUpdated.Style = MetroFramework.MetroColorStyle.Green;
            this.btnUpdated.TabIndex = 2;
            this.btnUpdated.Text = "Save Changes";
            this.btnUpdated.Theme = MetroFramework.MetroThemeStyle.Light;
            this.btnUpdated.UseSelectable = true;
            this.btnUpdated.Click += new System.EventHandler(this.btnUpdated_Click);
            // 
            // MappingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MappingControl";
            this.Size = new System.Drawing.Size(819, 617);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private MetroFramework.Controls.MetroButton btnUpdated;
        private ImageList treeImageList;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainer3;
        private SplitContainer splitContainer4;
        private PictureBox btnUp;
        private PictureBox btnDown;
        private DataGridViewTextBoxColumn Index;
        private DataGridViewTextBoxColumn ColumnName;
        private DataGridViewComboBoxColumn DataType;
        private DataGridViewTextBoxColumn Format;
        private DataGridViewTextBoxColumn Length;
        private DataGridViewTextBoxColumn DefaultValue;
        private DataGridViewCheckBoxColumn IsPrimary;
    }
}
