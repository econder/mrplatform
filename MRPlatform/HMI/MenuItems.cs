using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("646270A3-919F-450B-8728-C73710789262"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuEvents))]
    public class MenuItems : IMenuItem
    {
        //private MenuItem[] _menuItems;
        public Dictionary<int, MenuItem> Items;

        public MenuItems()
        {
            
        }

        public void Add(MenuItem item)
        {
            Items.Add(Items.Count + 1, item);
        }

        public void Remove(int index)
        {
            Items.Remove(index);
        }

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public MenuItem this[int index]
        {
            get
            {
                return Items[index];
            }
        }

        /*
        [DispId(-4)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public MenuItemsEnumerator GetEnumerator()
        {
            return new MenuItemsEnumerator(Items);
        }
        */
    }


    /*
    public class MenuItemsEnumerator : IEnumerator
    {
        //public MenuItem[] _menuItems;
        private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();
        int position = -1;

        public MenuItemsEnumerator(Dictionary<int, MenuItem> list)
        {
            _menuItems = list;
        }

        public bool MoveNext()
        {
            position++;
            return position < _menuItems.Count;
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public MenuItem Current
        {
            get
            {
                try
                {
                    return _menuItems[position];
                }
                catch
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
    */
}
