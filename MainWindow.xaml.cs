using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;

namespace АвтоЛорд
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = Properties.Settings.Default.connection;
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();

            string queryTemp = "SELECT COUNT(*) FROM Product";
            SqlCommand commandTemp = new SqlCommand(queryTemp);
            commandTemp.Connection = sql;

            int count = int.Parse(commandTemp.ExecuteScalar().ToString());
            int rows = count / 3 + 1;

            for(int i = 0; i < rows; i++)
            {
                main.RowDefinitions.Add(new RowDefinition());
            }


            Button btn = new Button();
            btn.Content = "Добавить товар";
            btn.Click += Btn_Click;
            main.Children.Add(btn);
            Grid.SetColumn(btn, 0);
            Grid.SetRow(btn, 0);

            Button btn2 = new Button();
            btn2.Content = "Обновить";
            btn2.Click += Btn_Click2;
            main.Children.Add(btn2);
            Grid.SetColumn(btn2, 1);
            Grid.SetRow(btn2, 0);

            UpdateData();

        }
        private void UpdateData()
        {
            SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();

            int c = 0, r = 1;
            string query = "SELECT * FROM Product ORDER BY ID ASC";
            SqlCommand command = new SqlCommand(query);
            command.Connection = sql;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                StackPanel panel = new StackPanel();

                panel.Margin = new Thickness(5);

                string name = reader["Title"].ToString();
                string cost = reader["Cost"].ToString();
                int pos = cost.IndexOf(",");
                cost = cost.Substring(0, pos + 3);

                string photo = Environment.CurrentDirectory + "\\" + reader["MainImagePath"].ToString();

                photo = photo.Replace(" Товары автосервиса", "photos");
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(photo));
                image.Height = 150;
                int id = int.Parse(reader["ID"].ToString());
                image.MouseUp += (o, i) =>
                {
                    OpenProduct open = new OpenProduct(id);
                    this.Hide();
                    open.ShowDialog();
                    this.Show();
                };
                panel.Children.Add(image);

                Label nameLabel = new Label();
                nameLabel.Content = name;
                nameLabel.HorizontalAlignment = HorizontalAlignment.Center;
                panel.Children.Add(nameLabel);

                Label costLabel = new Label();
                costLabel.Content = cost;
                costLabel.HorizontalAlignment = HorizontalAlignment.Center;
                panel.Children.Add(costLabel);

                bool active = bool.Parse(reader["IsActive"].ToString());
                if (active) panel.Background = new SolidColorBrush(Color.FromRgb(51, 181, 51));
                else panel.Background = new SolidColorBrush(Color.FromRgb(230, 11, 11));

                main.Children.Add(panel);
                Grid.SetColumn(panel, c);
                Grid.SetRow(panel, r);

                c++;
                if (c >= 3)
                {
                    r++;
                    c = 0;
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            NewProduct np = new NewProduct();
            this.Hide();
            np.ShowDialog();
            this.Show();
        }
        private void Btn_Click2(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }
    }
}
