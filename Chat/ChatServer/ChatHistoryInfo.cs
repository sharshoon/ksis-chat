using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class ChatHistoryInfo
    {
        public string ID { get; set; }
        public List<string> messages = new List<string>();
    }
}
