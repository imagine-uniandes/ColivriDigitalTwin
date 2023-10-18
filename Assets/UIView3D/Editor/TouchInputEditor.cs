//=============================================================================
//
// MIT License
//
// Copyright (c) 2017-2018 ALTIMIT SYSTEMS LTD
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
//=============================================================================

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TouchInput))]
public class TouchInputEditor : Editor
{
    #region Private members
    TouchInput touchInput = null;
    SerializedProperty propUIViewRenderer;
    SerializedObject objTouchInput;
    GUIContent ttDisableScroll = new GUIContent("Disable Viewport Scrolling", "Prevents touch input from moving the viewport");
    GUIContent ttDisableMove = new GUIContent("Disable TouchMove Event", "Disables the \"TouchMove\" event from being sent to the UIView");
    GUIContent ttEnableFling = new GUIContent("Fling Gesture", "Allow scrolling to be influenced by a \"Fling\" gesture");
    GUIContent ttFriction = new GUIContent("Fling Friction", "Rate of deceleration for a \"Fling\" gesture");
    bool gHelp = false;
    #endregion

    #region Editor overrides
    void OnEnable()
    {
        if (target != null)
        {
            objTouchInput = new SerializedObject(target);
            propUIViewRenderer = objTouchInput.FindProperty("uIViewRenderer");
        }
    }

    public override void OnInspectorGUI()
    {
        if (touchInput == null)
        {
            touchInput = target as TouchInput;
        }
        
        if (gHelp = EditorGUILayout.Foldout(gHelp, "Info"))
        {
            EditorGUILayout.HelpBox("Basic component for enabling touch interactivity with a UIView3D surface\nA Collider component is required", MessageType.Info, true);
        }
        EditorGUILayout.Separator();

        objTouchInput.Update();

        UIView3DRenderer rendererVal = (UIView3DRenderer)propUIViewRenderer.objectReferenceValue;
        rendererVal = (UIView3DRenderer)EditorGUILayout.ObjectField("UIView3DRenderer", rendererVal, typeof(UIView3DRenderer), true);
        if (rendererVal == null && GUILayout.Button("Detect UIView3DRenderer"))
        {
            rendererVal = touchInput.GetComponent<UIView3DRenderer>();
        }
        propUIViewRenderer.objectReferenceValue = rendererVal;

        touchInput.disableScrolling = EditorGUILayout.Toggle(ttDisableScroll, touchInput.disableScrolling);
        touchInput.disableMoveEvent = EditorGUILayout.Toggle(ttDisableMove, touchInput.disableMoveEvent);
        touchInput.enableFling = EditorGUILayout.Toggle(ttEnableFling, touchInput.enableFling);

        EditorGUI.BeginDisabledGroup(!touchInput.enableFling);
        touchInput.friction = touchInput.friction < 1.01f ? 1.01f : touchInput.friction;
        touchInput.friction = EditorGUILayout.FloatField(ttFriction, touchInput.friction);
        EditorGUI.EndDisabledGroup();

        objTouchInput.ApplyModifiedProperties();

        EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("UIView3D 1.4\tAltimit Systems LTD", MessageType.None, true);
    }
    #endregion

}
