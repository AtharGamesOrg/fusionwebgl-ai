using UnityEngine;

namespace FusionWebGL.AI
{
   public class NPCContext
   {
      public string Type { get; set; }
      public string Faction { get; set; }
      public string Name { get; set; }
      public string Personality { get; set; }
      public string Background { get; set; }
      public Color NameplateColor { get; set; }
      public bool IsInteractive { get; set; }

      public NPCContext(string type, string faction, string name)
      {
         Type = type;
         Faction = faction;
         Name = name;
         Personality = "";
         Background = "";
         NameplateColor = Color.white;
         IsInteractive = true;
      }

      public NPCContext(string type, string faction, string name, string personality, string background, Color nameplateColor, bool isInteractive)
      {
         Type = type;
         Faction = faction;
         Name = name;
         Personality = personality;
         Background = background;
         NameplateColor = nameplateColor;
         IsInteractive = isInteractive;
      }
   }
}