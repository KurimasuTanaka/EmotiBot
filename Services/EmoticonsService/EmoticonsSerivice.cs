
using DataAccess.DataAccess;
using DataAccess.Models;
using Microsoft.Extensions.Logging;
using Services.EmoticonsService;
using Telegram.Bot.Types.InlineQueryResults;

class EmoticonsSerivice : IEmoticonsSerivice
{
    IDataAccess _dataAccess;
    ILogger<EmoticonsSerivice> _logger;

    public EmoticonsSerivice(ILogger<EmoticonsSerivice> logger, IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        _logger = logger;
    }

    //Get all emoticons or emoticons by tag
    public async Task<List<EmoticonModel>> GetEmoticonsAsync(string search = "")
    {
        if (String.IsNullOrEmpty(search))
        {
            _logger.LogInformation("Getting all emoticons");
            return await _dataAccess.GetEmoticons();
        }
        else
        {
            _logger.LogInformation("Getting emoticons by tag: {tag}", search);
            return await _dataAccess.GetEmoticonsByTag(search);
        }
    }

    //Add new emoticon where emoticon itself is separated from tags by [emo] tag from both sides
    public async Task AddEmoticon(string inputString)
    {
        string emoticon = inputString.Substring(5, inputString.IndexOf("[emo]", 5) - 5);
        if (String.IsNullOrEmpty(emoticon))
        {
            _logger.LogError("Emoticon is empty");
            return;
        }

        string tags = inputString.Substring(inputString.IndexOf("[emo]", 5) + 5);
        
        if (String.IsNullOrEmpty(tags))
        {
            _logger.LogError("Tags are empty");
            return;
        }

        _logger.LogInformation("Adding emoticon: {emoticon} with tags: {tags}", emoticon, tags);

        await _dataAccess.AddEmoticon(emoticon, tags.Split(" ").Where(t => !String.IsNullOrEmpty(t)).ToList());
        
        _logger.LogInformation("Emoticon added successfully");
    

    }
}