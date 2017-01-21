using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;

namespace Annie
{
    class Q
    {
        static void onStartCasting() { }
        static void onFinishCasting(Champion owner, Spell spell, Unit castTarget)
        {
            float AP = owner.GetStats().AbilityPower.Total * 0.8f;
            float damage = 45 + (spell.Level * 35) + AP;

            if(castTarget != null && !ApiFunctionManager.IsDead(castTarget))
            {

            }
        }
    }
}
