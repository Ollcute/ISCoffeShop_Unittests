using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
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
using Button = System.Windows.Controls.Button;
using Window = System.Windows.Window;
using Word = Microsoft.Office.Interop.Word;

namespace Cafeterei.View
{
    /// <summary>
    /// Логика взаимодействия для BuckedWindow.xaml
    /// </summary>
    public partial class BuckedWindow : Window
    {
        CreateOrder createOrder;
        private double finalSumCard;
        
        public BuckedWindow()
        {
            InitializeComponent();
          
           
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            tB_sum_Order.Text += (this.Owner as CreateOrder).SummaOrder.ToString();
            finalSumCard = (this.Owner as CreateOrder).SummaBankCard - (this.Owner as CreateOrder).SummaOrder;
            tB_sum_Card.Text += finalSumCard.ToString();
            

        }
        private void but_Exit_create_order_Click(object sender, RoutedEventArgs e)
        {

            //if (MessageBox.Show("Вы уверены, что хотите закрыть корзину?",
            //        "Save file",
            //        MessageBoxButton.YesNo,
            //        MessageBoxImage.Question) == MessageBoxResult.Yes) {
            //    this.Hide();
            //    foreach (Window window in App.Current.Windows)
            //    {
            //        if (!(window is MainWindow))
            //            window.Close();
            //    }
            //}


            //else
            //{
            //    this.Show();
            //}

            MessageBox.Show("Close!!!");
        }

        private void but_Create_order_Click(object sender, RoutedEventArgs e)
        {
            View.CreateOrder createOrderWindow = new CreateOrder(finalSumCard);
            this.Hide();
            createOrderWindow.ShowDialog();
            MessageBox.Show("Переход успешно выполнен!");
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            createOrder = this.Owner as CreateOrder;

            dgOrder.ItemsSource = createOrder.listProductsInOrders;
        }

      private void but_Correct_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Classes.ProductsinOrder productsinOrder = button.DataContext as Classes.ProductsinOrder;
            switch (button.Content)
            {
                case "+":
                    if(productsinOrder.Cost + createOrder.SummaOrder <= createOrder.SummaBankCard)
                    {
                        productsinOrder.Count++;
                        productsinOrder.Costing += productsinOrder.Cost;
                        createOrder.SummaOrder += productsinOrder.Cost;
                        finalSumCard -= productsinOrder.Cost;
                        tB_sum_Card.Text =  "Account balance: "+ finalSumCard.ToString(); 
                        tB_sum_Order.Text = "Total: " + createOrder.SummaOrder.ToString();
                    }
                    break;
                case "-":
                    if (productsinOrder.Count>1)
                    {
                        productsinOrder.Count--;
                        productsinOrder.Costing -= productsinOrder.Cost;
                        createOrder.SummaOrder -= productsinOrder.Cost;
                        finalSumCard += productsinOrder.Cost;
                        tB_sum_Card.Text = "Account balance: " + finalSumCard.ToString();
                        tB_sum_Order.Text = "Total: " + createOrder.SummaOrder.ToString();

                    }
                    else
                    {
                        button.Content = "x";
                        but_Correct_Click(button, null);
                      

                    }
                    break;
                case "Delete":
                    createOrder.listProductsInOrders.Remove(productsinOrder);
                    createOrder.SummaOrder -= productsinOrder.Costing;
                    finalSumCard += productsinOrder.Costing;
                    tB_sum_Card.Text = "Account balance: " + finalSumCard.ToString();
                    tB_sum_Order.Text = "Total: " + createOrder.SummaOrder.ToString();
                    break;

            }
            dgOrder.Items.Refresh();
           
            

        }

        private void button_Zakaz_Click(object sender, RoutedEventArgs e)
        {
            Word.Application wordApp;
            Word.Document wordDoc;
            Word.Paragraph wordPar;
            Word.Range wordRange;
            Word.Table wordTable;
            Word.InlineShape wordShape;
            try
            {
                wordApp = new Word.Application();
                wordApp.Visible = false;
            }
            catch
            {
                MessageBox.Show("Товарный чек в Word создать не удалось");
                return;

            }
            wordDoc = wordApp.Documents.Add();
            wordDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientPortrait;

            wordPar = wordDoc.Paragraphs.Add();
            wordPar.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            wordRange = wordPar.Range;
            wordPar.set_Style("Заголовок 1");
            wordRange.Text = "Дата заказа: " +DateTime.Now.ToLongDateString();
            wordShape = wordDoc.InlineShapes.AddPicture(App.pathExe + @"\заставка.png",Type.Missing);
            wordShape.Width = 100;
            wordShape.Height= 100;


            wordRange.InsertParagraphAfter();
            wordPar = wordDoc.Paragraphs.Add();
            wordPar.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            wordRange = wordPar.Range;
            wordRange.Font.Size = 16;
            wordRange.Font.Color = Word.WdColor.wdColorBrown;
            wordRange.Font.Name = "Time New Roman";
            wordRange.Text = "Cписок заказанных блюд";

            wordRange = wordPar.Range;

            wordTable = wordDoc.Tables.Add(wordRange, this.createOrder.listProductsInOrders.Count + 1, 4);
            wordTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            wordTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleDouble;
            Word.Range cellRange;
            for(int col=1; col <= 4; col++)
            {
                cellRange = wordTable.Cell(1,col).Range;
                cellRange.Text = dgOrder.Columns[col - 1].Header.ToString();
            }

            wordTable.Rows[1].Shading.ForegroundPatternColor = Word.WdColor.wdColorLightOrange;
            wordTable.Rows[1].Shading.BackgroundPatternColorIndex = Word.WdColorIndex.wdBlue;
            wordTable.Rows[1].Range.Bold = 1;
            wordTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            wordRange.Font.Size = 14;
            wordRange.Font.Color = Word.WdColor.wdColorBrown;
            wordRange.Font.Name = "Time New Roman";

            wordPar.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            for(int row=2; row <= createOrder.listProductsInOrders.Count + 1; row++)
            {
                cellRange = wordTable.Cell(row, 1).Range;
                cellRange.Text = createOrder.listProductsInOrders[row - 2].Name;
                wordRange.Font.Size = 14;
                wordRange.Font.Color = Word.WdColor.wdColorBlack;
                wordRange.Font.Name  = "Time New Roman";

                cellRange = wordTable.Cell(row, 2).Range;
                cellRange.Text = createOrder.listProductsInOrders[row - 2].Cost.ToString();
                cellRange = wordTable.Cell(row, 3).Range;
                cellRange.Text = createOrder.listProductsInOrders[row - 2].Count.ToString();
                cellRange = wordTable.Cell(row, 4).Range;
                cellRange.Text = createOrder.listProductsInOrders[row - 2].Costing.ToString();

            }

            wordRange.InsertParagraphAfter();
            wordPar = wordDoc.Paragraphs.Add();
            wordRange = wordPar.Range;
            wordPar.set_Style("Заголовок 1");
            wordRange.Font.Color = Word.WdColor.wdColorDarkRed;
            wordRange.Font.Size = 20;
            wordRange.Bold = 3;
            wordRange.Text = "Стоимость заказа: " + createOrder.SummaOrder.ToString() + " рублей";
            wordApp.Visible = true;


            string filename = App.pathExe + @"\Чек";
            wordDoc.SaveAs2(filename + ".docx");
            wordDoc.SaveAs2(filename + ".pdf", Word.WdExportFormat.wdExportFormatPDF);

            wordDoc.Close(true, null, null);
            wordApp.Quit();

            releaseObject(wordPar);
            releaseObject(wordDoc);
            releaseObject(wordApp);



        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                GC.Collect();
            }
        }
    }
}