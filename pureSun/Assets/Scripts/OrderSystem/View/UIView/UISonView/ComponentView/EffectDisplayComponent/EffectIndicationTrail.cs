

using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.EffectDisplayComponent
{
    public class EffectIndicationTrail : MonoBehaviour
    {
        /// <summary>
        /// 直线渲染器--鼠标轨迹-开始-结束
        /// </summary>
        [SerializeField]
        public LineRenderer lineRenderer;
        // 修改直线渲染器的坐标
        public void changePositions(Vector3[] positions)
        {
            lineRenderer.SetPositions(positions);
        }
    }
}
