using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetChoose;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetMinion;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect
{
    public class EffectInfo
    {
        //名称
        public string name { get; set; }
        //描述
        public string description { get; set; }
        //类型
        //单体生物 ONE_MINION 
        //选择一项 CHOOSE_ONE
        public string type { get; set; }

        //选择效果列表
        public string[] chooseEffectList { get; set; }
        //复合效果列表
        public string[] complexEffectList { get; set; }


        //目标要求
        //
        public string[] targetClaims { get; set; }
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


        
    }
}
