using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTService
{
    public class ChatGPTImageContentModel
    {
        public string Prompt { get; set; }
        public byte[] Image { get; set; }
    }
}
