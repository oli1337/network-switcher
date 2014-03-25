using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NetworkSwitcher
{
    public partial class HiddenForm : Form
    {
        private readonly List<GlobalHotkey> _hotkeys = new List<GlobalHotkey>();
        private readonly NetworkGroupManager manager;

        public HiddenForm()
        {
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;

            this.InitializeComponent();
            this.Load += OnLoad;

            manager = new NetworkGroupManager();
            manager.LoadConfiguration();

            foreach (var networkGroup in manager.NetworkGroups)
            {
                var hotkey = networkGroup.GetHotkey();
                _hotkeys.Add(new GlobalHotkey(hotkey.Modifiers, hotkey.Key, this, manager.NetworkGroups.IndexOf(networkGroup)));
            }
        }

        private void OnLoad(object sender, EventArgs args)
        {
            this._hotkeys.ForEach(e => e.Register());
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
            {
                HandleHotkey(m.WParam.ToInt32());
            }

            base.WndProc(ref m);
        }

        private void HandleHotkey(int id)
        {
            var networkGroup = this.manager.NetworkGroups[id];

            Network.SetActiveNetworkGroup(networkGroup);
        }
    }
}
