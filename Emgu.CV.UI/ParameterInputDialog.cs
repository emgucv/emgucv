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
   public partial class ParameterInputDialog : Form
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
         for (int i = 0; i < _paramInfo.Length; i++)
         {
            ParamInputPanel panel = _paramPanel[i];
            Object value = panel.GetValue();
            if (value == null)
            {
               MessageBox.Show("Parameter {0} is invalid.", panel.Name);
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
            catch (Exception)
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
      /// <param name="defaultValue">The default value for the parameter</param>
      /// <returns>the panel</returns>
      private static ParamInputPanel CreatePanelForParameter(ParameterInfo param, object defaultValue)
      {
         ParamInputPanel panel = new ParamInputPanel();
         panel.Height = 50;
         panel.Width = 400;
         int textBoxStartX = 100, textBoxStartY = 10;

         #region add the label for the parameter
         Label paramNameLabel = new Label();
         paramNameLabel.AutoSize = true;
         panel.Controls.Add(paramNameLabel);
         paramNameLabel.Location = new System.Drawing.Point(10, textBoxStartY);
         #endregion

         if (param == null)
         {  // a generic parameter
            String defaultString = (String)defaultValue;
            String[] splitTypeName = defaultString.Split('|');
            paramNameLabel.Text = String.Format("{0}:", splitTypeName[0]);
            String[] splitDefaultValue = splitTypeName[1].Split(':');

            String[] options = splitDefaultValue[1].Split(',');
            ComboBox combo = new ComboBox();
            panel.Controls.Add(combo);
            combo.Location = new System.Drawing.Point(textBoxStartX, textBoxStartY);
            combo.Items.AddRange(options);
            combo.SelectedIndex = Array.FindIndex<String>(options, splitDefaultValue[0].Equals);
            panel.GetParamFunction =
                delegate()
                {
                   return String.Format("{0}|{1}:{2}", 
                      splitTypeName[0], 
                      combo.Text, 
                      splitDefaultValue[1]);
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
               inputTextBox.Text = defaultValue == null ? "0" : defaultValue.ToString();

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
            else if (paramType.IsSubclassOf(typeof(ColorType)))
            {
               ColorType t = Activator.CreateInstance(paramType) as ColorType;
               string[] channelNames = t.ChannelNames;
               TextBox[] inputBoxes = new TextBox[channelNames.Length];
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
                      ColorType color = Activator.CreateInstance(paramType) as ColorType;
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
      /// Obtain the parameters for <paramref name="method"/> and put them in <paramref name="paramList"/>
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
            Type[] genericTypes = method.GetGenericArguments();
            ExposableMethodAttribute methodAtt = method.GetCustomAttributes(typeof(ExposableMethodAttribute), false)[0] as ExposableMethodAttribute;
            String[] genericOptions = methodAtt.GenericParametersOptions.Split(';');

            Type[] instanceGenericParameters = method.ReflectedType.GetGenericArguments();
            for (int i = 0; i < genericOptions.Length; i++)
            {
               String option = genericOptions[i];
               if (option.Substring(0, 1).Equals(":"))
                  option = instanceGenericParameters[i].FullName + option;
               genericOptions[i] = option;
            }

            for (int i = 0; i < genericTypes.Length; i++)
            {
               parameterList.Add(null);
               Object defaultParameterValue = defaultParameterValues == null
                  ? String.Format("{0}|{1}", genericTypes[i].Name, genericOptions[i]) :
                  defaultParameterValues[i];

               defaultParameterValueList.Add(defaultParameterValue);
            }
         }
         #endregion

         parameterList.AddRange(method.GetParameters());

         if (defaultParameterValues != null)
         {
            defaultParameterValueList.AddRange(defaultParameterValues);
         }

         //if the method requires no parameter, simply return true
         if (parameterList.Count == 0)
         {
            return new object[0];
         }

         #region Handle the cases where at least one parameter is required as input
         ParameterInputDialog dlg = new ParameterInputDialog(parameterList.ToArray(), defaultParameterValueList.ToArray());
         dlg.ShowDialog();
         return dlg.Successed ? dlg.Parameters : null;
         #endregion
      }
   }
}
