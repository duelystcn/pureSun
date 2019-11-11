/*************************************************************************************
     * 类 名 称：       VariableAttribute
     * 文 件 名：       VariableAttribute
     * 创建时间：       2019-08-14
     * 作    者：       chenxi
     * 说   明：        这个类是用来管理可变属性，提供一些通用方法
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Common
{
    public class VariableAttributeMap
    {
        public Dictionary<string, VariableAttribute> variableAttributeMap = new Dictionary<string, VariableAttribute>();

        //创建一个可变属性，需要提供一个初始值和code
        public void CreateVariableAttributeByOriginalValueAndCodeAndBetterAndAutoRestore(string valueCode, int originalValue, bool biggerThebetter) {
            VariableAttribute variableAttribute = new VariableAttribute();
            variableAttribute.valueCode = valueCode;
            variableAttribute.biggerThebetter = biggerThebetter;
            variableAttribute.valueMap[VATtrtype.OriginalValue] = originalValue;
            variableAttribute.valueMap[VATtrtype.ChangeValue] = 0;
            variableAttribute.valueMap[VATtrtype.DamageValue] = 0;
            variableAttributeMap.Add(valueCode, variableAttribute);
        }

        //改变当前值
        public void ChangeValueByCodeAndType(string valueCode, VATtrtype vAType ,int changeValue) {
            variableAttributeMap[valueCode].valueMap[vAType] += changeValue;
        }
        //改变当前值按照逻辑
        public void ChangeValueByCodeAndTypeAndIsReverse(string valueCode, int changeValue, bool isReverse)
        {
            if (isReverse) {
                changeValue = -changeValue;
            }
            variableAttributeMap[valueCode].valueMap[VATtrtype.ChangeValue] += changeValue;
            
           
          
        }
        //获取当前值
        public int GetValueByCodeAndType(string valueCode, VATtrtype vAType) {
            return variableAttributeMap[valueCode].valueMap[VATtrtype.OriginalValue] + variableAttributeMap[valueCode].valueMap[VATtrtype.ChangeValue] + variableAttributeMap[valueCode].valueMap[VATtrtype.DamageValue];
        }

        //判断一个属性值是该提示绿色还是提示红色
        public string CheckCurrentValueIsBetterByCode(string valueCode)
        {
            //获取当前值
            int DamageValue = variableAttributeMap[valueCode].valueMap[VATtrtype.DamageValue];
            //获取变化值
            int ChangeValue = variableAttributeMap[valueCode].valueMap[VATtrtype.ChangeValue];
            //获取初始值
            int originalValue = variableAttributeMap[valueCode].valueMap[VATtrtype.OriginalValue];
            //判断是否已经受伤
            if (DamageValue < 0)
            {
                if (variableAttributeMap[valueCode].biggerThebetter == true)
                {
                    return "Bad";
                }
                else
                {
                    return "Good";
                }
            }
            else {
                if (ChangeValue > 0)
                {
                    if (variableAttributeMap[valueCode].biggerThebetter == true)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Bad";
                    }
                }
                else if (ChangeValue == 0)
                {
                    return "NoChange";
                }
                else {
                    if (variableAttributeMap[valueCode].biggerThebetter == true)
                    {
                        return "Bad";
                    }
                    else
                    {
                        return "Good";
                    }
                }
            }
        }


    }
}
