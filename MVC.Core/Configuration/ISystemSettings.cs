namespace MVC.Core.Configuration
{
	public interface ISystemSettings
	{
		bool IsProductionEnvironment { get; }

		string DwsDomainName { get; }
	}
}
