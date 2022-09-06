using System;
using System.Windows;
using System.Windows.Controls;

namespace ServerForChorch
{
    /// <summary>
    /// Логика взаимодействия для Numeric.xaml
    /// </summary>
    public partial class Numeric : UserControl
    {
        public int Value { get { return textBox.Text != "" ? Convert.ToInt32(textBox.Text) : 0; } set { textBox.Text = value.ToString(); } } // Значение textBox.Text в типе int
        public Numeric()
        {
            InitializeComponent();
            Value = 5050;
        }
        private void Button_Up(object sender, RoutedEventArgs e)
        {
            Value += 5;
        }
        private void Button_Down(object sender, RoutedEventArgs e)
        {
            Value -= 5;
        }
        private void textChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox.Text != "")
            {
                try
                {
                    Convert.ToInt32(textBox.Text);
                }
                catch (Exception)
                {
                    string str = "";
                    int outStr;
                    for (int i = 0; i < textBox.Text.Length; i++)
                    {
                        if (int.TryParse(textBox.Text[i].ToString(), out outStr))
                            str += outStr.ToString();
                    }
                    int index = textBox.CaretIndex;
                    textBox.Text = str;
                    textBox.CaretIndex = index - 1;
                }
            }
        }
        public void setBlackStyle()
        {
            buttonUp.Style = (Style)Application.Current.Resources["blackButtonStyle"];
            buttonDown.Style = (Style)Application.Current.Resources["blackButtonStyle"];
            textBox.Style = (Style)Application.Current.Resources["blackTextBox"];
        }
        public void setWhiteStyle()
        {
            buttonUp.Style = (Style)Application.Current.Resources["whiteButtonStyle"];
            buttonDown.Style = (Style)Application.Current.Resources["whiteButtonStyle"];
            textBox.Style = (Style)Application.Current.Resources["whiteTextBox"];
        }
    }
}
