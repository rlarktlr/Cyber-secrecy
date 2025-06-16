using System;
using System.Collections;
using UnityEngine;

public static class UIAnimationUtil
{
    #region Scale
    public static void PlayScaleIn(MonoBehaviour host, GameObject root, Transform target, float duration = 0.1f)
    {
        root.SetActive(true);
        target.localScale = Vector3.zero;

        host.StartCoroutine(UpdateAnimation(
            duration, 0, 1,
            v => target.localScale = Vector3.one * v,
            () => target.localScale = Vector3.one
        ));
    }

    public static void PlayScaleOut(MonoBehaviour host, GameObject root, Transform target, float duration = 0.05f)
    {
        target.localScale = Vector3.one;

        host.StartCoroutine(UpdateAnimation(
            duration, 1, 0,
            (v) => target.localScale = Vector3.one * v,
            () =>
            { target.localScale = Vector3.zero; root.SetActive(false); }
        ));
    }
    #endregion
    #region Fade
    public static void PlayFadeIn(MonoBehaviour host, GameObject root, CanvasGroup cg, float duration = 0.1f)
    {
        root.SetActive(true);
        cg.alpha = 0f;

        host.StartCoroutine(UpdateAnimation(
            duration, 0, 1,
            (v) => cg.alpha = v,
            () => cg.alpha = 1f
        ));
    }

    public static void PlayFadeOut(MonoBehaviour host, GameObject root, CanvasGroup cg, float duration = 0.05f)
    {
        cg.alpha = 1f;

        host.StartCoroutine(UpdateAnimation(
            duration, 1, 0, 
            (v) => cg.alpha = v,
            () => { cg.alpha = 0f; root.SetActive(false); }
        ));
    }
    #endregion

    static IEnumerator UpdateAnimation(float duration, float from, float to, Action<float> onUpdate, Action onComplete = null)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float v = Mathf.SmoothStep(from, to, t / duration);
            onUpdate?.Invoke(v);
            yield return null;
        }

        onComplete?.Invoke();
    }
}
