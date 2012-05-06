using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class DialogAdapter : BaseAdapter<Section>
	{
		const int TYPE_SECTION_HEADER = 0;

		Context context;
		LayoutInflater inflater;

		public DialogAdapter(Context context, RootElement root)
		{
			this.context = context;
			this.inflater = LayoutInflater.From(context);
			this.Root = root;			
		}

		public RootElement Root
		{
			get;
			set;
		}

		public override bool IsEnabled(int position)
		{
			// start counting from here
			int typeOffset = TYPE_SECTION_HEADER + 1;

			foreach (var s in Root.Sections)
			{
				if (position == 0)
					return false;

				int size = s.Adapter.Count + 1;

				if (position < size)
					return true;

				position -= size;
				typeOffset += s.Adapter.ViewTypeCount;
			}

			return false;
		}

		public override int Count
		{
			get
			{
				int count = 0;

				//Get each adapter's count + 1 for the header
				foreach (var s in Root.Sections)
					count += s.Adapter.Count + 1;

				return count;
			}
		}

		public override int ViewTypeCount
		{
			get
			{
				//The headers count as a view type too
				int viewTypeCount = 1;

				//Get each adapter's ViewTypeCount
				foreach (var s in Root.Sections)
					viewTypeCount += s.Adapter.ViewTypeCount;

				return viewTypeCount;
			}
		}

		public override Section this[int position]
		{
			get { return this.Root.Sections[position]; }
		}

		public override bool AreAllItemsEnabled()
		{
			return false;
		}

		public override int GetItemViewType(int position)
		{
			// start counting from here
			int typeOffset = TYPE_SECTION_HEADER + 1;

			foreach (var s in Root.Sections)
			{
				if (position == 0)
					return (TYPE_SECTION_HEADER);

				int size = s.Adapter.Count + 1;

				if (position < size)
					return (typeOffset + s.Adapter.GetItemViewType(position - 1));

				position -= size;
				typeOffset += s.Adapter.ViewTypeCount;
			}

			return -1;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			int sectionIndex = 0;

			foreach (var s in Root.Sections)
			{
				if (s.Adapter.Context == null)
					s.Adapter.Context = this.context;

				if (position == 0)
					return s.GetView(context, convertView, parent);

				int size = s.Adapter.Count + 1;

				if (position < size)
					return (s.Adapter.GetView(position - 1, convertView, parent));

				position -= size;
				sectionIndex++;
			}

			return null;
		}
	}
}