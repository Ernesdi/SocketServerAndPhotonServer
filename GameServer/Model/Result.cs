using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    class Result
    {
        public int Id;
        public int UserID;
        public int TotalCount;
        public int WinCount;


        public Result(int Id, int UserID, int TotalCount, int WinCount)
        {
            this.Id = Id;
            this.UserID = UserID;
            this.TotalCount = TotalCount;
            this.WinCount = WinCount;
        }

    }
}
