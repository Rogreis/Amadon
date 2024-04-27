using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadonStandardLib.Classes
{
    public static class EnumerationExtension
    {
        public static string Description(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Description ?? "na";
        }
    }

    /// <summary>
    /// Used to change information about controls and web page design information
    /// <see href="https://mahapps.com/docs/themes/thememanager"/>
    /// </summary>
    public class ControlsAppearance
    {
        /// <summary>
        /// Returns font for a block object (family, size and foregroung
        /// </summary>
        //public Style ForegroundStyle
        //{
        //    get
        //    {
        //        Style style = new Style
        //        {
        //            TargetType = typeof(System.Windows.Documents.Block)
        //        };

        //        style.Setters.Add(new Setter(System.Windows.Documents.Block.FontFamilyProperty, new FontFamily(StaticObjects.Parameters.FontFamilyInfo)));
        //        style.Setters.Add(new Setter(System.Windows.Documents.Block.FontSizeProperty, StaticObjects.Parameters.FontSizeInfo));
        //        style.Setters.Add(new Setter(System.Windows.Documents.Block.ForegroundProperty, App.Appearance.GetForegroundColorBrush()));
        //        return style;
        //    }
        //}

        private ResourceDictionary Dic = null;

        //ICollection<ResourceDictionary> MergedDictionaries = Microsoft.Maui.Controls.Application.Current.Resources.MergedDictionaries;
        public ControlsAppearance()
        {
            //foreach (ResourceDictionary dic in Microsoft.Maui.Controls.Application.Current.Resources.MergedDictionaries)
            //{
            //    if ( (dic is DarkTheme || (dic.Source != null && dic.Source.OriginalString != null && dic.Source.OriginalString.IndexOf("DarkTheme") > 0)) ||
            //         (dic is LightTheme || (dic.Source != null && dic.Source.OriginalString != null && dic.Source.OriginalString.IndexOf("LightTheme") > 0)) )
            //    {
            //        Dic = dic;
            //    }
            //}

        }


        public string GetBackgroundColor()
        {
            return Dic["PageBackgroundColor"].ToString();
        }

        public string GetForegroundColor()
        {
            return Dic["PrimaryTextColor"].ToString();
        }

        public string GetHighlightColor()
        {
            return Dic["HighligthTextColor"].ToString();
        }



        //public string GetColor(MahColorNames color)
        //{
        //    string colorStr = color.Description() == "na" ? color.ToString() : color.Description();
        //    return Convert.ToString(Application.Current.FindResource($"MahApps.Colors.{colorStr}"));
        //}


        public string GetGrayColor(int grayColorNumber = 1)
        {
            return Colors.Gray.ToString();
        }

        public Brush GetGrayColorBrush(int grayColorNumber = 1)
        {
            return SolidColorBrush.Gray;
        }


        public Brush GetBackgroundColorBrush()
        {
            return SolidColorBrush.Black;
        }

        public Brush GetForegroundColorBrush()
        {
            return SolidColorBrush.White;
        }


        public Brush GetHighlightColorBrush()
        {
            return SolidColorBrush.Yellow;
        }


        public Brush GetGrayInativeColorBrush()
        {
            return SolidColorBrush.Gray;
        }


        //public void SetThemeInfo(Control control, bool reverse= false)
        //{
        //    //Style style = new Style
        //    //{
        //    //    TargetType = typeof(Control)
        //    //};

        //    //style.Setters.Add(new Setter(Control.FontFamilyProperty, new FontFamily(StaticObjects.Parameters.FontFamilyInfo)));
        //    //style.Setters.Add(new Setter(Control.FontSizeProperty, StaticObjects.Parameters.FontSizeInfo));
        //    //if (!(control is ComboBox || control is ListView))
        //    //{
        //    //    style.Setters.Add(new Setter(Control.BackgroundProperty, reverse ? GetHighlightColorBrush() : GetBackgroundColorBrush()));
        //    //    style.Setters.Add(new Setter(Control.ForegroundProperty, reverse ? GetBackgroundColorBrush() : GetForegroundColorBrush()));
        //    //}
        //    //control.Style = style;
        //}

        //public void SetThemeInfo(TextBlock control, bool reverse = false)
        //{

        //    //Style style = new Style
        //    //{
        //    //    TargetType = typeof(TextBlock)
        //    //};

        //    //style.Setters.Add(new Setter(TextBlock.FontFamilyProperty, new FontFamily(StaticObjects.Parameters.FontFamilyInfo)));
        //    //style.Setters.Add(new Setter(TextBlock.FontSizeProperty, StaticObjects.Parameters.FontSizeInfo));

        //    //style.Setters.Add(new Setter(TextBlock.BackgroundProperty, reverse ? GetHighlightColorBrush() : GetBackgroundColorBrush()));
        //    //style.Setters.Add(new Setter(TextBlock.ForegroundProperty, reverse ? GetBackgroundColorBrush() : GetForegroundColorBrush()));
        //    //control.Style = style;
        //}


        //public void SetFontSize(Control control)
        //{
        //    //Style style = new Style
        //    //{
        //    //    TargetType = typeof(Control)
        //    //};
        //    //style.Setters.Add(new Setter(Control.FontSizeProperty, StaticObjects.Parameters.FontSizeInfo));
        //    //control.Style = style;
        //}

        //public void SetFontSize(TextBlock control)
        //{
        //    control.FontSize = StaticObjects.Parameters.FontSizeInfo;
        //}

        //public void SetHeight(Control control)
        //{
        //    double delta = 10;
        //    control.Height = StaticObjects.Parameters.FontSizeInfo + delta;
        //}

        //public string ThemeColor
        //{
        //    get
        //    {
        //        Theme theme = ThemeManager.Current.DetectTheme();
        //        char[] sep = { '.' };
        //        return theme.Name.Split(sep, StringSplitOptions.RemoveEmptyEntries)[1];
        //    }
        //    set
        //    {
        //        ((ParametersCore)StaticObjects.Parameters).ThemeColor = value;
        //        ThemeManager.Current.ChangeTheme(Application.Current, $"{Theme}.{value}");
        //        EventsControl.FireAppearanceChanged();
        //    }
        //}

        //public string Theme
        //{
        //    get
        //    {
        //        Theme theme = ThemeManager.Current.DetectTheme();
        //        char[] sep = { '.' };
        //        return theme.Name.Split(sep)[0];
        //    }
        //    set
        //    {
        //        ((ParametersCore)StaticObjects.Parameters).ThemeName = value;
        //        ThemeManager.Current.ChangeTheme(Application.Current, $"{value}.{((ParametersCore)StaticObjects.Parameters).ThemeColor}");
        //        EventsControl.FireAppearanceChanged();
        //    }
        //}

    }
}
