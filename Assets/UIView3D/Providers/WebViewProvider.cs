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
using UnityEngine.Events;

public class WebViewProvider :
#if UNITY_ANDROID
    UIView3DRenderer.AndroidViewProvider
#else
    UIView3DRenderer.UIViewProvider
#endif
{
    #region Abstract classes
    public abstract class WebViewDriver
    {
        public abstract void Update();
        public abstract void Destroy();
        public abstract void SetPaused(bool paused);
        public abstract string GetUrl();
        public abstract void LoadUrl(string url);
        public abstract void EnableJavascript();
        public abstract void DisableJavascript();
        public abstract IEnumerator GetWebformPolling();
        public abstract void UpdateWebforms(WebViewProvider provider, KeyboardInput.Keyboard input);
        public abstract KeyboardInput.Keyboard GetWebformKeyboard(WebViewProvider provider, KeyboardInput keyboardInput);
        public abstract bool PageStartedLoading();
        public abstract bool PageFinishedLoading();
    }
    #endregion

    #region Serializables
    [Serializable]
    private class PageEvents
    {
        public UnityEvent onPageLoading = null;
        public UnityEvent onPageLoaded = null;
    }
    #endregion

    #region Serialize fields
    [SerializeField]
    string url;

    [SerializeField]
    bool javascriptEnabled = true;

    [SerializeField]
    bool keyboardEnabled = true;

    [SerializeField]
    KeyboardInput webformKeyboard = null;
        
    [SerializeField]
    PageEvents pageEvents;
    #endregion

    #region Private members
    WebViewDriver webViewDriver;
    KeyboardInput.Keyboard activeInput = null;
    #endregion

    #region MonoBehaviour overrides
    void Awake()
    {
#if UNITY_ANDROID
        webViewDriver = new AndroidWebViewDriver();
#else
        webViewDriver = null;
#endif
    }

    void Start()
    {
        if (webViewDriver == null)
        {
            return;
        }
        if (webformKeyboard == null)
        {
            webformKeyboard = gameObject.AddComponent<TouchKeyboardInput>();
        }

        if (javascriptEnabled)
        {
            webViewDriver.EnableJavascript();
        }
        if (keyboardEnabled)
        {
            StartCoroutine(webViewDriver.GetWebformPolling());
        }

        webViewDriver.LoadUrl(url);
    }

    void Update()
    {
        if (webViewDriver == null)
        {
            return;
        }
        
        if (keyboardEnabled)
        {
            if (activeInput != null && !activeInput.IsActive())
            {
                webViewDriver.UpdateWebforms(this, activeInput);
                activeInput = null;
            }
            else if (webformKeyboard != null && activeInput == null)
            {
                activeInput = webViewDriver.GetWebformKeyboard(this, webformKeyboard);
            }
        }

        if (webViewDriver.PageStartedLoading())
        {
            pageEvents.onPageLoading.Invoke();
        }
        if (webViewDriver.PageFinishedLoading())
        {
            pageEvents.onPageLoaded.Invoke();
        }
    }

    void LateUpdate()
    {
        if (webViewDriver == null)
        {
            return;
        }
        webViewDriver.Update();
    }

    void OnDestroy()
    {
        if (webViewDriver == null)
        {
            return;
        }
        if (keyboardEnabled)
        {
            StopCoroutine(webViewDriver.GetWebformPolling());
        }
        if (javascriptEnabled)
        {
            webViewDriver.DisableJavascript();
        }
        webViewDriver.Destroy();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (webViewDriver == null)
        {
            return;
        }
        webViewDriver.SetPaused(pauseStatus);
    }
    #endregion

    #region Public methods
    public string GetUrl()
    {
        return webViewDriver.GetUrl();
    }

    public void LoadUrl(string url)
    {
        webViewDriver.LoadUrl(url);
    }
    #endregion

    #region ViewProvider overrides
#if UNITY_ANDROID
    public override void DispatchMotionEvent(AndroidJavaObject motionEvent)
    {
        (webViewDriver as AndroidWebViewDriver).DispatchMotionEvent(motionEvent);
    }

    public override IntPtr GetNativeObject()
    {
        return (webViewDriver as AndroidWebViewDriver).GetNativeObject();
    }
#endif
    #endregion

}
