/*************************************************************************************
     * 类 名 称：       VariableAttribute
     * 文 件 名：       VariableAttribute
     * 创建时间：       2019-08-14
     * 作    者：       chenxi
     * 说   明：        这个类是希望实现类似于生命，攻击，费用这些可变化值的变动范围，从规律上来说这些数值的变动都是一样的
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Common
{
    //初始值，当前值，上限值
    public enum VATtrtype { OriginalValue, CurrentValue, UpperLimitValue };
    public class VariableAttribute
    {
        //code
        public string valueCode;

        //因为攻击生命是越大越好，费用是越低越好，所以这里希望可以有一个属性区分
        public bool biggerThebetter;

        //是否会自动回复，生命不会自动回复，但是攻击会
        public bool autoRestore;

        public Dictionary<VATtrtype, int> valueMap = new Dictionary<VATtrtype, int>();



    }
}
