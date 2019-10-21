
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    public class HandControlView : MonoBehaviour
    {
        public HandGridView gridViewPrefab;

        public Dictionary<string,HandGridView> handGridViewMap = new Dictionary<string, HandGridView>();

        public void CreateHandGridViewByPlayerCode(string playerCode,bool isMyself) {
            //创建一个实例
            HandGridView handGridView = Instantiate<HandGridView>(gridViewPrefab);
            handGridView.transform.SetParent(transform, false);
            Vector3 position = new Vector3();
            if (isMyself)
            {
                position.x = -270;
                position.y = -200;
                position.z = 0;
            }
            else {
                position.x = -190;
                position.y = 380;
                position.z = 0;
                handGridView.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
            }     
            handGridView.transform.localPosition = position;
            handGridView.myself = isMyself;
            handGridViewMap.Add(playerCode, handGridView);

        }

        

    }
}
