using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class TagModel
{
    [Key]
    public string Tag { get; set; } = "";
    public List<EmoticonModel> Emoticons { get; set; } = new List<EmoticonModel>();
}
