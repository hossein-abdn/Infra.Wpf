﻿using System;

namespace Infra.Wpf.Business
{
    public class BusinessResult<T> : BusinessResult
    {
        public T Data { get; set; }
    }

    public class BusinessResult
    {
        public Exception Exception { get; set; }

        public BusinessMessage Message { get; set; }

        public bool IsOnBeforExecute { get; set; }

        public bool IsOnExecute { get; set; }

        public bool IsOnAfterExecute { get; set; }

        public bool HasException
        {
            get
            {
                return Exception != null;
            }
        }
    }
}
