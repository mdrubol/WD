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



// namespace: WD.XLC.WIN.Controls
//
// summary:	.


namespace WD.XLC.WIN.Controls
{
    
    /// <summary>   A template list view. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    

    public partial class TemplateListControl : MetroUserControl
    {
        
        /// <summary>   Cell double click. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Data grid view cell event information. </param>
        

        public delegate void CellDoubleClick(object sender, DataGridViewCellEventArgs e);

        /// <summary>   The on double clicked. </summary>
        public CellDoubleClick OnDoubleClicked;

        
        /// <summary>   Deletes the grid described by ID. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="Id">   The identifier. </param>
        

        public delegate void DeleteGrid(string Id);
        public delegate void CopyGrid(string Id, string templateName);
        /// <summary>   The on delete grid. </summary>
        public DeleteGrid OnDeleteGrid;
        /// <summary>
        /// The on copy grid
        /// </summary>
        public CopyGrid OnCopyGrid;
        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        

        public TemplateListControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <remarks>Shahid K, 7/21/2017.</remarks>
                
        public TemplateListControl(int max)
        {
            this.max = max;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        
        /// <summary>   Loads a grid. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="iList">    Zero-based index of the list. </param>
        

        public void LoadGrid(List<AppConfig> iList) {

            this.grdTemplateList.AutoGenerateColumns = false;
            this.grdTemplateList.DataSource = iList;
        }

        
        /// <summary>   Event handler. Called by grdTemplateList for cell double click events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Data grid view cell event information. </param>
        

        private void grdTemplateList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (OnDoubleClicked != null)
            {
                OnDoubleClicked(sender, e);
            }
        }

        
        /// <summary>   Event handler. Called by grdTemplateList for mouse click events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Mouse event information. </param>
        

        private void grdTemplateList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Delete", btn_delete));
                m.MenuItems.Add(new MenuItem("Copy", btn_duplicate));
                m.Show(grdTemplateList, new Point(e.X, e.Y));
            }
        }

        
        /// <summary>   Event handler. Called by btn for delete events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        

        private void btn_delete(object sender, EventArgs e)
        {
            if(OnDeleteGrid!=null)
            {
                OnDeleteGrid(grdTemplateList.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            }
        }

        private void btn_duplicate(object sender, EventArgs e) {

            if (OnCopyGrid != null)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Please add template name?", "Duplicate", grdTemplateList.SelectedCells[0].OwningRow.Cells[1].Value.ToString() + " (1)");
                if (!string.IsNullOrEmpty(input))
                {
                    bool isduplicate = false;
                    foreach (DataGridViewRow r in grdTemplateList.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[1].Value != null))
                    {
                        if (r.Cells[1].Value.ToString().ToUpper() == input.ToUpper())
                        {
                            isduplicate = true;
                            break;
                        }
                    }
                    if (!isduplicate)
                    {
                        OnCopyGrid(grdTemplateList.SelectedCells[0].OwningRow.Cells[0].Value.ToString(), input);
                    }
                    else
                    {

                        MessageBox.Show("Duplicate names not allowed!","Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btn_duplicate(sender, e);
                    }
                }
               
            }
        }

        private void grdTemplateList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) 
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DataGridView grd = (DataGridView)sender;
                    DataGridViewRow row = grd.CurrentRow;
                    OnDeleteGrid(row.Cells[0].Value.ToString());
                }
            }
            
        }
    }
}
