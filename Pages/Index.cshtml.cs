using System.Threading.Tasks;
using Kunc.RiotGames.Lol.DataDragon;
using Kunc.RiotGames.Lol.LeagueClientUpdate;
using LolApp.Lcu;
using LolApp.Lcu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LolApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ILcuClient _lcuClient;
    //private readonly ILolLeagueClientUpdate _lolLeagueClientUpdate;
    private readonly ILolDataDragon _lolDataDragon;

    public IndexModel(
        ILogger<IndexModel> logger,
        ILcuClient lcuClient,
        ILolDataDragon lolDataDragon)//,
        //ILolLeagueClientUpdate lolLeagueClientUpdate)
    {
        _logger = logger;
        _lcuClient = lcuClient;
        //_lolLeagueClientUpdate = lolLeagueClientUpdate;
        _lolDataDragon = lolDataDragon;
    }

    public async Task OnGet()
    {
        //var sum = await _lcuClient.GetCurrentSummonerAsync();
        //var versions = await _lolDataDragon.GetVersionsAsync();
        //var languages = await _lolDataDragon.GetLanguagesAsync();
        //var champions = await _lolDataDragon.GetChampionsAsync(versions.First(), "en_US");
        //var champions = await _lolDataDragon.GetChampionsBaseAsync(versions.First(), "en_US");
        var d = 0;
    }
}
