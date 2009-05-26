using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Emgu.Util.TypeEnum;

namespace Emgu.CV.UI
{
   /// <summary>
   /// A user control to display the operations
   /// </summary>
   internal partial class OperationsView : UserControl
   {
      private DataGridViewTextBoxColumn _codeColumn;
      private ProgrammingLanguage _language;

      /// <summary>
      /// Constructor
      /// </summary>
      public OperationsView()
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
      /// Set the programming language for this Operation View
      /// </summary>
      public ProgrammingLanguage Language
      {
         set
         {
            _language = value;
         }
      }

      /// <summary>
      /// Display the operations
      /// </summary>
      /// <param name="operations">The operations to be displayed</param>
      public void DisplayOperations(List<Operation> operations)
      {
         dataGridView1.Rows.Clear();
         String[] codes = GetOperationCode(operations);
         if (codes.Length > 0)
         {
            dataGridView1.Rows.Add(codes.Length);
            for (int i = 0; i < codes.Length; i++)
               dataGridView1.Rows[i].Cells[_codeColumn.Name].Value = codes[i];
         }
      }

      private String ImageTypeToString(Type imageType)
      {
         if (imageType == typeof(IImage))
         {
            switch (_language)
            {
               case ProgrammingLanguage.CSharp:
                  return "IImage";
               case ProgrammingLanguage.CPlusPlus:
                  return "IImage^";
            }
         }

         Type[] genericParameterTypes = imageType.GetGenericArguments();
         String genericParamString = String.Join(",",
            Array.ConvertAll<Type, String>(genericParameterTypes,
               delegate(Type t) { return t.Name; }));

         switch (_language)
         {
            case ProgrammingLanguage.CSharp:
               return String.Format("Image<{0}>", genericParamString);
            case ProgrammingLanguage.CPlusPlus:
               return String.Format("Image<{0}>^", genericParamString);
            default:
               throw new NotImplementedException("Code generation for this programming language is not implemented");
         }
      }

      private String[] GetOperationCode(List<Operation> operationList)
      {
         Type imageType;
         if (operationList.Count == 0) 
            return new string[0];
         else
            imageType = operationList[0].Method.DeclaringType;

         List<String> ops = new List<string>();

         int currentInstanceIndex = 0;

         switch (_language)
         {
            case ProgrammingLanguage.CSharp:
               ops.Add(
                  String.Format("public static IImage Function({0} image{1}){{", ImageTypeToString(imageType), currentInstanceIndex));
               break;
            case ProgrammingLanguage.CPlusPlus:
               ops.Add(
                  String.Format("public static IImage Function({0} image{1}){{", ImageTypeToString(imageType), currentInstanceIndex));
               break;
         }
         
         foreach (Operation op in operationList)
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
                  imageType = op.Method.ReturnType;
                  currentInstanceIndex++;
                  str = String.Format("{0} image{1} = {2};",
                     ImageTypeToString(op.Method.ReturnType), 
                     currentInstanceIndex, 
                     str);
               }
            }
            ops.Add(str);
         }
         ops.Add(String.Format("return image{0};", currentInstanceIndex));

         switch (_language)
         {
            case ProgrammingLanguage.CSharp:
            case ProgrammingLanguage.CPlusPlus:
               ops.Add("}");
               break;
         }

         ops[0] = ops[0].Replace("IImage", ImageTypeToString(imageType));
         return ops.ToArray();
      }
   }
}
