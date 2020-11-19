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
        private sealed class Int32ArrayProcessor : GenericDataProcessor<int[]>
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
                    return "int[]";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "int[]",
                    "int32[]",
                };
            }

            public override int[] Parse(string value)
            {
                string[] ses = value.Split(DataArraySeparators, StringSplitOptions.RemoveEmptyEntries);
                int[] array = new int[ses.Length];
                for (int i = 0; i < ses.Length; i++)
                {
                    array[i] = int.Parse(ses[i]);
                }
                return array;
            }
          

            public override void WriteToStream(GameMain.Editor.DataTableTools.DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                //数组写之前 写入数组长度         
                int[] array = Parse(value);
                binaryWriter.Write((Int32)array.Length);
                foreach (var i in array)
                    binaryWriter.Write(i);
            }
        }
    }
}
