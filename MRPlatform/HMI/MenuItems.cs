using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace MRPlatform.HMI
{
    public class MenuItems : IMenuItem, IEnumerable
    {
        private MenuItem[] _menuItems;


        public MenuItem this[int index]
        {
            get
            {
                return _menuItems[index];
            }
        }

        [DispId(-4)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public MenuItemsEnumerator GetEnumerator()
        {
            return new MenuItemsEnumerator(_menuItems);
        }
    }


    public class MenuItemsEnumerator : IEnumerator
    {
        public MenuItem[] _menuItems;
        int position = -1;

        public MenuItemsEnumerator(MenuItem[] list)
        {
            _menuItems = list;
        }

        public bool MoveNext()
        {
            position++;
            return position < _menuItems.Length;
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
}
