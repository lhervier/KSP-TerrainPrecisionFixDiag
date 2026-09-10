namespace com.github.lhervier.ksp.groundheightprobe
{
    /// <summary>
    /// One line of the table: where the vessel started the scene, and where it is now. Both start
    /// unknown, and a frozen line is just one that has stopped being updated.
    /// </summary>
    internal class Reading
    {
        public double OnRailsMm = double.NaN;
        public double SettledMm = double.NaN;

        /// <summary>
        /// How far the vessel ended up from the position the save gave back, in millimetres, negative
        /// downwards. NaN as long as either of the two distances is unknown.
        /// </summary>
        public double MovedMm()
        {
            return SettledMm - OnRailsMm;
        }
    }
}
