using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewTemplateUtils
{
    public partial class FormSelectTemplates : Form
    {
        public List<MyView> templates;
        public List<MyView> selectedTemplates;

        public FormSelectTemplates(List<MyView> Templates)
        {
            InitializeComponent();
            templates = Templates;
            listBox1.DataSource = templates;
            
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            selectedTemplates = new List<MyView>();
            foreach(object item in listBox1.SelectedItems)
            {
                MyView selView = item as MyView;
                if (selView == null) throw new Exception("Это не вид!");

                selectedTemplates.Add(selView);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
