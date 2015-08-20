using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Interactivity;
using System.Windows.Input;
using System.Globalization;
using System.Reflection;
using System.Xml.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace WpfSaveToXmlSample
{
    public static class Setting
    {
        // CA1810が発生するが、設計上仕方ないので無効にする。

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        [method: SuppressMessage("Microsoft.Performance", "CA1810", Justification = "static class使用のため。")]
        static Setting()
        {
            // フィールドの初期化
            Setting.FSettingFile = new SettingFile();
            Setting.No = 0;

            // 初期値は実行中のアセンブリの位置
            string path = Assembly.GetExecutingAssembly().Location;

            // 実行ファイルのパスを取得
            // （dllの場合、呼び出し元の実行ファイルのパスになる）
            var ass = Assembly.GetEntryAssembly();
            if (ass != null)
            {
                path = ass.Location;
            }

            // パスからディレクトリ名を抽出
            Setting.EntryLocation = Path.GetDirectoryName(path);

            // Setting.xmlが無い時はApp.configから読む
            if (Setting.ExistSetting())
            {
                ReadSettings();
            }
            else
            {
                LoadDefaultSettings();
            }
        }

        /// <summary>
        /// 設定ファイルが存在したらtrueを返します。
        /// </summary>
        /// <returns></returns>
        private static bool ExistSetting()
        {
            var file = Setting.GetSettingFileName();
            var path = Path.Combine(Setting.EntryLocation, file);
            return File.Exists(path);
        }

        /// <summary>
        /// 設定ファイルのファイル名を作成して返します。
        /// </summary>
        /// <returns></returns>
        private static string GetSettingFileName()
        {
            var sb = new StringBuilder();
            sb.Append("Setting");
            sb.Append(Setting.No.ToString("D2",CultureInfo.GetCultureInfo("ja-jp")));
            sb.Append(".xml");
            return sb.ToString();
        }

        /// <summary>
        /// Setting.xmlから設定値を読み込む
        /// </summary>
        private static void ReadSettings()
        {
            // ファイルのパスを取得
            var file = Setting.GetSettingFileName();
            var path = Path.Combine(Setting.EntryLocation, file);

            // 設定ファイルを読み込み
            var serializer = new XmlSerializer(typeof(SettingFile));
            using (var stream = new StreamReader(path, new UTF8Encoding(false)))
            {
                Setting.FSettingFile = (SettingFile)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 初期値をApp.configから読み込む
        /// </summary>
        private static void LoadDefaultSettings()
        {
            var f = Setting.FSettingFile;
            f.MainWindowBounds = Properties.Settings.Default.MainWindow_Bounds;
            var c = f.DataGridColumns;
            c.Clear();
            foreach (var item in Properties.Settings.Default.DataGrid_Columns)
            {
                c.Add(item);
            }
        }

        /// <summary>
        /// 各種設定をファイルに保存します。
        /// </summary>
        public static void Save()
        {
            // ファイルのパスを取得
            var file = Setting.GetSettingFileName();
            var path = Path.Combine(Setting.EntryLocation, file);

            // 設定ファイルの書き込み
            var serializer = new XmlSerializer(typeof(SettingFile));
            using (var stream = new StreamWriter(path, false, new UTF8Encoding(false)))
            {
                serializer.Serialize(stream, Setting.FSettingFile);
            }
        }

        /// <summary>
        /// 設定ファイルの情報を記憶します。
        /// </summary>
        private static SettingFile FSettingFile;

        /// <summary>
        /// 実行ファイルのディレクトリ名を管理します。
        /// </summary>
        public static string EntryLocation { get; private set; }

        /// <summary>
        /// 現在使用中の設定ファイルの番号を管理します。
        /// </summary>
        public static int No { get; set; }

        #region MainWindow位置、サイズ保存

        /// <summary>
        /// MainWindow位置、サイズ保存
        /// </summary>
        public static Rect MainWindowBounds
        {
            get
            {
                return Setting.FSettingFile.MainWindowBounds;
            }
            private set
            {
                Setting.FSettingFile.MainWindowBounds = value;
            }
        }

        /// <summary>
        /// Windowサイズを現在使用中の設定ファイルに保存します。
        /// </summary>
        /// <param name="savWindow">記憶するWindowオブジェクト</param>
        public static void SetMainWindowBounds(Window savWindow)
        {
            var w = savWindow;
            if (w != null)
            {
                Rect r = (w.WindowState == WindowState.Minimized) ?
                    w.RestoreBounds : new Rect(w.Left, w.Top, w.Width, w.Height);

                if (w.Name == "FMainWindow")
                {
                    Setting.MainWindowBounds = r;
                }
            }
            else
            {
                throw new ArgumentNullException("savWindow");
            }
        }

        #endregion

        #region DataGridカラム位置、幅保存

        public static ColumnSettingCollection DataGridColumns
        {
            get
            {
                return Setting.FSettingFile.DataGridColumns;
            }
        }

        public static void SetDataGridColumns(DataGrid savGrid)
        {
            var dg = savGrid;
            if (dg != null)
            {
                if (dg.Name == "FDataGrid")
                {
                    var c = Setting.DataGridColumns;
                    c.Clear();
                    foreach (var item in dg.Columns)
                    {
                        double width = (item.Width.IsAuto) ? -1.0 : item.Width.Value;
                        c.Add(new ColumnSetting() { DisplayIndex = item.DisplayIndex, Width = width });
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("savGrid");
            }
        }

        #endregion

    }

    public class SettingFile
    {
        /// <summary>
        /// MainWindowの位置、大きさの情報を管理します。
        /// </summary>
        public Rect MainWindowBounds { get; set; }

        /// <summary>
        /// DataGridのカラム情報を管理します。
        /// </summary>
        public ColumnSettingCollection DataGridColumns { get; }
    }

    /// <summary>
    /// DataGridのカラム情報を管理するクラスのコレクションを定義します。
    /// </summary>
    public class ColumnSettingCollection : Collection<ColumnSetting>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ColumnSettingCollection()
        {

        }

        /// <summary>
        /// 引数付きコンストラクタ
        /// </summary>
        /// <param name="list">コレクションにあらかじめ追加するList型のコレクションを指定します。</param>
        public ColumnSettingCollection(IList<ColumnSetting> list) : base(list)
        {
        }
    }

    public class DoubleToDataGridLengthConverter : IValueConverter
    {
        /// <summary>
        /// Doubleの値をDataGridLengthに変換します。-1のときはDataGridLength.Autoに変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridLength l = 0;
            if (value != null)
            {
                string d = value.ToString();
                switch (d)
                {
                    case "-1":
                        l = DataGridLength.Auto;
                        break;
                    default:
                        l = (double)value;
                        break;
                }
            }
            else
            {
                throw new ArgumentNullException("value");
            }
            return l;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Windowの情報を保存するTriggerActionを定義します。
    /// </summary>
    public class SavingWindowStateAction : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// Windowの情報を保存します。
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            var param = this.Parameter;
            if (param != null)
            {
                Setting.SetMainWindowBounds(param);
            }
        }

        /// <summary>
        /// アクションの引数となるオブジェクトを管理します。
        /// </summary>
        public Window Parameter
        {
            get { return (Window)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register("Parameter", typeof(Window), typeof(SavingWindowStateAction), new PropertyMetadata(null));
    }

    /// <summary>
    /// Windowの情報を保存するTriggerActionを定義します。
    /// </summary>
    public class SavingDataGridStateAction : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// Windowの情報を保存します。
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            var param = this.Parameter;
            if (param != null)
            {
                Setting.SetDataGridColumns(param);
            }
        }

        /// <summary>
        /// アクションの引数となるオブジェクトを管理します。
        /// </summary>
        public DataGrid Parameter
        {
            get { return (DataGrid)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register("Parameter", typeof(DataGrid), typeof(SavingDataGridStateAction), new PropertyMetadata(null));
    }

    /// <summary>
    /// DataGridColumnの設定保存用クラス
    /// </summary>
    [Serializable]
    public class ColumnSetting
    {
        public int DisplayIndex
        { get; set; }
        public double Width { get; set; }

        public override bool Equals(object obj)
        {
            var c = obj as ColumnSetting;
            if (this.DisplayIndex != c.DisplayIndex)            
            {
                return false;
            }
            if (this.Width != c.Width)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
