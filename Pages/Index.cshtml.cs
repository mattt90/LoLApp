using System.Threading.Tasks;
using Kunc.RiotGames.Lol.DataDragon;
using Kunc.RiotGames.Lol.LeagueClientUpdate;
using LolApp.Data;
using LolApp.Lcu;
using LolApp.Lcu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LolApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ILcuClient _lcuClient;
    //private readonly ILolLeagueClientUpdate _lolLeagueClientUpdate;
    private readonly ILolDataDragon _lolDataDragon;
    private LeagueContext _context { get; }

    [BindProperty]
    public AutoSettings AutoSettings { get; set; } = new();

    public IndexModel(
        ILogger<IndexModel> logger,
        ILcuClient lcuClient,
        ILolDataDragon lolDataDragon,
        LeagueContext context)//,
        //ILolLeagueClientUpdate lolLeagueClientUpdate)
    {
        _context = context;
        _logger = logger;
        _lcuClient = lcuClient;
        //_lolLeagueClientUpdate = lolLeagueClientUpdate;
        _lolDataDragon = lolDataDragon;
    }

    public async Task OnGetAsync()
    {
        //var sum = await _lcuClient.GetCurrentSummonerAsync();
        //var versions = await _lolDataDragon.GetVersionsAsync();
        //var languages = await _lolDataDragon.GetLanguagesAsync();
        //var champions = await _lolDataDragon.GetChampionsAsync(versions.First(), "en_US");
        //var champions = await _lolDataDragon.GetChampionsBaseAsync(versions.First(), "en_US");
        var autoSettigns = await _context.AutoSettings.FirstOrDefaultAsync();
        if (autoSettigns == null)
        {
            await _context.AutoSettings.AddAsync(new AutoSettings());
            await _context.SaveChangesAsync();
            autoSettigns = await _context.AutoSettings.FirstOrDefaultAsync() ?? new AutoSettings();
        }
        AutoSettings = autoSettigns;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var update = await _context.AutoSettings.FindAsync(AutoSettings!.Id);
        if (update == null)
        {
            return NotFound();
        }

        update.AutoAcceptQueue = AutoSettings.AutoAcceptQueue;
        update.AutoRequeue = AutoSettings.AutoRequeue;
        update.AutoRandomChampionSkin = AutoSettings.AutoRandomChampionSkin;
        update.AutoRerollAram = AutoSettings.AutoRerollAram;
        update.AutoSelectChampion = AutoSettings.AutoSelectChampion;
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}
