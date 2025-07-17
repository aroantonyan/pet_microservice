using LogService.Dto;

namespace LogService.Interfaces;

public interface ILogInterface
{
    public Task LogAsync( LogDto logDto);
}