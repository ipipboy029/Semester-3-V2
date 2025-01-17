using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class SocketService
    {
        private static List<string> messages = new List<string>();

        public static void AddMessage(string message)
        {
            messages.Add(message);
        }
        public static string GetMessage() 
        { 
            if(messages.Count > 0)
            {
                string message = messages.First();
                messages.Remove(message);
                return message;
            }
            return null;
        }
    }
}
