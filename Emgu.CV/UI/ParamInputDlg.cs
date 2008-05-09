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

            public Object GetValue()
            {
                return _getParamFunction();
            }

            public delegate Object GetParamDelegate();

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

            Label paramNameLabel = new Label();
            paramNameLabel.Text = param.Name;
            paramNameLabel.AutoSize = true;
            panel.Controls.Add(paramNameLabel);
            paramNameLabel.Location = new System.Drawing.Point(10, 10);

            if (paramType == typeof(int))
            {
                //Do something here
                TextBox inputTextBox = new TextBox();
                panel.Controls.Add(inputTextBox);
                inputTextBox.Location = new System.Drawing.Point(100, 10);
                panel.GetParamFunction =
                    delegate()
                    {
                        Object o = null;
                        try
                        {
                            o = System.Convert.ToInt32(inputTextBox.Text);
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                        return o;
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