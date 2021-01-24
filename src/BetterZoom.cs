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
    public class BetterZoom : Mod
    {
        // Hotkeys
        public static ModHotKey LockScreen, SetTracker, RemoveTracker, ShowUI;

        /// <summary>
        /// Load Hotkeys and UI
        /// </summary>
        public override void Load()
        {
            // Hotkeys
            LockScreen = RegisterHotKey("Lock Screen", "L");
            SetTracker = RegisterHotKey("Set Tracker", "K");
            RemoveTracker = RegisterHotKey("Remove Tracker", "O");
            ShowUI = RegisterHotKey("Show UI", "B");
            // UI
            if (!Main.dedServ)
            {
                UIModSystem.UserInterface = new UserInterface();
                UIModSystem.UserInterface.SetState(null);

                UIModSystem.UITracker = new TrackerUI();
                UIModSystem.UITracker.Activate();
                UIModSystem.TrackerUserInterface = new UserInterface();
                UIModSystem.TrackerUserInterface.SetState(UIModSystem.UITracker);

                UI.UIElements.TabPanel.lastTab = new BZUI();
            }

            Trackers.PathTrackers.trackers = new List<Trackers.PathTrackers>();
        }

        public override void Unload()
        {
            // Other static Fields
            foreach (var tracker in Trackers.PathTrackers.trackers)
            {
                tracker.Connection.ControlPoint = null;
                tracker.PTrackerImg = null;
                tracker.Connection = null;
            }
            Trackers.PathTrackers.trackers = null;
            Trackers.EntityTracker.TrackedEntity = null;
            Trackers.EntityTracker.tracker = null;
            Trackers.EntityTracker.ETrackerImg = null;
            Camera.locked = false;
            CCUI.lockScreenBtn = null;
            CCUI.placeTracker = null;
            Config.Instance = null;
            UI.UIElements.TabPanel.lastTab = null;

            // UI
            UIModSystem.UserInterface = null;
            UIModSystem.TrackerUserInterface = null;
            UIModSystem.UITracker = null;

            // Hotkeys
            LockScreen =
            SetTracker =
            ShowUI =
            RemoveTracker = null;
        }
    }
}