using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

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
                var cancellationTokenSource = new CancellationTokenSource();

                var me = await botClient.GetMe();
                Console.WriteLine($"{me.FirstName} запущен!");

                await Task.Delay(10000);
                CloseApp(me, cancellationTokenSource);
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

        private static void CloseApp(User user, CancellationTokenSource cancellation)
        {
            Console.WriteLine("Нажмите клавишу A для выхода");
            var input = Console.ReadKey().KeyChar;
            if (input == 'A')
            {
                cancellation.Cancel();
            }
            else
            {
                Console.WriteLine($"{user.Id}");
            }
        }
    }
}
