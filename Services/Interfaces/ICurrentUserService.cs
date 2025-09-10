namespace Services.Interfaces;

public interface ICurrentUserService
{
    Guid GetProfileGuidFromHttpContext();
}
