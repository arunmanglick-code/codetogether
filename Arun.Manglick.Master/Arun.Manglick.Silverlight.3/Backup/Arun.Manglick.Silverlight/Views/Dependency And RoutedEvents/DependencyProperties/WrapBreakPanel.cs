using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Arun.Manglick.Silverlight.Views.DependencyAndRoutedEvents.DependencyProperties
{
    public class WrapBreakPanel : Panel
    {
        #region DependencyProperty

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WrapBreakPanel),new PropertyMetadata(Orientation.Horizontal));
        public static DependencyProperty LineBreakBeforeProperty = DependencyProperty.RegisterAttached("LineBreakBefore", typeof(bool),typeof(WrapBreakPanel), null);

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetLineBreakBefore(UIElement element, Boolean value)
        {
            element.SetValue(LineBreakBeforeProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Boolean GetLineBreakBefore(UIElement element)
        {
            return (bool)element.GetValue(LineBreakBeforeProperty);
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            if (Orientation != Orientation.Horizontal)
                throw new NotImplementedException("The WrapBreakPanel only supports horizontal orientation.");

            Size currentLineSize = new Size();
            Size panelSize = new Size();

            // Examine all the elements in this panel.
            foreach (UIElement element in this.Children)
            {
                // Get the desired size of the element.
                element.Measure(constraint);
                Size desiredSize = element.DesiredSize;

                // Check if the element fits in the line, if given its desired size.
                if ((currentLineSize.Width + desiredSize.Width > constraint.Width) || (WrapBreakPanel.GetLineBreakBefore(element)))
                {
                    // Switch to a new line because space has run out.
                    panelSize.Height += currentLineSize.Height;
                    panelSize.Width = Math.Max(currentLineSize.Width, panelSize.Width);
                    currentLineSize = desiredSize;

                    // If the element is too wide to fit using the maximum width of the line,
                    // just give it a separate line.
                    if (desiredSize.Width > constraint.Width)
                    {
                        // Make the width of the element the new desired width.
                        panelSize.Width = Math.Max(desiredSize.Width, panelSize.Width);
                    }
                }
                else
                {
                    // Add the element to the current line.
                    currentLineSize.Width += desiredSize.Width;

                    // Make sure the line is as tall as its tallest element.
                    currentLineSize.Height = Math.Max(desiredSize.Height, currentLineSize.Height);
                }
            }

            // Return the size required to fit all elements.
            // Ordinarily, this is the width of the constraint, and the height
            // required to fit all the elements.
            // However, if an element is wider than the width given to the panel,
            // the desired width will be the width of that line.
            panelSize.Width = Math.Max(currentLineSize.Width, panelSize.Width);
            panelSize.Height += currentLineSize.Height;
            return panelSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrangeBounds"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (Orientation != Orientation.Horizontal)
                throw new NotImplementedException("The WrapBreakPanel only supports horizontal orientation.");

            Size currentLineSize = new Size();
            double totalHeight = 0;

            // Examine all the elements in this panel.
            foreach (UIElement element in this.Children)
            {
                // Get the desired size of the element, but don't call Measure() again,
                // or that will trigger the measure layout pass and MeasureOverride()!               
                Size desiredSize = element.DesiredSize;

                // Check if the element fits in the line, if given its desired size.
                if ((currentLineSize.Width + desiredSize.Width > arrangeBounds.Width) || (WrapBreakPanel.GetLineBreakBefore(element)))
                {
                    // Switch to a new line because space has run out.
                    totalHeight += currentLineSize.Height;
                    currentLineSize = new Size();
                }

                // Make sure the line is as tall as its tallest element.
                currentLineSize.Height = Math.Max(desiredSize.Height, currentLineSize.Height);

                // Place the element on the line, giving it its desired size.
                element.Arrange(new Rect(currentLineSize.Width, totalHeight,
                    element.DesiredSize.Width, element.DesiredSize.Height));

                // Move over for the next element.
                currentLineSize.Width += desiredSize.Width;
            }

            // Return the size this panel actually occupies.
            totalHeight += currentLineSize.Height;
            return new Size(arrangeBounds.Width, totalHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="lineHeight"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void arrangeLine(double y, double lineHeight, int start, int end)
        {
            double x = 0;
            UIElementCollection children = this.Children;
            for (int i = start; i < end; i++)
            {
                UIElement child = children[i];
                child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineHeight));
                x += child.DesiredSize.Width;
            }
        }

        #endregion

        // Alternate implementation gives full line height to all elements on that line.
        //protected override Size ArrangeOverride(Size arrangeBounds)
        //{
        //    Size currentLineSize = new Size();

        //    int firstInLine = 0;
        //    double totalHeight = 0;

        //    // Examine all the elements in this panel.            
        //    UIElementCollection elements = this.Children;
        //    for (int i=0; i < elements.Count; i++)
        //    {
        //        Size desiredSize = elements[i].DesiredSize;

        //        // Check if the element fits in the line, if given its desired size.
        //        if (currentLineSize.Width + desiredSize.Width > arrangeBounds.Width)
        //        {
        //            // Switch to a new line because space has run out.
        //            arrangeLine(totalHeight, currentLineSize.Height, firstInLine, i);

        //            totalHeight += currentLineSize.Height;
        //            currentLineSize = desiredSize;

        //            // If the element is too wide to fit using the maximum width of the line,
        //            // just give it a separate line.
        //            if (desiredSize.Width > arrangeBounds.Width)
        //            {
        //                arrangeLine(totalHeight, desiredSize.Height, i, ++i);
        //                totalHeight += desiredSize.Height;
        //                currentLineSize = new Size();
        //            }
        //            firstInLine = i;
        //        }
        //        else
        //        {
        //            // Add the element to the current line.
        //            currentLineSize.Width += desiredSize.Width;
        //            currentLineSize.Height = Math.Max(desiredSize.Height, currentLineSize.Height);
        //        }
        //    }

        //    if (firstInLine < elements.Count)
        //        arrangeLine(totalHeight, currentLineSize.Height, firstInLine, elements.Count);

        //    return arrangeBounds;
        //}
    }

}
