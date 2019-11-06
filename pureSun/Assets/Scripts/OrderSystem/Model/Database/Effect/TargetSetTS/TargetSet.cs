using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS
{
    public class TargetSet
    {
        //名称
        public string name { get; set; }
        //描述
        public string description { get; set; }

        public string target { get; set; }

        public string targetSource { get; set; }

        //目标要求
        //所有者
        //目标数量
        //Owner
        //--Myself
        //--Enemy
        public string[] targetClaims { get; set; }

        public string[] targetClaimsContents { get; set; }

        public int targetClaimsNums { get; set; }

        public bool hasTarget;

        //执行对象
        //目前可能存在的目标？卡（墓地，牌组，手牌）,生物，玩家，效果（选择性发动的效果）
        public List<CardEntry> targetCardEntries = new List<CardEntry>();
        public List<MinionCellItem> targetMinionCellItems = new List<MinionCellItem>();
        public List<PlayerItem> targetPlayerItems = new List<PlayerItem>();
        public List<EffectInfo> targetEffectInfos = new List<EffectInfo>();

        //执行对象全部初始化，持续性效果每次触发前都应该初始化一次
        public void CleanEffectTargetSetList() {
            targetCardEntries.Clear();
            targetMinionCellItems.Clear();
            targetPlayerItems.Clear();
            targetEffectInfos.Clear();
        }
    }
}
