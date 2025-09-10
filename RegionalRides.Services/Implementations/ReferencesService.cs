using Common.Extensions;
using Constants.Enums;
using DataContracts.Shep;
using Microsoft.EntityFrameworkCore;
using RegionalRides.DAL;
using RegionalRides.DAL.Entities.References;
using RegionalRides.Services.Interfaces;
using Services.Interfaces;

namespace RegionalRides.Services.Implementations;

public class ReferencesService : IReferencesService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RegionalRidesContext _regionalRidesContext;
    private readonly IWaygoHttpService _httpService;

    public ReferencesService(IHttpClientFactory httpClientFactory, RegionalRidesContext regionalRidesContext,
        IWaygoHttpService waygoHttpService)
    {
        _httpClientFactory = httpClientFactory;
        _regionalRidesContext = regionalRidesContext;
        _httpService = waygoHttpService;
    }

    public async Task<bool> KatoSync()
    {
        var httpClient = _httpClientFactory.CreateClient("ic");
        var resp = await _httpService.SendEmptyAsync<BnsKatoResponse[]>(httpClient, "api/national/stat/kato",
            HttpMethod.Get);
        if (!resp.Data.Any())
            return false;
        await this.SyncBnsIds(resp.Data);
        await this.SyncCreateNewKatosFromBns(resp.Data);
        await this.SyncParIdKatoFromBns(resp.Data);
        return true;
    }

    private async Task SyncBnsIds(BnsKatoResponse[] bnsKatos)
    {
        var katos = await _regionalRidesContext.RefKatos.ToListAsync();
        var changesKatos = new List<RefKato>();

        foreach (var k in katos)
        {
            var bnsKato = bnsKatos.FirstOrDefault(x => x.Code == k.Te.ToString());

            if (bnsKato == null || k.BnsExternalId == bnsKato.Id)
                continue;

            k.BnsExternalId = bnsKato.Id;
            changesKatos.Add(k);
        }

        if (changesKatos.Any())
        {
            _regionalRidesContext.RefKatos.UpdateRange(changesKatos);
            await _regionalRidesContext.SaveChangesAsync();
        }
    }

    private async Task SyncCreateNewKatosFromBns(BnsKatoResponse[] bnsKatos)
    {
        Console.WriteLine("Начинаю создавать новые KatoBns.");

        var existingBnsIds = _regionalRidesContext.RefKatos
            .Select(k => k.BnsExternalId)
            .ToHashSet();

        var newBnsKatos = bnsKatos
            .Where(bnsKato => !existingBnsIds.Contains(bnsKato.Id))
            .ToArray();

        Console.WriteLine($"Количество новых KatoBns для создания: {newBnsKatos.Length}");

        var newKatos = newBnsKatos
            .Select(bnsKato => CreateRefKatoFromBnsResponse(bnsKato))
            .ToList();

        if (newKatos.Any())
        {
            await _regionalRidesContext.RefKatos.AddRangeAsync(newKatos);
            await _regionalRidesContext.SaveChangesAsync();
        }

        Console.WriteLine("Успешно закончил создавать новые KatoBns.");
    }

    private RefKato CreateRefKatoFromBnsResponse(BnsKatoResponse bnsKato)
    {
        var values = bnsKato.Code.ExtractValuesFromCode();
        return new RefKato()
        {
            BnsExternalId = bnsKato.Id,
            BnsExternalParId = bnsKato.ParId,
            BnsLocationType = (BnsLocationType)bnsKato.LocationNumber,
            IsInactive = !bnsKato.IsActual,
            Te = int.Parse(bnsKato.Code),
            Ab = values.ab,
            Cd = values.cd,
            Ef = values.ef,
            Hij = values.hij,
            K = bnsKato.LocationNumber,
            NameRu = bnsKato.NameRu,
            NameKz = bnsKato.NameKz,
            Code = bnsKato.Code
        };
    }

    private async Task SyncParIdKatoFromBns(BnsKatoResponse[] bnsKatos)
    {
        Console.WriteLine($"Начинаю обновлять ParKatoBns");
        var katos = await _regionalRidesContext.RefKatos.Where(x => x.BnsExternalId > 0).ToArrayAsync();
        var changesKatos = new List<RefKato>();
        var i = 0;
        foreach (var kato in katos)
        {
            var bnsKato = bnsKatos.FirstOrDefault(x => x.Id == kato.BnsExternalId);
            if (bnsKato == null)
            {
                Console.WriteLine($"При обновлении родителя като не нашел {kato.Te} {kato.NameRu}");
                continue;
            }

            if (bnsKato.ParId == 0) continue; //РК
            var parent = katos.FirstOrDefault(x => x.BnsExternalId == bnsKato.ParId);
            kato.Parent = parent;
            kato.IsInactive = !bnsKato.IsActual;
            kato.NameRu = bnsKato.NameRu;
            kato.NameKz = bnsKato.NameKz;
            if (kato.Ab > 0 && kato.Cd > 0)
            {
                var region = katos.FirstOrDefault(x =>
                    x.Ab == kato.Ab && x.BnsLocationType == BnsLocationType.Region && !x.IsInactive);
                kato.Region = region;
            }

            changesKatos.Add(kato);
            Console.WriteLine($"i={i}");
            i++;
        }

        _regionalRidesContext.RefKatos.UpdateRange(changesKatos);
        Console.WriteLine($"спешно закончил обновлять ParKatoBns");
    }
}