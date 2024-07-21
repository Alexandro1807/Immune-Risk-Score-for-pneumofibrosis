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
        const string _riskStateLow = "НИЗКИЙ";
        const string _riskStateMiddle = "СРЕДНИЙ";
        const string _riskStateHigh = "ВЫСОКИЙ";
        const string _riskStateError = "ОШИБКА!";
        public MainWindow()
        {
            InitializeComponent();
            InitializeFormToEndState();
        }

        private void InitializeFormToEndState()
        {
            if (TextBoxResult.Text == _riskStateError) FillTextBoxes(string.Empty, true);
            FillEllipsis(null, true);
            TextBox1.IsEnabled = TextBox2.IsEnabled = TextBox3.IsEnabled = TextBox4.IsEnabled = TextBox5.IsEnabled = ButtonStart.IsEnabled = true;
            ButtonRestart.Visibility = LabelResult.Visibility = TextBoxResult.Visibility = EllipseResult.Visibility = Visibility.Hidden;
        }

        private void InitializeFormToResultState(string riskState)
        {
            TextBoxResult.Text = riskState;
            switch (riskState)
            {
                case _riskStateLow:
                    {
                        EllipseResult.Fill = Brushes.Green;
                        break;
                    }
                case _riskStateMiddle:
                    {
                        EllipseResult.Fill = Brushes.Yellow;
                        break;
                    }
                case _riskStateHigh:
                    {
                        EllipseResult.Fill = Brushes.Red;
                        break;
                    }
                case _riskStateError:
                    {
                        FillTextBoxes(riskState, true);
                        FillEllipsis(Brushes.Black, true);
                        break;
                    }
            }
            TextBox1.IsEnabled = TextBox2.IsEnabled = TextBox3.IsEnabled = TextBox4.IsEnabled = TextBox5.IsEnabled = ButtonStart.IsEnabled = false;
            ButtonRestart.Visibility = LabelResult.Visibility = TextBoxResult.Visibility = EllipseResult.Visibility = Visibility.Visible;
        }

        private void FillEllipsis(SolidColorBrush color, bool isWithResult = false)
        {
            Ellipse1.Fill = Ellipse2.Fill = Ellipse3.Fill = Ellipse4.Fill = Ellipse5.Fill = color;
            if (isWithResult) EllipseResult.Fill = color;
        }

        private void FillTextBoxes(string value, bool isWithResult = false)
        {
            TextBox1.Text = TextBox2.Text = TextBox3.Text = TextBox4.Text = TextBox5.Text = value;
            if (isWithResult) TextBoxResult.Text = value;
        }

        private string CalculateRiskState()
        {
            string riskState = string.Empty;
            if (!double.TryParse(TextBox1.Text, out double index1)) return _riskStateError;
            if (!double.TryParse(TextBox2.Text, out double index2)) return _riskStateError;
            if (!double.TryParse(TextBox3.Text, out double index3)) return _riskStateError;
            if (!double.TryParse(TextBox4.Text, out double index4)) return _riskStateError;
            if (!double.TryParse(TextBox5.Text, out double index5)) return _riskStateError;
            int counter = 0;
            if (index1 <= 5.5) { Ellipse1.Fill = Brushes.Red; counter++; }
            if (index2 >= 48.7) { Ellipse2.Fill = Brushes.Red; counter++; }
            if (index3 >= 15.85) { Ellipse3.Fill = Brushes.Red; counter++; }
            if (index4 <= 0.89) { Ellipse4.Fill = Brushes.Red; counter++; }
            if (index5 >= 40.0) { Ellipse5.Fill = Brushes.Red; counter++; }
            switch (counter)
            {
                case 0: case 1: case 2:
                    {
                        riskState = _riskStateLow; break;
                    }
                case 3: case 4:
                    {
                        riskState = _riskStateMiddle; break;
                    }
                case 5:
                    {
                        riskState = _riskStateHigh; break;
                    }
            }
            return riskState;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            FillEllipsis(Brushes.Green);
            string riskState = CalculateRiskState();
            InitializeFormToResultState(riskState);
        }

        private void ButtonRestart_Click(object sender, RoutedEventArgs e)
        {
            InitializeFormToEndState();
        }

        private static readonly Regex regexNumbers = new Regex("[0-9]*\\.?[0-9]+");

        public static void NumberPreviewTextInput(object sender, TextCompositionEventArgs e, double max)
        {
            TextBox textBox = (sender as TextBox);
            e.Handled = !regexNumbers.IsMatch(e.Text);
            string maxS = max.ToString();
            if (!e.Handled)
            {
                if (double.Parse(textBox.Text + e.Text) > max)
                {
                    e.Handled = true;
                    textBox.Text = maxS;
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
            else if (e.Text == ",")
            {
                if (textBox.Text.Length > 0 && textBox.Text != maxS && !textBox.Text.EndsWith(",")) e.Handled = false;
            }
        }

        private void TextBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberPreviewTextInput(sender, e, 100.0);
        }

        private void TextBox2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberPreviewTextInput(sender, e, 300.0);
        }

        private void TextBox3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberPreviewTextInput(sender, e, 100.0);
        }

        private void TextBox4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberPreviewTextInput(sender, e, 10.0);
        }

        private void TextBox5_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberPreviewTextInput(sender, e, 100.0);
        }
    }
}