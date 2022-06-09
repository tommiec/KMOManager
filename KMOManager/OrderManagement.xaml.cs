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
using Microsoft.EntityFrameworkCore;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Drawing;
using System.IO;
using System.Data;
using System.ComponentModel;

namespace KMOManager
{
    /// <summary>
    /// Interaction logic for OrderManagement.xaml
    /// </summary>
    public partial class OrderManagement : Page
    {
        public OrderManagement()
        {
            InitializeComponent();
            UpdateDataGridsProducts();
        }

        private void UpdateDataGridsProducts()
        {
            DataShoppingCart.ItemsSource = Globals.ShoppingCart.ToList();

            using (Context ctx = new Context())
            {
                var queryAllOrders = ctx.Orders
                    .Where(x => x.CustomerId == Globals.selectedCustomer.CustomerId)
                    .Include(o => o.Customer)
                    .Include(o => o.Employee);

                /*DataOrders.ItemsSource = queryAllOrders
                    .Select(o => new { OrderId = o.OrderId, OrderDate = o.OrderDate }).ToList();*/

                DataOrders.ItemsSource = queryAllOrders.ToList();
            }
        }

        private void btnEmptyProduct_Click(object sender, RoutedEventArgs e)
        {
            Globals.ShoppingCart.Clear();
            UpdateDataGridsProducts();
        }

        private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            Globals.ShoppingCart.RemoveAt(DataShoppingCart.SelectedIndex);
            UpdateDataGridsProducts();
        }

        private Order CreateOrder()
        {
            Order newOrder = new Order
            {
                CustomerId = Globals.selectedCustomer.CustomerId,
                EmployeeId = Globals.currentUser.EmployeeId,
                OrderDate = DateTime.Now
            };

            using (Context ctx = new Context())
            {
                ctx.Orders.Add(newOrder);
                ctx.SaveChanges();
            }
            return newOrder;
        }

        private void AddOrderDetails(List<Product> ShoppingCart)
        {
            Order newOrder = CreateOrder();

            using (Context ctx = new Context())
            {
                foreach (Product product in Globals.ShoppingCart)
                {
                    ctx.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = product.ProductId,
                        Quantity = 1
                    });
                    UpdateStock(product);
                }
                ctx.SaveChanges();
            }
            CreateInvoice((Order)newOrder);
            Globals.ShoppingCart.Clear();
            UpdateDataGridsProducts();
        }


        private void UpdateStock(Product soldProduct)
        {
            using (Context ctx = new Context())
            {
                soldProduct.Stock -= 1;
                ctx.Products.Update(soldProduct);
                ctx.SaveChanges();
            }
        }


        private static void CreateInvoice(Order order)
        {
            double total = 0;
            double vat = 0;
            double totalVAT = 0;

            using (Context ctx = new Context())
            {
                var queryOrderWithProducts = ctx.OrderDetails
                    .Where(x => x.OrderId == order.OrderId)
                    .Include(od => od.Product);

                foreach (var product in queryOrderWithProducts)
                {
                    total += product.Product.Price;
                }
            }
            vat = total * 0.21;
            totalVAT = total + vat;

            using (Context ctx = new Context())
            {
                ctx.Invoices.Add(new Invoice()
                {
                    OrderId = order.OrderId,
                    Date = DateTime.Now,
                    Total = total,
                    VAT = vat,
                    TotalVAT = total + vat
                });

                ctx.SaveChanges();
            }
        }


        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            AddOrderDetails(Globals.ShoppingCart);
            UpdateDataGridsProducts();
        }

        private void DataOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Order selectedOrder = (Order)DataOrders.SelectedItem;
            using (Context ctx = new Context())
            {
                var queryOrderWithProducts = ctx.OrderDetails
                    .Where(x => x.OrderId == selectedOrder.OrderId)
                    .Include(od => od.Product);

                DataOrderDetails.ItemsSource = queryOrderWithProducts
                    .Select(od => new { ProductId = od.ProductId, ProductName = od.Product, Quantity = od.Quantity }).ToList();
            }

        }


        private void btnInvoice_Click(object sender, RoutedEventArgs e)
        {
            string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);

            Order selectedOrder = (Order)DataOrders.SelectedItem;
            if (selectedOrder == null)
            {
                MessageBox.Show("Selecteer een order aub.");
                return;
            }
            else
            {
                try
                {
                    //Creates a new PDF document
                    PdfDocument document = new PdfDocument();

                    //Adds page settings
                    document.PageSettings.Orientation = PdfPageOrientation.Portrait;
                    document.PageSettings.Margins.All = 50;

                    //Adds a page to the document
                    PdfPage page = document.Pages.Add();
                    PdfGraphics graphics = page.Graphics;

                    //Loads the image from disk
                    PdfImage image = PdfImage.FromFile("warehouse.png");
                    RectangleF bounds = new RectangleF(175, 0, 150, 150);

                    //Draws the image to the PDF page
                    page.Graphics.DrawImage(image, bounds);


                    PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
                    bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);

                    //Draws a rectangle to place the heading in that region.
                    graphics.DrawRectangle(solidBrush, bounds);
                    //Creates a font for adding the heading in the page
                    PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
                    //Creates a text element to add the invoice number
                    PdfTextElement element = new PdfTextElement("Invoice " + selectedOrder.OrderId.ToString(), subHeadingFont);
                    element.Brush = PdfBrushes.White;
                    //Draws the heading on the page
                    PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));
                    string currentDate = "Date: " + selectedOrder.OrderDate.ToString();
                    //Measures the width of the text to place it in the correct location
                    SizeF textSize = subHeadingFont.MeasureString(currentDate);
                    PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
                    //Draws the date by using DrawString method
                    graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
                    PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);

                    //Creates text elements to add the address and draw it to the page.
                    string customer = Convert.ToString($"{selectedOrder.Customer.FirstName} {selectedOrder.Customer.LastName}, {selectedOrder.Customer.Address}, {selectedOrder.Customer.PostalCode} {selectedOrder.Customer.City} ({selectedOrder.Customer.Country})");
                    element = new PdfTextElement("Facturatie: " + customer, timesRoman);
                    element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
                    result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
                    PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
                    PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
                    PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
                    //Draws a line at the bottom of the address
                    graphics.DrawLine(linePen, startPoint, endPoint);


                    //Create a PdfGrid.
                    PdfGrid pdfGrid = new PdfGrid();
                    //Create a DataTable.
                    DataTable dataTable = new DataTable();
                    //Add columns to the DataTable
                    dataTable.Columns.Add("ID");
                    dataTable.Columns.Add("Name");
                    dataTable.Columns.Add("Quantity");
                    dataTable.Columns.Add("Price");
                    using (Context ctx = new Context())
                    {
                        var queryOrderWithProducts = ctx.OrderDetails
                            .Where(x => x.OrderId == selectedOrder.OrderId)
                            .Include(od => od.Product);
                        var selection = queryOrderWithProducts
                            .Select(od => new { ProductId = od.ProductId, ProductName = od.Product, Quantity = od.Quantity, Price = od.Product.Price }).ToList();
                        //Add rows to the DataTable.
                        foreach (var product in selection)
                        {
                            dataTable.Rows.Add(new object[] { product.ProductId.ToString(), product.ProductName.ToString(), product.Quantity.ToString() + " stuk(s)", "€ " + product.Price.ToString() });
                        }
                    }
                    //Assign data source.
                    pdfGrid.DataSource = dataTable;
                    //Draw grid to the page of PDF document.
                    pdfGrid.Draw(page, new PointF(0, 400));

                    //Get invoice for selected order and print totals 
                    using (Context ctx = new Context())
                    {
                        var queryInvoiceForOrder = ctx.Invoices
                            .Where(x => x.OrderId == selectedOrder.OrderId).Single();

                        string total = queryInvoiceForOrder.Total.ToString();
                        element = new PdfTextElement("Total: " + total, timesRoman);
                        element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
                        result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 300));

                        string VAT = queryInvoiceForOrder.VAT.ToString();
                        element = new PdfTextElement("BTW: " + VAT, timesRoman);
                        element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
                        result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 10));

                        string totalVAT = queryInvoiceForOrder.TotalVAT.ToString();
                        element = new PdfTextElement("Total + BTW: " + totalVAT, timesRoman);
                        element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
                        result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 10));
                    }


                    //Save the document  
                    document.Save(strPath + "\\" + "Output.pdf");
                }
                catch (Exception)
                {
                    MessageBox.Show("Oops, dat ging fout. Contacteer Thomas!");
                }
            }
        }
    }
}