using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Material hitMaterial;
    [SerializeField] private Color[] ignitedColor;
    [SerializeField] private Color[] shockColor;
    [SerializeField] private Color chillColor;
    private Color currentColor;
    private Material originMaterial;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        currentColor = Color.white;
        originMaterial = sr.material;
        ignitedColor[0] = new Color(1, 0.48f, 0.48f, 1);
        ignitedColor[1] = new Color(.96f, .65f, .65f, 1);
        chillColor = new Color(.15f, .4f, .8f, 1);
        shockColor[0] = new Color(.9f, 0.70f, 0.12f, 1);
        shockColor[1] = new Color(.75f, 0.70f, 0.70f, 1);
    }

    public IEnumerator FlashFX()
    {
        sr.color = Color.white;
        sr.material = hitMaterial;

        yield return new WaitForSeconds(.2f);
        sr.material = originMaterial;
        sr.color = currentColor;
    }

    private void FlashBlink()
    {
        if (sr.material != originMaterial)
            sr.material = originMaterial;
        else
            sr.material = hitMaterial;
    }

    private void CancleColorChange()
    {
        CancelInvoke();
        sr.material = originMaterial;
        currentColor = Color.white;
        sr.color = Color.white;

    }

    public void IgniteFXFor(float _duration, float frequency)
    {
        InvokeRepeating("shockColor", .2f, frequency);
        Invoke("CancleColorChange", _duration);
    }

    public void ShockFXFor(float _duration, float frequency)
    {
        InvokeRepeating("ShockColorFX", .2f, frequency);
        Invoke("CancleColorChange", _duration);
    }

    public void ChillFXFor(float _duration)
    {
        Invoke("ChillColorFX", .2f);
        Invoke("CancleColorChange", _duration);
    }

    private void IgniteColorFX()
    {
        if (sr.color == ignitedColor[0])
            currentColor = ignitedColor[1];
        else
            currentColor = ignitedColor[0];

        sr.color = currentColor;

    }

    private void ShockColorFX()
    {
        if (sr.color == shockColor[0])
            currentColor = shockColor[1];
        else
            currentColor = shockColor[0];

        sr.color = currentColor;

    }

    private void ChillColorFX()
    {

        sr.color = chillColor;
        currentColor = chillColor;
    }
}
