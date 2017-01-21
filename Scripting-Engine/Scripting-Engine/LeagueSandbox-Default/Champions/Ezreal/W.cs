using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;

namespace Ezreal
{
    class W
    {
        static void onStartCasting(Champion owner)
        {
            ApiFunctionManager.AddParticleTarget(owner, "ezreal_bow_yellow.troy", owner, 1, "L_HAND");
        }
        static void onFinishCasting(Champion owner, Spell spell)
        {
            Vector2 current = new Vector2(owner.X, owner.Y);
            Vector2 to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            Vector2 range = to * 1000;
            Vector2 trueCoords = current + range;

            spell.AddProjectile("EzrealEssenceFluxMissile", trueCoords.X, trueCoords.Y);
        }
        static void applyEffects(Champion owner, Unit target, Spell spell)
        {
            if(owner.Team != target.Team)
            {
                owner.dealDamageTo(target, 25f + spell.Level * 45f + owner.GetStats().AbilityPower.Total * 0.8f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }
        static void onUpdate(double diff) { }
    }
}
