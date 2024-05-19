using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
//using System.Windows.Media;

namespace MyCreate {
    public class Sounds {
    
    #region ------------音楽再生機能  インスタンスの接続口--------
    public Sounds(string Path) {
            Property_path = Path;
            //読み込む
            player = new System.Media.SoundPlayer(Property_path);
    }
    #endregion

        //---------【Property】--------//
        public string Property_path { get; set; }
        private System.Media.SoundPlayer player = null;
        //-------------------------------//


    public void Sound_Play()
    {
        PlaySound();//音楽ファイルのパスから再生
        // PlaySound("C:\\music.wav");
    }


    public void Sound_STOP()
    {
            StopSound();//再生していた音楽を停止
    }




        #region ----------プログラム本体----------
        //WAVEファイルを再生する
        private void PlaySound()
        {
            ////再生されているときは止める
            //if (player != null)
            //{
            //    StopSound();
            //}

            //非同期再生する
            player.Play();

            //次のようにすると、ループ再生される
            //player.PlayLooping();

            //次のようにすると、最後まで再生し終えるまで待機する
           //player.PlaySync();
        }

        //再生されている音を止める
        private void StopSound()
        {
            if (player != null)
            {
                player.Stop();
            }
        }
        #endregion

        public void EndSound() {
            player.Dispose();
            player = null;
        }

        #region  --------------オマケ-----------
        public void Explorer_Open()
        {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            System.Diagnostics.Process.Start("EXPLORER.EXE", folderPath1);

        }

        public void Explorer_Open(string Folder)
        {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            folderPath1 += @"\" + Folder + @"\";
            System.Diagnostics.Process.Start("EXPLORER.EXE", folderPath1);

        }

        public string Exe_Path()
        {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            folderPath1 += @"\";
            return folderPath1;

        }
        public string Exe_Path(string Folder)
        {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            folderPath1 += @"\" + Folder + @"\";
            return folderPath1;

        }

        public void exe_Folder_in_SoundList(string Folder, ref ListBox list1, ref ListBox list2)
        {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            folderPath1 += @"\" + Folder + @"\";
            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(folderPath1 + "\\", "*.wav", System.IO.SearchOption.TopDirectoryOnly);//実行するのは検索したい場所の親フォルダから


            //ファイルを列挙する
            foreach (string f in files)
            {
                //ListBox1に結果を表示する
                list1.Items.Add(f);

                //拡張子なしのファイル名をパスから取得するには、「GetFileNameWithoutExtensionメソッド」を使います。

                string filePath = Path.GetFileNameWithoutExtension(f);
                list2.Items.Add(filePath);
            }

            if (list2.Items.Count > 0)
            {
                list2.SelectedIndex = 0;
            }
        }//-----------

        #endregion


    }

}
