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

public class TouchInput : MonoBehaviour
{
    #region Public variables
    public bool disableMoveEvent = false;
    public bool disableScrolling = false;
    public bool enableFling = true;
    public float friction = 1.1f;
    #endregion

    #region Private members
    [SerializeField]
    UIView3DRenderer uIViewRenderer = null;

    UIView3DRenderer.InputDriver inputDriver = null;
    Collider viewCollider;
    Vector2 motionStart;
    Vector2 velocity;
    #endregion

    #region MonoBehaviour overrides
    void Awake()
    {
        if (uIViewRenderer == null)
        {
            uIViewRenderer = GetComponent<UIView3DRenderer>();
        }

#if UNITY_ANDROID
        inputDriver = new AndroidInputDriver(uIViewRenderer.viewProvider as UIView3DRenderer.AndroidViewProvider);
#else
        inputDriver = null;
#endif
        motionStart.x = float.NaN;
        motionStart.y = float.NaN;
    }

    void Start()
    {
        viewCollider = uIViewRenderer.GetComponent<Collider>();
    }

    void Update()
    {
        if (inputDriver == null)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            RaycastHit rhit;
            if (!viewCollider.Raycast(Camera.main.ScreenPointToRay(touch.position), out rhit, float.MaxValue))
            {
                motionStart.x = float.NaN;
                motionStart.y = float.NaN;
                return;
            }

            Vector2 uvPosition;
            uvPosition.x = (1.0f - rhit.textureCoord.x) * uIViewRenderer.contentWidth;
            uvPosition.y = rhit.textureCoord.y * uIViewRenderer.contentHeight;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    inputDriver.Down(0, uvPosition);
                    motionStart = uvPosition;
                    velocity = Vector2.zero;
                    break;
                case TouchPhase.Moved:
                    if (!disableMoveEvent)
                    {
                        inputDriver.Move(0, uvPosition);
                    }

                    if (motionStart.x == float.NaN || motionStart.y == float.NaN)
                    {
                        motionStart = uvPosition;
                        velocity = Vector2.zero;
                    }
                    else
                    {
                        velocity = motionStart - uvPosition;
                        if (!disableScrolling && Input.touchCount == 1 && velocity.sqrMagnitude > float.Epsilon)
                        {
                            uIViewRenderer.scroll += velocity;
                        }
                        motionStart = uvPosition;
                    }
                    break;
                case TouchPhase.Ended:
                    inputDriver.Up(0, uvPosition);
                    motionStart.x = float.NaN;
                    motionStart.y = float.NaN;
                    break;
                case TouchPhase.Canceled:
                    inputDriver.Cancel(0);
                    motionStart.x = float.NaN;
                    motionStart.y = float.NaN;
                    break;
                default:
                    motionStart.x = float.NaN;
                    motionStart.y = float.NaN;
                    break;
            }
        }
        else if(enableFling && velocity.sqrMagnitude > float.Epsilon)
        {
            uIViewRenderer.scroll += velocity;
            velocity /= friction;
        }
    }
    #endregion
    
}
