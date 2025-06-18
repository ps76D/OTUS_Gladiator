using UnityEditor;
using UnityEngine;

namespace Editor
{
	public class SnapScreenshot : EditorWindow
	{
		public int _scaleMod = 1;
		
		[MenuItem("MyTools/Capture Screenshot")]
		public static void Launch()
		{
			SnapScreenshot window = (SnapScreenshot)EditorWindow.GetWindow(typeof(SnapScreenshot));
			window.Show();
		}

		private void OnGUI()
		{
			_scaleMod = EditorGUILayout.IntField("Scale Mode:", _scaleMod);

			if (GUILayout.Button("Screenshot"))
			{
				var data = System.DateTime.Now.Month + "-" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Hour + "-" + System.DateTime.Now.Minute + "-" + System.DateTime.Now.Second;
				var path = Application.dataPath + "/../Screenshot-" + data + ".png";
				ScreenCapture.CaptureScreenshot(path, _scaleMod);
				ShowNotification(new GUIContent("Done!"));
			}

		}

	}
}
