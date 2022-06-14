using System.Globalization;
using UnityEngine;
 
public class PointCloud : MonoBehaviour
{
    public Material material;
    protected int number = 512 * 512;
    protected ComputeBuffer positionbuffer;
    protected ComputeBuffer colorbuffer;
 
    void Start ()
    {
        string[] lines = System.IO.File.ReadAllLines(@"Assets\model.pts");

        Vector3[] pos = new Vector3[lines.Length];
        Color[] col = new Color[lines.Length];

        for(int i = 0; i < lines.Length; i++) {
            string[] numbers = lines[i].Split(null);
            float x = float.Parse(numbers[0], CultureInfo.InvariantCulture.NumberFormat);
            float z = float.Parse(numbers[1], CultureInfo.InvariantCulture.NumberFormat) - 20;
            float y = float.Parse(numbers[2], CultureInfo.InvariantCulture.NumberFormat);
            
            pos[i] = new Vector3(x, y, z);
            col[i] = new Color32(byte.Parse(numbers[3]), byte.Parse(numbers[4]), byte.Parse(numbers[5]), 255);
        }

        number = lines.Length;

        positionbuffer = new ComputeBuffer(lines.Length, sizeof(float) * 3, ComputeBufferType.Default);
        colorbuffer = new ComputeBuffer(lines.Length, sizeof(float) * 4, ComputeBufferType.Default);

        positionbuffer.SetData(pos);
        colorbuffer.SetData(col);
    }
   
    void OnRenderObject()
    {
        material.SetPass(0);
        material.SetBuffer("positionbuffer", positionbuffer);
        material.SetBuffer("colorbuffer", colorbuffer);
        Graphics.DrawProceduralNow(MeshTopology.Points, number, 1);
    }
    void OnDestroy()
    {
        positionbuffer.Release();
        colorbuffer.Release();
    }
}