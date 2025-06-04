using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ynost.Services
{
    public class NetworkAvailabilityService : IDisposable
    {
        // Сделаем _hostToPing публичным свойством только для чтения
        public string HostToPing { get; }
        private readonly DispatcherTimer _timer;
        private bool _isCurrentlyPingSuccessful; // Переименуем для ясности

        public event Action<bool, string> ConnectionStatusChanged;

        public NetworkAvailabilityService(string hostNameOrAddress, TimeSpan pemeriksaanInterval)
        {
            HostToPing = hostNameOrAddress; // Присваиваем публичному свойству
            _isCurrentlyPingSuccessful = false;

            _timer = new DispatcherTimer
            {
                Interval = pemeriksaanInterval
            };
            _timer.Tick += async (s, e) => await CheckConnectionLoopAsync();
        }

        public async Task<bool> CheckHostConnectionAsync()
        {
            try
            {
                using (var ping = new Ping())
                {
                    PingReply reply = await ping.SendPingAsync(HostToPing, 2000); // Используем свойство
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (PingException ex)
            {
                Console.WriteLine($"[NetworkService] PingException to {HostToPing}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[NetworkService] General Exception pinging {HostToPing}: {ex.Message}");
                return false;
            }
        }

        private async Task CheckConnectionLoopAsync()
        {
            bool newPingStatus = await CheckHostConnectionAsync();
            string message;

            if (newPingStatus != _isCurrentlyPingSuccessful)
            {
                _isCurrentlyPingSuccessful = newPingStatus;
                message = _isCurrentlyPingSuccessful ? $"Пинг к серверу ({HostToPing}) успешен." : $"Пинг к серверу ({HostToPing}) не проходит.";
                ConnectionStatusChanged?.Invoke(_isCurrentlyPingSuccessful, message);
            }
        }

        public void StartMonitoring()
        {
            if (!_timer.IsEnabled)
            {
                Console.WriteLine($"[NetworkService] Starting network monitoring for {HostToPing}.");
                Task.Run(async () => await CheckConnectionLoopAsync());
                _timer.Start();
            }
        }

        public void StopMonitoring()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                Console.WriteLine($"[NetworkService] Stopped network monitoring for {HostToPing}.");
            }
        }

        public void Dispose()
        {
            StopMonitoring();
        }
    }
}