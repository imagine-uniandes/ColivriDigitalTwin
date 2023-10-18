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

using System;
using System.Collections;
using UnityEngine;

public class UIView3DRenderer : MonoBehaviour
{
    #region Abstract classes
    public abstract class UIViewProvider : MonoBehaviour
    {
    }

    public abstract class AndroidViewProvider : UIViewProvider
    {
        public abstract IntPtr GetNativeObject();
        public abstract void DispatchMotionEvent(AndroidJavaObject motionEvent);
    }

    public abstract class InputDriver
    {
        public abstract void Down(int id, Vector2 position);
        public abstract void Move(int id, Vector2 position);
        public abstract void Up(int id, Vector2 position);
        public abstract void Cancel(int id);
    }

    public abstract class UIView3D
    {
        public abstract void Create(UIViewProvider provider, int width, int height);
        public abstract void Destroy();
        public abstract void SetScale(float scale);
        public abstract void SetTileCount(int x, int y);
        public abstract void Update();
        public abstract void SetScroll(float x, float y);
        public abstract void Redraw(Texture2D texture);
        public abstract void WaitForRender();
        public abstract int GetContentWidth();
        public abstract int GetContentHeight();
        public abstract int GetContentScrollWidth();
        public abstract int GetContentScrollHeight();
    }
    #endregion

    #region Private members
    [SerializeField]
    float tickTimeSeconds = 1.0f / 20.0f;
    [SerializeField]
    UIViewProvider provider = null;

    UIView3D uIView3D = null;
    Vector2 scrollLast = new Vector2(0.0f, 0.0f);
    bool needsUpdate = true;
    int currViewXTiles = -1;
    int currViewYTiles = -1;
    float currResolutionScale = -1;
    int currViewWidth = -1;
    int currViewHeight = -1;
    int lastUpdateFrame = -1;
    #endregion

    #region Public variables
    public Texture2D textureTarget = null;
    public Vector2 scroll = new Vector2(0.0f, 0.0f);
    public int viewXTiles = 1;
    public int viewYTiles = 1;
    public float viewScale = 1.0f;
    public int viewWidth = 1024;
    public int viewHeight = 1024;
    #endregion

    #region Accessors
    public int updateRate
    {
        get
        {
            return tickTimeSeconds <= 0.0f ? 0 : (int)Mathf.Round(1.0f / tickTimeSeconds);
        }
        set
        {
            float lastValue = tickTimeSeconds;
            tickTimeSeconds = 1.0f / value;

            if (lastValue == 0.0f && value != 0)
            {
                StartCoroutine("Tick");
            }
            else if (lastValue > 0.0f && value <= 0)
            {
                StopCoroutine("Tick");
            }
        }
    }

    public UIViewProvider viewProvider
    {
        get
        {
            return provider;
        }
        set
        {
            provider = value;
            CreateLibrary();
        }
    }

    public int contentWidth
    {
        get
        {
            return uIView3D.GetContentWidth();
        }
    }

    public int contentHeight
    {
        get
        {
            return uIView3D.GetContentHeight();
        }
    }
    #endregion

    #region MonoBehaviour overrides
    void Start()
    {
        if (provider == null)
        {
            provider = GetComponent<UIViewProvider>();
        }

        if (uIView3D == null)
        {
#if UNITY_ANDROID
            uIView3D = new AndroidUIView3D();
#else
            uIView3D = new EditorUIView3D();
#endif
        }

        CreateLibrary();

        if (tickTimeSeconds > 0.0f)
        {
            StartCoroutine("Tick");
        }
    }

    void Update()
    {
        if (Vector2.Distance(scroll, scrollLast) > float.Epsilon)
        {
            ClampScroll();
            uIView3D.SetScroll(scroll.x, scroll.y);
            scrollLast = scroll;
        }

        if (needsUpdate)
        {
            if (currViewWidth != viewWidth || currViewHeight != viewHeight)
            {
                CreateLibrary();
            }
            uIView3D.Redraw(textureTarget);
        }
    }

    void OnDestroy()
    {
        if (tickTimeSeconds > 0.0f)
        {
            StopCoroutine("Tick");
        }
        uIView3D.Destroy();
        uIView3D = null;
    }
    #endregion

    #region Private methods
    IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickTimeSeconds);
            if (needsUpdate)
            {
                RenderUIView();
                needsUpdate = false;
            }
        }
    }

    void RenderUIView()
    {
        if (currViewXTiles != viewXTiles || currViewYTiles != viewYTiles)
        {
            uIView3D.SetTileCount(viewXTiles, viewYTiles);
            currViewXTiles = viewXTiles;
            currViewYTiles = viewYTiles;
        }
        if (currResolutionScale != viewScale)
        {
            uIView3D.SetScale(viewScale);
            currResolutionScale = viewScale;
        }
        uIView3D.Update();
    }

    void CreateLibrary()
    {
        uIView3D.Create(provider, viewWidth, viewHeight);
        uIView3D.SetTileCount(viewXTiles, viewYTiles);
        uIView3D.SetScale(viewScale);
        uIView3D.Update();

        currViewXTiles = viewXTiles;
        currViewYTiles = viewYTiles;
        currResolutionScale = viewScale;
        currViewWidth = viewWidth;
        currViewHeight = viewHeight;
    }

    void ClampScroll()
    {
        scroll.x = Mathf.Clamp(scroll.x, 0, uIView3D.GetContentScrollWidth());
        scroll.y = Mathf.Clamp(scroll.y, 0, uIView3D.GetContentScrollHeight());
    }
    #endregion

    #region Public methods
    public void RequestUpdate()
    {
        if (tickTimeSeconds <= 0.0f && lastUpdateFrame != Time.frameCount)
        {
            RenderUIView();
            lastUpdateFrame = Time.frameCount;
        }
        else
        {
            needsUpdate = true;
        }
        uIView3D.WaitForRender();
    }

    public void ResetScroll()
    {
        scroll.x = 0;
        scroll.y = 0;
    }
    #endregion

}
