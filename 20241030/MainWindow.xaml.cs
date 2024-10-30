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
        int strokeThickness = 1; // 預設筆刷粗細為1
        string shapeType = ""; // 當前選擇的形狀類型

        private void myCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            myCanvas.Cursor = Cursors.Pen;// 當鼠標進入畫布時，設置鼠標光標為筆形
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = Cursors.Cross; // 按下鼠標左鍵時，設置光標為十字形
            start = e.GetPosition(myCanvas); // 獲取鼠標按下時的位置
            DisplayStatus(start, dest); // 更新狀態顯示
        }

        private void DisplayStatus(Point start, Point dest)
        {
            // 顯示當前的起始點和目的點座標
            pointLabel.Content = $"({Convert.ToInt32(start.X)}, {Convert.ToInt32(start.Y)}) -  ({Convert.ToInt32(dest.X)}, {Convert.ToInt32(dest.Y)})";
        }

        public MainWindow()
        {
            InitializeComponent();
            // 設置顏色選擇器的預設顏色
            strokeColorPicker.SelectedColor = strokeColor;
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            dest = e.GetPosition(myCanvas); // 獲取鼠標移動時的位置
            DisplayStatus(start, dest); // 更新狀態顯示
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 創建新的畫筆並設置屬性
            Brush brush = new SolidColorBrush(strokeColor);
            Line line = new Line
            {
                Stroke = brush, // 設置畫筆顏色
                StrokeThickness = strokeThickness, // 設置畫筆粗細
                X1 = start.X, // 起始點 X 座標
                Y1 = start.Y, // 起始點 Y 座標
                X2 = dest.X, // 目的點 X 座標
                Y2 = dest.Y  // 目的點 Y 座標
            };
            myCanvas.Children.Add(line); // 將新繪製的線添加到畫布上
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
    }
}