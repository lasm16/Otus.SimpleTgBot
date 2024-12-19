using System.Net.Http.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

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
    }
}
