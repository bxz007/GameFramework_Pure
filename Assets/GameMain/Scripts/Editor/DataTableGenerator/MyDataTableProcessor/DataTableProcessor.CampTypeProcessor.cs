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
        private sealed class CampTypeProcessor : GenericDataProcessor<GameMain.CampType>
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
                    return "CampType";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "camptype",
                };
            }

            public override GameMain.CampType Parse(string value)
            {
                return (GameMain.CampType)Enum.Parse(typeof(GameMain.CampType), value);
            }

            public override void WriteToStream(GameMain.Editor.DataTableTools.DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(binaryWriter.BaseStream, Parse(value)); //将对象序列化为内存流中
            }
        }
    }
}
