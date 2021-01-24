using BetterZoom.src.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace BetterZoom.src
{
    public class UIModSystem : ModSystem
    {
        // UI
        internal static UserInterface UserInterface;

        internal static UserInterface TrackerUserInterface;
        internal static UIState UITracker;

        private GameTime _lastUpdateUiGameTime;

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (UserInterface.CurrentState != null)
                UserInterface.Update(gameTime);
            if (!TrackerUI.hide)
                TrackerUserInterface.Update(gameTime);
        }

        /// <summary>
        /// Add UI
        /// </summary>
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Better Zoom: UI",
                    delegate
                    {
                        if (UserInterface?.CurrentState != null)
                            UserInterface?.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        return true;
                    }, InterfaceScaleType.UI));
            }

            int RulerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Ruler"));
            if (RulerIndex != -1)
            {
                layers.Insert(RulerIndex, new LegacyGameInterfaceLayer(
                    "Better Zoom: TrackerUI",
                    delegate
                    {
                        if (!TrackerUI.hide)
                            TrackerUserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        return true;
                    }, InterfaceScaleType.Game));
            }
        }

        public static float zoom = 1, uiScale = 1, hotbarScale = 1;
        public static bool flipBackground = true, zoomBackground = false;

        /// <summary>
        /// Change Zoom
        /// </summary>
        /// <param name="Transform">Screen Transform Matrix</param>
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            if (!Main.gameMenu)
            {
                if (zoom != 1)
                {
                    // Between 0.75f and 10f (unfortunately, due to changes to the lighting system in 1.4, anything below 0.75f seems to crash :(
                    zoom = MathHelper.Clamp(zoom, 0.75f, 10f);

                    //prevent crash
                    if (zoom >= -0.18f && zoom <= 0.18f
                        && !(zoom <= -0.2f)
                        && !Main.keyState.IsKeyDown(Keys.OemPlus))
                    {
                        zoom = -0.2f;
                    }
                    if (zoom >= -0.18f && zoom <= 0.18f
                        && !(zoom <= -0.2f)
                        && Main.keyState.IsKeyDown(Keys.OemPlus))
                    {
                        zoom = 0.2f;
                    }

                    //Change zoom
                    Main.GameZoomTarget = zoom;
                    Transform.Zoom = new Vector2(Main.GameZoomTarget);

                    //Flip background if below zero
                    if (flipBackground && zoom < 0)
                        Main.BackgroundViewMatrix.Zoom = new Vector2(-1, -1);

                    //Zoom with background if above one
                    if (zoomBackground)
                        Main.BackgroundViewMatrix.Zoom = new Vector2(Main.GameZoomTarget);
                }
                // change hotbar scale
                if (hotbarScale != 1f)
                {
                    float[] scale = { hotbarScale, hotbarScale, hotbarScale, hotbarScale, hotbarScale, hotbarScale, hotbarScale, hotbarScale, hotbarScale, hotbarScale }; // for each hotbar slot
                    Main.hotbarScale = scale;
                }
                if (uiScale != 1)
                {
                    // between 0.2 and 2
                    uiScale = MathHelper.Clamp(uiScale, 0.2f, 2);

                    // change UI scale
                    Main.UIScale = uiScale;
                }
            }
        }
    }
}