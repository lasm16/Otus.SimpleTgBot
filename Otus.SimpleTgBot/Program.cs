using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;

namespace Otus.SimpleTgBot
{
    internal class Program
    {
        private static string _token = "";
        static async Task Main(string[] args)
        {

            var botClient = new TelegramBotClient(_token);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message],
                DropPendingUpdates = true
            };
            var handler = new UpdateHandler();
            handler.OnHandleUpdateStarted += OnHandleUpdateStarted;
            handler.OnHandleUpdateCompleted += OnHandleUpdateCompleted;
            try
            {
                botClient.StartReceiving(handler, receiverOptions);
                var cancellationToken = new CancellationTokenSource();

                var me = await botClient.GetMe();
                Console.WriteLine($"{me.FirstName} запущен!");
                await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
            }
            finally
            {
                handler.OnHandleUpdateCompleted -= OnHandleUpdateCompleted;
                handler.OnHandleUpdateStarted -= OnHandleUpdateStarted;
            }
        }

        private static void OnHandleUpdateStarted(string message)
        {
            Console.WriteLine($"Началась обработка сообщения {message}");
        }

        private static void OnHandleUpdateCompleted(string message)
        {
            Console.WriteLine($"Закончилась  обработка сообщения {message}");
        }
    }
}
