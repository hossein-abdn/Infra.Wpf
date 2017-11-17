﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class BusinessResult<T>
    {
        public T Data { get; set; }

        public Exception Exception;

        public BusinessMessage Message { get; set; }
    }
}
