using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launch : MonoBehaviour
{
    [SerializeField]
    private RawImage cameraImage;

    [SerializeField]
    private Text t_tips;
    private RectTransform rectTransform;
    private WebCamTexture webCamTex;
    private int lastRotationAngle;
    private int index = 0;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        t_tips.text = "v1.0.3\n";
        rectTransform = GetComponent<RectTransform>();

        // 请求摄像头权限
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        // 如果获取到摄像头权限
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            // 获取所有的摄像头设备
            for (int i = 0; i < 100; i++)
            {
                WebCamDevice[] devices = WebCamTexture.devices;

                
                if (devices != null && devices.Length > 0)
                {
                    //t_tips.text += "No Rear Camera\n";
                    index = devices.Length > 1 ? 1 : 0;

                    // 索引为1的摄像头一般为后置摄像头，参数分别为设备名称、图像宽度、高度、刷新率
                    webCamTex = new WebCamTexture(devices[index].name, 720, 1280, 60);
                    // 实时获取摄像头的画面
                    webCamTex.Play();

                    cameraImage.texture = webCamTex;

                    cameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTex.videoRotationAngle);

                    break;
                }
               
                yield return new WaitForEndOfFrame();
            }

            if(WebCamTexture.devices == null || WebCamTexture.devices.Length <= 0)
            {
                t_tips.text += "No Cameras\n";
            }
        }
    }

    //切换前后摄像头
    public void SwitchCamera()
    {
        if (WebCamTexture.devices.Length < 1)
            return;

        if (webCamTex != null)
            webCamTex.Stop();

        index++;
        index = index % WebCamTexture.devices.Length;
        t_tips.text += "index:" + index + "," + WebCamTexture.devices[index].depthCameraName + "," + WebCamTexture.devices[index].kind;
        t_tips.text += "\n";

        // 创建相机贴图
        webCamTex = new WebCamTexture(WebCamTexture.devices[index].name, 720, 1280, 60);
        cameraImage.texture = webCamTex;
        webCamTex.Play();

        //前置后置摄像头需要旋转一定角度，否则画面是不正确的,必须置于Play()函数后
        cameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTex.videoRotationAngle);
    }



    private void OnApplicationQuit()
    {
        webCamTex?.Stop();
    }
}
