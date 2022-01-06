using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PixelSprite : MonoBehaviour
{

    public Palette palette;
    public Texture2D NormalMap;
    public float h = 0;
    public float s = 1;
    public float v = 0;
    public float time = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetProperties(Texture2D mainTex,Palette palette,float hue,float saturation,float value, Material mat)
    {
        this.palette = palette;
        h = hue;
        s = saturation;
        v = value;
        // set renderer
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sharedMaterial = mat;
        Rect rect = new Rect(0, 0, mainTex.width, mainTex.height);
        mainTex.hideFlags = HideFlags.NotEditable;
        sr.sprite = Sprite.Create(mainTex, rect, rect.center);
        UpdateSR();
    }

    private void UpdateSR()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        MaterialPropertyBlock properties = new MaterialPropertyBlock();
        sr.GetPropertyBlock(properties);
        properties.SetTexture("_MaskTex", palette.Texture);
        properties.SetVector("HSV", new Vector3(h, s, v));
        properties.SetFloat("Time", time);
        properties.SetInt("Height", palette.Height);
        sr.SetPropertyBlock(properties);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSR();
    }
}
