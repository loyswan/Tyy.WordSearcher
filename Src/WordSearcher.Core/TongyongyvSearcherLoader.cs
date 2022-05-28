using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tyy.WordSearcher.Core.Dictionary;
using Tyy.WordSearcher.Core.Models;

namespace Tyy.WordSearcher.Core
{
    public class TongyongyvSearcherLoader
    {
        private WordDictionary _wordDictionary = new WordDictionary();
        private CharWordDictionary _searchDictionary = new CharWordDictionary();
        private CharacterDictionary _characterDictionary = new CharacterDictionary();

        public WordDictionary WordDictionary => _wordDictionary;
        public CharWordDictionary SearchDictionary => _searchDictionary;
        public CharacterDictionary CharacterDictionary => _characterDictionary;

        public void LoadFromTxt()
        {
            //加载词汇
            try
            {
                using (var sr = new StreamReader(@"Data\TongyongyvWords.txt", Encoding.UTF8))
                {

                    _ = sr.ReadLine();//丢弃标题行
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var items = line.Trim().Split('\t');
                        if (items.Length != 3)
                        {
                            continue;
                        }
                        var tw = _wordDictionary.Add(items[0], items[1], items[2]);
                        _searchDictionary.Add(tw);
                        //Console.WriteLine(tw);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            //加载单字
            try
            {
                using (var sr = new StreamReader(@"Data\TongyongyvCharacters.txt", Encoding.UTF8))
                {
                    _ = sr.ReadLine();//丢弃标题行
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //T拼音 汉字 类别 意符 通用语 释义
                        var items = line.Trim().Split('\t');
                        if (items.Length < 5)
                        {
                            continue;
                        }
                        var tongyongyvCharacter = items.Length == 6
                            ? new TongyongyvCharacter()
                            {
                                Character = new KeyCharacter() { Chinese = items[1][0], TPinyin = items[0] },
                                Type = items[2],
                                Flag = items[3],
                                Tongyongyv = items[4],
                                Description = items[5]
                            }
                            : new TongyongyvCharacter()
                            {
                                Character = new KeyCharacter() { Chinese = items[1][0], TPinyin = items[0] },
                                Type = items[2],
                                Flag = items[3],
                                Tongyongyv = items[4],
                                Description = ""
                             
                            };


                        _characterDictionary.Add(tongyongyvCharacter);
                        //Console.WriteLine(tongyongyvCharacter);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

        }

        public bool LoadFromBin()
        {
            try
            {
                using (var binaryReader = new BinaryReader(new FileStream(BinFile, FileMode.Open, FileAccess.Read)))
                {
                    string fileFlag = binaryReader.ReadString();//读取文件类型
                    if (fileFlag != "TyyWord") { return false; }
                    //加载 WordDictionary
                    if (!ReadWordDictionayBin(binaryReader)) { return false; }
                    //加载XX
                    if (!ReadCharWordDictionaryBin(binaryReader)) { return false; }
                    //加载XX
                    if (!ReadCharacterDictionaryBin(binaryReader)) { return false; }
                    //验证文件尾
                    string fileEndFlag = binaryReader.ReadString();
                    if (fileEndFlag != "TyyWord") { return false; }
                    //加载完成
                    return true;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public void WriteToBin()
        {
            //创建二进制文件
            FileStream stream = new FileStream(BinFile, FileMode.Create, FileAccess.Write);
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write("TyyWord");//写入文件类型
            //共3个字典
            //写入第一个字典 WordDictionary
            WriteWordDictionayBin(binaryWriter);

            //写入第二个字典
            WriteCharWordDictionaryBin(binaryWriter);

            //写入第三个字典
            WriteCharacterDictionaryBin(binaryWriter);

            //写入完成
            binaryWriter.Write("TyyWord"); //写入验证标志

            //清除缓冲区的内容，将缓冲区中的内容写入到文件中
            binaryWriter.Flush();

            //关闭二进制流
            binaryWriter.Close();
            stream.Close();
        }

        private void WriteWordDictionayBin(BinaryWriter binaryWriter)
        {
            binaryWriter.Write("WordDictionary");//写入字典类型
            binaryWriter.Write(this._wordDictionary.Count);//写入字典长度

            foreach (var kv in this._wordDictionary) //循环写入字典数据
            {
                var tw = kv.Value;
                binaryWriter.Write((byte)(tw.Characters.Count * 2 + 3));//写入字符串数量
                binaryWriter.Write(kv.Key);//写入词汇 与tw.Word 相同，所以只写入一次
                binaryWriter.Write(tw.TPinyin);
                binaryWriter.Write(tw.Tongyongyv);
                foreach (var item in tw.Characters)
                {
                    binaryWriter.Write(item.Chinese.ToString());
                    binaryWriter.Write(item.TPinyin);
                }
            }
            binaryWriter.Write("WordDictionary"); //写入验证标志
        }

        private bool ReadWordDictionayBin(BinaryReader binaryReader)
        {
            //加载 WordDictionary
            string dictFlag = binaryReader.ReadString();
            if (dictFlag != "WordDictionary") { return false; }
            int dictlength = binaryReader.ReadInt32();
            for (int i = 0; i < dictlength; i++)
            {
                byte datacount = binaryReader.ReadByte();
                string[] data = new string[datacount];
                for (byte n = 0; n < datacount; n++)
                {
                    data[n] = binaryReader.ReadString();
                }
                TongyongyvWord tw = new TongyongyvWord(data);
                _wordDictionary.Add(tw.Word, tw);
            }
            //验证 WordDictionary 字典尾
            string dictEndFlag = binaryReader.ReadString();
            return dictEndFlag == "WordDictionary";
        }



        private void WriteCharWordDictionaryBin(BinaryWriter binaryWriter)
        {
            binaryWriter.Write("CharWordDictionary");//写入字典类型

            //写入 WordList
            binaryWriter.Write(this._searchDictionary.wordList.Count); //写入WordList 长度
            foreach (var item in this._searchDictionary.wordList)
            {
                binaryWriter.Write(item);
            }

            //写入 words
            binaryWriter.Write(this._searchDictionary.words.Count);//写入字典长度
            foreach (var kv in this._searchDictionary.words) //循环写入字典数据
            {
                //key
                binaryWriter.Write(kv.Key.Chinese.ToString());
                binaryWriter.Write(kv.Key.TPinyin.ToString());
                //value  HashSet<TongyongyvWord>
                var tws = kv.Value;
                binaryWriter.Write(tws.Count);//写入词汇数量
                foreach (var tw in tws)
                {
                    binaryWriter.Write((byte)(tw.Characters.Count * 2 + 3));//写入字符串数量
                    binaryWriter.Write(tw.Word);//写入词汇
                    binaryWriter.Write(tw.TPinyin);
                    binaryWriter.Write(tw.Tongyongyv);
                    foreach (var item in tw.Characters)
                    {
                        binaryWriter.Write(item.Chinese.ToString());
                        binaryWriter.Write(item.TPinyin);
                    }
                }
            }
            binaryWriter.Write("CharWordDictionary"); //写入验证标志
        }

        private bool ReadCharWordDictionaryBin(BinaryReader binaryReader)
        {
            //加载 CharWordDictionary
            string dictFlag = binaryReader.ReadString();
            if (dictFlag != "CharWordDictionary") { return false; }

            //WordList
            int wordListlength = binaryReader.ReadInt32();
            for (int i = 0; i < wordListlength; i++)
            {
                string wd = binaryReader.ReadString();
                this._searchDictionary.wordList.Add(wd);
            }

            //words
            int wordslength = binaryReader.ReadInt32();//字典长度
            for (int i = 0; i < wordslength; i++)
            {
                //key
                var keychar = new KeyCharacter()
                {
                    Chinese = binaryReader.ReadString()[0],
                    TPinyin = binaryReader.ReadString()
                };

                //value  HashSet<TongyongyvWord>
                var twcount = binaryReader.ReadInt32();//词汇数量
                var hashset = new HashSet<TongyongyvWord>();
                for (int c = 0; c < twcount; c++)
                {
                    byte datacount = binaryReader.ReadByte();//字符串数量
                    string[] data = new string[datacount];
                    for (byte n = 0; n < datacount; n++)
                    {
                        data[n] = binaryReader.ReadString();
                    }
                    TongyongyvWord tw = new TongyongyvWord(data);
                    hashset.Add(tw);
                }

                this._searchDictionary.words.Add(keychar, hashset);
            }

            //验证 CharWordDictionary 字典尾
            string dictEndFlag = binaryReader.ReadString();
            return dictEndFlag == "CharWordDictionary";
        }

        private void WriteCharacterDictionaryBin(BinaryWriter binaryWriter)
        {
            binaryWriter.Write("CharacterDictionary");//写入字典类型
            binaryWriter.Write(this._characterDictionary.Count);//写入字典长度

            foreach (var kv in this._characterDictionary._tongyongyvCharacters) //循环写入字典数据
            {
                //key 
                binaryWriter.Write(kv.Key.Chinese.ToString());
                binaryWriter.Write(kv.Key.TPinyin.ToString());
                //value  List<TongyongyvCharacter>
                var tcs = kv.Value;
                binaryWriter.Write((byte)(tcs.Count));//写入单字数量
                foreach (var tc in tcs)
                {
                    binaryWriter.Write(tc.Type);//类别
                    binaryWriter.Write(tc.Flag);//意符
                    binaryWriter.Write(tc.Tongyongyv);//通用语
                    binaryWriter.Write(tc.Description);//释义
                }
            }
            binaryWriter.Write("CharacterDictionary"); //写入验证标志
        }

        private bool ReadCharacterDictionaryBin(BinaryReader binaryReader)
        {
            //加载 CharacterDictionary
            string dictFlag = binaryReader.ReadString();
            if (dictFlag != "CharacterDictionary") { return false; }

            int dictlength = binaryReader.ReadInt32();
            for (int i = 0; i < dictlength; i++)
            {
                //key
                var keychar = new KeyCharacter
                {
                    Chinese = binaryReader.ReadString()[0],
                    TPinyin = binaryReader.ReadString()
                };
                //value  List<TongyongyvCharacter>
                byte tccount = binaryReader.ReadByte();
                var tcs = new List<TongyongyvCharacter>(tccount);

                for (byte n = 0; n < tccount; n++)
                {
                    var tc = new TongyongyvCharacter
                    {
                        Character = keychar,
                        Type = binaryReader.ReadString(),
                        Flag = binaryReader.ReadString(),
                        Tongyongyv = binaryReader.ReadString(),
                        Description = binaryReader.ReadString()
                    };
                    tcs.Add(tc);
                }

                this._characterDictionary._tongyongyvCharacters.Add(keychar, tcs);
            }

            //验证 CharacterDictionary 字典尾
            string dictEndFlag = binaryReader.ReadString();
            return dictEndFlag == "CharacterDictionary";
        }

        //判断是否存在二进制文件
        private bool HasBin()
        {
            return File.Exists(BinFile);
        }

        //默认优先从二进制文件加载
        public void Load(bool fromtxt = false)
        {
            if (!fromtxt && HasBin())
            {
                if (LoadFromBin())
                    return;
            }
            ClearDictionary();
            LoadFromTxt();
            WriteToBin();//生成新的二进制文件
        }

        private void ClearDictionary()
        {
            this._wordDictionary.Clear();
            this._characterDictionary.Clear();
            this._searchDictionary.Clear();
        }

        private readonly string BinFile = @".\Data\TongyongyvSearcherData.bin";
    }
}
