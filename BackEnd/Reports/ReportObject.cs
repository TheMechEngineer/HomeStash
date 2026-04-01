namespace BackEnd.Reports
{
    /// <summary>
    /// Represents A Single Item Or Container For Reporting Purposes
    /// </summary>
    internal class ReportObject
    {
        /// <summary>
        /// The Name Of The Item
        /// </summary>
        required internal string Name;

        /// <summary>
        /// The Location Of The Item (Building, Room, Or Container)
        /// </summary>
        required internal string Location;

        /// <summary>
        /// The Description Of The Item
        /// </summary>
        required internal string Description;

        /// <summary>
        /// The Monetary Value Of The Item
        /// </summary>
        required internal double Value;

        /// <summary>
        /// The Quantity Of The Item At The Specified Location
        /// </summary>
        required internal int Quantity;
    }
}
