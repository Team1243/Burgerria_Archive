using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportTexture : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField] private List<RenderTexture> renderTextures = new List<RenderTexture>();
    int i = 0;
    [ContextMenu("ExportAll")]
    public void ExportrRnderTextures()
    {
        DumpRenderTexture(renderTextures[0], Application.dataPath + "/99.Others/Damin");

        //foreach (RenderTexture renderTexture in renderTextures)
        //{
        //    DumpRenderTexture(renderTexture, Application.dataPath + "/99.Others/Damin");
        //}
    }

    private void DumpRenderTexture(RenderTexture rt, string pngOutPath)
    {
        ////var oldRT = RenderTexture.active;

        //var tex = new Texture2D(rt.width, rt.height);
        //RenderTexture.active = rt;
        //tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        //tex.Apply();

        //byte[] bytes = tex.EncodeToPNG();

        //File.WriteAllBytes(pngOutPath + $"/{rt.name}.png", bytes);
        ////RenderTexture.active = oldRT;



        RenderTexture mRt = new RenderTexture(rt.width, rt.height, rt.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        mRt.antiAliasing = rt.antiAliasing;

        var tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
        cam.targetTexture = mRt;
        cam.Render();
        RenderTexture.active = mRt;

        tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
        tex.Apply();

        var path = pngOutPath + rt.name + i++.ToString() + ".png";
        File.WriteAllBytes(path, tex.EncodeToPNG());
        Debug.Log("Saved file to: " + path);

        DestroyImmediate(tex);

        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;

        DestroyImmediate(mRt);
    }
}
