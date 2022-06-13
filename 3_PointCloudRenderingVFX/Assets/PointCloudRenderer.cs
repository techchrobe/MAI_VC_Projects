using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.VFX;

public class PointCloudRenderer : MonoBehaviour
{
    Texture2D texColor;
    Texture2D texPosScale;
    VisualEffect vfx;
    uint resolution = 2048;

    public float particleSize = 0.1f;
    bool toUpdate = false;
    uint particleCount = 0;

    private void Start() {
        vfx = GetComponent<VisualEffect>();

        string[] lines = System.IO.File.ReadAllLines(@"Assets\model.pts");
        Debug.Log(lines[0]);


        Vector3[] pos = new Vector3[lines.Length];
        Color[] col = new Color[lines.Length];
        for(int i = 0; i < lines.Length; i++) {
            string[] numbers = lines[i].Split(null);
            float x = float.Parse(numbers[0], CultureInfo.InvariantCulture.NumberFormat);
            float z = float.Parse(numbers[1], CultureInfo.InvariantCulture.NumberFormat) -20;
            float y = float.Parse(numbers[2], CultureInfo.InvariantCulture.NumberFormat);
            pos[i] = new Vector3(x, y, z);
            float r = byte.Parse(numbers[3]);
            float g = byte.Parse(numbers[4]);
            float b = byte.Parse(numbers[5]);
            col[i] = new Color32(byte.Parse(numbers[3]), byte.Parse(numbers[4]), byte.Parse(numbers[5]), 255);
        }

        SetParticles(pos, col);
    }

    private void Update() {
        if(toUpdate) {
            toUpdate = false;

            vfx.Reinit();
            vfx.SetUInt(Shader.PropertyToID("ParticleCount"), particleCount);
            vfx.SetTexture(Shader.PropertyToID("TexColor"), texColor);
            vfx.SetTexture(Shader.PropertyToID("TexPosScale"), texPosScale);
            vfx.SetUInt(Shader.PropertyToID("Resolution"), resolution);
        }
    }

    public void SetParticles(Vector3[] positions, Color[] color) {
        texColor = new Texture2D(positions.Length > (int)resolution ? (int)resolution : positions.Length, Mathf.Clamp(positions.Length / (int)resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        texPosScale = new Texture2D(positions.Length > (int)resolution ? (int)resolution : positions.Length, Mathf.Clamp(positions.Length / (int)resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        int texWidth = texColor.width;
        int texHeight = texColor.height;

        for(int y = 0; y < texHeight; y++) {
            for(int x = 0; x < texWidth; x++) {
                int index = x + y * texWidth;
                texColor.SetPixel(x, y, color[index]);
                var data = new Color(positions[index].x, positions[index].y, positions[index].z, particleSize);
                texPosScale.SetPixel(x, y, data);
            }
        }

        texColor.Apply();
        texPosScale.Apply();
        particleCount = (uint)positions.Length;
        toUpdate = true;
    }
}
