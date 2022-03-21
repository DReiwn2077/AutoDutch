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
using System.IO;

namespace АвтоЛорд
{
    /// <summary>
    /// Логика взаимодействия для NewProduct.xaml
    /// </summary>
    public partial class NewProduct : Window
    {
        int[] ids = new int[100];
        string connectionString = Properties.Settings.Default.connection;
        string photo = null;
        public NewProduct()
        {
            InitializeComponent();

            this.Loaded += NewProduct_Loaded;
        }

        private void NewProduct_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();

            int c = 0, r = 1;
            string query = "SELECT * FROM Manufacturer";
            SqlCommand command = new SqlCommand(query);
            command.Connection = sql;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int index = manufacter.Items.Add(reader["Name"].ToString());
                ids[index] = int.Parse(reader["ID"].ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int position = manufacter.SelectedIndex;
            if (position == -1) return;
            if (photo == null) return;

            int manufacturerId = ids[position];
            string name = productname.Text;
            string temp = productcost.Text;
            int cost = int.Parse(temp);
            bool active = (bool)activity.IsChecked;


            string query = "INSERT INTO Product (Title, Cost, Description, MainImagePath, IsActive, ManufacturerID) VALUES ";
            query += "('" + name + "', " + cost + ", '', '" + photo + "', '" + active + "', " + manufacturerId + ")";

            SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();
            SqlCommand command = new SqlCommand(query);
            command.Connection = sql;
            command.ExecuteNonQuery();
            this.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            bool result = (bool) dlg.ShowDialog();
            if (result == true)
            {
                
                string filename = dlg.FileName;
                string tempPhoto = CreatePassword(6) + ".png";
                string path = Environment.CurrentDirectory + "/photos/" + tempPhoto;
                File.Copy(dlg.FileName, path);
                photo = "photos\\" + tempPhoto;
                newproductimage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/photos/" + tempPhoto));
            }
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
