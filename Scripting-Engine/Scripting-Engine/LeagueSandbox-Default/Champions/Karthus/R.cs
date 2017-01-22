using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;
namespace Karthus
{
    class R
    {
        static void onStartCasting(Champion owner)
        {
            foreach(Champion target in ApiFunctionManager.GetChampionsInRange(owner, 20000, true).Where(target => target.Team != owner.Team))
            {
                ApiFunctionManager.AddParticleTarget(owner, "KarthusFallenOne", target);
            }
        }
        static void onFinishCasting(Champion owner, Spell spell)
        {
            float AP = owner.GetStats().AbilityPower.Total;
            float damage = 100 + (spell.Level * 150) + AP * 0.6f;
            foreach (Champion target in ApiFunctionManager.GetChampionsInRange(owner, 20000, true).Where(target => target.Team != owner.Team))
            {
                owner.dealDamageTo(target, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }
        static void applyEffects()
        {
        
        }
        static void onUpdate(double diff)
        {
        
        }
    }
}
