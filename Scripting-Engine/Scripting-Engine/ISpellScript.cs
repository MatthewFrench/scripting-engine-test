using LeagueSandbox.GameServer.Logic.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripting_Engine
{
    public interface ISpellScript
    {
         void onStartCasting(Champion owner);
         void onFinishCasting(Champion owner, Spell spell);
         void applyEffects(Champion owner, Unit target, Spell spell, Projectile projectile);
         void onUpdate(double diff);
    }
}
