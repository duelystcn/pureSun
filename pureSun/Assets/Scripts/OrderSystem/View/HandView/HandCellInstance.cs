
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    //暂时弃用，以后改了贴图再用这个
    public class HandCellInstance : MonoBehaviour
    {
        public Image image;//获取图片组件，设置图片
        public Sprite[] img;
        /// 设置卡牌图片 
        public void SetImage()
        {
            image.sprite = img[0];
        }
    }
}
