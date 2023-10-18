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

public class AndroidInputDriver : UIView3DRenderer.InputDriver
{
    #region Private members
    UIView3DRenderer.AndroidViewProvider viewProvider;
    AndroidJavaClass aSystemClock;
    AndroidJavaClass aMotionEvent;
    #endregion

    #region ctor
    public AndroidInputDriver(UIView3DRenderer.AndroidViewProvider viewProvider)
    {
        this.viewProvider = viewProvider;
        aSystemClock = new AndroidJavaClass("android.os.SystemClock");
        aMotionEvent = new AndroidJavaClass("android.view.MotionEvent");
    }
    #endregion

    #region InputDriver overrides
    public override void Down(int id, Vector2 position)
    {
        long time = aSystemClock.CallStatic<long>("uptimeMillis");
        viewProvider.DispatchMotionEvent(aMotionEvent.CallStatic<AndroidJavaObject>("obtain", time, time, 0, position.x, position.y, 0));
    }

    public override void Move(int id, Vector2 position)
    {
        long time = aSystemClock.CallStatic<long>("uptimeMillis");
        viewProvider.DispatchMotionEvent(aMotionEvent.CallStatic<AndroidJavaObject>("obtain", time, time + 33, 2, position.x, position.y, 0));
    }

    public override void Up(int id, Vector2 position)
    {
        long time = aSystemClock.CallStatic<long>("uptimeMillis");
        viewProvider.DispatchMotionEvent(aMotionEvent.CallStatic<AndroidJavaObject>("obtain", time, time, 1, position.x, position.y, 0));
    }

    public override void Cancel(int id)
    {
        long time = aSystemClock.CallStatic<long>("uptimeMillis");
        viewProvider.DispatchMotionEvent(aMotionEvent.CallStatic<AndroidJavaObject>("obtain", time, time, 3, 0, 0, 0));
    }
    #endregion

}
