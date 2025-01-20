using DataAccess.Models;
using Telegram.Bot.Types.InlineQueryResults;

namespace Services.EmoticonsService
{
    public interface IEmoticonsSerivice
    {
        public Task<List<EmoticonModel>> GetEmoticonsAsync(string search = "");
        public Task AddEmoticon(string inputString);
    }
}