using UnityEditor;
using UnityEngine;

namespace UMediator.Editor
{
    internal class UMediatorSetupEditor : EditorWindow
    {
        private static Texture2D _logo;
        private const int Width = 300;
        private const int Height = 250;
        private const int Margin = 10;
        private const string ImagePath = "Packages/com.rustamkhonoff.umediator/Editor/Images/umediator_logo.jpg";
        private const string ZenjectDefine = "UMEDIATOR_USE_ZENJECT";


        [MenuItem("Plugins/UMediator/Open Setup Window")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<UMediatorSetupEditor>("UMediator Setup");
            window.minSize = new Vector2(Width, Height);
        }

        private void OnEnable()
        {
            _logo = (Texture2D)AssetDatabase.LoadAssetAtPath(ImagePath, typeof(Texture2D));
        }

        private void OnGUI()
        {
            if (_logo != null)
            {
                float aspectRatio = (float)_logo.width / _logo.height;
                float adaptedHeight = position.width / aspectRatio;
                adaptedHeight = Mathf.Min(adaptedHeight, 150);
                Rect imageRect = GUILayoutUtility.GetRect(position.width, adaptedHeight, GUILayout.ExpandWidth(true));
                imageRect.x += Margin;
                imageRect.y += Margin;
                imageRect.width -= 2 * Margin;
                imageRect.height -= 2 * Margin;
                EditorGUI.DrawPreviewTexture(imageRect, _logo, null, ScaleMode.ScaleToFit);
            }

            EditorGUILayout.HelpBox($"Changes Scripting Define, Symbols Adds/Removes {ZenjectDefine}", MessageType.Info);
            if (IsDefineExist(ZenjectDefine))
            {
                if (GUILayout.Button("Remove Zenject Support"))
                    RemoveDefine(ZenjectDefine);
            }
            else
            {
                if (GUILayout.Button("Add Zenject Support"))
                    AddDefine(ZenjectDefine);
            }
        }

        private static bool IsDefineExist(string define)
        {
            return PlayerSettings
                .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup)
                .Contains(define);
        }

        private static void AddDefine(string define)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            if (defines.Contains(define))
            {
                Debug.LogWarning($"{define} already exist int Scripting Define Symbols");
                return;
            }

            string appendedDefine = defines + ";" + define;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, appendedDefine);
            Debug.LogWarning($"{define} added to Scripting Define Symbols");
        }

        private static void RemoveDefine(string define)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            if (!defines.Contains(define))
            {
                Debug.LogWarning($"{define} doesn't exist in Scripting Define Symbols");
                return;
            }

            string clearedDefine = defines.Replace(";" + define, "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, clearedDefine);
            Debug.LogWarning($"{define} removed from Scripting Define Symbols");
        }
    }
}