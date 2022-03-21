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
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace АвтоЛорд
{
    /// <summary>
    /// Логика взаимодействия для OpenProduct.xaml
    /// </summary>
    public partial class OpenProduct : Window
    {
        int id = 1;
        string connectionString = Properties.Settings.Default.connection;
        public OpenProduct(int id)
        {
            InitializeComponent();
            this.Loaded += OpenProduct_Loaded;
            this.id = id;
        }

        private void OpenProduct_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();

            int c = 0, r = 1;
            string query = "SELECT * FROM Product WHERE ID = " + id;
            SqlCommand command = new SqlCommand(query);
            command.Connection = sql;
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            productname.Text = reader["Title"].ToString();
            productcost.Text = reader["Cost"].ToString();
            this.Title = reader["Title"].ToString();

            activity.IsChecked = bool.Parse(reader["IsActive"].ToString());


            string photo = Environment.CurrentDirectory + "\\" + reader["MainImagePath"].ToString();

            photo = photo.Replace(" Товары автосервиса", "photos");
            newproductimage.Source = new BitmapImage(new Uri(photo));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
