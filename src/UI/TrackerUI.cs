﻿using BetterZoom.src.Trackers;
using Microsoft.Xna.Framework;
using Terraria.UI;

namespace BetterZoom.src.UI
{
    class TrackerUI : UIState
    {
        public static bool hide;
        public override void OnInitialize()
        {
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Fix
            if (EntityTracker.TrackedEntity != null)
            {
                Append(EntityTracker.ETrackerImg);

                // Fix Entity Tracker Position
                EntityTracker.FixPosition();
            }
            // Fix Path Tracker Position
            PathTrackers.FixPosition();

            for (int i = 0; i < PathTrackers.trackers.Count; i++)
            {
                // Append PathTrackers
                Append(PathTrackers.trackers[i].PTrackerImg);

                // Append Lines
                if (PathTrackers.trackers[i].Connection != null)
                    Append(PathTrackers.trackers[i].Connection);

                // Append Control Points
                if (i + 1 < PathTrackers.trackers.Count && CCUI.selectedInterp == 2 && PathTrackers.trackers[i].Connection != null)
                {
                    Append(PathTrackers.trackers[i].Connection.ControlPoint);
                }
            }

            // Fix Line Position
            PathTrackers.FixLinePosition();
        }
    }
}
