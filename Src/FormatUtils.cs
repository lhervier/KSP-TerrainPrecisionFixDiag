using System.Globalization;

namespace com.github.lhervier.ksp.terrainprecisionfixdiag
{
    /// <summary>
    /// Turns what has been measured into what the table shows. Invariant culture throughout, so that two
    /// players comparing their tables read the same digits whatever their machine is set to.
    /// </summary>
    internal static class FormatUtils
    {
        /// <summary>A record number.</summary>
        public static string Format(int number)
        {
            return number.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// A distance in millimetres, to the thousandth, or "--" when it has not been read yet.
        /// </summary>
        public static string Format(double mm)
        {
            return double.IsNaN(mm) ? "--" : mm.ToString("N3", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The same, with the sign always shown: the sign is what tells a fall from an ejection.
        /// </summary>
        public static string FormatSigned(double mm)
        {
            return double.IsNaN(mm) ? "--" : mm.ToString("+0.000;-0.000;0.000", CultureInfo.InvariantCulture);
        }
    }
}
