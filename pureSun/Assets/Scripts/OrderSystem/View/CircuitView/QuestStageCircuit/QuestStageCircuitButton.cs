

using Assets.Scripts.OrderSystem.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.CircuitView.QuestStageCircuit
{
    public class QuestStageCircuitButton : MonoBehaviour
    {
        public UnityAction OnClick = null;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleInput();
            }
        }

        void HandleInput()
        {
            //需要一个tag为MainCanmera的Camera
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                TouchButton(hit.point);
            }
        }

        public void TouchButton(Vector3 position)
        {
            //判断是否点击了按钮
            position = transform.InverseTransformPoint(position);
            bool check = GeometricUtil.CheckPointInRectangle(2f,2f,position);
            if (check) {
                OnClick();
            }
        }
    }
}
