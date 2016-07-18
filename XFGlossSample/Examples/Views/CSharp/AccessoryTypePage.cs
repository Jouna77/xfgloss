﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XFGloss.Models;

namespace XFGlossSample.Examples.Views.CSharp
{
	public class AccessoryTypePage : ContentPage
	{
		public AccessoryTypePage()
		{
			var section = new TableSection();
			section.Add(CreateAccessoryTypeCells());

			Content = new StackLayout
			{
				Children =
				{
					new Label { Text = "Cell AccessoryType values set in C#:", Margin = new Thickness(10) },
					new TableView
					{
						Root = new TableRoot
						{
							section
						}
					}
				}
			};
		}

		Cell[] CreateAccessoryTypeCells()
		{
			List<Cell> result = new List<Cell>();

			// Iterate through the enumeration's values, creating a new text cell for each entity
			var typeNames = Enum.GetNames(typeof(CellAccessoryType));
			foreach (string typeName in typeNames)
			{
				Cell cell;
				if (typeName == nameof(CellAccessoryType.EditIndicator))
				{
					cell = new EntryCell();
				}
				else
				{
					cell = new TextCell();
				}

				if (cell is EntryCell)
				{
					var entryCell = cell as EntryCell;
					entryCell.Label = typeName;
					entryCell.Placeholder = "Optional";
					entryCell.HorizontalTextAlignment = TextAlignment.End;
				}
				else
				{
					(cell as TextCell).Text = typeName;
				}

				// Assign our gloss properties
				var accType = (CellAccessoryType)Enum.Parse(typeof(CellAccessoryType), typeName);

				// You can use the standard static setter...
				XFGloss.Views.Cell.SetAccessoryType(cell, accType);

				// ...or instantiate an instance of the Gloss properties you want to assign values to
				//	var gloss = new XFGloss.Views.Cell(cell);
				//	gloss.AccessoryType = accType;
				//	gloss.BackgroundColor = Color.Blue;
				//	gloss.TintColor = Color.Red;
				//	...

				result.Add(cell);
			}

			return result.ToArray();
		}
	}
}