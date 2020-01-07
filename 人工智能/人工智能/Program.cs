using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace 人工智能
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            
                var keyString = "人工智能";
                TestStreamReaderEnumerable(keyString);
                Console.ReadKey();
            }


            /// <summary>
            /// 使用迭代子方式检索指定的文本文件
            /// </summary>
            /// <param name="keyString"></param>
            public static void TestStreamReaderEnumerable(string keyString)
            {
                IEnumerable<String> stringsFound;
                // 使用 StreamReaderEnumerable 打开目标文件,并装入一个IEnumerable<String>类型变量中
                try
                {
                    stringsFound =
                          from line in new StreamReaderEnumerable("temp/tempFile.txt")
                          select line;


                    Console.WriteLine("文件名：tempFile1.txt");
                    Console.WriteLine();
                    Console.WriteLine("关键字：{0}", keyString);
                    Console.WriteLine();

                    //定义一个记录遍历数的变量，一行一行的遍历文本。第X行，X即遍历了第几次
                    int i = 1;
                    foreach (var w in stringsFound)
                    {

                        if (w.Contains(keyString))//判断行文本中是否含有“人工智能”，stringsFound中包含了tempFile.txt的所有内容
                        {
                            int x = w.IndexOf(keyString, StringComparison.Ordinal);//利用IndexOf属性得到“人工智能“在该行字符串中的索引值
                            int j = w.Length - (x + 1);//计算从“人工智能”开始直到该行文本的结尾的字符数
                            if (j > 13)
                                Console.WriteLine("第{0}行，第{1}个字母开始：{2}...；", i, x + 1, w.Substring(x, 13));
                            if (j <= 13)
                                Console.WriteLine("第{0}行，第{1}个字母开始：{2}；", i, x + 1, w.Substring(x));
                        }

                        i++;
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine(@"这个例子需要该项目下一个名为\bin\Debug\temp\tempFile.txt 的文件。");
                    return;
                }
            }


        }

        /// <summary>
        /// 一个定制的实现IEnumerable<T> 的类，为此，还需要实现相应的
        ///  IEnumerable 和 IEnumerator<T>
        /// </summary>
        public class StreamReaderEnumerable : IEnumerable<string>
        {
            private string _filePath;
            public StreamReaderEnumerable(string filePath)
            {
                _filePath = filePath;
            }

            // 必须实现 GetEnumerator，用于返回一个新的 StreamReaderEnumerator.
            public IEnumerator<string> GetEnumerator() => new StreamReaderEnumerator(_filePath);

            // 同时必须实现 IEnumerable.GetEnumerator，但当成一个私有方法实现
            private IEnumerator GetEnumerator1() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator1();

            /// <summary>
            /// 在实现 IEnumerable<T> 时，必须实现IEnumerator<T>，范例代码中，遍历文件内容时，每一行一次
            /// 实现 IEnumerable<T> 还需要实现 IEnumerator 和析构函数 IDisposable
            /// </summary>
            public class StreamReaderEnumerator : IEnumerator<string>
            {
                private StreamReader _sr;
                public StreamReaderEnumerator(string filePath)
                {
                    _sr = new StreamReader(filePath);
                }
                private string _current;
                // 实现 IEnumerator<T>().Current 公开属性，但实现所必须的 IEnumerator.Current 则为私有属性.
                public string Current
                {
                    get
                    {
                        if (_sr == null || _current == null)
                            throw new InvalidOperationException();
                        return _current;
                    }
                }
                private object Current1 => this.Current;
                object IEnumerator.Current => Current1;
                // 实现 IEnumerator 所必须的 MoveNext 和 Reset。
                public bool MoveNext()
                {
                    _current = _sr.ReadLine();
                    if (_current == null)
                        return false;
                    return true;
                }
                public void Reset()
                {
                    _sr.DiscardBufferedData();
                    _sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    _current = null;
                }
                // 实现析构函数，必须的。
                private bool disposedValue = false;
                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
                protected virtual void Dispose(bool disposing)
                {
                    if (!this.disposedValue)
                    {
                        if (disposing) { } // 析构所需要的资源
                        _current = null;
                        if (_sr != null)
                        {
                            _sr.Close();
                            _sr.Dispose();
                        }
                    }
                    this.disposedValue = true;
                }
                ~StreamReaderEnumerator() { Dispose(false); }
            }
        }
    }
