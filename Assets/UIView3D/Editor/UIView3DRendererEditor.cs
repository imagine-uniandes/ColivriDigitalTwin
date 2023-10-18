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

[CustomEditor(typeof(UIView3DRenderer))]
public class UIView3DRendererEditor : Editor
{
    #region Private members
    UIView3DRenderer renderer;

    int textureCreateSizeWidth;
    int textureCreateSizeHeight;
    bool textureCreateAutoSize;
    TextureFormat textureCreateFormat;
    bool textureCreateEnableMips;

    GUIContent ttUpdateRate = new GUIContent("Update Rate", "Update rate of the UIView in ticks-per-second");
    GUIContent ttUIViewScale = new GUIContent("Resolution Scale", "Resolution scaling of the UIView content");

    bool gTextureTarget = true;
    bool gUIView = true;
    bool gHelp = false;

    SerializedProperty propTickTimeSeconds;
    SerializedProperty propProvider;
    SerializedObject objRenderer;
    #endregion

    #region Editor overrides
    void OnEnable()
    {
        if (target != null)
        {
            objRenderer = new SerializedObject(target);
            propTickTimeSeconds = objRenderer.FindProperty("tickTimeSeconds");
            propProvider = objRenderer.FindProperty("provider");
        }
    }

    public override void OnInspectorGUI()
    {
        if (renderer == null)
        {
            Initialize();
        }
        
        if (gHelp = EditorGUILayout.Foldout(gHelp, "Info"))
        {
            EditorGUILayout.HelpBox("Configure the rendering behaviour of the UIView3D\nA UIViewProvider component is required to source the UIView", MessageType.Info, true);
        }
        EditorGUILayout.Separator();

        objRenderer.Update();
        UpdateRate();
        TextureTarget();
        UIView();
        objRenderer.ApplyModifiedProperties();

        EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("UIView3D 1.4\tAltimit Systems LTD", MessageType.None, true);
    }
    #endregion

    #region Private methods
    void Initialize()
    {
        renderer = target as UIView3DRenderer;

        if (renderer.textureTarget != null)
        {
            textureCreateSizeWidth = renderer.textureTarget.width;
            textureCreateSizeHeight = renderer.textureTarget.height;
            textureCreateFormat = renderer.textureTarget.format;
            textureCreateEnableMips = renderer.textureTarget.mipmapCount > 1;

            textureCreateAutoSize = (textureCreateSizeWidth == renderer.viewWidth && textureCreateSizeHeight == renderer.viewHeight);
        }
        else
        {
            textureCreateFormat = TextureFormat.RGB565;
            textureCreateEnableMips = true;
            renderer.textureTarget = new Texture2D(renderer.viewWidth, renderer.viewHeight, textureCreateFormat, textureCreateEnableMips);
            textureCreateAutoSize = true;
        }
    }

    void UpdateRate()
    {
        int intVal = propTickTimeSeconds.floatValue <= 0 ? 0 : (int)Mathf.Round(1.0f / propTickTimeSeconds.floatValue);
        intVal = EditorGUILayout.IntSlider(ttUpdateRate, intVal, 0, 60);
        float floatValue = intVal <= 0 ? 0 : 1.0f / intVal;
        propTickTimeSeconds.floatValue = floatValue;
    }

    void TextureTarget()
    {
        if (gTextureTarget = EditorGUILayout.Foldout(gTextureTarget, "Texture Target"))
        {
            EditorGUI.indentLevel++;

            EditorGUI.BeginChangeCheck();
            textureCreateAutoSize = EditorGUILayout.Toggle("Auto Size", textureCreateAutoSize);

            if (textureCreateAutoSize)
            {
                GUI.enabled = false;
                textureCreateSizeWidth = renderer.viewWidth;
                textureCreateSizeHeight = renderer.viewHeight;
            }

            SizeField("Size", ref textureCreateSizeWidth, ref textureCreateSizeHeight);

            GUI.enabled = true;

            textureCreateFormat = (TextureFormat)EditorGUILayout.EnumPopup("Color Format", textureCreateFormat);
            textureCreateEnableMips = EditorGUILayout.Toggle("Enable Mip Maps", textureCreateEnableMips);

            if (EditorGUI.EndChangeCheck())
            {
                renderer.textureTarget = new Texture2D(textureCreateSizeWidth, textureCreateSizeHeight, textureCreateFormat, textureCreateEnableMips);
            }

            EditorGUI.indentLevel--;
        }
    }

    void UIView()
    {
        if (gUIView = EditorGUILayout.Foldout(gUIView, "UIView"))
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();

            UIView3DRenderer.UIViewProvider providerVal = (UIView3DRenderer.UIViewProvider)propProvider.objectReferenceValue;
            providerVal = (UIView3DRenderer.UIViewProvider)EditorGUILayout.ObjectField("UIView Provider", providerVal, typeof(UIView3DRenderer.UIViewProvider), true);
            if (providerVal == null && GUILayout.Button("Detect UIViewProvider"))
            {
                providerVal = renderer.GetComponent<UIView3DRenderer.UIViewProvider>();
            }
            propProvider.objectReferenceValue = providerVal;

            EditorGUILayout.EndHorizontal();

            if (textureCreateAutoSize)
            {
                EditorGUI.BeginChangeCheck();
            }
            SizeField("Size", ref renderer.viewWidth, ref renderer.viewHeight);
            if (textureCreateAutoSize && EditorGUI.EndChangeCheck())
            {
                textureCreateSizeWidth = renderer.viewWidth;
                textureCreateSizeHeight = renderer.viewHeight;
                renderer.textureTarget = new Texture2D(textureCreateSizeWidth, textureCreateSizeHeight, textureCreateFormat, textureCreateEnableMips);
            }

            renderer.viewScale = EditorGUILayout.FloatField(ttUIViewScale, renderer.viewScale);
            if (renderer.viewScale < 0.0f)
            {
                renderer.viewScale = 0.0f;
            }

            SizeField("Render Tiles", ref renderer.viewXTiles, ref renderer.viewYTiles);

            EditorGUI.indentLevel--;
        }
    }
    #endregion

    #region Static functions
    static void SizeField(string label, ref int outWidth, ref int outHeight)
    {
        int indentLevel = EditorGUI.indentLevel;
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PrefixLabel(label);
        EditorGUI.indentLevel = 0;

        outWidth = EditorGUILayout.IntField(outWidth);
        EditorGUILayout.LabelField("x", GUILayout.Width(11));
        outHeight = EditorGUILayout.IntField(outHeight);

        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel = indentLevel;

        outWidth = outWidth > 0 ? outWidth : 1;
        outHeight = outHeight > 0 ? outHeight : 1;
    }
    #endregion

}
