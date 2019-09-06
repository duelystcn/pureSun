using Assets.Scripts.OrderSystem.Common.UnityExpand;
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
            if (inUse) {
                onDrawLine();
            }
            
            firstMouseDown = false;
        }
        private void onDrawLine()
        {
            if (firstMouseDown)
            {
                //先把计数器设为0
                posCount = 0;
               // var ray = camera.ScreenPointToRay(Input.mousePosition).origin;
                head = UICamera.ScreenToWorldPoint(Input.mousePosition);
                last = head;
                last.y = 10;
                positions[0] = last;

            }
            if (mouseDown)
            {
                last = UICamera.ScreenToWorldPoint(Input.mousePosition);
                //Vector3 overVec = Input.mousePosition;
                //overVec.y = 10;
                //Debug.Log(TempC.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)));
                //UtilityLog.Log(BTCamera.ScreenToWorldPoint(overVec));
                last.y = 10;
                positions[1] = last;
            }
            else
            {

                if (firstMouseUp)
                {
                    inUse = false;
                    Ray ray = BTCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        GameObject gameobj = hit.collider.gameObject;
                        if (gameobj.tag == "HexCell")
                        {
                            overVec = hit.point;
                        }
                    }
                    else {
                        overVec = new Vector3(0, 0, -9999);
                    }

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

