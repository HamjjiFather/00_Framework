using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace KKSFramework.Editor
{
    /// <summary>
    /// 애니메이션의 Root를 변경하는 에디터 클래스.
    /// </summary>
    public class AnimationRootChangerEditorWindow : EditorWindow
    {
        private static readonly int columnWidth = 300;
        private readonly List<AnimationClip> animationClips;

        private Animator animatorObject;
        private Hashtable paths;
        private ArrayList pathsKeys;

        private Vector2 scrollPos = Vector2.zero;
        private string sNewRoot = "SomeNewObject/Root";

        private string sOriginalRoot = "Root";
        private string sReplacementNewRoot;

        private string sReplacementOldRoot;

        private readonly Dictionary<string, string> tempPathOverrides;

        public AnimationRootChangerEditorWindow()
        {
            animationClips = new List<AnimationClip>();
            tempPathOverrides = new Dictionary<string, string>();
        }

        [MenuItem("Window/Animation Hierarchy Editor")]
        private static void ShowWindow()
        {
            GetWindow<AnimationRootChangerEditorWindow>();
        }

        private void OnSelectionChange()
        {
            if (Selection.objects.Length > 1)
            {
                Debug.Log("Length? " + Selection.objects.Length);
                animationClips.Clear();
                foreach (var o in Selection.objects)
                    if (o is AnimationClip)
                        animationClips.Add((AnimationClip) o);
            }
            else if (Selection.activeObject is AnimationClip)
            {
                animationClips.Clear();
                animationClips.Add((AnimationClip) Selection.activeObject);
                FillModel();
            }
            else
            {
                animationClips.Clear();
            }

            Repaint();
        }

        private void OnGUI()
        {
            if (UnityEngine.Event.current.type == EventType.ValidateCommand)
                switch (UnityEngine.Event.current.commandName)
                {
                    case "UndoRedoPerformed":
                        FillModel();
                        break;
                }

            if (animationClips.Count > 0)
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos, GUIStyle.none);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Referenced Animator (Root):", GUILayout.Width(columnWidth));

                animatorObject = (Animator) EditorGUILayout.ObjectField(
                    animatorObject,
                    typeof(Animator),
                    true,
                    GUILayout.Width(columnWidth));


                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Animation Clip:", GUILayout.Width(columnWidth));

                if (animationClips.Count == 1)
                    animationClips[0] = (AnimationClip) EditorGUILayout.ObjectField(
                        animationClips[0],
                        typeof(AnimationClip),
                        true,
                        GUILayout.Width(columnWidth));
                else
                    GUILayout.Label("Multiple Anim Clips: " + animationClips.Count, GUILayout.Width(columnWidth));
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(20);

                EditorGUILayout.BeginHorizontal();

                sOriginalRoot = EditorGUILayout.TextField(sOriginalRoot, GUILayout.Width(columnWidth));
                sNewRoot = EditorGUILayout.TextField(sNewRoot, GUILayout.Width(columnWidth));
                if (GUILayout.Button("Replace Root"))
                {
                    Debug.Log("O: " + sOriginalRoot + " N: " + sNewRoot);
                    ReplaceRoot(sOriginalRoot, sNewRoot);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Reference path:", GUILayout.Width(columnWidth));
                GUILayout.Label("Animated properties:", GUILayout.Width(columnWidth * 0.5f));
                GUILayout.Label("(Count)", GUILayout.Width(60));
                GUILayout.Label("Object:", GUILayout.Width(columnWidth));
                EditorGUILayout.EndHorizontal();

                if (paths != null)
                    foreach (string path in pathsKeys)
                        GUICreatePathItem(path);

                GUILayout.Space(40);
                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("Please select an Animation Clip");
            }
        }


        private void GUICreatePathItem(string path)
        {
            var newPath = path;
            var obj = FindObjectInRoot(path);
            GameObject newObj;
            var properties = (ArrayList) paths[path];

            var pathOverride = path;

            if (tempPathOverrides.ContainsKey(path)) pathOverride = tempPathOverrides[path];

            EditorGUILayout.BeginHorizontal();

            pathOverride = EditorGUILayout.TextField(pathOverride, GUILayout.Width(columnWidth));
            if (pathOverride != path) tempPathOverrides[path] = pathOverride;

            if (GUILayout.Button("Change", GUILayout.Width(60)))
            {
                newPath = pathOverride;
                tempPathOverrides.Remove(path);
            }

            EditorGUILayout.LabelField(
                properties != null ? properties.Count.ToString() : "0",
                GUILayout.Width(60)
            );

            var standardColor = GUI.color;

            if (obj != null)
                GUI.color = Color.green;
            else
                GUI.color = Color.red;

            newObj = (GameObject) EditorGUILayout.ObjectField(
                obj,
                typeof(GameObject),
                true,
                GUILayout.Width(columnWidth)
            );

            GUI.color = standardColor;

            EditorGUILayout.EndHorizontal();

            try
            {
                if (obj != newObj) UpdatePath(path, ChildPath(newObj));

                if (newPath != path) UpdatePath(path, newPath);
            }
            catch (UnityException ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void FillModel()
        {
            paths = new Hashtable();
            pathsKeys = new ArrayList();

            foreach (var animationClip in animationClips)
            {
                FillModelWithCurves(AnimationUtility.GetCurveBindings(animationClip));
                FillModelWithCurves(AnimationUtility.GetObjectReferenceCurveBindings(animationClip));
            }
        }

        private void FillModelWithCurves(EditorCurveBinding[] curves)
        {
            foreach (var curveData in curves)
            {
                var key = curveData.path;

                if (paths.ContainsKey(key))
                {
                    ((ArrayList) paths[key]).Add(curveData);
                }
                else
                {
                    var newProperties = new ArrayList();
                    newProperties.Add(curveData);
                    paths.Add(key, newProperties);
                    pathsKeys.Add(key);
                }
            }
        }


        private void ReplaceRoot(string oldRoot, string newRoot)
        {
            var fProgress = 0.0f;
            sReplacementOldRoot = oldRoot;
            sReplacementNewRoot = newRoot;

            AssetDatabase.StartAssetEditing();

            for (var iCurrentClip = 0; iCurrentClip < animationClips.Count; iCurrentClip++)
            {
                var animationClip = animationClips[iCurrentClip];
                Undo.RecordObject(animationClip, "Animation Hierarchy Root Change");

                for (var iCurrentPath = 0; iCurrentPath < pathsKeys.Count; iCurrentPath++)
                {
                    var path = pathsKeys[iCurrentPath] as string;
                    var curves = (ArrayList) paths[path];

                    for (var i = 0; i < curves.Count; i++)
                    {
                        var binding = (EditorCurveBinding) curves[i];

                        if (path.Contains(sReplacementOldRoot))
                            if (!path.Contains(sReplacementNewRoot))
                            {
                                var sNewPath = Regex.Replace(path, "^" + sReplacementOldRoot, sReplacementNewRoot);

                                var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                                if (curve != null)
                                {
                                    AnimationUtility.SetEditorCurve(animationClip, binding, null);
                                    binding.path = sNewPath;
                                    AnimationUtility.SetEditorCurve(animationClip, binding, curve);
                                }
                                else
                                {
                                    var objectReferenceCurve =
                                        AnimationUtility.GetObjectReferenceCurve(animationClip, binding);
                                    AnimationUtility.SetObjectReferenceCurve(animationClip, binding, null);
                                    binding.path = sNewPath;
                                    AnimationUtility.SetObjectReferenceCurve(animationClip, binding,
                                        objectReferenceCurve);
                                }
                            }
                    }

                    // Update the progress meter
                    var fChunk = 1f / animationClips.Count;
                    fProgress = iCurrentClip * fChunk + fChunk * (iCurrentPath / (float) pathsKeys.Count);

                    EditorUtility.DisplayProgressBar(
                        "Animation Hierarchy Progress",
                        "How far along the animation editing has progressed.",
                        fProgress);
                }
            }

            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();

            FillModel();
            Repaint();
        }

        private void UpdatePath(string oldPath, string newPath)
        {
            if (paths[newPath] != null)
                throw new UnityException("Path " + newPath + " already exists in that animation!");
            AssetDatabase.StartAssetEditing();
            for (var iCurrentClip = 0; iCurrentClip < animationClips.Count; iCurrentClip++)
            {
                var animationClip = animationClips[iCurrentClip];
                Undo.RecordObject(animationClip, "Animation Hierarchy Change");

                //recreating all curves one by one
                //to maintain proper order in the editor - 
                //slower than just removing old curve
                //and adding a corrected one, but it's more
                //user-friendly
                for (var iCurrentPath = 0; iCurrentPath < pathsKeys.Count; iCurrentPath++)
                {
                    var path = pathsKeys[iCurrentPath] as string;
                    var curves = (ArrayList) paths[path];

                    for (var i = 0; i < curves.Count; i++)
                    {
                        var binding = (EditorCurveBinding) curves[i];
                        var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                        var objectReferenceCurve = AnimationUtility.GetObjectReferenceCurve(animationClip, binding);


                        if (curve != null)
                            AnimationUtility.SetEditorCurve(animationClip, binding, null);
                        else
                            AnimationUtility.SetObjectReferenceCurve(animationClip, binding, null);

                        if (path == oldPath)
                            binding.path = newPath;

                        if (curve != null)
                            AnimationUtility.SetEditorCurve(animationClip, binding, curve);
                        else
                            AnimationUtility.SetObjectReferenceCurve(animationClip, binding, objectReferenceCurve);

                        var fChunk = 1f / animationClips.Count;
                        var fProgress = iCurrentClip * fChunk + fChunk * (iCurrentPath / (float) pathsKeys.Count);

                        EditorUtility.DisplayProgressBar(
                            "Animation Hierarchy Progress",
                            "How far along the animation editing has progressed.",
                            fProgress);
                    }
                }
            }

            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            FillModel();
            Repaint();
        }

        private GameObject FindObjectInRoot(string path)
        {
            if (animatorObject == null) return null;

            var child = animatorObject.transform.Find(path);

            if (child != null)
                return child.gameObject;
            return null;
        }

        private string ChildPath(GameObject obj, bool sep = false)
        {
            if (animatorObject == null) throw new UnityException("Please assign Referenced Animator (Root) first!");

            if (obj == animatorObject.gameObject) return "";

            if (obj.transform.parent == null)
                throw new UnityException("Object must belong to " + animatorObject + "!");
            return ChildPath(obj.transform.parent.gameObject, true) + obj.name + (sep ? "/" : "");
        }
    }
}