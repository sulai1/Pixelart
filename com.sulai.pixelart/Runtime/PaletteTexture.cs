using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "new PaletteTexture"),Serializable]
public class PaletteTexture : ScriptableObject
{
    public Palette palette;
    [SerializeField,HideInInspector]
    private Texture texture;

    public Texture Texture { get => texture; set { if (texture == null) texture = value; } }
}
