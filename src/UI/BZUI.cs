﻿using BetterZoom.src.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.UI;

namespace BetterZoom.src.UI
{
    class BZUI : UIState
    {
        private UIFloatRangedDataValue zoom;
        private UIRange<float> zoomSldr;

        private UIFloatRangedDataValue uiScale;
        private UIRange<float> uiScaleSldr;
        public override void OnInitialize()
        {
            UIElement Menu = new TabPanel(400, 350,
                new Tab("Better Zoom", this),
                new Tab(" Camera Control", new CCUI())
                );

            Menu.Left.Set(TabPanel.lastPos.X, 0f);
            Menu.Top.Set(TabPanel.lastPos.Y, 0f);
            Append(Menu);

            zoom = new UIFloatRangedDataValue("Zoom", 1, -1f, 10);
            zoomSldr = new UIRange<float>(zoom);
            zoom.OnValueChanged += () => BetterZoom.zoom = zoom.Data;
            zoomSldr.Width.Set(0, 1);
            zoomSldr.MarginTop = 50;
            zoomSldr.MarginLeft = -20;
            Menu.Append(zoomSldr);

            uiScale = new UIFloatRangedDataValue("", 1, 0.2f, 2);
            uiScaleSldr = new UIRange<float>(uiScale);
            uiScaleSldr.Width.Set(0, 1);
            uiScaleSldr.MarginTop = 100;
            uiScaleSldr.MarginLeft = -20;
            Menu.Append(uiScaleSldr);

            var uiScaleBtn = new UITextPanel<string>("UI Scale");
            uiScaleBtn.SetPadding(4);
            uiScaleBtn.MarginLeft = 40;
            uiScaleBtn.OnClick += (evt, elm) => BetterZoom.uiScale = uiScale.Data;
            uiScaleSldr.Append(uiScaleBtn);

            UIToggleImage flipBgBtn = new UIToggleImage(TextureManager.Load("Images/UI/Settings_Toggle"), 13, 13, new Point(17, 1), new Point(1, 1));
            flipBgBtn.MarginTop = 150;
            flipBgBtn.MarginLeft = 250;
            flipBgBtn.OnClick += (evt, elm) => BetterZoom.flipBackground = !BetterZoom.flipBackground;
            flipBgBtn.SetState(BetterZoom.flipBackground);
            flipBgBtn.Append(new UIText("Flip Background", 0.9f) { MarginLeft = -230 });
            Menu.Append(flipBgBtn);

            UIToggleImage zoomBgBtn = new UIToggleImage(TextureManager.Load("Images/UI/Settings_Toggle"), 13, 13, new Point(17, 1), new Point(1, 1));
            zoomBgBtn.MarginTop = 200;
            zoomBgBtn.MarginLeft = 250;
            zoomBgBtn.OnClick += (evt, elm) => BetterZoom.zoomBackground = !BetterZoom.zoomBackground;
            zoomBgBtn.SetState(BetterZoom.zoomBackground);
            zoomBgBtn.Append(new UIText("Zoom Background", 0.9f) { MarginLeft = -230 });
            Menu.Append(zoomBgBtn);

            UIFloatRangedDataValue hotbarScale = new UIFloatRangedDataValue("Hotbar Scale", 1, 0.2f, 5);
            var hotbarScaleSldr = new UIRange<float>(hotbarScale);
            hotbarScaleSldr.Width.Set(0, 1);
            hotbarScaleSldr.MarginTop = 250;
            hotbarScaleSldr.MarginLeft = -20;
            hotbarScale.OnValueChanged += () => BetterZoom.hotbarScale = hotbarScale.Data;
            Menu.Append(hotbarScaleSldr);

            var resetBtn = new UITextPanel<string>("Set to Default");
            resetBtn.SetPadding(4);
            resetBtn.MarginLeft = 20;
            resetBtn.Top.Set(-40, 1);
            resetBtn.OnClick += (evt, elm) =>
            {
                zoom.ResetToDefaultValue();
                uiScale.ResetToDefaultValue();
                uiScale.SetValue(1);
                flipBgBtn.SetState(true);
                zoomBgBtn.SetState(false);
                hotbarScale.ResetToDefaultValue();
            };
            Menu.Append(resetBtn);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (uiScaleSldr != null && !uiScaleSldr.input.focused &&
                (Main.keyState.IsKeyDown(Keys.OemPlus) || Main.keyState.IsKeyDown(Keys.OemMinus))
                && (Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift)))
            {
                uiScale.SetValue.Invoke(BetterZoom.uiScale);
            }

            if (zoomSldr != null && !zoomSldr.input.focused)
                zoom.SetValue.Invoke(BetterZoom.zoom);
        }
    }
}