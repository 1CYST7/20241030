using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2024_WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 記錄起始點和目的點的位置
        Point start = new Point { X = 0, Y = 0 };
        Point dest = new Point { X = 0, Y = 0 };

        // 預設筆刷顏色（紅色），及填充顏色（Aqua）
        Color strokeColor = Colors.Red;
        Color fillColor = Colors.Aqua;

        // 預設筆刷粗細為1
        int strokeThickness = 1;

        // 當前選擇的形狀類型，預設為 "line"
        string shapeType = "line";
        string actionType = "draw";
        private void myCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            if (actionType == "erase")
            {
                myCanvas.Cursor = Cursors.Hand;
            }
            else
            {
                myCanvas.Cursor = Cursors.Pen;// 當鼠標進入畫布時，設置鼠標光標為筆形
            }
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = Cursors.Cross; // 按下鼠標左鍵時，設置光標為十字形
            start = e.GetPosition(myCanvas); // 獲取鼠標按下時的位置
                                             // 根據選擇的形狀類型，創建相應的形狀
            if (actionType == "draw")
            {
                switch (shapeType)
                {
                    case "line":
                        Line line = new Line
                        {
                            X1 = start.X,
                            Y1 = start.Y,
                            X2 = dest.X,
                            Y2 = dest.Y,
                            StrokeThickness = 1,
                            Stroke = Brushes.Gray
                        };
                        myCanvas.Children.Add(line);
                        break;

                    case "rectangle":
                        Rectangle rect = new Rectangle
                        {
                            Stroke = Brushes.Gray,
                            Fill = Brushes.LightGray
                        };
                        myCanvas.Children.Add(rect);
                        rect.SetValue(Canvas.LeftProperty, start.X);
                        rect.SetValue(Canvas.TopProperty, start.Y);
                        break;

                    case "ellipse":
                        Ellipse ellipse = new Ellipse
                        {
                            Stroke = Brushes.Gray,
                            Fill = Brushes.LightGray
                        };
                        myCanvas.Children.Add(ellipse);
                        ellipse.SetValue(Canvas.LeftProperty, start.X);
                        ellipse.SetValue(Canvas.TopProperty, start.Y);
                        break;

                    case "polyline":
                        Polyline polyline = new Polyline
                        {
                            Stroke = Brushes.Gray,
                            Fill = Brushes.LightGray
                        };
                        myCanvas.Children.Add(polyline);
                        break;
                }
            }

            DisplayStatus();
        }

        private void DisplayStatus()
        {
            // 顯示當前的起始點和目的點座標
            pointLabel.Content = $"({Convert.ToInt32(start.X)}, {Convert.ToInt32(start.Y)}) -  ({Convert.ToInt32(dest.X)}, {Convert.ToInt32(dest.Y)})";
            shapeLabel.Content = shapeType;
            int lineCount = myCanvas.Children.OfType<Line>().Count();
            int rectCount = myCanvas.Children.OfType<Rectangle>().Count();
            int ellipseCount = myCanvas.Children.OfType<Ellipse>().Count();
            int polylineCount = myCanvas.Children.OfType<Polyline>().Count();

            statusLabel.Content = $"工作模式：{actionType}, Line:{lineCount}, Rectangle:{rectCount}, Ellipse:{ellipseCount}, Polyline:{polylineCount}";
        }

        public MainWindow()
        {
            InitializeComponent();
            // 設置顏色選擇器的預設顏色
            strokeColorPicker.SelectedColor = strokeColor;
            fillColorPicker.SelectedColor = fillColor;
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            dest = e.GetPosition(myCanvas); // 獲取鼠標移動時的位置
            // 如果左鍵按下，更新形狀的大小或位置
            switch (actionType)
            {
                case "draw":
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        Point origin;
                        origin.X = Math.Min(start.X, dest.X);
                        origin.Y = Math.Min(start.Y, dest.Y);
                        double width = Math.Abs(start.X - dest.X);
                        double height = Math.Abs(start.Y - dest.Y);

                        switch (shapeType)
                        {
                            case "line":
                                var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                                line.X2 = dest.X;
                                line.Y2 = dest.Y;
                                break;

                            case "rectangle":
                                var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                                rect.Width = width;
                                rect.Height = height;
                                rect.SetValue(Canvas.LeftProperty, origin.X);
                                rect.SetValue(Canvas.TopProperty, origin.Y);
                                break;

                            case "ellipse":
                                var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                                ellipse.Width = width;
                                ellipse.Height = height;
                                ellipse.SetValue(Canvas.LeftProperty, origin.X);
                                ellipse.SetValue(Canvas.TopProperty, origin.Y);
                                break;

                            case "polyline":
                                var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                                polyline.Points.Add(dest);
                                break;
                        }
                    }
                    break;

                case "erase":
                    var shape = e.OriginalSource as Shape;
                    myCanvas.Children.Remove(shape);
                    if (myCanvas.Children.Count == 0)
                        myCanvas.Cursor = Cursors.Arrow;
                    break;
            }

            DisplayStatus();
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Brush strokeBrush = new SolidColorBrush(strokeColor);
            Brush fillBrush = new SolidColorBrush(fillColor);

            switch (actionType)
            {
                case "draw":
                    switch (shapeType)
                    {
                        case "line":
                            var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                            line.Stroke = strokeBrush;
                            line.StrokeThickness = strokeThickness;
                            break;

                        case "rectangle":
                            var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                            rect.Stroke = strokeBrush;
                            rect.Fill = fillBrush;
                            rect.StrokeThickness = strokeThickness;
                            break;

                        case "ellipse":
                            var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                            ellipse.Stroke = strokeBrush;
                            ellipse.Fill = fillBrush;
                            ellipse.StrokeThickness = strokeThickness;
                            break;

                        case "polyline":
                            var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                            polyline.Stroke = strokeBrush;
                            polyline.Fill = fillBrush;
                            polyline.StrokeThickness = strokeThickness;
                            break;
                    }
                    break;

                case "erase":
                    break;
            }
        }

        private void strokeThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // 更新筆刷粗細為滑桿的當前值
            strokeThickness = Convert.ToInt32(strokeThicknessSlider.Value);
        }

        // 當形狀選擇按鈕被點擊時，更新當前選擇的形狀類型
        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString();
            actionType = "draw";
            DisplayStatus();
        }
        // 當筆刷顏色選擇器的顏色改變時，更新筆刷顏色
        private void StrokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = strokeColorPicker.SelectedColor.Value; // 更新筆刷顏色
        }

        // 當填充顏色選擇器的顏色改變時，更新填充顏色
        private void FillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = fillColorPicker.SelectedColor.Value; // 更新填充顏色
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            DisplayStatus();
        }

        private void EraseButton_Click(object sender, RoutedEventArgs e)
        {
            actionType = "erase";
            if (myCanvas.Children.Count > 0)
            {
                myCanvas.Cursor = Cursors.Hand;
            }
            DisplayStatus();
        }

        private void ShapeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString();
            actionType = "draw";
            DisplayStatus();
        }
    }
}