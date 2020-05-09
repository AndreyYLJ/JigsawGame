using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace Jigsaw
{
    class LoadBitmap
    {
        public static Bitmap LoadBmp(string bmpFileName)
        {
            return new Bitmap(Application.StartupPath + "\\GamePictures\\" + bmpFileName+".bmp" );
        }

   
    }
}
