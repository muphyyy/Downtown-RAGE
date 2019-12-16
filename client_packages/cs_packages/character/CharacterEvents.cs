using RAGE;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP_cs.character
{
    public class CharacterEvents : Events.Script
    {
        public CharacterEvents()
        {
            Events.Add("HideChat", HideChat);
            Events.Add("ShowChat", ShowChat);
        }

        private void HideChat(object[] args)
        {
            Chat.Show(false);
        }

        private void ShowChat(object[] args)
        {
            Chat.Show(true);
        }
    }
}
