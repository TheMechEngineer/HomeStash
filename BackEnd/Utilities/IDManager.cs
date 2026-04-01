namespace BackEnd.Utilities
{
    /// <summary>
    /// NOTE: This Class Is Not Currently In Use In The Program.
    /// 
    /// Provides A Simple Mechanism For Generating Unique IDs.
    /// </summary>
    public static class IDManager
    {
        /// <summary>
        /// The Current ID Counter
        /// </summary>
        private static int CurrentID = 0;

        /// <summary>
        /// Retrieves The Next Unique ID
        /// </summary>
        /// <returns>An Incremented Integer ID</returns>
        public static int GetNextID()
        {
            CurrentID++;
            return CurrentID;
        }

        /// <summary>
        /// Sets The Current ID To A Specific Value
        /// </summary>
        /// <param name="id">The ID To Set As Current</param>
        public static void SetCurrentID(int id)
        {
            //int maxRoomID = rooms.Max(r => r.ID);
            //int maxContainerID = containers.Max(c => c.ID);
            //int maxItemID = items.Max(i => i.ID);

            //CurrentID = Math.Max(maxRoomID, Math.Max(maxContainerID, maxItemID));
        }
    }
}
