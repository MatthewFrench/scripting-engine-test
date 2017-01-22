using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;
namespace Caitlyn
{
    class R
    {
        static void onStartCasting()
        {
        
        }
        static void onFinishCasting(Champion owner, Spell spell, Unit castTarget)
        {
            Vector2 current = new Vector2(owner.X, owner.Y);
            if(Vector2.Distance(current, new Vector2(castTarget.X, castTarget.Y)) <= spell.getEffectValue(1))
            {
                spell.AddProjectileTarget("CaitlynAceintheHoleMissile", castTarget);
            }
        }
        static void applyEffects(Champion owner, Spell spell, Unit castTarget, Projectile projectile)
        {
            if(castTarget != null && ApiFunctionManager.IsDead(castTarget))
            {
                float damage = spell.getEffectValue(0) + owner.GetStats().AttackDamage.Total * 2;
                owner.dealDamageTo(castTarget, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
            projectile.setToRemove();
        }
        static void onUpdate(double diff)
        {
        
        }
    }
}
