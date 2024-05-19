using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using MyCreate;

namespace ShootingGame3 {
    public partial class Form1 : Form {

        Dictionary<string,Sounds> soundList;

        const int CHARA_MOVE = 8;

        const int ReLODE_MAX = 150;
        int ReLode = 0;

        int PlayerHP = 5;
        bool DamageFlg =false;

        bool Clear_Flg = false;
        int BossHP = 250;

        public Form1() {
            InitializeComponent();
        }

        //グローバル変数
        struct sprite {
            public int x;//スプライトx座標
            public int y;//スプライトy座標
            public bool enabled;// 使用フラグ
        }

        //戦闘機
        static sprite Player;
        static sprite Boss;

        Bitmap red_img;

        static sprite beam1 ,beam2, beam3; // ビーム
        sprite[] beamList = { beam1, beam2, beam3 }; //ビームリスト
        static sprite monster1, monster2, monster3, monster4, monster5, monster6, monster7, monster8, monster9, monster10, monster11, monster12, monster13, monster14, monster15;//モンスター
        sprite[] monsterList = {monster1, monster2, monster3,monster4, monster5, monster6, monster7, monster8, monster9, monster10, monster11, monster12, monster13, monster14, monster15 };//モンスターリスト
        bool prevSpaceKey = false; //前回スペースキーの押下状態

        double crtMonsterTimer = 300.0;//モンスター生成タイマー
        Random rndCrtMonster = new Random();//モンスター生成間隔 (ms) 乱数
        Random rndMonsterX = new Random();//モンスター開始位置X座標乱数
        Random rndMoveX = new Random();//モンスターX座標移動乱数
        DateTime prevTime = DateTime.Now;//前回処理時刻
        double playTime = 500000;//プレイタイム
        int score = 0;// スコア
        Font font = new Font("MS ゴシック", 14); // フォント

        bool gameOverF = false;// ゲームオーバーフラグ
        string sound_path;


        // フォームを開く時に発生するイベント
        #region -------------【 ロードイベント   】---------------------------------------//
        private void Form1_Load(object sender, EventArgs e) {

            sound_path = Exe_Path.exe_Path() + @"sound\";
            soundList = new Dictionary<string, Sounds>(){
                 { "shoot", new Sounds(sound_path + "hakidasi_Star.wav") },
                 { "hit", new Sounds(sound_path + "strike_Hit.wav") },
                 { "famima", new Sounds(sound_path + "FamilyMart.wav") },
                 { "clear", new Sounds(sound_path + "clear.wav") },
                 { "bad", new Sounds(sound_path + "badend.wav") },
                 { "Damage", new Sounds(sound_path + "Damage.wav") },
            };


            Player.x = 220;//戦闘機x座標を設定
            Player.y = 450;//戦闘機y座標を設定
            Player.enabled = true; //戦闘機使用を設定

            Boss.x = this.Width / 2-50;
            Boss.y = 0;
            Boss.enabled = true;

            //red_img = Image_Color.Change_Color(Properties.Resources.Kirby_Star1);
            red_img = Image_Color.CreateNegativeImage(Properties.Resources.Kirby_Star1);

            soundList["famima"].Sound_Play();
        }
        //------------    【   末尾  】     ----------------//
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            //ループ変数にKeyValuePairを使う
            foreach (KeyValuePair<string, Sounds> kvp in soundList) {
                string id = kvp.Key;
                Sounds s = kvp.Value;
                s.EndSound();
            }

        }

        // フォームの再表示が必要になった時に発生するイベント
        #region -------------【 Formペイント 描画　イベント   】---------------------------------------//
        private void Form1_Paint(object sender, PaintEventArgs e) {

            e.Graphics.Clear(this.BackColor);// フォームの背景色を塗りつぶす
            if (!gameOverF && !Clear_Flg) { // ゲーム中       

                if (DamageFlg) {
                    e.Graphics.DrawImage(red_img, Player.x, Player.y);//戦闘機を表示
                } else {
                    e.Graphics.DrawImage(Properties.Resources.Kirby_Star1, Player.x, Player.y);//戦闘機を表示
                }

                if (Clear_Flg) {
                    e.Graphics.DrawImage(Properties.Resources.bang, Boss.x, Boss.y);//戦闘機を表示
                } else {
                    e.Graphics.DrawImage(Properties.Resources.lastBoss, Boss.x, Boss.y);//戦闘機を表示
                }
              
                
                #region -------------【 発射中のビームを表示する   】---------------------------------------//
                for (int i = 0; i < beamList.Length; i++) {
                    if (beamList[i].enabled == true) {
                        e.Graphics.DrawImage(Properties.Resources.Star1, beamList[i].x, beamList[i].y);
                    }
                }  //------------    【   末尾  】     ----------------//
                #endregion


                #region -------------【  敵の表示  】---------------------------------------//
                //出現中のモンスターを表示する。
                for (int i = 0; i < monsterList.Length; i++) {

                    if (monsterList[i].enabled == true) {
                        e.Graphics.DrawImage(Properties.Resources.Shoot, monsterList[i].x, monsterList[i].y);
                    }
                }
                //------------    【 敵の表示  末尾  】     ----------------//
                #endregion


            }else if (Clear_Flg) {
                e.Graphics.DrawImage(Properties.Resources.gameclear, 80, 250);
                soundList["clear"].Sound_Play();
            } 
            else { //ゲームオーバー
                e.Graphics.DrawImage(Properties.Resources.gameover, 80, 250);
                soundList["bad"].Sound_Play();
            }

           #region -------------【 得点と残り時間の表示   】---------------------------------------//
            e.Graphics.DrawString("得点:" + score, font, Brushes.White, 250, 10);
            int time = (int)playTime / 1000;
            e.Graphics.DrawString("Time: " + time, font,Brushes.White, 350, 10);
            //------------    【   末尾  】     ----------------//
           #endregion

        }  //------------    【  Form　ペイントイベント 末尾  】     ----------------//
        #endregion



        #region -------------【  タイマーイベント  】---------------------------------------//
        private void tmrUpdate_Tick(object sender, EventArgs e) {

            #region -------------【 ゲームオーバーの処理   】---------------------------------------//
            if (gameOverF) { return; } //ゲームオーバー
            //------------    【   末尾  】     ----------------//
            #endregion

            #region -------------【  制限時間の計算処理  】---------------------------------------//
            TimeSpan lap = DateTime.Now - prevTime; // 前回からの経過時間
            prevTime = DateTime.Now; //前回処理の時刻を設定
            playTime -= lap.TotalMilliseconds; //残り時間を設定
            if (playTime <= 0.0) { gameOverF = true; } //タイムオーバー
            //------------    【   末尾  】     ----------------//
            #endregion


            #region -------------【  自分の左右の移動  】---------------------------------------//
            //→キーが押されていたら右に5ピクセル移動
            if (Keyboard.IsKeyDown(Key.Right)) { Player.x += CHARA_MOVE; }
            //←キーが押されていたら左に5ピクセル移動
            if (Keyboard.IsKeyDown(Key.Left)) { Player.x -= CHARA_MOVE; }
            //------------    【   末尾  】     ----------------//
            #endregion

            #region -------------【  ビーム判定  】---------------------------------------//

            if (Keyboard.IsKeyDown(Key.Space)) { //スペースキーON?
                if (!prevSpaceKey) { //前回スペースキーOFF?
                    prevSpaceKey = true; //前回スペースキーをONに設定
                                         //ビームリストより未使用のビームを探して使用に変更
                    for (int i = 0; i < beamList.Length; i++) {//3発ずつ出せるビーム
                        if (beamList[i].enabled == false) {
                            beamList[i].x = Player.x;
                            beamList[i].y = Player.y;
                            beamList[i].enabled = true;
                            soundList["shoot"].Sound_Play();
                            break;
                        }
                    }
                }

            } else {
                prevSpaceKey = false; //前回スペースキーをOFFに設定
            }
            
            //ビームを動かしてモンスターに命中しているかを判定
            for (int i = 0; i < beamList.Length; i++) {
                    if (beamList[i].enabled == true) { //ビーム発射中?
                        beamList[i].y -= 20; //ビームを上に移動
                                            // フォームの上端に達したらビームを未使用に設定
                        if (beamList[i].y < 0) {
                            beamList[i].enabled = false;
                        } else {
                            // ビームがモンスターに命中した時の処理
                            for (int j = 0; j < monsterList.Length; j++) {
                                //モンスターは使用中
                                if (monsterList[j].enabled == true) {
                                    //ビームガモンスターに命中?
                                    if (IsHit(Properties.Resources.Star1, beamList[i], Properties.Resources.Shoot, monsterList[j])) {
                                        // ビームとモンスターに未使用を設定
                                        beamList[i].enabled = false;
                                        monsterList[j].enabled = false;
                                       // soundList["hit"].Sound_Play();
                                        score += 10; //スコアを加算
                                    }

                                if (IsHit(Properties.Resources.Star1, beamList[i], Properties.Resources.lastBoss, Boss)) {
                                    // ビームとモンスターに未使用を設定
                                    beamList[i].enabled = false;
                                    BossHP--;

                                    score += 20; //スコアを加算

                                    if (BossHP>0) {
                                        progressBoss.Value = BossHP;
                                    } else {
                                        progressBoss.Value = 0;
                                        Clear_Flg = true;
                                    }
                                }
                            }
                            }
                        }
                    }
            }//------------    【   末尾  】     ----------------//
            #endregion

            #region -------------【  敵の表示判定  】---------------------------------------//
            //モンスターを下に移動させる
            for (int i = 0; i < monsterList.Length; i++) {
                if (monsterList[i].enabled == true) {
                    monsterList[i].x += rndMoveX.Next(-10, 11);
                    monsterList[i].y += 10;

                    if (DamageFlg) {
                        ReLode++;
                        if (ReLODE_MAX<=ReLode) {
                            DamageFlg = false;
                            ReLode = 0;
                        }
                    } else {
                        //モンスターが戦闘機に接触した時はゲームオーバー
                        if (IsHit_Me(Properties.Resources.Kirby_Star1, Player, Properties.Resources.Shoot, monsterList[i])) {
                            PlayerHP--;
                            

                            if ((PlayerHP) > 0) {
                                DamageFlg = true;
                                soundList["Damage"].Sound_Play();
                                progressBar1.Value = PlayerHP;
                                break;
                            } else {
                                progressBar1.Value = 0;
                                gameOverF = true;
                                break;
                            }

                        }
                    }

                    //下端まで達したモンスターを未使用に設定
                    if (monsterList[i].y > this.Height) {
                        monsterList[i].enabled = false;
                    }
                } 
            }

            //モンスターの生成タイマがタイムアップした時、モンスターを生成
            crtMonsterTimer -= lap.TotalMilliseconds;
            if (crtMonsterTimer <= 0) {
                for (int i = 0; i < monsterList.Length; i++) {
                    if (!monsterList[i].enabled) {
                        //乱数でモンスターの出現位置Xを取得
                        monsterList[i].x = rndMonsterX.Next(80, 400);
                        monsterList[i].y = 0;
                        // 次回、 モンスターを生成する時間をタイマーに設定
                        crtMonsterTimer = rndCrtMonster.Next(500, 1000);
                        monsterList[i].enabled = true;
                        break;
                    }
                }
            }
             //------------    【   末尾  】     ----------------//
             #endregion

             this.Invalidate(); //Form1_Paintを実行させる
        }//------------    【  タイマー 末尾  】     ----------------//
        #endregion


        #region -------------【  四角形と四角形との接触を判定  】---------------------------------------//
        bool IsHit(Image meSprite, sprite me, Image youSprite, sprite you) { // 
            double meX1 = me.x; //自分の四角形左上座標x
            double meY1 = me.y; //自分の四角形左上座標y
            double meX2 = meX1 + meSprite.Width; //自分の四角形右下座標x
            double meY2 = meY1 + meSprite.Height;//自分の四角形右下座標y
            double youX1 = you.x;//相手の四角形左上座標x
            double youY1 = you.y;//相手の四角形左上座標y
            double youX2 = youX1 + youSprite.Width; //相手の四角形右下座標x
            double youY2 = youY1 + youSprite.Height; //相手の四角形右下座標x

            //Hit_Rectangle((int)meX1,(int)me.y);

            //四角形と四角形の接触の判定
            if (meX1 < youX2 && youX1 < meX2 && meY1 < youY2 && youY1 < meY2) {
                return true;
            } else {
                return false;
            }
        }//------------    【  四角形と四角形の接触判定 末尾  】     ----------------//
        #endregion

        #region -------------【  四角形と四角形との接触を判定  】---------------------------------------//
        bool IsHit_Me(Image Player_img,sprite Player,Image youSprite, sprite you) { // 
            double meX1 = Player.x+5; //自分の四角形左上座標x
            double meY1 = Player.y+5; //自分の四角形左上座標y
            double meX2 = meX1 + Player_img.Width-10; //自分の四角形右下座標x
            double meY2 = meY1 + Player_img.Height-10;//自分の四角形右下座標y
            double youX1 = you.x;//相手の四角形左上座標x
            double youY1 = you.y;//相手の四角形左上座標y
            double youX2 = youX1 + youSprite.Width; //相手の四角形右下座標x
            double youY2 = youY1 + youSprite.Height; //相手の四角形右下座標x

            //Hit_Rectangle((int)meX1, (int)meY1);

            //四角形と四角形の接触の判定
            if (meX1 < youX2 && youX1 < meX2 && meY1 < youY2 && youY1 < meY2) {
                return true;
            } else {
                return false;
            }
        }//------------    【  四角形と四角形の接触判定 末尾  】     ----------------//
        #endregion


        void Hit_Rectangle(int x, int y) {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(this.Width, this.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            //位置(10, 20)に100x80の長方形を描く
            g.FillRectangle(Brushes.Red, x, y, 40, 40);

            //PictureBox1に表示する
            this.BackgroundImage = canvas;
        }


    }//classの最後
}
