using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;

namespace Ezreal
{
    class E
    {
        static void onStartCasting()
        {

        }

        static void onFinishCasting(Champion owner, Spell spell)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = new Vector2(spell.X, spell.Y) - current;
            Vector2 trueCoords;

            if (to.Length() > 475) {
                //to = to.Normalize()
                //local range = to * 475
                //trueCoords = current:copy() + range
            } else {
                trueCoords = new Vector2(spell.X, spell.Y);
             }
            /*

    if to:length() > 475 then
        to = to:normalize()
                local range = to * 475
                trueCoords = current:copy() + range
            else
        trueCoords = Vector2:new(spell.X, spell.Y)
            end
        
    addParticle(owner, "Ezreal_arcaneshift_cas.troy", owner.X, owner.Y);
            teleportTo(owner, trueCoords.x, trueCoords.y)
            addParticleTarget(owner, "Ezreal_arcaneshift_flash.troy", owner);

            local target = nil
            local units = getUnitsInRange(owner, 700, true)
        

    for i = 0, units.Count - 1 do
                    value = units[i]
     
             local distance = 700
                if owner.Team ~= value.Team then
                    if Vector2:new(trueCoords.x, trueCoords.y):distance(Vector2: new(value.X, value.Y)) <= distance then
                 target = value
                        distance = Vector2:new(trueCoords.x, trueCoords.y):distance(Vector2: new(value.X, value.Y))
                    end
        end
            end
            if (target) and(not unitIsTurret(target)) then
               addProjectileTarget("EzrealArcaneShiftMissile", target)
            end
            */
        }


        static void applyEffects()
        {
            /*
            dealMagicalDamage(25 + spellLevel * 50 + owner:GetStats().AbilityPower.Total * 0.75)
            destroyProjectile()
            */
        }

        static void onUpdate(double diff) { }

    }
}
