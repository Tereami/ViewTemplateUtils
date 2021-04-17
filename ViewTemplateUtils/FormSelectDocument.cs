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
    public partial class FormSelectDocument : Form
    {
        public MyDocument selectedDocument;
        public List<MyDocument> allDocuments;

        public FormSelectDocument(List<MyDocument> AllDocuments)
        {
            InitializeComponent();

            allDocuments = AllDocuments;
            comboBox1.DataSource = allDocuments;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            selectedDocument = comboBox1.SelectedItem as MyDocument;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
