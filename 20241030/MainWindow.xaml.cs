using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        // 當前操作模式，預設為 "draw"（繪圖模式）
        string actionType = "draw";
        // 當鼠標進入畫布時，根據當前操作模式設置光標
        private void myCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            if (actionType == "erase")
            {
                myCanvas.Cursor = Cursors.Hand; // 擦除模式下設置光標為手形
            }
            else
            {
                myCanvas.Cursor = Cursors.Pen; // 繪圖模式下設置光標為筆形
            }
        }
        // 當鼠標左鍵按下時，開始繪製形狀或進入擦除模式
        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = Cursors.Cross; // 按下鼠標左鍵時，設置光標為十字形
            start = e.GetPosition(myCanvas); // 獲取鼠標按下時的位置

            // 根據選擇的操作模式，進行相應的操作
            if (actionType == "draw")
            {
                // 根據選擇的形狀類型創建相應的形狀
                switch (shapeType)
                {
                    case "line":
                        // 繪製直線
                        Line line = new Line
                        {
                            X1 = start.X,
                            Y1 = start.Y,
                            X2 = dest.X,
                            Y2 = dest.Y,
                            StrokeThickness = 1, // 設定線條粗細
                            Stroke = Brushes.Gray // 設定線條顏色
                        };
                        myCanvas.Children.Add(line); // 將線條添加到畫布上
                        break;

                    case "rectangle":
                        // 繪製矩形
                        Rectangle rect = new Rectangle
                        {
                            Stroke = Brushes.Gray, // 設定矩形邊框顏色
                            Fill = Brushes.LightGray // 設定矩形填充顏色
                        };
                        myCanvas.Children.Add(rect); // 將矩形添加到畫布上
                        rect.SetValue(Canvas.LeftProperty, start.X); // 設定矩形左邊的位置
                        rect.SetValue(Canvas.TopProperty, start.Y);  // 設定矩形上邊的位置
                        break;

                    case "ellipse":
                        // 繪製圓形（橢圓）
                        Ellipse ellipse = new Ellipse
                        {
                            Stroke = Brushes.Gray, // 設定圓形邊框顏色
                            Fill = Brushes.LightGray // 設定圓形填充顏色
                        };
                        myCanvas.Children.Add(ellipse); // 將圓形添加到畫布上
                        ellipse.SetValue(Canvas.LeftProperty, start.X); // 設定圓形左邊的位置
                        ellipse.SetValue(Canvas.TopProperty, start.Y);  // 設定圓形上邊的位置
                        break;

                    case "polyline":
                        // 繪製折線（線段組）
                        Polyline polyline = new Polyline
                        {
                            Stroke = Brushes.Gray, // 設定折線邊框顏色
                            Fill = Brushes.LightGray // 設定折線填充顏色
                        };
                        myCanvas.Children.Add(polyline); // 將折線添加到畫布上
                        break;
                }
            }

            DisplayStatus(); // 顯示當前狀態
        }
        // 顯示當前的起始點、目標點和畫布中的形狀數量等狀態
        private void DisplayStatus()
        {
            // 顯示當前的起始點和目的點座標
            pointLabel.Content = $"({Convert.ToInt32(start.X)}, {Convert.ToInt32(start.Y)}) -  ({Convert.ToInt32(dest.X)}, {Convert.ToInt32(dest.Y)})";
            shapeLabel.Content = shapeType; // 顯示當前選擇的形狀類型
            // 計算並顯示畫布中各種形狀的數量
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
        // 當鼠標移動時，根據選擇的操作模式更新形狀的大小或進行擦除操作
        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            dest = e.GetPosition(myCanvas); // 獲取鼠標移動時的位置
            // 如果左鍵按下，更新形狀的大小或位置
            // 根據當前操作模式，進行不同的操作
            switch (actionType)
            {
                case "draw":
                    // 在繪圖模式下，如果鼠標左鍵按下，實時更新形狀的大小或位置
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        Point origin;
                        origin.X = Math.Min(start.X, dest.X); // 起始點和目標點的較小值作為矩形或圓形的起始位置
                        origin.Y = Math.Min(start.Y, dest.Y);
                        double width = Math.Abs(start.X - dest.X); // 計算寬度
                        double height = Math.Abs(start.Y - dest.Y); // 計算高度

                        switch (shapeType)
                        {
                            case "line":
                                var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                                line.X2 = dest.X; // 更新線條的終點 X
                                line.Y2 = dest.Y; // 更新線條的終點 Y
                                break;

                            case "rectangle":
                                var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                                rect.Width = width; // 更新矩形寬度
                                rect.Height = height; // 更新矩形高度
                                rect.SetValue(Canvas.LeftProperty, origin.X); // 設定矩形的起始位置
                                rect.SetValue(Canvas.TopProperty, origin.Y); // 設定矩形的上邊位置
                                break;

                            case "ellipse":
                                var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                                ellipse.Width = width; // 更新圓形寬度
                                ellipse.Height = height; // 更新圓形高度
                                ellipse.SetValue(Canvas.LeftProperty, origin.X); // 設定圓形起始位置
                                ellipse.SetValue(Canvas.TopProperty, origin.Y); // 設定圓形的上邊位置
                                break;

                            case "polyline":
                                var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                                polyline.Points.Add(dest); // 更新折線的終點
                                break;
                        }
                    }
                    break;

                case "erase":
                    // 在擦除模式下，檢查鼠標下的形狀並將其刪除
                    var shape = e.OriginalSource as Shape;
                    myCanvas.Children.Remove(shape); // 刪除該形狀
                    if (myCanvas.Children.Count == 0)
                    {
                        myCanvas.Cursor = Cursors.Arrow; // 當畫布中沒有形狀時，設置光標為箭頭
                    }    
                    break;
            }

            DisplayStatus(); // 更新當前狀態
        }
        // 當鼠標左鍵釋放時，完成形狀的繪製或擦除
        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 設置筆刷顏色和填充顏色
            Brush strokeBrush = new SolidColorBrush(strokeColor);
            Brush fillBrush = new SolidColorBrush(fillColor);

            switch (actionType)
            {
                case "draw":
                    // 根據選擇的形狀類型設置屬性
                    switch (shapeType)
                    {
                        case "line":
                            var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                            line.Stroke = strokeBrush; // 設定線條顏色
                            line.StrokeThickness = strokeThickness; // 設定線條粗細
                            break;

                        case "rectangle":
                            var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                            rect.Stroke = strokeBrush; // 設定矩形邊框顏色
                            rect.Fill = fillBrush; // 設定矩形填充顏色
                            rect.StrokeThickness = strokeThickness; // 設定矩形邊框粗細
                            break;

                        case "ellipse":
                            var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                            ellipse.Stroke = strokeBrush; // 設定圓形邊框顏色
                            ellipse.Fill = fillBrush; // 設定圓形填充顏色
                            ellipse.StrokeThickness = strokeThickness; // 設定圓形邊框粗細
                            break;

                        case "polyline":
                            var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                            polyline.Stroke = strokeBrush; // 設定折線邊框顏色
                            polyline.Fill = fillBrush; // 設定折線填充顏色
                            polyline.StrokeThickness = strokeThickness; // 設定折線邊框粗細
                            break;
                    }
                    break;

                case "erase":
                    break;
            }
        }

        // 更新筆刷粗細為滑桿的當前值
        private void strokeThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = Convert.ToInt32(strokeThicknessSlider.Value); // 更新筆刷粗細
        }

        // 當形狀選擇按鈕被點擊時，更新當前選擇的形狀類型
        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString(); // 根據選擇的按鈕標籤設定形狀類型
            actionType = "draw"; // 設置操作模式為繪製
            DisplayStatus(); // 更新顯示狀態
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
        // 清除畫布上的所有形狀
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear(); // 清除畫布上的所有子元素（形狀）
            DisplayStatus(); // 更新顯示狀態
        }
        // 切換到擦除模式
        private void EraseButton_Click(object sender, RoutedEventArgs e)
        {
            actionType = "erase"; // 設置操作模式為擦除
            if (myCanvas.Children.Count > 0)
            {
                myCanvas.Cursor = Cursors.Hand; // 如果畫布上有形狀，設置光標為手形
            }
            DisplayStatus(); // 更新顯示狀態
        }
        // 當形狀的單選按鈕被選中時，更新選擇的形狀類型
        private void ShapeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString(); // 更新選擇的形狀類型
            actionType = "draw"; // 設置操作模式為繪製
            DisplayStatus(); // 更新顯示狀態
        }

        private void SaveCanvasMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "儲存畫布",
                Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|All Files (*.*)|*.*",
                DefaultExt = ".png"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                int w = Convert.ToInt32(myCanvas.RenderSize.Width);
                int h = Convert.ToInt32(myCanvas.RenderSize.Height);

                // 創建 RenderTargetBitmap
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(w, h, 96d, 96d, PixelFormats.Pbgra32);

                // 渲染 Canvas
                renderBitmap.Render(myCanvas);

                // 選擇適當的 BitmapEncoder
                BitmapEncoder encoder;
                string extension = System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower();
                switch (extension)
                {
                    case ".jpg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    default:
                        encoder = new PngBitmapEncoder();
                        break;
                }

                // 將 RenderTargetBitmap 添加到編碼器的幀中
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                // 儲存影像到檔案
                using (FileStream outStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    encoder.Save(outStream);
                }
            }
        }
    }
}