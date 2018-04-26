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
    
    /// <summary>   A ServerInfo list view. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>
    

    public partial class ServerInfoListControl : MetroUserControl
    {
        
        /// <summary>   Cell double click. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Data grid view cell event information. </param>
        

        public delegate void ServerCellDoubleClick(object sender, DataGridViewCellEventArgs e);

        /// <summary>   The on double clicked. </summary>
        public ServerCellDoubleClick OnServerDoubleClicked;

        /// <summary>   Deletes the grid described by ID. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="Id">   The identifier. </param>
        

        public delegate void ServerDeleteGrid(string Id);
        /// <summary>   The on delete grid. </summary>
        public ServerDeleteGrid OnServerDeleteGrid;
        
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        

        public ServerInfoListControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <remarks>Shahid K, 7/21/2017.</remarks>
                
        public ServerInfoListControl(int max)
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
        

        public void LoadGrid(List<ServerInfo> iList) {

            this.grdServerInfoList.AutoGenerateColumns = false;
            this.grdServerInfoList.DataSource = iList;
        }

        
        /// <summary>   Event handler. Called by grdServerInfoList for cell double click events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Data grid view cell event information. </param>
        

        private void grdServerInfoList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (OnServerDoubleClicked != null)
            {
                OnServerDoubleClicked(sender, e);
            }
        }

        
        /// <summary>   Event handler. Called by grdServerInfoList for mouse click events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Mouse event information. </param>
        

        private void grdServerInfoList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Delete", btn_delete));
                m.Show(grdServerInfoList, new Point(e.X, e.Y));
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
            if (OnServerDeleteGrid != null)
            {
                OnServerDeleteGrid(grdServerInfoList.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            }
        }

        private void grdServerInfoList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DataGridView grd = (DataGridView)sender;
                    DataGridViewRow row = grd.CurrentRow;
                    OnServerDeleteGrid(row.Cells[0].Value.ToString());
                }
            }
        }

    }
}
