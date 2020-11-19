//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static GameMain.Editor.DataTableTools.DataTableProcessor;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class StringArrayProcessor : GenericDataProcessor<string[]>
        {
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "string[]";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "string[]",                   
                };
            }

            public override string[] Parse(string value)
            {
                return value.Split(DataArraySeparators, StringSplitOptions.RemoveEmptyEntries);
            }

            public override void WriteToStream(GameMain.Editor.DataTableTools.DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                //数组写之前 写入数组长度                 
                string[] array = Parse(value);
                binaryWriter.Write((Int32)array.Length);
                foreach (var s in array)
                    binaryWriter.Write(s);
            }
        }
    }
}
