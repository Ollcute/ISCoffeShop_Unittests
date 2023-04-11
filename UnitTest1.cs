using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Controls;
using Cafeterei;
using System.Windows;
using Cafeterei.View;

namespace CafeteriTest
{
    [TestClass]
    public class UnitTest1
    {
        //Вход в окно авторизации с верными паролем и логином.
        [TestMethod]
        public void TestPositiveAuthorization()
        {
            //Arrange
            var auth = new AuthorizationWindow();
            var login = (TextBox)auth.FindName("login");
            var password = (PasswordBox)auth.FindName("password");
            var button = (Button)auth.FindName("Sign_in");

            //Act
            login.Text = "admin";
            password.Password = "admin";
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsTrue(App.Login == login.Text && App.Password == password.Password);
        }


        //Вход в окно авторизации с неверными паролем и логином.
        [TestMethod]
        public void TestNegativAuthorization()
        {
            //Arrange
            var auth = new AuthorizationWindow();
            var login = (TextBox)auth.FindName("login");
            var password = (PasswordBox)auth.FindName("password");
            var button = (Button)auth.FindName("Sign_in");

            //Act
            login.Text = "FalseAdmin";
            password.Password = "FalseAdmin";
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsFalse(App.Login == login.Text && App.Password == password.Password);
        }

        //Открытие окна PriceList по кнопке с главного окна
        [TestMethod]
        public void TestOpenPriceList()
        {
            //Arrange
            var main = new MainWindow();
            main.Show();
            var button = (Button)main.FindName("but_Price");

            //Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsTrue(button == main.FindName("but_Price"));
        }

        //Открытие окна создания заказа
        [TestMethod]
        public void TestOpenCreateOrderWindow()
        {
            //Arrange
            var main = new MainWindow();
            main.Show();
            var button = (Button)main.FindName("but_Order");

            //Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsTrue(button == main.FindName("but_Order"));
        }

        //Открытие окна работы с каталогом
        [TestMethod]
        public void TestOpenWorkCatalogWindow()
        {
            //Arrange
            var main = new MainWindow();
            main.Show();
            var button = (Button)main.FindName("but_Catalog");

            //Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsTrue(button == main.FindName("but_Catalog"));
        }

        //Открытие окна корзины
        [TestMethod]
        public void TestOpenBuckedWindow()
        {
            //Arrange
            double sum = 12000;
            var createorder = new CreateOrder(sum);
            //createorder.Show();
            var button = (Button)createorder.FindName("but_Place_order");

            //Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsTrue(button == createorder.FindName("but_Place_order"));
        }

        //Выход из приложения в главном меню
        [TestMethod]
        public void TestExitMainWindow()
        {
            //Arrange
            var main = new MainWindow();
            var button = (Button)main.FindName("but_Exit");

            //Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsTrue(button == main.FindName("but_Exit"));
        }

        //Проверка на правильность поступления счёта на карту 
        [TestMethod]
        public void TestDataTransmissionMainWindow()
        {
            //Arrange
            var main = new MainWindow();
            double card = main.value;
            var createorder = new CreateOrder(card);
            var button = (Button)main.FindName("but_Order");

            //Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsTrue(card == createorder.SummaBankCard);
        }

        //Выход из окна оформления заказа
        [TestMethod]
        public void TestExitCreateOrder()
        {
            //Arrange
            double sum = 12000;
            var createorder = new CreateOrder(sum);
            var button = (Button)createorder.FindName("but_Exit_Menu");

            //Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsTrue(button == createorder.FindName("but_Exit_Menu"));
        }

        //Оформление чека в корзине
        [TestMethod]
        public void TestCreateReceiptInBucked()
        {
            //Arrange
            var bucked = new BuckedWindow();
            var button = (Button)bucked.FindName("button_Zakaz");

            //Act
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            //Assert
            Assert.IsTrue(button == bucked.FindName("button_Zakaz"));
        }





    }
}
