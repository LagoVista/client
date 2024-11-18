using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Services
{
    public static class ResourceSupport
    {
        public static Color GetColor(string name)
        {
            if (Application.Current.Resources.TryGetValue(name, out var resource))
            {
                return (Color)resource;
            }

            return default;
        }

        public static bool UseCustomColors { get; set; } = false;

        public static bool UseCustomfonts { get; set; } = false;

        public static Color AccentColor
        {
            get
            {
                return Color.Accent;
            }
        }

        public static Style GetStyle(string name)
        {

            if (Application.Current.Resources.TryGetValue(name, out var resource))
            {
                return (Style)resource;
            }
            return default;
        }

        public static double GetNumber(string name)
        {
            if (Application.Current.Resources.TryGetValue(name, out var resource))
            {
                return (double)resource;
            }
            return default(double);
        }

        public static string GetString(string name)
        {
            if (Application.Current.Resources.TryGetValue(name, out var resource))
            {
                return (string)resource;
            }

            return default;
        }
    }
}
