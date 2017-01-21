﻿using System;
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
        static void onFinishCasting(Unit castTarget, Spell spell)
        {
            spell.AddProjectileTarget("Disintegrate", castTarget, false);
        }
        static void applyEffects(Champion owner, Spell spell, Unit castTarget, Projectile projectile)
        {
            float AP = owner.GetStats().AbilityPower.Total * 0.8f;
            float damage = 45 + (spell.Level * 35) + AP;

            if (castTarget != null && !ApiFunctionManager.IsDead(castTarget))
            {
                owner.dealDamageTo(castTarget, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                if (ApiFunctionManager.IsDead(castTarget))
                {
                    spell.LowerCooldown(0, spell.getCooldown());
                    float manaToRecover = 55 + (spell.Level * 5);
                    float newMana = owner.GetStats().CurrentMana + manaToRecover;
                    float maxMana = owner.GetStats().ManaPoints.Total;
                    if (newMana >= maxMana)
                    {
                        owner.GetStats().CurrentMana = maxMana;
                    }
                    else
                    {
                        owner.GetStats().CurrentMana = newMana;
                    }
                }
            }
            projectile.setToRemove();
        }
        static void onUpdate(double diff) { }
    }
}
