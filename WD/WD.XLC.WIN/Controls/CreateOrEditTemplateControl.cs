using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using WD.XLC.Domain.Entities;
using WD.DataAccess.Context;
using WD.XLC.Domain.Helpers;
using System.Text.RegularExpressions;



// namespace: WD.XLC.WIN.Controls
//
// summary:	.


namespace WD.XLC.WIN.Controls
{
    public partial class CreateOrEditTemplateControl : MetroUserControl
    {
        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="input">    The input. </param>


        public CreateOrEditTemplateControl(AppConfig input)
            : this(input, 1)
        { 
        
        }

        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="input">    The input. </param>
        /// <param name="max">      The maximum. </param>


        public CreateOrEditTemplateControl(AppConfig input, int max)
            : base()
        {
            this.appConfig = input;
            this.mapper = string.IsNullOrEmpty(input.Config) ? new Mapping() : Utility.ConvertJsonToObject<Mapping>(input.Config);
            this.max = max;
            InitializeComponent();
            txtTemplateName.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            txtTemplateName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            btnAddTemplate.Text = string.IsNullOrEmpty(appConfig.Id) ? "Add Template" : "Update Template";
            txtTemplateName.Text = appConfig.RecId;
            chkIsActive.Checked = input.IsActive == 0 ? false : true;
            txtTemplateName.Focus();
            this.Dock = DockStyle.Fill;
        }
        /// <summary>   Event handler. Called by btnAddTemplate for click events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        private void btnAddTemplate_Click(object sender, EventArgs e)
        {
            if (SaveChanges != null)
            {
                if (cbxServer.SelectedItem != null &&
                    cbxTable.SelectedItem != null &&
                    !string.IsNullOrEmpty(txtTemplateName.Text)
                    )
                {
                    appConfig.ServerName = ((ServerInfo)cbxServer.SelectedItem).ServerName;
                    appConfig.DbProvider = ((ServerInfo)cbxServer.SelectedItem).DbProvider;
                    appConfig.ConnString = ((ServerInfo)cbxServer.SelectedItem).ConnString;
                    appConfig.TargetTableName = cbxTable.SelectedItem.ToString();
                    appConfig.ServerId = ((ServerInfo)cbxServer.SelectedItem).Id;
                    appConfig.RecId = txtTemplateName.Text;
                    appConfig.Config = Utility.ConvertObjectToJson<Mapping>(new Mapping()
                    {
                        Columns = this.mapper.Columns,

                    });
                    appConfig.Config = appConfig.Config.Trim();
                    appConfig.IsActive = chkIsActive.Checked ? 1 : 0;
                    if (!CheckDuplicate(appConfig))
                    {
                        SaveChanges(appConfig);
                    }
                    else
                    {
                        txtTemplateName.Focus();
                        MessageBox.Show("Template Name already present.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("All fields are required.","Validation",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
        }
        /// <summary>   Event handler. Called by btnCancel for click events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (ResetChanges != null)
            {
                ResetChanges();
            }
        }
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Cancel event information. </param>
        
        public void LoadServer(List<ServerInfo> items)
        {
            cbxServer.ValueMember = "Id";
            cbxServer.DisplayMember = "ServerName";
            cbxServer.DataSource = items;
        }

       
      public void LoadTable() 
      {

          try
          {
              cbxTable.Text = string.Empty;
              cbxTable.Items.Clear();
              if (cbxServer.SelectedItem != null)
              {
                  if (!String.IsNullOrEmpty(((ServerInfo)cbxServer.SelectedItem).ConnString))
                  {
                      ServerInfo info = cbxServer.SelectedItem as ServerInfo;
                      cbxTable.Items.AddRange(CheckTable(info.ConnString, info.DbProvider));
                  }
              }
          }
          catch (Exception exc) {
              MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
        

        private void cbxServer_SelectionChangeCommitted(object sender, EventArgs e)
        {

            LoadTable();
        }

        private void CreateOrEditTemplateControl_Resize(object sender, EventArgs e)
        {
            txtTemplateName.Width = cbxTable.Width = cbxServer.Width = btnAddTemplate.Width = btnCancel.Width = this.Width - 10;
        }
    }

    public delegate void CheckBoxClickedHandler(bool state);
    public class DataGridViewCheckBoxHeaderCellEventArgs : EventArgs
    {
        bool _bChecked;
        public DataGridViewCheckBoxHeaderCellEventArgs(bool bChecked)
        {
            _bChecked = bChecked;
        }
        public bool Checked
        {
            get { return _bChecked; }
        }
    }
    public class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        Point checkBoxLocation;
        Size checkBoxSize;
        bool _checked = false;
        Point _cellLocation = new Point();
        System.Windows.Forms.VisualStyles.CheckBoxState _cbState =
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
        public event CheckBoxClickedHandler OnCheckBoxClicked;

        public DatagridViewCheckBoxHeaderCell()
            : base()
        {
        }

        protected override void Paint(System.Drawing.Graphics graphics,
            System.Drawing.Rectangle clipBounds,
            System.Drawing.Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                dataGridViewElementState, value,
                formattedValue, errorText, cellStyle,
                advancedBorderStyle, paintParts);
            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(graphics,
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            p.X = cellBounds.Location.X +
                (cellBounds.Width / 2) - (s.Width / 2);
            p.Y = cellBounds.Location.Y +
                (cellBounds.Height / 2) - (s.Height / 2);
            _cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;
            if (_checked)
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.CheckedNormal;
            else
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.UncheckedNormal;
            CheckBoxRenderer.DrawCheckBox
            (graphics, checkBoxLocation, _cbState);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
            if (p.X >= checkBoxLocation.X && p.X <=
                checkBoxLocation.X + checkBoxSize.Width
            && p.Y >= checkBoxLocation.Y && p.Y <=
                checkBoxLocation.Y + checkBoxSize.Height)
            {
                _checked = !_checked;
                if (OnCheckBoxClicked != null)
                {
                    OnCheckBoxClicked(_checked);
                    this.DataGridView.InvalidateCell(this);
                }

            }
            base.OnMouseClick(e);
        }
    }
}
