using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.Player;
using System.Collections.Generic;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetCardEntry;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetChoose;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetMinion;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetPlayer;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect
{
    public enum EffectInfoStage
    {
        //未开始
        UnStart,
        //选择宾语目标中
        ConfirmingObject,
        //已选择宾语目标
        ConfirmedObject,
        //选择目标中
        ConfirmingTarget,
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
        //代码
        public string code { get; set; }
        //描述
        public string description { get; set; }
        //目标集合
        public string[] targetSet { get; set; }
        //实例化目标集
        public List<TargetSet> targetSetList = new List<TargetSet>();
        //宾物体集合
        public string[] objectSet { get; set; }
        //实例化宾物体集合
        public List<TargetSet> objectSetList = new List<TargetSet>();

        //选择效果列表
        public string[] chooseEffectList { get; set; }
        //复合效果列表
        public string[] complexEffectList { get; set; }
        //是否需要展示
        public string whetherToshow { get; set; }
        //持续时间
        public EffectiveTime effectiveTime { get; set; }
        //影响类别
        //瞬间MOMENT，持续CONTINUE
        public string impactType { get; set; }
        //如果是持续，那么需要有触发时点和触发要求
        public string[] impactTimeTriggers { get; set; }
        //实例化的触发器避免重复获取
        public List<ImpactTimeTrigger> impactTimeTriggerList = new List<ImpactTimeTrigger>();
        //影响目标
        public string[] impactTargets { get; set; }
        //影响内容
        //数组 和目标一一对应
        public string[] impactContents { get; set; }
        

        //指定生物
        public EATargetMinionList TargetMinionList = null;
        //进行选择的效果
        public EATargetChooseGrid TargetChooseGrid = null;
        //指定玩家
        public EATargetPlayerList TargetPlayerList = null;
        //指定了某一张牌
        public EATargetCardEntryList TargetCardEntryList = null;


        //游戏运行中所需要进行的判断？
        //效果来源于哪一张卡
        public CardEntry cardEntry;
        //效果执行状态
        public EffectInfoStage effectInfoStage = EffectInfoStage.UnStart;
        //效果所有者
        public PlayerItem player;
        //效果选择者，当一个效果需要进行选择的时候使用
        public PlayerItem chooseByPlayer;
        //是否是反向执行，清除buff的时候可能会用上，默认是false
        public bool isReverse = false;




        public void CleanEffectTargetSetList() {
            foreach (TargetSet targetSet in targetSetList) {
                targetSet.CleanEffectTargetSetList();
            }
        
        }
   

        //检查一个效果是否可以作用与目标生物
        public bool checkEffectToTargetMinionCellItem(MinionCellItem minionCellItem) {
            bool isEffectTarget = true;
            //遍历条件
            foreach (TargetSet targetSet in this.targetSetList)
            {
                //条件
                string[] targetClaims = targetSet.targetClaims;
                //条件内容
                string[] targetClaimsContents = targetSet.targetClaimsContents;
               
                for (int n = 0; n < targetClaims.Length; n++)
                {
                    //判断所有权
                    if (targetClaims[n] == "Owner")
                    {
                        //是自己选
                        if (targetClaimsContents[n] == "Myself")
                        {
                            if (minionCellItem.playerCode != this.player.playerCode)
                            {
                                isEffectTarget = false;
                            }
                        }
                    }
                }

            }
            return isEffectTarget;
          
        }
       
      
    }
}
