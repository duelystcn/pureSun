using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.OperateSystem
{
    public class OperateSystemItem
    {
        public enum OperateType { Close, HandUse, CardSettle };

        //当前模式
        public OperateType operateModeType = OperateType.Close;
        //暂定？上一个模式
        //进入卡牌结算模式后，需要判断是否能结算成功，如果不能结算成功需要返回上一个模式，进行特殊处理？

        //效果结算？写在这里吧
        //正在结算的卡
        public CardEntry cardEntry;


        //正在结算的效果集合
        public List<EffectInfo> effectInfos;


        //正在结算的效果
        public EffectInfo effectInfo;


        public HandCellItem onChooseHandCellItem
        {
            get;  set;
        }
        //当前操作用户
        public PlayerItem playerItem
        {
            get;  set;
        }
        //
    }
}
