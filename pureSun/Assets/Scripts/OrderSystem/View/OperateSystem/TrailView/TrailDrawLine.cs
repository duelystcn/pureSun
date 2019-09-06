

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.OperateSystem.TrailView
{
    public abstract class TrailDrawLine : MonoBehaviour
    {
        public Camera UICamera;

        public Camera BTCamera;

        //控制是否划线的开关
        public bool inUse = false;

        // 是否第一次鼠标按下
        public bool firstMouseDown = false;

        // 是否鼠标一直按下
        public bool mouseDown = false;
        //鼠标弹起
        public bool firstMouseUp = false;

        // 当前保存的坐标数量
        public int posCount = 0;
        // 代表这一帧鼠标的位置 就 头的坐标
        public Vector3 head;
        // 代表上一帧鼠标的位置
        public Vector3 last;
        //保存路径坐标
        public Vector3[] positions = new Vector3[2];
        /// <summary>
        /// 直线渲染器--鼠标轨迹-开始-结束
        /// </summary>
        [SerializeField]
        public LineRenderer lineRenderer;
        // 修改直线渲染器的坐标
        private void changePositions(Vector3[] positions)
        {
            lineRenderer.SetPositions(positions);
        }
        //是否已经初始化
        public bool isAchieve = false;
        //鼠标抬起时执行
        public UnityAction OnMouseButtonUp = null;

        //鼠标点击不再由划线脚本监听了，改为触发
        public void TrailDrawStart()
        {
            inUse = true;
            firstMouseDown = true;
            mouseDown = true;
            firstMouseUp = false;
          

        }
        
        // 代表上一帧鼠标的位置
        public Vector3 overVec;
    }
}
