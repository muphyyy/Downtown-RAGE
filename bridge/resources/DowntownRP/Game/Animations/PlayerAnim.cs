using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Game.Animations
{
    enum AnimationFlags
    {
        Loop = 1 << 0,
        StopOnLastFrame = 1 << 1,
        OnlyAnimateUpperBody = 1 << 4,
        AllowPlayerControl = 1 << 5,
        Cancellable = 1 << 7
    }

    public class PlayerAnim
    {
        public string Name;
        public string AnimDict;
        public string AnimName;

        public PlayerAnim(string name, string anim_dict, string anim_name)
        {
            Name = name;
            AnimDict = anim_dict;
            AnimName = anim_name;
        }
    }
}
