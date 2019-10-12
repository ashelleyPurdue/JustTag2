using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DP = System.Windows.DependencyProperty;

namespace JustTag2.Views
{
    /// <summary>
    /// Interaction logic for ResizablePanel.xaml
    /// </summary>
    public partial class ResizablePanel : UserControl
    {
        public static DP ResizeLeftProperty = MakeDP("ResizeLeft", false);
        public bool ResizeLeft
        {
            get => (bool)(GetValue(ResizeLeftProperty));
            set => SetValue(ResizeLeftProperty, value);
        }

        public static DP ResizeRightProperty  = MakeDP("ResizeRight", false);
        public bool ResizeRight
        {
            get => (bool)(GetValue(ResizeRightProperty));
            set => SetValue(ResizeRightProperty, value);
        }

        public static DP DraggedWidthProperty = MakeDP("DraggedWidth", 10.0);
        public double DraggedWidth
        {
            get => (double)GetValue(DraggedWidthProperty);
            set => SetValue(DraggedWidthProperty, value);
        }

        public static DP HandleThicknessProperty = MakeDP("HandleThickness", 4.0);
        public double HandleThickness
        {
            get => (double)GetValue(HandleThicknessProperty);
            set => SetValue(HandleThicknessProperty, value);
        }

        private bool isDragging = false;
        private double prevDragX;
        private MouseButton dragButton;     // The button the user started dragging with.

        // We use this to prevent the drag from stopping when
        // the user releases a button other than the one they
        // started dragging with.

        public ResizablePanel()
        {
            InitializeComponent();
        }

        private static DP MakeDP<T>(string name, T defaultVal = default) => DP.Register
        (
            name,
            typeof(T),
            typeof(ResizablePanel),
            new PropertyMetadata(defaultVal)
        );

        private void HorizontalDragStart(object sender, Point cursorPos, Action<IInputElement> captureDevice)
        {
            if (isDragging)
                return;

            // Begin dragging
            isDragging = true;
            prevDragX = cursorPos.X;
            captureDevice(sender as Border);
        }

        private void HorizontalDragMove(object sender, Point cursorPos)
        {
            if (!isDragging)
                return;

            // Calculate how much the mouse moved this frame
            var x = cursorPos.X;
            var diff = x - prevDragX;
            prevDragX = x;

            // HACK: flip it if dragging it from the left
            if ((sender as Border).Tag as string == "l")
                diff *= -1;

            // Adjust the width by that much
            DraggedWidth += diff;
        }

        private void HorizontalDragEnd(object sender, Action<IInputElement> releaseCapture)
        {
            if (!isDragging)
                return;

            // End dragging
            isDragging = false;
            (sender as Border).ReleaseMouseCapture();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Remember which button we started dragging with.
            // We should only stop dragging when the same button is released.
            if (!isDragging) dragButton = e.ChangedButton;
            HorizontalDragStart(sender, e.GetPosition(null), b => b.CaptureMouse());
        }
        private void Border_StylusDown(object s, StylusEventArgs e) => 
            HorizontalDragStart(s, e.GetPosition(null), b => b.CaptureStylus());

        private void Border_TouchDown(object s, TouchEventArgs e) =>
            HorizontalDragStart(s, e.GetTouchPoint(null).Position, b => e.TouchDevice.Capture(b));

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != dragButton) return;
            HorizontalDragEnd(sender, b => b.ReleaseMouseCapture());
        }
        private void Border_StylusUp(object s, StylusEventArgs e)   => HorizontalDragEnd(s, b => b.ReleaseStylusCapture());
        private void Border_TouchUp(object s, TouchEventArgs e)     => HorizontalDragEnd(s, b => e.TouchDevice.Capture(null));

        private void Border_MouseMove(object s, MouseEventArgs e)   => HorizontalDragMove(s, e.GetPosition(null));
        private void Border_StylusMove(object s, StylusEventArgs e) => HorizontalDragMove(s, e.GetPosition(null));
        private void Border_TouchMove(object s, TouchEventArgs e)   => HorizontalDragMove(s, e.GetTouchPoint(null).Position);
    }
}
