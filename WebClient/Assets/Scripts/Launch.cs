using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launch : MonoBehaviour
{
    [SerializeField]
    protected RawImage cameraImage;

    [SerializeField]
    protected Text t_tips;

    [SerializeField] 
    protected Image img_sel;

    [SerializeField]
    protected Text t_name;

    private WebCamTexture webCamTex;
    private int index = 0;

    private float aspect = 9f / 16f;

    [SerializeField]
    public GameObject lastCardGo;

    protected int mode = 0;
    void Start()
    {
        this.OnStart();
    }

    protected virtual void OnStart()
    {
        try
        {
            StartCoroutine(Cor_LaunchMobileCamera());
        }
        catch (System.Exception e)
        {
            Log("err message:" + e.Message + "\n" + e.StackTrace);
        }
    }

    private IEnumerator Cor_LaunchMobileCamera()
    {
        yield return new WaitForEndOfFrame();
        Log("v1.0.17");
        Log("Screen:" + Screen.width + "x" + Screen.height);

        //Screen.SetResolution(Screen.width, Screen.height,0);
        //float ratio = (float)Screen.width / (float)Screen.height;

     
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


                    webCamTex = new WebCamTexture(devices[index].name, Screen.width, Screen.height, 60);
                    // 实时获取摄像头的画面
                    webCamTex.Play();

                    cameraImage.texture = webCamTex;


                    cameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, webCamTex.videoRotationAngle);

                    yield return new WaitForEndOfFrame();

                    //cameraImage.rectTransform.sizeDelta = new Vector2(Screen.width , Screen.height);

                    //Log("wh1:" + webCamTex.width + "," + webCamTex.height);
                    //Log("wh2:" + webCamTex.requestedWidth + "," + webCamTex.requestedHeight);
                    float ratio = (float)Screen.width / (float)Screen.height;
                    cameraImage.rectTransform.sizeDelta = new Vector2(Screen.width + Mathf.Clamp(Screen.width * (ratio - aspect) * 10, 0, (float)Screen.width / 2), Screen.height);

                    Log("rawImage:" + cameraImage.rectTransform.sizeDelta.x + "," + cameraImage.rectTransform.sizeDelta.y);


                    break;
                }

                yield return new WaitForEndOfFrame();
            }

            if (WebCamTexture.devices == null || WebCamTexture.devices.Length <= 0)
            {
                Log("No Cameras");
#if UNITY_EDITOR
                float ratio = (float)Screen.width / (float)Screen.height;
                cameraImage.rectTransform.sizeDelta = new Vector2(Screen.width + Mathf.Clamp(Screen.width * (ratio - aspect) * 10,0,(float)Screen.width / 2), Screen.height);
                Log("rawImage:" + cameraImage.rectTransform.sizeDelta.x + "," + cameraImage.rectTransform.sizeDelta.y);
#endif
            }

        }

        this.mode = 1;

    }

    protected virtual void Log(string str)
    {
        t_tips.text += str + "\n";
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

        // 创建相机贴图
        webCamTex = new WebCamTexture(WebCamTexture.devices[index].name, 0, 0, 60);
        cameraImage.texture = webCamTex;
        webCamTex.Play();

        //前置后置摄像头需要旋转一定角度，否则画面是不正确的,必须置于Play()函数后
        cameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTex.videoRotationAngle);
    }


    public void ChangeCardGo(GameObject cardGo)
    {
        if(this.lastCardGo != null)
        {
            this.lastCardGo.SetActive(false);
        }

        if (cardGo != null)
        {
            cardGo.SetActive(true);
            this.lastCardGo = cardGo;
        }

       
    }

    public void SetSelPos(GameObject selPos)
    {
        if (this.img_sel != null)
        {
            this.img_sel.transform.localPosition = selPos.transform.localPosition + Vector3.up * 20;
        }
    }

    public void OnShow(GameObject go)
    {
        go?.SetActive(true);
    }

    public void OnHide(GameObject go)
    {
        go.SetActive(false);
    }

    public void SetCardName(string name)
    {
        if (t_name != null)
        {
            t_name.text = name;
        }
    }

    public void SwithMode(int mode)
    {
        if(this.mode == mode)
        {
            return;
        }

        if(this.mode == 1 && mode == 2)
        {
            this.mode = 2;
            webCamTex?.Stop();
            this.cameraImage.texture = null;
        }
        else if(this.mode == 2 && mode == 1)
        {
            this.OnStart();
        }
    }
    

    private void OnApplicationQuit()
    {
        webCamTex?.Stop();
    }
}
