using Cafeterei.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace Cafeterei.View
{
    /// <summary>
    /// Логика взаимодействия для CreateOrder.xaml
    /// </summary>
    public partial class CreateOrder : Window
    {
        
        public string List;
       
        List<Classes.Product> listProducts;
        List<Classes.Product> listBasketProducts = new List<Product>();
      
        Product currentItem;
        Random rand = new Random();
        public double SummaBankCard { get; set; }   //Сумма на карте
        public double SummaOrder { get; set; } 		//Сумма заказа

        public List<Classes.ProductsinOrder> listProductsInOrders; 
        //Для диаграммы
        ChartArea area;					    //Площадь диаграммы
        Series series;						//Серия точек
        


        public CreateOrder(double sum)
        {
            InitializeComponent();
            
            area = new ChartArea("Default");
            chartSumma.ChartAreas.Add(area);
            series = new Series("Summa");
            chartSumma.Series.Add(series);
            chartSumma.Series["Summa"].ChartArea = "Default";
            chartSumma.Series["Summa"].ChartType = SeriesChartType.Pie;
            this.DataContext = this;	  //Элементы интерфейса связать с данными
            this.SummaBankCard = sum;     //Сумма на карте
            tB_sum_Bank.Text = "На карте: " + this.SummaBankCard;
            listProductsInOrders = new List<Classes.ProductsinOrder>();
            SummaOrder = 0;
            tB_sum_Order.Text = "Сумма заказа: " + SummaOrder;


        }


        private void but_Place_order_Click(object sender, RoutedEventArgs e)
        {
            //Random rnd = new Random();
            //SummaBankCard = rnd.Next(1, 9999);
            MessageBox.Show($"Сумма Вашего заказа составила");
            //View.BuckedWindow bucked = new BuckedWindow(); //создание объекта окна
            //bucked.Owner = this; //Указать владельца у дополнительного окна
            //bucked.ShowDialog(); //Показать модальное дополнительное
            //this.Hide();


        }

        private void but_Exit_Menu_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Вы уверены, что хотите закрыть каталог?",
                    "Save file",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.Hide();
            else
            {
                this.Show();
            }

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
           lst_Category.Items.Clear();
            for(int i = 1; i <= App.excelBook.Worksheets.Count; i++)
            {
                // lst_Category.Items.Add(App.excelBook.Worksheets[i].Name);
                App.CreateListCategory();

                lst_Category.Items.Add(App.ListCat);
            }
        }

        private void lst_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string categoryName = lst_Category.SelectedItem.ToString();
            App.excelSheet = App.excelBook.Sheets[categoryName];

            listProducts = new List<Classes.Product>();
            Classes.Product product;

            App.excelCells = App.excelSheet.UsedRange;

            //получить все заполненные ячейки листа в цикле
            for (int i = 1; i <= App.excelSheet.UsedRange.Rows.Count; i++)
            {
                product = new Classes.Product(); 
                product.Name = (string)App.excelCells.Cells[i, 1].Value2; 
                product.Cost = (int)App.excelCells.Cells[i, 2].Value2;
                product.Gramm = (int)App.excelCells.Cells[i, 3].Value2;
                product.BGY = (int)App.excelCells.Cells[i, 4].Value2;
                product.Photo = App.pathExe + $"\\{categoryName}\\{App.excelCells.Cells[i, 5].Value2}.png";
                listProducts.Add(product); 

            }

            
            ListBoxProducts.ItemsSource = listProducts; 
        }
       
        private void ListBoxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            Classes.ProductsinOrder productInOrder = null;
            //Объект из списка  в строке которой нажали кнопку
            Classes.Product product = (sender as Button).DataContext as Classes.Product;
            string productName = product.Name;	          	//Название блюда
            int productCost = product.Cost;			        //Стоимость блюда
            if (SummaOrder + productCost <= SummaBankCard)  //Проверка под сумму на карте
            {
                SummaOrder += productCost;			            //Общая сумма в заказе
                //SummaBankCard-=productCost;
                tB_sum_Order.Text = "Сумма заказа: " + SummaOrder;
               // tB_sum_Bank.Text= "Остаток на счете: " + SummaBankCard;
                //Поиск этого блюда среди заказанных блюд
                int index = listProductsInOrders.FindIndex(x => x.Name == productName);
                if (index < 0)                                 //Такого товара еще в заказе нет
                {
                    //Создаем новый элемент списка
                    productInOrder = new Classes.ProductsinOrder();
                    productInOrder.Name = productName;
                    productInOrder.Cost = productCost;
                    productInOrder.Count = 1;                   //Для нового
                    productInOrder.Costing = productCost;	    //Стоимость
                    listProductsInOrders.Add(productInOrder);	//добавляем в список
                }
                else        
                {
                    listProductsInOrders[index].Count++;
                    listProductsInOrders[index].Costing =
                                                listProductsInOrders[index].Cost * listProductsInOrders[index].Count;
                }
                ChartShow();					//Метод отображения диаграммы
            }
            else
            {
                MessageBox.Show("У Вас уже не хватает денег");
            }

        }
        public void ChartShow()
        {
            chartSumma.Series["Summa"].Points.Clear();
            //Сектор Оставшиеся деньги
            chartSumma.Series["Summa"].Points.AddXY(0, SummaBankCard - SummaOrder);
            //Сектор Сумма заказа
            chartSumma.Series["Summa"].Points.AddXY(0, SummaOrder);
        }

    }
}
