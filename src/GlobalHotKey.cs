using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NetworkSwitcher
{
    public class GlobalHotkey
    {
        private readonly int modifier;
        private readonly int key;
        private IntPtr hWnd;
        private readonly int id;

        public int Id
        {
            get { return this.id; }
        }

        public GlobalHotkey(int modifier, Keys key, Form form, int id)
        {
            this.modifier = modifier;
            this.key = (int)key;
            this.hWnd = form.Handle;
            this.id = id;
        }

        public bool Register()
        {
            return RegisterHotKey(hWnd, id, modifier, key);
        }

        public bool Unregiser()
        {
            return UnregisterHotKey(hWnd, id);
        }

        public override int GetHashCode()
        {
            return modifier ^ key ^ hWnd.ToInt32();
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
