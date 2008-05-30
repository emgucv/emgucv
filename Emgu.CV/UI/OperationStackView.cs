using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Emgu.Reflection;
using Emgu;

namespace Emgu.CV.UI
{
    public partial class OperationStackView : UserControl
    {
        //private Stack<Operation<IImage>> _operationStack;
        private DataGridViewTextBoxColumn _codeColumn;
        private Emgu.Utils.ProgrammingLanguage _language;
        public OperationStackView()
        {
            InitializeComponent();
            _codeColumn = new DataGridViewTextBoxColumn();
            _codeColumn.Name = "code";
            _codeColumn.ReadOnly = true;
            _codeColumn.Width = 300;
            dataGridView1.Columns.Add(_codeColumn);
            dataGridView1.RowHeadersVisible = false;
        }

        public Emgu.Utils.ProgrammingLanguage Language
        {
            set
            {
                _language = value;
            }
        }

        public void SetOperationStack(Stack<Operation<IImage>> operationStack)
        {
            if (_language == Emgu.Utils.ProgrammingLanguage.CSharp)
            {
                topLabel.Text = "public static IImage Function(IImage image)\r\n{";
                bottomLabel.Text = "}";
            }
            else if (_language == Emgu.Utils.ProgrammingLanguage.CPlusPlus)
            {
                topLabel.Text = "public static IImage^ Function(IImage^ image)\r\n{";
                bottomLabel.Text = "}";
            }

            dataGridView1.Rows.Clear();
            String[] codes = GetOperationCode(operationStack);
            if (codes.Length > 0)
            {
                dataGridView1.Rows.Add(codes.Length);
                for (int i = 0; i < codes.Length; i++)
                    dataGridView1.Rows[i].Cells[_codeColumn.Name].Value = codes[i];
            }
        }

        private String[] GetOperationCode(Stack<Operation<IImage>> operationStack)
        {
            List<String> ops = new List<string>();
            foreach (Operation<IImage> op in operationStack)
            {
                String str = op.ToCode(_language).Replace("{instance}", "image");

                if (_language == Emgu.Utils.ProgrammingLanguage.CSharp || _language == Emgu.Utils.ProgrammingLanguage.CPlusPlus)
                {
                    if (op.Method.ReturnType == typeof(void))
                    {
                        str += ";";
                    }
                    else
                    {
                        str = "image = " + str + ";";
                    }
                }

                ops.Add(str);
            }
            string[] res =  ops.ToArray();
            Array.Reverse(res);
            return res;
        }
    }
}
