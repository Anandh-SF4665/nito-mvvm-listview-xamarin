**[View document in Syncfusion Xamarin Knowledge base](https://www.syncfusion.com/kb/12214/how-to-use-nito-mvvm-in-xamarin-forms-listview-sflistview)**

## Sample

```xaml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="50"/>
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>
    <Button Text="Load ListView" Command="{Binding LoadDataCommand}" HeightRequest="50"/>
    <sfbusyindicator:SfBusyIndicator Grid.Row="1" AnimationType="Gear" TextColor="BlueViolet" IsBusy="{Binding IsLoading}"/>
    <syncfusion:SfListView x:Name="listView" AutoFitMode="Height" Grid.Row="1" ItemSpacing="5" ItemsSource="{Binding UserInfo}" >
        <syncfusion:SfListView.ItemTemplate >
            <DataTemplate>
                <code>
                . . .
                . . .
                <code>
            </DataTemplate>
        </syncfusion:SfListView.ItemTemplate>
    </syncfusion:SfListView>
</Grid>

ViewModel.cs:

System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

public IAsyncCommand LoadDataCommand { get; set; }

client = new HttpClient();
LoadDataCommand = new AsyncCommand(ExecuteLoadData);

public async Task ExecuteLoadData()
{
    var notifyTask = NotifyTask.Create(GetDataAsync);
    IsLoading = true;
    await notifyTask.TaskCompleted;
    if (notifyTask.IsCompleted)
    {
        UserInfo = notifyTask.Result as dynamic;
    }
    IsLoading = false;
}

public async Task<dynamic> GetDataAsync()
{
    try
    {
        var response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
        if (response.IsSuccessStatusCode)
        {
            await Task.Delay(1500);
            var content = await response.Content.ReadAsStringAsync();
            var details = JsonConvert.DeserializeObject<dynamic>(content);
            return details;
        }
    }
    catch (Exception ex)
    {
        Debug.WriteLine(@"ERROR {0}", ex.Message);
    }
    return null;
}
```