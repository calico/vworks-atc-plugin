using stdole;
using System.Drawing;
using System.Windows.Forms;

namespace VworksAtcPlugin
{
    public partial class ATCPlugin
    {
        internal class AxHostConverter : AxHost
        {
            private AxHostConverter() : base("") { }

            static public IPictureDisp ImageToPictureDisp(Image image)
            {
                return (IPictureDisp)GetIPictureDispFromPicture(image);
            }

            static public Image PictureDispToImage(IPictureDisp pictureDisp)
            {
                return GetPictureFromIPicture(pictureDisp);
            }
        }
    }
}
