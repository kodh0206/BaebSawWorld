using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
     // Start is called before the first frame update
    private void Awake()
    {
        Camera camera =GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleHeight =((float)Screen.width/Screen.height)/((float)9/16); //(가로/세로)
        float scaleWidth = 1f/scaleHeight;
        if(scaleHeight<1){
            rect.height =scaleHeight;
            rect.y =(1f-scaleHeight)/2f;

        }
        else
        {
            rect.width =scaleWidth;
            rect.x=(1f-scaleWidth)/2f;
        }
        camera.rect =rect;
    }
         /// <summary>
        /// OnPreCull is called before a camera culls the scene.
        /// </summary>
        void OnPreCull() => GL.Clear(true,true, Color.black);
    // Update is called once per frame
}
