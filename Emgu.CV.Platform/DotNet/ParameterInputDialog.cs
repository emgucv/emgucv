//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Emgu.CV.Reflection;
using Emgu.CV.Structure;

namespace Emgu.CV.UI
{
    /// <summary>
    /// The dialog to ask user for parameters to the specific method
    /// </summary>
    internal partial class ParameterInputDialog : Form
    {
        private bool _sucessed;

        /// <summary>
        /// The List of parameter values
        /// </summary>
        private Object[] _paramValue;

        /// <summary>
        /// The array of parameter info
        /// </summary>
        private ParameterInfo[] _paramInfo;

        private ParamInputPanel[] _paramPanel;

        /// <summary>
        /// Get the parameters obtained by this parameter input dialog
        /// </summary>
        public Object[] Parameters
        {
            get
            {
                return _paramValue;
            }
        }

        private ParameterInputDialog(ParameterInfo[] paramInfo, Object[] paramList)
        {
            InitializeComponent();

            int panelsHeight = 0;

            _paramValue = new object[paramInfo.Length];

            _paramInfo = paramInfo;
            _paramPanel = new ParamInputPanel[paramInfo.Length];

            for (int i = 0; i < paramInfo.Length; i++)
            {
                ParamInputPanel panel = CreatePanelForParameter(
                   paramInfo[i],
                   (paramList == null || i >= paramList.Length) ? null : paramList[i]);
                parameterInputPanel.Controls.Add(panel);
                panel.Location = new Point(0, i * panel.Height);
                _paramPanel[i] = panel;

                panelsHeight += panel.Height;
            }

            Height = panelsHeight + 100;
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
            for (int i = 0; i < _paramInfo.Length; i++)
            {
                ParamInputPanel panel = _paramPanel[i];
                Object value = panel.GetValue();
                if (value == null)
                {
                    MessageBox.Show(String.Format("Parameter {0} is invalid.", panel.Name));
                    return;
                }

                _paramValue[i] = value;
            }

            _sucessed = true;
            Close();
        }

        private class ParamInputPanel : Panel
        {
            private GetParamDelegate _getParamFunction;

            /// <summary>
            /// Return the value of the parameter input panel, if unable to retrieve value, return null
            /// </summary>
            /// <returns>The value of the parameter input panel, if unable to retrieve value, return null</returns>
            public Object GetValue()
            {
                try
                {
                    return _getParamFunction();
                }
                catch
                {
                    return null;
                }
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
        }

        /// <summary>
        /// Get the name of the parameter
        /// </summary>
        /// <param name="param">the parameter</param>
        /// <returns>the name of the parameter</returns>
        private static String ParseParameterName(ParameterInfo param)
        {
            String name = param.Name;

            #region Add space before every upper case character
            Char[] nameChars = name.ToCharArray();
            List<Char> charList = new List<char>();
            foreach (Char c in nameChars)
            {
                if (Char.IsUpper(c))
                {   //upper case char
                    charList.Add(' ');
                }
                charList.Add(c);
            }
            #endregion

            //convert the first letter to upper case
            charList[0] = Char.ToUpper(charList[0]);
            return new string(charList.ToArray());
        }

        /// <summary>
        /// Create a panel for the specific parameter
        /// </summary>
        /// <param name="param">the parameter to create panel for</param>
        /// <param name="defaultValue">The default value for the parameter</param>
        /// <returns>the panel</returns>
        private static ParamInputPanel CreatePanelForParameter(ParameterInfo param, object defaultValue)
        {
            ParamInputPanel panel = new ParamInputPanel();
            panel.Height = 50;
            panel.Width = 400;
            Point textBoxStart = new Point(100, 10);

            #region add the label for the parameter
            Label paramNameLabel = new Label();
            paramNameLabel.AutoSize = true;
            panel.Controls.Add(paramNameLabel);
            paramNameLabel.Location = new Point(10, textBoxStart.Y);
            #endregion

            if (param == null)
            {  // a generic parameter

                GenericParameter p = defaultValue as GenericParameter;

                paramNameLabel.Text = "";

                String[] options = Array.ConvertAll<Type, String>(p.AvailableTypes, delegate (Type t) { return t.Name; });
                ComboBox combo = new ComboBox();
                panel.Controls.Add(combo);
                combo.Location = textBoxStart;
                combo.Items.AddRange(options);
                combo.SelectedIndex = Array.FindIndex<String>(options, p.SelectedType.ToString().Equals);
                panel.GetParamFunction =
                    delegate ()
                    {
                        return
                       new GenericParameter(
                        p.AvailableTypes[Array.FindIndex<String>(options, combo.Text.ToString().Equals)],
                        p.AvailableTypes);
                    };
            }
            else
            {
                Type paramType = param.ParameterType;
                paramNameLabel.Text = String.Format("{0}:", ParseParameterName(param));

                if (paramType.IsEnum)
                {
                    ComboBox combo = new ComboBox();
                    panel.Controls.Add(combo);
                    combo.Location = textBoxStart;
                    combo.Items.AddRange(Enum.GetNames(paramType));
                    combo.SelectedIndex = 0;
                    combo.Width = 240;

                    panel.GetParamFunction =
                        delegate
                        {
                            return Enum.Parse(paramType, combo.SelectedItem.ToString(), true);
                        };
                }
                else if (paramType == typeof(bool))
                {
                    ComboBox combo = new ComboBox();
                    panel.Controls.Add(combo);
                    combo.Location = textBoxStart;
                    combo.Items.AddRange(new String[] { "True", "False" });
                    combo.SelectedIndex = 0;
                    panel.GetParamFunction =
                        delegate ()
                        {
                            return combo.SelectedItem.ToString().Equals("True");
                        };
                }
                else if (paramType == typeof(UInt64) || paramType == typeof(int) || paramType == typeof(double))
                {
                    //Create inpout box for the int paramater
                    TextBox inputTextBox = new TextBox();
                    panel.Controls.Add(inputTextBox);
                    inputTextBox.Location = textBoxStart;
                    inputTextBox.Text = defaultValue == null ? "0" : defaultValue.ToString();

                    panel.GetParamFunction =
                        delegate ()
                        {
                            return Convert.ChangeType(inputTextBox.Text, paramType);
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
                        inputBoxes[i].Location = new Point(textBoxStart.X + i * (boxWidth + 5), textBoxStart.Y);
                        inputBoxes[i].Width = boxWidth;
                        inputBoxes[i].Text = "0.0";
                    }
                    panel.GetParamFunction =
                        delegate ()
                        {
                            double[] values = new double[4];
                            for (int i = 0; i < inputBoxes.Length; i++)
                            {
                                values[i] = Convert.ToDouble(inputBoxes[i].Text);
                            }
                            return new MCvScalar(values[0], values[1], values[2], values[3]);
                        };
                }
                else if (paramType == typeof(PointF))
                {
                    TextBox[] inputBoxes = new TextBox[2];
                    int boxWidth = 40;

                    //Create input boxes for the scalar value
                    for (int i = 0; i < 2; i++)
                    {
                        inputBoxes[i] = new TextBox();
                        panel.Controls.Add(inputBoxes[i]);
                        inputBoxes[i].Location = new Point(textBoxStart.X + i * (boxWidth + 5), textBoxStart.Y);
                        inputBoxes[i].Width = boxWidth;
                        inputBoxes[i].Text = "0.0";
                    }
                    panel.GetParamFunction =
                        delegate ()
                        {
                            float[] values = new float[inputBoxes.Length];
                            for (int i = 0; i < inputBoxes.Length; i++)
                            {
                                values[i] = Convert.ToSingle(inputBoxes[i].Text);
                            }
                            return new PointF(values[0], values[1]);
                        };
                }
                else if (paramType.GetInterface("IColor") == typeof(IColor))
                {
                    IColor t = Activator.CreateInstance(paramType) as IColor;
                    //string[] channelNames = ReflectColorType.GetNamesOfChannels(t);
                    TextBox[] inputBoxes = new TextBox[t.Dimension];
                    int boxWidth = 40;

                    //Create input boxes for the scalar value
                    for (int i = 0; i < inputBoxes.Length; i++)
                    {
                        inputBoxes[i] = new TextBox();
                        panel.Controls.Add(inputBoxes[i]);
                        inputBoxes[i].Location = new Point(textBoxStart.X + i * (boxWidth + 5), textBoxStart.Y);
                        inputBoxes[i].Width = boxWidth;
                        inputBoxes[i].Text = "0.0";
                    }
                    panel.GetParamFunction =
                        delegate ()
                        {
                            double[] values = new double[4];
                            for (int i = 0; i < inputBoxes.Length; i++)
                            {
                                values[i] = Convert.ToDouble(inputBoxes[i].Text);
                            }
                            IColor color = Activator.CreateInstance(paramType) as IColor;
                            color.MCvScalar = new MCvScalar(values[0], values[1], values[2], values[3]);
                            return color;
                        };
                }
                else
                {
                    throw new NotSupportedException(String.Format("Parameter type '{0}' is not supported", paramType.Name));
                }
            }
            return panel;
        }

        /// <summary>
        /// Obtain the parameters for <paramref name="method"/> and put them in <paramref name="defaultParameterValues"/>
        /// </summary>
        /// <param name="method">The method to Obtain parameters from</param>
        /// <param name="defaultParameterValues">The list that will be used as the storage for the retrieved parameters, if it contains elements, those elements will be used as default value</param>
        /// <returns>True if successed, false otherwise</returns>
        public static Object[] GetParams(MethodInfo method, Object[] defaultParameterValues)
        {
            List<ParameterInfo> parameterList = new List<ParameterInfo>();
            List<Object> defaultParameterValueList = new List<object>();

            #region find all the generic types and options and add that to the lists.
            if (method.ContainsGenericParameters)
            {
                ExposableMethodAttribute att =
                   (method.GetCustomAttributes(typeof(ExposableMethodAttribute), false)[0] as ExposableMethodAttribute);
                Type[] typeOptions = att.GenericParametersOptions;
                int[] optionSize = att.GenericParametersOptionSizes;

                GenericParameter[] genericOptions = new GenericParameter[optionSize.Length];
                int count = 0;
                for (int i = 0; i < optionSize.Length; i++)
                {
                    Type[] types = new Type[optionSize[i]];
                    for (int j = 0; j < types.Length; j++)
                        types[j] = typeOptions[count++];
                    genericOptions[i] = new GenericParameter(types[0], types);
                }

                //Type[] instanceGenericParameters = method.ReflectedType.GetGenericArguments();
                Type[] genericTypes = method.GetGenericArguments();

                for (int i = 0; i < genericTypes.Length; i++)
                {
                    parameterList.Add(null);
                    defaultParameterValueList.Add(
                       defaultParameterValues == null ?
                       genericOptions[i] :
                       defaultParameterValues[i]);
                }
            }
            #endregion

            parameterList.AddRange(method.GetParameters());

            if (defaultParameterValues != null)
                defaultParameterValueList.AddRange(defaultParameterValues);

            //if the method requires no parameter, simply return an empty array
            if (parameterList.Count == 0)
                return new object[0];

            #region Handle the cases where at least one parameter is required as input
            using (ParameterInputDialog dlg = new ParameterInputDialog(parameterList.ToArray(), defaultParameterValueList.ToArray()))
            {
                dlg.ShowDialog();
                return dlg.Successed ? dlg.Parameters : null;
            }
            #endregion
        }
    }
}
