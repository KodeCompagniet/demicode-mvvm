using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;
using System.ComponentModel;

namespace DemiCode.Hours.Win.Resources
{
    [ContentProperty("Items"), DefaultProperty("Items")]
    public class MenuItem : IEnumerable<MenuItem>
    {
        public MenuItem()
        {
            Items = new List<MenuItem>();
        }
        public string Header { get; set; }
        public String Action { get; set; }
        public Type ViewType { get; set; }
        public List<MenuItem> Items { get; private set; }

        public IEnumerator<MenuItem> GetEnumerator()
        {
            return ((IEnumerable<MenuItem>)Items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Items).GetEnumerator();
        }
    }
}
