// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        public global::Mocklis.Core.EventMock<global::System.EventHandler<global::Test.DuplexConnectionDataEventArgs>> DataReceived { get; }

        event global::System.EventHandler<global::Test.DuplexConnectionDataEventArgs> global::Test.IDuplexConnection.DataReceived { add => DataReceived.Add(value); remove => DataReceived.Remove(value); }

        public global::Mocklis.Core.EventMock<global::System.EventHandler<global::Test.DuplexConnectionTextEventArgs>> TextReceived { get; }

        event global::System.EventHandler<global::Test.DuplexConnectionTextEventArgs> global::Test.IDuplexConnection.TextReceived { add => TextReceived.Add(value); remove => TextReceived.Remove(value); }

        public global::Mocklis.Core.ActionMethodMock<byte[]> SendData { get; }

        void global::Test.IDuplexConnection.SendData(byte[] data) => SendData.Call(data);

        public global::Mocklis.Core.ActionMethodMock<string> SendText { get; }

        void global::Test.IDuplexConnection.SendText(string text) => SendText.Call(text);

        public global::Mocklis.Core.FuncMethodMock<string, global::System.Threading.Tasks.Task<string>> Close { get; }

        global::System.Threading.Tasks.Task<string> global::Test.IDuplexConnection.Close(string reason) => Close.Call(reason);

        public global::Mocklis.Core.FuncMethodMock<global::System.Threading.Tasks.Task> Run { get; }

        global::System.Threading.Tasks.Task global::Test.IDuplexConnection.Run() => Run.Call();

        public global::Mocklis.Core.PropertyMock<string> SubProtocol { get; }

        string global::Test.IDuplexConnection.SubProtocol => SubProtocol.Value;

        public TestClass() : base()
        {
            this.DataReceived = new global::Mocklis.Core.EventMock<global::System.EventHandler<global::Test.DuplexConnectionDataEventArgs>>(this, "TestClass", "IDuplexConnection", "DataReceived", "DataReceived", global::Mocklis.Core.Strictness.Lenient);
            this.TextReceived = new global::Mocklis.Core.EventMock<global::System.EventHandler<global::Test.DuplexConnectionTextEventArgs>>(this, "TestClass", "IDuplexConnection", "TextReceived", "TextReceived", global::Mocklis.Core.Strictness.Lenient);
            this.SendData = new global::Mocklis.Core.ActionMethodMock<byte[]>(this, "TestClass", "IDuplexConnection", "SendData", "SendData", global::Mocklis.Core.Strictness.Lenient);
            this.SendText = new global::Mocklis.Core.ActionMethodMock<string>(this, "TestClass", "IDuplexConnection", "SendText", "SendText", global::Mocklis.Core.Strictness.Lenient);
            this.Close = new global::Mocklis.Core.FuncMethodMock<string, global::System.Threading.Tasks.Task<string>>(this, "TestClass", "IDuplexConnection", "Close", "Close", global::Mocklis.Core.Strictness.Lenient);
            this.Run = new global::Mocklis.Core.FuncMethodMock<global::System.Threading.Tasks.Task>(this, "TestClass", "IDuplexConnection", "Run", "Run", global::Mocklis.Core.Strictness.Lenient);
            this.SubProtocol = new global::Mocklis.Core.PropertyMock<string>(this, "TestClass", "IDuplexConnection", "SubProtocol", "SubProtocol", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
