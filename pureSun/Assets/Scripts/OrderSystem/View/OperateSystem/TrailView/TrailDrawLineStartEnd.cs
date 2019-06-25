using UnityEngine;
namespace Assets.Scripts.OrderSystem.View.OperateSystem.TrailView
{
    public class TrailDrawLineStartEnd : TrailDrawLine
    {

        private void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
               
                //播放声音
                //audioSource.Play();
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseDown = false;
                firstMouseUp = true;
            }
            onDrawLine();
            firstMouseDown = false;
        }
        private void onDrawLine()
        {
            if (firstMouseDown)
            {
                //先把计数器设为0
                posCount = 0;

                head = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                last = head;
                last.y = 20;
                positions[0] = last;

            }
            if (mouseDown)
            {
                last = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                last.y = 20;
                positions[1] = last;
            }
            else
            {
                if (firstMouseUp)
                {
                    firstMouseUp = false;
                    if (isAchieve)
                    {
                        OnMouseButtonUp();
                    }
                }
                positions = new Vector3[2];
                
            }
            changePositions(positions);
        }

        // 保存坐标点
        private void savePosition(Vector3 pos)
        {
            pos.y = 10;

            if (posCount <= 59)
            {
                for (int i = posCount; i <= 59; i++)
                {
                    positions[i] = pos;
                }
            }
            else
            {
                for (int i = 0; i < 59; i++)
                    positions[i] = positions[i + 1];

                positions[59] = pos;
            }
        }
        // 修改直线渲染器的坐标
        private void changePositions(Vector3[] positions)
        {
            lineRenderer.SetPositions(positions);
        }


    }
}

