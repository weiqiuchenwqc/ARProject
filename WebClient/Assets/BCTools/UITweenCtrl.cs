using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UITweenCtrl : MonoBehaviour
{
    private CanvasGroup cv = null;

    public BCTweenPosition[] bcPoses = null;
    public BCTweenRotation[] bcRots = null;
    public BCTweenScale[] bcScales = null;
    public BCTweenAlpha[] bcAlphas = null;
    public BCTweenColor[] bcColors = null;

    void Awake()
    {
        //OnEnable();
        cv = this.gameObject.GetComponent<CanvasGroup>();
        if(null == cv)
            cv = this.gameObject.AddComponent<CanvasGroup>();

        cv.alpha = 0.0f;
    }

    void OnEnable()
    {
        cv.alpha = 0.0f;
        Play(bcPoses);
        Play(bcRots);
        Play(bcScales);
        Play(bcAlphas);
        Play(bcColors);
    }

    private void Play(BCUITweener[] tweens)
    {
        if (null != tweens)
        {
            for (int i = 0; i < tweens.Length; ++i)
            {
                if (tweens[i] != null)
                {
                    StartCoroutine(DelayPlay(tweens[i].PlayForwardForce));
                }
            }
        }
    }

    IEnumerator DelayPlay(Action act, bool delay = false)
    {
        yield return null;
        cv.alpha = 1.0f;
        if (act != null)
        {
            act();
        }
        yield return null;
    }
	

    [ContextMenu("Fix Tweens")]
    public void FixTweens()
    {
        bcPoses = GetComponentsInChildren<BCTweenPosition>();
        bcRots = GetComponentsInChildren<BCTweenRotation>();
        bcScales = GetComponentsInChildren<BCTweenScale>();
        bcAlphas = GetComponentsInChildren<BCTweenAlpha>();
        bcColors = GetComponentsInChildren<BCTweenColor>();

        Fix(bcPoses);
        Fix(bcRots);
        Fix(bcScales);
        Fix(bcAlphas);
        Fix(bcColors);
    }

    private void Fix(BCUITweener[] tweens)
    {
        if (null != tweens)
        {
            for (int i = 0; i < tweens.Length; ++i)
            {
                if (tweens[i] != null)
                {
                    tweens[i].enabled = false;
                }
            }
        }
    }
}
