using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;

namespace Lucian
{
    class W
    {
        static void onStartCasting(Champion owner) { }
        static void onFinishCasting(Champion owner, Spell spell)
        {
            Vector2 current = new Vector2(owner.X, owner.Y);
            Vector2 to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            Vector2 range = to * 445;
            Vector2 trueCoords = current + range;

            dashToLocation(owner, trueCoords.x, trueCoords.y, 1500, false, "SPELL3");
        }
        static void applyEffects(Champion owner, Spell spell, Projectile projectile){ }
        static void onUpdate(double diff) { }
    }
}
