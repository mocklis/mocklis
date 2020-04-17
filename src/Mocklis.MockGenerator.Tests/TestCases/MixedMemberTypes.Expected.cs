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

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : IDuplexConnection
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            DataReceived = new EventMock<EventHandler<DuplexConnectionDataEventArgs>>(this, "TestClass", "IDuplexConnection", "DataReceived", "DataReceived", Strictness.Lenient);
            TextReceived = new EventMock<EventHandler<DuplexConnectionTextEventArgs>>(this, "TestClass", "IDuplexConnection", "TextReceived", "TextReceived", Strictness.Lenient);
            SendData = new ActionMethodMock<byte[]>(this, "TestClass", "IDuplexConnection", "SendData", "SendData", Strictness.Lenient);
            SendText = new ActionMethodMock<string>(this, "TestClass", "IDuplexConnection", "SendText", "SendText", Strictness.Lenient);
            Close = new FuncMethodMock<string, Task<string>>(this, "TestClass", "IDuplexConnection", "Close", "Close", Strictness.Lenient);
            Run = new FuncMethodMock<Task>(this, "TestClass", "IDuplexConnection", "Run", "Run", Strictness.Lenient);
            SubProtocol = new PropertyMock<string>(this, "TestClass", "IDuplexConnection", "SubProtocol", "SubProtocol", Strictness.Lenient);
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
