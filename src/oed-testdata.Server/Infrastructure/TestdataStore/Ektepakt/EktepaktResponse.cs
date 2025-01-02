namespace oed_testdata.Server.Infrastructure.TestdataStore.Ektepakt;

public class EktepaktResponse
{
    public List<Ektepakt> Ektepakter { get; set; }
}

public class Ektepakt
{
    public string SpouseName { get; set; }

    public DateTime? EntryDate { get; set; }
}