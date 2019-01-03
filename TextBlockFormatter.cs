using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp2
{

    public class TextBlockFormatter
    {
        const string @namespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        public static readonly DependencyProperty FormattedTextProperty = DependencyProperty.RegisterAttached(
        "FormattedText",
        typeof(string),
        typeof(TextBlockFormatter),
        new PropertyMetadata(null, FormattedTextPropertyChanged));

        public static void SetFormattedText(DependencyObject textBlock, string value)
        {
            textBlock.SetValue(FormattedTextProperty, value);
        }

        public static string GetFormattedText(DependencyObject textBlock)
        {
            return (string)textBlock.GetValue(FormattedTextProperty);
        }

        private static void FormattedTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = (TextBlock)d;
            if (textBlock.DataContext == null)
            {
                textBlock.DataContextChanged += TextBlock_DataContextChanged;
                return;
            }
            var query = (string)e.NewValue ?? string.Empty;
            HighlightSearch(textBlock, query);
        }

        private static void HighlightSearch(TextBlock textBlock, string value)
        {
            var data = (Node)textBlock.DataContext;
            var name = ((Node)textBlock.DataContext).TreeValue;
            var query = value.ToUpper();
            bool b = false;
            SetTextBlockVisibility(textBlock, value, data, ref b);
            ColorTextblockBackground(textBlock, value, name);
        }

        private static void SetTextBlockVisibility(TextBlock textBlock, string value, Node data, ref bool b)
        {
            if (value == "" && data.ItemVisibility == Visibility.Collapsed)
            {
                data.ItemVisibility = Visibility.Visible;
                if (textBlock.Inlines.Count > 0)
                {
                    textBlock.Inlines.Clear();
                    textBlock.Text = data.TreeValue;
                }
                return;
            }
            if (data.NodeList.Count > 0 && value != "")
            {
                b = data.NodeList.Any(x => x.TreeValue.ToLower().Contains(value));
                if (b || data.TreeValue.ToLower().Contains(value.ToLower()))
                {
                    data.IsItemExpanded = true;
                    data.ItemVisibility = Visibility.Visible;
                }
                else
                {
                    data.ItemVisibility = Visibility.Collapsed;
                }
            }
            else if (data.NodeList.Count == 0)
            {
                if (!data.TreeValue.ToLower().Contains(value.ToLower()))
                {
                    data.ItemVisibility = Visibility.Collapsed;
                    return;
                }
                else
                {
                    data.ItemVisibility = Visibility.Visible;
                }
            }
        }

        private static void ColorTextblockBackground(TextBlock textBlock, string value, string name)
        {
            var stringArray = name.Split(new string[] { value }, StringSplitOptions.None);
            var laststring = stringArray.Last();
            textBlock.ClearValue(TextBlock.TextProperty);
            textBlock.Inlines.Clear();
            foreach (var item in stringArray)
            {
                if (item == laststring)
                {
                    textBlock.Inlines.Add(item);
                    continue;
                }
                textBlock.Inlines.Add(item);
                textBlock.Inlines.Add(new Run(value) { Background = Brushes.OrangeRed });
            }
        }

        private static void TextBlock_DataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var fr = (FrameworkElement)sender;
            var block = (TextBlock)sender;
            if (block.DataContext == null) return;
            block.DataContextChanged -= TextBlock_DataContextChanged;
            var query = (string)fr.GetValue(FormattedTextProperty);
            HighlightSearch(block, query);
        }

        public static T LoadXaml<T>(string xaml)
        {
            using (var stringReader = new System.IO.StringReader(xaml))
            using (var xmlReader = System.Xml.XmlReader.Create(stringReader))
                return (T)System.Windows.Markup.XamlReader.Load(xmlReader);
        }

    }
}
