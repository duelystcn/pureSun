using Assets.Scripts.OrderSystem.Model.Database.Card;

namespace Assets.Scripts.OrderSystem.Model.OperateSystem
{
    public class OneUserSelectionItem
    {
        public string selectionText;

        public bool defaultAvailab;

        //是生效还是不生效
        public bool isExecute;

        //携带的卡
        public CardEntry cardEntry;
    }
}
