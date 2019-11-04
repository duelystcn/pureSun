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
        public void CreateVariableAttributeByOriginalValueAndCodeAndBetterAndAutoRestore(string valueCode, int originalValue, bool biggerThebetter, bool autoRestore) {
            VariableAttribute variableAttribute = new VariableAttribute();
            variableAttribute.valueCode = valueCode;
            variableAttribute.biggerThebetter = biggerThebetter;
            variableAttribute.autoRestore = autoRestore;
            variableAttribute.valueMap[VATtrtype.OriginalValue] = originalValue;
            variableAttribute.valueMap[VATtrtype.CurrentValue] = originalValue;
            variableAttribute.valueMap[VATtrtype.UpperLimitValue] = originalValue;
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
            if (!isReverse)
            {
                variableAttributeMap[valueCode].valueMap[VATtrtype.UpperLimitValue] += changeValue;
                variableAttributeMap[valueCode].valueMap[VATtrtype.CurrentValue] += changeValue;
            }
            else {
                if (changeValue >= 0)
                {
                    if (variableAttributeMap[valueCode].autoRestore)
                    {
                        variableAttributeMap[valueCode].valueMap[VATtrtype.UpperLimitValue] += changeValue;
                        variableAttributeMap[valueCode].valueMap[VATtrtype.CurrentValue] += changeValue;
                    }
                    else {
                        variableAttributeMap[valueCode].valueMap[VATtrtype.UpperLimitValue] += changeValue;
                    }
                }
                else
                {
                    variableAttributeMap[valueCode].valueMap[VATtrtype.UpperLimitValue] += changeValue;
                    //判断当前值是否大于上限，如果是，那么当前值要减到上限为止
                    if (variableAttributeMap[valueCode].valueMap[VATtrtype.CurrentValue] > 
                            variableAttributeMap[valueCode].valueMap[VATtrtype.UpperLimitValue]) {
                        variableAttributeMap[valueCode].valueMap[VATtrtype.CurrentValue] = variableAttributeMap[valueCode].valueMap[VATtrtype.UpperLimitValue];
                    }

                }
            }
           
          
        }
        //获取当前值
        public int GetValueByCodeAndType(string valueCode, VATtrtype vAType) {
            return variableAttributeMap[valueCode].valueMap[vAType];
        }

        //判断一个属性值是该提示绿色还是提示红色
        public string CheckCurrentValueIsBetterByCode(string valueCode)
        {
            //获取当前值
            int currentValue = variableAttributeMap[valueCode].valueMap[VATtrtype.CurrentValue];
            //获取上限
            int upperLimitValue = variableAttributeMap[valueCode].valueMap[VATtrtype.UpperLimitValue];
            //获取初始值
            int originalValue = variableAttributeMap[valueCode].valueMap[VATtrtype.OriginalValue];
            //判断当前值是否小于上限
            if (currentValue < upperLimitValue) {
                if (variableAttributeMap[valueCode].biggerThebetter == true)
                {
                    return "Bad";
                }
                else {
                    return "Good";
                }

            } else if (currentValue == upperLimitValue) {
                //判断当前值和初始值的大小
                if (originalValue < currentValue) {
                    if (variableAttributeMap[valueCode].biggerThebetter == true)
                    {
                        return "Good";
                    }
                    else
                    {
                        return "Bad";
                    }
                }
                else if (originalValue == currentValue) {
                    return "NoChange";
                }
                else{
                    if (variableAttributeMap[valueCode].biggerThebetter == true)
                    {
                        return "Bad";
                    }
                    else
                    {
                        return "Good";
                    }
                }

            } else{
                //按照逻辑，当前值不可能大于上限值
                UtilityLog.LogError("按照逻辑，当前值不可能大于上限值，值代码为【" + valueCode + "】");
                return "Error";
            }
        }


    }
}
