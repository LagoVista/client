using LagoVista.Core.Interfaces;
using LagoVista.Core.Models.Drawing;

namespace LagoVista.XPlat.Core.Resources
{
    public interface IMobileStyle : IAppStyle
    {
        Color EditControlPlaceholder { get; }
        Color EditControlFrameDisabled { get; }
        Color EditControlBackgroundDisabled { get; }
        Color EditControlTextDisabled { get; }
        Color EditControlPlaceHolderDisabled { get; }

        Color EditControlBackgroundFocus { get; }
        Color EditControlTextFocus { get; }
        Color EditControlPlaceholderFocus { get; }

        Color EditControlTextActive { get; }
        Color EditControlPlaceholderActive {get;}
        Color EditControlBackgroundActive { get; }
        Color EditControlBorderActive { get; }
    
        Color ButtonBackgroundDisabled { get; }
        Color ButtonForegroundDisabled { get; }
        Color ButtonBorderDisabled { get; }
    
        Color LinkColor { get; }
        Color LinkActive { get; }
        Color LinkDisabled { get; }
    }
}
