using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrialProgram
{
    public partial class AboutLinkForm : Form
    {
        private AboutLink aboutLink;
        private DataTable positions;
        private DataColumn uriPos, line;
        private DataTable redirects;
        private DataColumn uriRed, status;

        private void LocationGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CodeForm codeForm = new CodeForm(LocationGridView[0, e.RowIndex].Value.ToString(), aboutLink.FirstUri, int.Parse(LocationGridView[1, e.RowIndex].Value.ToString()));
            codeForm.Show();
        }

        public AboutLinkForm(AboutLink aboutLink, string type)
        {
            InitializeComponent();
            this.aboutLink = aboutLink;
            URLLabel.Text = aboutLink.Uri;
            if(aboutLink.ExceptionMessage.Length == 0)
            {
                StatusLabel.Text = (int)aboutLink.Status + " " + aboutLink.Status.ToString();
                HeadersLabel.Text = LinkInfo.GetHeaders(aboutLink.Uri);
            }
            else
            {
                StatusLabel.Text = aboutLink.ExceptionMessage;
            }
            TypeLabel.Text = type;
            uriPos = new DataColumn("Страница");
            line = new DataColumn("Номер строки");
            positions = new DataTable("Местонахождение");
            positions.Columns.AddRange(new DataColumn[] { uriPos, line });
            foreach(var item in aboutLink.Positions)
            {
                DataRow row = positions.NewRow();
                row[0] = item.ParentUri;
                row[1] = item.Line;
                positions.Rows.Add(row);
            }
            LocationGridView.DataSource = positions;
            LocationGridView.Columns[0].MinimumWidth = 250;
            LocationGridView.Columns[1].MinimumWidth = 60;
            for (int i = 0; i < LocationGridView.Rows.Count; i++)
            {
                LocationGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            uriRed = new DataColumn("Страница");
            status = new DataColumn("Статус");
            redirects = new DataTable("Перенаправления");
            redirects.Columns.AddRange(new DataColumn[] { uriRed, status });
            var mas = aboutLink.Redirects;
            for (int i = 1; i < mas.Count; i++)
            {
                DataRow row = redirects.NewRow();
                row[0] = mas[i].Locations;
                row[1] = (int)mas[i].Status + mas[i].Status.ToString();
                redirects.Rows.Add(row);
            }
            RedirectGridView.DataSource = redirects;
            RedirectGridView.Columns[0].MinimumWidth = 250;
            RedirectGridView.Columns[1].MinimumWidth = 60;
            for (int i = 0; i < RedirectGridView.Rows.Count; i++)
            {
                RedirectGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }
    }
}
