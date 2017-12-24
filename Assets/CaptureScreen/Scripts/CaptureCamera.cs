// =================================
//
//	CaptureCamera.cs
//	Created by Takuya Himeji
//
// =================================

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CaptureCamera : MonoBehaviour
{
	#region Inspector Settings
	public int resolution		= 1;	// 解像度
	public int antiAiliasing	= 0;	// アンチエイリアス
	public bool inheritance		= true; // 親カメラのパラメータを継承するか
	public int width	= 1280;
	public int height	= 720;
	#endregion // Inspector Settings


	#region Member Field
	private RenderTexture renderTexture; // 保存対象のRenderTexture
	#endregion // Member Field


	#region MonoBehaviour Methods
	void Start ()
	{
		// カメラ取得
		Camera cam = GetComponent<Camera> ();
		// RenderTexture取得
		renderTexture = cam.targetTexture;

		// 解像度の設定
		switch (resolution)
		{
			// SD
            case 0:
                renderTexture.width	= 720;
                renderTexture.height = 480;
                break;
            // HD
            case 1:
                renderTexture.width = 1280;
                renderTexture.height = 720;
                break;
			// FullHD
			case 2:
                renderTexture.width = 1920;
                renderTexture.height = 1080;
            	break;
			// 4K
			case 3:
                renderTexture.width = 4096;
                renderTexture.height = 2160;
				break;
			// 8K
			case 4:
                renderTexture.width = 7680;
                renderTexture.height = 4320;
				break;

			// Free
			case 5:
                renderTexture.width = width;
                renderTexture.height = height;
				break;
		}

		// アンチエイリアスの設定
		switch (antiAiliasing)
		{
			case 0:
				renderTexture.antiAliasing = 0;
				break;
            case 1:
                renderTexture.antiAliasing = 2;
				break;
            case 2:
                renderTexture.antiAliasing = 4;
            	break;
            case 3:
                renderTexture.antiAliasing = 8;
                break;
		}

		// 親カメラの設定を引き継ぐ
		if (inheritance) {
			Camera parentCam	= transform.parent.GetComponent<Camera>();
			cam.fieldOfView		= parentCam.fieldOfView;
			cam.farClipPlane	= parentCam.farClipPlane;
			cam.nearClipPlane	= parentCam.nearClipPlane;
			cam.clearFlags		= parentCam.clearFlags;
			cam.backgroundColor = parentCam.backgroundColor;
			cam.aspect			= parentCam.aspect;
			if (parentCam.orthographic)
			{
				cam.projectionMatrix = parentCam.projectionMatrix;
			}
		}
	}
	
	void Update ()
	{
		
	}

	void OnGUI ()
	{
		if (GUI.Button(new Rect(10, 10, 150, 100), "Shoot!!!")) {
			// 撮影
			CaptureOnScreen ();
		}
	}
	#endregion // MonoBehaviour Methods


	#region Member Methods
	// 撮影し、書き出す
    public void CaptureOnScreen()
    {
		Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
 
        // pngテクスチャの生成
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);
 
        // 指定のディレクトリへ保存
		string saveDir = Application.dataPath + "/../Captures";
		if (!Directory.Exists(saveDir))
		{
			Directory.CreateDirectory(saveDir);
		}
        File.WriteAllBytes(saveDir + "/SavedScreen_"+ System.DateTime.Now.ToString("yyyyMMddHHmmss") +".png", bytes);
    }
	#endregion // Member Methods
}