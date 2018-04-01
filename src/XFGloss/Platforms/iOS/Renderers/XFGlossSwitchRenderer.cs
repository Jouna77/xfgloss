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

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFGloss.iOS.Extensions;

[assembly: ExportRenderer(typeof(Switch), typeof(XFGloss.iOS.Renderers.XFGlossSwitchRenderer))]
namespace XFGloss.iOS.Renderers
{
	/// <summary>
	/// The iOS platform-specific Xamarin.Forms renderer used for all <see cref="T:Xamarin.Forms.Switch"/>
	/// derived classes.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class XFGlossSwitchRenderer : SwitchRenderer
	{
		SwitchGloss _properties;

		/// <summary>
		/// <see cref="T:Xamarin.Forms.Platform.iOS.SwitchRenderer"/> override that is called whenever the associated
		/// <see cref="T:Xamarin.Forms.Switch"/> instance changes
		/// </summary>
		/// <param name="e"><see cref="T:Xamarin.Forms.Platform.iOS.ElementChangedEventArgs"/> that specifies the
		/// previously and newly assigned <see cref="T:Xamarin.Forms.Switch"/> instances
		/// </param>
		protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
		{
			base.OnElementChanged(e);

			_properties = (e.NewElement != null) ? new SwitchGloss(e.NewElement) : null;

			if (Control != null && _properties != null)
			{
				Control.UpdateColorProperty(_properties, null);
			}
		}

		/// <summary>
		/// <see cref="T:Xamarin.Forms.Platform.iOS.SwitchRenderer"/> override that is called whenever the
		/// <see cref="T:Xamarin.Forms.Switch.PropertyChanged"/> event is fired
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (_properties != null)
			{
				if (e.PropertyName == SwitchGloss.TintColorProperty.PropertyName ||
				    e.PropertyName == SwitchGloss.OnTintColorProperty.PropertyName ||
				    e.PropertyName == SwitchGloss.ThumbTintColorProperty.PropertyName ||
				    e.PropertyName == SwitchGloss.ThumbOnTintColorProperty.PropertyName ||
				   	e.PropertyName == Switch.IsToggledProperty.PropertyName)
				{
					Control.UpdateColorProperty(_properties, e.PropertyName);
				}
			}

			base.OnElementPropertyChanged(sender, e);
		}
	}
}