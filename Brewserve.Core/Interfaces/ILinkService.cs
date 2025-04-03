using System.Net.Mail;

namespace BrewServe.Core.Interfaces;

public interface ILinkService
{
    List<LinkedResource> GenerateLinks<T>(T resource);
}