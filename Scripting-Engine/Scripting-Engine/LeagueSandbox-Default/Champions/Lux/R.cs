using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LeagueSandbox.GameServer.Logic.API;
using LeagueSandbox.GameServer.Logic.GameObjects;

namespace Lux
{
    class R
    {
        static void onStartCasting(Champion owner, Spell spell)
        {
            Vector2 current = new Vector2(owner.X, owner.Y);
            Vector2 to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            Vector2 range = to * 3340;
            Vector2 trueCoords = current + range;

            spell.AddLaser(trueCoords.X, trueCoords.Y, true);
            ApiFunctionManager.AddParticle(owner, "LuxMaliceCannon_beam.troy", trueCoords.X, trueCoords.Y);
            spell.spellAnimation("SPELL4", owner);
            ApiFunctionManager.AddParticleTarget(owner, "LuxMaliceCannon_cas.troy", owner);
        }
        static void onFinishCasting() { }
        static void applyEffects(Champion owner, Spell spell)
        {
            owner.dealDamageTo(spell.Target, 200f + spell.Level * 100f + owner.GetStats().AbilityPower.Total * 0.75f, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }
        static void onUpdate(double diff) { }
    }
}
