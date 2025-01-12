using Telegram.Bot.Types.InlineQueryResults;

namespace Services.EmoticonsService
{
    public interface IEmoticonsSerivice
    {
        public List<Emoticon> GetEmoticons(string search = "");
    }
}