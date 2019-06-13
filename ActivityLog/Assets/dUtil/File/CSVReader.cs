
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.IO;


namespace du.File {

    // 参考サイト
    // http://magnaga.com/unity/2016/06/20/csvreader/

    //! CSVの列とのマッピングのための属性クラス
    public class CSVColAttr : Attribute {
        public int ColumnIndex { get; set; }
        public object DefaultValue { get; set; }

        public CSVColAttr(int columnIndex, object defaultValue = null) {
            ColumnIndex = columnIndex;
            DefaultValue = defaultValue;
        }

    }


    public class CSVReader<T> : IEnumerable<T>, IDisposable
    where T : class, new()
    {
        #region field
#if USE_EVENT    // 変換できない場合に、イベントを発生させ使用者に判断させる場合
        public event EventHandler<ConvertFailedEventArgs> ConvertFailed;
#endif
        /// <summary>
        /// Type毎のデータコンバーター
        /// </summary>
        private Dictionary<Type, TypeConverter> converters = new Dictionary<Type, TypeConverter>();

        /// <summary>
        /// 列番号をキーとしてフィールド or プロパティへのsetメソッドが格納されます。
        /// </summary>
        private Dictionary<int, Action<object, string>>
            setters = new Dictionary<int, Action<object, string>>();

        /// <summary>
        /// Tの情報をロードします。
        /// setterには列番号をキーとしたsetメソッドが格納されます。
        /// </summary>
        private void LoadType() {

            Type type = typeof(T);

            // Field, Property のみを対象とする
            var memberTypes = new MemberTypes[] { MemberTypes.Field, MemberTypes.Property };

            // インスタンスメンバーを対象とする
            BindingFlags flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (MemberInfo member in type.GetMembers(flag).Where((member) => memberTypes.Contains(member.MemberType)))
            {
                CSVColAttr csvCol = GetCSVColumnAttribute(member);
                if (csvCol == null) continue;

                if (member.MemberType == MemberTypes.Field)
                {
                    // field
                    FieldInfo fieldInfo = type.GetField(member.Name, flag);
                    setters[csvCol.ColumnIndex] = (target, value) =>
                        fieldInfo.SetValue(target, GetConvertedValue(fieldInfo, value, csvCol.DefaultValue));
                }
                else
                {
                    // property
                    PropertyInfo propertyInfo = type.GetProperty(member.Name, flag);
                    setters[csvCol.ColumnIndex] = (target, value) =>
                        propertyInfo.SetValue(target, GetConvertedValue(propertyInfo, value, csvCol.DefaultValue), null);
                }
            }

        }

        /// <summary>
        /// 対象のMemberInfoからCSVColumnAttributeを取得する
        /// </summary>
        /// <param name="member">確認対象のMemberInfo</param>
        /// <returns>CSVColumnAttributeのインスタンス、設定されていなければnull</returns>
        private CSVColAttr GetCSVColumnAttribute(MemberInfo member)
        {
            return  (CSVColAttr) member.GetCustomAttributes(typeof(CSVColAttr), true).FirstOrDefault();
            //return member.GetCustomAttributes<CSVColumnAttribute>().FirstOrDefault();
        }

        /// <summary>
        /// valueを対象のTypeへ変換する。できない場合はdefaultを返す
        /// </summary>
        /// <param name="type">変換後の型</param>
        /// <param name="value">変換元の値</param>
        /// <param name="default">規定値</param>
        /// <returns></returns>
        private object GetConvertedValue(MemberInfo info, object value, object @default)
        {
            Type type = null;
            if (info is FieldInfo)
            {
                type = (info as FieldInfo).FieldType;
            }
            else if (info is PropertyInfo)
            {
                type = (info as PropertyInfo).PropertyType;
            }

            // コンバーターは同じTypeを使用することがあるため、キャッシュしておく
            if (!converters.ContainsKey(type))
            {
                converters[type] = TypeDescriptor.GetConverter(type);
            }

            TypeConverter converter = converters[type];

            ////変換できない場合に例外を受け取りたい場合
            //return converter.ConvertFrom(value);

            //失敗した場合に CSVColumnAttribute の規定値プロパティを返す場合
            try
            {
                // 変換した値を返す。
                return converter.ConvertFrom(value);
            }
            catch (Exception)
            {
                // 変換できなかった場合は規定値を返す
                return @default;
            }

#if USE_EVENT    // 変換できない場合に、イベントを発生させ使用者に判断させる場合
            try {
                return converter.ConvertFrom(value);
            }
            catch (Exception ex) {
                // イベント引数の作成
                var e = new ConvertFailedEventArgs(info, value, @default, ex);
                // イベントに関連付けられたメソッドがない場合は例外を投げる
                if (ConvertFailed == null) { throw; }
                ConvertFailed(this, e); // 使用する際に判断させる
                return e.CorrectValue;
            }
#endif

        }


        private TextReader m_reader;
        //private StringReader reader;
        #endregion

        #region ctor/dtor
        /// <param name="filePath"> .csvを除いたファイル名を指定 </param>
        public CSVReader(string filePath, bool skipFirstLine, bool loadFromResources = false, Encoding encoding = null)
        {
            // 既定のエンコードの設定
            encoding = encoding ?? Encoding.GetEncoding("utf-8");
            LoadType(); // Tを解析する
            //! Resources内のファイルをロード
            m_reader = loadFromResources ? CreateTextReaderFromResources(filePath) : CreateTextReaderRaw(filePath);
            if (m_reader == null) {
                Debug.Assert(false, "failure to load file : " + filePath + ".csv");
                return;
            }
            // ヘッダーを飛ばす場合は1行読む
            if (skipFirstLine) { this.m_reader.ReadLine(); }
        }

        public void Dispose() {
            using (m_reader) { }
            m_reader = null;
        }
        #endregion

        #region public
        public IEnumerator<T> GetEnumerator() {
            string line;
            while ((line = m_reader.ReadLine()) != null) {
                if (line.Count() == 0) { continue; }    // 空行は読み飛ばす
                var data = new T();
                string[] fields = line.Split(',');
                // セル数分だけループを回す
                foreach (int columnIndex in Enumerable.Range(0, fields.Length))
                {
                    // 列番号に対応するsetメソッドがない場合は処理しない
                    if (!setters.ContainsKey(columnIndex)) { continue; }
                    // setメソッドでdataに値を入れる
                    if (fields[columnIndex].Contains('/')) {
                        fields[columnIndex].Replace('/', ',');
                    }
                    setters[columnIndex](data, fields[columnIndex]);
                }
                yield return data;
            }
        }
        #endregion

        #region private
        private static TextReader CreateTextReaderFromResources(string filePath) {
            var csv = Resources.Load(filePath) as TextAsset;
            if (csv == null) { return null; }
            else { return new StringReader(csv.text); }
        }
        private static TextReader CreateTextReaderRaw(string filePath) {
            return new StreamReader(filePath + ".csv");
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
        #endregion

    }

    /// <summary>
    /// 変換失敗時のイベント引数クラス
    /// </summary>
    public class ConvertFailedEventArgs : EventArgs {

        public ConvertFailedEventArgs(MemberInfo info, object value, object defaultValue, Exception ex)
        {
            this.MemberInfo = info;
            this.FailedValue = value;
            this.CorrectValue = defaultValue;
            this.Exception = ex;
        }

        /// <summary>
        /// 変換に失敗したメンバーの情報
        /// </summary>
        public MemberInfo MemberInfo { get; private set; }

        /// <summary>
        /// 失敗時の値
        /// </summary>
        public object FailedValue { get; private set; }

        /// <summary>
        /// 正しい値をイベントで受け取る側が設定してください。規定値はCSVColumnAttribute.DefaultValueです。
        /// </summary>
        public object CorrectValue { get; set; }

        /// <summary>
        /// 発生した例外
        /// </summary>
        public Exception Exception { get; private set; }

    }

}
