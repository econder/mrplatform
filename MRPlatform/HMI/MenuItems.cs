using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("646270A3-919F-450B-8728-C73710789262"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuItemsEvent))]
    public class MenuItems : IMenuItems
    {
        //private MenuItem[] _menuItems;
        private Dictionary<int, MenuItem> _items = new Dictionary<int, MenuItem>();
        private int _position = -1;

        public MenuItems()
        {

        }

        public void Add(MenuItem item)
        {
            _items.Add(_position++, item);
        }

        public void Remove(int index)
        {
            _items.Remove(index);
        }

        public bool MoveNext()
        {
            if (_position + 1 >= Count)
            {
                return false;
            }

            return true;
        }

        public void Reset()
        {
            _position = -1;
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public object Current
        {
            get
            {
                if(_position < 0)
                {
                    throw new InvalidOperationException();
                }
                else if(_position > Count)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    return this[_position];
                }
            }
        }

        public MenuItem this[int index]
        {
            get
            {
                return _items[index];
            }
        }

        [DispId(-4)]
        public IEnumerator GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }
    }
}
