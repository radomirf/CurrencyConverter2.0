using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json;
namespace CurrencyConverter2._0
{
 
    public partial class MainWindow : Window
    {
        Root val = new Root();

        public class Root
        {
            public Rate rates { get; set; }
            public long timestamp;
            public string license;
        }
        public class Rate
        {
            public double BGN { get; set; }
            public double BTC { get; set; }
            public double CAD { get; set; }
            public double USD { get; set; }
            public double CHF { get; set; }
            public double CNY { get; set; }
            public double EUR   { get; set; }
            public double JPY { get; set; }
            public double TRY { get; set; }
            public double RUB { get; set; } 
        }
        
        public MainWindow()
        {
            InitializeComponent();
            ClearAll();
            GetValue();
        }

        public static async Task<Root> GetData<T>(string url)
        {
            var myRoot = new Root();
            try
            {
                using(var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage response = await client.GetAsync(url);
                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ResponceString = await response.Content.ReadAsStringAsync();
                        var ResponceObject = JsonConvert.DeserializeObject<Root>(ResponceString);

                       
                        return ResponceObject;
                    }
                    return myRoot;
                }
            }
            catch
            {
                return myRoot;
            }
        }
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            double ConvertedValue;
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCurrency.Focus();
                return;

            }
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                ConvertedValue = double.Parse(txtCurrency.Text);
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString())) * double.Parse(txtCurrency.Text) / double.Parse(cmbFromCurrency.SelectedValue.ToString());
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }



        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }


        private void ClearAll()
        {
            txtCurrency.Text = String.Empty;
            if (cmbFromCurrency.Items.Count > 0)
            {
                cmbFromCurrency.SelectedIndex = 0;
            }
            if (cmbToCurrency.Items.Count > 0)
            {
                cmbToCurrency.SelectedIndex = 0;
            }
            lblCurrency.Content = "";
            txtCurrency.Focus();
        }


        private void NumberValidationTextBox(Object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void GetValue()
        {
            val = await GetData<Root>("https://openexchangerates.org/api/latest.json?app_id=7a7beea78d114ddd9c304ba7fd34035f");
            BindCurrency();
        }

        private void BindCurrency()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Value");
            dt.Rows.Add("Select",0);
            dt.Rows.Add("BGN", val.rates.BGN);
            dt.Rows.Add("USD", val.rates.USD);
            dt.Rows.Add("BTC", val.rates.BTC);
            dt.Rows.Add("CAD", val.rates.CAD);
            dt.Rows.Add("CHF", val.rates.CHF);
            dt.Rows.Add("CNY", val.rates.CNY);
            dt.Rows.Add("EUR", val.rates.EUR); 
            dt.Rows.Add("JPY", val.rates.JPY);
            dt.Rows.Add("TRY", val.rates.TRY);
            dt.Rows.Add("RUB", val.rates.RUB);

            cmbFromCurrency.ItemsSource = dt.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Text";
            cmbFromCurrency.SelectedValuePath = "Value";
            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.ItemsSource = dt.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Value";
            cmbToCurrency.SelectedIndex = 0;
        }


    }

}
    


     
   

      

