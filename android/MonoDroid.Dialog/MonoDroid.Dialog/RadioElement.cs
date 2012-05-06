using System;
using Android.Content;
using Android.Views;

namespace MonoDroid.Dialog
{
    public class RadioElement : StringElement
    {
        public string Group;
        internal int RadioIdx;

        public RadioElement(string caption, string group)
            : base(caption)
        {
            Group = group;
        }

        public RadioElement(string caption)
            : base(caption)
        {
        }

		public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            var cell = base.GetView(context, convertView, parent);
            var root = (RootElement)Parent.Parent;

            if (!(root.group is RadioGroup))
                throw new Exception("The RootElement's Group is null or is not a RadioGroup");

            bool selected = RadioIdx == ((RadioGroup)(root.group)).Selected;
            //cell.Accessory = selected ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

            return cell;
        }

		//public override void Selected()
		//{
		//    RootElement root = (RootElement)Parent.Parent;
		//    if (RadioIdx != root.RadioSelected)
		//    {
		//        //var cell = tableView.CellAt(root.PathForRadio(root.RadioSelected));
		//        //if (cell != null)
		//        //    cell.Accessory = UITableViewCellAccessory.None;
		//        //cell = tableView.CellAt(indexPath);
		//        //if (cell != null)
		//        //    cell.Accessory = UITableViewCellAccessory.Checkmark;
		//        root.RadioSelected = RadioIdx;
		//    }
		//    base.Selected();
		//}
    }
}