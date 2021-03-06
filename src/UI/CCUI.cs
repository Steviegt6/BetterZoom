﻿using BetterZoom.src.Trackers;
using BetterZoom.src.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace BetterZoom.src.UI
{
    internal class CCUI : UIState
    {
        private UITextPanel<string> playButton;
        private UIFloatRangedDataValue speed;
        private byte placing;
        private byte move = 0;
        private bool erasing;
        private bool moving;
        public static UIToggleImage lockScreenBtn;
        public static byte selectedInterp = 2;
        public static UIImage placeTracker;

        public override void OnInitialize()
        {
            Camera.fixedscreen = Main.LocalPlayer.position - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

            TabPanel Menu = new TabPanel(400, 350,
                new Tab("Better Zoom", new BZUI()),
                new Tab(" Camera Control", this)
                );
            Menu.Left.Set(DragableUIPanel.lastPos.X, 0f);
            Menu.Top.Set(DragableUIPanel.lastPos.Y, 0f);
            Menu.OnCloseBtnClicked += () => UIModSystem.UserInterface.SetState(null);
            Append(Menu);

            speed = new UIFloatRangedDataValue("Tracking Speed", 1, 0.1f, 100);
            var speedSldr = new UIRange<float>(speed);
            speedSldr.Width.Set(0, 1);
            speedSldr.MarginTop = 50;
            speedSldr.MarginLeft = -20;
            Menu.Append(speedSldr);

            Menu.Append(new UIText("Control Trackers: ") { MarginTop = 130, MarginLeft = 210 });

            UIHoverImageButton PathTrackerBtn = new UIHoverImageButton("BetterZoom/Assets/PathTrackerButton", "Place Path tracker");
            PathTrackerBtn.OnClick += (evt, elm) =>
            {
                placing = 1;
            };
            PathTrackerBtn.MarginLeft = 245;
            PathTrackerBtn.MarginTop = 155;
            Menu.Append(PathTrackerBtn);

            UIHoverImageButton EraseTrackerBtn = new UIHoverImageButton("BetterZoom/Assets/EraserButton", "Erase Trackers");
            EraseTrackerBtn.OnClick += (evt, elm) => erasing = !erasing;
            EraseTrackerBtn.MarginLeft = 245;
            EraseTrackerBtn.MarginTop = 190;
            Menu.Append(EraseTrackerBtn);

            DragableUIPanel ConfirmPanel = new DragableUIPanel("Are you sure you want to remove all trackers?", 700, 120);
            UIHoverImageButton DelBtn = new UIHoverImageButton("BetterZoom/Assets/DelButton", "Delete all Trackers");
            DelBtn.OnClick += (evt, elm) =>
            {
                if (!ConfirmPanel.active)
                {
                    ConfirmPanel.Left.Set(1000, 0f);
                    ConfirmPanel.Top.Set(500, 0f);
                    ConfirmPanel.Width.Set(400, 0f);
                    ConfirmPanel.Height.Set(120, 0f);
                    Append(ConfirmPanel);

                    UITextPanel<string> yep = new UITextPanel<string>("Yes")
                    {
                        HAlign = 0.2f,
                        VAlign = 0.7f
                    };
                    yep.Width.Set(100, 0f);
                    yep.OnClick += (evt1, elm1) => { PathTrackers.RemoveAll(); ConfirmPanel.Remove(); };
                    ConfirmPanel.Append(yep);

                    UITextPanel<string> nop = new UITextPanel<string>("No")
                    {
                        HAlign = 0.8f,
                        VAlign = 0.7f
                    };
                    nop.Width.Set(100, 0f);
                    nop.OnClick += (evt1, elm1) => { ConfirmPanel.Remove(); };
                    ConfirmPanel.Append(nop);
                }
            };
            DelBtn.MarginLeft = 285;
            DelBtn.MarginTop = 190;
            Menu.Append(DelBtn);

            UIHoverImageButton EntityBtn = new UIHoverImageButton("BetterZoom/Assets/EntityTrackerButton", "Place Entity Tracker");
            EntityBtn.OnClick += (evt, elm) =>
            {
                if (EntityTracker.tracker == null) placing = 2;
                else EntityTracker.RemoveTracker();
            };
            EntityBtn.MarginLeft = 285;
            EntityBtn.MarginTop = 155;
            Menu.Append(EntityBtn);

            UIHoverImageButton MoveBtn = new UIHoverImageButton("BetterZoom/Assets/MoveButton", "Move Path Tracker");
            MoveBtn.OnClick += (evt, elm) => moving = !moving;
            MoveBtn.MarginLeft = 325;
            MoveBtn.MarginTop = 190;
            Menu.Append(MoveBtn);

            lockScreenBtn = new UIToggleImage(Main.Assets.Request<Texture2D>("Images\\UI\\Settings_Toggle"), 13, 13, new Point(17, 1), new Point(1, 1))
            {
                MarginTop = 100,
                MarginLeft = 250
            };
            lockScreenBtn.OnClick += (evt, elm) =>
            {
                Camera.fixedscreen = Main.screenPosition;
                Camera.locked = !Camera.locked;
            };
            lockScreenBtn.Append(new UIText("Lock Screen", 0.9f) { MarginLeft = -230 });
            Menu.Append(lockScreenBtn);

            Menu.Append(new UIText("Control Screen: ") { MarginTop = 130, MarginLeft = 20 });

            // Dpad
            var Dpad = UIHelper.Dpad(60, 155);
            for (int i = 0; i < Dpad.Length; i++)
                Menu.Append(Dpad[i]);

            Dpad[0].OnMouseDown += (evt, elm) => move = 1;
            Dpad[0].OnMouseUp += (evt, elm) => move = 0;
            Dpad[0].OnClick += (evt, elm) => Camera.locked = true;

            Dpad[1].OnMouseDown += (evt, elm) => move = 2;
            Dpad[1].OnMouseUp += (evt, elm) => move = 0;
            Dpad[1].OnClick += (evt, elm) => Camera.locked = true;

            Dpad[2].OnMouseDown += (evt, elm) => move = 3;
            Dpad[2].OnMouseUp += (evt, elm) => move = 0;
            Dpad[2].OnClick += (evt, elm) => Camera.locked = true;

            Dpad[3].OnMouseDown += (evt, elm) => move = 4;
            Dpad[3].OnMouseUp += (evt, elm) => move = 0;
            Dpad[3].OnClick += (evt, elm) => Camera.locked = true;

            var hideTrackersBtn = new UIToggleImage(Main.Assets.Request<Texture2D>("Images\\UI\\Settings_Toggle"), 13, 13, new Point(17, 1), new Point(1, 1))
            {
                MarginTop = 250,
                MarginLeft = 250
            };
            hideTrackersBtn.OnClick += (evt, elm) => TrackerUI.hide = !TrackerUI.hide;
            hideTrackersBtn.Append(new UIText("Hide Trackers", 0.9f) { MarginLeft = -230 });
            Menu.Append(hideTrackersBtn);

            // Control Buttons
            playButton = new UITextPanel<string>("Play")
            {
                VAlign = 0.9f,
                HAlign = 0.1f
            };
            playButton.OnClick += (evt, elm) => Camera.PlayStopTracking();
            Menu.Append(playButton);

            UITextPanel<string> pauseButton = new UITextPanel<string>("Pause")
            {
                VAlign = 0.9f,
                HAlign = 0.5f
            };
            pauseButton.OnClick += (evt, elm) =>
            {
                Camera.PauseTracking();
                pauseButton.SetText(text: Camera.Playing ? "Pause" : "Resume");
            };
            Menu.Append(pauseButton);

            var repeatBtn = new UITextPanel<string>("Repeat")
            {
                VAlign = 0.9f,
                HAlign = 0.9f
            };
            repeatBtn.OnClick += (evt, elm) =>
            {
                Camera.repeat = !Camera.repeat;
                repeatBtn.SetText(text: Camera.repeat ? "End" : "Repeat");
            };
            Menu.Append(repeatBtn);

            placeTracker = new UIImage(ModContent.GetTexture("BetterZoom/Assets/PathTracker"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (lockScreenBtn != null)
                lockScreenBtn.SetState(Camera.locked);
            Camera.speed = speed.Data * 100;

            playButton.SetText(text: Camera.Playing ? "Stop" : "Play");

            // Placer
            if (placing != 0)
            {
                Main.cursorOverride = 16;
                Main.LocalPlayer.mouseInterface = true;
                placeTracker.ImageScale = 0.5f;
                placeTracker.MarginLeft = Main.MouseScreen.X - placeTracker.Width.Pixels / 2;
                placeTracker.MarginTop = Main.MouseScreen.Y - placeTracker.Height.Pixels / 2;
                erasing = moving = false;

                if (placing == 1)
                {
                    placeTracker.SetImage(ModContent.GetTexture("BetterZoom/Assets/PathTracker"));
                    Append(placeTracker);

                    if (Main.mouseLeft)
                    {
                        new PathTrackers(Main.MouseWorld);
                        placeTracker.Remove();
                        placing = 0;
                    }
                }
                // Entity Tracker
                else if (placing == 2)
                {
                    placeTracker.SetImage(ModContent.GetTexture("BetterZoom/Assets/EntityTracker"));
                    Append(placeTracker);

                    if (Main.mouseLeft)
                    {
                        Camera.locked = true;
                        new EntityTracker(Main.MouseWorld);
                        placeTracker.Remove();
                        placing = 0;
                    }
                }
            }
            // Eraser
            if (erasing)
            {
                Main.cursorOverride = 6;
                Main.LocalPlayer.mouseInterface = true;
                moving = false;
                placing = 0;

                if (Main.mouseLeft)
                {
                    PathTrackers.Remove();
                }
            }

            // Tracker Mover
            if (moving)
            {
                placing = 0;
                erasing = false;
                Main.cursorOverride = 16;
                Main.LocalPlayer.mouseInterface = true;

                if (Main.mouseLeft)
                {
                    for (int i = 0; i < PathTrackers.trackers.Count; i++)
                    {
                        if (PathTrackers.trackers[i].PTrackerImg.IsMouseHovering)
                        {
                            PathTrackers.trackers[i].Position = Main.MouseWorld;
                        }
                    }
                }
            }

            // Camera Mover
            switch (move)
            {
                case 1:
                    Camera.fixedscreen += new Vector2(0, -5f);
                    break;

                case 2:
                    Camera.fixedscreen += new Vector2(0, 5f);
                    break;

                case 3:
                    Camera.fixedscreen += new Vector2(-5f, 0);
                    break;

                case 4:
                    Camera.fixedscreen += new Vector2(5f, 0);
                    break;

                default:
                    break;
            }
        }
    }
}