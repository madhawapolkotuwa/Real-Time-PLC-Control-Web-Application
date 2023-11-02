using Microsoft.AspNetCore.SignalR;
using PLC.WebApp.Hubs;
using PLC.WebApp.Models;
using PLC.WebApp.Models.Dtos;
using SLMP;
using SLMP.SlmpClient;
using System.Net.Sockets;

namespace PLC.WebApp.Services
{
    public class SLMPConnection
    {
        public static string SLMP_ADDR = "192.168.3.39";
        public static int SLMP_PORT = 1026;

        private SlmpConfig _slmpConfig { get; set; }
        private SlmpClient _slmpClient { get; set; }

        public CancellationTokenSource cancellationTokenSource;
        public CancellationToken cancellationToken;
        public Task workerTask;

        private readonly IHubContext<ConnectionHub> _hubContext;

        public SLMPConnection(IHubContext<ConnectionHub> hubContext) 
        {
            _hubContext = hubContext;
        }

        public void CreateSlmpConnection(IpAddress ipAddress)
        {
            SLMP_ADDR = ipAddress.Ipaddress;
            SLMP_PORT = int.Parse(ipAddress.Port);
            _slmpConfig = new SlmpConfig(SLMP_ADDR, SLMP_PORT);
            _slmpClient = new SlmpClient(_slmpConfig);

            _slmpClient.Connect();

            if (!_slmpClient.InternalIsConnected())
                throw new Exception("Can't connect to PLC: Connction Error");
        }

        public async void Disconnect()
        {
            CloseTask();
            _slmpClient?.Disconnect();
        }

        public bool IsConnected()
        {
            return _slmpClient.InternalIsConnected();
        }

        public void OpenTask(CancellationTokenSource cTSource, CancellationToken cToken)
        {
            cancellationTokenSource = cTSource;
            cancellationToken = cToken;
            workerTask = Task.Run(() => recvTaskMain(cancellationToken));
        }

        public void recvTaskMain(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                counterRead();
            }
        }

        public async void counterRead()
        {
            ushort result = _slmpClient.ReadWordDevice(Device.CN, 0);
            await _hubContext.Clients.Group("PlcHub").SendAsync("CounterValue", result);
            Thread.Sleep(100);
        }

        public async void CloseTask()
        {
            cancellationTokenSource.Cancel();
            Thread.Sleep(100);
            try
            {
                await workerTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Worker task was canceled.");
            }
        }

        public void CounterReset()
        {
            _slmpClient.WriteWordDevice("CN0", 0);
        }

        public void Xwrite(xDto xDto)
        {
            bool[] data = xDto.BooleanData.ToArray();
            _slmpClient.WriteBitDevice("X0", data);
        }

        public void Dwrite(dDto dDto)
        {
            ushort[] data = dDto.ushortData.ToArray();
            _slmpClient.WriteWordDevice("D100", data);
        }
    }
}
