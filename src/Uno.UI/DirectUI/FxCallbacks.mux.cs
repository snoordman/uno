﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// MUX Reference dxaml\xcp\dxaml\lib\FxCallbacks.cpp, tag winui3/release/1.4.3, commit 685d2bf

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace Uno.UI.DirectUI;

internal static class FxCallbacks
{
	internal static bool KeyboardAccelerator_RaiseKeyboardAcceleratorInvoked(
		KeyboardAccelerator pNativeAccelerator,
		DependencyObject pElement) =>
		KeyboardAccelerator.RaiseKeyboardAcceleratorInvoked(pNativeAccelerator, pElement);

	internal static void UIElement_RaiseProcessKeyboardAccelerators(
		UIElement pUIElement,
		VirtualKey key,
		VirtualKeyModifiers keyModifiers,
		ref bool pHandled,
		ref bool pHandledShouldNotImpedeTextInput) =>
		UIElement.RaiseProcessKeyboardAcceleratorsStatic(pUIElement, key, keyModifiers, ref pHandled, ref pHandledShouldNotImpedeTextInput);
}