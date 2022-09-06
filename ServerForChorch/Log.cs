using System;

namespace ServerForChorch
{
    class Log
    {
        private DateTime time;
        private string message;
        private string address;
        public Log(string address, string message)
        {
            this.address = address.Split(':')[0];
            this.message = message;
            time = DateTime.Now;
        }
        public string Time => time.ToLongTimeString();
        public string Message => message;
        public string Address => address;
    }
}
