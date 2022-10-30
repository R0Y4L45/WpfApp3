using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp3;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        comboSize.ItemsSource = new List<int> { 8, 9, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        comboStyle.ItemsSource = Fonts.SystemFontFamilies;
        foreach (var item in typeof(Colors).GetProperties())
            comboColor.Items.Add(item.Name);
    }

    private void comboSize_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        var btn = sender as ComboBox;

        if (btn == comboStyle)
        {
            if (textBox.Selection.Text != string.Empty)
                textBox.Selection.ApplyPropertyValue(FontFamilyProperty, comboStyle.SelectedItem.ToString());
            else
                textBox.FontFamily = (FontFamily)comboStyle.SelectedItem;
        }
        else if (btn == comboSize)
        {
            if (textBox.Selection.Text != string.Empty)
                textBox.Selection?.ApplyPropertyValue(FontSizeProperty, comboSize.SelectedItem.ToString());
            else
                textBox.FontSize = double.Parse(comboSize.SelectedItem.ToString()!);
        }
        else if (btn == comboColor)
        {
            if (textBox.Selection.Text != string.Empty)
                textBox.Selection?.ApplyPropertyValue(ForegroundProperty, comboColor.SelectedItem);
            else
                foreach (var item in typeof(Colors).GetProperties())
                    if (item.Name == comboColor.SelectedItem.ToString())
                        textBox.Foreground = new SolidColorBrush((Color)(item as PropertyInfo)!.GetValue(null, null)!);
        }
        else
            MessageBox.Show("Null occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var btn = sender as Button;

        if (btn == bold)
            textBox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
        if (btn == italic)
            textBox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
        if (btn == under)
            textBox.Selection.ApplyPropertyValue(TextBlock.TextDecorationsProperty, TextDecorations.Underline);
        if (btn == normal)
        {
            textBox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
            textBox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
        }
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var btn = sender as MenuItem;
        if (btn is not null)
        {
            if (btn.Header.ToString() == "_Save")
            {
                SaveFileDialog sv = new SaveFileDialog();
                sv.Filter = "Text Files(*.txt) | *.txt";
                if (sv.ShowDialog() == true)
                {
                    using StreamWriter sw = new(sv.FileName + ".txt");
                    sw.Write(new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text);
                }
            }
            else if (btn.Header.ToString() == "_Upload")
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Text Files |*.txt";

                if (op.ShowDialog() == true)
                {
                    using StreamReader sr = new(op.FileName);
                    textBox.Document.Blocks.Add(new Paragraph(new Run(sr.ReadToEnd())));
                }
            }
        }
    }
}
