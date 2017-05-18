using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("51FA2D6C-1DEC-4038-A777-49BD4B27D885"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuItems))]
    public class MenuItems : IMenuItems
    {
        private SortedList _items;

        
        public MenuItems()
        {
            _items = new SortedList();
        }

        public void Add(int key, MenuItem item)
        {
            _items.Add(key, item);
        }

        public void Remove(int key)
        {
            _items.Remove(key);
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public object this[int key]
        {
            get
            {
                return _items[key];
            }
            set
            {
                _items[key] = value;
            }
        }


        [DispId(-4)]
        public IEnumerator GetEnumerator()
        {
            ICollection keys = _items.Keys;
            return (IEnumerator)keys.GetEnumerator();
        }
    }
}
