using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface ISmsService
    {
        bool SendSmsToNumbers(List<string> numbers, string message);
        bool SendSmsToWatsApp(string number, string message);
    }
}
