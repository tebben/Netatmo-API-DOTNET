# Netatmo-API-DOTNET
Netatmo API for .Net (PCL)

Start of a Netatmo Weather Station API client implementation for .NET, currently supporting:
- Login
- GetStationsData
- GetMeasure

Example use:

```csharp
readonly NetatmoApi _api = new NetatmoApi("[ClientId]", "[ClientSecret]");

public Test()
{
	_api.LoginSuccessful += ApiLoginSuccessful;
	_api.Login("[username]", "[password]", new[] {NetatmoScope.read_station });
}

private async void ApiLoginSuccessful(object sender)
{
	var data = await _api.GetStationsData();
	var measurement = await _api.GetMeasure("[DeviceId]", Scale.Max, new[]
	{
		MeasurementType.Co2, MeasurementType.Humidity, MeasurementType.Noise, MeasurementType.Pressure,
		MeasurementType.Temperature
	});
}
```