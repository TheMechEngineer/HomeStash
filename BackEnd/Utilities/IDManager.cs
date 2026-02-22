using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Utilities
{
    public static class IDManager
    {
        private static int CurrentID = 0;

        public static int GetNextID()
        {
            CurrentID++;
            return CurrentID;
        }

        public static void SetCurrentID(int id)
        {
            //int maxRoomID = rooms.Max(r => r.ID);
            //int maxContainerID = containers.Max(c => c.ID);
            //int maxItemID = items.Max(i => i.ID);

            //CurrentID = Math.Max(maxRoomID, Math.Max(maxContainerID, maxItemID));
        }
    }
}
