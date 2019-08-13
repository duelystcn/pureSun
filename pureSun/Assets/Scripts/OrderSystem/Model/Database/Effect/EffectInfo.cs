using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.Player;
using System.Collections.Generic;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetChoose;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetMinion;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetPlayer;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect
{
    public enum EffectInfoStage
    {
        //未开始
        UnStart,
        //选择目标中
        Confirming,
        //已选择目标
        ConfirmedTarget,
        //执行中
        Executing,
        //执行完毕
        Finished
    }

    public class EffectInfo
    {
        //名称
        public string name { get; set; }
        //描述
        public string description { get; set; }

        public string target { get; set; }

        //选择效果列表
        public string[] chooseEffectList { get; set; }
        //复合效果列表
        public string[] complexEffectList { get; set; }


        //目标要求
        //所有者
        //Owner
        //--Myself
        //--Enemy
        public string[] targetClaims { get; set; }

        public string[] targetClaimsContents { get; set; }

        //影响类别
        //瞬间MOMENT，持续CONTINUE
        public string impactType { get; set; }
        //影响目标
        //数组，生命DEF,攻击ATK
        public string[] impactTargets { get; set; }
        //影响内容
        //数组 和目标一一对应
        public string[] impactContents { get; set; }
        

        //指定一个生物
        public EATargetMinionOne TargetMinionOne = null;
        //进行选择
        public EATargetChooseGrid TargetChooseGrid = null;
        //指定一个玩家
        public EATargetPlayerList TargetPlayerList = null;



        //游戏运行中所需要进行的判断？
        //效果来源于哪一张卡
        public CardEntry cardEntry;
        //效果执行状态
        public EffectInfoStage effectInfoStage = EffectInfoStage.UnStart;
        //效果所有者
        public PlayerItem player;
        //效果选择者，当一个效果需要进行选择的时候使用
        public PlayerItem chooseByPlayer;

        //执行对象
        //目前可能存在的目标？卡（墓地，牌组，手牌）,生物，玩家，效果（选择性发动的效果）
        public List<CardEntry> TargetCardEntries = new List<CardEntry>();
        public List<MinionCellItem> TargetMinionCellItems = new List<MinionCellItem>();
        public List<PlayerItem> TargetPlayerItems = new List<PlayerItem>();
        public List<EffectInfo> TargetEffectInfos = new List<EffectInfo>();


       
      
    }
}
