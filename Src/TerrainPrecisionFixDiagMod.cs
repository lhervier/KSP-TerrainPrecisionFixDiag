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

        // The line in progress, the only one that still moves. Its OnRailsMm is read while the vessel is
        // still on rails, before any physics step has run on it: that is the control column, and it
        // should not move from one loading to the next, being the saved value handed back untouched.
        private readonly Reading live = new Reading();

        // The vessel the readings are about, and what to tell the player about that choice. Refreshed
        // by Update, displayed by OnGUI.
        private Vessel subject;
        private string subjectLabel = "";

        // CAMPAIGN ONLY, to be removed before publishing: how many times the game has moved the origin
        // of the world since the scene opened, and since the last recorded line. A shift is the moment
        // the game re-places the ground, so a line taken without one in between proves nothing.
        private int originShifts;
        private int originShiftsSinceRecord;

        private void Awake()
        {
            // An instance method: EventData refuses a static handler.
            GameEvents.onFloatingOriginShift.Add(OnOriginShift);
        }

        private void OnDestroy()
        {
            GameEvents.onFloatingOriginShift.Remove(OnOriginShift);
        }

        private void OnOriginShift(Vector3d offset, Vector3d nonFrame)
        {
            originShifts++;
            originShiftsSinceRecord++;
        }

        private void Update()
        {
            Vessel vessel = SelectSubject(out subjectLabel);

            // The control column belongs to one craft: changing subject starts it over, rather than
            // facing a distance taken on one craft with a distance taken on another.
            if (vessel != subject)
            {
                subject = vessel;
                live.OnRailsMm = double.NaN;
            }

            if (vessel == null || vessel.mainBody == null)
            {
                live.SettledMm = double.NaN;
                return;
            }
            live.SettledMm = DistanceToCentreMm(vessel);

            // Packed means no physics step runs on it, so it sits where the game holds it: the saved
            // state when the scene has just opened, and otherwise where it last came to rest.
            if (vessel.packed)
            {
                live.OnRailsMm = live.SettledMm;
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
                if (!double.IsNaN(live.SettledMm))
                {
                    READINGS.Add(
                        new Reading
                        {
                            OnRailsMm = live.OnRailsMm,
                            SettledMm = live.SettledMm
                        }
                    );
                    originShiftsSinceRecord = 0;
                }
            }
            GUILayout.EndHorizontal();

            // CAMPAIGN ONLY: the context every line needs to mean anything.
            GUILayout.Space(4f);
            GUILayout.Label(OriginLine());

            // Clear table button
            GUILayout.Space(10f);
            if (GUILayout.Button("Clear table"))
            {
                READINGS.Clear();
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>CAMPAIGN ONLY: how far the craft being flown has drifted from the origin of the
        /// world, and how many times that origin has been reset.</summary>
        private string OriginLine()
        {
            Vessel active = FlightGlobals.ActiveVessel;
            string distance = (active == null)
                ? "?"
                : ((Vector3d)active.vesselTransform.position).magnitude.ToString("N1");
            return "You are " + distance + " m from the origin of the world -- " + originShifts
                + " shift(s) so far, " + originShiftsSinceRecord + " since the last record";
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
