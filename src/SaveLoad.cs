using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BetterZoom.src
{
    internal class SaveLoad : ModPlayer
    {
        public override TagCompound Save()
        {
            return new TagCompound {
				// {"somethingelse", somethingelse}, // To save more data, add additional lines
				{"zoom", UIModSystem.zoom},
                {"uiscale", UIModSystem.uiScale},
                {"flipbackground", UIModSystem.flipBackground},
                {"hotbarscale", UIModSystem.hotbarScale},
                {"zoombackground", UIModSystem.zoomBackground}
            };
        }

        public override void Load(TagCompound tag)
        {
            UIModSystem.zoom = tag.GetFloat("zoom");
            UIModSystem.uiScale = tag.GetFloat("uiscale");
            UIModSystem.flipBackground = tag.GetBool("flipbackground");
            UIModSystem.hotbarScale = tag.GetFloat("hotbarscale");
            UIModSystem.zoomBackground = tag.GetBool("zoombackground");
        }
    }
}