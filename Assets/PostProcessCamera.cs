using UnityEngine;

public class PostProcessCamera : MonoBehaviour
{
    public static PostProcessCamera Instance;
    public Material PostProcessMat;
    public AnimationCurve blurCurve;
    
    private float blurStrength;
    private float curBlur;
    private Material matInst;
    private int _blurId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        _blurId = Shader.PropertyToID("_Blur");
        matInst = new Material(PostProcessMat);
        matInst.SetFloat(_blurId, 0);
        PostProcessMat = matInst;
    }
    

    // Update is called once per frame
    void Update()
    {
        curBlur = Mathf.Lerp(curBlur, blurStrength, Time.deltaTime * 2);
    }
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (PostProcessMat)
        {
            matInst.SetFloat(_blurId, blurCurve.Evaluate(curBlur) * .5f);
            Graphics.Blit(source, destination, PostProcessMat);
        }
    }

    public static void SetBlur(float blur)
    {
        blur = 1 - blur;
        if(!Instance) return;
        Instance.blurStrength = blur;
    }
}
