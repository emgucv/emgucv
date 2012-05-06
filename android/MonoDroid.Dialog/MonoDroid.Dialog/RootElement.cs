using System;
using System.Collections;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class RootElement : Element, IEnumerable
    {
       // private static string rkey = "RootElement";
        int summarySection, summaryElement;
        internal Group group;
        public bool UnevenRows;
        public Func<RootElement, View> createOnSelected;

        /// <summary>
        ///  Initializes a RootSection with a caption
        /// </summary>
        /// <param name="caption">
        ///  The caption to render.
        /// </param>
        public RootElement(string caption)
            : base(caption)
        {
            summarySection = -1;
            Sections = new List<Section>();
        }

        /// <summary>
        /// Initializes a RootSection with a caption and a callback that will
        /// create the nested UIViewController that is activated when the user
        /// taps on the element.
        /// </summary>
        /// <param name="caption">
        ///  The caption to render.
        /// </param>
        public RootElement(string caption, Func<RootElement, View> createOnSelected)
            : base(caption)
        {
            summarySection = -1;
            this.createOnSelected = createOnSelected;
            Sections = new List<Section>();
        }

        /// <summary>
        ///   Initializes a RootElement with a caption with a summary fetched from the specified section and leement
        /// </summary>
        /// <param name="caption">
        /// The caption to render cref="System.String"/>
        /// </param>
        /// <param name="section">
        /// The section that contains the element with the summary.
        /// </param>
        /// <param name="element">
        /// The element index inside the section that contains the summary for this RootSection.
        /// </param>
        public RootElement(string caption, int section, int element)
            : base(caption)
        {
            summarySection = section;
            summaryElement = element;
        }

        /// <summary>
        /// Initializes a RootElement that renders the summary based on the radio settings of the contained elements. 
        /// </summary>
        /// <param name="caption">
        /// The caption to ender
        /// </param>
        /// <param name="group">
        /// The group that contains the checkbox or radio information.  This is used to display
        /// the summary information when a RootElement is rendered inside a section.
        /// </param>
        public RootElement(string caption, Group group)
            : base(caption)
        {
            this.group = group;
        }

        internal List<Section> Sections = new List<Section>();

        //internal NSIndexPath PathForRadio(int idx)
        //{
        //    RadioGroup radio = group as RadioGroup;
        //    if (radio == null)
        //        return null;

        //    uint current = 0, section = 0;
        //    foreach (Section s in Sections)
        //    {
        //        uint row = 0;

        //        foreach (Element e in s.Elements)
        //        {
        //            if (!(e is RadioElement))
        //                continue;

        //            if (current == idx)
        //            {
        //                return NSIndexPath.Create(section, row);
        //            }
        //            row++;
        //            current++;
        //        }
        //        section++;
        //    }
        //    return null;
        //}

        public int Count
        {
            get
            {
                return Sections.Count;
            }
        }

        public Section this[int idx]
        {
            get
            {
                return Sections[idx];
            }
        }

        internal int IndexOf(Section target)
        {
            int idx = 0;
            foreach (Section s in Sections)
            {
                if (s == target)
                    return idx;
                idx++;
            }
            return -1;
        }

        internal void Prepare()
        {
            int current = 0;
            foreach (Section s in Sections)
            {
                foreach (Element e in s.Elements)
                {
                    var re = e as RadioElement;
                    if (re != null)
                        re.RadioIdx = current++;
                    if (UnevenRows == false && e is IElementSizing)
                        UnevenRows = true;
                }
            }
        }

        /// <summary>
        /// Adds a new section to this RootElement
        /// </summary>
        /// <param name="section">
        /// The section to add, if the root is visible, the section is inserted with no animation
        /// </param>
        public void Add(Section section)
        {
            if (section == null)
                return;

            Sections.Add(section);
            section.Parent = this;
        }

        //
        // This makes things LINQ friendly;  You can now create RootElements
        // with an embedded LINQ expression, like this:
        // new RootElement ("Title") {
        //     from x in names
        //         select new Section (x) { new StringElement ("Sample") }
        //
        public void Add(IEnumerable<Section> sections)
        {
            foreach (var s in sections)
                Add(s);
        }

        //NSIndexSet MakeIndexSet(int start, int count)
        //{
        //    NSRange range;
        //    range.Location = start;
        //    range.Length = count;
        //    return NSIndexSet.FromNSRange(range);
        //}

        /// <summary>
        /// Inserts a new section into the RootElement
        /// </summary>
        /// <param name="idx">
        /// The index where the section is added <see cref="System.Int32"/>
        /// </param>
        /// <param name="newSections">
        /// A <see cref="Section[]"/> list of sections to insert
        /// </param>
        /// <remarks>
        ///    This inserts the specified list of sections (a params argument) into the
        ///    root using the specified animation.
        /// </remarks>
        public void Insert(int idx, params Section[] newSections)
        {
            if (idx < 0 || idx > Sections.Count)
                return;
            if (newSections == null)
                return;

            //if (Table != null)
            //    Table.BeginUpdates();

            int pos = idx;
            foreach (var s in newSections)
            {
                s.Parent = this;
                Sections.Insert(pos++, s);
            }
        }

        /// <summary>
        /// Removes a section at a specified location
        /// </summary>
        public void RemoveAt(int idx)
        {
            if (idx < 0 || idx >= Sections.Count)
                return;

            Sections.RemoveAt(idx);
        }

        public void Remove(Section s)
        {
            if (s == null)
                return;
            int idx = Sections.IndexOf(s);
            if (idx == -1)
                return;
            RemoveAt(idx);
        }

        public void Clear()
        {
            foreach (var s in Sections)
                s.Dispose();
            Sections = new List<Section>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Sections == null)
                    return;
                Clear();
                Sections = null;
            }
        }

        /// <summary>
        /// Enumerator that returns all the sections in the RootElement.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator"/>
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

        /// <summary>
        /// The currently selected Radio item in the whole Root.
        /// </summary>
        public int RadioSelected
        {
            get
            {
                var radio = group as RadioGroup;
                if (radio != null)
                    return radio.Selected;
                return -1;
            }
            set
            {
                var radio = group as RadioGroup;
                if (radio != null)
                    radio.Selected = value;
            }
        }

		public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            var cell = new TextView(context) {TextSize = 16f, Text = Caption};

            var radio = group as RadioGroup;
            if (radio != null)
            {
                int selected = radio.Selected;
                int current = 0;

                foreach (var s in Sections)
                {
                    foreach (var e in s.Elements)
                    {
                        if (!(e is RadioElement))
                            continue;

                        if (current == selected)
                        {
                            cell.Text = e.Summary();
                            goto le;
                        }
                        current++;
                    }
                }
            }
            else if (group != null)
            {
                int count = 0;

                foreach (var s in Sections)
                {
                    foreach (var e in s.Elements)
                    {
                        var ce = e as CheckboxElement;
                        if (ce != null)
                        {
                            if (ce.Value)
                                count++;
                            continue;
                        }
                        var be = e as BoolElement;
                        if (be != null)
                        {
                            if (be.Value)
                                count++;
                            continue;
                        }
                    }
                }
                //cell.DetailTextLabel.Text = count.ToString();
            }
            else if (summarySection != -1 && summarySection < Sections.Count)
            {
                var s = Sections[summarySection];
                //if (summaryElement < s.Elements.Count)
                //    cell.DetailTextLabel.Text = s.Elements[summaryElement].Summary();
            }
            le:
            //cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            return cell;
        }

		///// <summary>
		/////    This method does nothing by default, but gives a chance to subclasses to
		/////    customize the UIViewController before it is presented
		///// </summary>
		//protected virtual void PrepareDialogViewController(View dvc)
		//{
		//}

		///// <summary>
		///// Creates the UIViewController that will be pushed by this RootElement
		///// </summary>
		//protected virtual View MakeViewController(Context context)
		//{
		//    if (createOnSelected != null)
		//        return createOnSelected(this);

		//    return new DialogView(context, this);
		//}

        //public override void Selected()
       // {
        //    //tableView.DeselectRow(path, false);
       //     //var newDvc = MakeViewController();
       //     //PrepareDialogViewController(newDvc);
       //     //dvc.ActivateController(newDvc);
       //     base.Selected();
       // }

        //public void Reload(Section section, UITableViewRowAnimation animation)
        //{
        //    if (section == null)
        //        throw new ArgumentNullException("section");
        //    if (section.Parent == null || section.Parent != this)
        //        throw new ArgumentException("Section is not attached to this root");

        //    int idx = 0;
        //    foreach (var sect in Sections)
        //    {
        //        if (sect == section)
        //        {
        //            Table.ReloadSections(new NSIndexSet((uint)idx), animation);
        //            return;
        //        }
        //        idx++;
        //    }
        //}

        //public void Reload(Element element, UITableViewRowAnimation animation)
        //{
        //    if (element == null)
        //        throw new ArgumentNullException("element");
        //    var section = element.Parent as Section;
        //    if (section == null)
        //        throw new ArgumentException("Element is not attached to this root");
        //    var root = section.Parent as RootElement;
        //    if (root == null)
        //        throw new ArgumentException("Element is not attached to this root");
        //    var path = element.IndexPath;
        //    if (path == null)
        //        return;
        //    Table.ReloadRows(new NSIndexPath[] { path }, animation);
        //}

    }
}