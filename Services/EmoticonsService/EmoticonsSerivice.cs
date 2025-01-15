
using DataAccess.DataAccess;
using DataAccess.Models;
using Services.EmoticonsService;
using Telegram.Bot.Types.InlineQueryResults;

public class Emoticon{

    static int idCounter = 0;

    public string tag { get; set; }
    public string emoticon { get; set; }

    public int id { get; set; }
    public Emoticon(string tag, string emoticon)
    {
        this.id = idCounter++;
        this.tag = tag;
        this.emoticon = emoticon;
    }
}
class EmoticonsSerivice : IEmoticonsSerivice
{
    IDataAccess _dataAccess;

    public EmoticonsSerivice(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    private List<Emoticon> emoticonList = new List<Emoticon>();


    public async Task<List<EmoticonModel>> GetEmoticonsAsync(string search = "")
    {
        if (search == "")
        {
            return await _dataAccess.GetEmoticons();
        }
        else
        {
            return await _dataAccess.GetEmoticonsByTag(search);
        }
    }

    public async Task AddEmoticon(string inputString)
    {
        await _dataAccess.AddEmoticon(inputString.Split(" ")[0], inputString.Split(" ").Skip(1).ToList());
    }

}