using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Emgu.CV.UI
{
    /// <summary>
    /// The dialog to ask user for parameters to the specific method
    /// </summary>
    public partial class ParamInputDlg : Form
    {
        private bool _sucessed = false;
        private Dictionary<ParameterInfo, ParamInputPanel> _paramPanelDictionary;
        private List<Object> _pList;
        private ParameterInfo[] _paramInfo;

        private ParamInputDlg(ParameterInfo[] paramInfo, List<Object> paramList)
        {
            InitializeComponent();

            _paramPanelDictionary = new Dictionary<ParameterInfo, ParamInputPanel>();
            _pList = paramList;
            _paramInfo = paramInfo;

            for (int i = 0; i < paramInfo.Length; i++)
            {
                ParamInputPanel panel = CreatePanelForParameter(paramInfo[i]);
                parameterInputPanel.Controls.Add(panel);
                panel.Location = new Point(0, i * panel.Height);
                _paramPanelDictionary.Add(paramInfo[i], panel);
            }
        }

        private bool Successed
        {
            get { return _sucessed; }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            bool valid = true;
            _pList.Clear();

            foreach (ParameterInfo pi in _paramInfo)
            {
                ParamInputPanel panel = _paramPanelDictionary[pi];
                Object value = panel.GetValue();
                if (value == null)
                {
                    valid = false;
                    MessageBox.Show("Parameter {0} is invalid.", pi.Name);
                    break;
                }
                else
                {
                    _pList.Add(value);
                }
            }

            if (valid)
            {
                _sucessed = true;
                Close();
            }
        }

        private class ParamInputPanel : Panel
        {
            private GetParamDelegate _getParamFunction;
            private String _paramName;

            /// <summary>
            /// Return the value of the parameter input panel, if unable to retrieve value, return null
            /// </summary>
            /// <returns>The value of the parameter input panel, if unable to retrieve value, return null</returns>
            public Object GetValue()
            {
                Object o = null;
                try
                {
                    o = _getParamFunction();
                }
                catch (Exception)
                {
                    return null;
                }
                return o;
            }

            public delegate Object GetParamDelegate();

            /// <summary>
            /// The function used to obtain the parameter from this input panel
            /// </summary>
            public GetParamDelegate GetParamFunction
            {
                set
                {
                    _getParamFunction = value;
                }
            }

            public String ParamName
            {
                get
                {
                    return _paramName;
                }
                set
                {
                    _paramName = value;
                }
            }
        }

        private static String ParseParameterName(ParameterInfo param)
        {
            String name = param.Name;

            #region Add space before every upper case character
            Char[] nameChars = name.ToCharArray();
            List<Char> charList = new List<char>();
            foreach (Char c in nameChars)
            {
                if (c.CompareTo('A') >= 0 && c.CompareTo('Z') <= 0)
                {   //upper case char
                    charList.Add(' ');
                }
                charList.Add(c);
            }
            name = new string(charList.ToArray());
            #endregion

            //convert the first letter to upper case
            name = name.Substring(0, 1).ToUpper() + name.Substring(1);
            return name;
        }

        /// <summary>
        /// Create a panel for the specific parameter
        /// </summary>
        /// <param name="param">the parameter to create panel for</param>
        /// <returns>the panel</returns>
        private static ParamInputPanel CreatePanelForParameter(ParameterInfo param)
        {
            Type paramType = param.ParameterType;

            ParamInputPanel panel = new ParamInputPanel();
            panel.Height = 50;
            panel.Width = 400;
            int textBoxStartX = 100, textBoxStartY = 10;

            #region add the label for the parameter
            Label paramNameLabel = new Label();
            paramNameLabel.Text = ParseParameterName(param) + ":";
            paramNameLabel.AutoSize = true;
            panel.Controls.Add(paramNameLabel);
            paramNameLabel.Location = new System.Drawing.Point(10, textBoxStartY);
            #endregion

            if (paramType.IsEnum)
            {
                ComboBox combo = new ComboBox();
                panel.Controls.Add(combo);
                combo.Location = new System.Drawing.Point(textBoxStartX, textBoxStartY);
                combo.Items.AddRange(Enum.GetNames(paramType));
                combo.SelectedIndex = 0;

                panel.GetParamFunction =
                    delegate()
                    {
                        return Enum.Parse(paramType, combo.SelectedItem.ToString(), true);
                    };
            }
            else if (paramType == typeof(bool))
            {
                ComboBox combo = new ComboBox();
                panel.Controls.Add(combo);
                combo.Location = new System.Drawing.Point(textBoxStartX, textBoxStartY);
                combo.Items.AddRange(new String[] { "True", "False" });
                combo.SelectedIndex = 0;
                panel.GetParamFunction =
                    delegate()
                    {
                        return combo.SelectedItem.ToString().Equals("True");
                    };
            }
            else if (paramType == typeof(UInt64) || paramType == typeof(int) || paramType == typeof(double))
            {
                //Create inpout box for the int paramater
                TextBox inputTextBox = new TextBox();
                panel.Controls.Add(inputTextBox);
                inputTextBox.Location = new System.Drawing.Point(textBoxStartX, textBoxStartY);
                inputTextBox.Text = "0";

                panel.GetParamFunction =
                    delegate()
                    {
                        return System.Convert.ChangeType(inputTextBox.Text, paramType);
                    };
            }
            else if (paramType == typeof(MCvScalar))
            {
                TextBox[] inputBoxes = new TextBox[4];
                int boxWidth = 40;

                //Create input boxes for the scalar value
                for (int i = 0; i < inputBoxes.Length; i++)
                {
                    inputBoxes[i] = new TextBox();
                    panel.Controls.Add(inputBoxes[i]);
                    inputBoxes[i].Location = new System.Drawing.Point(textBoxStartX + i * (boxWidth + 5), textBoxStartY);
                    inputBoxes[i].Width = boxWidth;
                    inputBoxes[i].Text = "0.0";
                }
                panel.GetParamFunction =
                    delegate()
                    {
                        double[] values = new double[4];
                        for (int i = 0; i < inputBoxes.Length; i++)
                        {
                            values[i] = System.Convert.ToDouble(inputBoxes[i].Text);
                        }
                        return new MCvScalar(values[0], values[1], values[2], values[3]);
                    };
            }
            else
            {
                throw new NotSupportedException(String.Format("Parameter type '{0}' is not supported", paramType.Name));
            }
            return panel;
        }

        /// <summary>
        /// Obtain the parameters for <paramref name="method"/> and put them in <paramref name="paramList"/>
        /// </summary>
        /// <param name="method">The method to Obtain parameters from</param>
        /// <param name="paramList">The list that will be used as the storage for the retrieved parameters</param>
        /// <returns>True if successed, false otherwise</returns>
        public static bool GetParams(MethodInfo method, List<Object> paramList)
        {
            ParameterInfo[] parameters = method.GetParameters();

            //if the method requires no parameter, simply return true
            if (parameters.Length == 0) return true;

            #region Handle the cases where at least one parameter is required as input

            ParamInputDlg dlg = new ParamInputDlg(parameters, paramList);
            dlg.ShowDialog();
            return dlg.Successed;

            #endregion
        }
    }
}