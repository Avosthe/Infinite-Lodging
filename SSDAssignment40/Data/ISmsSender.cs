using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
