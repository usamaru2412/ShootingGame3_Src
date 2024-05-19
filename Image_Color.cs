using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCreate {
   public class Image_Color {

        public static Bitmap Change_Color(Bitmap img) {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(img.Width, img.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            ////画像を取得
            //Bitmap img = SystemIcons.WinLogo.ToBitmap();

            //ColorMapオブジェクトの配列（カラーリマップテーブル）を作成
            System.Drawing.Imaging.ColorMap[] cms =
                new System.Drawing.Imaging.ColorMap[]
                    {new System.Drawing.Imaging.ColorMap(), new System.Drawing.Imaging.ColorMap()};

            //青を黒に変換する
            cms[0].OldColor = Color.Yellow;
            cms[0].NewColor = Color.MistyRose;
            //黒を白に変換する
            cms[1].OldColor = Color.White;
            cms[1].NewColor = Color.Red;

            //ImageAttributesオブジェクトの作成
            System.Drawing.Imaging.ImageAttributes ia =
                new System.Drawing.Imaging.ImageAttributes();
            //ColorMapを設定
            ia.SetRemapTable(cms);

            //画像を普通に描画
            g.DrawImage(img, 0, 0);

            //色を変換して画像を描画
            g.DrawImage(img, new Rectangle(img.Width + 10, 0, img.Width, img.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

            //Graphicsオブジェクトのリソースを解放する
            g.Dispose();

            //PictureBox1に表示する
            return canvas;
        }


        // 色反転の処理
        public static Bitmap CreateNegativeImage(Bitmap img) {
            //ネガティブイメージの描画先となるImageオブジェクトを作成する
            Bitmap negaImg = new Bitmap(img.Width, img.Height);
            //negaImgのGraphicsオブジェクトを取得
            Graphics g = Graphics.FromImage(negaImg);
            //ColorMatrixオブジェクトの作成
            ColorMatrix cm = new ColorMatrix();
            cm.Matrix00 = -1;
            cm.Matrix11 = -1;
            cm.Matrix22 = -1;
            cm.Matrix33 = 1;
            cm.Matrix40 = cm.Matrix41 = cm.Matrix42 = cm.Matrix44 = 1;

            //ImageAttributesを使用して色が反転した画像を描画する
            //ImageAttributesオブジェクトを作成する
            ImageAttributes ia = new ImageAttributes();
            //ColorMatrixを設定する
            ia.SetColorMatrix(cm);
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            //リソースを解放する
            g.Dispose();
            return negaImg;
        }






    }
}
