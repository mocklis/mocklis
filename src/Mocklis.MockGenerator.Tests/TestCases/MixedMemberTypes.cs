using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Mocklis.Core;

namespace Test
{
    [Serializable]
    public sealed class DuplexConnectionDataEventArgs : EventArgs
    {
        public byte[] Data { get; }
        public DuplexConnectionDataEventArgs(byte[] data)
        {
            Data = data;
        }
    }

    [Serializable]
    public sealed class DuplexConnectionTextEventArgs : EventArgs
    {
        public string Text { get; }
        public DuplexConnectionTextEventArgs(string text)
        {
            Text = text;
        }
    }

    public interface IDuplexConnection
    {
        event EventHandler<DuplexConnectionDataEventArgs> DataReceived;
        event EventHandler<DuplexConnectionTextEventArgs> TextReceived;
        void SendData(byte[] data);
        void SendText(string text);
        Task<string> Close(string reason);
        Task Run();
        string SubProtocol { get; }
    }

    [MocklisClass]
    public class TestClass : IDuplexConnection
    {
    }
}
