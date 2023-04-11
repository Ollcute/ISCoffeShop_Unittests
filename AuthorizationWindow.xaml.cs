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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cafeterei.View
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        int countTry;
        public AuthorizationWindow()
        {
            InitializeComponent();
            countTry = 3;
        }

        private void but_Exit_Menu_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Вы уверены, что хотите закрыть форму авторизации?",
                    "Save file",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.Hide();

            else
            {
                this.Show();
            }
        }

        private void Sign_in_Click(object sender, RoutedEventArgs e)
        {
            string login_ = login.Text;
            string password_ = password.Password;


            if (login_ == App.Login && password_ == App.Password)
            {
                MessageBox.Show("Вы успешно зашли как администратор");
                //View.WorkingCatalogWindow work_catalog = new View.WorkingCatalogWindow(); 
                //this.Hide();
                //work_catalog.ShowDialog();
                //this.Close();
            }
            else
            {
                countTry--;
                if (countTry == 0)
                {
                    MessageBox.Show("Попытки входа закончились");
                    this.Close();

                }
                else
                    MessageBox.Show("У вас осталась "+ countTry + " попытки");
            }

           

        }
    }
}
