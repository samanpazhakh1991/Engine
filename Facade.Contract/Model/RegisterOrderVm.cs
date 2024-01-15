using Domain.Orders.Entities;

namespace Facade.Contract.Model;

public class RegisterOrderVm
{
    public int Price { get; set; }
    public int Amount { get; set; }
    public Side Side { get; set; }
    public DateTime ExpireTime { get; set; }
    public bool IsFillAndKill { get; set; }
}