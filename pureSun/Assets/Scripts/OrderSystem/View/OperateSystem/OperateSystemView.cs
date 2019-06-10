

using Assets.Scripts.OrderSystem.View.OperateSystem.TrailView;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.OperateSystem
{
    public class OperateSystemView : MonoBehaviour
    {
        public TrailDrawLine trailDrawLine;
        public TrailDrawLine trailDrawLinePrefab;
        //初始化
        public void AchieveOperateSystemView()
        {
            trailDrawLine = Instantiate<TrailDrawLine>(trailDrawLinePrefab);
            trailDrawLine.transform.SetParent(transform, false);
            trailDrawLine.transform.localPosition = new Vector3(0,0,0);
            trailDrawLine.isAchieve = true;
        }

    }
}
