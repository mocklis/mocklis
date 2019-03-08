using System;
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
        public TestClass()
        {
            DataReceived = new EventMock<EventHandler<DuplexConnectionDataEventArgs>>(this, "TestClass", "IDuplexConnection", "DataReceived", "DataReceived");
            TextReceived = new EventMock<EventHandler<DuplexConnectionTextEventArgs>>(this, "TestClass", "IDuplexConnection", "TextReceived", "TextReceived");
            SendData = new ActionMethodMock<byte[]>(this, "TestClass", "IDuplexConnection", "SendData", "SendData");
            SendText = new ActionMethodMock<string>(this, "TestClass", "IDuplexConnection", "SendText", "SendText");
            Close = new FuncMethodMock<string, Task<string>>(this, "TestClass", "IDuplexConnection", "Close", "Close");
            Run = new FuncMethodMock<Task>(this, "TestClass", "IDuplexConnection", "Run", "Run");
            SubProtocol = new PropertyMock<string>(this, "TestClass", "IDuplexConnection", "SubProtocol", "SubProtocol");
        }

        public EventMock<EventHandler<DuplexConnectionDataEventArgs>> DataReceived { get; }

        event EventHandler<DuplexConnectionDataEventArgs> IDuplexConnection.DataReceived { add => DataReceived.Add(value); remove => DataReceived.Remove(value); }

        public EventMock<EventHandler<DuplexConnectionTextEventArgs>> TextReceived { get; }

        event EventHandler<DuplexConnectionTextEventArgs> IDuplexConnection.TextReceived { add => TextReceived.Add(value); remove => TextReceived.Remove(value); }

        public ActionMethodMock<byte[]> SendData { get; }

        void IDuplexConnection.SendData(byte[] data) => SendData.Call(data);

        public ActionMethodMock<string> SendText { get; }

        void IDuplexConnection.SendText(string text) => SendText.Call(text);

        public FuncMethodMock<string, Task<string>> Close { get; }

        Task<string> IDuplexConnection.Close(string reason) => Close.Call(reason);

        public FuncMethodMock<Task> Run { get; }

        Task IDuplexConnection.Run() => Run.Call();

        public PropertyMock<string> SubProtocol { get; }

        string IDuplexConnection.SubProtocol => SubProtocol.Value;
    }
}
