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

[CustomEditor(typeof(MaterialBinder))]
public class MaterialBinderEditor : Editor
{
    #region Private members
    MaterialBinder binder = null;
    GUIContent ttRenderer = new GUIContent("UIView3D Renderer", "The rendering source for the UIView");
    GUIContent ttMaterial = new GUIContent("Material", "Target Material");
    bool gHelp = false;
    #endregion

    #region Editor overrides
    public override void OnInspectorGUI()
    {
        if (binder == null)
        {
            Initialize();
        }

        if (gHelp = EditorGUILayout.Foldout(gHelp, "Info"))
        {
            EditorGUILayout.HelpBox("Helper for binding the texture of a UIView3DRenderer to a Material mainTexture", MessageType.Info, true);
        }
        EditorGUILayout.Separator();

        Renderer();
        Material();

        EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("UIView3D 1.4\tAltimit Systems LTD", MessageType.None, true);
    }
    #endregion

    #region Private methods
    void Initialize()
    {
        binder = target as MaterialBinder;
    }

    void Renderer()
    {
        binder.viewRenderer = (UIView3DRenderer)EditorGUILayout.ObjectField(ttRenderer, binder.viewRenderer, typeof(UIView3DRenderer), true);
        if (binder.viewRenderer == null && GUILayout.Button("Detect UIView3DRenderer"))
        {
            binder.viewRenderer = binder.GetComponent<UIView3DRenderer>();
        }
    }

    void Material()
    {
        binder.material = (Material)EditorGUILayout.ObjectField(ttMaterial, binder.material, typeof(Material), true);
        if (binder.material == null && GUILayout.Button("Detect Material"))
        {
            binder.material = binder.GetComponent<Renderer>().sharedMaterial;
        }
    }
    #endregion

}
