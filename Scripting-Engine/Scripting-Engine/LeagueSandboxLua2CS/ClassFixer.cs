using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueSandboxLua2CS
{
    public class ClassFixer
    {
        public ClassFixer(string folder)
        {
            String[] allfiles = System.IO.Directory.GetFiles(folder, "*.cs", System.IO.SearchOption.AllDirectories);

            for (var i = 0; i < allfiles.Length; i++)
            {
                String fileLocation = allfiles[i];
                String fileName = fileLocation;
                String spellName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                spellName = spellName.Substring(0, spellName.Length - 3);
                String championName = fileName.Substring(0, fileName.LastIndexOf("\\"));
                championName = championName.Substring(championName.LastIndexOf("\\") + 1);
                Console.WriteLine("Champion Name: " + championName);
                Console.WriteLine("Spell Name: " + spellName);

                String oldScript = "";
                String fixedScript = "";

                if (spellName != "Q" && spellName != "W" && spellName != "E" && spellName != "R")
                {
                    if (spellName == "Passive") {
                     fixedScript = @"using LeagueSandbox.GameServer.Logic.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace "+ championName + @"
{
    public class " + spellName + @"
    {
        public static void onUpdate(double diff) { }
        public static void onDamageTaken(Unit attacker, float damage, DamageType type, DamageSource source) { }
        public static void onAutoAttack(Unit target) { }
        public static void onDealDamage(Unit target, float damage, DamageType damageType, DamageSource source) { }
        public static void onSpellCast(float x, float y, Spell slot, Unit target) { }
        public static void onDie(Unit killer) { }
    }
}
";
                    } else
                    {
                        continue;
                    }
                } else
                {
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        oldScript = sr.ReadToEnd();
                    }

                    String innerOnStartCasting = "";
                    String innerOnFinishCasting = "";
                    String innerApplyEffects = "";
                    String innerOnUpdate = "";
                    int locationOfFunctionStart;
                    if (oldScript.IndexOf("onStartCasting") != -1)
                    {

                         locationOfFunctionStart = oldScript.IndexOf("{", oldScript.IndexOf("onStartCasting")) + 1;
                        innerOnStartCasting = oldScript.Substring(locationOfFunctionStart, oldScript.IndexOf("}", locationOfFunctionStart) - locationOfFunctionStart);

                    }
                    if (oldScript.IndexOf("onFinishCasting") != -1)
                    {
                        locationOfFunctionStart = oldScript.IndexOf("{", oldScript.IndexOf("onFinishCasting")) + 1;
                        innerOnFinishCasting = oldScript.Substring(locationOfFunctionStart, oldScript.IndexOf("}", locationOfFunctionStart) - locationOfFunctionStart);
                    }
                    if (oldScript.IndexOf("applyEffects") != -1)
                    {
                        locationOfFunctionStart = oldScript.IndexOf("{", oldScript.IndexOf("applyEffects")) + 1;
                        innerApplyEffects = oldScript.Substring(locationOfFunctionStart, oldScript.IndexOf("}", locationOfFunctionStart) - locationOfFunctionStart);
                    }
                    if (oldScript.IndexOf("onUpdate") != -1)
                    {
                        locationOfFunctionStart = oldScript.IndexOf("{", oldScript.IndexOf("onUpdate")) + 1;
                        innerOnUpdate = oldScript.Substring(locationOfFunctionStart, oldScript.IndexOf("}", locationOfFunctionStart) - locationOfFunctionStart);
                    }
                     fixedScript = @"using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.Threading.Tasks;
 using System.Numerics;
 using LeagueSandbox.GameServer.Logic.GameObjects;
 using LeagueSandbox.GameServer.Logic.API;

 namespace " + championName + @"
 {
     public class " + spellName + @"
     {
         public static void onStartCasting(Champion owner)
         {
" + innerOnStartCasting + @"
         }
         public static void onFinishCasting(Champion owner, Spell spell)
         {
" + innerOnFinishCasting + @"
         }
         public static void applyEffects(Champion owner, Unit target, Spell spell, Projectile projectile)
         {
" + innerApplyEffects + @"
         }
         public static void onUpdate(double diff) {
       " + innerOnUpdate + @"  
         }
     }
 }";
                }
                

                Console.WriteLine("Old script: " + oldScript + "[End Old Script]");
                Console.WriteLine("New Script: " + fixedScript + "[End New Script]");

                System.IO.File.WriteAllText(fileLocation, fixedScript);
            }
            
        }
    }
}
