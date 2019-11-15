using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect.EffectCompent;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Player;
using System.Collections.Generic;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetChoose;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EffectExecutionAction;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect
{
    public enum EffectInfoStage
    {
        //未开始
        UnStart,
        //选择目标中
        ConfirmingTarget,
        //已选择目标
        ConfirmedTarget,
        //询问用户操作
        AskTheUser,
        //询问完毕
        AskTheUserOver,
        //执行中
        Executing,
        //执行完毕
        Finished
    }

    public enum EffectExeType
    {
        //执行
        Execute,
        //检查
        Check
    }


    public class EffectInfo
    {
        //名称
        public string name { get; set; }
        //代码
        public string code { get; set; }
        //类型
        public string effectType { get; set; }

        //描述
        public string description { get; set; }


        //是否需要用户确认发动
        public string mustBeLaunched { get; set; }

        //效果执行完毕发布信号
        public string[] impactTimeTriggertMonitorListWhenOver { get; set; }

        public bool playerDecisionLaunched;


        public EffectOperationalTarget operationalTarget { get; set; }

        public EffectOperationalContent operationalContent { get; set; }




        //选择效果列表
        public string[] chooseEffectList { get; set; }


        //检查是否可以发动的结果
        public bool checkCanExecution;

        //前置效果列表
        public string[] preEffectList { get; set; }
        //实例化前置效果列表
        public List<EffectInfo> preEffectEntryList = new List<EffectInfo>();
        //需要进行用户判断的前置效果
        public EffectInfo needChoosePreEffect;

        //用户的判断结果
        public bool userChooseExecution;



        //后置效果列表
        public string[] postEffectList { get; set; }
        //实例化前置效果列表
        public List<EffectInfo> postEffectEntryList = new List<EffectInfo>();

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



        //用户当前需要进行选择的targetSet
        public TargetSet needPlayerToChooseTargetSet;





        //public EffectExecution effectExecution = null;


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
            if (this.operationalTarget != null) {
                operationalTarget.CleanEffectTargetSetList();
            }
        }
   

       
       
      
    }
}
