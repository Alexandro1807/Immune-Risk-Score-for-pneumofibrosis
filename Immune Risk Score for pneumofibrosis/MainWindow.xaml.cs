using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Immune_Risk_Score_for_pneumofibrosis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeFormToEndState();
        }

        private void InitializeFormToEndState()
        {
            if (TextBoxResult.Text == "ОШИБКА!") TextBox1.Text = TextBox2.Text = TextBox3.Text = TextBox4.Text = TextBoxResult.Text = string.Empty;
            Ellipse1.Fill = Ellipse2.Fill = Ellipse3.Fill = Ellipse4.Fill = EllipseResult.Fill = null;
            TextBox1.IsEnabled = TextBox2.IsEnabled = TextBox3.IsEnabled = TextBox4.IsEnabled = ButtonStart.IsEnabled = true;
            ButtonRestart.Visibility = LabelResult.Visibility = TextBoxResult.Visibility = EllipseResult.Visibility = Visibility.Hidden;
        }

        private void InitializeFormToResultState(string riskState)
        {
            TextBoxResult.Text = riskState;
            switch (riskState)
            {
                case "НИЗКИЙ":
                    {
                        EllipseResult.Fill = Brushes.Green;
                        break;
                    }
                case "СРЕДНИЙ":
                    {
                        EllipseResult.Fill = Brushes.Yellow;
                        break;
                    }
                case "ВЫСОКИЙ":
                    {
                        EllipseResult.Fill = Brushes.Red;
                        break;
                    }
                case "ОШИБКА!":
                    {
                        TextBox1.Text = TextBox2.Text = TextBox3.Text = TextBox4.Text = TextBoxResult.Text = riskState;
                        Ellipse1.Fill = Ellipse2.Fill = Ellipse3.Fill = Ellipse4.Fill = EllipseResult.Fill = Brushes.Black;
                        break;
                    }
            }
            TextBox1.IsEnabled = TextBox2.IsEnabled = TextBox3.IsEnabled = TextBox4.IsEnabled = ButtonStart.IsEnabled = false;
            ButtonRestart.Visibility = LabelResult.Visibility = TextBoxResult.Visibility = EllipseResult.Visibility = Visibility.Visible;
        }

        private string CalculateRiskState()
        {
            string riskState = string.Empty;
            if (!double.TryParse(TextBox1.Text, out double index1)) return "ОШИБКА!";
            if (!double.TryParse(TextBox2.Text, out double index2)) return "ОШИБКА!";
            if (!double.TryParse(TextBox3.Text, out double index3)) return "ОШИБКА!";
            if (!double.TryParse(TextBox4.Text, out double index4)) return "ОШИБКА!";
            int counter = 0;
            if (index1 <= 5.2) { Ellipse1.Fill = Brushes.Red; counter++; }
            if (index2 >= 48.7) { Ellipse2.Fill = Brushes.Red; counter++; }
            if (index3 >= 15.85) { Ellipse3.Fill = Brushes.Red; counter++; }
            if (index4 <= 0.89) { Ellipse4.Fill = Brushes.Red; counter++; }
            switch (counter)
            {
                case 0: case 1: case 2:
                    {
                        riskState = "НИЗКИЙ";
                        break;
                    }
                case 3:
                    {
                        riskState = "СРЕДНИЙ";
                        break;
                    }
                case 4:
                    {
                        riskState = "ВЫСОКИЙ";
                        break;
                    }
            }
            return riskState;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            Ellipse1.Fill = Ellipse2.Fill = Ellipse3.Fill = Ellipse4.Fill = Brushes.Green;
            string riskState = CalculateRiskState();
            InitializeFormToResultState(riskState);
        }

        private void ButtonRestart_Click(object sender, RoutedEventArgs e)
        {
            InitializeFormToEndState();
        }

        private static readonly Regex regexNumbers = new Regex("[0-9]*\\.?[0-9]+");

        public static void NumberPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            e.Handled = !regexNumbers.IsMatch(e.Text);
            if (!e.Handled)
            {
                if (double.Parse(textBox.Text + e.Text) > 100.0)
                {
                    e.Handled = true;
                    textBox.Text = "100";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
            else if (e.Text == ",")
            {
                if (textBox.Text.Length > 0 && textBox.Text != "100" && !textBox.Text.EndsWith(",")) e.Handled = false;
            }
        }

        private void TextBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberPreviewTextInput(sender, e);
        }

        private void TextBox2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberPreviewTextInput(sender, e);
        }

        private void TextBox3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberPreviewTextInput(sender, e);
        }

        private void TextBox4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberPreviewTextInput(sender, e);
        }
    }
}