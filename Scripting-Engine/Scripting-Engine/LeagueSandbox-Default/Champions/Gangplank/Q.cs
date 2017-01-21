using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;
namespace Gangplank
{
    class Q
    {
        static void onStartCasting() { }
        static void onFinishCasting(Unit castTarget, Spell spell)
        {
            Unit target = castTarget;
            spell.AddProjectileTarget("pirate_parley_mis", target, false);
        }
        static void applyEffects(Champion owner, Spell spell, Unit castTarget, Projectile projectile)
        {
            Unit target = castTarget;
            float damage = owner.GetStats().AttackDamage.Total + -5 + (25 * spell.Level);
            float newGold = owner.GetStats().Gold + 3 + (1 * spell.Level);
            if(target != null && !ApiFunctionManager.IsDead(target))
            {
                if(castTarget.GetStats().CurrentHealth > damage)
                {
                    owner.dealDamageTo(target, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                }
                else
                {
                    owner.GetStats().Gold = newGold;
                    owner.dealDamageTo(target, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                }
                projectile.setToRemove();
            }
        }
        static void onUpdate(double diff) { }
    }
}
