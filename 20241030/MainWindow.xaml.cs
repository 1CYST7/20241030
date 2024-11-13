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
        // 記錄起始點和目的點
        Point start = new Point { X = 0, Y = 0 };
        Point dest = new Point { X = 0, Y = 0 };
        Color strokeColor = Colors.Red; // 預設筆刷顏色為紅色
        Color fillColor = Colors.Aqua;
        int strokeThickness = 1; // 預設筆刷粗細為1
        string shapeType = "line"; // 當前選擇的形狀類型

        private void myCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            myCanvas.Cursor = Cursors.Pen;// 當鼠標進入畫布時，設置鼠標光標為筆形
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = Cursors.Cross; // 按下鼠標左鍵時，設置光標為十字形
            start = e.GetPosition(myCanvas); // 獲取鼠標按下時的位置
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
                    break;
            }
            DisplayStatus();
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
                        break;
                }
            }
            DisplayStatus();
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (shapeType)
            {
                case "line":
                    var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                    line.Stroke = new SolidColorBrush(strokeColor);
                    line.StrokeThickness = strokeThickness;
                    break;
                case "rectangle":
                    var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                    rect.Stroke = new SolidColorBrush(strokeColor);
                    rect.Fill = new SolidColorBrush(fillColor);
                    rect.StrokeThickness = strokeThickness;
                    break;
                case "ellipse":
                    var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                    ellipse.Stroke = new SolidColorBrush(strokeColor);
                    ellipse.Fill = new SolidColorBrush(fillColor);
                    ellipse.StrokeThickness = strokeThickness;
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

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton; // 獲取被點擊的單選按鈕
            shapeType = targetRadioButton.Tag.ToString(); // 獲取該單選按鈕的標籤作為形狀類型
            shapeLabel.Content = shapeType; // 更新狀態顯示為當前選擇的形狀類型
        }
        private void StrokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = strokeColorPicker.SelectedColor.Value;
        }

        private void FillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = fillColorPicker.SelectedColor.Value;
        }
    }
}