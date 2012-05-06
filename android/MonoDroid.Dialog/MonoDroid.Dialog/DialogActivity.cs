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
	public class DialogActivity : ListActivity
	{
		public event Action<Section, Element> ElementClick;
		public event Action<Section, Element> ElementLongClick;

		public DialogActivity(RootElement root)
			: base()
		{
			this.Root = root;
			this.DialogAdapter = new DialogAdapter(this, root);
		}

		public RootElement Root
		{
			get;
			set;
		}

		public DialogAdapter DialogAdapter
		{
			get;
			private set;
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			this.ListAdapter = this.DialogAdapter;
			this.ListView.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(ListView_ItemClick);
			this.ListView.ItemLongClick += new EventHandler<AdapterView.ItemLongClickEventArgs>(ListView_ItemLongClick);
		}

		void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
		{
			var elem = this.DialogAdapter[e.Position] as Element;

			if (elem != null && elem.LongClick != null)
				elem.LongClick();
		}

		void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var elem = this.DialogAdapter[e.Position] as Element;

			if (elem != null && elem.Click != null)
				elem.Click();
		}

	}
}