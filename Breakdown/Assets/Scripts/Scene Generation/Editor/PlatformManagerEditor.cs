using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(PlatformManager))]
public class PlatformManagerEditor : Editor {

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		if (GUILayout.Button ("DONT EVER CLICK ME!")) {
			Text[] texts = GameObject.FindObjectsOfType<Text>();
			foreach (Text text in texts) {
				TextMeshProUGUI tmpro = text.gameObject.GetComponent<TextMeshProUGUI> ();
				if (tmpro == null) {
					GameObject textGo = text.gameObject;
					Vector2 size = textGo.GetComponent<RectTransform> ().sizeDelta;
					string displayText = text.text;
					Color textColor = text.color;
					float fontSize = (float)text.fontSize;
					bool italic = text.fontStyle == FontStyle.Italic;
					TextAnchor alignment = text.alignment;
					DestroyImmediate (text);

					tmpro = textGo.AddComponent<TextMeshProUGUI> ();
					tmpro.text = displayText;
					tmpro.color = textColor;
					tmpro.fontSize = fontSize;
					if (italic) {
						tmpro.fontStyle = FontStyles.Italic;
					}
					switch (alignment) {
					case TextAnchor.LowerCenter:
						tmpro.alignment = TextAlignmentOptions.Bottom;
						break;
					case TextAnchor.LowerLeft:
						tmpro.alignment = TextAlignmentOptions.BottomLeft;
						break;
					case TextAnchor.LowerRight:
						tmpro.alignment = TextAlignmentOptions.BottomRight;
						break;
					case TextAnchor.MiddleCenter:
						tmpro.alignment = TextAlignmentOptions.Center;
						break;
					case TextAnchor.MiddleLeft:
						tmpro.alignment = TextAlignmentOptions.Left;
						break;
					case TextAnchor.MiddleRight:
						tmpro.alignment = TextAlignmentOptions.Right;
						break;
					case TextAnchor.UpperCenter:
						tmpro.alignment = TextAlignmentOptions.Top;
						break;
					case TextAnchor.UpperLeft:
						tmpro.alignment = TextAlignmentOptions.TopLeft;
						break;
					case TextAnchor.UpperRight:
						tmpro.alignment = TextAlignmentOptions.TopRight;
						break;
					}
					tmpro.font = (TMP_FontAsset)AssetDatabase.LoadAssetAtPath ("Assets/Textures and Sprites/Roboto-Medium SDF.asset", typeof(TMP_FontAsset));
					textGo.GetComponent<RectTransform> ().sizeDelta = size;
				}
			}
		}
	}

}
