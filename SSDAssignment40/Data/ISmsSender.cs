using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public interface ISmsSender
    {
        bool SendSms(string number, string message);
    }
}
