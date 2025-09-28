using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class PostProcessCamera : MonoBehaviour
{
    public Material PostProcessMat;
    public float debugBlur;
    private Material matInst;
    private int _blurId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _blurId = Shader.PropertyToID("_BlurId");
        // matInst = new Material(PostProcessMat);
        matInst = PostProcessMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (PostProcessMat)
        {
            matInst.SetFloat(_blurId, debugBlur);
            Graphics.Blit(source, destination, PostProcessMat);
        }
    }
}
