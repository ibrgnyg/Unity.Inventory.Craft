using System;

public class BaseEntity 
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime UpdateDate { get; set; }
}
