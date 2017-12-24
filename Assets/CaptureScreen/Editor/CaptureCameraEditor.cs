// =================================
//
//	CaptureCameraEditor.cs
//	Created by Takuya Himeji
//
// =================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CaptureCamera))]
public class CaptureCameraEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CaptureCamera captureCamera = target as CaptureCamera;

		bool disabled = false;
        if (captureCamera.transform.parent == null || captureCamera.transform.parent.GetComponent<Camera>() == null)
        {
			disabled = true;
			captureCamera.inheritance = false;
        }

        EditorGUI.BeginDisabledGroup(disabled);
		captureCamera.inheritance = EditorGUILayout.Toggle("Inheritance", captureCamera.inheritance);
        EditorGUI.EndDisabledGroup();
		if (disabled)
		{
            EditorGUILayout.HelpBox("親カメラが存在しません。", MessageType.Warning);
		}
		else if (captureCamera.inheritance)
        {
            EditorGUILayout.HelpBox("親カメラ(" + captureCamera.transform.parent.name + ")のパラメータを継承します。", MessageType.Info);
        }

        // 解像度の設定
        captureCamera.resolution = EditorGUILayout.Popup(
            "CaptureResolution",
            captureCamera.resolution,
            new string[] { "SD (720x480)", "HD (1280x720)", "FullHD (1920x1080)", "4K (4096x2160)", "8K (7680x4320)", "Free" }
        );

        if (captureCamera.resolution == 4)
        {
            EditorGUILayout.HelpBox("8K解像度の場合、PCのスペックによっては実行時にUnityが強制終了する場合があります。", MessageType.Warning);
        }
        else if (captureCamera.resolution == 5)
        {
            EditorGUILayout.Vector2IntField("Screen Size", new Vector2Int(captureCamera.width, captureCamera.height));
            EditorGUILayout.Space();
        }

        // アンチエイリアスの設定
        captureCamera.antiAiliasing = EditorGUILayout.Popup(
            "Anti-Ailiasing",
            captureCamera.antiAiliasing,
            new string[] { "Disabled", "2 samples", "4 samples", "8 samples" }
        );
    }
}