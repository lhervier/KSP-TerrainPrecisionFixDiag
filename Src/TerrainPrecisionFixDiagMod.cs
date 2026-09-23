using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.github.lhervier.ksp.terrainprecisionfixdiag
{
    /// <summary>
    /// Position recorder. Follows two distances from the centre of the body, in millimetres: the one a
    /// craft is held at while the game runs it on rails, and the one it is at right now. It reads the
    /// craft being flown, or the target when one is set on another craft. The player freezes the pair
    /// into a table whenever it suits them, and the table survives scene changes, so reloading the same
    /// save several times builds it up line by line.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class TerrainPrecisionFixDiagMod : MonoBehaviour
    {
        private static readonly List<Reading> READINGS = new List<Reading>();

        // The line in progress, the only one that still moves. Its OnRailsMm is the last distance the
        // subject was held at while on rails: that is the control column, and it should not move from
        // one loading to the next, being the saved value handed back untouched.
        private readonly Reading live = new Reading();

        // What to tell the player about the craft the readings are about. Refreshed by Update,
        // displayed by OnGUI.
        private string subjectLabel = "";

        // The last distance every loaded craft was held at while on rails, by vessel id. Kept for all
        // of them rather than for the subject alone: a craft is only on rails for a while, and the
        // player may well pick it as a target after that window has closed -- at the opening of a
        // scene, the game restores a saved target several dozen frames in, by which time a landed
        // craft has been handed to physics.
        private readonly Dictionary<Guid, double> onRailsByVessel = new Dictionary<Guid, double>();

        private void Update()
        {
            TrackOnRailsDistances();

            Vessel vessel = SelectSubject(out subjectLabel);
            if (vessel == null || vessel.mainBody == null)
            {
                live.OnRailsMm = double.NaN;
                live.SettledMm = double.NaN;
                return;
            }

            live.SettledMm = DistanceToCentreMm(vessel);
            double onRails;
            live.OnRailsMm = onRailsByVessel.TryGetValue(vessel.id, out onRails) ? onRails : double.NaN;
        }

        /// <summary>
        /// Follows, for every loaded craft, the distance the game holds it at while it is on rails.
        /// </summary>
        private void TrackOnRailsDistances()
        {
            List<Vessel> loaded = (FlightGlobals.fetch == null) ? null : FlightGlobals.VesselsLoaded;
            if (loaded == null)
            {
                return;
            }

            for (int i = 0; i < loaded.Count; i++)
            {
                Vessel vessel = loaded[i];

                // Packed means no physics step runs on it, so it sits where the game holds it: the
                // saved state when the scene has just opened, and otherwise where it last came to
                // rest. A craft that leaves and comes back is read again on its way in, since the
                // game loads it at 2250 m and only hands it to physics at 200 m: no distance kept
                // here can outlive the trip that made it stale.
                if (vessel != null && vessel.mainBody != null && vessel.packed)
                {
                    onRailsByVessel[vessel.id] = DistanceToCentreMm(vessel);
                }
            }
        }

        /// <summary>
        /// The vessel the readings are about: the target when one is set on a vessel, the craft being
        /// flown otherwise. Null when that vessel cannot be read. <paramref name="label"/> receives what
        /// to tell the player about the choice, including why nothing can be read.
        /// </summary>
        private static Vessel SelectSubject(out string label)
        {
            ITargetable target = (FlightGlobals.fetch == null) ? null : FlightGlobals.fetch.VesselTarget;
            if (target != null)
            {
                // A target is anything targetable: a planet, a docking port, a craft. GetVessel gives
                // the vessel behind it, and null for what is not one -- targeting a planet leaves the
                // readings on the craft being flown rather than emptying the window.
                Vessel targetVessel = target.GetVessel();
                if (targetVessel != null)
                {
                    if (!targetVessel.loaded)
                    {
                        label = "Target: " + targetVessel.vesselName + " -- too far away to read";
                        return null;
                    }
                    label = "Target: " + targetVessel.vesselName;
                    return targetVessel;
                }
            }

            Vessel active = FlightGlobals.ActiveVessel;
            label = (active == null) ? "Nothing to read" : "Craft you are flying: " + active.vesselName;
            return active;
        }

        /// <summary>
        /// Distance from the origin of the root part of the vessel to the centre of the body it orbits,
        /// in millimetres. That origin is the point KSP saves and restores, so while the vessel is on
        /// rails, this distance is exactly the body radius plus the altitude the game holds it at.
        /// </summary>
        private static double DistanceToCentreMm(Vessel vessel)
        {
            // Vector3d on both sides, deliberately: reading a few millimetres out of six hundred
            // kilometres leaves no room for anything short of double precision.
            Vector3d toCentre = (Vector3d)vessel.vesselTransform.position - vessel.mainBody.position;
            return toCentre.magnitude * 1000.0;
        }

        // =========================================================
        // UI
        // =========================================================

        private Rect windowRect = new Rect(
            Constants.WINDOW_X, 
            Constants.WINDOW_Y, 
            Constants.WINDOW_WIDTH, 
            0f
        );

        private void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            windowRect = GUILayout.Window(
                Constants.WINDOW_ID, 
                windowRect, 
                DrawWindow, 
                "Terrain Precision Fix Diag"
            );
        }

        private void DrawWindow(int id)
        {
            GUILayout.BeginVertical();

            // Which craft the numbers are about. Without it the window reads the same whether it follows
            // the craft being flown or the one being approached.
            GUILayout.Label(subjectLabel);
            GUILayout.Space(4f);

            // Header
            GUILayout.BeginHorizontal();
            DrawCells("Record #", "On rails (mm)", "Settled (mm)", "Moved (mm)");
            GUILayout.EndHorizontal();

            // Recorded lines
            int deleteIndex = -1;
            for (int i = 0; i < READINGS.Count; i++)
            {
                Reading reading = READINGS[i];
                GUILayout.BeginHorizontal();
                DrawCells(
                    FormatUtils.Format(i + 1), 
                    FormatUtils.Format(reading.OnRailsMm),
                    FormatUtils.Format(reading.SettledMm), 
                    FormatUtils.FormatSigned(reading.MovedMm())
                );
                if (GUILayout.Button("Delete", GUILayout.Width(Constants.COL_BUTTON)))
                {
                    deleteIndex = i;
                }
                GUILayout.EndHorizontal();
            }
            if (deleteIndex >= 0)
            {
                READINGS.RemoveAt(deleteIndex);
            }

            // Current line
            GUILayout.BeginHorizontal();
            DrawCells(
                FormatUtils.Format(FormatUtils.NO_NUMBER), 
                FormatUtils.Format(live.OnRailsMm),
                FormatUtils.Format(live.SettledMm), 
                FormatUtils.FormatSigned(live.MovedMm())
            );
            if (GUILayout.Button("Record", GUILayout.Width(Constants.COL_BUTTON)))
            {
                // A line with nothing to read is worth freezing too: recorded while the vessel is out
                // of reach, it marks in the table that the two lines around it are separated by a real
                // trip away, and not by two readings taken where the player stood.
                READINGS.Add(
                    new Reading
                    {
                        OnRailsMm = live.OnRailsMm,
                        SettledMm = live.SettledMm
                    }
                );
            }
            GUILayout.EndHorizontal();

            // Clear table button
            GUILayout.Space(10f);
            if (GUILayout.Button("Clear table"))
            {
                READINGS.Clear();
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>Draws the four columns of one line. The caller owns the surrounding horizontal group,
        /// so that it can put a button at the end of the line.</summary>
        private static void DrawCells(string record, string onRails, string settled, string moved)
        {
            GUILayout.Label(record, GUILayout.Width(Constants.COL_RECORD));
            GUILayout.Label(onRails, GUILayout.Width(Constants.COL_DISTANCE));
            GUILayout.Label(settled, GUILayout.Width(Constants.COL_DISTANCE));
            GUILayout.Label(moved, GUILayout.Width(Constants.COL_MOVED));
        }
    }
}
