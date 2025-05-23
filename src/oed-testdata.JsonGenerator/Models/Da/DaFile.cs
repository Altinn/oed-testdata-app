﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace oed_testdata.JsonGenerator.Models.Da;


internal static class PartRoleConverter
{
    public static PartRole Convert(string tenorRelasjonsnavn)
    {
        if (Enum.TryParse<PartRole>(tenorRelasjonsnavn, ignoreCase: true, out var role))
        {
            return role;
        }

        var partialMatch = Enum.GetNames<PartRole>()
            .FirstOrDefault(roleName => roleName.ToLower().Contains(tenorRelasjonsnavn.ToLower()));

        if (!string.IsNullOrWhiteSpace(partialMatch) &&
            Enum.TryParse(partialMatch, ignoreCase: true, out role))
        {
            Console.WriteLine($"Resolved relation from tenor: [{tenorRelasjonsnavn}] => [{role.ToString()}]");
            return role;
        }

        Console.WriteLine($"!!!! Unresolvable relation from tenor: [{ tenorRelasjonsnavn }], using [{PartRole.PART_ANNEN.ToString()}] - Manual edit of json file required");
        return PartRole.PART_ANNEN;
    }
}


internal static class MetadataFile
{
    public static async Task SerializeAndWrite(EstateMetadata metadata, string estateSsn, string? outputPath = null)
    {
        var filename = $"{estateSsn}-metadata.json";
        var filepath = !string.IsNullOrWhiteSpace(outputPath) ? Path.Combine(outputPath, filename) : filename;

        await using var filestream = File.Open(filepath, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(filestream, metadata, JsonSerializerOptions.Web);
        await filestream.FlushAsync();
    }
}

public class EstateMetadata
{
    public EstateMetadataPerson[] Persons { get; set; } = [];
}

public class EstateMetadataPerson
{
    public required string Nin { get; set; }
    public required string Name { get; set; }
}

internal static class DaFile
{
    public static DaData New(string deacesedSsn, Guid caseId)
    {
        var data = Empty(caseId);

        var daCase = data.DaCaseList.Single();
        daCase.Avdode = deacesedSsn;

        return data;
    }

    public static async Task SerializeAndWrite(DaData daData, string deceasedName, string? outputPath = null)
    {
        var filename = $"{daData.DaCaseList.Single().Avdode}-{string.Join("_", deceasedName.Split(" "))}.json";
        var filepath = !string.IsNullOrWhiteSpace(outputPath) ? Path.Combine(outputPath, filename) : filename;

        await using var filestream = File.Open(filepath, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(filestream, daData, JsonSerializerOptions.Default);
        await filestream.FlushAsync();
    }

    public static DaData Empty(Guid? id = null)
    {
        id ??= Guid.NewGuid();
        var randomCaseNumberId = Random.Shared.NextInt64(0, 999999);

        return new DaData
        {
            DaEventList = new DaEvent[][]
            {
                [
                    new DaEvent
                    {
                        Id = id.Value.ToString(),
                        Specversion = "1.0",
                        Source = "https://domstol.no",
                        Type = "DODSFALLSAK-STATUS_OPPDATERT",
                        DataContentType = "application/json",
                        Time = DateTimeOffset.Now,
                        Data = new Data
                        {
                            Id = $"https://hendelsesliste.test.domstol.no/api/objects/{id}"
                        }
                    }
                ]
            },
            DaCaseList =
            [
                new DaCase
                {
                    SakId = id.Value.ToString(),
                    Saksnummer = $"25-{randomCaseNumberId}DFA-TOSL/07",
                    Avdode = "",
                    Embete = "Oslo Tingrett",
                    Status = "MOTTATT",
                    Parter = [],
                    ReceivedDate = DateTimeOffset.Now,
                    DeadlineDate = DateTimeOffset.Now.AddDays(60),
                }
            ]
        };
    }
}