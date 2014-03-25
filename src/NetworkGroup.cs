using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetworkSwitcher
{
    public class NetworkGroup
    {
        public string Name { get; set; }
        public string Hotkey { get; set; }
        public Interface[] Interfaces { get; set; }

        public NetworkGroup()
        {
            this.Interfaces = new Interface[0];
        }

        public Hotkey GetHotkey()
        {
            var parts = this.Hotkey.Split(new[] { "+" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            
            var modifiers = new List<Modifiers>();
            var keys = new List<Keys>();
            
            foreach (var part in parts)
            {
                Modifiers modifierValue;
                Keys keyValue;

                if (Enum.TryParse(part, true, out modifierValue))
                {
                    modifiers.Add(modifierValue);
                }
                else if (Enum.TryParse(part, true, out keyValue))
                {
                    keys.Add(keyValue);
                }
            }

            return new Hotkey { Modifiers = modifiers.Cast<int>().Aggregate((x, y) => x | y), Key = keys.First() };
        }
    }
}
