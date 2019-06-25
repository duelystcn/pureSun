
using Newtonsoft.Json;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect
{
    public class EffectInfoProxy : Proxy
    {
        public new const string NAME = "EffectInfoProxy";
        public EffectSysItem effectSysItem
        {
            get { return (EffectSysItem)base.Data; }
        }
        public EffectInfoProxy() : base(NAME)
        {
            EffectSysItem effectSysItem = new EffectSysItem();
            base.Data = effectSysItem;
            LoadCardDbByJson();
        }
        //读取JSON文件配置
        public void LoadCardDbByJson()
        {
            string jsonStr = File.ReadAllText("Assets/Resources/Json/EffectDb.json");
            effectSysItem.effectInfoMap =
                JsonConvert.DeserializeObject<Dictionary<string, EffectInfo>>(jsonStr);
            //初始化效果
            foreach (EffectInfo effectInfo in effectSysItem.effectInfoMap.Values)
            {
                effectSysItem.EffectActionReady(effectInfo);

            }

        }

    }
}
