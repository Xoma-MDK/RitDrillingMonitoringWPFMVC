using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using GMap.NET;
using WebSocketSharp;
using WebSocket = WebSocketSharp.WebSocket;
using RitDrillingMonitoringWPFMVC.Models;
using RitDrillingMonitoringWPFMVC.Views;

namespace RitDrillingMonitoringWPFMVC.Services
{
    internal class API
    {
        private readonly Action<Object> _action;
        private readonly WebSocket _webSocket;
        public API(Action<object> action)
        {
            _webSocket = new("ws://109.174.29.40:6686/ws/2");
            _webSocket.OnMessage += WebSocketOnMessage!;
            _action = action;
        }

        public void Init()
        {
            _webSocket.Connect();
            //_webSocket.Send("Hello server, i am client");
        }
        public void WebSocketOnMessage(object viewer, MessageEventArgs a)
        {
            try
            {
                var id  =Int32.Parse( a.Data.Split(',')[0]);
                var Latitude = float.Parse(a.Data.Split(",")[2].Replace('.', ','));
                var Longitude = float.Parse(a.Data.Split(",")[3].Replace('.', ','));
                using var db = new RitnavSystemForDrillMachinesContext();
                var drillMachine = db.DrillMachines.FirstOrDefault(d => d.IddrillingMachine == id);
                drillMachine.Latitude = Latitude;
                drillMachine.Longitude = Longitude;
                _action(drillMachine);

            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}
