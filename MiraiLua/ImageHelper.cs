using System.Drawing;

namespace MiraiLua
{
    // 这里是我抄的，我诚实，我值得骄傲
    public class ImageHelper
    {
        public void CreateImage(string text, string path)
        {
            Bitmap bmp = CreateImage(text);
            bmp.Save(path);
            bmp.Dispose();
        }
        public Bitmap CreateImage(string text)
        {
            int wid = 400;
            int high = 200;
            Font font;
            
            font = new Font("微软雅黑", 15, FontStyle.Regular);

            //绘笔颜色
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            Bitmap image = new Bitmap(wid, high);
            Graphics g = Graphics.FromImage(image);
            SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);//得到文本的宽高

            int width = (int)(sizef.Width + 1);
            int height = (int)(sizef.Height + 1);

            image.Dispose();
            image = new Bitmap(width, height);

            g = Graphics.FromImage(image);
            g.Clear(Color.White);//透明
            
            RectangleF rect = new RectangleF(0, 0, width, height);
            //绘制图片
            g.DrawString(text, font, brush, rect);
            //释放对象
            g.Dispose();

            return image;
        }
    }
}
