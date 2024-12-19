using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;

namespace Otus.SimpleTgBot
{
    internal class UpdateHandler : IUpdateHandler
    {
        public delegate void MessageHandler(string message);
        public event MessageHandler? OnHandleUpdateStarted;
        public event MessageHandler? OnHandleUpdateCompleted;
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            await Task.Run(() => Console.WriteLine($"Словил ошибку: {exception.Message}"), cancellationToken);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message == null)
            {
                return;
            }
            //if (update.Message.Text == "/stop")
            //{
            //    CloseApp(botClient);
            //}
            OnHandleUpdateStarted?.Invoke(update.Message.Text);
            if (update.Message.Text == "/cat")
            {
                using var client = new HttpClient();
                var catFactDto = await client.GetFromJsonAsync<CatFactDto>("https://catfact.ninja/fact", cancellationToken);
                var text = $"Факт о кошках: {catFactDto.Fact}";
                await SendMessege(botClient, update, text, cancellationToken);
            }
            else
            {
                var text = "Сообщение успешно принято";
                await SendMessege(botClient, update, text, cancellationToken);
            }
            OnHandleUpdateCompleted?.Invoke(update.Message.Text);
        }

        private static async Task SendMessege(ITelegramBotClient botClient, Update update, string text, CancellationToken cancellationToken)
        {
            await botClient.SendMessage(update.Message.Chat, text, cancellationToken: cancellationToken);
        }

        private void CloseApp(ITelegramBotClient botClient)
        {
            Console.WriteLine("Нажмите клавишу A для выхода");
            var input = Console.ReadLine();
            if (input == "A")
            {
                //await Task.(cancellationToken);
            }
            else
            {
                var user = botClient.GetMe();
                Console.WriteLine($"{user.Id}");
            }
        }
    }
}
