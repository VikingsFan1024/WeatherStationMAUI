namespace TempestMonitor.ViewModels;

sealed partial class ExportViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();

    private readonly DatabaseService _databaseService = serviceProvider.GetRequiredService<DatabaseService>();
    public SettingsModel Settings => _settings;

    public void OnAppearing()
    {
        //long skipCount = 0;
        //long fetchCount = 1000;

        //while(true)
        //{
        //    var observationModels = _databaseService.GetObservationModels(skipCount, fetchCount);
        //    if (observationModels is null || observationModels.Length == 0) break;
        //    foreach (var observationModel in observationModels)
        //    {
        //        var localDateTime = DateTimeOffset.FromUnixTimeSeconds(observationModel.ObservationTimestamp).ToLocalTime().DateTime;
        //        Log.Information(localDateTime.ToString("yyyyMMdd HH:mm:ss"));
        //    }
        //    skipCount += fetchCount;
    }

    //var mongoClient = new MongoClient("mongodb://dan:Sylvia3927@MetWorks.biz:27017");
    //var iMongoDatabase = mongoClient.GetDatabase("WeatherStationMAUI");
    //var iMongoCollection = iMongoDatabase.GetCollection<BsonDocument>("Observation");

    //var list = iMongoCollection.Find(new BsonDocument()).ToList();

    //foreach (var item in list)
    //{
    //    var revision = item["firmware_revision"].ToString();
    //    if (revision is not null) Log.Information(revision);

    //    var obs = item["obs"][0][0];
    //    var localDateTime = DateTimeOffset.FromUnixTimeSeconds(obs.AsInt32).ToLocalTime();            
    //}
}