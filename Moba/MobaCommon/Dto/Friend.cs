using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto
{
    public class Friend
    {
        public int Id;
        public string Name;
        public bool IsOnLine;

        public Friend()
        {

        }

        public Friend(int id, string name, bool online)
        {
            this.Id = id;
            this.Name = name;
            this.IsOnLine = online;
        }
    }
}
