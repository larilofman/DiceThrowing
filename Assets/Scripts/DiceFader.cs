using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiceFader : MonoBehaviour
{
    private MeshRenderer mRenderer;

    void Start()
    {
        mRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();
        StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        Color startColor = mRenderer.material.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        float fadeTime = GlobalSettings.Instance.bonusDiceVanishTime;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            float step = elapsedTime / fadeTime;
            mRenderer.material.color = Color.Lerp(startColor, endColor, step);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
