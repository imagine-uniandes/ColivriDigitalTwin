# UIView3D #

Version 1.4

## About ##

UIView3D is a software library for mobile devices that enables the use of native mobile UI content within a 3D engine.

This means that rich content, such as HTML through WebView, can be displayed in 3D mobile applications with full interactivity and smooth scrolling.

UIView3D works beautifully with Virtual Reality and Augmented Reality, supporting both Google Daydream View and Samsung Gear VR.

## Unity&reg; Plugin ##

### Android&trade; Example ###

Open the example scene "Examples/GyroWebView/GyroWebView.unity" to inspect how UIView3D can be used to display a basic web-browser inside Unity.

This example scene creates a 3D plane with a default material that has a UIView3D renderer and an example AndroidWebView provider attached, as well as some additional components for providing basic touch interactivity.

### Creating UIView Android Providers ###

To create a custom provider you must extend the UIView3DRenderer.AndroidViewProvider class and override the methods **::GetNativeObject()->IntPtr** and **::DispatchMotionEvent(AndroidJavaObject)**.

See how the WebViewProvider implements these methods and uses them with the MonoBehaviour methods for an example. 

#### GetNativeObject ####

**::GetNativeObject()->IntPtr** must return a native handle to an Android View.

You can create a native Android View inside Unity with the **AndroidJavaObject** class (https://docs.unity3d.com/ScriptReference/AndroidJavaObject.html). The Android native object can then be retrieved through **AndroidJavaObject::GetRawObject()->IntPtr**.

#### DispatchMotionEvent ####

**::DispatchMotionEvent(AndroidJavaObject)** is called by input components (such as the example TouchInput) to forward Unity touch events to the Android View.

The AndroidJavaObject argument is an instance of **android.view.MotionEvent** (https://developer.android.com/reference/android/view/MotionEvent.html) and should be sent to the Android View through **AndroidJavaObject::Call<bool>( "dispatchTouchEvent", _obtainedMotionEvent_ )**.

### Creating Input Drivers ###

An example input system for Google Daydream View Controller will be included in a future update to UIView3D.

The AndroidInputDriver can be used as a reference for forwarding touch input to Android Views.

The main requirements for custom input is to obtain the UIViewProvider for forwarding touch inputs to the view itself. On Android, this is an instance of AndroidViewProvider forwarded to the Android View with DispatchMotionEvent (see above).

The UIView3DRenderer can be scrolled independently for smooth scrolling by modifying the **UIView3DRenderer.scroll : Vector2** variable. The UIView3DRenderer will attempt to clamp the scroll value to the bounds of the UIView.

## Current Limitations ##

Hardware acceleration is currently not available in WebView, as such video content and WebGL content is not available. A more complete WebViewProvider with video support will be included in a future update to UIView3D. WebGL content is not planned to be supported.

Android Views are drawn on a single thread. This is a platform limitation of Android. To reduce the strain on this thread, increase the number of rendering tiles and decrease the update rate of the *UIView3DRenderer* component, this is at the cost of slower View rendering.

Vulkan is not supported at this time. When Unity is updated with complete Vulkan support, then a Vulkan renderer will be implemented.
*If Vulkan support is required for your project, then please contact us (details below).*

## Contact ##

Website: https://altimit.systems/uiview3d
Email: unity@altimit.systems

## MIT License ##

Copyright (c) 2017 Altimit Systems LTD

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
