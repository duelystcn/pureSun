using Assets.Scripts.OrderSystem.Model.Database.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OrderSystem.Model.Database.TestCase
{
    public class TestCaseInfo
    {
        public string name { get; set; }

        public PI_Player myselfPlayer { get; set; }

        public PI_Player enemyPlayer { get; set; }

        public string description { get; set; }

    }
}
