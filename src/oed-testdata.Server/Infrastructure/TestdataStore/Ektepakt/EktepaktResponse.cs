namespace oed_testdata.Server.Infrastructure.TestdataStore.Ektepakt;

#pragma warning disable CS8618

public class EktepaktResponse
{
    public List<Ektepakt> Ektepakter { get; set; }
}

public class Ektepakt
{
    public string SpouseName { get; set; }

    public DateTime? EntryDate { get; set; }
}

#pragma warning restore CS8618