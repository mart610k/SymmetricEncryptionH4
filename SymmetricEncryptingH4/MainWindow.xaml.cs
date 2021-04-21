using SymmetricEncryptingH4.Service;
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

namespace SymmetricEncryptingH4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EncryptionService encryptionService;

        public MainWindow()
        {
            InitializeComponent();
            AlgorithmSelector.ItemsSource = Enum.GetValues(typeof(HashingAlgorithm)).Cast<HashingAlgorithm>();
        }

        private void Generate_Key_And_IV(object sender, RoutedEventArgs e)
        {
            HashingAlgorithm algorithm;

            try
            {
                algorithm = (HashingAlgorithm)AlgorithmSelector.SelectedItem;
                encryptionService = new EncryptionService(algorithm);
                IVTextField.Text = encryptionService.IVAsBase64;
                KeyTextField.Text = encryptionService.KeyAsBase64;
            }
            catch
            {
                MessageBox.Show("You must select a valid Encryption method");
            }

           
        }

        private void EncryptMessage_Click(object sender, RoutedEventArgs e)
        {
            byte[] encryptedMessage = encryptionService.Encrypt(Encoding.UTF8.GetBytes(PlainTextMessage.Text));

            
            CipherTextBase64.Text= Convert.ToBase64String(encryptedMessage);
        }

        private void DecryptMessage_Click(object sender, RoutedEventArgs e)
        {
            byte[] encryptedMessage = encryptionService.Decrypt(Convert.FromBase64String(CipherTextBase64.Text));

            PlainTextBase64.Text = Convert.ToBase64String(encryptedMessage);
            PlainTextMessage.Text = Encoding.UTF8.GetString(encryptedMessage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            encryptionService.Algorithm.Key = Convert.FromBase64String(KeyTextField.Text);
            encryptionService.Algorithm.IV = Convert.FromBase64String(IVTextField.Text);
        }
    }
}
