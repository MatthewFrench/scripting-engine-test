using LeagueSandbox.GameServer.Logic.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripting_Engine.LeagueSandbox_Default.Champions.Ezreal
{
    class Passive
    {
        static void onUpdate(double diff) { }
        static void onDamageTaken(Unit attacker, float damage, DamageType type, DamageSource source) { }
        static void onAutoAttack(Unit target) { }
        static void onDealDamage(Unit target, float damage, DamageType damageType, DamageSource source) { }
        static void onSpellCast(float x, float y, Spell slot, Unit target) { }
        static void onDie(Unit killer) { }
    }
}
