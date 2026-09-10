using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace com.github.lhervier.ksp.groundheightprobe
{
    /// <summary>
    /// Measuring instrument. Shows, in a flight window, how far the active vessel is from the centre of
    /// the body it sits on, in millimetres, and keeps one line per scene loading so that reloading the
    /// same save several times builds a table that reads on its own.
    ///
    /// It measures and nothing else: no vessel is moved, no stock behaviour is patched, nothing is
    /// written to disk.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class GroundHeightProbeMod : MonoBehaviour
    {
        private const string LOG_PREFIX = "[GroundHeightProbe] ";

        private const int WINDOW_ID = 0x47485001;

        // Column widths, in pixels. Fixed rather than laid out by content: the numbers only speak once
        // aligned as a column, and the skin font is not monospaced.
        private const float COL_LOADING = 70f;
        private const float COL_DISTANCE = 165f;
        private const float COL_DELTA = 95f;

        /// <summary>One recorded scene loading.</summary>
        private class Reading
        {
            public int Loading;
            public string BodyName;
            public double OnRailsMm;
            public double SettledMm;
        }

        // The table lives in a static: a KSPAddon is rebuilt for every scene, so an instance field would
        // lose it on the very event being measured. Nothing is persisted, so it empties itself when KSP
        // is closed, which is exactly the lifetime one measurement campaign needs.
        private static readonly List<Reading> READINGS = new List<Reading>();

        // The distance restored from the save, read while the vessel is still on rails, before any
        // physics step has run. This is the control column: it must not move from one loading to the
        // next, being the saved value handed back untouched.
        private double onRailsMm = double.NaN;

        // The row this loading has already produced, if the player has clicked. Clicking again updates it
        // instead of adding a second one: one loading, one line.
        private Reading currentReading;

        private double liveMm = double.NaN;
        private string bodyName = "";
        private Rect windowRect = new Rect(60f, 60f, 560f, 0f);
        private GUIStyle headlineStyle;

        private void Update()
        {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel == null || vessel.mainBody == null)
            {
                liveMm = double.NaN;
                return;
            }
            bodyName = vessel.mainBody.bodyName;
            liveMm = DistanceToCentreMm(vessel);

            // Still packed means no physics step has run yet, so the vessel is where the save put it.
            if (double.IsNaN(onRailsMm) && vessel.packed)
            {
                onRailsMm = liveMm;
            }
        }

        /// <summary>
        /// Distance from the centre of mass of the vessel to the centre of the body it orbits, in
        /// millimetres.
        /// </summary>
        private static double DistanceToCentreMm(Vessel vessel)
        {
            // Vector3d on both sides, deliberately. What is looked for is a few centimetres out of six
            // hundred kilometres; a float, whose step is 62.5 mm at that distance, would not merely blur
            // the reading, it would reproduce the very defect this instrument exists to show.
            Vector3d toCentre = vessel.GetWorldPos3D() - vessel.mainBody.position;
            return toCentre.magnitude * 1000.0;
        }

        /// <summary>
        /// Adds the current reading to the table, or updates this loading's row if it already has one.
        /// </summary>
        private void Record()
        {
            if (double.IsNaN(liveMm))
            {
                return;
            }

            // A campaign covers one body. Mixing two would make the range line meaningless, and the range
            // line is the point of the whole table.
            if (READINGS.Count > 0 && READINGS[0].BodyName != bodyName)
            {
                READINGS.Clear();
                currentReading = null;
            }

            if (currentReading == null)
            {
                currentReading = new Reading();
                currentReading.Loading = READINGS.Count + 1;
                currentReading.BodyName = bodyName;
                READINGS.Add(currentReading);
            }
            currentReading.OnRailsMm = onRailsMm;
            currentReading.SettledMm = liveMm;

            Debug.Log(LOG_PREFIX + "loading " + currentReading.Loading + " on " + bodyName
                + ": on rails " + Format(onRailsMm) + " mm, settled " + Format(liveMm) + " mm");
        }

        private void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            if (headlineStyle == null)
            {
                headlineStyle = new GUIStyle(HighLogic.Skin.label);
                headlineStyle.fontStyle = FontStyle.Bold;
            }
            windowRect = GUILayout.Window(WINDOW_ID, windowRect, DrawWindow, "Ground Height Probe");
        }

        private void DrawWindow(int id)
        {
            GUILayout.BeginVertical();

            GUILayout.Label(bodyName.Length == 0 ? "No active vessel" : "Body: " + bodyName);
            GUILayout.Label("Distance to centre now: " + Format(liveMm) + " mm");

            if (GUILayout.Button(currentReading == null
                ? "Record this loading (once the vessel has settled)"
                : "Update this loading"))
            {
                Record();
            }

            GUILayout.Space(10f);
            DrawTable();
            GUILayout.Space(10f);

            if (GUILayout.Button("Clear table"))
            {
                READINGS.Clear();
                currentReading = null;
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void DrawTable()
        {
            DrawRow("Loading", "On rails (mm)", "Settled (mm)", "Delta");
            if (READINGS.Count == 0)
            {
                GUILayout.Label("Nothing recorded yet.");
                return;
            }

            double reference = READINGS[0].SettledMm;
            for (int i = 0; i < READINGS.Count; i++)
            {
                Reading reading = READINGS[i];
                string delta = i == 0
                    ? "reference"
                    : (reading.SettledMm - reference).ToString("+0.000;-0.000;0.000", CultureInfo.InvariantCulture);
                DrawRow(reading.Loading.ToString(CultureInfo.InvariantCulture),
                    Format(reading.OnRailsMm), Format(reading.SettledMm), delta);
            }

            GUILayout.Space(8f);

            // The two ranges side by side are the demonstration: the value read back from the save does
            // not move, the one measured once the vessel has settled does.
            GUILayout.Label("Range on rails, over " + READINGS.Count + " loadings: "
                + Format(RangeMm(true)) + " mm");
            GUILayout.Label("Range settled, over " + READINGS.Count + " loadings: "
                + Format(RangeMm(false)) + " mm", headlineStyle);
        }

        private void DrawRow(string loading, string onRails, string settled, string delta)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(loading, GUILayout.Width(COL_LOADING));
            GUILayout.Label(onRails, GUILayout.Width(COL_DISTANCE));
            GUILayout.Label(settled, GUILayout.Width(COL_DISTANCE));
            GUILayout.Label(delta, GUILayout.Width(COL_DELTA));
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Spread between the largest and the smallest reading of one column, in millimetres, or NaN when
        /// fewer than two rows carry a value.
        /// </summary>
        private static double RangeMm(bool onRails)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            int count = 0;
            for (int i = 0; i < READINGS.Count; i++)
            {
                double value = onRails ? READINGS[i].OnRailsMm : READINGS[i].SettledMm;
                if (double.IsNaN(value))
                {
                    continue;
                }
                min = value < min ? value : min;
                max = value > max ? value : max;
                count++;
            }
            return count < 2 ? double.NaN : max - min;
        }

        private static string Format(double mm)
        {
            return double.IsNaN(mm) ? "--" : mm.ToString("N3", CultureInfo.InvariantCulture);
        }
    }
}
