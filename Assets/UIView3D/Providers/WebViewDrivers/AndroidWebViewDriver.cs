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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class AndroidWebViewDriver : WebViewProvider.WebViewDriver {

    #region Private members
    AndroidJavaObject webViewClient;
    AndroidJavaObject javascriptInterface;
    AndroidJavaObject androidWebView;
    Queue<AndroidJavaObject> motionEvents = new Queue<AndroidJavaObject>();
    IEnumerator webformPoller;

    #if UNITY_2018_2_OR_NEWER
    IntPtr activeElementTypeMethodID;
    object[] activeElementArgs;
    #endif

    #endregion

    #region ctor
    public AndroidWebViewDriver()
    {
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        webViewClient = new AndroidJavaObject("systems.altimit.uiview3d.AltWebViewClient");
        javascriptInterface = new AndroidJavaObject("systems.altimit.uiview3d.AltJavascriptInterface");

        androidWebView = new AndroidJavaObject("android.webkit.WebView", unityActivity);
        androidWebView.Call("setVerticalScrollBarEnabled", false);
        androidWebView.Call("setHorizontalScrollBarEnabled", false);
        androidWebView.Call("setWebChromeClient", new AndroidJavaObject("android.webkit.WebChromeClient"));
        androidWebView.Call("setWebViewClient", webViewClient);

        webformPoller = WebformPoll();

        #if UNITY_2018_2_OR_NEWER
        activeElementArgs = new object[0];
        activeElementTypeMethodID = AndroidJNIHelper.GetMethodID<string>(javascriptInterface.GetRawClass(), "activeElementType", activeElementArgs, false);
        #endif
    }
    #endregion

    #region WebViewDriver overrides
    public override void Update()
    {
        while (motionEvents.Count > 0)
        {
            androidWebView.Call<bool>("dispatchTouchEvent", motionEvents.Dequeue());
        }
    }

    public override void Destroy()
    {
        androidWebView.Call("destroy");
    }

    public override void SetPaused(bool paused)
    {
        if (paused)
        {
            androidWebView.Call("onPause");
            androidWebView.Call("pauseTimers");
        }
        else
        {
            androidWebView.Call("resumeTimers");
            androidWebView.Call("onResume");
        }
    }

    public override string GetUrl()
    {
        if (androidWebView == null)
        {
            return "null";
        }
        return androidWebView.Call<string>("getUrl");
    }

    public override void LoadUrl(string url)
    {
        androidWebView.Call("loadUrl", url);
    }

    public override void EnableJavascript()
    {
        androidWebView.Call<AndroidJavaObject>("getSettings").Call("setJavaScriptEnabled", true);
        androidWebView.Call("addJavascriptInterface", javascriptInterface, "AltWebView");
    }

    public override void DisableJavascript()
    {
        androidWebView.Call("removeJavascriptInterface", "AltWebView");
        androidWebView.Call<AndroidJavaObject>("getSettings").Call("setJavaScriptEnabled", false);
    }

    public override IEnumerator GetWebformPolling()
    {
        return webformPoller;
    }

    public override void UpdateWebforms(WebViewProvider provider, KeyboardInput.Keyboard input)
    {
        string activeInputType = ActiveInputType();
        if (activeInputType != null && "div".Equals(activeInputType.ToLower()))
        {
            javascriptInterface.Call("evaluateJavascript", androidWebView,
                "(function() {" +
                    "function replaceTextNodes(node, newText) {" +
                        "if (node.hasChildNodes()) {" +
                            "for (var i = 0, len = node.childNodes.length; i < len; ++i) {" +
                                "replaceTextNodes(node.childNodes[i], newText);" +
                            "}" +
                        "} else {" +
                            "node.data = newText;" +
                        "}" +
                    "}" +
                    "replaceTextNodes(document.activeElement, '" + StringHelpers.Escape(input.GetText()) + "');" +
                    "document.activeElement.blur();" +
                "})();");
        }
        else
        {
            javascriptInterface.Call("evaluateJavascript", androidWebView,
                "(function() {" +
                    "document.activeElement.value = '" + StringHelpers.Escape(input.GetText()) + "';" +
                    "document.activeElement.blur();" +
                "})();");
        }
        provider.StartCoroutine(webformPoller);
    }

    public override KeyboardInput.Keyboard GetWebformKeyboard(WebViewProvider provider, KeyboardInput keyboardInput)
    {
        string activeInputType = ActiveInputType();
        if (activeInputType != null)
        {
            TouchScreenKeyboardType keyboardType;
            bool autocorrection = false;
            bool multiline = false;
            bool secure = false;

            switch (activeInputType)
            {
                case "email":
                    keyboardType = TouchScreenKeyboardType.EmailAddress;
                    break;
                case "number":
                    keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
                    break;
                case "password":
                    keyboardType = TouchScreenKeyboardType.ASCIICapable;
                    secure = true;
                    break;
                case "tel":
                    keyboardType = TouchScreenKeyboardType.PhonePad;
                    break;
                case "search":
                    keyboardType = TouchScreenKeyboardType.ASCIICapable;
                    autocorrection = true;
                    break;
                case "text":
                    keyboardType = TouchScreenKeyboardType.ASCIICapable;
                    autocorrection = true;
                    break;
                case "url":
                    keyboardType = TouchScreenKeyboardType.URL;
                    break;
                case "range":
                    keyboardType = TouchScreenKeyboardType.NumberPad;
                    break;
                case "button":
                case "checkbox":
                case "color":
                case "date":
                case "datetime":
                case "datetime-local":
                case "file":
                case "image":
                case "month":
                case "radio":
                case "reset":
                case "submit":
                case "week":
                    return null;
                default:
                    keyboardType = TouchScreenKeyboardType.Default;
                    autocorrection = true;
                    multiline = true;
                    break;
            }

            provider.StopCoroutine(webformPoller);
            javascriptInterface.Call("evaluateJavascript", androidWebView,
                "(function() {" +
                    "AltWebView.onActiveElement(null, null);" +
                "})();");

            return keyboardInput.Open(javascriptInterface.Call<string>("activeElementValue"), keyboardType, autocorrection, multiline, secure);
        }
        return null;
    }

    public override bool PageStartedLoading()
    {
        return webViewClient.Call<bool>("pageStartLoading");
    }

    public override bool PageFinishedLoading()
    {
        return webViewClient.Call<bool>("pageFinishedLoading");
    }
#endregion

#region Private methods
    IEnumerator WebformPoll()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            javascriptInterface.Call("evaluateJavascript", androidWebView,
                "(function() {" +
                    "if (!document.activeElement) {" +
                        "return;" +
                    "}" +
                    "var tagName = document.activeElement.tagName;" +
                    "if (tagName && 'input' === tagName.toLowerCase()) {" +
                        "AltWebView.onActiveElement(document.activeElement.type, document.activeElement.value);" +
                    "} else if (tagName && 'textarea' === tagName.toLowerCase()) {" +
                        "AltWebView.onActiveElement('textarea', document.activeElement.value);" +
                    "} else if (document.activeElement.isContentEditable) {" +
                        "function getTextNode(node) {" +
                            "if (node.hasChildNodes()) {" +
                                "return getTextNode(node.childNodes[0]);" +
                            "} else {" +
                                "return node.data;" +
                            "}" +
                        "}" +
                        "AltWebView.onActiveElement(tagName, getTextNode(document.activeElement));" +
                    "} else {" +
                        "AltWebView.onActiveElement(null, null);" +
                    "}" +
                "})();");
        }
    }

    string ActiveInputType()
    {
        #if UNITY_2018_2_OR_NEWER
        jvalue[] activeElementTypeArgs = AndroidJNIHelper.CreateJNIArgArray(activeElementArgs);
        try
        {
            IntPtr returnValue = AndroidJNI.CallObjectMethod(javascriptInterface.GetRawObject(), activeElementTypeMethodID, activeElementTypeArgs);
            if (IntPtr.Zero != returnValue)
            {
                var activeInputType = AndroidJNI.GetStringUTFChars(returnValue);
                AndroidJNI.DeleteLocalRef(returnValue);
                return activeInputType;
            }
        }
        finally
        {
            AndroidJNIHelper.DeleteJNIArgArray(activeElementArgs, activeElementTypeArgs);
        }
        return null;
        #else
        return javascriptInterface.Call<string>("activeElementType");
        #endif
    }
#endregion

#region Public methods
    public void DispatchMotionEvent(AndroidJavaObject motionEvent)
    {
        motionEvents.Enqueue(motionEvent);
    }

    public IntPtr GetNativeObject()
    {
        return androidWebView.GetRawObject();
    }
#endregion

}

#region Helper functions
public static class StringHelpers
{
    private static Dictionary<string, string> escapeMapping = new Dictionary<string, string>()
        {
            {"\'", "\\'"},
            {"\"", "\\\""},
            {"\\", "\\\\"},
            {"\b", "\\b"},
            {"\r", "\\r"},
            {"\f", "\\f"},
            {"\t", "\\t"},
            {"\v", "\\v"},
            {"\n", "\\n"},
            {"\0", "\\0"},
        };

    private static Regex escapeRegex = new Regex(string.Join("|", escapeMapping.Keys.ToArray()));

    public static string Escape(this string s)
    {
        return escapeRegex.Replace(s, EscapeMatchEval);
    }

    private static string EscapeMatchEval(Match m)
    {
        if (escapeMapping.ContainsKey(m.Value))
        {
            return escapeMapping[m.Value];
        }
        return escapeMapping[Regex.Escape(m.Value)];
    }

}
#endregion
