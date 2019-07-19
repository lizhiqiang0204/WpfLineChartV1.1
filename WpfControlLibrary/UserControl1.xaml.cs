using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfControlLibrary
{
    public class UserControl1_Para
    {
        public int xGrap;//X轴每隔多少个像素放一个点
        public int xGrapLable;//X轴每隔多少个像素放一个标签
        public int xGrapLablePoint;//X轴相邻时间标签内有多少个采集点
        public double xPointRight;//X点据右侧多少距离开始采集，右边留出一点空白为了美观
        public double xWidth;//在整个画布中所有可显示的点占据多少个像素，从左侧开始从0算起

        public double yStartValue;//Y轴起始值标签值
        public double yGrap;//Y轴相邻两个标签的差值
        public int yGrapLable;//Y轴每隔多少个像素放一个标签
        public int yLables;//Y轴可以显示多少个标签

    }
    public class CollectPoint //定义集合类  //定义采集点的类，包括横纵坐标，横坐标对应的时间
    {
        public Point point;
        public DateTime curTime;//当前点的时间
    }


    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        UserControl1_Para userControl1_Para = new UserControl1_Para();
        List<CollectPoint> collectPoints = new List<CollectPoint>();
        double x = 0;
        public UserControl1()
        {
            InitializeComponent();
        }

        public void myCanvasInit(UserControl1_Para para)
        {
            userControl1_Para.xGrap             = para.xGrap;
            userControl1_Para.xGrapLable        = para.xGrapLable;
            userControl1_Para.xGrapLablePoint   = para.xGrapLablePoint;
            userControl1_Para.yGrap             = para.yGrap;
            userControl1_Para.yGrapLable        = para.yGrapLable;
            userControl1_Para.yLables           = para.yLables;
            userControl1_Para.yStartValue       = para.yStartValue;
            userControl1_Para.xPointRight       = para.xPointRight;
            userControl1_Para.xWidth            = para.xWidth;


            for (int i = 0; i <= para.yLables; i++)
            {
                TextBlock yText = new TextBlock();
                yText.Text = (para.yStartValue+(i*para.yGrap)).ToString();
                yText.Margin = new Thickness(0, i * para.yGrapLable, 0, 0);
                yGrid.Children.Add(yText);
            }
        }


        public void AddPoint1(CollectPoint collectPoint)
        {
            x = x + userControl1_Para.xGrap;
            collectPoint.point.X = x;

            //移动背景不移动点
            double xMove = collectPoint.point.X  * (-1) + userControl1_Para.xWidth;
            _myPolyline.Points.Add(collectPoint.point);//添加新的数据点
           _myCanvas.Margin = new Thickness(xMove, 0, 0, 0);//设置外边距实现移动，放大缩小

            //移动X轴标签背景
            TextBlock xText = new TextBlock();
            xText.Visibility = Visibility.Hidden;
            xText.SetValue(Grid.RowProperty, 1); //设置按钮所在Grid控件的行
            xGrid.Children.Add(xText);//为每个点添加对应的时间值
            xText.Text = collectPoint.curTime.ToString("HH:mm:ss");
            xText.Margin = new Thickness(collectPoint.point.X, 0, 0, 0);
            collectPoints.Add((new CollectPoint() { point = collectPoint.point, curTime = collectPoint.curTime }));
            xLableCanvas.Margin = new Thickness(xMove, 0, 0, 0);//设置外边距实现移动，放大缩小

            if (xMove % userControl1_Para.xGrapLable == 0)
                xText.Visibility = Visibility.Visible;

            if (collectPoint.point.X > userControl1_Para.xWidth)
            {
                _myPolyline.Points.RemoveAt(0);
                collectPoints.RemoveAt(0);
                xGrid.Children.RemoveAt(0);
            }
        }
    }
}
