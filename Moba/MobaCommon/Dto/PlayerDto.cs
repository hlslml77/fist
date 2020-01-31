using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto
{
    public class PlayerDto
    {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int winCount;
        public int loseCount;
        public int runCount;
        public int[] heroIds;
        public Friend[] friends;

        public PlayerDto()
        {

        }
    }
}
