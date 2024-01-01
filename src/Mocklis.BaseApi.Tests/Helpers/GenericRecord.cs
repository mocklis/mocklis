// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericRecord.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using System;

    #endregion

    public readonly struct GenericRecord<TData>
    {
        public object? Instance { get; }
        public bool IsSuccess { get; }
        public TData Data { get; }
        public Exception? Exception { get; }

        private GenericRecord(object? instance, TData data)
        {
            Instance = instance;
            IsSuccess = true;
            Data = data;
            Exception = null;
        }

        private GenericRecord(object? instance, Exception exception)
        {
            Instance = instance;
            IsSuccess = false;
            Data = default!;
            Exception = exception;
        }

        public static GenericRecord<TData> One(TData data)
            => new GenericRecord<TData>(null, data);

        public static GenericRecord<TData> One(object instance, TData data)
            => new GenericRecord<TData>(instance, data);

        public static GenericRecord<TData> Ex(Exception exception)
            => new GenericRecord<TData>(null, exception);

        public static GenericRecord<TData> Ex(object instance, Exception exception)
            => new GenericRecord<TData>(instance, exception);
    }

    public readonly struct GenericRecord<TData1, TData2>
    {
        public object? Instance { get; }
        public bool IsSuccess { get; }
        public TData1 Data1 { get; }
        public TData2 Data2 { get; }
        public Exception? Exception { get; }

        private GenericRecord(object? instance, TData1 data1, TData2 data2)
        {
            Instance = instance;
            IsSuccess = true;
            Data1 = data1;
            Data2 = data2;
            Exception = null;
        }

        private GenericRecord(object? instance, TData1 data1, Exception exception)
        {
            Instance = instance;
            IsSuccess = false;
            Data1 = data1;
            Data2 = default!;
            Exception = exception;
        }

        public static GenericRecord<TData1, TData2> One(TData1 data1)
            => new GenericRecord<TData1, TData2>(null, data1, default(TData2)!);

        public static GenericRecord<TData1, TData2> One(object instance, TData1 data1)
            => new GenericRecord<TData1, TData2>(instance, data1, default(TData2)!);

        public static GenericRecord<TData1, TData2> Two(TData1 data1, TData2 data2)
            => new GenericRecord<TData1, TData2>(null, data1, data2);

        public static GenericRecord<TData1, TData2> Two(object instance, TData1 data1, TData2 data2)
            => new GenericRecord<TData1, TData2>(instance, data1, data2);

        public static GenericRecord<TData1, TData2> Ex(TData1 data1, Exception exception)
            => new GenericRecord<TData1, TData2>(null, data1, exception);

        public static GenericRecord<TData1, TData2> Ex(object instance, TData1 data1, Exception exception)
            => new GenericRecord<TData1, TData2>(instance, data1, exception);
    }
}
