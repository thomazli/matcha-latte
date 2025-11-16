using UnityEngine;

public class SurfacePainter : MonoBehaviour
{
    public Color paintColor = Color.red;
    public int brushSize = 10;
    private Texture2D paintTexture;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        Texture2D src = rend.material.mainTexture as Texture2D;

        // Create writable runtime copy of the main texture
        paintTexture = new Texture2D(src.width, src.height, TextureFormat.RGBA32, false);
        paintTexture.SetPixels(src.GetPixels());
        paintTexture.Apply();

        rend.material.mainTexture = paintTexture;
    }

    public void PaintAtUV(Vector2 uv)
    {
        int x = (int)(uv.x * paintTexture.width);
        int y = (int)(uv.y * paintTexture.height);
        
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (i * i + j * j <= brushSize * brushSize) // circular brush
                {
                    int px = x + i;
                    int py = y + j;

                    if (px >= 0 && px < paintTexture.width && py >= 0 && py < paintTexture.height)
                        paintTexture.SetPixel(px, py, paintColor);
                }
            }
        }

        paintTexture.Apply();
    }
}