namespace MVC.Core.Configuration
{
	public class SystemSettings : ISystemSettings
	{
		public bool IsProductionEnviroment => ConfigSettings.GetApplicationSetting("ProductionEnvironment", "1") == "1";

		public string DwsDomainName => ConfigSettings.GetApplicationSetting("DWSDomainName", string.Empty);
	}
}
