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

        private void myCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            myCanvas.Cursor = Cursors.Pen;// 當鼠標進入畫布時，設置鼠標光標為筆形
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = Cursors.Cross; // 按下鼠標左鍵時，設置光標為十字形
            start = e.GetPosition(myCanvas); // 獲取鼠標按下時的位置
                                             // 根據選擇的形狀類型，創建相應的形狀
            switch (shapeType)
            {
                case "line":
                
                    Line line = new Line
                    {
                        X1 = start.X,  // 起點 X
                        Y1 = start.Y,  // 起點 Y
                        X2 = dest.X,   // 目標點 X
                        Y2 = dest.Y,   // 目標點 Y
                        StrokeThickness = 1, // 線條粗細為1
                        Stroke = Brushes.Gray // 線條顏色為灰色
                    };
                    myCanvas.Children.Add(line); // 將線條添加到畫布上
                    break;
                case "rectangle":
                 
                    Rectangle rect = new Rectangle
                    {
                        Stroke = Brushes.Gray,  // 邊框顏色為灰色
                        Fill = Brushes.LightGray // 填充顏色為淡灰色
                    };
                    myCanvas.Children.Add(rect); // 將矩形添加到畫布上
                    rect.SetValue(Canvas.LeftProperty, start.X); // 設置矩形左邊的位置
                    rect.SetValue(Canvas.TopProperty, start.Y);  // 設置矩形上邊的位置
                    break;
                case "ellipse":
                 
                    Ellipse ellipse = new Ellipse
                    {
                        Stroke = Brushes.Gray,  // 邊框顏色為灰色
                        Fill = Brushes.LightGray // 填充顏色為淡灰色
                    };
                    myCanvas.Children.Add(ellipse); // 將圓形添加到畫布上
                    ellipse.SetValue(Canvas.LeftProperty, start.X); // 設置圓形左邊的位置
                    ellipse.SetValue(Canvas.TopProperty, start.Y);  // 設置圓形上邊的位置
                    break;
                case "polyline":
                    // 畫折線，尚未實作
                    break;
            }
            DisplayStatus(); // 顯示當前的起始點和目標點座標
        }

        private void DisplayStatus()
        {
            // 顯示當前的起始點和目的點座標
            pointLabel.Content = $"({Convert.ToInt32(start.X)}, {Convert.ToInt32(start.Y)}) -  ({Convert.ToInt32(dest.X)}, {Convert.ToInt32(dest.Y)})";
            shapeLabel.Content = shapeType;
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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point origin;
                origin.X = Math.Min(start.X, dest.X); // 起始點 X 和目標點 X 的較小值
                origin.Y = Math.Min(start.Y, dest.Y); // 起始點 Y 和目標點 Y 的較小值
                double width = Math.Abs(start.X - dest.X); // 計算寬度
                double height = Math.Abs(start.Y - dest.Y); // 計算高度

                // 根據選擇的形狀類型，實時更新形狀的大小
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
                        break;
                }
            }
            DisplayStatus(); // 更新狀態顯示
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (shapeType)
            {
                case "line":
                    var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                    line.Stroke = new SolidColorBrush(strokeColor); // 設置線條顏色
                    line.StrokeThickness = strokeThickness; // 設置線條粗細
                    break;
                case "rectangle":
                    var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                    rect.Stroke = new SolidColorBrush(strokeColor); // 設置矩形邊框顏色
                    rect.Fill = new SolidColorBrush(fillColor); // 設置矩形填充顏色
                    rect.StrokeThickness = strokeThickness; // 設置矩形邊框粗細
                    break;
                case "ellipse":
                    var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                    ellipse.Stroke = new SolidColorBrush(strokeColor); // 設置圓形邊框顏色
                    ellipse.Fill = new SolidColorBrush(fillColor); // 設置圓形填充顏色
                    ellipse.StrokeThickness = strokeThickness; // 設置圓形邊框粗細
                    break;
                case "polyline":
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
            var targetRadioButton = sender as RadioButton; // 獲取被點擊的單選按鈕
            shapeType = targetRadioButton.Tag.ToString(); // 根據單選按鈕的標籤設置形狀類型
            shapeLabel.Content = shapeType; // 更新形狀類型顯示
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
    }
}