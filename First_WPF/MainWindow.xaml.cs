using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace First_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        Point[] points = new Point[52];
        const string FILEGRAPHNAME = "metroGraph.txt";
        const string FILESTRINGSNAME = "metroNames.txt";
        const byte STATIONNUMBER = 52;
        const int INF = 100;
        const int TIMESPEED = 5;
        string[] stationNames;
        int[,] stationConnections;





        public MainWindow()
        {
            InitializeComponent();
            Read();
            EnterStartData();
        }



        

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if ((cbxStart.SelectedIndex != -1) && (cbxEnd.SelectedIndex != -1)&&(cbxStart.SelectedIndex!=cbxEnd.SelectedIndex))
            {
                PathGeometry animPath = new PathGeometry();
                PathFigure pf = new PathFigure();
                PolyLineSegment pls = new PolyLineSegment();
                Start(pls);
                pf.Segments.Add(pls);
                pf.StartPoint = pls.Points[0];
                pf.IsClosed = false;
                animPath.Figures.Add(pf);
                DoubleAnimationUsingPath daup = new DoubleAnimationUsingPath();
                daup.PathGeometry = animPath;
                daup.Duration = TimeSpan.FromSeconds(TIMESPEED);
                daup.Source = PathAnimationSource.X;
                DoubleAnimationUsingPath daup1 = new DoubleAnimationUsingPath();
                daup1.PathGeometry = animPath;
                daup1.Duration = TimeSpan.FromSeconds(TIMESPEED);
                daup1.Source = PathAnimationSource.Y;
                imgHuman.Visibility = Visibility.Visible;
                imgHuman.BeginAnimation(Canvas.TopProperty, daup1);
                imgHuman.BeginAnimation(Canvas.LeftProperty, daup);
            }
            else
                MessageBox.Show("Выберите стартовую и конечную станцию");
        }


        /// <summary>
        /// Поиск коротчайшего пути
        /// </summary>
        public static List<int> Shortest(int[,] array, int start, int end)
        {
            int[] d = new int[STATIONNUMBER];
            for (int i = 0; i < STATIONNUMBER; i++)
                d[i] = INF;
            d[start] = 0;

            List<int>[] paths = new List<int>[52];
            for (int i = 0; i < STATIONNUMBER; i++)
                paths[i] = new List<int>();

            bool[] u = new bool[52];

            for (int i = 0; i < STATIONNUMBER; i++)
                u[i] = false;
            u[start] = true;


            for (int i = 0; i < STATIONNUMBER; i++)
                for (int j = 0; j < STATIONNUMBER; j++)
                    if (u[j])
                    {

                        int currMin = 0;
                        int k;
                        for (k = 0; k < STATIONNUMBER; k++)
                            if ((array[j, k] != 0) && (!u[k]))
                            {
                                currMin = k;
                                break;
                            }

                        if ((k == STATIONNUMBER) || (array[j, k] == 0) || (u[k]))
                            continue;


                        for (int e = 1; e < STATIONNUMBER; e++)
                            if ((!u[e]) && (array[j, e] != 0) && (array[j, currMin] != 0) && (array[j, e] < array[j, currMin]))
                                currMin = e;


                        if ((!u[currMin]) && (d[j] + array[j, currMin] < d[currMin]))
                        {
                            d[currMin] = d[j] + array[j, currMin];
                            u[currMin] = true;
                            paths[currMin] = paths[j].ToList<int>();
                            paths[currMin].Add(j);
                        }
                    }
            paths[end].Add(end);
            return paths[end];
        }

        /// <summary>
        /// Чтение данных из файлов
        /// </summary>
        private void Read()
        {
            stationNames = new string[STATIONNUMBER];
            stationConnections = new int[STATIONNUMBER, STATIONNUMBER];
            StreamReader sr = new StreamReader(new FileStream(FILEGRAPHNAME, FileMode.Open, FileAccess.Read));
            for (int i = 0; i < STATIONNUMBER; i++)
            {
                for (int j = 0; j < STATIONNUMBER; j++)
                    stationConnections[i, j] = sr.Read() - 48;
                sr.Read();
                sr.Read();
            }
            sr.Close();
            sr = new StreamReader(new FileStream(FILESTRINGSNAME, FileMode.Open, FileAccess.Read), Encoding.Default);
            stationNames = sr.ReadLine().Split(',');
            sr.Close();
        }

        /// <summary>
        /// Определение стартовых позиций и поиск путей
        /// </summary>
        public void Start(PolyLineSegment pls)
        {
                string from = cbxStart.SelectedItem.ToString();
                string dest = cbxEnd.SelectedItem.ToString();
                int fromnum;
                int destnum;
                fromnum = stationNames.ToList<string>().IndexOf(from);
                destnum = stationNames.ToList<string>().IndexOf(dest);

                List<int> d;
                d = Shortest(stationConnections, fromnum, destnum);
                foreach (int num in d)
                    pls.Points.Add(points[num]);
        }

        /// <summary>
        /// Внесение стартовых данных для работы
        /// </summary>
        public void EnterStartData()
        {
            cbxStart.ItemsSource = stationNames;
            cbxEnd.ItemsSource = stationNames;

            //Красная ветка
            points[0] = new Point(10, 112);
            points[1] = new Point(10, 156);
            points[2] = new Point(54, 210);
            points[3] = new Point(102, 225);
            points[4] = new Point(146, 246);
            points[5] = new Point(191, 270);
            points[6] = new Point(236, 292);
            points[7] = new Point(280, 314);
            points[8] = new Point(325, 316);
            points[9] = new Point(394, 314);
            points[10] = new Point(506, 292);
            points[11] = new Point(572, 314);
            points[12] = new Point(618, 315);
            points[13] = new Point(664, 294);
            points[14] = new Point(711, 294);
            points[15] = new Point(754, 272);
            points[16] = new Point(798, 226);
            points[17] = new Point(842, 184);
            //Зеленая
            points[18] = new Point(258, 157);
            points[19] = new Point(304, 199);
            points[20] = new Point(348, 246);
            points[21] = new Point(394, 292);
            points[22] = new Point(460, 361);
            points[23] = new Point(484, 403);
            points[24] = new Point(482, 448);
            points[25] = new Point(506, 498);
            points[26] = new Point(527, 540);
            points[27] = new Point(596, 540);
            points[28] = new Point(641, 540);
            points[29] = new Point(686, 540);
            points[30] = new Point(730, 540);
            points[31] = new Point(776, 498);
            points[32] = new Point(798, 450);
            points[33] = new Point(820, 405);
            //Синяя
            points[34] = new Point(370, 24);
            points[35] = new Point(415, 44);
            points[36] = new Point(460, 65);
            points[37] = new Point(460, 112);
            points[38] = new Point(460, 156);
            points[39] = new Point(460, 201);
            points[40] = new Point(460, 248);
            points[41] = new Point(482, 294);
            points[42] = new Point(438, 362);
            points[43] = new Point(414, 406);
            points[44] = new Point(394, 448);
            points[45] = new Point(372, 493);
            points[46] = new Point(348, 540);
            points[47] = new Point(303, 540);
            points[48] = new Point(258, 540);
            points[49] = new Point(213, 540);
            points[50] = new Point(168, 540);
            points[51] = new Point(122, 540);
        }
    }
}
