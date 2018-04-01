﻿/*
 * Copyright (C) 2016-2017 Ansuria Solutions LLC & Tommy Baggett: 
 * http://github.com/tbaggett
 * http://twitter.com/tbaggett
 * http://tommyb.com
 * http://ansuria.com
 * 
 * The MIT License (MIT) see GitHub For more information
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.ComponentModel;
using System.IO;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFGloss.iOS.Extensions;
using XFGloss.iOS.Views;

[assembly: ExportCell(typeof(EntryCell), typeof(XFGloss.iOS.Renderers.XFGlossEntryCellRenderer))]
[assembly: ExportCell(typeof(SwitchCell), typeof(XFGloss.iOS.Renderers.XFGlossSwitchCellRenderer))]
[assembly: ExportCell(typeof(TextCell), typeof(XFGloss.iOS.Renderers.XFGlossTextCellRenderer))]
[assembly: ExportCell(typeof(ImageCell), typeof(XFGloss.iOS.Renderers.XFGlossImageCellRenderer))]
[assembly: ExportCell(typeof(ViewCell), typeof(XFGloss.iOS.Renderers.XFGlossViewCellRenderer))]

namespace XFGloss.iOS.Renderers
{
	#region iOSXFGlossCellRenderer

	/// <summary>
	/// The iOS platform-specific XFGlossRenderer base class used for all <see cref="T:Xamarin.Forms.Cell"/> types.
	/// </summary>
	[Preserve(AllMembers = true)]
	internal class iOSXFGlossCellRenderer : XFGlossCellRenderer<UITableViewCell>, IGradientRenderer
	{
		#region IGradientRenderer implementation

		/// <summary>
		/// Implementation of method required by the <see cref="T:XFGloss.IXFGlossRenderer"/> interface that the
		/// <see cref="T:XFGloss.IGradientRenderer"/> interface extends. Applies the passed 
		/// <see cref="T:XFGloss.XFGlossElement"/> properties to the iOS UITableViewCell controls.
		/// </summary>
		/// <param name="propertyName">The name of the XFGloss attached BindableProperty that changed</param>
		/// <param name="element">The <see cref="T:XFGloss.XFGlossElement"/> instance that changed</param>
		/// <typeparam name="TElement">The type <see cref="T:XFGloss.XFGlossElement"/> that changed</typeparam>
		public virtual void CreateNativeElement<TElement>(string propertyName, TElement element) where TElement : XFGlossElement
		{
			// No need to check property name yet. BackgroundGradient is the only property currently supported.
			//if (propertyName == CellGloss.BackgroundGradientProperty.PropertyName && element is Gradient)
			//{
			if (element is Gradient)
			{
				CreateBackgroundGradientView(GetNativeCell(), element as Gradient);
			}
			//}
		}

		/// <summary>
		/// Implementation of method required by the <see cref="T:XFGloss.IXFGlossRenderer"/> interface that the
		/// <see cref="T:XFGloss.IGradientRenderer"/> interface extends. Indicates if there is an existing 
		/// implementation of the property specified by the propertyName parameter.
		/// </summary>
		/// <returns><c>true</c>, if an existing implementation is found, <c>false</c> otherwise.</returns>
		/// <param name="propertyName">The name of the XFGloss attached BindableProperty that changed</param>
		public virtual bool CanUpdate(string propertyName)
		{
			// No need to check property name yet. BackgroundGradient is the only property currently supported.
			var nativeCell = GetNativeCell();
			if (nativeCell != null)
			{
				return GetBackgroundGradientView(nativeCell) != null;
			}

			return false;
		}

		/// <summary>
		/// Implementation of method required by the <see cref="T:XFGloss.IXFGlossRenderer"/> interface that the
		/// <see cref="T:XFGloss.IGradientRenderer"/> interface extends. Removes any existing implementation of
		/// the property specified by the propertyName parameter.
		/// </summary>
		/// <param name="propertyName">The name of the XFGloss attached BindableProperty that changed</param>
		public virtual void RemoveNativeElement(string propertyName)
		{
			// No need to check property name yet. BackgroundGradient is the only property currently supported.
			var nativeCell = GetNativeCell();
			if (nativeCell != null)
			{
				RemoveBackgroundGradientView(nativeCell);
			}
		}

		/// <summary>
		/// Implementation of method required by the <see cref="T:XFGloss.IGradientRenderer"/> interface. Updates
		/// the rotation angle being used by any existing implementation of the property specified by the propertyName
		/// parameter.
		/// </summary>
		/// <param name="propertyName">The name of the XFGloss attached BindableProperty that changed</param>
		/// <param name="rotation">The new rotation value, an integer number between 0 and 359</param>
		public void UpdateRotation(string propertyName, int rotation)
		{
			// No need to check property name yet, BackgroundGradient is the only one being handled here.
			var nativeCell = GetNativeCell();
			if (nativeCell != null)
			{
				GetBackgroundGradientView(nativeCell)?.UpdateRotation(rotation);
			}
		}

		/// <summary>
		/// Implementation of method required by the <see cref="T:XFGloss.IGradientRenderer"/> interface. Updates
		/// the gradient fill steps being used by any existing implementation of the property specified by the 
		/// propertyName parameter.
		/// </summary>
		/// <param name="propertyName">The name of the XFGloss attached BindableProperty that changed</param>
		/// <param name="steps">The new collection of <see cref="T:XFGloss.GradientStep"/> instances that specify the
		/// colors and positions of each step of the gradient fill</param>
		public void UpdateSteps(string propertyName, GradientStepCollection steps)
		{
			// No need to check property name yet, BackgroundGradient is the only one being handled here.
			var nativeCell = GetNativeCell();
			if (nativeCell != null)
			{
				GetBackgroundGradientView(nativeCell)?.UpdateSteps(steps);
			}
		}

		/// <summary>
		/// Creates a new <see cref="T:XFGloss.iOS.Views.UIBackgroundGradientView"/> instance and assigns it as the
		/// background view to the passed UITableViewCell instance.
		/// </summary>
		/// <returns>The new <see cref="T:XFGloss.iOS.Views.UIBackgroundGradientView"/> instance</returns>
		/// <param name="nativeCell">The native UITableViewCell instance to attach the gradient view to</param>
		/// <param name="gradient">The <see cref="T:XFGloss.Gradient"/> instance to copy properties from</param>
		UIBackgroundGradientView CreateBackgroundGradientView(UITableViewCell nativeCell, Gradient gradient)
		{
			RemoveBackgroundGradientView(nativeCell);

			if (nativeCell != null)
			{
				nativeCell.BackgroundView = new UIBackgroundGradientView(CGRect.Empty, gradient);
			}
			return nativeCell?.BackgroundView as UIBackgroundGradientView;
		}

		/// <summary>
		/// Private helper method used to find and return a previously-created 
		/// <see cref="T:XFGloss.iOS.Views.UIBackgroundGradientView"/> instance if found, null if not found.
		/// </summary>
		/// <returns>The background gradient view if found, null if not.</returns>
		/// <param name="nativeCell">The native UITableViewCell view used to display the cell contents</param>
		UIBackgroundGradientView GetBackgroundGradientView(UITableViewCell nativeCell)
		{
			if (nativeCell != null && nativeCell.BackgroundView is UIBackgroundGradientView)
			{
				return nativeCell.BackgroundView as UIBackgroundGradientView;
			}

			return null;
		}

		/// <summary>
		/// Private helper method used to remove any previously-created 
		/// <see cref="T:XFGloss.iOS.Views.UIBackgroundGradientView"/> instance if found.
		/// </summary>
		/// <param name="nativeCell">The native iOS UITableViewCell used to display the cell contents</param>
		void RemoveBackgroundGradientView(UITableViewCell nativeCell)
		{
			if (nativeCell != null)
			{
				nativeCell.BackgroundView?.Dispose();
				nativeCell.BackgroundView = null;
			}
		}

		#endregion

		#region UpdateProperties

		/// <summary>
		/// Static method called by custom Xamarin.Forms renderers, used to direct the call to the cross-platform base 
		/// class and provide the required <see cref="T:XFGloss.XFGlossCellRenderer"/> factory method.
		/// </summary>
		/// <param name="cell">The associated <see cref="T:Xamarin.Forms.Cell"/> instance</param>
		/// <param name="nativeCell">The native UITableViewCell used to display the cell contents</param>
		public static void UpdateProperties(Cell cell, UITableViewCell nativeCell)
		{
			UpdateProperties(cell, nativeCell, () => new iOSXFGlossCellRenderer());
		}

		/// <summary>
		/// Implementation of the cross-platform base class's abstract UpdateProperties method. Used to apply the
		/// XFGloss attached BindableProperty values for the property specified by the propertyName parameter.
		/// </summary>
		/// <param name="cell">The associated <see cref="T:Xamarin.Forms.Cell"/> instance</param>
		/// <param name="nativeCell">The native UITableViewCell used to display the cell contents</param>
		/// <param name="propertyName">The name of the XFGloss attached BindableProperty that changed</param>
		protected override void UpdateProperties(Cell cell, UITableViewCell nativeCell, string propertyName)
		{
			// TintColor property - to be passed to CreateEditIndicatorAccessoryView and possibly others in the future
			if (propertyName == null || propertyName == CellGloss.TintColorProperty.PropertyName)
			{
				var tintColor = (Color)cell.GetValue(CellGloss.TintColorProperty);
				if (tintColor != Color.Default)
				{
					nativeCell.TintColor = tintColor.ToUIColor();
					if (nativeCell.AccessoryView != null)
					{
						UIColor uiColor = tintColor.ToUIColor();
						nativeCell.AccessoryView.TintColor = uiColor;
					}
				}
			}

			// BackgroundColor and BackgroundGradient properties
			// We shouldn't apply BOTH a background gradient and solid color. The gradient takes preference.
			Gradient bkgrndGradient = (Gradient)cell.GetValue(CellGloss.BackgroundGradientProperty);
			if (bkgrndGradient != null && bkgrndGradient.UpdateProperties(CellGloss.BackgroundGradientProperty.PropertyName, 
			                                                              this, propertyName))
			{
				// We don't need to handle BackgroundColor if a BackgroundGradient is assigned/updated
				return ;
			}

			if (propertyName == null || propertyName == CellGloss.BackgroundColorProperty.PropertyName)
			{
				Color bkgrndColor = (Color)cell.GetValue(CellGloss.BackgroundColorProperty);

				if (bkgrndColor != Color.Default)
				{
					UIColor uiColor = bkgrndColor.ToUIColor();

					// First check for a background color view being already assigned. Update it if found
					if (nativeCell.BackgroundView is UIBackgroundColorView &&
					    nativeCell.BackgroundView.BackgroundColor != uiColor)
					{
						nativeCell.BackgroundView.BackgroundColor = uiColor;
					}
					else
					{
						// Dispose of any previously assigned background gradient view before replacing it with a background color view
						if (nativeCell.BackgroundView is UIBackgroundGradientView)
						{
							nativeCell.BackgroundView.Dispose();
							nativeCell.BackgroundView = null;
						}

						UIBackgroundColorView bkgrndView = new UIBackgroundColorView(CGRect.Empty);
						bkgrndView.BackgroundColor = uiColor;

						nativeCell.BackgroundView = bkgrndView;
					}
				}
				else
				{
					// Dispose of any previously assigned background color view as a color is no longer assigned
					if (nativeCell.BackgroundView is UIBackgroundColorView)
					{
						nativeCell.BackgroundView.Dispose();
						nativeCell.BackgroundView = null;
					}
				}
			}
		}

		/// <summary>
		/// A marker class used to confirm if an instance is assigned to the UINativeCell.BackgroundView property
		/// </summary>
		class UIBackgroundColorView : UIView
		{
			public UIBackgroundColorView(CGRect rect) : base(rect) { }
		}

		#endregion
	}
	#endregion

	#region iOSXFGlossAccessoryCellRenderer

	/// <summary>
	/// The iOS platform-specific XFGloss cell renderer class used for the <see cref="T:Xamarin.Forms.Cell"/> based
	/// classes that support customizing the accessory view on the iOS platform.
	/// </summary>
	[Preserve(AllMembers = true)]
	internal class iOSXFGlossAccessoryCellRenderer : iOSXFGlossCellRenderer
	{
		WeakReference<UIView> _accessoryView;

		/// <summary>
		/// Static method called by the custom <see cref="T:Xamarin.Forms.Cell"/> renderer, used to direct the 
		/// call to the cross-platform base class and provide the required 
		/// <see cref="T:XFGloss.XFGlossCellRenderer"/> factory method. Hides the
		/// <see cref="T:XFGloss.iOS.Renderers.iOSXFGlossCellRenderer"/> base class's implementation of this method.
		/// </summary>
		/// <param name="cell">The associated <see cref="T:Xamarin.Forms.Cell"/> instance</param>
		/// <param name="nativeCell">The native UITableViewCell used to display the cell contents</param>
		new public static void UpdateProperties(Cell cell, UITableViewCell nativeCell)
		{
			UpdateProperties(cell, nativeCell, () => new iOSXFGlossAccessoryCellRenderer());
		}

		/// <summary>
		/// Override of the <see cref="T:XFGloss.iOS.Renderers.iOSXFGlossCellRenderer"/> base class's implementation
		/// of the ElementPropertyChanged method, checks the <see cref="T:XFGloss.CellGloss.AccessoryType"/> property
		/// </summary>
		/// <param name="sender">The object instance the notification was received from</param>
		/// <param name="args">The PropertyChanged event arguments</param>
		protected override void ElementPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			// Check all the properties that this implementation supports for changes
			if (args.PropertyName == CellGloss.AccessoryTypeProperty.PropertyName)
			{
				UpdateProperties(args.PropertyName);
			}

			base.ElementPropertyChanged(sender, args);
		}

		/// <summary>
		/// Override of the <see cref="T:XFGloss.iOS.Renderers.iOSXFGlossCellRenderer"/> base class's implementation
		/// of the UpdateProperties method, applies any <see cref="T:XFGloss.CellGloss.AccessoryType"/> property changes
		/// to the native UITableViewCell.
		/// </summary>
		/// <param name="cell">Cell.</param>
		/// <param name="nativeCell">Native cell.</param>
		/// <param name="propertyName">Property name.</param>
		protected override void UpdateProperties(Cell cell, UITableViewCell nativeCell, string propertyName)
		{
			// AccessoryType property
			if (propertyName == null ||
				propertyName == CellGloss.AccessoryTypeProperty.PropertyName)
			{
				var accessoryType = (CellGlossAccessoryType)cell.GetValue(CellGloss.AccessoryTypeProperty);
				UIView accView;
				if (_accessoryView != null && _accessoryView.TryGetTarget(out accView))
				{
					if (accessoryType != CellGlossAccessoryType.EditIndicator)
					{
						accView.Dispose();
						_accessoryView = null;
						nativeCell.AccessoryView = null;
					}
				}

				switch (accessoryType)
				{
					case CellGlossAccessoryType.None:
						nativeCell.Accessory = UITableViewCellAccessory.None;
						//nativeCell.AccessoryView = new UIView(new CGRect(0, 0, 20, 40));
						//_accessoryView = new WeakReference<UIView>(nativeCell.AccessoryView);
						break;

					case CellGlossAccessoryType.Checkmark:
						nativeCell.Accessory = UITableViewCellAccessory.Checkmark;
						break;

					// Disabled until we can access the detail button tapped method in the table view source
					// for both the ListView (currently not possible) and TableView (currently possible) classes.
					/*
					case CellAccessoryType.DetailButton:
						nativeCell.Accessory = UITableViewCellAccessory.DetailButton;
						break;

					case CellAccessoryType.DetailDisclosureButton:
						nativeCell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
						break;
					*/

					case CellGlossAccessoryType.DisclosureIndicator:
						nativeCell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
						break;

					case CellGlossAccessoryType.EditIndicator:
						var tintColor = (Color)cell.GetValue(CellGloss.TintColorProperty);
						if (!(nativeCell.AccessoryView is EditIndicatorView))
						{
							nativeCell.Accessory = UITableViewCellAccessory.None;

							accView = new EditIndicatorView(tintColor);
							if (accView != null)
							{
								nativeCell.AccessoryView = accView;
								_accessoryView = new WeakReference<UIView>(accView);
							}
						}
						else
						{
							(nativeCell.AccessoryView as EditIndicatorView).ApplyTintColor(tintColor);
						}
						break;
				}
			}

			base.UpdateProperties(cell, nativeCell, propertyName);
		}

		/// <summary>
		/// Class used to render our custom "EditingIndicator" accessory view type
		/// </summary>
		class EditIndicatorView : UIView
		{
			WeakReference<UIColor> _defaultTintColor = null;

			public EditIndicatorView(Color tintColor)
			{
				UserInteractionEnabled = false;

				BackgroundColor = UIColor.Clear;

				ApplyTintColor(tintColor);

				Frame = new CGRect(0, 0, 6, 32);
			}

			public void ApplyTintColor(Color tintColor)
			{
				// Assign the user's custom tint color if one is specified.
				if (tintColor != Color.Default)
				{
					// Store whatever tint color is assigned before we overwrite it so it can be restored if needed.
					if (_defaultTintColor == null)
					{
						_defaultTintColor = new WeakReference<UIColor>(TintColor);
					}

					TintColor = tintColor.ToUIColor();
				} // Handle users clearing their custom tint color by reassigning Color.Default
				else if (_defaultTintColor != null)
				{
					UIColor defaultColor = null;
					if (_defaultTintColor.TryGetTarget(out defaultColor))
					{
						TintColor = defaultColor;
					}
				}
			}

			public override void Draw(CGRect rect)
			{
				//get graphics context
				using (CGContext g = UIGraphics.GetCurrentContext())
				{

					//set up drawing attributes
					g.SetLineWidth(1);
					if (TintColor != null)
					{
						TintColor.SetFill();
						TintColor.SetStroke();
					}
					else
					{
						UIColor.Black.SetFill();
						UIColor.Black.SetStroke();
					}

					//create geometry
					var path = new CGPath();

					path.AddLines(new CGPoint[]{
								  new CGPoint (1, 30),
								  new CGPoint (5, 30),
								  new CGPoint (5, 26)});

					path.CloseSubpath();

					//add geometry to graphics context and draw it
					g.AddPath(path);
					g.DrawPath(CGPathDrawingMode.FillStroke);
				}

				base.Draw(rect);
			}
		}
	}

	#endregion

	#region iOSXFGlossSwitchCellRenderer

	/// <summary>
	/// The iOS platform-specific XFGloss cell renderer class used for the <see cref="T:Xamarin.Forms.SwitchCell"/>
	/// class.
	/// </summary>
	[Preserve(AllMembers = true)]
	internal class iOSXFGlossSwitchCellRenderer : iOSXFGlossCellRenderer
	{
		SwitchCellGloss _properties;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:XFGloss.iOS.Renderers.iOSXFGlossSwitchCellRenderer"/> class.
		/// </summary>
		/// <param name="bindable">Bindable.</param>
		public iOSXFGlossSwitchCellRenderer(BindableObject bindable)
		{
			_properties = new SwitchCellGloss(bindable);
		}

		/// <summary>
		/// Static method called by the custom <see cref="T:Xamarin.Forms.SwitchCell"/> renderer, used to direct the 
		/// call to the cross-platform base class and provide the required 
		/// <see cref="T:XFGloss.XFGlossCellRenderer"/> factory method. Hides the
		/// <see cref="T:XFGloss.iOS.Renderers.iOSXFGlossCellRenderer"/> base class's implementation of this method.
		/// </summary>
		/// <param name="cell">The associated <see cref="T:Xamarin.Forms.Cell"/> instance</param>
		/// <param name="nativeCell">The native iOS UITableViewCell used to display the cell contents</param>
		new public static void UpdateProperties(Cell cell, UITableViewCell nativeCell)
		{
			UpdateProperties(cell, nativeCell, () => new iOSXFGlossSwitchCellRenderer(cell));
		}

		/// <summary>
		/// Override of the <see cref="T:XFGloss.iOS.Renderers.iOSXFGlossCellRenderer"/> base class's implementation
		/// of the ElementPropertyChanged method, checks the XFGloss properties that are unique to the 
		/// <see cref="T:Xamarin.Forms.SwitchCell"/> class.
		/// </summary>
		/// <param name="sender">The object instance the notification was received from</param>
		/// <param name="args">The PropertyChanged event arguments</param>
		protected override void ElementPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			// Check all the properties that this implementation supports for changes
			if (args.PropertyName == SwitchCellGloss.OnTintColorProperty.PropertyName ||
			    args.PropertyName == SwitchCellGloss.ThumbTintColorProperty.PropertyName ||
			    args.PropertyName == SwitchCellGloss.ThumbOnTintColorProperty.PropertyName)
			{
				UpdateProperties(args.PropertyName);
			}

			// Special handling of state change to make XF Switch and Switch property names consistent
			if (args.PropertyName == SwitchCell.OnProperty.PropertyName)
			{
				base.UpdateProperties(Switch.IsToggledProperty.PropertyName);
			}

			base.ElementPropertyChanged(sender, args);
		}

		/// <summary>
		/// Override of the <see cref="T:XFGloss.iOS.Renderers.iOSXFGlossCellRenderer"/> base class's implementation
		/// of the UpdateProperties method, applies XFGloss property changes that are unique to the 
		/// <see cref="T:Xamarin.Forms.SwitchCell"/> class.
		/// </summary>
		/// <param name="cell">Cell.</param>
		/// <param name="nativeCell">Native cell.</param>
		/// <param name="propertyName">Property name.</param>
		protected override void UpdateProperties(Cell cell, UITableViewCell nativeCell, string propertyName)
		{
			if (nativeCell.AccessoryView is UISwitch)
			{
				var uiSwitch = nativeCell.AccessoryView as UISwitch;
				uiSwitch.UpdateColorProperty(_properties, propertyName);
			}

			base.UpdateProperties(cell, nativeCell, propertyName);
		}
	}
	#endregion

	#region Xamarin.Forms renderers

	/// <summary>
	/// Custom <see cref="T:Xamarin.Forms.EntryCellRenderer"/>-based renderer class used to apply the custom XFGloss
	/// properties to the iOS platform-specific implementation
	/// </summary>
	[Preserve(AllMembers = true)]
	public class XFGlossEntryCellRenderer : EntryCellRenderer
	{
		/// <summary>
		/// Override of the <see cref="EntryCellRenderer"/> GetCell method, used to apply any custom 
		/// settings to the iOS platform-specific cell display element.
		/// </summary>
		/// <returns>The iOS platform-specific cell display element after applying any custom settings to it</returns>
		/// <param name="item">The <see cref="T:Xamarin.Forms.Cell"/> instance whose properties need to be transferred 
		/// from</param>
		/// <param name="reusableCell">A previously-created iOS UITableViewCell if this cell has been rendered before
		/// </param>
		/// <param name="tv">The parent iOS UITableView</param>
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var nativeCell = base.GetCell(item, reusableCell, tv);
			iOSXFGlossAccessoryCellRenderer.UpdateProperties(item, nativeCell);

			return nativeCell;
		}
	}

	/// <summary>
	/// Custom <see cref="T:Xamarin.Forms.SwitchCellRenderer"/>-based renderer class used to apply the custom XFGloss
	/// properties to the iOS platform-specific implementation
	/// </summary>
	[Preserve(AllMembers = true)]
	public class XFGlossSwitchCellRenderer : SwitchCellRenderer
	{
		/// <summary>
		/// Override of the <see cref="SwitchCellRenderer"/> GetCell method, used to apply any custom 
		/// settings to the iOS platform-specific cell display element.
		/// </summary>
		/// <returns>The iOS platform-specific cell display element after applying any custom settings to it</returns>
		/// <param name="item">The <see cref="T:Xamarin.Forms.Cell"/> instance whose properties need to be transferred 
		/// from</param>
		/// <param name="reusableCell">A previously-created iOS UITableViewCell if this cell has been rendered before
		/// </param>
		/// <param name="tv">The parent iOS UITableView</param>
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var nativeCell = base.GetCell(item, reusableCell, tv);
			iOSXFGlossSwitchCellRenderer.UpdateProperties(item, nativeCell);
			return nativeCell;
		}
	}

	/// <summary>
	/// Custom <see cref="T:Xamarin.Forms.TextCellRenderer"/>-based renderer class used to apply the custom XFGloss
	/// properties to the iOS platform-specific implementation
	/// </summary>
	[Preserve(AllMembers = true)]
	public class XFGlossTextCellRenderer : TextCellRenderer
	{
		/// <summary>
		/// Override of the <see cref="TextCellRenderer"/> GetCell method, used to apply any custom 
		/// settings to the iOS platform-specific cell display element.
		/// </summary>
		/// <returns>The iOS platform-specific cell display element after applying any custom settings to it</returns>
		/// <param name="item">The <see cref="T:Xamarin.Forms.Cell"/> instance whose properties need to be transferred 
		/// from</param>
		/// <param name="reusableCell">A previously-created iOS UITableViewCell if this cell has been rendered before
		/// </param>
		/// <param name="tv">The parent iOS UITableView</param>
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var nativeCell = base.GetCell(item, reusableCell, tv);
			iOSXFGlossAccessoryCellRenderer.UpdateProperties(item, nativeCell);
			return nativeCell;
		}
	}

	/// <summary>
	/// Custom <see cref="T:Xamarin.Forms.ImageCellRenderer"/>-based renderer class used to apply the custom XFGloss
	/// properties to the iOS platform-specific implementation
	/// </summary>
	[Preserve(AllMembers = true)]
	public class XFGlossImageCellRenderer : ImageCellRenderer
	{
		/// <summary>
		/// Override of the <see cref="ImageCellRenderer"/> GetCell method, used to apply any custom 
		/// settings to the iOS platform-specific cell display element.
		/// </summary>
		/// <returns>The iOS platform-specific cell display element after applying any custom settings to it</returns>
		/// <param name="item">The <see cref="T:Xamarin.Forms.Cell"/> instance whose properties need to be transferred 
		/// from</param>
		/// <param name="reusableCell">A previously-created iOS UITableViewCell if this cell has been rendered before
		/// </param>
		/// <param name="tv">The parent iOS UITableView</param>
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var nativeCell = base.GetCell(item, reusableCell, tv);
			iOSXFGlossAccessoryCellRenderer.UpdateProperties(item, nativeCell);
			return nativeCell;
		}
	}

	/// <summary>
	/// Custom <see cref="T:Xamarin.Forms.ViewCellRenderer"/>-based renderer class used to apply the custom XFGloss
	/// properties to the iOS platform-specific implementation
	/// </summary>
	[Preserve(AllMembers = true)]
	public class XFGlossViewCellRenderer : ViewCellRenderer
	{
		/// <summary>
		/// Override of the <see cref="ViewCellRenderer"/> GetCell method, used to apply any custom 
		/// settings to the iOS platform-specific cell display element.
		/// </summary>
		/// <returns>The iOS platform-specific cell display element after applying any custom settings to it</returns>
		/// <param name="item">The <see cref="T:Xamarin.Forms.Cell"/> instance whose properties need to be transferred 
		/// from</param>
		/// <param name="reusableCell">A previously-created iOS UITableViewCell if this cell has been rendered before
		/// </param>
		/// <param name="tv">The parent iOS UITableView</param>
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var nativeCell = base.GetCell(item, reusableCell, tv);
			iOSXFGlossAccessoryCellRenderer.UpdateProperties(item, nativeCell);
			return nativeCell;
		}
	}
	#endregion
}