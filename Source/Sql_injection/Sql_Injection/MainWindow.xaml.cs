using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sql_Injection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string conn = @"Data Source=LAPTOP-73JV2AJ9;Initial Catalog= Login;Integrated Security=True";

            string userName = UserName.Text;
            string password = Password.Text;

            string query = "SELECT * FROM Users WHERE Username = '" + userName + "' AND Password = '" + password + "'";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        MessageBox.Show("Login successful!");
                    }
                    else
                    {
                        MessageBox.Show("Login failed!");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

    }
}