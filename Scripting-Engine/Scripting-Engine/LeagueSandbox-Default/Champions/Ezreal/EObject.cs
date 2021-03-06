﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;
using Scripting_Engine;

namespace Ezreal
{
    public class EObject : ISpellScript
    {
        public void onStartCasting(Champion champion)
        {
            Console.WriteLine("Started casting from Ezreal EObject");
        }

        public void onFinishCasting(Champion owner, Spell spell)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = new Vector2(spell.X, spell.Y) - current;
            Vector2 trueCoords;

            if (to.Length() > 475) {
                to = Vector2.Normalize(to);
                Vector2 range = to * 475;
                trueCoords = current + range;
            }
            else
            {
                trueCoords = new Vector2(spell.X, spell.Y);
            }
            ApiFunctionManager.AddParticle(owner, "Ezreal_arcaneshift_cas.troy", owner.X, owner.Y);
            spell.Teleport(owner, trueCoords.X, trueCoords.Y);
            ApiFunctionManager.AddParticleTarget(owner, "Ezreal_arcaneshift_flash.troy", owner );
            Unit target = null;
            List<Unit> units = ApiFunctionManager.GetUnitsInRange(owner, 700, true);

            foreach(Unit value in units)
            {
                float distance = 700;
                if(owner.Team != value.Team)
                {
                    if(Vector2.Distance(new Vector2(trueCoords.X, trueCoords.Y), new Vector2(value.X, value.Y)) <= distance)
                    {
                        target = value;
                        distance = Vector2.Distance(new Vector2(trueCoords.X, trueCoords.Y), new Vector2(value.X, value.Y));
                    }
                }
            }
            if(!ApiFunctionManager.UnitIsTurret(target))
            {
                spell.AddProjectileTarget("EzrealArcaneShiftMissile", target);
            }
        }


        public void applyEffects(Champion owner, Unit target, Spell spell, Projectile projectile)
        {
            owner.dealDamageTo(target, 25f + spell.Level * 50f + owner.GetStats().AbilityPower.Total * 0.75f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            projectile.setToRemove();
        }

        public void onUpdate(double diff) { }

    }
}
