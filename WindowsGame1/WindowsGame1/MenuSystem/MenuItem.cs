using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.MenuSystem
{
    class MenuItem
    {
        public string Name { get; set; }
        public bool Active { get; set; }

        public event EventHandler Click;

        public MenuItem(string name)
        {
            this.Name = name;
            Active = true;
        }

        public void OnClick()
        {
            if (Click != null)
                Click(this, null);
        }
    }
}
