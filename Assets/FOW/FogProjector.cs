//Credit to Andrew Hung for this code! Thanks!
//https://andrewhungblog.wordpress.com/2018/06/23/implementing-fog-of-war-in-unity/

using System.Collections;
using UnityEngine;

public class FogProjector : MonoBehaviour
{
    public Material projectorMaterial;
    public float blendSpeed;
    public int textureScale;

    public RenderTexture fogTexture;

    private RenderTexture prevTexture;
    private RenderTexture currTexture;
    private Projector projector;

    private float blendAmount;

    private void Awake()
    {
        projector = GetComponent<Projector>();
        projector.enabled = true;

        prevTexture = GenerateTexture();
        currTexture = GenerateTexture();

        // Projector materials aren't instanced, resulting in the material asset getting changed.
        // Instance it here to prevent us from having to check in or discard these changes manually.
        projector.material = new Material(projectorMaterial);

        projector.material.SetTexture("_PrevTexture", prevTexture);
        projector.material.SetTexture("_CurrTexture", currTexture);

        StartNewBlend();
    }

    RenderTexture GenerateTexture()
    {
        RenderTexture rt = new RenderTexture(
            fogTexture.width * textureScale,
            fogTexture.height * textureScale,
            0,
            fogTexture.format)
        { filterMode = FilterMode.Bilinear };
        rt.antiAliasing = fogTexture.antiAliasing;

        return rt;
    }

    public void StartNewBlend()
    {
        StopCoroutine(BlendFog());
        blendAmount = 0;
        // Swap the textures
        Graphics.Blit(currTexture, prevTexture);
        Graphics.Blit(fogTexture, currTexture);

        StartCoroutine(BlendFog());
    }

    IEnumerator BlendFog()
    {
        while (blendAmount < 1)
        {
            // increase the interpolation amount
            blendAmount += Time.deltaTime * blendSpeed;
            // Set the blend property so the shader knows how much to lerp
            // by when checking the alpha value
            projector.material.SetFloat("_Blend", blendAmount);
            yield return null;
        }
        // once finished blending, swap the textures and start a new blend
        StartNewBlend();
    }
}