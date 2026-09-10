using System.Collections.Generic;
using UnityEngine;

namespace com.github.lhervier.ksp.groundheightprobe
{
    /// <summary>
    /// Position recorder. Follows two distances from the centre of the body, in millimetres: the one the
    /// active vessel starts the scene at, taken once at loading, and the one it is at right now. The
    /// player freezes the pair into a table whenever it suits them, and the table survives scene changes,
    /// so reloading the same save several times builds it up line by line.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class GroundHeightProbeMod : MonoBehaviour
    {
        private static readonly List<Reading> READINGS = new List<Reading>();

        // The line in progress, the only one that still moves. Its OnRailsMm is read while the vessel is
        // still on rails, before any physics step has run: that is the control column, and it should not
        // move from one loading to the next, being the saved value handed back untouched.
        private readonly Reading live = new Reading();

        private void Update()
        {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel == null || vessel.mainBody == null)
            {
                live.SettledMm = double.NaN;
                return;
            }
            live.SettledMm = DistanceToCentreMm(vessel);

            // Still packed means no physics step has run yet, so the vessel is where the save put it.
            if (vessel.packed)
            {
                live.OnRailsMm = live.SettledMm;
            }
        }

        /// <summary>
        /// Distance from the centre of mass of the vessel to the centre of the body it orbits, in
        /// millimetres.
        /// </summary>
        private static double DistanceToCentreMm(Vessel vessel)
        {
            // Vector3d on both sides, deliberately: reading a few millimetres out of six hundred
            // kilometres leaves no room for anything short of double precision.
            Vector3d toCentre = vessel.GetWorldPos3D() - vessel.mainBody.position;
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
                "Ground Height Probe"
            );
        }

        private void DrawWindow(int id)
        {
            GUILayout.BeginVertical();

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
                FormatUtils.Format(READINGS.Count + 1), 
                FormatUtils.Format(live.OnRailsMm),
                FormatUtils.Format(live.SettledMm), 
                FormatUtils.FormatSigned(live.MovedMm())
            );
            if (GUILayout.Button("Record", GUILayout.Width(Constants.COL_BUTTON)))
            {
                if (!double.IsNaN(live.SettledMm))
                {
                    READINGS.Add(
                        new Reading
                        {
                            OnRailsMm = live.OnRailsMm,
                            SettledMm = live.SettledMm
                        }
                    );
                }
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
