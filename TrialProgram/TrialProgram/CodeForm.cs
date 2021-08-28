using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TrialProgram
{
    public partial class CodeForm : Form
    {
        public CodeForm(string uri, string link, int n)
        {
            InitializeComponent();
            TextUri.Text = uri;
            TextCode.Text = LinkInfo.GetPageCode(new Uri(uri));
            FindLinksInText(link, n);
        }

        private void FindLinksInText(string link, int n)
        {
            int i = 0;
            for (int k = 0; k < n - 1; k++)
            {
                i = TextCode.Text.IndexOf('\n', i);
                //i++;
            }
            i = TextCode.Text.IndexOf(link, i);
            if (i < 0)
            {
                return;
            }
            TextCode.SelectionStart = i;
            TextCode.SelectionLength = link.Length;
            TextCode.SelectionColor = Color.Red;
        }
    }
}
