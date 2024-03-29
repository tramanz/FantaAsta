﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FantaAsta.Resources.Controls
{
	/// <summary>
	/// Logica di interazione per StarsControl.xaml
	/// </summary>
	public partial class StarsControl : UserControl
	{
		#region Constants

		private const int STAR_DIM = 30;

		private static Thickness STAR_MARGIN = new Thickness(2);

		private static SolidColorBrush GOLD_BRUSH = new SolidColorBrush(Colors.DarkGoldenrod);

		#endregion

		#region Properties

		private static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(StarsControl), new PropertyMetadata(0D, (s, e) =>
		{
			StarsControl thisInstance = s as StarsControl;
			thisInstance.DrawStars();
		}));
		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private static readonly DependencyProperty MeanProperty = DependencyProperty.Register("Mean", typeof(double), typeof(StarsControl), new PropertyMetadata(0D, (s, e) =>
		{
			StarsControl thisInstance = s as StarsControl;
			thisInstance.DrawStars();
		}));
		public double Mean
		{
			get { return (double)GetValue(MeanProperty); }
			set { SetValue(MeanProperty, value); }
		}

		#endregion

		public StarsControl()
		{
			InitializeComponent();

			DrawStars();
		}

		#region Private methods

		private void DrawStars()
		{
			panel.Children.Clear();

			int paintedStars = 0;

			Geometry starGeometry = (Geometry)Application.Current.TryFindResource("StarIcon");
			double starGeometryWidth = starGeometry.Bounds.Width;
			double starGeometryHeight = starGeometry.Bounds.Height;

			Image starImage; GeometryDrawing starDrawing;

			if (Mean > 0 && Value > 0)
			{
				double starsToPaint = 2.5 * Value / Mean;
				double dimToPaint = starsToPaint * starGeometryWidth;
				int starsToPaintCompletely = Math.Min((int)Math.Floor(dimToPaint / starGeometryWidth), 5);

				for (int i = 0; i < starsToPaintCompletely; i++)
				{
					starDrawing = new GeometryDrawing(GOLD_BRUSH, new Pen(GOLD_BRUSH, 2), starGeometry);

					starImage = new Image
					{
						Source = new DrawingImage(starDrawing),
						Margin = STAR_MARGIN,
						Width = STAR_DIM,
						VerticalAlignment = VerticalAlignment.Center
					};

					_ = panel.Children.Add(starImage);
				}

				paintedStars = starsToPaintCompletely;

				if (starsToPaintCompletely < 5)
				{
					Geometry rectGeometry = new RectangleGeometry(new Rect(new Size(dimToPaint - (starsToPaintCompletely * starGeometryWidth), starGeometryHeight)));
					Geometry rectStarGeometry = new CombinedGeometry(GeometryCombineMode.Intersect, starGeometry, rectGeometry);

					DrawingGroup drawingGroup = new DrawingGroup();
					drawingGroup.Children.Add(new GeometryDrawing(GOLD_BRUSH, null, rectStarGeometry));
					drawingGroup.Children.Add(new GeometryDrawing(null, new Pen(GOLD_BRUSH, 2), starGeometry));

					starImage = new Image
					{
						Source = new DrawingImage(drawingGroup),
						Margin = STAR_MARGIN,
						Width = STAR_DIM,
						VerticalAlignment = VerticalAlignment.Center
					};

					_ = panel.Children.Add(starImage);

					paintedStars++;
				}
			}

			while (paintedStars < 5)
			{
				starDrawing = new GeometryDrawing(null, new Pen(GOLD_BRUSH, 2), starGeometry);

				starImage = new Image
				{
					Source = new DrawingImage(starDrawing),
					Margin = STAR_MARGIN,
					Width = STAR_DIM,
					VerticalAlignment = VerticalAlignment.Center
				};

				_ = panel.Children.Add(starImage);

				paintedStars++;
			}
		}

		#endregion
	}
}
