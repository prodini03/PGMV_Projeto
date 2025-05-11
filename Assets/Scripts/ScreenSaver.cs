using UnityEngine;

public class ScreenSaver : MonoBehaviour
{
    public Texture[] images;
    public float interval = 5f;
    public int screenMaterialIndex = 2; 

    private Renderer rend;
    private int currentIndex = 0;
    private float timer;
    private Material screenMaterialInstance;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (rend != null && images.Length > 0 && screenMaterialIndex < rend.materials.Length)
        {
            Material[] mats = rend.materials;
            screenMaterialInstance = new Material(mats[screenMaterialIndex]);
            mats[screenMaterialIndex] = screenMaterialInstance;
            rend.materials = mats;

            screenMaterialInstance.mainTexture = images[0];
        }
    }

    void Update()
    {
        if (screenMaterialInstance == null || images.Length == 0)
            return;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % images.Length;
            screenMaterialInstance.mainTexture = images[currentIndex];
        }
    }
}
