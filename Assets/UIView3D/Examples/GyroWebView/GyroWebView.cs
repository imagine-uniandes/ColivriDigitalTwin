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

using UnityEngine;

public class GyroWebView : MonoBehaviour
{
    #region Private members
    WebViewProvider webView;
    string url;
    #endregion

    #region MonoBehaviour overrides
    void Start()
    {
        webView = GetComponent<WebViewProvider>();
        url = webView.GetUrl();

        Input.gyro.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        transform.Rotate(0, 0, Input.gyro.rotationRateUnbiased.y);
    }

    void OnGUI()
    {
        float screenSize = Mathf.Min(Screen.width, Screen.height);
        float btnSize = screenSize / 8.0f;

        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = (int)(btnSize / 4);
        myButtonStyle.normal.textColor = Color.white;

        url = GUI.TextField(new Rect(0, 0, Screen.width - btnSize, btnSize), url, myButtonStyle);
        if (GUI.Button(new Rect(Screen.width - btnSize, 0, btnSize, btnSize), ">", myButtonStyle))
        {
            webView.LoadUrl(url);
        }
    }
    #endregion

    #region Public methods
    public void ResetUrl()
    {
        url = webView.GetUrl();
    }
    #endregion

}
