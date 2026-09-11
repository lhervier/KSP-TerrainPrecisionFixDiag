using System.Globalization;

namespace com.github.lhervier.ksp.terrainprecisionfixdiag
{
    /// <summary>
    /// Turns what has been measured into what the table shows. Invariant culture throughout, so that two
    /// players comparing their tables read the same digits whatever their machine is set to.
    /// </summary>
    internal static class FormatUtils
    {
        /// <summary>
        /// The number a record carries in the table, or "--" for NO_NUMBER.
        /// </summary>
        public static string Format(int number)
        {
            return number < 0 ? "--" : number.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Stands for the record number of a line that has none: the line in progress, which is not a
        /// record until it is frozen into the table. What NaN does for the distances, int cannot do, so
        /// any negative number does it instead.
        /// </summary>
        public const int NO_NUMBER = -1;

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
