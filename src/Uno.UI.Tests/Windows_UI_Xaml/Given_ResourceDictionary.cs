﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.Tests.App.Xaml;
using Uno.UI.Tests.Helpers;
using Uno.UI.Xaml;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using Colors = Windows.UI.Colors;

namespace Uno.UI.Tests.Windows_UI_Xaml
{
	[TestClass]
	public class Given_ResourceDictionary
	{
		[TestInitialize]
		public void Init()
		{
			UnitTestsApp.App.EnsureApplication();
		}

		[TestMethod]
		public void When_Simple_Add_And_Retrieve()
		{
			var rd = new ResourceDictionary();
			rd["Grin"] = new SolidColorBrush(Colors.DarkOliveGreen);

			var retrieved = rd["Grin"];

			Assert.IsTrue(rd.ContainsKey("Grin"));

			Assert.AreEqual(Colors.DarkOliveGreen, ((SolidColorBrush)retrieved).Color);

			rd.TryGetValue("Grin", out var retrieved2);
			Assert.AreEqual(Colors.DarkOliveGreen, ((SolidColorBrush)retrieved2).Color);
		}

		[TestMethod]
		public void When_Key_Not_Present()
		{
			var rd = new ResourceDictionary();

			//var retrieved = rd["Nope"]; //Throws on UWP

			;
		}

		[TestMethod]
		public void When_Merged()
		{
			var rdInner = new ResourceDictionary();
			rdInner["Grin"] = new SolidColorBrush(Colors.DarkOliveGreen);

			var rd = new ResourceDictionary();
			rd.MergedDictionaries.Add(rdInner);

			Assert.IsTrue(rd.ContainsKey("Grin"));

			var retrieved = rd["Grin"];
			Assert.AreEqual(Colors.DarkOliveGreen, ((SolidColorBrush)retrieved).Color);

			rd.TryGetValue("Grin", out var retrieved2);
			Assert.AreEqual(Colors.DarkOliveGreen, ((SolidColorBrush)retrieved2).Color);
		}

		[TestMethod]
		public void When_Deep_Merged()
		{
			var rd1 = new ResourceDictionary();
			rd1["Grin"] = new SolidColorBrush(Colors.DarkOliveGreen);

			var rd2 = new ResourceDictionary();
			rd2.MergedDictionaries.Add(rd1);

			var rd3 = new ResourceDictionary();
			rd3.MergedDictionaries.Add(rd2);

			var rd = new ResourceDictionary();
			rd.MergedDictionaries.Add(rd3);

			Assert.IsTrue(rd.ContainsKey("Grin"));

			Assert.IsFalse(rd.ContainsKey("Blu"));

			var retrieved = rd["Grin"];
			Assert.AreEqual(Colors.DarkOliveGreen, ((SolidColorBrush)retrieved).Color);

			rd.TryGetValue("Grin", out var retrieved2);
			Assert.AreEqual(Colors.DarkOliveGreen, ((SolidColorBrush)retrieved2).Color);
		}

		[TestMethod]
		public void When_Duplicates_Merged()
		{
			var rd1 = new ResourceDictionary();
			rd1["Grin"] = new SolidColorBrush(Colors.DarkOliveGreen);

			var rd2 = new ResourceDictionary();
			rd2["Grin"] = new SolidColorBrush(Colors.ForestGreen);

			var rd = new ResourceDictionary();
			rd.MergedDictionaries.Add(rd1);
			rd.MergedDictionaries.Add(rd2);

			var retrieved = rd["Grin"];

			//https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/resourcedictionary-and-xaml-resource-references#merged-resource-dictionaries
			Assert.AreEqual(Colors.ForestGreen, ((SolidColorBrush)retrieved).Color);
		}



		[TestMethod]
		public void When_Duplicates_Merged_And_Last_Is_Null()
		{
			var rd1 = new ResourceDictionary();
			rd1["Grin"] = new SolidColorBrush(Colors.DarkOliveGreen);

			var rd2 = new ResourceDictionary();
			rd2["Grin"] = null;

			var rd = new ResourceDictionary();
			rd.MergedDictionaries.Add(rd1);
			rd.MergedDictionaries.Add(rd2);

			var retrieved = rd["Grin"];

			//https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/resourcedictionary-and-xaml-resource-references#merged-resource-dictionaries
			Assert.IsNull(retrieved);

			Assert.IsTrue(rd.ContainsKey("Grin"));
		}

		[TestMethod]
		public void When_Has_Themes()
		{
			var rd = new ResourceDictionary();
			var dflt = new ResourceDictionary();
			rd.ThemeDictionaries["Default"] = dflt;

			dflt["Blu"] = new SolidColorBrush(Colors.DodgerBlue);

			var retrievedExplicit = (rd.ThemeDictionaries["Default"] as ResourceDictionary)["Blu"];
			Assert.AreEqual(Colors.DodgerBlue, ((SolidColorBrush)retrievedExplicit).Color);
			;

			var retrieved = rd["Blu"];
			Assert.AreEqual(Colors.DodgerBlue, ((SolidColorBrush)retrieved).Color);
			;
			Assert.IsTrue(rd.ContainsKey("Blu"));
		}

		[TestMethod]
		public void When_Has_Themes_And_Direct()
		{
			var rd = new ResourceDictionary();
			var dflt = new ResourceDictionary();
			rd.ThemeDictionaries["Default"] = dflt;

			dflt["Blu"] = new SolidColorBrush(Colors.DodgerBlue);

			rd["Blu"] = new SolidColorBrush(Colors.CornflowerBlue);

			var retrievedExplicit = (rd.ThemeDictionaries["Default"] as ResourceDictionary)["Blu"];
			Assert.AreEqual(Colors.DodgerBlue, ((SolidColorBrush)retrievedExplicit).Color);
			;

			var retrieved = rd["Blu"];
			Assert.AreEqual(Colors.CornflowerBlue, ((SolidColorBrush)retrieved).Color);
			;
		}

		[TestMethod]
		public void When_Has_Themes_And_Inherited_Direct()
		{
			var rd = new ResourceDictionary();
			var dflt = new ResourceDictionary();
			rd.ThemeDictionaries["Default"] = dflt;

			dflt["Blu"] = new SolidColorBrush(Colors.DodgerBlue);

			var rd2 = new ResourceDictionary();
			rd2["Blu"] = new SolidColorBrush(Colors.CornflowerBlue);
			rd.MergedDictionaries.Add(rd2);

			var retrievedExplicit = (rd.ThemeDictionaries["Default"] as ResourceDictionary)["Blu"];
			Assert.AreEqual(Colors.DodgerBlue, ((SolidColorBrush)retrievedExplicit).Color);
			;

			var retrieved = rd["Blu"];
			Assert.AreEqual(Colors.CornflowerBlue, ((SolidColorBrush)retrieved).Color);
			;
		}

		[TestMethod]
		public void When_Has_Inherited_Themes()
		{
			var rd = new ResourceDictionary();

			var rd2 = new ResourceDictionary();
			var dflt = new ResourceDictionary();
			rd2.ThemeDictionaries["Default"] = dflt;
			dflt["Blu"] = new SolidColorBrush(Colors.DodgerBlue);
			rd.MergedDictionaries.Add(rd2);
			;

			var retrieved = rd["Blu"];
			Assert.AreEqual(Colors.DodgerBlue, ((SolidColorBrush)retrieved).Color);
			;
			Assert.IsTrue(rd.ContainsKey("Blu"));
		}

		[TestMethod]
		public void When_Has_Multiple_Themes()
		{
#if !NETFX_CORE
			UnitTestsApp.App.EnsureApplication();			
#endif

			var rd = new ResourceDictionary();
			var light = new ResourceDictionary();
			light["Blu"] = new SolidColorBrush(Colors.LightBlue);
			var dark = new ResourceDictionary();
			dark["Blu"] = new SolidColorBrush(Colors.DarkBlue);

			rd.ThemeDictionaries["Light"] = light;
			rd.ThemeDictionaries["Dark"] = dark;

			Assert.IsTrue(rd.ContainsKey("Blu"));

			var retrieved1 = rd["Blu"];
			Assert.AreEqual(ApplicationTheme.Light, Application.Current.RequestedTheme);
			Assert.AreEqual(Colors.LightBlue, ((SolidColorBrush)retrieved1).Color);

#if !NETFX_CORE //Not legal on UWP to change theme after app has launched
			try
			{
				Application.Current.SetRequestedTheme(ApplicationTheme.Dark);
				var retrieved2 = rd["Blu"];
				Assert.AreEqual(Colors.DarkBlue, ((SolidColorBrush)retrieved2).Color);
			}
			finally
			{
				Application.Current.SetRequestedTheme(ApplicationTheme.Light);
			}
#endif
		}

		[TestMethod]
		public void When_Has_Inactive_Theme()
		{
#if !NETFX_CORE
			UnitTestsApp.App.EnsureApplication();
#endif
			var rd = new ResourceDictionary();
			var dark = new ResourceDictionary();
			dark["Blu"] = new SolidColorBrush(Colors.DarkBlue);

			rd.ThemeDictionaries["Dark"] = dark;

			Assert.AreEqual(ApplicationTheme.Light, Application.Current.RequestedTheme);
			Assert.IsFalse(rd.ContainsKey("Blu"));
			;
		}

		[TestMethod]
		public void When_Has_Default_And_Light()
		{
			UnitTestsApp.App.EnsureApplication();

			var rd = new ResourceDictionary();
			var dflt = new ResourceDictionary();
			dflt["Blu"] = new SolidColorBrush(Colors.Aqua);
			var light = new ResourceDictionary();

			rd.ThemeDictionaries["Default"] = dflt;
			rd.ThemeDictionaries["Light"] = light;

			Assert.AreEqual(ApplicationTheme.Light, Application.Current.RequestedTheme);

			Assert.IsFalse(rd.ContainsKey("Blu"));

			var inner = new ResourceDictionary();
			var lightInner = new ResourceDictionary();
			lightInner["Blu"] = new SolidColorBrush(Colors.DarkSlateBlue);
			inner.ThemeDictionaries["Light"] = lightInner;
			rd.MergedDictionaries.Add(inner);

			Assert.IsTrue(rd.ContainsKey("Blu"));

			Assert.AreEqual(Colors.DarkSlateBlue, (rd["Blu"] as SolidColorBrush).Color);
		}

		[TestMethod]
		public void When_Has_Custom_Theme()
		{
			var rd = new ResourceDictionary();
			var pink = new ResourceDictionary();
			pink["Color1"] = new SolidColorBrush(Colors.HotPink);

			rd.ThemeDictionaries["Pink"] = pink;

#if !NETFX_CORE
			UnitTestsApp.App.EnsureApplication();

			ApplicationHelper.RequestedCustomTheme = "Pink";

			Assert.IsTrue(rd.ContainsKey("Color1"));

			var retrieved = rd["Color1"];
			Assert.AreEqual(Colors.HotPink, ((SolidColorBrush)retrieved).Color);

			ApplicationHelper.RequestedCustomTheme = null;
#endif
		}

		[TestMethod]
		public void When_Resource_On_Top_Control_Xaml()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			var control = new Test_Control();
			app.HostView.Children.Add(control);

			var brush = control.Resources["LocalColorBrush"] as SolidColorBrush;
			Assert.AreEqual(Colors.Linen, brush.Color);
		}

		[TestMethod]
		public void When_Resource_On_Inner_Control_Xaml()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			var control = new Test_Control();
			app.HostView.Children.Add(control);

			var brush = control.TestGrid.Resources["LocalToGridColorBrush"] as SolidColorBrush;

			Assert.AreEqual(Colors.LimeGreen, brush.Color);
		}

		[TestMethod]
		public void When_Resource_In_Merged_Source_Xaml()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			AssertEx.AssertContainsColorBrushResource(app.Resources, "SuperiorColorBrush", Colors.MediumSpringGreen);
			AssertEx.AssertContainsColorBrushResource(app.Resources, "StrangeColorBrush", Colors.Gainsboro);
		}

		[TestMethod]
		public void When_Resource_In_Merged_Source_Xaml_Element()
		{
			var control = new Test_Control();

			AssertEx.AssertContainsColorBrushResource(control.TestGrid.Resources, "AbominableColorBrush", Colors.Teal);
		}

		[TestMethod]
		public void When_Resource_In_Merged_Source_Xaml_Check_Source()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			var source = app.Resources.MergedDictionaries.First().Source;
			Assert.AreEqual("/Files/App/Xaml/Test_Dictionary.xaml", source.AbsolutePath);
			Assert.AreEqual("ms-resource:///Files/App/Xaml/Test_Dictionary.xaml", source.AbsoluteUri);
		}

		[TestMethod]
		public void When_Resource_In_Merged_Inline_Xaml()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			AssertEx.AssertContainsColorBrushResource(app.Resources, "JustHangingOutInMergedDictColorBrush", Colors.Maroon);
			AssertEx.AssertContainsColorBrushResource(app.Resources, "HangingOutInRecursiveMergedColorBrush", Colors.DarkMagenta);
		}

		[TestMethod]
		public void When_Has_Themes_Inline_Xaml()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			AssertEx.AssertContainsColorBrushResource(app.Resources, "PlayfulColorBrush", Colors.RosyBrown);
			AssertEx.AssertContainsColorBrushResource(app.Resources, "LucrativeColorBrush", Colors.Azure);
		}

		[TestMethod]
		public void When_Has_Themes_Merged_Xaml()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			var dark = app.Resources.MergedDictionaries.First().ThemeDictionaries["Dark"] as ResourceDictionary;

			AssertEx.AssertContainsColorBrushResource(dark, "MoodyColorBrush", Colors.DarkKhaki);
		}

		[TestMethod]
		public void When_Different_Control_Instances()
		{
			var control1 = new Test_Control();
			var control2 = new Test_Control();
			Assert.IsTrue(control1.Resources.ContainsKey("LocalColorBrush"));
			Assert.IsTrue(control2.Resources.ContainsKey("LocalColorBrush"));

			Assert.IsFalse(ReferenceEquals(control1.Resources, control2.Resources));

			control2.Resources.Remove("LocalColorBrush");
			Assert.IsTrue(control1.Resources.ContainsKey("LocalColorBrush"));
			Assert.IsFalse(control2.Resources.ContainsKey("LocalColorBrush"));
		}

		[TestMethod]
		public void When_xName_In_Dictionary()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			AssertEx.AssertContainsColorBrushResource(app.Resources, "AliceTheColorBrush", Colors.AliceBlue);
		}

		[TestMethod]
		public void When_xName_In_Merged_Dictionary()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			AssertEx.AssertContainsColorBrushResource(app.Resources, "CadetColorBrush", Colors.CadetBlue);
		}

		[TestMethod]
		public void When_Resource_Referencing_Resource()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			Assert.AreEqual(ApplicationTheme.Light, app.RequestedTheme);

			AssertEx.AssertContainsColorBrushResource(app.Resources, "ReferenceInSameDictionaryColorBrush", Colors.Sienna);
			AssertEx.AssertContainsColorBrushResource(app.Resources, "LexicallyBackwardColorBrush", Colors.Coral);
		}

		[TestMethod]
		public void When_Enumerated()
		{
			var rd = new ResourceDictionary();
			rd["foot"] = "Boot";
			var inner = new ResourceDictionary();
			inner["hand"] = "Glove";
			rd.MergedDictionaries.Add(inner);
			var def = new ResourceDictionary();
			def["eyes"] = "Sunglasses";
			rd.ThemeDictionaries["Default"] = def;

			var keys = new List<object>();
			foreach (var kvp in rd)
			{
				keys.Add(kvp.Key);
			}

			Assert.IsTrue(keys.Contains("foot"));

			Assert.IsFalse(keys.Contains("hand"));
			Assert.IsTrue(rd.ContainsKey("hand"));

			Assert.IsFalse(keys.Contains("eyes"));
			Assert.IsTrue(rd.ContainsKey("eyes"));
		}

		[TestMethod]
		public void When_Implicit_Style_From_Code()
		{
			var control = new Test_Control();
			Assert.IsTrue(control.Resources.ContainsKey(typeof(RadioButton)));
			Assert.IsNotNull(control.Resources[typeof(RadioButton)] as Style);
		}

		[TestMethod]
		public void When_Merged_Dictionary_Local()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			Assert.IsFalse(app.Resources.ContainsKey("NotInAppResources")); // These dictionaries shouldn't end up in App.Resources, or this test won't test anything

			var control = new Test_Control();
			control.TestTextBlock2.DataContext = true;
			AssertEx.AssertHasColor(control.TestTextBlock2.Foreground, Colors.Orange);
		}

		[TestMethod]
		public void When_Merged_Dictionary_Style()
		{
			var app = UnitTestsApp.App.EnsureApplication();

			Assert.IsFalse(app.Resources.ContainsKey("NotInAppResources")); // These dictionaries shouldn't end up in App.Resources, or this test won't test anything

			var page = new Test_Page();

			app.HostView.Children.Add(page);

			page.Measure(new Size(1000, 1000));

			AssertEx.AssertHasColor(page.TestProgressRing.Foreground, Colors.Fuchsia);
		}

		[TestMethod]
		public void When_Local_Dictionary_Intra_Reference()
		{
			var control = new Test_Control();
			control.TestTextBlock2.DataContext = false;
			AssertEx.AssertHasColor(control.TestTextBlock2.Foreground, Colors.LimeGreen);
		}

		[TestMethod]
		public void When_Local_Lexically_Forward()
		{
			var page = new Test_Page();
			page.TestTextBlock2.DataContext = true;

			AssertEx.AssertHasColor(page.TestTextBlock2.Foreground, Colors.MidnightBlue);
		}

		[TestMethod]
		public void When_Direct_Local_Assignation_From_Source()
		{
			var page = new Test_Page();

			AssertEx.AssertContainsColorBrushResource(page.TestStackPanel.Resources, "NotInAppResources", Colors.Cyan);
		}

		[TestMethod]
		public void When_Created_From_Source_In_Codebehind()
		{
			var rd = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.UI.Tests.ViewLibrary/Themes/More/Test_Dictionary_Abs.xaml") };
			AssertEx.AssertContainsColorBrushResource(rd, "BituminousColorBrush", Colors.SlateGray);

			var rd2 = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.UI.Tests.ViewLibrary/Themes/More/Test_Dictionary_Abs.xaml") };

			rd2.Remove("BituminousColorBrush");
			Assert.IsFalse(rd2.ContainsKey("BituminousColorBrush"));
			Assert.IsTrue(rd.ContainsKey("BituminousColorBrush"));
		}

		[TestMethod]
		public void When_Created_From_Local_Source_In_Codebehind()
		{
			var rd = new ResourceDictionary { Source = new Uri("ms-resource:///Files/App/Xaml/Test_Dictionary.xaml") };
			AssertEx.AssertContainsColorBrushResource(rd, "SuperiorColorBrush", Colors.MediumSpringGreen);
		}

		[TestMethod]
		public void When_Created_From_Local_Source_In_Codebehind_AppX()
		{
			var rd = new ResourceDictionary { Source = new Uri("ms-appx:///App/Xaml/Test_Dictionary.xaml") };
			AssertEx.AssertContainsColorBrushResource(rd, "SuperiorColorBrush", Colors.MediumSpringGreen);
		}

		[TestMethod]
		public void When_External_Source()
		{
			var page = new Test_Page();
			var rp = page.TestRelativePanel;
			var rd = rp.Resources;
			AssertEx.AssertContainsColorBrushResource(rd, "BituminousColorBrush", Colors.SlateGray);
		}

		[TestMethod]
		public void When_Local_Source_Absolute()
		{
			var page = new Test_Page();
			var b = page.TestBorder;
			var rd = b.Resources;
			AssertEx.AssertContainsColorBrushResource(rd, "FerociousColorBrush", Colors.Fuchsia);
		}

		[TestMethod]
		public void When_Merged_Xaml_By_Type()
		{
			var page = new Test_Page();
			Assert.AreEqual("Hakuna Matata", page.testSubclassedDictionaryTextBlock.Text);
		}

		[TestMethod]
		public void When_Xaml_By_Type_Ref_Equality()
		{
			var page = new Test_Page_Other();

			Assert.IsInstanceOfType(page.testGrid1.Resources, typeof(Subclassed_Dictionary));

			var color = Colors.WhiteSmoke;
			var b1 = page.testBorder1.Background;
			var b2 = page.testBorder2.Background;
			AssertEx.AssertHasColor(b1, color);
			AssertEx.AssertHasColor(b2, color);

			var areRefEqual = ReferenceEquals(b1, b2);
			Assert.IsFalse(areRefEqual);
		}

		[TestMethod]
		public void When_By_Type_From_Code()
		{
			var dict = new Subclassed_Dictionary();

			Assert.AreEqual("Hakuna Matata", dict["ProblemFreePhilosophy"]);
			AssertEx.AssertContainsColorBrushResource(dict, "PerilousColorBrush", Colors.WhiteSmoke);

			Assert.AreEqual("The Cold", dict["NeverBotheredMeAnyway"]);
		}

		[TestMethod]
		public void When_By_Type_With_Template()
		{
			var dict = new Subclassed_Dictionary();

			var template = dict["UproariousTemplate"] as DataTemplate;
			Assert.IsNotNull(template);
			var content = template.LoadContent();
			Assert.IsInstanceOfType(content, typeof(CheckBox));
		}

		[TestMethod]
		public void When_By_xName_And_Key_In_Element_Resources()
		{
			var testControl = new Test_Control();

			Assert.AreEqual(2, testControl.SubliminalGradientBrushByName.GradientStops.Count);

			var fromResources = testControl.Resources["SubliminalGradientBrush"] as LinearGradientBrush;
			Assert.IsNotNull(fromResources);
			Assert.AreEqual(2, fromResources.GradientStops.Count);
		}

		[TestMethod]
		public void When_Accessing_System_Resource()
		{
			var rd = new ResourceDictionary();

			Assert.IsTrue(rd.ContainsKey("SystemAltHighColor"));
			var systemColor = (Color)rd["SystemAltHighColor"];
			Assert.AreEqual(Colors.White, systemColor);
		}

		[TestMethod]
		public void When_Relative_Path_With_Leading_Slash_From_Root()
		{
			var withSlash = XamlFilePathHelper.ResolveAbsoluteSource("App.xaml", "/App/Xaml/Test_Dictionary.xaml");
			var withoutSlash = XamlFilePathHelper.ResolveAbsoluteSource("App.xaml", "App/Xaml/Test_Dictionary.xaml");

			Assert.AreEqual(withoutSlash, withSlash);
		}

		[TestMethod]
		public void When_XamlControlsResources()
		{
			var xcr = new Microsoft.UI.Xaml.Controls.XamlControlsResources();
			Assert.IsTrue(xcr.ContainsKey(typeof(Button)));
			Assert.IsInstanceOfType(xcr[typeof(Button)], typeof(Style));
		}
	}
}
