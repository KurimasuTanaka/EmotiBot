using System;
using DataAccess.Models;

namespace DataAccess.DataAccess;

public interface IDataAccess
{
    Task<List<EmoticonModel>> GetEmoticons();
    Task<List<EmoticonModel>> GetEmoticonsByTag(string tag);
    Task AddEmoticon(string emoticon, List<string> tags);
}
