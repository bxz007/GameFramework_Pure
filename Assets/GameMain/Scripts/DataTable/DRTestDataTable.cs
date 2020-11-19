//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：__DATA_TABLE_CREATE_TIME__
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// table 配置数据测试。
    /// </summary>
    public class DRTestDataTable : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取整形。
        /// </summary>
        public int TestInt
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取Bool。
        /// </summary>
        public bool TestBool
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取浮点。
        /// </summary>
        public float TestFloat
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取Vector3。
        /// </summary>
        public Vector3 TestVector3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取整形数组。
        /// </summary>
        public int[] TestIntArray
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取浮点型数组。
        /// </summary>
        public float[] TestFloatArray
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取字符数组。
        /// </summary>
        public string[] TestStringArray
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取枚举。
        /// </summary>
        public CampType TestEnum
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            TestInt = int.Parse(columnStrings[index++]);
            TestBool = bool.Parse(columnStrings[index++]);
            TestFloat = float.Parse(columnStrings[index++]);
            TestVector3 = DataTableExtension.ParseVector3(columnStrings[index++]);
            TestIntArray = DataTableExtension.ParseInt32Array(columnStrings[index++]);
            TestFloatArray = DataTableExtension.ParseSingleArray(columnStrings[index++]);
            TestStringArray = DataTableExtension.ParseStringArray(columnStrings[index++]);
            TestEnum = DataTableExtension.ParseCampType(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    TestInt = binaryReader.Read7BitEncodedInt32();
                    TestBool = binaryReader.ReadBoolean();
                    TestFloat = binaryReader.ReadSingle();
                    TestVector3 = binaryReader.ReadVector3();
                    TestIntArray = binaryReader.ReadInt32Array();
                    TestFloatArray = binaryReader.ReadSingleArray();
                    TestStringArray = binaryReader.ReadStringArray();
                    TestEnum = binaryReader.ReadCampType();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, Vector3>[] m_TestVector = null;

        public int TestVectorCount
        {
            get
            {
                return m_TestVector.Length;
            }
        }

        public Vector3 GetTestVector(int id)
        {
            foreach (KeyValuePair<int, Vector3> i in m_TestVector)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetTestVector with invalid id '{0}'.", id.ToString()));
        }

        public Vector3 GetTestVectorAt(int index)
        {
            if (index < 0 || index >= m_TestVector.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetTestVectorAt with invalid index '{0}'.", index.ToString()));
            }

            return m_TestVector[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_TestVector = new KeyValuePair<int, Vector3>[]
            {
                new KeyValuePair<int, Vector3>(3, TestVector3),
            };
        }
    }
}
