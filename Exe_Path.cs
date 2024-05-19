using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MyCreate {
    public class Exe_Path {

        public static string DownLoad_path() 
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\";
        }

        public static string Desktop_path() 
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+@"\";
        }
        public static string Document_path() 
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\";
        }
        public static string ScreenShot_path() 
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"\Screenshots\";
        }

        public static string Picter_path() 
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"\";
        }
        public static string Favorites_path() 
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites) + @"\";
        }
        public static string StartMenu_path() 
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\";
        }
        public static string System_path() 
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\";
        }

        public static string Parent_Folder(string path) {
            // string path = Application.ExecutablePath;
            return Path.GetDirectoryName(path);
        }

        public static void Parent_Folder_Open(string path) {
            // string path = Application.ExecutablePath;
            System.Diagnostics.Process.Start("EXPLORER.EXE", Path.GetDirectoryName(path));
        }

        public static void Explorer_Open(string strPath) {
            System.Diagnostics.Process.Start("EXPLORER.EXE", strPath);
        }

        public void Explorer_Open() {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            System.Diagnostics.Process.Start("EXPLORER.EXE", folderPath1);

        }



        public static string exe_Path() {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            folderPath1 += @"\";
            return folderPath1;

        }

        public string exe_Path(string Folder) {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            folderPath1 += @"\" + Folder + @"\";
            return folderPath1;

        }

        public string OpenFile_DownLoad() {

            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "すべてのファイル(*)|*|すべてのファイル(*.*)|*.*";
            op.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\";

            /* ダイアログを表示し「開く」場合 */
            if (op.ShowDialog() == DialogResult.OK) {

                /* ファイル名を表示する */
                MessageBox.Show(op.FileName);
                string Path = op.FileName;//選択したファイルへのパス(文字列型)

                //ファイル名をパスから取得するには、「GetFileNameメソッド」を使います
                string Name = System.IO.Path.GetFileName(Path);
                return Path;

            } else {
                return "";
            }
        }

        public string OpenFile_exe() {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);

            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "すべてのファイル(*)|*|すべてのファイル(*.*)|*.*";
            op.InitialDirectory = folderPath1;

            /* ダイアログを表示し「開く」場合 */
            if (op.ShowDialog() == DialogResult.OK) {

                /* ファイル名を表示する */
                MessageBox.Show(op.FileName);
                string Path = op.FileName;//選択したファイルへのパス(文字列型)

                return Path;

            } else {
                return "";
            }
        }

        public string OpenFile_exe(ref string Name) {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);

            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "すべてのファイル(*)|*|すべてのファイル(*.*)|*.*";
            op.InitialDirectory = folderPath1;

            /* ダイアログを表示し「開く」場合 */
            if (op.ShowDialog() == DialogResult.OK) {

                /* ファイル名を表示する */
                MessageBox.Show(op.FileName);
                string Path = op.FileName;//選択したファイルへのパス(文字列型)

                //ファイル名をパスから取得するには、「GetFileNameメソッド」を使います
                string name = System.IO.Path.GetFileName(Path);
                Name = name;

                return Path;

            } else {
                return "";
            }
        }

        public string OpenFile_exe(string FoldPath) {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);

            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "すべてのファイル(*)|*|すべてのファイル(*.*)|*.*";
            op.InitialDirectory = folderPath1 + @"\" + FoldPath;

            /* ダイアログを表示し「開く」場合 */
            if (op.ShowDialog() == DialogResult.OK) {


                string Path = op.FileName;//選択したファイルへのパス(文字列型)

                //ファイル名をパスから取得するには、「GetFileNameメソッド」を使います
                string Name = System.IO.Path.GetFileName(Path);
                /* ファイル名を表示する */
                MessageBox.Show(Name);

                return Path;

            } else {
                return "";
            }
        }//------------------

        public void exe_Folder_inList(string Folder, string Filter, ref ListBox list1, ref ListBox list2) {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            folderPath1 += @"\" + Folder + @"\";
            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(folderPath1 + "\\", Filter, System.IO.SearchOption.TopDirectoryOnly);//実行するのは検索したい場所の親フォルダから


            //ファイルを列挙する
            foreach (string f in files) {
                //ListBox1に結果を表示する
                list1.Items.Add(f);

                //拡張子なしのファイル名をパスから取得するには、「GetFileNameWithoutExtensionメソッド」を使います。

                string filePath = Path.GetFileNameWithoutExtension(f);
                list2.Items.Add(filePath);
            }

            if (list2.Items.Count > 0) {
                list2.SelectedIndex = 0;
            }
        }//----------------

        public void exe_Folder_inList_Access(string Folder, ref ListBox list1, ref ListBox list2) {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            folderPath1 += @"\" + Folder + @"\";
            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(folderPath1 + "\\", "*.accdb", System.IO.SearchOption.TopDirectoryOnly);//実行するのは検索したい場所の親フォルダから


            //ファイルを列挙する
            foreach (string f in files) {
                //ListBox1に結果を表示する
                list1.Items.Add(f);

                //拡張子なしのファイル名をパスから取得するには、「GetFileNameWithoutExtensionメソッド」を使います。

                string filePath = Path.GetFileNameWithoutExtension(f);
                list2.Items.Add(filePath);
            }

            if (list2.Items.Count > 0) {
                list2.SelectedIndex = 0;
            }
        }//-----------

        public void exe_Folder_inList_Text(string Folder, ref ListBox list1, ref ListBox list2) {
            string path = Application.ExecutablePath;
            string folderPath1 = Path.GetDirectoryName(path);
            folderPath1 += @"\"+Folder+@"\";
            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(folderPath1 + "\\", "*.txt", System.IO.SearchOption.TopDirectoryOnly);//実行するのは検索したい場所の親フォルダから


            //ファイルを列挙する
            foreach (string f in files) {
                //ListBox1に結果を表示する
                list1.Items.Add(f);

                //拡張子なしのファイル名をパスから取得するには、「GetFileNameWithoutExtensionメソッド」を使います。

                string filePath = Path.GetFileNameWithoutExtension(f);
                list2.Items.Add(filePath);
            }

            if (list2.Items.Count > 0) {
                list2.SelectedIndex = 0;
            }
        }//-----------


        public void Folder_inList_Text(string strPath, ref ListBox list1, ref ListBox list2) {


            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(strPath + "\\", "*.txt", System.IO.SearchOption.TopDirectoryOnly);//実行するのは検索したい場所の親フォルダから


            //ファイルを列挙する
            foreach (string f in files) {
                //ListBox1に結果を表示する
                list1.Items.Add(f);

                //拡張子なしのファイル名をパスから取得するには、「GetFileNameWithoutExtensionメソッド」を使います。

                string filePath = Path.GetFileNameWithoutExtension(f);
                list2.Items.Add(filePath);
            }

            if (list2.Items.Count > 0) {
                list2.SelectedIndex = 0;
            }
        }//-----------

    }
}
