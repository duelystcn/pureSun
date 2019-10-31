using UnityEngine;
using System.Collections;

public class UIFollow : MonoBehaviour
{
    public GameObject target; public Camera worldCamera; public Camera guiCamera;
    void Awake() { }
    public void LateUpdate()
    {
       
        worldCamera = FindCameraForLayer(target.layer);
        guiCamera = FindCameraForLayer(gameObject.layer);
        Vector3 pos = worldCamera.WorldToViewportPoint(target.transform.position);
        if (pos.z >= 0) {
            pos = guiCamera.ViewportToWorldPoint(pos);
            pos.z = 0;
            transform.position = pos;
        } else {
            pos = guiCamera.ViewportToWorldPoint(pos);
            pos.z = guiCamera.farClipPlane + 10f;
            transform.position = pos;
        }
    }
    static public Camera FindCameraForLayer(int layer)
    {

        int layerMask = 1 << layer;

        Camera cam;

      
        return null;

    }
}