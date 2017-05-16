using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace MRPlatform.HMI
{
    public class MenuItems
    {
        private Dictionary<object, object> _items = new Dictionary<object, object>();

        public void Add(ref object Item, [Optional]ref object Key, [Optional]ref object Before, [Optional]ref object After)
        {
            _items.Add(Key, Item);
        }

        public int Count()
        {
            return _items.Count;
        }

        public IEnumerator<MenuItem> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        public dynamic Item(ref object Index)
        {
            return _items[Index];
        }

        public void Remove(ref object Index)
        {
            _items.Remove(Index);
        }
    }
}
