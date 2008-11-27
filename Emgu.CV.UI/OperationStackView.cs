using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Emgu.Util;
using Emgu.Util.TypeEnum;

namespace Emgu.CV.UI
{
   /// <summary>
   /// A user control to display the operation stack
   /// </summary>
   public partial class OperationStackView : UserControl
   {
      private DataGridViewTextBoxColumn _codeColumn;
      private ProgrammingLanguage _language;

      /// <summary>
      /// Constructor
      /// </summary>
      public OperationStackView()
      {
         InitializeComponent();
         _codeColumn = new DataGridViewTextBoxColumn();
         _codeColumn.Name = "code";
         _codeColumn.ReadOnly = true;
         _codeColumn.Width = 300;
         dataGridView1.Columns.Add(_codeColumn);
         dataGridView1.RowHeadersVisible = false;
         _language = ProgrammingLanguage.CSharp;
      }

      /// <summary>
      /// Set the programming language for this Operation Stack View
      /// </summary>
      public ProgrammingLanguage Language
      {
         set
         {
            _language = value;
         }
      }

      /// <summary>
      /// Display the operation stack
      /// </summary>
      /// <param name="operationStack">The operation stack to be displayed</param>
      public void DisplayOperationStack(Stack<Operation> operationStack)
      {
         dataGridView1.Rows.Clear();
         String[] codes = GetOperationCode(operationStack);
         if (codes.Length > 0)
         {
            dataGridView1.Rows.Add(codes.Length);
            for (int i = 0; i < codes.Length; i++)
               dataGridView1.Rows[i].Cells[_codeColumn.Name].Value = codes[i];
         }
      }

      /*
      private String ImageTypeToString(Type imageType)
      {
         Type[] genericParameterTypes = imageType.GetGenericArguments();
         String genericParamString = String.Join(",",
            Array.ConvertAll<Type, String>(genericParameterTypes,
               delegate(Type t) { return t.Name; }));

         if (_language == ProgrammingLanguage.CSharp)
         {
            return String.Format("{0}<{1}>", imageType.Name, genericParamString);
         }
         else if (_language == ProgrammingLanguage.CPlusPlus)
         {
            return String.Format("{0}<{1}>^", imageType.Name, genericParamString);
         }
         else
         {
            throw new NotImplementedException("Code generation for this programming language is not implemented");
         }
      }*/

      private String[] GetOperationCode(Stack<Operation> operationStack)
      {
         List<String> ops = new List<string>();

         Operation[] operationArray = operationStack.ToArray();
         Array.Reverse(operationArray);

         int currentInstanceIndex = 0;

         if (_language == ProgrammingLanguage.CSharp)
         {
            ops.Add("public static IImage Function(IImage image" + currentInstanceIndex + "){");
         }
         else if (_language == ProgrammingLanguage.CPlusPlus)
         {
            ops.Add("public static IImage^ Function(IImage^ image" + currentInstanceIndex + "){");
         }

         foreach (Operation op in operationArray)
         {
            String str = op.ToCode(_language).Replace("{instance}", "image" + currentInstanceIndex);

            if (_language == ProgrammingLanguage.CSharp || _language == ProgrammingLanguage.CPlusPlus)
            {
               if (op.Method.ReturnType == typeof(void))
               {
                  str = String.Format("{0};", str);
               }
               else
               {
                  currentInstanceIndex++;
                  str = String.Format("image{0} = {1};",
                     //ImageTypeToString(op.Method.ReturnType), 
                     currentInstanceIndex, str);
               }
            }
            ops.Add(str);
         }
         ops.Add(String.Format("return image{0};", currentInstanceIndex));

         if (_language == ProgrammingLanguage.CSharp)
         {
            ops.Add("}");
         }
         else if (_language == ProgrammingLanguage.CPlusPlus)
         {
            ops.Add("}");
         }
         return ops.ToArray();
      }
   }
}
