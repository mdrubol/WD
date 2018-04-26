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
using WD.DataAccess.Helpers;

namespace WD.XLC.WIN.Controls
{
    public partial class InstanceConfigControl : MetroUserControl
    {
        public OnDeleteConfig DeleteConfig;
        public delegate void OnDeleteConfig();
        public OnSaveConfig SaveConfig;
        public delegate void OnSaveConfig(string id);
        private readonly int max = 0;
        public InstanceConfigControl(int max)
        {
            this.max = max;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Size = new System.Drawing.Size(400, 700);

        }
        internal void Bind(ServerProcess sp)
        {
            try
            {
                if (sp != null)
                {
                    metroRadioButton2.Checked = true;
                }
                else
                {
                    metroRadioButton1.Checked = true;
                }
                dataGridView1.Visible = metroRadioButton2.Checked;
            }
            catch
            {

            }
        }
        private void On_CheckedChanged(object sender, EventArgs e)
        {
            if (metroRadioButton1.Checked) 
            {
                DeleteConfig();
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    row.Cells[2].Value = false;
                }
            }
            dataGridView1.Visible = metroRadioButton2.Checked;
        }
        internal void LoadGrid(DataTable dt)
        {
            foreach (DataRow row in dt.Rows) {

                System.Windows.Forms.DataGridViewRow r = new System.Windows.Forms.DataGridViewRow();
                bool flag = (row["ProcessId"].ToString()==this.max.ToString() && row["MachineId"].ToString() ==Environment.MachineName) ? true : false;
                r.CreateCells(dataGridView1, new object[] { row["ServerId"].ToString(), row["ServerName"].ToString(), flag });
                dataGridView1.Rows.AddRange(r);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //We make DataGridCheckBoxColumn commit changes with single click
            //use index of logout column
            //if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            //    this.dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //Check the value of cell
            if ((bool)this.dataGridView1.CurrentCell.EditedFormattedValue == true)
            {
                //Use index of TimeOut column
                //this.dataGridView1.Rows[e.RowIndex].Cells[3].Value = DateTime.Now;

                //Set other columns values
                foreach (DataGridViewRow row in this.dataGridView1.Rows.Cast<DataGridViewRow>().Where(x => x.Index != e.RowIndex)) {
                    row.Cells[2].Value = false;
                }
                if (SaveConfig != null) {
                    SaveConfig(this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                }

            }
            else
            {
                //Use index of TimeOut column
             //   this.dataGridView1.Rows[e.RowIndex].Cells[3].Value = DBNull.Value;

                //Set other columns values
            }
        }
    }
}
